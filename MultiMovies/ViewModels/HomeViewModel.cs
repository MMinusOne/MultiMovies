using MultiMovies.Lib;
using System;
using System.Collections.Generic;
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

            // Get random movie from popular movies
            PopularMovie = popularMovies[_rand.Next(popularMovies.Count)];

            //foreach (var movie in popularMovies)
            //{
            //    if (movie.vote_average > PopularMovie.vote_average)
            //        PopularMovie = movie;
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
