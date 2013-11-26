@ECHO OFF

REM %CD% = C:\Projects\BDHero\src

call Build\Scripts\tools.bat

REM Path to the code signing certificate private key (.pfx) file
IF "%CodeSigningCertPK%"=="" echo Environment variable %%CodeSigningCertPK%% is missing && GOTO END

REM Password for the PK
IF "%CodeSigningCertPW%"=="" echo Environment variable %%CodeSigningCertPK%% is missing && GOTO END

set ExeDescription=%1
set WebsiteUrl=%2
set PathToExe=%3

IF %ExeDescription%=="" echo Argument %%1 (ExeDescription) is missing && GOTO USAGE
IF %WebsiteUrl%=="" echo Argument %%2 (WebsiteUrl) is missing && GOTO USAGE
IF %PathToExe%=="" echo Argument %%3 (PathToExe) is missing && GOTO USAGE

%SignTool%\signtool sign /v /p %CodeSigningCertPW% /f %CodeSigningCertPK% /d %ExeDescription% /du %WebsiteUrl% /t http://timestamp.comodoca.com/authenticode %PathToExe%

GOTO END

:USAGE
echo.
echo Usage:   sign.bat EXE_DESCRIPTION WEBSITE_URL PATH_TO_EXE
echo Example: sign.bat "My Program" "http://link.to/website" "path/to.exe"
goto END

:END
