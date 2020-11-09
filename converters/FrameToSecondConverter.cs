using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using VideoMetaInfo.common;
using VideoMetaInfo.models;

namespace VideoMetaInfo.converters
{
    public class FrameToSecondConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (InMemory.Can("SelectedVideo"))
            {
                if (InMemory.Get("SelectedVideo") is Video video)
                {
                    int fps = video.Fps;
                    TimeSpan duration = video.Duration;
                    long frame = (long)value;

                    int time = (int)(frame / fps);

                    return new TimeSpan(0, 0, time);
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}
