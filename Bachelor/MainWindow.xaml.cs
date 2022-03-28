using System.Windows;
using System.Diagnostics;

using MahApps.Metro.Controls;
using System.Runtime.InteropServices;
using System.Windows.Shapes;
using System.Windows.Media;
using Bachelor.Snake;
using System.Windows.Threading;
using System;
using System.Windows.Input;

namespace Bachelor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public Game? Game { get; private set; }

        private DispatcherTimer _gameTickTimer;


        public MainWindow()
        {
            InitializeComponent();
            _gameTickTimer = new DispatcherTimer();
        }

        public void StopSnake()
        {
            if (Game != null)
            {
                Game.Stop();
                _gameTickTimer.Stop();
            }
        }

        private void LaunchGitHub(object sender, RoutedEventArgs e)
        {
            string url = "https://github.com/Igincan/Bachelor";
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        private void StartSnake(object sender, RoutedEventArgs e)
        {
            if (Game != null)
            {
                Game.Start();
                _gameTickTimer.Start();
                Focusable = false;
            }
        }

        private void StopSnake(object sender, RoutedEventArgs e)
        {
            StopSnake();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Game = new Game(SnakeCanvas, this);

            _gameTickTimer.Tick += (object? sender, EventArgs e) => Game.Tick();
            _gameTickTimer.Stop();
            _gameTickTimer.Interval = TimeSpan.FromMilliseconds(200);

            this.KeyDown += (object sender, KeyEventArgs e) =>
            {
                switch (e.Key)
                {
                    case Key.A:
                        Game.ChangeSnakeNextDirection(NextDirection.LEFT);
                        break;
                    case Key.D:
                        Game.ChangeSnakeNextDirection(NextDirection.RIGHT);
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
