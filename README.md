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
[tvdb]: http://thetvdb.com/
[chapterdb]: http://chapterdb.org/
[icaros]: http://shark007.net/tools.html
[aacs]: http://en.wikipedia.org/wiki/Advanced_Access_Content_System
[bdplus]: http://en.wikipedia.org/wiki/BD%2B
[dmca]: http://en.wikipedia.org/wiki/Digital_Millennium_Copyright_Act
