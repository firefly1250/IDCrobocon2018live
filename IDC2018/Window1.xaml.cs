using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace IDC2018
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        DispatcherTimer timer;
        DateTime start;

        public Window1()
        {
            InitializeComponent();

            this.DataContext = this;

            this.MouseLeftButtonDown += (s, e) => this.DragMove();

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
