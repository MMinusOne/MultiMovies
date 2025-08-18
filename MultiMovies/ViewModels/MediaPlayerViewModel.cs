using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using MultiMovies.Lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Vlc.DotNet.Core.Interops.Signatures;

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

        public long _duration;
        public long Duration
        {
            get { return _duration; }
            set { _duration = value; OnPropertyChanged(nameof(Duration)); ; }
        }

        public long _currentTime;
        public long CurrentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; OnPropertyChanged(nameof(CurrentTime)); ; }
        }

        bool isUpdatingInternally = false;

        private DispatcherTimer _timer;
        private DispatcherTimer _timelineThrottle;


        public MediaPlayerViewModel()
        {
            _instance = this;

            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaplayer = new MediaPlayer(_libVLC);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) =>
            {
                if (_mediaplayer.IsPlaying)
                {
                    isUpdatingInternally = true;
                    CurrentTime = _mediaplayer.Time;
                    Duration = _mediaplayer.Media.Duration;
                    if (!TimelineLocked) Timeline = (int)((CurrentTime / (double)Duration) * 100);
                    isUpdatingInternally = false;
                }
            };

        }

        bool _isPlaying;
        public bool IsPlaying {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }

        string CurrentStreamUrl;

        public void PlayStream(string url)
        {
            var media = new Media(_libVLC, url, FromType.FromLocation);
            CurrentStreamUrl = url;
            _mediaplayer.Play(media);
            _timer.Start();
        }

        public void SetSubtitleTrack(string track)
        {
            _mediaplayer.AddSlave(MediaSlaveType.Subtitle, track, true);
        }

        public void SetTrack(int trackIndex)
        {
            _mediaplayer.SetVideoTrack(trackIndex);
        }

        public void SeekTo(TimeSpan time)
        {
            _mediaplayer.Time = (long)time.TotalMilliseconds;
        }


        public void SetVolume(int volume)
        {
            _mediaplayer.Volume = volume;
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
        public int Timeline
        {
            get { return _timeline; }
            set
            {
                _timeline = value;
                OnPropertyChanged(nameof(Timeline));
            }
        }

        bool TimelineLocked = false;

        ICommand _timelineLock;
        public ICommand TimelineLock
        {
            get
            {
                if (_timelineLock == null)
                {
                    _timelineLock = new RelayCommand(TimelineLockExecute, (object e) => true);
                }
                return _timelineLock;
            }
        }

        void TimelineLockExecute(object e)
        {
            TimelineLocked = true;
        }


        ICommand _timelineRelease;
        public ICommand TimelineRelease
        {
            get
            {
                if (_timelineRelease == null)
                {
                    _timelineRelease = new RelayCommand(TimelineReleaseExecute, (object e) => true);
                }
                return _timelineRelease;
            }
        }

        void TimelineReleaseExecute(object e)
        {
            long seekTime = (long)((Timeline / 100.0) * Duration);
            SeekTo(TimeSpan.FromMilliseconds(seekTime));
            TimelineLocked = false;
        }


        ICommand _playPauseCommand;
        public ICommand PlayPauseCommand
        {
            get
            {
                if (_playPauseCommand == null)
                {
                    _playPauseCommand = new RelayCommand(PlayPauseExecute, (object parameter) => true);
                }
                return _playPauseCommand;
            }
        }

        void PlayPauseExecute(object obj)
        {
            if (_mediaplayer.IsPlaying) {
                _mediaplayer.Pause();
                IsPlaying = true;
            } else { 
                _mediaplayer.Play();
                IsPlaying = false;
            }
            
            _timer.Stop();

        }

        int _volume = 50;
        public int Volume
        {
            get { return _volume;  }
            set
            {
                _volume = value;
                SetVolume(_volume);
                OnPropertyChanged(nameof(Volume));
            }
        }

        ObservableCollection<string> _qualities = new ObservableCollection<string>();
        public ObservableCollection<string> Qualities
        {
            get {  return _qualities; }
            set { _qualities = value; OnPropertyChanged(nameof(Qualities)); }
        }


        public void OnEnd()
        {
            Timeline = 0;
            SubtitleLabels = new ObservableCollection<string>();
            CurrentSubtitleLabel = null;
            Subtitles = new ObservableCollection<Subtitle>();
            IsPlaying = false;
            _timer.Stop();
            _mediaplayer.Time = 0;
        }
        public void OnClose()
        {
            _mediaplayer.Dispose();
            _libVLC.Dispose();

        }

        ICommand _downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if(_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand(DownloadExecute, (object e) => true);
                }

                return _downloadCommand;
            }
        }

        public void DownloadExecute(object obj)
        {
            string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "MultiMovies", "Downloads");
            var dupLibVLC = new LibVLC(new string[] { "--no-video-title-show", "--quiet" });
            var dupMediaPlayer = new MediaPlayer(dupLibVLC);
            if (CurrentStreamUrl == null) return;
            using (var media = new Media(_libVLC, CurrentStreamUrl, FromType.FromLocation))
            {
                media.AddOption("($\":sout=#transcode{{vcodec=h264,acodec=mp4a}}:standard{{access=file,mux=mp4,dst={outputFilePath}}}\"");
                media.AddOption(":no-sout-all");
                media.AddOption(":sout-keep");

                dupMediaPlayer.Play(media);

                while (dupMediaPlayer.IsPlaying)
                {
                    System.Threading.Thread.Sleep(1000);
                }

                dupMediaPlayer.Dispose();
            }
        }

        ObservableCollection<Subtitle> _subtitles;
        public ObservableCollection<Subtitle> Subtitles
        {

            get { return _subtitles; }
            set
            {
                _subtitles = value;
                OnPropertyChanged(nameof(Subtitles));
            }
        }

        ObservableCollection<string> _subtitlesLabels;
        public ObservableCollection<string> SubtitleLabels
        {

            get { return _subtitlesLabels; }
            set
            {
                _subtitlesLabels = value;
                OnPropertyChanged(nameof(SubtitleLabels));
            }
        }

        string _currentSubtitleLabel;
        public string CurrentSubtitleLabel
        {
            get => _currentSubtitleLabel;
            set {
                _currentSubtitleLabel = value;
                var subtitle = Subtitles.SingleOrDefault(e => e.label == value);
                if (subtitle != null)
                {
                    SetSubtitleTrack(subtitle.file);
                };
                OnPropertyChanged(nameof(CurrentSubtitleLabel));
            }
        }

        public void SetSubtitles(ObservableCollection<Subtitle> subtitles)
        {
            Subtitles = subtitles;
            var subtitleLabels = new ObservableCollection<string>();
            foreach(var subtitle in subtitles)
            {
                subtitleLabels.Add(subtitle.label);
            }
            SubtitleLabels = subtitleLabels;
        }


        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
