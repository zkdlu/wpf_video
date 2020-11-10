using System;
using System.Globalization;
using System.Windows.Data;

namespace VideoMetaInfo.converters
{
    class FileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string fileName)
            {
                fileName = fileName.Replace('\\', '/');

                int index = fileName.LastIndexOf("/");
                if (index == -1)
                {
                    index = fileName.LastIndexOf("\\");
                }

                if (index == -1)
                {
                    return fileName;
                }

                index++;

                return fileName.Substring(index, fileName.Length - index);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
