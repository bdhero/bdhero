# IMPORTANT: This script MUST be run within Visual Studio's NuGet "Package Manager Console".
# THIS SCRIPT WILL NOT WORK ANYWHERE ELSE.

# Restore missing packages
.nuget/NuGet restore -NonInteractive

# Forcibly install missing binaries.
# NuGet skips files that are listed in the .csproj, even if they don't exist on disk.
Update-Package -Id FFmpeg.Binaries -Reinstall -FileConflictAction Overwrite
Update-Package -Id mkvpropedit.Binaries -Reinstall -FileConflictAction Overwrite
Update-Package -Id BDHero.BuildTools -Reinstall -FileConflictAction Overwrite

# Other useful NuGet commants:

# Reinstall a specific package in a specific project:
# Update-Package -Id <PACKAGE_ID> -Reinstall -FileConflictAction Overwrite -ProjectName <PROJECT_NAME>

# Reinstall EVERYTHING:
# Update-Package -Reinstall -FileConflictAction Overwrite
