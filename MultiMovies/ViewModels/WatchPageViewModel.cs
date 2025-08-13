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

        ICommand _serverSelectCommand;

        public ICommand ServerSelectCommand
        {

            get
            {
                if (_serverSelectCommand == null)
                {
                    _serverSelectCommand = new RelayCommand(ServerSelectExecute, (object parameter) => true);
                }
                return _serverSelectCommand;
            }
        }

        ObservableCollection<EpisodeSource> _sources;
        public ObservableCollection<EpisodeSource> Sources { 
        get { return _sources;  }
            set { _sources = value; OnPropertyChanged(nameof(Sources)); }
        }

        public WatchPageViewModel()
        {
            _instance = this;
            test();
        }

        async void test()
        {
            var movieDetails = await APIManager.Instance.GetStreamingUrls("400160");
            MovieDetails = movieDetails;
            Sources = MovieDetails.sources;
            Source = movieDetails.sources[2];
            var url = APIManager.Instance.UseM3U8Proxy(Source.url);
            MediaPlayerViewModel.Instance.PlayStream(url);
        }

        void ServerSelectExecute(object obj)
        {
            var serverSelected = (EpisodeSource)obj;
            Source = null;
            Source = serverSelected;
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
