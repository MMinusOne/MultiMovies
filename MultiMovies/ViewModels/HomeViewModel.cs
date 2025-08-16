using MultiMovies.Lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MultiMovies.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private static HomeViewModel _instance;
        public static HomeViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HomeViewModel();
                }

                return _instance;
            }
        }

        private static Random _rand = new Random();

        public HomeViewModel()
        {
            _instance = this;
            GetPopularMovies();
        }

        private TMDBSearchResult _popularMovie;


        public TMDBSearchResult PopularMovie
        {
            get => _popularMovie;
            set
            {
                _popularMovie = value;
                OnPropertyChanged();
            }
        }

        private async void GetPopularMovies()
        {
            var popularMovies = await APIManager.Instance.GetPopular();

            if (popularMovies == null) return;

            // Get random movie from popular movies
            PopularMovie = popularMovies[_rand.Next(popularMovies.Count)];

            MovieCards = popularMovies;
        }

        private ObservableCollection<TMDBSearchResult> _movieCards;
        public ObservableCollection<TMDBSearchResult> MovieCards
        {
            get => _movieCards;
            set
            {
                _movieCards = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
