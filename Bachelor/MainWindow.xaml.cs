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
using Bachelor.AI;

namespace Bachelor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public Game? Game { get; private set; }
        private bool _manualMode;
        private QLearningAgent _qLearningAgent;

        private DispatcherTimer _gameTickTimer;

        public MainWindow()
        {
            InitializeComponent();
            _gameTickTimer = new DispatcherTimer();
            _manualMode = true;
            _qLearningAgent = new QLearningAgent();
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

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Game = new Game(SnakeCanvas, this);

            _gameTickTimer.Tick += (object? sender, EventArgs e) => Game.Tick();
            _gameTickTimer.Stop();
            _gameTickTimer.Interval = TimeSpan.FromMilliseconds(200);

            KeyDown += (object sender, KeyEventArgs e) =>
            {
                if (_manualMode)
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
                }
            };
        }

        private void StartSnake(object sender, RoutedEventArgs e)
        {
            if (Game != null)
            {
                Game.Start();
                _gameTickTimer.Start();
                _manualMode = true;
            }
        }

        private void StopSnake(object sender, RoutedEventArgs e)
        {
            StopSnake();
        }

        private void StartQLearningAgent(object sender, RoutedEventArgs e)
        {
            if (Game != null)
            {
                Game.Start(true, _qLearningAgent);
                _gameTickTimer.Start();
                _manualMode = false;
            }
        }

        private void TrainQLearningAgent(object sender, RoutedEventArgs e)
        {
            if (Game != null)
            {
                Game.TrainAgent(_qLearningAgent = new QLearningAgent());
            }
        }
    }
}
