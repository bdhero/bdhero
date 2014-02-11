@ECHO OFF

REM %CD% = C:\Projects\BDHero\src

call Build\Scripts\tools.bat

cd Libraries\LicenseUtils\Licenses

for %%f in (*.md) do (
    echo %%~nf
    ..\..\..\Build\Tools\Markdowner.exe "%%~nf.md" "%%~nf.html"
)
