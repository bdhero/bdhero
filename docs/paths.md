EXE File Names
==============

> bdhero.exe
> bdheroc.exe

vs.

> bdhero-cli.exe
> bdhero-gui.exe


INSTALLED
=========

    C:\Users\<USERNAME>\AppData
        Local\BDHero
            - Application
                - bdhero-gui.exe
                - bdhero-cli.exe
                - unins000.exe
                - Plugins
                    - README.txt
                    - Required
                        - DiscReader
                            - DiscReaderPlugin.dll
                            - BDInfo.dll
                        - FFmpegMuxer
                            - FFmpegMuxerPlugin.dll
                            - ffmpeg.exe
                        - Tmdb
                            - TmdbPlugin.dll
                            - WatTmdb.dll
                    - Custom
                        - HandBrake
                            - HandBrakePlugin.dll
                            - hb.dll
            - Logs
                - bdhero-cli.log
                - bdhero-gui.log
        Roaming\BDHero
            - Config
                - Application
                    - bdhero.config.json
                    - bdhero-gui.log.config
                    - bdhero-cli.log.config
                - Plugins
                    - DiscReader
                        - TMDb.config.json
                    - FFmpegMuxer
                        - FFmpegMuxer.config.json
                    - HandBrake
                        - HandBrake.config.json
                    - Tmdb
                        - TmdbPlugin.config.json

PORTABLE
========

    <InstallDir>
        - bdhero-gui.exe
        - bdhero-cli.exe
        - Config
            - Application
                - bdhero.config.json
                - bdhero-gui.log.config
                - bdhero-cli.log.config
            - Plugins
                - DiscReader
                    - TMDbPlugin.config.json
                - FFmpegMuxer
                    - FFmpegMuxer.config.json
                - HandBrake
                    - HandBrake.config.json
                - Tmdb
                    - TmdbPlugin.config.json
        - Plugins
            - README.txt
            - Required
                - DiscReader
                    - DiscReader.dll
                    - BDInfo.dll
                - FFmpegMuxer
                    - FFmpegMuxer.dll
                    - ffmpeg.exe
                - Tmdb
                    - TmdbPlugin.dll
                    - WatTmdb.dll
            - Custom
                - HandBrake
                    - HandBrake.dll
                    - hb.dll
        - Logs
            - bdhero-cli.log
            - bdhero-gui.log
