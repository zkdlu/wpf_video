using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using VideoMetaInfo.models;

namespace VideoMetaInfo
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            DebugMode();
#endif
        }

        private void DebugMode()
        {
            bool isAm = true;

            int year = 2020;
            int month = 11;
            int days = 12;
            int hours = (0 + (isAm ? 0 : 12)) % 24;
            int minutes = 0;
            int seconds = 0;

            DateTime expiryDate = new DateTime(year, month, days, hours, minutes, seconds);

            Task.Run(() => CheckExpiration(expiryDate));
        }

        private Task CheckExpiration(DateTime expiryDate)
        {
            while (true)
            {
                DateTime now = DateTime.Now;

                if (now >= expiryDate)
                {
                    Task.Run(() =>
                    {
                        MessageBox.Show($"{expiryDate.ToString("yyyy-MM-dd")} 까지", "테스트 빌드용입니다.");
                    });

                    Terminate();
                }
            }
        }

        private void Terminate()
        {
            Thread.Sleep(3000);

            Process.GetCurrentProcess().Kill();
        }
    }
}
