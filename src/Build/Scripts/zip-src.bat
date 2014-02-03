@ECHO OFF

REM %CD% = C:\Projects\BDHero\src

call Build\Scripts\tools.bat

set CloneDir=%TEMP%\bdhero-src
set ZipFileName=bdhero-src.zip
set ZipFileDir1=..\Artifacts\Portable
set ZipFilePath1=%ZipFileDir1%\%ZipFileName%
set ZipFileDir2=..\Artifacts\Installer\ProgramFiles
set ZipFilePath2=%ZipFileDir2%\%ZipFileName%

echo PATH=%PATH%

REM Delete the cloned Git repo
del /Q /S "%CloneDir%\*"
rmdir "%CloneDir%" /q /s

REM Clone the GIt repo so we have a nice clean source directory without any binary output or NuGet packages
git clone ../ "%CloneDir%"

REM Delete old zip files
del /Q /S "%ZipFilePath1%" "%ZipFilePath2%"

REM Zip up the source files
%SevenZip%\7za a -r "%ZipFilePath1%" "%CloneDir%\*" -xr!.git

REM Copy the zip file
xcopy /R /Y "%ZipFilePath1%" "%ZipFileDir2%"

REM Delete the cloned Git repo
del /Q /S "%CloneDir%\*"
rmdir "%CloneDir%" /q /s
