using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

namespace IDC2018
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 
    class TeamColor
    {
        public string Name { get; }
        public SolidColorBrush Color { get; }

        public TeamColor(string name, Color color)
        {
            Name = name;
            Color = new SolidColorBrush(color);
        }
    }

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
                data = TimeSpan.FromMinutes(2) - data;
                timer_block.Text = data.ToString(@"mm':'ss");
            };

            var teams = new TeamColor[]
            {
                new TeamColor("Apricot", Color.FromRgb(241, 143, 49)),
                new TeamColor("Meron", Color.FromRgb(133, 193, 45)),
                new TeamColor("Pink", Color.FromRgb(227, 57, 142)),
                new TeamColor("Yellow", Color.FromRgb(232, 231, 60)),
                new TeamColor("Mint", Color.FromRgb(113, 196, 176)),
                new TeamColor("Green", Color.FromRgb(14, 111, 56)),
                new TeamColor("Purple", Color.FromRgb(136, 103, 170)),
                new TeamColor("Red", Color.FromRgb(180, 29, 39)),
                new TeamColor("Blue", Color.FromRgb(44, 83, 164)),
                new TeamColor("Navy", Color.FromRgb(31, 43, 99)),
                new TeamColor("Brown", Color.FromRgb(84, 52, 23))
            };


            ellipse_blue.ContextMenu = new ContextMenu();
            ellipse_red.ContextMenu = new ContextMenu();
            foreach (var team in teams)
            {
                var item_blue = new MenuItem() { Header = team.Name, CommandParameter = team, Background=team.Color};
                item_blue.Click += (s, e) =>
                {
                    var param = ((MenuItem)s).CommandParameter as TeamColor;
                    ellipse_blue.Fill = param.Color;
                    team_blue.Text = param.Name;
                };
                    ellipse_blue.ContextMenu.Items.Add(item_blue);

                var item_red = new MenuItem() { Header = team.Name, CommandParameter = team, Background = team.Color };
                item_red.Click += (s, e) =>
                {
                    var param = ((MenuItem)s).CommandParameter as TeamColor;
                    ellipse_red.Fill = param.Color;
                    team_red.Text = param.Name;
                };
                ellipse_red.ContextMenu.Items.Add(item_red);
            }
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
                timer_block.Text = "02:00";
            }
            else
            {
                start = DateTime.Now;
                timer.Start();
            }
        }

        private void scroll_MouseEnter(object sender, MouseEventArgs e) => ((ScrollBar)sender).Opacity = 1;
        private void scroll_MouseLeave(object sender, MouseEventArgs e) => ((ScrollBar)sender).Opacity = 0;

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ResizeMode = ResizeMode == ResizeMode.CanResizeWithGrip ? ResizeMode.CanResize : ResizeMode.CanResizeWithGrip;
        }
    }
}
