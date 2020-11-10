using System.Windows;

namespace VideoMetaInfo.common
{
    static class Logger
    {
        public static void Throw(string message)
        {
            MessageBox.Show(message, "Exception", MessageBoxButton.OK);

            throw new System.Exception(message);
        }
    }
}
