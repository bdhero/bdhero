@ECHO OFF

REM %CD% = C:\Projects\BDHero\src

REM Capture stdout from Versioner.exe and store it in a variable
REM See http://stackoverflow.com/a/108511/467582
FOR /F "delims=" %%i IN ('Build\Tools\Versioner --version') DO set NewVersion=%%i

git status

git add -f update.json
git add BDHero\Properties\AssemblyInfo.cs
git add BDHeroCLI\Properties\AssemblyInfo.cs
git add BDHeroGUI\Properties\AssemblyInfo.cs
git add Build\InnoSetup\setup.iss

git status

set Message=Bumped BDHero version to %NewVersion%

echo.
echo Committing
echo.

git commit -m "%Message%"

echo.
echo Tagging
echo.

git tag -a v%NewVersion% -m v%NewVersion%

echo.
echo Pushing
echo.

git push -u origin master --tags

git status
