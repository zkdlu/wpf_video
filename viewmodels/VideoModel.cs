using GalaSoft.MvvmLight.Command;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using VideoMetaInfo.common;
using VideoMetaInfo.models;

namespace VideoMetaInfo.viewmodels
{
    class VideoModel : BaseViewModel
    {
        public Video Video { get; private set; }
        
        public ICommand SelectCommand { get; private set; }

        public VideoModel(Video video)
        {
            Video = video;

            SelectCommand = new RelayCommand(OnSelected);
        }

        private void OnSelected()
        {
            InMemory.Set("SelectedVideo", Video);

            Mediator.NotifyColleagues("OnPlayVideo", Video);
            if (InMemory.Can("tag"))
            {
                Mediator.NotifyColleagues("OnChangeTag", InMemory.Get("tag"));
            }
            else
            {
                Mediator.NotifyColleagues("OnChangeTag", string.Empty);
            }
        }
    }
}
