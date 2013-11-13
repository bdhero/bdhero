.nuget/NuGet restore -NonInteractive
Update-Package -Id FFmpeg.Binaries -Reinstall -FileConflictAction Overwrite
Update-Package -Id mkvpropedit.Binaries -Reinstall -FileConflictAction Overwrite
Update-Package -Id BDHero.BuildTools -Reinstall -FileConflictAction Overwrite