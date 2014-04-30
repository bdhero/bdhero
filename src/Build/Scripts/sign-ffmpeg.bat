@ECHO OFF

REM %CD% = C:\Projects\BDHero\src

call Build\Scripts\tools.bat

set FFmpegBinDir=%1
set FFmpegVersion=%2

set Sign=Build\Scripts\sign.bat
set FFmpegUrl=http://ffmpeg.org/

call %Sign% "FFmpeg %FFmpegVersion% 32-bit" "%FFmpegUrl%" %FFmpegBinDir%\ffmpeg-%FFmpegVersion%-32.exe
call %Sign% "FFmpeg %FFmpegVersion% 64-bit" "%FFmpegUrl%" %FFmpegBinDir%\ffmpeg-%FFmpegVersion%-64.exe
