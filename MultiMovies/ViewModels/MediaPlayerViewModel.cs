using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using MultiMovies.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public void SetSubtitleTrack(string track)
        {
            _mediaplayer.AddSlave(MediaSlaveType.Subtitle, track, true);
        }

        public MediaTrack[] GetQualities() {
            var tracks = _mediaplayer.Media.Tracks;
            return tracks;
        }

        public void SetTrack(int trackIndex)
        {
            _mediaplayer.SetVideoTrack(trackIndex);
        }

        public void SeekTo(TimeSpan time)
        {
            _mediaplayer.Time = (long)time.TotalMilliseconds;
        }

        public TimeSpan GetCurrentTime()
        {
            return TimeSpan.FromMilliseconds(_mediaplayer.Time);
        }

        public long GetMediaDuration()
        {
            return _mediaplayer.Media.Duration;
        }

        int _timeline;
        public int Timeline { 
        get { return _timeline; }
            set { 
                _timeline = value; 
                OnPropertyChanged(nameof(Timeline));
            }
        }

        ICommand _playPauseCommand;
        public ICommand PlayPauseCommand
        {
            get
            {
                if (_playPauseCommand == null) {
                    _playPauseCommand = new RelayCommand(PlayPauseExecute, (object parameter) => true);
                }
                return _playPauseCommand;
            }
        }

        void PlayPauseExecute(object obj) {
            _mediaplayer.Pause();
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
