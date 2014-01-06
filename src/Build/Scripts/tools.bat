@ECHO OFF

REM %CD% = C:\Projects\BDHero\src

set ProjectUrl=http://bdhero.org/
set MirrorUrl=http://dl.cdn.bdhero.org/

set Tools=Build\Tools
set SevenZip=%Tools%\7-Zip
set InnoSetup=%Tools%\InnoSetup
set SignTool=%Tools%\SignTool
set ILMerge=%Tools%\ILMerge
set unxutils=%Tools%\unxutils

Build\Tools\Versioner -v > VERSION.tmp
set /p Version= < VERSION.tmp
del VERSION.tmp

set ArtifactsPath=..\Artifacts

set SetupPath=%ArtifactsPath%\bdhero-%Version%-setup.exe
set SfxPath=%ArtifactsPath%\bdhero-%Version%-sfx.exe
set SevenZipPath=%ArtifactsPath%\bdhero-%Version%.7z
set ZipPath=%ArtifactsPath%\bdhero-%Version%.zip
