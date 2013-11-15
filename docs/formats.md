# Format Reference

## Container Formats

*  **MPEG-2 Transport Stream (M2TS)**
*  **Matroska (MKV)**

## Encoding Formats and Codecs

### Audio

#### Lossless

##### Uncompressed

*  **Blu-ray LPCM**: Blu-ray-specific LPCM

    See "PCM".

*  **LPCM**: Linear Pulse-Code Modulation

    See "PCM".

*  **PCM**: Pulse-Code Modulation

    PCM is broad, generic **concept** that in practice is used very loosely to refer to many different things,
    and as such tends to cause confusion among those who are new to the world of audio formats.

    In common usage, PCM typically refers either to a specific _encoding format_
    (the standard that specifies _how_ data is stored) or to a particular _codec_
    (the _program_ used to actually encode/decode the data).  This is a very important distinction
    and is probably the single greatest source of confusion for most people.

    LPCM is a _type_ of PCM that uses linear quantization.  Like regular PCM, LPCM is a broad, generic **concept** -
    _**NOT**_ a specific _encoding format_.  Some encoding formats employ the _concept_ of LPCM, but each does so
    in its own specific way.  LPCM is employed by Blu-ray for storing lossless uncompressed audio.

    There are many different _encoding formats_ that use the _concept_ of PCM.

    For example, these are some of the PCM _encoding formats_ that FFmpeg can encode/decode (write/read):

    (run ```ffmpeg -formats``` for the full list)

        D  = Decoding supported
         E = Encoding supported
        --
        DE  f32be  -  32-bit floating-point big-endian
        DE  f32le  -  32-bit floating-point little-endian
        DE  f64be  -  64-bit floating-point big-endian
        DE  f64le  -  64-bit floating-point little-endian
        DE  s16be  -  signed 16-bit big-endian
        DE  s16le  -  signed 16-bit little-endian
        DE  s24be  -  signed 24-bit big-endian
        DE  s24le  -  signed 24-bit little-endian
        DE  s32be  -  signed 32-bit big-endian
        DE  s32le  -  signed 32-bit little-endian
        DE  s8     -  signed 8-bit
        DE  u16be  -  unsigned 16-bit big-endian
        DE  u16le  -  unsigned 16-bit little-endian
        DE  u24be  -  unsigned 24-bit big-endian
        DE  u24le  -  unsigned 24-bit little-endian
        DE  u32be  -  unsigned 32-bit big-endian
        DE  u32le  -  unsigned 32-bit little-endian
        DE  u8     -  unsigned 8-bit

    These are some of the _codecs_ (encoding/decoding _programs_) that FFmpeg ships with
    to encode/decode (write/read) the above formats:

    (run ```ffmpeg -codecs``` for the full list)

        D  = Decoding supported
         E = Encoding supported
        --
        D   pcm_bluray        -  signed 16|20|24-bit big-endian for Blu-ray media
        D   pcm_dvd           -  signed 20|24-bit big-endian
        DE  pcm_f32be         -  32-bit floating point big-endian
        DE  pcm_f32le         -  32-bit floating point little-endian
        DE  pcm_f64be         -  64-bit floating point big-endian
        DE  pcm_f64le         -  64-bit floating point little-endian
        D   pcm_lxf           -  signed 20-bit little-endian planar
        DE  pcm_s16be         -  signed 16-bit big-endian
        DE  pcm_s16be_planar  -  signed 16-bit big-endian planar
        DE  pcm_s16le         -  signed 16-bit little-endian
        DE  pcm_s16le_planar  -  signed 16-bit little-endian planar
        DE  pcm_s24be         -  signed 24-bit big-endian
        DE  pcm_s24le         -  signed 24-bit little-endian
        DE  pcm_s24le_planar  -  signed 24-bit little-endian planar
        DE  pcm_s32be         -  signed 32-bit big-endian
        DE  pcm_s32le         -  signed 32-bit little-endian
        DE  pcm_s32le_planar  -  signed 32-bit little-endian planar
        DE  pcm_s8            -  signed 8-bit
        DE  pcm_s8_planar     -  signed 8-bit planar
        DE  pcm_u16be         -  unsigned 16-bit big-endian
        DE  pcm_u16le         -  unsigned 16-bit little-endian
        DE  pcm_u24be         -  unsigned 24-bit big-endian
        DE  pcm_u24le         -  unsigned 24-bit little-endian
        DE  pcm_u32be         -  unsigned 32-bit big-endian
        DE  pcm_u32le         -  unsigned 32-bit little-endian
        DE  pcm_u8            -  unsigned 8-bit

    As you can see, FFmpeg's ```pcm_bluray``` codec is able to _decode_ Blu-ray LPCM.
    So to mux a Blu-ray playlist containing a Blu-ray LPCM track to MKV, you need to use
    the ```pcm_bluray``` codec to _decode_ the data on the Blu-ray Disc and another codec
    to _encode_ the data into a different PCM format that your media player can understand
    (either ```pcm_s16le``` or ```pcm_s24le``` for most Matroska players).
