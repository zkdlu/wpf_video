using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoMetaInfo.common;
using VideoMetaInfo.models;

namespace VideoMetaInfo.viewmodels
{
    class ThumbnailViewModel : BaseViewModel
    {
        public IList<VideoModel> TotalVideos { get; set; }

        public IList<string> VideoTags
        {
            get
            {
                var result = VideoFactory.Instance.Labels.Select(label =>
                {
                    return label.Tag;

                }).ToList();
                result.Insert(0, "전체");

                return result;
            }
        }

        private ObservableCollection<VideoModel> videos;
        public ObservableCollection<VideoModel> Videos 
        {
            get { return videos; }
            set
            {
                videos = value;
                OnRaiseProperty(nameof(Videos));
            }
        }

        private string selectedTag;
        public string SelectedTag 
        {
            get { return selectedTag; }
            set
            {
                if (value.Equals(selectedTag))
                {
                    return;
                }

                selectedTag = value;
                InMemory.Set("tag", value);
                
                FilteringVideo(selectedTag);
                OnRaiseProperty(nameof(SelectedTag));
            }
        }
        
        public ThumbnailViewModel()
        {
            LoadVideo();
        }

        private void LoadVideo()
        {
            VideoFactory videoFactory = VideoFactory.Instance;
            videoFactory.LoadLabel("resources/label.xml");
            var videos = videoFactory.GetVideoes();

            TotalVideos = videos
                .Select(v => new VideoModel(v)).ToList();

            Videos = new ObservableCollection<VideoModel>(TotalVideos);
        }

        private void FilteringVideo(string selectedTag)
        {
            if ("전체".Equals(selectedTag))
            {
                Videos = new ObservableCollection<VideoModel>(TotalVideos);
            }
            else
            {
                Videos = new ObservableCollection<VideoModel>(
                    TotalVideos.Where(v => 
                    {
                        return v.Video.ContainTag(selectedTag);
                    }).ToList());
            }
        }
    }
}
