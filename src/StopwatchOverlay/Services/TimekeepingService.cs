using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace StopwatchOverlay.Logic
{
    public class TimekeepingService
    {
        private bool blinkState = false;
        private DispatcherTimer dispatcherTimer;
        private Stopwatch stopwatch;

        public TimekeepingService(TextBlock TimeDisplayElement)
        {
            dispatcherTimer = new DispatcherTimer();
            stopwatch = new Stopwatch();

            dispatcherTimer.Tick += new EventHandler(TickEventHandler);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);

            dispatcherTimer.Start();

            this.TimeDisplayElement = TimeDisplayElement;
        }

        // UI element that displays the elapsed time
        public TextBlock TimeDisplayElement { get; set; }

        public void Start()
        {
            TimeDisplayElement.Foreground = Brushes.White;

            stopwatch.Start();
        }

        public void Stop()
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
            }
        }

        public void Reset()
        {
            stopwatch.Reset();
            TimeDisplayElement.Text = "00:00";
        }

        public bool IsRunning()
        {
            return stopwatch.IsRunning;
        }

        private void TickEventHandler(object? sender, EventArgs e)
        {
            if (stopwatch.IsRunning)
            {
                TimeSpan elapsedTime = stopwatch.Elapsed;
                TimeDisplayElement.Text = string.Format("{0:00}:{1:00}", elapsedTime.Hours, elapsedTime.Seconds);
            }
            else
            {
                Blink();
            }
        }

        private void Blink()
        {
            if (!blinkState)
            {
                TimeDisplayElement.Foreground = Brushes.OrangeRed;
                blinkState = true;

                return;
            }
            TimeDisplayElement.Foreground = Brushes.White;
            blinkState = false;
        }
    }
}