@echo off

if '%1'=='/?' goto help
if '%1'=='-help' goto help
if '%1'=='-h' goto help

powershell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0psake.ps1' %*; if ($psake.build_success -eq $false) { Write-Host \"Press any key to continue...\"; $host.UI.RawUI.ReadKey(\"NoEcho,IncludeKeyDown\") | out-null; exit 1 } else { exit 0 }"
goto :eof

:help
powershell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0psake.ps1' -help"