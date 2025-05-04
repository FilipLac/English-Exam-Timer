using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
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
using System.Text.Json;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure, and more about our project templates, see: http://aka.ms/winui-project-info.

namespace English_Exam_Timer
{
    public sealed partial class MainWindow : Window
    {
        public TimerViewModel ViewModel { get; } = new TimerViewModel();

        public MainWindow()
        {
            //this.InitializeComponent();

            //// P�i�azen� hlavn�ho okna pro zm�nu pozad�
            //TimerViewModel.MainWindow = this;

            //// P�ihl�en� k ud�losti zm�ny vlastnost� ViewModelu
            //ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            //// Po��te�n� napln�n� UI
            //UpdateUI();

            //ViewModel.SetBackgroundAction = (brush) =>
            //{
            //    DispatcherQueue.TryEnqueue(() =>
            //    {
            //        RootGrid.Background = brush;
            //    });
            //};
            this.InitializeComponent();
            TimerViewModel.MainWindow = this;
            _ = ViewModel.LoadPhasesAsync(); // <--- p�idat
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            UpdateUI();
            ViewModel.SetBackgroundAction = brush => DispatcherQueue.TryEnqueue(() => RootGrid.Background = brush);

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
        }

        private void LoopTS_IsOn(object sender, RoutedEventArgs e)
        {
            ViewModel.SetLoop(LoopTS.IsOn == true);
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Zm�ny v UI podle ViewModelu
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
        public List<PhaseTime> Phases { get; private set; } = new();
        private const string FileName = "phases.json";


        public static Window MainWindow { get; set; } = new Window();
        public event PropertyChangedEventHandler? PropertyChanged;
        private static readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        public Action<Brush>? SetBackgroundAction { get; set; }


        private int l = 0;
        private int remainingTime;
        private bool paused = false;
        private bool started = false;
        private bool wantLoop = true;
        private readonly int[] InitialTime = [30, 150, 90, 90, 60, 300, 180, 300];

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

            Times[0] = lap1;
            Times[1] = lap2;
            Times[2] = lap3;
            Times[3] = lap4;
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
            SetBackgroundAction?.Invoke(new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke));
            UpdateUI();
        }

        private CancellationTokenSource flashCts;
        private async Task StartFlashing(int secondsLeft)
        {
            flashCts?.Cancel();
            flashCts = new CancellationTokenSource();
            var token = flashCts.Token;

            try
            {
                if (secondsLeft <= 5)
                {
                    SetBackgroundAction?.Invoke(new SolidColorBrush(Microsoft.UI.Colors.Red));
                }
                else if (secondsLeft < 10)
                {
                    await FlashBlink(Microsoft.UI.Colors.Red, TimeSpan.FromMilliseconds(300), secondsLeft, token);
                }
                else if (secondsLeft <= 30 && secondsLeft >=10)
                {
                    await FlashBlink(Microsoft.UI.Colors.Yellow, TimeSpan.FromMilliseconds(500), secondsLeft, token);
                }
                else
                {
                        SetBackgroundAction?.Invoke(new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke));
                }
            }
            catch (TaskCanceledException)
            {
                // Ignoruj p�eru�en�
            }
        }
        private async Task FlashBlink(Color color, TimeSpan interval, int secondsLeft, CancellationToken token)
        {
            var brush = new SolidColorBrush(color);
            var offBrush = new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke);
            for (int i = 0; i < secondsLeft && !token.IsCancellationRequested; i++)
            {
                SetBackgroundAction?.Invoke(brush);
                await Task.Delay(interval, token);
                SetBackgroundAction?.Invoke(offBrush);
                await Task.Delay(interval, token);
            }
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
                        return;
                    }
                    else
                    {
                        ResetTimer();
                        StartTimer();
                        return;

                    }
                }
                else
                {
                    remainingTime = Times[l];
                }
            }
            await StartFlashing(remainingTime);
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
            SetBackgroundAction?.Invoke(new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke));
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

        public async Task LoadPhasesAsync()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(FileName);
                string json = await FileIO.ReadTextAsync(file);
                Phases = JsonSerializer.Deserialize<List<PhaseTime>>(json) ?? GetDefaultPhases();
            }
            catch
            {
                Phases = GetDefaultPhases();
            }

            ApplyPhasesToTimes();
        }

        private List<PhaseTime> GetDefaultPhases()
        {
            return new List<PhaseTime>
            {
                new("Instrukce", 30),
                new("�ten�", 150),
                new("Ot�zky 1", 90),
                new("Ot�zky 2", 90),
                new("Psan� pl�n", 60),
                new("Psan� 1", 300),
                new("Psan� 2", 180),
                new("Kontrola", 300),
            };
        }

        public void ApplyPhasesToTimes()
        {
            Times = Phases.Select(p => p.DurationSeconds).ToArray();
        }

        public async Task SavePhasesAsync()
        {
            string json = JsonSerializer.Serialize(Phases);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }

    }
}