@echo off

@IF NOT EXIST %windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe @ECHO COULDN'T FIND MSBUILD: %windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe (Is .NET 4 installed?)

%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe Project/%2/%3/build.xml  /maxcpucount /l:FileLogger,Microsoft.Build.Engine;logfile=Project/%2/%3/log.log /t:build

set fd=%3_Release

cd %1
cd %2
cd %3

cd web
rename web.config web.config.sample
cd ..

"C:\Program Files\WinRAR\rar.exe"  a -x*.rar -x*.log -x*.sln -x*.suo -x*.vb -x*.scc -x*.vbproj -x*.user -x*.vspscc -x*.bak -x*.resx -x*.pdb -x*.zip -x*\obj -x*\lib -x*\ExcelFolder\* -x*\temp\* -k -r -s -ibck -ag+YYYYMMDDHHMM %fd%.rar Web

cd web
rename web.config.sample web.config
cd ..