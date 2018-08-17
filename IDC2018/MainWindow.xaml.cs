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
        public Collection<EncoderDevice> VideoDevices { get; set; }

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
            window1.Show();

            this.Closed += (s, e) => window1.Close();

            ContextMenu contextMenu = new ContextMenu();
            rect.ContextMenu = contextMenu;

            rect.ContextMenu.Opened += (s, e) =>
            {
                VideoDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);

                rect.ContextMenu.Items.Clear();

                foreach (var device in VideoDevices)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = device.Name;
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
    }
}
