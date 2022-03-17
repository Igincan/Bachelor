using System.Windows;
using System.Diagnostics;

using MahApps.Metro.Controls;
using System.Runtime.InteropServices;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Bachelor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
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
            FirstRectangle.Visibility = Visibility.Visible;
        }

        private void StopSnake(object sender, RoutedEventArgs e)
        {
            FirstRectangle.Visibility = Visibility.Hidden;
        }
    }
}
