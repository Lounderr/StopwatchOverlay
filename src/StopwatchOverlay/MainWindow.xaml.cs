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
            timekeeper.Start();

            var hideOnHoverService = new HideOnHoverService(AppWindow, BorderContainer);
            hideOnHoverService.Start();

            var clickThroughWindowService = new ClickThroughWindowService();
            var hotkeyAssignmentService = new HotkeysService(timekeeper);
        }
    }
}
