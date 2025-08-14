using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMovies.Lib
{
    public class TMDBGenre
    {
        public int id { get; set; }
        public int name { get; set; }
    }

    public class TMDBVideo
    {
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string site { get; set; }
        public string type { get; set; }
    }

    public class TMDBMovieResponse
    {
        public string id { get; set; }
        public string title { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public string backdrop_path { get; set; }
        public string release_date { get; set; }
        public int vote_average { get; set; }
        public TMDBGenre[] genres { get; set; }
        public string status { get; set; }
        public TMDBVideos videos { get; set; }
    }

    public class TMDBVideos
    {
        public TMDBVideo[] videos { get; set; }
    }

    public class TMDBSearchResult
    {
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public int[] genre_ids { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public string popularity { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public double vote_count { get; set; }
    }

    public class TMDBEmbedMovieResult
    {
        public string tmdbId { get; set; }
        public string tmdbTitle { get; set; }
        public string tmdbPosterPath { get; set; }
        public string tmdbBackdropPath { get; set; }
        public string tmdbPosterUrl { get; set; }
        public string tmdbBackdropUrl { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public ObservableCollection<EpisodeSource> sources { get; set; }
    }

    public class EpisodeSource
    {
        public string server { get; set; }
        public string url { get; set; }
        public bool isM3U8 { get; set; }
        public string quality { get; set; }
        public ObservableCollection<Subtitle> subtitles { get; set; }
    }

    public class Subtitle
    {
        public string file { get; set; }
        public string label { get; set; }
        public string kind { get; set; }
    }
}