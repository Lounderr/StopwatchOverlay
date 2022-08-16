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

            MouseHook.Start();
            MouseHook.MouseAction += new EventHandler(OnHoverHide);

            // Mouse position while wintransparent 
            // why isnt mouseaction working when its a different method

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
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }
        public void OnHoverHide(object? sender, EventArgs e)
        {
            Point MousePos = GetMousePosition();

            Debug.WriteLine($"\n\n#\n\n{MousePos.X}-{MousePos.Y}\n\n#\n");

            Point TopLeftWindowPoint = new Point(AppWindow.Left, AppWindow.Top);
            Point BottomRightWindowPoint = new Point(AppWindow.Left + AppWindow.Width, AppWindow.Top + AppWindow.Height);

            //If mouse isn't over window
            if (MousePos.X >= TopLeftWindowPoint.X && MousePos.Y >= TopLeftWindowPoint.Y)
            {
                if (MousePos.X <= BottomRightWindowPoint.X && MousePos.Y <= BottomRightWindowPoint.Y)
                {
                    BorderContainer.Opacity = 0;
                    return;
                }
            }
            BorderContainer.Opacity = 1;
            //if (MousePos.X == -25 && MousePos.Y == -25)
            //{
            //    BorderContainer.Opacity = 1;
            //}
            //else
            //{
            //    BorderContainer.Opacity = 0.01;
            //}
        }

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

        public static class MouseHook
        {
            public static event EventHandler MouseAction = delegate { };

            public static void Start()
            {
                _hookID = SetHook(_proc);


            }
            public static void stop()
            {
                UnhookWindowsHookEx(_hookID);
            }

            private static LowLevelMouseProc _proc = HookCallback;
            private static IntPtr _hookID = IntPtr.Zero;

            private static IntPtr SetHook(LowLevelMouseProc proc)
            {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_MOUSE_LL, proc,
                      GetModuleHandle(curModule.ModuleName), 0);
                }
            }

            private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

            private static IntPtr HookCallback(
              int nCode, IntPtr wParam, IntPtr lParam)
            {
                if (nCode >= 0 && MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
                {
                    MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    MouseAction(null, new EventArgs());
                }
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }

            private const int WH_MOUSE_LL = 14;

            private enum MouseMessages
            {
                WM_LBUTTONDOWN = 0x0201,
                WM_LBUTTONUP = 0x0202,
                WM_MOUSEMOVE = 0x0200,
                WM_MOUSEWHEEL = 0x020A,
                WM_RBUTTONDOWN = 0x0204,
                WM_RBUTTONUP = 0x0205
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct POINT
            {
                public int x;
                public int y;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct MSLLHOOKSTRUCT
            {
                public POINT pt;
                public uint mouseData;
                public uint flags;
                public uint time;
                public IntPtr dwExtraInfo;
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr SetWindowsHookEx(int idHook,
              LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool UnhookWindowsHookEx(IntPtr hhk);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
              IntPtr wParam, IntPtr lParam);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr GetModuleHandle(string lpModuleName);


        }
    }
}
