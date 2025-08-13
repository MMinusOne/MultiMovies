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

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TMDBSearchResult> MoviesResults
        {
            get { return _moviesResults; }
            set { _moviesResults = value; OnPropertyChanged(nameof(MoviesResults)); }
        }

        public SearchPageViewModel()
        {

            _instance = this;
            test();
        }

        async void test()
        {
            await Search("Titanic");
        }

        async Task<ObservableCollection<TMDBSearchResult>> Search(string query)
        {
            MoviesResults = new ObservableCollection<TMDBSearchResult>();
            var searchResults = await APIManager.Instance.SearchMovies(query);
            MoviesResults = searchResults;
            OnPropertyChanged(nameof(MoviesResults));
            return searchResults;
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
