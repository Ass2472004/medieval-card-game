@echo off
REM ══════════════════════════════════════════════════════
REM  Nahkor — Build local para Windows (portátil)
REM  Doble-clic para compilar y ejecutar el juego
REM ══════════════════════════════════════════════════════

set UNITY="C:\Program Files\Unity\Hub\Editor\6000.4.0f1\Editor\Unity.exe"
set PROJECT=%~dp0
set BUILD=%~dp0build\Windows
set LOG=%~dp0build\build_log.txt

echo.
echo  Compilando Nahkor para Windows...
echo  Log: %LOG%
echo.

if not exist "%BUILD%" mkdir "%BUILD%"

%UNITY% ^
  -batchmode ^
  -nographics ^
  -quit ^
  -projectPath "%PROJECT%" ^
  -executeMethod BuildScript.BuildWindows ^
  -logFile "%LOG%"

if %ERRORLEVEL% == 0 (
    echo.
    echo  BUILD EXITOSO
    echo  Ejecutando el juego...
    echo.
    start "" "%BUILD%\Nahkor.exe"
) else (
    echo.
    echo  ERROR en la compilacion. Revisa: %LOG%
    echo.
    pause
)
