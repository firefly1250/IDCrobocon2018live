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

        Window1 window1;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            this.CommandBindings.Add(new CommandBinding(MyCommands.FullScreen, async (s, e) =>
            {
                if (menu.Visibility == Visibility.Visible)
                {
                    this.WindowState = WindowState.Normal;
                    menu.Visibility = Visibility.Collapsed;
                    this.WindowStyle = WindowStyle.None;
                    //this.Topmost = true;
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
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    //this.Topmost = false;
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

            window1 = new Window1();
            window1.Show();

            this.Closed += (s, e) => window1.Close();
        }
    }
}
