using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace VideoMetaInfo.models
{
    public class Video
    {
        public IList<Meta> MetaInfos { get; set; }

        public string Name { get; set; }

        public int Fps { get; set; }

        public TimeSpan Duration { get; set; }

        public BitmapImage Thumbnail { get; set; }

        public Video(string name, int fps, TimeSpan duration, IList<Meta> metas)
        {
            Name = name;
            Fps = fps;
            Duration = duration;
            MetaInfos = metas;

            Thumbnail = MakeThumbnail(name);
        }

        public bool ContainTag(string tag)
        {
            return MetaInfos.Where(m => m.Tag == tag).Count<Meta>() != 0;
        }

        private BitmapImage MakeThumbnail(string name)
        {
            using (ShellFile shellFile = ShellFile.FromFilePath(name))
            {
                Bitmap bitmap = shellFile.Thumbnail.Bitmap;

                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;

                image.DecodePixelWidth = 120;
                image.DecodePixelHeight = 80;

                image.EndInit();

                return image;
            }
        }
    }
}
