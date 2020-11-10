using System;
using System.Diagnostics;
using System.Windows;
using VideoMetaInfo.models;

namespace VideoMetaInfo.viewmodels
{
    class VideoViewModel : BaseViewModel
    {
        public Video Video { get; private set; }
        
        public VideoViewModel()
        {
            Mediator.Register(nameof(OnPlayVideo), OnPlayVideo);
        }

        private void OnPlayVideo(object obj)
        {
            Video = obj as Video;
        }
    }
}
