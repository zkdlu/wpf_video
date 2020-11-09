using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using VideoMetaInfo.models;

namespace VideoMetaInfo.viewmodels
{
    class PlayerViewModel : BaseViewModel
    {
        private bool isPause = false;

        private string state;
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                OnRaiseProperty(nameof(State));
            }
        }

        private double val;
        public double Value
        {
            get { return val; }
            set
            {
                val = value;
                OnRaiseProperty(nameof(Value));
            }
        }

        private double maximum;
        public double Maximum
        {
            get { return maximum; }
            set
            {
                maximum = value;
                OnRaiseProperty(nameof(Maximum));
            }
        }

        private Video video;
        public Video Video 
        {
            get { return video; }
            set
            {
                video = value;
                OnRaiseProperty(nameof(Video));
            }
        }

        private ObservableCollection<Meta> metas;
        public ObservableCollection<Meta> Metas
        {
            get { return metas; }
            set
            {
                metas = value;
                OnRaiseProperty(nameof(Metas));
            }
        }

        public ICommand PlayerChangeCommand { get; private set; }
        public ICommand StateChangeCommand { get; private set; }

        public PlayerViewModel()
        {
            Mediator.Register(nameof(OnChangeTag), OnChangeTag);
            Mediator.Register(nameof(OnPlayVideo), OnPlayVideo);

            Mediator.Register(nameof(SetProgress), SetProgress);
            Mediator.Register(nameof(OnProgress), OnProgress);

            Mediator.Register(nameof(ToggleVideo), ToggleVideo);

            PlayerChangeCommand = new RelayCommand(OnPlayerChanged);
            StateChangeCommand = new RelayCommand(OnStateChanged);
        }

        private void OnStateChanged()
        {
            VideoStart(isPause);
        }

        private void OnPlayerChanged()
        {
            ToggleVideo();
            Mediator.NotifyColleagues("SetPosition", Value);
            ToggleVideo();
        }

        private void OnProgress(object obj)
        {
            if (obj is double currentSecond)
            {
                Value = currentSecond;
            }
        }

        private void SetProgress(object obj)
        {
            if (obj is double totalSecond)
            {
                Maximum = totalSecond;
            }
        }

        private void OnChangeTag(object obj)
        {
            if (obj is string tag)
            {
                if ("전체".Equals(tag) || "".Equals(tag))
                {
                    Metas = new ObservableCollection<Meta>(Video.MetaInfos);
                }
                else
                {
                    Metas = new ObservableCollection<Meta>(
                        Video.MetaInfos.Where(m => m.Tag.Equals(tag)).ToList());
                }
            }
        }

        private void OnPlayVideo(object obj)
        {
            Video = obj as Video;
            Value = 0;

            VideoStart(true);
        }

        private void VideoStart(bool start)
        {
            isPause = !start;

            if (isPause)
            {
                Mediator.NotifyColleagues("Pause", null);
                State = "재생";
            }
            else
            {
                Mediator.NotifyColleagues("Start", null);
                State = "중지";
            }
        }

        private void ToggleVideo(object obj = null)
        {
            VideoStart(isPause);
        }
    }
}
