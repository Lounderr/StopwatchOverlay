using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StopwatchOverlay.MainWindow;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;

namespace StopwatchOverlay.Logic
{
    public class HotkeysService
    {
        private TimekeepingService timekeeper;
        public HotkeysService(TimekeepingService timekeeper)
        {
            this.timekeeper = timekeeper;

            var StartStopHotkey = new DetectHotkeyService(Key.F7, KeyModifier.Shift | KeyModifier.Win, StartStop);

            var ResetHotkey = new DetectHotkeyService(Key.F8, KeyModifier.Shift | KeyModifier.Win, Reset);
        }

        private void StartStop(DetectHotkeyService hotKey)
        {
            if (timekeeper.IsRunning())
            {
                timekeeper.Stop();
                return;
            }

            timekeeper.Start();
        }

        private void Reset(DetectHotkeyService hotKey)
        {
            timekeeper.Reset();
        }
    }
}
