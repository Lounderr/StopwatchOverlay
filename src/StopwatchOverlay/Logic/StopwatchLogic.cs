using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;

namespace StopwatchOverlay.Logic
{ 
    internal class StopwatchLogic
    {
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private Stopwatch stopWatch = new Stopwatch();

        // UI element that displays the elapsed time
        public TextBlock ElapsedTimeDisplay;

        public StopwatchLogic(TextBlock elapsedTimeDisplay)
        {
            dispatcherTimer.Tick += new EventHandler(TickEventHandler);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);

            this.ElapsedTimeDisplay = elapsedTimeDisplay;

            dispatcherTimer.Start();
            stopWatch.Start();
        }

        void TickEventHandler(object? sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                TimeSpan ts = stopWatch.Elapsed;
                ElapsedTimeDisplay.Text = string.Format("{0:00}:{1:00}", ts.Hours, ts.Seconds);
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
    }
}
