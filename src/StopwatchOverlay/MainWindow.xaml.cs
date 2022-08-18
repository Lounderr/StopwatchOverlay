using System;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics.Metrics;
using StopwatchOverlay.Logic;
using static StopwatchOverlay.MainWindow;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Win32;

namespace StopwatchOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            var timekeeper = new TimekeepingService(Counter);

            var hideOnHoverService = new HideOnHoverService(AppWindow, BorderContainer);
            hideOnHoverService.Start();

            var clickThroughWindowService = new ClickThroughWindowService();
            var hotkeyAssignmentService = new HotkeysService(timekeeper, AppWindow);


            var screenWidth = System.Windows.SystemParameters.WorkArea.Width;
            var screenHeight = System.Windows.SystemParameters.WorkArea.Height;

            Point topRightPoint = new Point(screenWidth - this.AppWindow.Width - 25, 25);

            this.AppWindow.Left = topRightPoint.X;
            this.AppWindow.Top = topRightPoint.Y;
            Properties.Settings.Default.FirstLaunch = true;

            if (Properties.Settings.Default.FirstLaunch)
            {
                MessageBox.Show("\tShift + Win + F7    |    Start / Stop Time\n\n\tShift + Win + F8    |    Restart Time\n\n\tShift + Win + F9    |    Change Window Position", "Controls");
                Properties.Settings.Default.FirstLaunch = false;
                Properties.Settings.Default.Save();
            }
        }
    }
}
