# Скрипт для обработки изображения карты и создания тайлов
# Использует существующий функционал MapTileService

param(
    [Parameter(Mandatory=$true)]
    [string]$ImagePath,
    
    [Parameter(Mandatory=$false)]
    [string]$OutputDir = "tiles/botanical_garden_main",
    
    [Parameter(Mandatory=$false)]
    [string]$MapName = "Основная карта ботанического сада",
    
    [Parameter(Mandatory=$false)]
    [string]$Description = "Карта территории ботанического сада",
    
    [Parameter(Mandatory=$false)]
    [int]$MinZoom = 10,
    
    [Parameter(Mandatory=$false)]
    [int]$MaxZoom = 18
)

# Проверяем существование исходного файла
if (-not (Test-Path $ImagePath)) {
    Write-Error "Исходное изображение не найдено по пути: $ImagePath"
    exit 1
}

# Создаем директорию для тайлов, если она не существует
$fullOutputPath = Join-Path (Get-Location) $OutputDir
if (-not (Test-Path $fullOutputPath)) {
    New-Item -Path $fullOutputPath -ItemType Directory -Force | Out-Null
    Write-Host "Создана директория для тайлов: $fullOutputPath"
}

# Запускаем обработку карты
try {
    # Здесь вы можете вызвать ваше консольное приложение или API
    # Пример для консольного приложения:
    dotnet run --project Infrastructure/Infrastructure.csproj --map-image $ImagePath --output-dir $OutputDir --name $MapName --description $Description --min-zoom $MinZoom --max-zoom $MaxZoom
    
    Write-Host "Обработка карты успешно завершена. Тайлы созданы в директории: $fullOutputPath"
    Write-Host "После этого вы можете создать слой карты с помощью IMapTileService.CreateMapLayerFromTilesAsync"
} catch {
    Write-Error "Ошибка при обработке карты: $_"
    exit 1
} 