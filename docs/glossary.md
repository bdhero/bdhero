# Glossary

The following is a list of terms used by BDHero, Blu-ray, Matroska, and the audio/video community in general.

*  **Codec** _(noun)_: Short for coder-decoder.  The program used to encode or decode an audio/video/subtitle _track_.

    While technically incorrect, in common usage the word _codec_ also refers to the digital coding standard
    or compression format of the track being encoded or decoded by the _actual_ codec (program).

    For more information, see [Codec][codec] on Wikipedia.

*  **Container File** _(noun)_: A single file that holds one or more _track_s, as well as any associated
    _metadata_ for the movie and/or tracks.

*  **Container Format** _(noun)_: The method by which data is physically stored in a _container file_.

    Every container format has at least one file extension associated with it; some have several.

    Examples:

    *  **Matroska**: ```.mkv``` (video), ```.mka``` (audio)

        Codec agnostic.  Can theoretically hold any arbitrary type of data and any codec.

    *  **MPEG-4 video (standard)**: ```.mp4```

        Only supports MPEG-4 audio and video.  Only supports bitmap subtitles (not plain text or closed captioning).

    *  **MPEG-4 video (Apple)**: ```.m4v```

        Apple's proprietary extension to the "standard" MPEG-4 video container (```.mp4```).
        Only supports MPEG-4 audio and video.  Only supports bitmap subtitles (not plain text or closed captioning).

    *  **AVI**: ```.avi```

        Supports nearly all audio and video codecs.  Does not support chapters or subtitles.

    For more information, see [Container format][container-format] or [Comparison of container formats][container-format-comparison]
    on Wikipedia.

*  **Demux** _(verb)_: Short for de-multiplex.  The process of extracting the raw data for one or more _tracks_ from a _container file_.

    Once demuxed, the tracks may either be muxed into another _container file_ or saved as individual files.

*  **Elementary Stream** _(noun)_: Synonym for _track_ and _elementary stream_.  The MPEG and Blu-ray specs use this term.

*  **Encoding Format** _(noun)_: A standard that specifies _how_ audio/video/subtitle information is physically stored in bytes.

*  **Metadata** _(noun)_: Information that describes an aspect or property of a movie or _track_.

    Title and language are probably the most common types of metadata stored in audio/video containers.

*  **Mux** _(verb)_: Short for multiplex.  The process of combining the raw data for one or more _track_s into a single _container file_.

    For more information, see [Multiplexing][multiplexing] on Wikipedia.

*  **Playlist** _(noun)_: A Blu-ray .MPLS file.  Represents a single, complete movie, episode, special feature, or commercial.

    Choosing the right playlist can be one of the more difficult aspects of muxing a Blu-ray movie to MKV.
    For a movie, there are typically only one or two "main feature" playlists on the disc, but finding them can be a challenge.
    BDHero solves this problem by filtering out all the short and bogus playlists that you probably don't want
    so that all that's left are the main features.

*  **Stream** _(noun)_: Synonym for _track_ and _elementary stream_.  The [FFmpeg][ffmpeg] project uses this term.

*  **Track** _(noun)_: An individual audio, video, or subtitle recording.

    Typically the output of an audio or video encoder.

    **NOTE**: _track_, _stream_, and _elementary stream_ all mean the same thing.
    We prefer the term _track_ because that's what the Matroska spec uses.

[ffmpeg]: http://ffmpeg.org/
[multiplexing]: http://en.wikipedia.org/wiki/Multiplexing
[codec]: http://en.wikipedia.org/wiki/Codec
[container-format]: http://en.wikipedia.org/wiki/Container_format_(digital)
[container-format-comparison]: http://en.wikipedia.org/wiki/Comparison_of_container_formats
