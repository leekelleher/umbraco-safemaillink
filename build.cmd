%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe package\package.proj
@IF %ERRORLEVEL% NEQ 0 GOTO err
@exit /B 0
:err
@PAUSE
@exit /B 1