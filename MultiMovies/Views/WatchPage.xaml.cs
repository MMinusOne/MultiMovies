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
    /// Interaction logic for WatchPage.xaml
    /// </summary>
    public partial class WatchPage : Page
    {
        public WatchPage()
        {
            InitializeComponent();
        }

        private void GoHome_Click(object sender, RoutedEventArgs e)
        {
            var mediaPlayerVM = MediaPlayerViewModel.Instance;
            mediaPlayerVM.OnEnd();
            this.NavigationService.Navigate(new HomePage());
        }
    }
}
