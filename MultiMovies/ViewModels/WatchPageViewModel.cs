using MultiMovies.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultiMovies.ViewModels
{
    internal class WatchPageViewModel : BaseVM, INotifyPropertyChanged
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

        WatchPageViewModel()
        {
            _instance = this;
            ServerSelectCommand = new RelayCommand(ServerSelectExecute, (object parameter) => { return true; });
            test();
        }

        async void test()
        {
            var movieDetails = await APIManager.Instance.GetStreamingUrls("400160");
            MovieDetails = movieDetails;
            Source = movieDetails.sources[0];
            var url = APIManager.Instance.UseM3U8Proxy(Source.url);
            MediaPlayerViewModel.Instance.PlayStream(url);
        }

        public ICommand ServerSelectCommand;
        void ServerSelectExecute(object obj)
        {
            var serverSelected = (EpisodeSource)obj;
            Source = serverSelected;
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
