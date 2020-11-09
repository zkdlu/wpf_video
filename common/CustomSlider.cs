using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VideoMetaInfo.common
{
    class CustomSlider : Slider
    {
        public static RoutedEvent MoveEvent =
            EventManager.RegisterRoutedEvent("Moved", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(CustomSlider));

        public event RoutedEventHandler Moved
        {
            add { AddHandler(MoveEvent, value); }
            remove { RemoveHandler(MoveEvent, value); }
        }

        private bool defaultIsMoveToPointEnabled;

        public static readonly DependencyProperty AutoMoveProperty =
            DependencyProperty.Register(
           nameof(AutoMove),
           typeof(bool),
           typeof(CustomSlider),
           new FrameworkPropertyMetadata(false, ChangeAutoMoveProperty));

        public bool AutoMove
        {
            get { return (bool)GetValue(AutoMoveProperty); }
            set { SetValue(AutoMoveProperty, value); }
        }

        private static void ChangeAutoMoveProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomSlider slider = d as CustomSlider;

            if (slider != null)
            {
                if ((bool)e.NewValue)
                {
                    slider.defaultIsMoveToPointEnabled = slider.IsMoveToPointEnabled;
                    slider.IsMoveToPointEnabled = true;
                    slider.PreviewMouseMove += CustomSlider_PreviewMouseMove;
                }
                else
                {
                    slider.IsMoveToPointEnabled = slider.defaultIsMoveToPointEnabled;
                    slider.PreviewMouseMove -= CustomSlider_PreviewMouseMove;
                }
            }
        }

        private static void CustomSlider_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                CustomSlider slider = sender as CustomSlider;

                Point point = e.GetPosition(slider);

                // 현재 Slider 내 마우스 좌표 값을 Value 값으로 계산.
                slider.Value = point.X / (slider.ActualWidth / slider.Maximum);

                RoutedEventArgs args = new RoutedEventArgs(MoveEvent, typeof(CustomSlider));
                slider.RaiseEvent(args);
            }
        }
    }
}
