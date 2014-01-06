@ECHO OFF

REM %CD% = C:\Projects\BDHero\src

call Build\Scripts\tools.bat

echo PATH=%PATH%

call Build\Scripts\sign.bat "BDHero GUI" "%ProjectUrl%" %ArtifactsPath%\Installer\ProgramFiles\*.exe

xcopy /Y %ArtifactsPath%\Installer\ProgramFiles\*.exe %ArtifactsPath%\Portable\

REM "%ProgramFiles%\BitRock InstallBuilder Enterprise 8.6.0\bin\builder-cli.exe" build "Installer.xml" windows

REM Use `sed` to redact the path to and password for the private key file used to digitally sign EXEs
call %InnoSetup%\iscc "/sCustom=%SignTool%\signtool.exe $p" Build\InnoSetup\setup.iss | %unxutils%\sed "s/\/[df] .*/[REDACTED]/gi"

REM %SevenZip%\7za a -sfx7z.sfx -r "%SfxPath%" %ArtifactsPath%\Portable\*
REM %SevenZip%\7za a -r "%SevenZipPath%" %ArtifactsPath%\Portable\*
%SevenZip%\7za a -r "%ZipPath%" %ArtifactsPath%\Portable\*

REM call Build\Scripts\sign.bat "BDHero Portable (Self Extracting Archive)" "%ProjectUrl%" "%SfxPath%"
