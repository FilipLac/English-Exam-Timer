using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace English_Exam_Timer
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly int[] InitialTime = { 30, 150, 90, 90, 60, 300, 180, 300 };
        private int l = 0;
        private int remainingTime;
        private bool paused = false;
        private bool started = false;
        private bool wantLoop = true;

        public int[] Times { get; private set; }
        public string DisplayTime { get; private set; } = "00:00";
        public string LapNumber { get; private set; } = "1";
        public string RemainingSeconds { get; private set; } = "000s";

        private DispatcherTimer lapTimer;

        public TimerViewModel()
        {
            Times = new int[8];
            InitialTime.CopyTo(Times, 0);

            lapTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            lapTimer.Tick += LapTimer_Tick;
        }

        public void StartTimer()
        {
            if (!paused && !started)
            {
                started = true;
                remainingTime = Times[l];
                UpdateUI();
                lapTimer.Start();
            }
            else if (paused && !started)
            {
                paused = false;
                started = true;
                lapTimer.Start();
            }
        }

        public void ResetTimer()
        {
            lapTimer.Stop();
            started = false;
            paused = false;
            l = 0;
            remainingTime = 0;
            UpdateUI();
        }

        private async void Flash(string colorMode)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = Application.Current.MainWindow;
                if (colorMode == "yellow") window.Background = System.Windows.Media.Brushes.Yellow;
                if (colorMode == "red") window.Background = System.Windows.Media.Brushes.Red;
            });
            await Task.Delay(500);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.MainWindow.Background = System.Windows.Media.Brushes.WhiteSmoke;
            });
        }

        private void LapTimer_Tick(object sender, EventArgs e)
        {
            if (remainingTime > 0)
            {
                remainingTime--;
            }
            else
            {
                l++;
                if (l == Times.Length)
                {
                    if (!wantLoop)
                    {
                        lapTimer.Stop();
                        MessageBox.Show("End", "Timer finished", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        ResetTimer();
                    }
                    else
                    {
                        ResetTimer();
                        StartTimer();
                    }
                }
                else
                {
                    remainingTime = Times[l];
                }
            }

            if (remainingTime < 10) Flash("red");
            else if (remainingTime <= 30) Flash("yellow");

            UpdateUI();
        }

        private void UpdateUI()
        {
            LapNumber = (l + 1).ToString();
            RemainingSeconds = $"{remainingTime}s";
            DisplayTime = $"{remainingTime / 60:D2}:{remainingTime % 60:D2}";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LapNumber"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RemainingSeconds"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayTime"));
        }
    }
}
