using LibVLCSharp.Shared;
using MultiMovies.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMovies.ViewModels
{
    public class MediaPlayerViewModel : BaseVM, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        static MediaPlayerViewModel _instance;
        public static MediaPlayerViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    return new MediaPlayerViewModel();
                }

                return _instance;
            }
        }


        private LibVLC _libVLC;
        private MediaPlayer _mediaplayer;
        public MediaPlayer Mediaplayer
        {
            get { return _mediaplayer; }
            set { _mediaplayer = value; OnPropertyChanged(nameof(Mediaplayer)); }
        }

        public MediaPlayerViewModel()
        {
            _instance = this;

            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaplayer = new MediaPlayer(_libVLC);
        }

        public void PlayStream(string url)
        {
            var media = new Media(_libVLC, url, FromType.FromLocation);
            _mediaplayer.Play(media);
        }

        void OnClose(EventArgs e)
        {
            _mediaplayer.Dispose();
            _libVLC.Dispose();
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
