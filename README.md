# BDHero

An intelligent, automatic, cross-platform Blu-ray™ to MKV muxer.

## Features

All features are fully automatic, but can of course be customized, disabled, or overridden by the user:

*  Lossless conversion to MKV
*  Movie name detection
*  Main movie playlist (.MPLS file) detection
*  Metadata:
    *  Movie title, release year, etc.
    *  Cover art (movie poster) displayed in Windows Explorer (via [Icaros][icaros])
    *  Chapter names
*  Customizable file name and output directory with variables
*  Audio/video/subtitle track auto-select based on preferences (language, codec, channel count, etc.)
*  Cross-platform (via [Mono][mono])
*  Free and Open Source

## Credit

BDHero is little more than a bit of glue and some brains wrapped around existing open-source software:

*  [BDInfo][bdinfo]
*  [FFmpeg][ffmpeg]
*  [MKVToolNix][mkvtoolnix]

# Development

## Required tools

*  Visual Studio 2010+
*  ReSharper 7+
*  NuGet extension for VS

## Project Setup

1.  Open the solution in Visual Studio
2.  Open the NuGet Package Manager Console:

        Tools > Library Package Manager > Package Manager Console

3.  Run the following script:

        .\restore.ps1

4.  Build, debug, and rejoice!

[mono]: http://mono-project.com/
[bdinfo]: http://cinemasquid.com/blu-ray/tools/bdinfo
[ffmpeg]: http://ffmpeg.org/
[mkvtoolnix]: http://bunkus.org/videotools/mkvtoolnix/
[tmdb]: http://tmdb.org/
[chapterdb]: http://chapterdb.org/
[icaros]: http://shark007.net/tools.html
