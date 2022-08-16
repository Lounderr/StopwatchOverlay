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

namespace StopwatchOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        string currentTime = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dt_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            stopWatch.Start();
            dispatcherTimer.Start();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                TimeSpan ts = stopWatch.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}",
                ts.Hours, ts.Minutes);
                Counter.Text = currentTime;
            }
        }

        //private void startbtn_Click(object sender, RoutedEventArgs e)
        //{
        //    stopWatch.Start();
        //    dispatcherTimer.Start();
        //}

        //private void stopbtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (stopWatch.IsRunning)
        //    {
        //        stopWatch.Stop();
        //    }
        //    //elapsedtimeitem.Items.Add(currentTime);
        //}

        //private void resetbtn_Click(object sender, RoutedEventArgs e)
        //{
        //    stopWatch.Reset();
        //    Counter.Text = "00:00:00";
        //}

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Make entire window and everything in it "transparent" to the Mouse
            var windowHwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(windowHwnd);

            // Make the button "visible" to the Mouse
            //var buttonHwndSource = (HwndSource)HwndSource.FromVisual(btn);
            //var buttonHwnd = buttonHwndSource.Handle;
            //WindowsServices.SetWindowExNotTransparent(buttonHwnd);
        }

        public static class WindowsServices
        {
            const int WS_EX_TRANSPARENT = 0x00000020;
            const int GWL_EXSTYLE = (-20);

            [DllImport("user32.dll")]
            static extern int GetWindowLong(IntPtr hwnd, int index);

            [DllImport("user32.dll")]
            static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

            public static void SetWindowExTransparent(IntPtr hwnd)
            {
                var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
            }

            public static void SetWindowExNotTransparent(IntPtr hwnd)
            {
                var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle & ~WS_EX_TRANSPARENT);
            }
        }
    }
}
