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

            string proxyUrl = "http://127.0.0.1:8080";
            string m3u8standAlone = "https://f8.megacdn.co:2228/v3-hls-playback/8555570a154229fb5f91c1381b93da6ee7aefd31582a6ef3ddf27d0694b5e7583c339266f28086ab8fbc82cc2f268b8b63ed14be7ed6c9dd3c21000ac0ad67ec0690cf36009ebc768bbe35e0f224d1eb32ee281328b3a880e413c4549729713646807b5f27258c594e14f58b9d8935f3c427a5b1861ef6e146dd82eed5f979a91212b98f573105a6179ca459062fd1488cff0f3aea984aadbaac18890ea09f74322b250e252cbef26ab05cb89ce550b96bb3accc5c1bfd07d552ba178c6d9bc0/playlist.m3u8";
            string m3u8TestUrl = $"{proxyUrl}/?url={m3u8standAlone}";

            PlayStream(m3u8TestUrl);
            this.test();
        }

        async void test()
        {
            TMDBSearchResult[] search = await APIManager.Instance.SearchMovies("Titanic");
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
