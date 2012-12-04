goto :EOF

rem ***************************************************************************
rem remove old versions from GAC
gacutil.exe /i %2
if errorlevel 9009 goto ungacpath
goto installgac

:ungacpath
"%ProgramFiles%\Microsoft Visual Studio .NET 2003\SDK\v1.1\Bin\gacutil.exe" /u %1

rem ***************************************************************************
rem add csUnit.dll to GAC and create the type library 

rem We have to use an absolute path. Otherwise registration of the assembly will 
rem fail. [ml]

:installgac
gacutil.exe /i %2
if errorlevel 9009 goto usegacpath
goto :tlb

:usegacpath
"%ProgramFiles%\Microsoft Visual Studio .NET 2003\SDK\v1.1\Bin\gacutil.exe" /i %2

:tlb
tlbexp.exe %2
if errorlevel 9009 goto usetlbpath

:usetlbpath
"%ProgramFiles%\Microsoft Visual Studio .NET 2003\SDK\v1.1\Bin\tlbexp.exe" %2

