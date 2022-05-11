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
using System.Windows.Controls;
using System.Collections.Generic;

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
        private QLearningSimpleEnviromentAgent _qLearningSimpleEnviromentAgent;

        private DispatcherTimer _gameTickTimer;
        private List<Button> _startButtons;
        private List<Button> _stopButtons;

        public MainWindow()
        {
            InitializeComponent();
            _gameTickTimer = new DispatcherTimer();
            _manualMode = true;
            _qLearningAgent = new QLearningAgent();
            _qLearningSimpleEnviromentAgent = new QLearningSimpleEnviromentAgent();
            QLearningProgressBar.Minimum = 0;
            QLearningProgressBar.Maximum = 100;
            if (_qLearningAgent.LoadTable())
            {
                QLearningProgressBar.Value = 100;
            }
            if (_qLearningSimpleEnviromentAgent.LoadTable())
            {
                QLearningSimpleEnviromentProgressBar.Value = 100;
            }
            _startButtons = new List<Button> { StartSnakeButton, StartQLearningAgentButton, StartQLearningSimpleEnviromentAgentButton };
            _stopButtons = new List<Button> { StopSnakeButton, StopQLearningAgentButton, StopQLearningSimpleEnviromentAgentButton };
        }

        public void StopSnake()
        {
            if (Game != null)
            {
                Game.Stop();
                _gameTickTimer.Stop();
                _startButtons.ForEach(button => button.IsEnabled = true);
                _stopButtons.ForEach(button => button.IsEnabled = false);
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
                _startButtons.ForEach(button => button.IsEnabled = false);
                _stopButtons.ForEach(button => button.IsEnabled = true);
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
                _startButtons.ForEach(button => button.IsEnabled = false);
                _stopButtons.ForEach(button => button.IsEnabled = true);
            }
        }

        private void TrainQLearningAgent(object sender, RoutedEventArgs e)
        {
            if (Game != null)
            {
                if (!int.TryParse(QLearningGenerationCount.Text, out int count))
                {
                    count = 10000;
                    QLearningGenerationCount.Text = count.ToString();
                }
                _qLearningAgent = new QLearningAgent();
                Game.TrainAgent(_qLearningAgent, count, QLearningProgressBar);
            }
        }

        private void TrainQLearningSimpleEnviromentAgent(object sender, RoutedEventArgs e)
        {
            if (Game != null)
            {
                if (!int.TryParse(QLearningSimpleEnviromentGenerationCount.Text, out int count))
                {
                    count = 10000;
                    QLearningSimpleEnviromentGenerationCount.Text = count.ToString();
                }
                _qLearningSimpleEnviromentAgent = new QLearningSimpleEnviromentAgent();
                Game.TrainAgent(_qLearningSimpleEnviromentAgent, count, QLearningSimpleEnviromentProgressBar);
            }
        }

        private void StartQLearningSimpleEnviromentAgent(object sender, RoutedEventArgs e)
        {
            if (Game != null)
            {
                Game.Start(true, _qLearningSimpleEnviromentAgent);
                _gameTickTimer.Start();
                _manualMode = false;
                _startButtons.ForEach(button => button.IsEnabled = false);
                _stopButtons.ForEach(button => button.IsEnabled = true);
            }
        }
    }
}
