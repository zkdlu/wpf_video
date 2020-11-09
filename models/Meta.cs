using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;
using System.Windows.Input;
using VideoMetaInfo.common;
using VideoMetaInfo.viewmodels;

namespace VideoMetaInfo.models
{
    public class Meta
    {
        public int Id { get; set; }

        public string Tag { get; set; }

        public long BeginFrame { get; set; }

        public long EndFrame { get; set; }

        public ICommand SelectMetaCommand { get; set; }

        public Meta()
        {
            SelectMetaCommand = new RelayCommand(OnSelectMeta);
        }

        private void OnSelectMeta()
        {
            if (InMemory.Can("SelectedVideo"))
            {
                if (InMemory.Get("SelectedVideo") is Video video)
                {
                    int fps = video.Fps;
                    TimeSpan duration = video.Duration;
                    long frame = (long)BeginFrame;

                    double time = ((double)frame / fps);
                    Mediator.NotifyColleagues("SetPosition", time);
                    Mediator.NotifyColleagues("OnProgress", time);
                }
            }
        }
    }
}