using MultiMovies.Lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMovies.ViewModels
{
    public class SearchPageViewModel : BaseVM, INotifyPropertyChanged
    {
        private static SearchPageViewModel _instance;
        public static SearchPageViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SearchPageViewModel();
                }
                return _instance;
            }
        }
        private ObservableCollection<TMDBSearchResult> _moviesResults = new ObservableCollection<TMDBSearchResult> { };

        private string _query;
        public string Query
        {
            get => _query;
            set
            {
                if (_query != value)
                {
                    _query = value;
                    OnPropertyChanged(nameof(Query));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TMDBSearchResult> MoviesResults
        {
            get { return _moviesResults; }
            set { _moviesResults = value; OnPropertyChanged(nameof(MoviesResults)); }
        }

        public SearchPageViewModel()
        {
            _instance = this;
        }

        //async void test()
        //{
        //    await Search("Titanic");
        //}

        public async Task PerformSearch(string query)
        {
            Query = query;
            await SearchForMovie();
        }

        private async Task SearchForMovie()
        {
            if (string.IsNullOrWhiteSpace(Query)) return;

            MoviesResults = new ObservableCollection<TMDBSearchResult>();
            var searchResults = await APIManager.Instance.SearchMovies(Query);
            if (searchResults == null) return;
            MoviesResults = searchResults;
            OnPropertyChanged(nameof(MoviesResults));
        }

        //async Task<ObservableCollection<TMDBSearchResult>> Search(string query)
        //{
        //    MoviesResults = new ObservableCollection<TMDBSearchResult>();
        //    var searchResults = await APIManager.Instance.SearchMovies(query);
        //    MoviesResults = searchResults;
        //    OnPropertyChanged(nameof(MoviesResults));
        //    return searchResults;
        //}

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
