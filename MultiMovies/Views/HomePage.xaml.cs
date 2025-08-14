using MultiMovies.Lib;
using MultiMovies.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiMovies.Views
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text;
            var vm = SearchPageViewModel.Instance;
            await vm.PerformSearch(query);

            this.NavigationService.Navigate(new SearchPage());
        }

        private async void Card_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            var vm = WatchPageViewModel.Instance;
            var selected = (TMDBSearchResult)button.CommandParameter;
            this.NavigationService.Navigate(new WatchPage());
            await vm.SetMovie(selected.id);
        }

        private async void Watch_ButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            var vm = WatchPageViewModel.Instance;
            var selected = (TMDBSearchResult)button.CommandParameter;
            this.NavigationService.Navigate(new WatchPage());
            await vm.SetMovie(selected.id);
        }
    }
}
