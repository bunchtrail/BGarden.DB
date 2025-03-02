using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;

namespace BGarden.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly BotanicalContext _context;
        
        public AuthService(BotanicalContext context)
        {
            _context = context;
        }
        
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Username == username);

            // Проверка существования пользователя и активности аккаунта
            if (user == null || !user.IsActive)
                return null;

            // Проверяем пароль
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // Обновляем дату последнего входа
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return user;
        }
        
        public async Task<User> CreateUserAsync(User user, string password)
        {
            // Проверка на уникальность имени пользователя и email
            if (await _context.Users.AnyAsync(x => x.Username == user.Username))
                throw new InvalidOperationException("Пользователь с таким именем уже существует");
                
            if (await _context.Users.AnyAsync(x => x.Email == user.Email))
                throw new InvalidOperationException("Пользователь с таким email уже существует");
            
            // Создаем хеш пароля
            CreatePasswordHash(password, out string passwordHash, out string passwordSalt);
            
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.CreatedAt = DateTime.UtcNow;
            
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
            return user;
        }
        
        public async Task<User> UpdateUserAsync(User userParam)
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
                
            user.IsActive = userParam.IsActive;
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            
            return user;
        }
        
        public async Task ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");
                
            // Проверка текущего пароля
            if (!VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
                throw new InvalidOperationException("Неверный текущий пароль");
                
            // Создаем новый хеш пароля
            CreatePasswordHash(newPassword, out string passwordHash, out string passwordSalt);
            
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
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
                    case "ViewMap":
                    case "EditMap":
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
                    case "ViewMap":
                        return true;
                    default:
                        return false;
                }
            }
            
            return false;
        }
        
        // Вспомогательные методы для работы с паролями
        private static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
                );
            }
        }
        
        private static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            
            using (var hmac = new HMACSHA512(saltBytes))
            {
                var computedHash = Convert.ToBase64String(
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
                );
                
                return computedHash == storedHash;
            }
        }
    }
} 