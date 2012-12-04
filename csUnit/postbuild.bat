goto :EOF

rem ***************************************************************************
rem remove old versions from GAC
"%ProgramFiles%\Microsoft Visual Studio .NET 2003\SDK\v1.1\Bin\gacutil.exe" /u %1


rem ***************************************************************************
rem add csUnit.dll to GAC

rem We have to use an absolute path. Otherwise registration of the assembly will
rem fail. [ml]

"%ProgramFiles%\Microsoft Visual Studio .NET 2003\SDK\v1.1\Bin\gacutil.exe" /i %2
