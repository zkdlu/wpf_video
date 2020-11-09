using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace VideoMetaInfo.models
{
    class VideoFactory
    {
        public JObject Json { get; private set; }

        public VideoFactory(string jsonPath)
        {
            LoadJson(jsonPath);
        }

        private void LoadJson(string jsonPath)
        {
            try
            {
                string fileText = File.ReadAllText(jsonPath);
                JObject json = JObject.Parse(fileText);

                Json = json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Video> GetVideoes()
        {
            foreach (var videoInfo in Json["videos"])
            {
                string name = videoInfo["video"].ToString();
                int fps = int.Parse(videoInfo["fps"].ToString());
                TimeSpan duration = TimeSpan.Parse(videoInfo["duration"].ToString());

                var metas = new List<Meta>();
                int id = 1;
                foreach (var metaInfo in videoInfo["meta"])
                {
                    Meta meta = new Meta
                    {
                        Id = id++,
                        Tag = metaInfo["tag"].ToString(),
                        BeginFrame = long.Parse(metaInfo["begin"].ToString()),
                        EndFrame = long.Parse(metaInfo["end"].ToString())
                    };

                    metas.Add(meta);
                }

                Video video = new Video(name, fps, duration, metas);

                yield return video;
            }
        }
    }
}
