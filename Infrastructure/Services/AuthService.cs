using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;
using BCrypt.Net;
using Google.Authenticator;
using Microsoft.Extensions.Configuration;

namespace BGarden.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly BotanicalContext _context;
        private readonly int _refreshTokenLifetimeDays;
        
        public AuthService(BotanicalContext context, IConfiguration configuration)
        {
            _context = context;
            
            // Получение настроек из конфигурации (только для refresh токенов)
            var jwtSettings = configuration.GetSection("JwtSettings");
            _refreshTokenLifetimeDays = int.TryParse(jwtSettings["RefreshTokenExpiryDays"], out int refreshTokenDays) ? refreshTokenDays : 7;
        }
        
        public async Task<User?> AuthenticateAsync(string username, string password, string? ipAddress = null, string? userAgent = null)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Username == username);

            // Проверка существования пользователя
            if (user == null)
            {
                await LogAuthEventAsync(AuthEventType.FailedLogin, username, null, false, "Пользователь не найден", ipAddress, userAgent);
                return null;
            }

            // Проверка активности аккаунта
            if (!user.IsActive)
            {
                await LogAuthEventAsync(AuthEventType.FailedLogin, username, user.Id, false, "Аккаунт деактивирован", ipAddress, userAgent);
                return null;
            }
            
            // Проверка блокировки аккаунта
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                await LogAuthEventAsync(AuthEventType.FailedLogin, username, user.Id, false, "Аккаунт заблокирован", ipAddress, userAgent);
                return null;
            }

            // Проверяем пароль
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            
            if (!isPasswordValid)
            {
                // Увеличиваем счетчик неудачных попыток
                user.FailedLoginAttempts++;
                
                // Если достигнут порог, блокируем аккаунт
                if (user.FailedLoginAttempts >= 5)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                    await LogAuthEventAsync(AuthEventType.AccountLocked, username, user.Id, false, 
                        $"Аккаунт заблокирован на 15 минут после {user.FailedLoginAttempts} неудачных попыток", ipAddress, userAgent);
                }
                else 
                {
                    await LogAuthEventAsync(AuthEventType.FailedLogin, username, user.Id, false, 
                        $"Неверный пароль. Попытка {user.FailedLoginAttempts}/5", ipAddress, userAgent);
                }
                
                await _context.SaveChangesAsync();
                return null;
            }

            // При успешном входе сбрасываем счетчик попыток
            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            
            // Обновляем дату последнего входа
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            await LogAuthEventAsync(AuthEventType.Login, username, user.Id, true, "Успешный вход", ipAddress, userAgent);

            return user;
        }
        
        public async Task<User> CreateUserAsync(User user, string password, string? ipAddress = null)
        {
            // Проверка на уникальность имени пользователя и email
            if (await _context.Users.AnyAsync(x => x.Username == user.Username))
                throw new InvalidOperationException("Пользователь с таким именем уже существует");
                
            if (await _context.Users.AnyAsync(x => x.Email == user.Email))
                throw new InvalidOperationException("Пользователь с таким email уже существует");
            
            // Создаем хеш пароля с помощью BCrypt
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordSalt = string.Empty; // BCrypt хранит соль в самом хеше
            
            // Задаем начальные значения
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;
            user.FailedLoginAttempts = 0;
            user.TwoFactorEnabled = false;
            
            // Добавляем пользователя
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
            await LogAuthEventAsync(AuthEventType.UserCreated, user.Username, user.Id, true, 
                $"Создан новый пользователь с ролью {user.Role}", ipAddress);
            
            return user;
        }
        
        public async Task<User> UpdateUserAsync(User userParam, string? ipAddress = null)
        {
            var user = await _context.Users.FindAsync(userParam.Id);
            
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");
                
            // Обновление информации о пользователе
            if (!string.IsNullOrWhiteSpace(userParam.Email) && userParam.Email != user.Email)
            {
                // Проверка на уникальность email
                if (await _context.Users.AnyAsync(x => x.Email == userParam.Email))
                    throw new InvalidOperationException("Email уже используется");
                    
                user.Email = userParam.Email;
            }
            
            if (!string.IsNullOrWhiteSpace(userParam.FullName))
                user.FullName = userParam.FullName;
                
            if (!string.IsNullOrWhiteSpace(userParam.Position))
                user.Position = userParam.Position;
                
            if (userParam.IsActive != user.IsActive)
            {
                user.IsActive = userParam.IsActive;
                
                // Если активируем, то сбрасываем счетчик и блокировку
                if (user.IsActive)
                {
                    user.FailedLoginAttempts = 0;
                    user.LockoutEnd = null;
                }
            }
            
            // Обновляем роль если она изменилась
            if (userParam.Role != user.Role)
            {
                user.Role = userParam.Role;
            }
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            
            await LogAuthEventAsync(AuthEventType.UserUpdated, user.Username, user.Id, true, 
                "Обновлена информация о пользователе", ipAddress);
            
            return user;
        }
        
        public async Task ChangePasswordAsync(int userId, string currentPassword, string newPassword, string? ipAddress = null)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");
                
            // Проверка текущего пароля
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash);
            
            if (!isPasswordValid)
                throw new InvalidOperationException("Неверный текущий пароль");
                
            // Создаем новый хеш пароля с помощью BCrypt
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordSalt = string.Empty; // BCrypt хранит соль в самом хеше
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            
            // Отзываем все refresh токены
            var activeRefreshTokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked)
                .ToListAsync();
                
            foreach (var token in activeRefreshTokens)
            {
                token.IsRevoked = true;
                token.RevokedByIp = ipAddress;
            }
            
            if (activeRefreshTokens.Any())
            {
                await _context.SaveChangesAsync();
            }
            
            await LogAuthEventAsync(AuthEventType.PasswordChange, user.Username, user.Id, true, 
                "Пароль успешно изменен", ipAddress);
        }
        
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        
        public bool HasPermission(User user, string operation)
        {
            // Проверка прав доступа в зависимости от операции и роли пользователя
            
            // Администраторы имеют полный доступ
            if (user.Role == UserRole.Administrator)
                return true;
                
            // Сотрудники имеют доступ к управлению растениями
            if (user.Role == UserRole.Employee)
            {
                switch (operation)
                {
                    case "ViewPlants":
                    case "EditPlants":
                    case "AddPlants":
                    case "DeletePlants":
                        return true;
                    default:
                        return false;
                }
            }
            
            // Клиенты имеют доступ только к просмотру
            if (user.Role == UserRole.Client)
            {
                switch (operation)
                {
                    case "ViewPlants":
                        return true;
                    default:
                        return false;
                }
            }
            
            return false;
        }
        
        public async Task<RefreshToken> GenerateRefreshTokenAsync(User user, string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = GenerateUniqueToken(),
                ExpiryDate = DateTime.UtcNow.AddDays(_refreshTokenLifetimeDays),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                IsRevoked = false
            };
            
            // Сохраняем токен в базе данных
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            
            return refreshToken;
        }
        
        public async Task<(User User, RefreshToken RefreshToken)> RefreshTokenAsync(string tokenValue, string ipAddress)
        {
            var refreshToken = await _context.RefreshTokens
                .Include(r => r.User)
                .SingleOrDefaultAsync(r => r.Token == tokenValue);

            // Проверка наличия токена
            if (refreshToken == null)
            {
                await LogAuthEventAsync(AuthEventType.InvalidRefreshToken, "Unknown", null, false, 
                    "Попытка использовать несуществующий refresh token", ipAddress);
                throw new InvalidOperationException("Невалидный refresh token");
            }

            // Проверка не отозван ли токен
            if (refreshToken.Revoked != null)
            {
                // Отзываем все refresh токены для этого пользователя (семья токенов)
                await RevokeDescendantRefreshTokensAsync(refreshToken, ipAddress, 
                    $"Попытка использовать уже отозванный refresh token: {tokenValue}");
                
                await _context.SaveChangesAsync();
                await LogAuthEventAsync(AuthEventType.RevokedRefreshToken, refreshToken.User.Username, refreshToken.User.Id, false, 
                    "Попытка использовать уже отозванный refresh token", ipAddress);
                
                throw new InvalidOperationException("Невалидный refresh token");
            }

            // Проверка срока действия
            if (refreshToken.ExpiryDate < DateTime.UtcNow)
            {
                refreshToken.Revoked = DateTime.UtcNow;
                refreshToken.RevokedByIp = ipAddress;
                refreshToken.ReasonRevoked = "Истек срок действия токена";
                await _context.SaveChangesAsync();
                
                await LogAuthEventAsync(AuthEventType.ExpiredRefreshToken, refreshToken.User.Username, refreshToken.User.Id, false, 
                    "Refresh token истек", ipAddress);
                
                throw new InvalidOperationException("Refresh token истек");
            }

            // Проверка пользователя
            var user = refreshToken.User;
            if (user == null || !user.IsActive)
            {
                await LogAuthEventAsync(AuthEventType.InvalidRefreshToken, user?.Username ?? "Unknown", user?.Id, false, 
                    "Пользователь не активен или не найден", ipAddress);
                
                throw new InvalidOperationException("Невалидный refresh token");
            }

            // Создаем новый refresh token и заменяем старый
            var newRefreshToken = await RotateRefreshTokenAsync(refreshToken, ipAddress);
            
            user.LastActiveAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            await LogAuthEventAsync(AuthEventType.RefreshTokenSuccess, user.Username, user.Id, true, 
                "Успешное обновление токена", ipAddress);
            
            // Возвращаем пользователя и новый токен
            return (user, newRefreshToken);
        }
        
        public async Task RevokeTokenAsync(string token, string ipAddress)
        {
            var refreshToken = await _context.RefreshTokens
                .Include(r => r.User)
                .SingleOrDefaultAsync(r => r.Token == token);
                
            if (refreshToken == null)
                throw new InvalidOperationException("Токен не найден");
                
            if (refreshToken.IsRevoked)
                throw new InvalidOperationException("Токен уже отозван");
                
            // Отзываем токен
            refreshToken.IsRevoked = true;
            refreshToken.RevokedByIp = ipAddress;
            
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
            
            if (refreshToken.User != null)
            {
                await LogAuthEventAsync(AuthEventType.Logout, refreshToken.User.Username, refreshToken.User.Id, true, 
                    "Токен отозван", ipAddress);
            }
        }
        
        public async Task UnlockUserAccountAsync(int userId, string? ipAddress = null)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");
                
            // Разблокируем аккаунт
            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            
            await LogAuthEventAsync(AuthEventType.AccountUnlocked, user.Username, user.Id, true, 
                "Аккаунт разблокирован", ipAddress);
        }
        
        public async Task<IEnumerable<AuthLog>> GetUserAuthLogsAsync(int userId, int limit = 20)
        {
            return await _context.AuthLogs
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.Timestamp)
                .Take(limit)
                .ToListAsync();
        }
        
        public async Task<string> SetupTwoFactorAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");
                
            // Создаем секретный ключ
            var tfa = new TwoFactorAuthenticator();
            var secretKey = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            
            // Сохраняем ключ в базе
            user.TwoFactorKey = secretKey;
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            
            // Генерируем URL для QR-кода
            var setupInfo = tfa.GenerateSetupCode("BGarden", user.Email, secretKey, false, 3);
            
            return setupInfo.ManualEntryKey;
        }
        
        public async Task<bool> VerifyTwoFactorCodeAsync(int userId, string code)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null || string.IsNullOrEmpty(user.TwoFactorKey))
                return false;
                
            var tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN(user.TwoFactorKey, code);
        }
        
        public async Task DisableTwoFactorAsync(int userId, string? ipAddress = null)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");
                
            user.TwoFactorEnabled = false;
            user.TwoFactorKey = null;
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            
            await LogAuthEventAsync(AuthEventType.UserUpdated, user.Username, user.Id, true, 
                "Двухфакторная аутентификация отключена", ipAddress);
        }
        
        // Вспомогательные методы
        
        // Генерация случайного токена
        private string GenerateUniqueToken()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[40];
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
        
        // Логирование событий авторизации
        private async Task LogAuthEventAsync(AuthEventType eventType, string username, int? userId, bool success, string? details = null, string? ipAddress = null, string? userAgent = null)
        {
            var authLog = new AuthLog
            {
                EventType = eventType,
                Username = username,
                UserId = userId,
                Success = success,
                Timestamp = DateTime.UtcNow,
                Details = details,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };
            
            await _context.AuthLogs.AddAsync(authLog);
            await _context.SaveChangesAsync();
        }
        
        // Приватный метод для создания нового refresh токена и отзыва старого
        private async Task<RefreshToken> RotateRefreshTokenAsync(RefreshToken oldToken, string ipAddress)
        {
            // Создаем новый refresh token
            var newRefreshToken = new RefreshToken
            {
                UserId = oldToken.UserId,
                User = oldToken.User,
                Token = GenerateUniqueToken(),
                ExpiryDate = DateTime.UtcNow.AddDays(_refreshTokenLifetimeDays),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
            
            // Отзываем старый токен
            oldToken.Revoked = DateTime.UtcNow;
            oldToken.RevokedByIp = ipAddress;
            oldToken.ReplacedByToken = newRefreshToken.Token;
            oldToken.ReasonRevoked = "Заменен на новый токен";
            
            // Добавляем новый токен в базу
            _context.RefreshTokens.Add(newRefreshToken);
            _context.RefreshTokens.Update(oldToken);
            
            return newRefreshToken;
        }
        
        // Отзыв всех дочерних refresh токенов
        private async Task RevokeDescendantRefreshTokensAsync(RefreshToken refreshToken, string ipAddress, string reason)
        {
            // Рекурсивно отзываем все токены, которые заменили данный токен
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = await _context.RefreshTokens.SingleOrDefaultAsync(r => 
                    r.Token == refreshToken.ReplacedByToken);
                    
                if (childToken != null && childToken.Revoked == null)
                {
                    childToken.Revoked = DateTime.UtcNow;
                    childToken.RevokedByIp = ipAddress;
                    childToken.ReasonRevoked = reason;
                    await _context.SaveChangesAsync();
                    
                    // Рекурсивно отзываем всех потомков
                    await RevokeDescendantRefreshTokensAsync(childToken, ipAddress, reason);
                }
            }
        }
    }
} 