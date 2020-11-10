using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VideoMetaInfo.common;

namespace VideoMetaInfo.models
{
    class VideoFactory
    {
        private static readonly VideoFactory instance = new VideoFactory();
        public static VideoFactory Instance
        {
            get
            {
                return instance;
            }
        }
        public IList<Label> Labels { get; private set; }

        public void LoadLabel(string xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xml);
            XmlNodeList labelNodes = xmlDocument.SelectNodes("/labels/label");

            List<Label> labels = new List<Label>();

            foreach (XmlNode node in labelNodes)
            {
                string tag = node.InnerText;
                string name = node.Attributes["name"].InnerText;

                Label label = new Label(name, tag);
                labels.Add(label);
            }

            Labels = labels.AsReadOnly();
        }

        public IList<Video> GetVideoes()
        {
            List<Video> totalVideos = new List<Video>();

            foreach (var label in Labels)
            {
                IList<Video> videos = GetVideoesByLabel(label);
                totalVideos.AddRange(videos);
            }

            return totalVideos;
        }

        private IList<Video> GetVideoesByLabel(Label label)
        {
            string videoPath = $"input/video/{label.Name}_video";
            string[] files = Directory.GetFiles(videoPath, "*.mp4");
            if (files.Length == 0)
            {
                Logger.Throw($"Not exist {label.Name} video files");
            }

            var videoes = new List<Video>();

            foreach (string file in files)
            {
                using (ShellFile shellFile = ShellFile.FromFilePath(file))
                using (ShellObject shell = ShellObject.FromParsingName(shellFile.Path))
                {
                    IShellProperty prop = shell.Properties.System.Media.Duration;
                    string durationStr = prop.FormatForDisplay(PropertyDescriptionFormatOptions.None);

                    int fps = (int)shellFile.Properties.System.Video.FrameRate.Value.Value / 1000;
                    TimeSpan duration = TimeSpan.Parse(durationStr);

                    IList<Meta> metas = GetMetasByVideo(label, 
                        Path.GetFileNameWithoutExtension(file));

                    Video video = new Video(file, fps, duration, metas);

                    videoes.Add(video);
                }
            }

            return videoes;
        }

        private IList<Meta> GetMetasByVideo(Label label, string csvName)
        {
            string csvPath = $"input/label/{label.Name}_label/{csvName}.csv";

            if (!File.Exists(csvPath))
            {
                Logger.Throw($"Not exists {csvName}.csv");
            }

            return ReadMetaByCsv(label, csvPath);
        }

        private IList<Meta> ReadMetaByCsv(Label label, string csvFile)
        {
            var metas = new List<Meta>();
            using (StreamReader reader = new StreamReader(csvFile, Encoding.UTF8))
            {
                int before = 0;
                bool isBegin = true;

                long beginFrame = 0;
                long endFrame = 0;

                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] elem = line.Split(',');

                    int frame = int.Parse(elem[0]);
                    int labeled = int.Parse(elem[1]);

                    if (labeled != before)
                    {
                        if (isBegin)
                        {
                            beginFrame = frame;
                            isBegin = false;
                        }
                        else
                        {
                            endFrame = frame - 1;

                            Meta meta = new Meta
                            {
                                Id = 0,
                                Tag = label.Tag,
                                Name = label.Name,
                                BeginFrame = beginFrame,
                                EndFrame = endFrame
                            };

                            metas.Add(meta);

                            isBegin = true;
                        }
                    }

                    before = labeled;
                }
            }

            return metas;
        }
    }
}
