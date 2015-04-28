@echo off
setlocal enableextensions

rem Local paths -- configure appropriately for your system
set ANT=C:\src\apache-ant-1.9.4\bin\ant
set OUTDIR=..\AudioBypassTest\Assets\Plugins\Android\

set OUTJAR=AudioBypassPlugin.jar

call %ANT% jar

echo COPYING %OUTJAR% to plugin directory
COPY bin\%OUTJAR% %OUTDIR%
echo Done
