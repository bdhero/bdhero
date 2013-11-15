# BDHero

An intelligent, automatic, cross-platform Blu-ray™ to MKV muxer.

## Features

All features are fully automatic, and most can be customized or disabled if desired:

*  Lossless conversion to MKV
*  Movie name detection
*  Main movie playlist (.MPLS file) detection
*  Metadata:
    *  Movie title, release year, etc.
    *  Cover art (movie poster) displayed in Windows Explorer (via [Icaros][icaros])
    *  Chapter names
*  Customizable file name and output directory with variables
*  Audio/video/subtitle track auto-select based on preferences (language, codec, channel count, etc.)
*  Free and Open Source

## Requirements

*  .NET 4.0
*  Windows XP, Vista, 7, 8, or 8.1 (32- or 64-bit)
*  ~20-50 GB of free disk space per movie

You can use a tool such as [HandBrake][handbrake] to reduce the file size after muxing.

## Feature Roadmap

*  Cross-platform via [Mono][mono] (in progress)
*  TV show support (not started)

## Copy Protection

**BDHero does not circumvent [AACS][aacs]/[BD+][bdplus] copy protection** in any way,
as doing so would violate the [DMCA][dmca].

BDHero is **only** intended for use with **unencrypted** Blu-ray Discs®.

## Credit

BDHero is little more than a bit of glue and some brains wrapped around existing open-source software:

*  [BDInfo][bdinfo]
*  [FFmpeg][ffmpeg]
*  [MKVToolNix][mkvtoolnix]

BDHero also uses the following web services for content:

*  [TMDb][tmdb]
*  [TVDB][tvdb]
*  [ChapterDb][chapterdb]

BDHero would not be possible without the fantastic work of the above projects.

## Donations

We do not accept monetary donations of any kind.  If BDHero has made your life easier
and you wish to thank us, please consider making a donation to one of the above projects.
You'll be glad you did :-)

# Development

BDHero is written in C# / .NET 4.0.

## Required Tools

*  Visual Studio 2010+

## Recommended Tools

*  ReSharper 7+
*  NuGet extension for VS

## Project Setup

When you first open the solution in Visual Studio, you will see a ton of errors about missing assembly references.
This is because we use NuGet Package Restore instead of keeping our NuGet packages in source control.

Simply build the solution to automatically restore all missing NuGet packages and fix the reference errors.

## Development Roadmap

*  [Mono][mono] compatibility
*  Upgrade to .NET 4.5.1 (will require dropping XP support)

[mono]: http://mono-project.com/
[bdinfo]: http://cinemasquid.com/blu-ray/tools/bdinfo
[ffmpeg]: http://ffmpeg.org/
[mkvtoolnix]: http://bunkus.org/videotools/mkvtoolnix/
[handbrake]: http://handbrake.fr/
[tmdb]: http://tmdb.org/
[tvdb]: http://thetvdb.com/
[chapterdb]: http://chapterdb.org/
[icaros]: http://shark007.net/tools.html
[aacs]: http://en.wikipedia.org/wiki/Advanced_Access_Content_System
[bdplus]: http://en.wikipedia.org/wiki/BD%2B
[dmca]: http://en.wikipedia.org/wiki/Digital_Millennium_Copyright_Act
