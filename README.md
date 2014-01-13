# BDHero

An intelligent, automatic Blu-ray™ to MKV muxer and analysis tool.

![BDHero logo](http://i.bdhero.org/logo/v5/bdhero_gui_128x100_trim.png)

# Development

BDHero is written in C# / .NET 4.0.

## Required Software

*  Windows XP, Vista, 7, 8, or 8.1 (32- or 64-bit)
*  Microsoft .NET Framework 4.0
*  Visual Studio 2010 or newer

## Recommended Tools

*  ReSharper 7 or newer
*  NuGet extension for Visual Studio

## Project Setup

When you first open the solution in Visual Studio, you will see gobs of errors about missing assembly references.
This is because we use [nuget-package-restore][NuGet Package Restore] instead of keeping our packages in source control.

Simply build the solution to automatically restore all missing NuGet packages and fix the reference errors.

## Development Roadmap

*  [Mono][mono] compatibility
*  Upgrade to .NET 4.5.1 (will require dropping XP support)

[nuget-package-restore]: http://docs.nuget.org/docs/reference/package-restore

[mono]: http://mono-project.com/

[bdinfo]: http://cinemasquid.com/blu-ray/tools/bdinfo
[ffmpeg]: http://ffmpeg.org/
[mkvtoolnix]: http://bunkus.org/videotools/mkvtoolnix/

[tmdb]: http://tmdb.org/
[tvdb]: http://thetvdb.com/
[chapterdb]: http://chapterdb.org/
