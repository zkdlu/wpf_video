using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using VideoMetaInfo.common;
using VideoMetaInfo.models;
using VideoMetaInfo.viewmodels;

namespace VideoMetaInfo.views
{
    /// <summary>
    /// VideoView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VideoView : UserControl
    {
        private DispatcherTimer timer;

        public Video Video { get; private set; }

        public VideoView()
        {
            InitializeComponent();

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            timer.Tick += TimerTickHandler;

            Mediator.Register(nameof(OnPlayVideo), OnPlayVideo);
            Mediator.Register(nameof(SetPosition), SetPosition);
            Mediator.Register(nameof(Start), Start);
            Mediator.Register(nameof(Pause), Pause);
        }

        private void SetPosition(object obj)
        {
            if (obj is double val)
            {
                mePlayer.Position = TimeSpan.FromSeconds(val);
            }
        }

        private void OnPlayVideo(object obj)
        {
            Video = obj as Video;

            InitVideo(Video);
        }

        private void mePlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            InitProcessBar();
        }

        private void InitProcessBar()
        {
            double totalSecond = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;

            Mediator.NotifyColleagues("SetProgress", totalSecond);
        }

        private void InitVideo(Video video)
        {
            string path = $"{Environment.CurrentDirectory}/{video.Name}";
            mePlayer.Source = new Uri(path);
            mePlayer.Volume = 0.5;
            mePlayer.SpeedRatio = 1;
        }

        void TimerTickHandler(object sender, EventArgs e)
        {
            double currentSecond = mePlayer.Position.TotalSeconds;

            Mediator.NotifyColleagues("OnProgress", currentSecond);
        }

        private void Start(object obj)
        {
            if (mePlayer.Source != null)
            {
                timer.Start();
                mePlayer.Play();
            }
        }

        private void Pause(object obj)
        {
            if (mePlayer.Source != null)
            {
                timer.Stop();
                mePlayer.Pause();
            }
        }

        private void mePlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            timer?.Stop();
        }
    }
}
