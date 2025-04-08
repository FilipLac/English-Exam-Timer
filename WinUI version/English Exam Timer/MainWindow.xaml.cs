using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure, and more about our project templates, see: http://aka.ms/winui-project-info.

namespace English_Exam_Timer
{
    public sealed partial class MainWindow : Window
    {
        public TimerViewModel ViewModel { get; } = new TimerViewModel();

        public MainWindow()
        {
            this.InitializeComponent();

            // Pøiøazení hlavního okna pro zmìnu pozadí
            TimerViewModel.MainWindow = this;

            // Pøihlášení k události zmìny vlastností ViewModelu
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            // Poèáteèní naplnìní UI
            UpdateUI();
        }

        private void StartTimerButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartTimer();
        }

        private void PauseTimerButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PauseTimer();
        }

        private void StopAndResetTimerButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetTimer();
        }

        private async void ButtonModifyTimer_Click(object sender, RoutedEventArgs e)
        {
            //ContentDialog dialog = new()
            //{
            //    Title = "Unimplemented dialog",
            //    Content = "This feature is not yet implemented.",
            //    CloseButtonText = "OK",
            //    XamlRoot = this.Content.XamlRoot
            //};
            //_ = dialog.ShowAsync();
            var dialog = new ModifyTimerDialog(ViewModel)
            {
                XamlRoot = this.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ViewModel.UpdateLapTimes(dialog.Lap1, dialog.Lap2, dialog.Lap3, dialog.Lap4);
                // Pøípadný reset pro timer po uložení ViewModel.ResetTimer();
            }
        }

        //private void ChbWantLoop_CheckedChanged(object sender, RoutedEventArgs e)
        //{
        //    ViewModel.SetLoop(LoopTS.IsOn == true);
        //}

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Zmìny v UI podle ViewModelu
            if (e.PropertyName == nameof(ViewModel.DisplayTime) ||
                e.PropertyName == nameof(ViewModel.LapNumber) ||
                e.PropertyName == nameof(ViewModel.RemainingSeconds))
            {
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            TimerMain.Text = ViewModel.DisplayTime;
            lap.Text = ViewModel.LapNumber;
            lRemainingTime.Text = ViewModel.RemainingSeconds;
        }
    }
    public partial class TimerViewModel : INotifyPropertyChanged
    {
        public static Window MainWindow { get; set; } = new Window();
        public event PropertyChangedEventHandler? PropertyChanged;
        private static readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private int l = 0;
        private int remainingTime;
        private bool paused = false;
        private bool started = false;
        private bool wantLoop = true;
        private readonly int[] InitialTime = [35, 150, 90, 90, 60, 300, 180, 300];

        public int Lap1Seconds { get; private set; } = 120;
        public int Lap2Seconds { get; private set; } = 180;
        public int Lap3Seconds { get; private set; } = 150;
        public int Lap4Seconds { get; private set; } = 240;

        public void UpdateLapTimes(int lap1, int lap2, int lap3, int lap4)
        {
            Lap1Seconds = lap1;
            Lap2Seconds = lap2;
            Lap3Seconds = lap3;
            Lap4Seconds = lap4;
        }

        public int[] Times { get; private set; }
        public string DisplayTime { get; private set; } = "00:00";
        public string LapNumber { get; private set; } = "1";
        public string RemainingSeconds { get; private set; } = "000s";

        private readonly DispatcherTimer lapTimer;

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

        private static async Task Flash(string colorMode)
        {
            dispatcherQueue.TryEnqueue(() =>
            {
                if (colorMode == "yellow") MainWindow.Content = new Grid { Background = new SolidColorBrush(Microsoft.UI.Colors.Yellow) };
                if (colorMode == "red") MainWindow.Content = new Grid { Background = new SolidColorBrush(Microsoft.UI.Colors.Red) };
            });

            await Task.Delay(500);

            dispatcherQueue.TryEnqueue(() =>
            {
                MainWindow.Content = new Grid { Background = new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke) };
            });
        }

        private async void LapTimer_Tick(object? sender, object? e)
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
                        await ShowMessageDialog("End", "Timer finished");
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

            if (remainingTime < 10) await Flash("red");
            else if (remainingTime <= 30) await Flash("yellow");

            UpdateUI();
        }
        public void PauseTimer()
        {
            if (started)
            {
                paused = true;
                started = false;
                lapTimer.Stop();
            }
        }

        public void SetLoop(bool loop)
        {
            wantLoop = loop;
        }


        private static async Task ShowMessageDialog(string title, string content)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = MainWindow.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private void UpdateUI()
        {
            LapNumber = (l + 1).ToString();
            RemainingSeconds = $"{remainingTime}s";
            DisplayTime = $"{remainingTime / 60:D2}:{remainingTime % 60:D2}";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LapNumber)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RemainingSeconds)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayTime)));
        }
    }
}