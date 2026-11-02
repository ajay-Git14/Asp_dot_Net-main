@echo off
setlocal

echo Restoring the project...
dotnet restore src\ActionFlowDemo\ActionFlowDemo.csproj
if errorlevel 1 goto error

echo Building the project...
dotnet build src\ActionFlowDemo\ActionFlowDemo.csproj
if errorlevel 1 goto error

echo Starting the application...
dotnet run --project src\ActionFlowDemo\ActionFlowDemo.csproj
goto end

:error
echo.
echo The command failed. Check that the .NET 10 SDK is installed.
pause

:end
endlocal
