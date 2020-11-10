using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

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

        public IEnumerable<Video> GetVideoes()
        {
            foreach (var label in Labels)
            {
                IList<Meta> metas = GetMetasByCsv(label);
                yield return GetVideo(metas, label);
            }
        }

        private IList<Meta> GetMetasByCsv(Label label)
        {
            string csvPath = $"input/label/{label.Name}_label";
            string[] files = Directory.GetFiles(csvPath, "*.csv");
            if (files.Length != 1)
            {
                throw new Exception($"Not exist {label.Name} csv file");
            }
            IList<Meta> metas = ReadMetaByCsv(label, files[0]);

            return metas;
        }

        private Video GetVideo(IList<Meta> metas, Label label)
        {
            string videoPath = $"input/video/{label.Name}_video";
            string[] files = Directory.GetFiles(videoPath, "*.mp4");
            if (files.Length != 1)
            {
                throw new Exception($"Not exist {label.Name} video file");
            }

            using (ShellFile shellFile = ShellFile.FromFilePath(files[0]))
            using (ShellObject shell = ShellObject.FromParsingName(shellFile.Path))
            {
                IShellProperty prop = shell.Properties.System.Media.Duration;
                string durationStr = prop.FormatForDisplay(PropertyDescriptionFormatOptions.None);

                string name = files[0];
                int fps = (int)shellFile.Properties.System.Video.FrameRate.Value.Value / 1000;
                TimeSpan duration = TimeSpan.Parse(durationStr);

                Video video = new Video(name, fps, duration, metas);

                return video;
            }
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
