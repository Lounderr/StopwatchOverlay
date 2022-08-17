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
            this.dispatcherTimer = new DispatcherTimer();
            this.stopwatch = new Stopwatch();

            this.dispatcherTimer.Tick += new EventHandler(TickEventHandler);
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);

            this.dispatcherTimer.Start();

            this.TimeDisplayElement = TimeDisplayElement;
        }

        // UI element that displays the elapsed time
        public TextBlock TimeDisplayElement { get; set; }

        public void Start()
        {
            this.TimeDisplayElement.Foreground = Brushes.White;

            this.stopwatch.Start();
        }

        public void Stop()
        {
            if (this.stopwatch.IsRunning)
            {
                this.stopwatch.Stop();
            }
        }

        public void Reset()
        {
            this.stopwatch.Reset();
            this.TimeDisplayElement.Text = "00:00";
        }

        public bool IsRunning()
        {
            return this.stopwatch.IsRunning;
        }

        private void TickEventHandler(object? sender, EventArgs e)
        {
            if (this.stopwatch.IsRunning)
            {
                TimeSpan elapsedTime = stopwatch.Elapsed;
                this.TimeDisplayElement.Text = string.Format("{0:00}:{1:00}", elapsedTime.Hours, elapsedTime.Seconds);
            }
            else
            {
                Blink();
            }
        }

        private void Blink()
        {
            if (!this.blinkState)
            {
                this.TimeDisplayElement.Foreground = Brushes.OrangeRed;
                this.blinkState = true;

                return;
            }
            this.TimeDisplayElement.Foreground = Brushes.White;
            this.blinkState = false;
        }
    }
}