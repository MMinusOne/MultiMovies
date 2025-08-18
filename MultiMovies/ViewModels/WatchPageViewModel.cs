using MultiMovies.Lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultiMovies.ViewModels
{
    public class WatchPageViewModel : BaseVM, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static WatchPageViewModel _instance;
        public static WatchPageViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WatchPageViewModel();
                }
                return _instance;
            }
        }

        TMDBEmbedMovieResult _movieDetails;
        public TMDBEmbedMovieResult MovieDetails
        {
            get { return _movieDetails; }
            set { _movieDetails = value; OnPropertyChanged(nameof(MovieDetails)); }
        }

        EpisodeSource _source;
        public EpisodeSource Source
        {
            get { return _source; }
            set { _source = value; OnPropertyChanged(nameof(Source)); }
        }

        public WatchPageViewModel()
        {
            _instance = this;
        }

        public async Task SetMovie(string movieId)
        {
            var movieDetails = await APIManager.Instance.GetStreamingUrls(movieId);
            
            MovieDetails = movieDetails;
            
            Source = movieDetails.sources.Single(e=>e.server=="megacloud");
            if (Source == null) Source = movieDetails.sources.Last();
            var url = APIManager.Instance.UseM3U8Proxy(Source.url);
            MediaPlayerViewModel.Instance.PlayStream(url);
            MediaPlayerViewModel.Instance.SetSubtitles(Source.subtitles);
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
