@ECHO OFF

REM %CD% = C:\Projects\BDHero

call Build\Scripts\tools.bat

echo PATH=%PATH%

call Build\Scripts\sign.bat "BDHero GUI" "%ProjectUrl%" Artifacts\Installer\ProgramFiles\*.exe

xcopy /Y Artifacts\Installer\ProgramFiles\*.exe Artifacts\Portable\

REM "%ProgramFiles%\BitRock InstallBuilder Enterprise 8.6.0\bin\builder-cli.exe" build "Installer.xml" windows

REM Use `sed` to redact the path to and password for the private key file used to digitally sign EXEs
call %InnoSetup%\iscc "/sCustom=%SignTool%\signtool.exe $p" Build\InnoSetup\setup.iss | %unxutils%\sed "s/\/[df] .*/[REDACTED]/gi"

REM %SevenZip%\7za a -sfx7z.sfx -r "%SfxPath%" .\Artifacts\Portable\*
REM %SevenZip%\7za a -r "%SevenZipPath%" .\Artifacts\Portable\*
%SevenZip%\7za a -r "%ZipPath%" .\Artifacts\Portable\*

REM call Build\Scripts\sign.bat "BDHero Portable (Self Extracting Archive)" "%ProjectUrl%" "%SfxPath%"
