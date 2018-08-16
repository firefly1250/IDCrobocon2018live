using System.Windows;
using Microsoft.Expression.Encoder.Devices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Threading;
using System;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace IDC2018
{
    /// https://www.codeproject.com/Articles/285964/WPF-Webcam-Control
    /// 

    public class MyCommands
    {
        public static RoutedUICommand FullScreen = new RoutedUICommand("", "FullScreen", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F11, ModifierKeys.None) });
        public static RoutedUICommand SelectDevice = new RoutedUICommand("", "SelectDevice", typeof(MyCommands));
    }

    public partial class MainWindow : Window
    {
        public Collection<EncoderDevice> VideoDevices { get; set; }

        DispatcherTimer timer;
        DateTime start;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            timer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            timer.Tick += (s, e) =>
            {
                var data = DateTime.Now - start;
                if (data.TotalMinutes >= 2) data = TimeSpan.FromMinutes(2);
                timer_block.Text = data.ToString(@"mm':'ss");
            };

            this.CommandBindings.Add(new CommandBinding(MyCommands.FullScreen, async (s, e) =>
            {
                if (menu.Visibility == Visibility.Visible)
                {
                    this.WindowState = WindowState.Normal;
                    menu.Visibility = Visibility.Collapsed;
                    this.WindowStyle = WindowStyle.None;
                    //this.AllowsTransparency = true;
                    this.Topmost = true;
                    this.WindowState = WindowState.Maximized;
                    this.ResizeMode = ResizeMode.NoResize;

                    WebcamViewer.StopPreview();
                    await Task.Delay(100);
                    WebcamViewer.StartPreview();
                }
                else
                {
                    this.ResizeMode = ResizeMode.CanResize;
                    menu.Visibility = Visibility.Visible;
                    //this.AllowsTransparency = true;
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.Topmost = false;
                    this.WindowState = WindowState.Maximized;
                }
            }));


            this.CommandBindings.Add(new CommandBinding(MyCommands.SelectDevice, (s, e) =>
            {
                WebcamViewer.VideoDevice = e.Parameter as EncoderDevice;

                try
                {
                    WebcamViewer.StartPreview();
                }
                catch (Microsoft.Expression.Encoder.SystemErrorException ex)
                {
                    MessageBox.Show("Device is in use by another application");
                }
            }));


            VideoDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
        }

        void UpdateScore()
        {
            var blue_score = ((int)blue_scroll.Value) 
                       + (blue_star1.Opacity == 1 ? 10 : 0)
                       + (blue_star2.Opacity == 1 ? 30 : 0)
                       + (blue_star3.Opacity == 1 ? 10 : 0)
                       + (blue_starball.Opacity == 1 ? 10 : 0);

            if (blue_starball.Opacity == 1) blue_score *= 2;

            blue_scorebox.Text = blue_score.ToString();

            var red_score = ((int)red_scroll.Value)
                       + (red_star1.Opacity == 1 ? 10 : 0)
                       + (red_star2.Opacity == 1 ? 30 : 0)
                       + (red_star3.Opacity == 1 ? 10 : 0)
                       + (red_starball.Opacity == 1 ? 10 : 0);

            if (red_starball.Opacity == 1) red_score *= 2;

            red_scorebox.Text = red_score.ToString();
        }


        private void star_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            image.Opacity = image.Opacity == 1 ? 0.3 : 1;
            UpdateScore();
        }

        private void blue_scroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            blue_insectbox.Text = ((int)blue_scroll.Value).ToString();
            UpdateScore();
        }

        private void red_scroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            red_insectbox.Text = ((int)red_scroll.Value).ToString();
            UpdateScore();
        }

        private void timer_block_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                timer_block.Text = "00:00";
            }
            else
            {
                start = DateTime.Now;
                timer.Start();
            }
        }

        private void scroll_MouseEnter(object sender, MouseEventArgs e) => ((ScrollBar)sender).Opacity = 1;
        private void scroll_MouseLeave(object sender, MouseEventArgs e) => ((ScrollBar)sender).Opacity = 0;
    }
}
