@echo off
REM Командный файл для обработки изображения карты
powershell -ExecutionPolicy Bypass -File "%~dp0\process-map-image.ps1" -ImagePath "%~dp0\botGardenMapCut.png"

REM Ожидание нажатия клавиши для предотвращения закрытия окна
pause 