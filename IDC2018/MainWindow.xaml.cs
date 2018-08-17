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
    
    public partial class MainWindow : Window
    {
        Window1 window1;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            
            window1 = new Window1();
            window1.Closed += (s, e) => this.Close();
            window1.Show();

            this.Closed += (s, e) => window1.Close();

            rect.ContextMenu = new ContextMenu();

            rect.ContextMenu.Opened += (s, e) =>
            {
                var VideoDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);

                rect.ContextMenu.Items.Clear();

                foreach (var device in VideoDevices)
                {
                    var menuItem = new MenuItem
                    {
                        Header = device.Name
                    };
                    menuItem.Click += (s1, e1) =>
                    {
                        WebcamViewer.VideoDevice = device;
                        try
                        {
                            WebcamViewer.StartPreview();
                        }
                        catch (Microsoft.Expression.Encoder.SystemErrorException ex)
                        {
                            MessageBox.Show("Device is in use by another application");
                        }
                    };
                    rect.ContextMenu.Items.Add(menuItem);
                }
            };
        }

        bool waiting = false;
        private async void WebcamViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (waiting) return;
            waiting = true;
            WebcamViewer.StopPreview();
            await Task.Delay(100);
            WebcamViewer.StartPreview();
            waiting = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F11)
            {
                if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
                else WindowState = WindowState.Maximized;
            }
        }
    }
}
