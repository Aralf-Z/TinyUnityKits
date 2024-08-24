set WORKSPACE=..
set LUBAN_DLL=.\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -c cs-simple-json^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputDataDir=%WORKSPACE%\Assets\Config\Luban\Data^
    -x outputCodeDir=%WORKSPACE%\Assets\Config\Luban\Code
pause