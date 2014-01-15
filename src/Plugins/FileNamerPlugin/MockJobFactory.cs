// Copyright 2012, 2013, 2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using BDHero.JobQueue;
using BDInfo;
using DotNetUtils.Extensions;
using I18N;

namespace BDHero.Plugin.FileNamer
{
    internal static class MockJobFactory
    {
        internal static Job CreateMovieJob()
        {
            var metadata = new DiscMetadata
            {
                Derived = new DiscMetadata.DerivedMetadata
                {
                    VolumeLabel = "EMPIRE_STRIKES_BACK"
                }
            };
            var disc = new Disc
            {
                Metadata = metadata,
                Playlists = new List<Playlist>
                        {
                            new Playlist
                                {
                                    Tracks = new List<Track>
                                        {
                                            new Track
                                                {
                                                    IsVideo = true,
                                                    Codec = Codec.AVC,
                                                    Type = TrackType.MainFeature,
                                                    VideoFormat = TSVideoFormat.VIDEOFORMAT_1080p,
                                                    AspectRatio = TSAspectRatio.ASPECT_16_9,
                                                    Index = 0,
                                                    IndexOfType = 0,
                                                    IsBestGuess = true,
                                                    Keep = true,
                                                    Language = Language.English
                                                },
                                            new Track
                                                {
                                                    IsAudio = true,
                                                    Codec = Codec.DTSHDMA,
                                                    Type = TrackType.MainFeature,
                                                    ChannelCount = 6.1,
                                                    Index = 1,
                                                    IndexOfType = 0,
                                                    IsBestGuess = true,
                                                    Keep = true,
                                                    Language = Language.English
                                                },
                                            new Track
                                                {
                                                    IsSubtitle = true,
                                                    Codec = Codec.PGS,
                                                    Type = TrackType.MainFeature,
                                                    Index = 2,
                                                    IndexOfType = 0,
                                                    IsBestGuess = true,
                                                    Keep = true,
                                                    Language = Language.English
                                                },
                                        }
                                }
                        }
            };
            var job = new Job(disc)
            {
                ReleaseMediumType = ReleaseMediumType.Movie,
                SearchQuery = new SearchQuery
                {
                    Title = "Star Wars: Episode V - The Empire Strikes Back",
                    Year = 1980,
                    Language = Language.English
                }
            };
            job.Movies.Add(new Movie
            {
                IsSelected = true,
                Title = "Star Wars: Episode V - The Empire Strikes Back",
                ReleaseYear = 1980,
                Id = 1891,
                Url = "http://www.themoviedb.org/movie/1891-star-wars-episode-v-the-empire-strikes-back"
            });
            return job;
        }

        internal static Job CreateTVShowJob()
        {
            var metadata = new DiscMetadata
                {
                    Derived = new DiscMetadata.DerivedMetadata
                        {
                            VolumeLabel = "SCRUBS_S1_D1"
                        }
                };
            var disc = new Disc
                {
                    Metadata = metadata,
                    Playlists = new List<Playlist>
                        {
                            new Playlist
                                {
                                    Tracks = new List<Track>
                                        {
                                            new Track
                                                {
                                                    IsVideo = true,
                                                    Codec = Codec.AVC,
                                                    Type = TrackType.MainFeature,
                                                    VideoFormat = TSVideoFormat.VIDEOFORMAT_1080p,
                                                    AspectRatio = TSAspectRatio.ASPECT_16_9,
                                                    Index = 0,
                                                    IndexOfType = 0,
                                                    IsBestGuess = true,
                                                    Keep = true,
                                                    Language = Language.English
                                                },
                                            new Track
                                                {
                                                    IsAudio = true,
                                                    Codec = Codec.DTSHDMA,
                                                    Type = TrackType.MainFeature,
                                                    ChannelCount = 5.1,
                                                    Index = 1,
                                                    IndexOfType = 0,
                                                    IsBestGuess = true,
                                                    Keep = true,
                                                    Language = Language.English
                                                },
                                            new Track
                                                {
                                                    IsSubtitle = true,
                                                    Codec = Codec.PGS,
                                                    Type = TrackType.MainFeature,
                                                    Index = 2,
                                                    IndexOfType = 0,
                                                    IsBestGuess = true,
                                                    Keep = true,
                                                    Language = Language.English
                                                },
                                        }
                                }
                        }
                };
            var job = new Job(disc)
                {
                    ReleaseMediumType = ReleaseMediumType.TVShow,
                    SearchQuery = new SearchQuery
                        {
                            Title = "Scrubs",
                            Year = 2001,
                            Language = Language.English
                        }
                };
            var tvShow = new TVShow
                {
                    IsSelected = true,
                    Title = "Scrubs",
                    Id = 76156,
                    Url = "http://thetvdb.com/?tab=series&id=76156&lid=7",
                    SelectedEpisodeIndex = 0
                };
            tvShow.Episodes.AddRange(new[]
                {
                    new TVShow.Episode
                        {
                            SeasonNumber = 1,
                            EpisodeNumber = 1,
                            Title = "My First Day",
                            Id = 184602,
                            ReleaseDate = DateTime.Parse("2001-10-02")
                        }
                });
            job.TVShows.Add(tvShow);
            return job;
        }
    }
}
