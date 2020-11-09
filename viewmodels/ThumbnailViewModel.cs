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

        public IList<string> VideoTags { get; set; }

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
            VideoTags = new List<string> 
            { 
                "전체", "노래하다", "차에 타다", "음식을 섞다",
                "춤추다", "낚시하다", "음식을 굽다", "칼질하다",
                "누워있다", "음식을 볶다", "악수하다", "음료를 따르다",
                "마시다", "박수치다", "하이파이브 하다"
            };
            
            LoadVideo();
        }

        private void LoadVideo()
        {
            VideoFactory videoFactory = new VideoFactory("resources/meta.json");
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
                    TotalVideos.Where(v => v.Video.ContainTag(selectedTag)).ToList());
            }
        }
    }
}
