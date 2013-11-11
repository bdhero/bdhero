@ECHO OFF

set ConfigurationName=%1
set SolutionDir=%2

IF "%ConfigurationName%"=="" set ConfigurationName=Debug
IF NOT "%SolutionDir%"=="" cd "%SolutionDir%"

set PluginDir=Artifacts\Installer\Plugins\Required
set ConfigDir=Artifacts\Installer\Config
set ProgramFilesDir=Artifacts\Installer\ProgramFiles
set PortableDir=Artifacts\Portable
set PackagerBin=Build\Packager\bin\%ConfigurationName%

echo %ConfigurationName%

REM Clean up old merged files
rmdir /S /Q Artifacts

REM Delete empty dummy DLL
del /F %PackagerBin%\Packager.dll

REM Copy core BDHero EXEs, DLLs, and config files to Artifacts dir
xcopy /Y %PackagerBin%\*.exe %ProgramFilesDir%\
xcopy /Y %PackagerBin%\*.dll %ProgramFilesDir%\
xcopy /Y /S %PackagerBin%\Config %ConfigDir%\

REM Copy required plugins

xcopy /Y Plugins\DiscReaderPlugin\bin\%ConfigurationName%\DiscReaderPlugin.dll %PluginDir%\DiscReader\
xcopy /Y Plugins\DiscReaderPlugin\bin\%ConfigurationName%\INIFileParser.dll %PluginDir%\DiscReader\

xcopy /Y Plugins\TmdbPlugin\bin\%ConfigurationName%\TmdbPlugin.dll %PluginDir%\Tmdb\
xcopy /Y Plugins\TmdbPlugin\bin\%ConfigurationName%\RestSharp.dll %PluginDir%\Tmdb\
xcopy /Y Plugins\TmdbPlugin\bin\%ConfigurationName%\WatTmdb.dll %PluginDir%\Tmdb\

xcopy /Y Plugins\ChapterGrabberPlugin\bin\%ConfigurationName%\ChapterGrabberPlugin.dll %PluginDir%\ChapterGrabber\

xcopy /Y Plugins\IsanPlugin\bin\%ConfigurationName%\IsanPlugin.dll %PluginDir%\Isan\
xcopy /Y Plugins\IsanPlugin\bin\%ConfigurationName%\CsQuery.dll %PluginDir%\Isan\

xcopy /Y Plugins\AutoDetectorPlugin\bin\%ConfigurationName%\AutoDetectorPlugin.dll %PluginDir%\AutoDetector\

xcopy /Y Plugins\FileNamerPlugin\bin\%ConfigurationName%\FileNamerPlugin.dll %PluginDir%\FileNamer\

xcopy /Y Plugins\FFmpegMuxerPlugin\bin\%ConfigurationName%\FFmpegMuxerPlugin.dll %PluginDir%\FFmpegMuxer\
xcopy /Y Plugins\FFmpegMuxerPlugin\bin\%ConfigurationName%\*.exe %PluginDir%\FFmpegMuxer\

REM Copy everything to Portable directory

xcopy /Y /S %PluginDir% %PortableDir%\Plugins\Required\
xcopy /Y /S %ConfigDir% %PortableDir%\Config\
xcopy /Y %ProgramFilesDir%\* %PortableDir%\
