# FFmpeg Usage

From http://ffmpeg.org/trac/ffmpeg/wiki/How%20to%20concatenate%20(join%2C%20merge)%20media%20files#samecodec

More examples at http://pvdm.xs4all.nl/wiki/index.php5/Convert_an_AVCHD_/_MTS_file_to_MP4_using_ffmpeg

There are 2 ways to concatenate multiple ```.m2ts``` input files:

## Method 1: Concat Protocol (WORKS)

### First A/V/S Tracks Only (Succeeds)

    ffmpeg -y -i "concat:BDMV\STREAM\00000.M2TS|BDMV\STREAM\00001.M2TS" -c copy CONCAT.mkv

### Selected Tracks Only (Succeeds - VERY SLOW)

    ffmpeg -y -i "concat:BDMV\STREAM\00000.M2TS|BDMV\STREAM\00001.M2TS" -map 0:0 -map 0:1 -map 0:2 -map 0:4 -map 0:5 -map 0:6 -map 0:7 -map 0:8 -c copy -c:a:1 pcm_s16le CONCAT.mkv

Notes:

> VERY VERY SLOW
> FFmpeg automatically selects ```pcm_bluray``` for the decoder
> Need to explicitly specify ```pcm_s16le``` for the encoder

### Selected Tracks Only (No PCM)

    ffmpeg -y -i "concat:BDMV\STREAM\00000.M2TS|BDMV\STREAM\00001.M2TS" -map 0:0 -map 0:1 -map 0:4 -map 0:5 -map 0:6 -map 0:7 -map 0:8 -c copy CONCAT.mkv

## Method 2: Concat Demuxer (DOES NOT WORK)

The concat demuxer was added to ffmpeg 1.1 . You can read about it in the documentation.

Create a file "mylist.txt" with all the files you want to have concatenated in the following form ( Lines starting with a dash are ignored ) :

    # this is a comment
    file '/path/to/file1'
    file '/path/to/file2'
    file '/path/to/file3'

Note that these can be either relative or absolute paths. Then you can encode your files with:

    ffmpeg -f concat -i mylist.txt -c copy output
