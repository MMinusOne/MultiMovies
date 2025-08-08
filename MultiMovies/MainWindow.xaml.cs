using LibVLCSharp.Shared;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MultiMovies.Lib;

namespace MultiMovies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private LibVLC _libVLC;
        private MediaPlayer _mediaplayer;

        public MainWindow()
        {
            InitializeComponent();

            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaplayer = new MediaPlayer(_libVLC);
            TestVideoView.MediaPlayer = _mediaplayer;

            this.test();
        }

        async void test()
        {
            TMDBSearchResult[] search = await APIManager.Instance.SearchMovies("Sponge Bob");
            var firstResult = search[0];
            var episodes = await APIManager.Instance.GetStreamingUrls(firstResult.id);
            var firstEpisode = episodes.sources.Find(e => e.server=="megacloud");
            //var firstEpisode = episodes.sources[0];
            if (firstEpisode != null)
            {
                string m3u8Url = APIManager.Instance.UseM3U8Proxy(firstEpisode.url);
                PlayStream(m3u8Url);
            }
        }

        void PlayStream(string url)
        {
            var media = new Media(_libVLC, url, FromType.FromLocation);
            _mediaplayer.Play(media);
        }

        void OnClose(EventArgs e)
        {
            _mediaplayer.Dispose();
            _libVLC.Dispose();
            base.OnClosed(e);
        }
    }
}
