using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.UI;
using Windows.Storage;
//Offline
    using Microsoft.UI;
    using Microsoft.UI.Windowing;
    using Microsoft.UI.Xaml.Controls.Primitives;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Navigation;
    using Windows.Foundation;
    using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure, and more about our project templates, see: http://aka.ms/winui-project-info.

namespace English_Exam_Timer
{
    public sealed partial class MainWindow : Window
    {
        public TimerViewModel ViewModel { get; } = new TimerViewModel();

        public MainWindow()
        {
            this.InitializeComponent();

            //// Pøiøazení hlavního okna pro zmìnu pozadí
            TimerViewModel.MainWindow = this;

            //// Pøihlášení k události zmìny vlastností ViewModelu
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            //// Poèáteèní naplnìní UI
            UpdateUI();

            _ = ViewModel.LoadPhasesAsync();
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
            var dialog = new ModifyTimerDialog(ViewModel)
            {
                XamlRoot = this.Content.XamlRoot
            };

            // Pøidání fáze
            dialog.AddPhaseRequested += async (s, _) =>
            {
                // Zavøe hlavní dialog
                dialog.Hide();

                // Otevøe dialog pro zadání nové fáze
                var inputDialog = new PhaseInputDialog { XamlRoot = this.Content.XamlRoot };
                var result = await inputDialog.ShowAsync();

                if (result == ContentDialogResult.Primary && inputDialog.Phase != null)
                {
                    dialog.Phases.Add(inputDialog.Phase);
                }

                // Znovu otevøe hlavní dialog
                await dialog.ShowAsync();
            };

            // Editace fáze
            dialog.EditPhaseRequested += async (s, phase) =>
            {
                if (phase is PhaseTime selectedPhase)
                {
                    // Zavøe hlavní dialog
                    dialog.Hide();

                    // Otevøe dialog pro editaci vybrané fáze
                    var inputDialog = new PhaseInputDialog(selectedPhase) { XamlRoot = this.Content.XamlRoot };
                    var result = await inputDialog.ShowAsync();

                    if (result == ContentDialogResult.Primary && inputDialog.Phase != null)
                    {
                        int index = dialog.Phases.IndexOf(selectedPhase);
                        if (index >= 0)
                            dialog.Phases[index] = inputDialog.Phase;
                    }

                    // Znovu otevøe hlavní dialog
                    await dialog.ShowAsync();
                }
            };

            // otevøe hlavní dialog
            await dialog.ShowAsync();
        }


        private void LoopTS_IsOn(object sender, RoutedEventArgs e)
        {
            ViewModel.SetLoop(LoopTS.IsOn == true);
        }

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

        //public int Lap1Seconds { get; private set; } = 120;
        //public int Lap2Seconds { get; private set; } = 180;
        //public int Lap3Seconds { get; private set; } = 150;
        //public int Lap4Seconds { get; private set; } = 240;

        //public void UpdateLapTimes(int lap1, int lap2, int lap3, int lap4)
        //{
        //    Lap1Seconds = lap1;
        //    Lap2Seconds = lap2;
        //    Lap3Seconds = lap3;
        //    Lap4Seconds = lap4;

        //    Times[0] = lap1;
        //    Times[1] = lap2;
        //    Times[2] = lap3;
        //    Times[3] = lap4;
        //}


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
                // Ignoruj pøerušení
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
                new("Ètení", 150),
                new("Otázky 1", 90),
                new("Otázky 2", 90),
                new("Psaní plán", 60),
                new("Psaní 1", 300),
                new("Psaní 2", 180),
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