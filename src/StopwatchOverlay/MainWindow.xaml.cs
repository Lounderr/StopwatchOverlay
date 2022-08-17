using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using System.Threading;
using System.Diagnostics.Metrics;
using StopwatchOverlay.Logic;

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
            StopwatchLogic stopwatchLogic = new StopwatchLogic(Counter);
            HideOnHoverLogic hideOnHoverLogic = new HideOnHoverLogic(AppWindow, BorderContainer);    
            ClickThroughLogic clickThroughLogic = new ClickThroughLogic();

        }
    }
}
