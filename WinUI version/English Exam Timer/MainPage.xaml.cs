using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;
    using Microsoft.UI;
    using Microsoft.UI.Xaml.Controls.Primitives;
    using Microsoft.UI.Xaml.Navigation;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Windowing;
    using Windows.Foundation.Collections;


namespace English_Exam_Timer
{
    public sealed partial class MainPage : Page
    {
        public TimerViewModel ViewModel { get; } = new TimerViewModel();

        public MainPage()
        {
            this.InitializeComponent();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            UpdateUI();

            _ = ViewModel.LoadPhasesAsync();
            ViewModel.SetBackgroundAction = brush =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    FlashLayer.Background = brush;
                    var color = (brush as SolidColorBrush)?.Color ?? Colors.White;
                    var foreground = GetContrastingForeground(color);
                    TimerText.Foreground = new SolidColorBrush(foreground);
                    LoopTS.Foreground = new SolidColorBrush(foreground);
                    LoopTSHeader.Foreground = new SolidColorBrush(foreground);
                    if (LoopTS.Header is string headerText)
                        LoopTS.Header = new TextBlock { Text = headerText, Foreground = new SolidColorBrush(foreground) };
                    AdjustPanelBackground(color);
                });
            };
        }
        private void AdjustPanelBackground(Color backgroundColor)
        {
            byte adjust(byte c) => (byte)Math.Max(0, c - 30);
            Color darker = Color.FromArgb(255, adjust(backgroundColor.R), adjust(backgroundColor.G), adjust(backgroundColor.B));
            BottomPanel.Background = new SolidColorBrush(darker);
        }

        private Color GetContrastingForeground(Color bg)
        {
            double luminance = (0.299 * bg.R + 0.587 * bg.G + 0.114 * bg.B) / 255;
            return luminance > 0.5 ? Colors.Black : Colors.White;
        }

        private void StartTimerButton_Click(object sender, RoutedEventArgs e) => ViewModel.StartTimer();
        private void PauseTimerButton_Click(object sender, RoutedEventArgs e) => ViewModel.PauseTimer();
        private void StopAndResetTimerButton_Click(object sender, RoutedEventArgs e) => ViewModel.ResetTimer();
        private void LoopTS_IsOn(object sender, RoutedEventArgs e) => ViewModel.SetLoop(LoopTS.IsOn == true);

        private async void ButtonModifyTimer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ModifyTimerDialog(ViewModel) { XamlRoot = this.XamlRoot };

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
            UpdateTextDisplay();
            UpdateGauge();
            PlayPhaseTransitionAnimation();
        }

        private void UpdateTextDisplay()
        {
            //lap.Text = ViewModel.LapNumber;
            //lRemainingTime.Text = ViewModel.RemainingSeconds;
            TimerText.Text = ViewModel.DisplayTime;
        }
        private void UpdateGauge()
        {
            int total = ViewModel.Times.Length > ViewModel.CurrentPhaseIndex
                ? ViewModel.Times[ViewModel.CurrentPhaseIndex]
                : 1;

            double value = ViewModel.RemainingSecondsInt;
            double angle = 360 * (value / total);
            double radians = (angle - 90) * Math.PI / 180;
            double radius = 100;

            double x = 100 + radius * Math.Cos(radians);
            double y = 100 + radius * Math.Sin(radians);

            bool isLargeArc = angle > 180;

            var arcSegment = new ArcSegment
            {
                Point = new Point(x, y),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = isLargeArc
            };

            var figure = new PathFigure
            {
                StartPoint = new Point(100, 0),
                IsClosed = false
            };
            figure.Segments.Add(arcSegment);

            var geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            GaugeArc.Data = geometry;
        }

        private void PlayPhaseTransitionAnimation()
        {
            if (this.Resources.TryGetValue("PhaseTransitionStoryboard", out var storyboardObj) && storyboardObj is Storyboard storyboard)
            {
                storyboard.Begin();
            }
        }
    }
        public partial class TimerViewModel : INotifyPropertyChanged
        {
        public List<PhaseTime> Phases { get; private set; } = [];
        private const string FileName = "phases.json";

        public int RemainingSecondsInt => remainingTime;
        public int CurrentPhaseIndex => l;

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

        public int[] Times { get; private set; }
        private string _lapNumber = "1";
        public string LapNumber
        {
            get => _lapNumber;
            private set
            {
                if (_lapNumber != value)
                {
                    _lapNumber = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LapNumber)));
                }
            }
        }

        private string _remainingSeconds = "000s";
        public string RemainingSeconds
        {
            get => _remainingSeconds;
            private set
            {
                if (_remainingSeconds != value)
                {
                    _remainingSeconds = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RemainingSeconds)));
                }
            }
        }

        private string _displayTime = "00:00";
        public string DisplayTime
        {
            get => _displayTime;
            private set
            {
                if (_displayTime != value)
                {
                    _displayTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayTime)));
                }
            }
        }

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
        private SolidColorBrush _currentBrush = new(Microsoft.UI.Colors.WhiteSmoke);

        private async Task StartFlashing(int secondsLeft)
        {
            flashCts?.Cancel();
            flashCts = new CancellationTokenSource();

            try
            {
                if (secondsLeft <= 5)
                {
                    await SetSolidColor(Microsoft.UI.Colors.Red);
                }
                else if (secondsLeft < 10)
                {
                    await FlashColor(Microsoft.UI.Colors.Red, 500);
                }
                else if (secondsLeft <= 30)
                {
                    await FlashColor(Microsoft.UI.Colors.Yellow, 500);
                }
                else
                {
                    await ResetBackground();
                }
            }
            catch (TaskCanceledException) { }
        }

        private async Task FlashColor(Color color, int intervalMs)
        {
            var targetBrush = new SolidColorBrush(color);
            var originalBrush = new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke);

            while (!flashCts.IsCancellationRequested)
            {
                await EnqueueAsync(() =>
                {
                    _currentBrush = targetBrush;
                    SetBackgroundAction?.Invoke(targetBrush);
                });

                await Task.Delay(intervalMs, flashCts.Token);

                await EnqueueAsync(() =>
                {
                    _currentBrush = originalBrush;
                    SetBackgroundAction?.Invoke(originalBrush);
                });

                await Task.Delay(intervalMs, flashCts.Token);
            }
        }

        private async Task SetSolidColor(Color color)
        {
            await EnqueueAsync(() =>
            {
                _currentBrush = new SolidColorBrush(color);
                SetBackgroundAction?.Invoke(_currentBrush);
            });
        }

        private async Task ResetBackground()
        {
            await EnqueueAsync(() =>
            {
                _currentBrush = new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke);
                SetBackgroundAction?.Invoke(_currentBrush);
            });
        }

        // Vlastní implementace EnqueueAsync
        private Task EnqueueAsync(Action action)
        {
            var dispatcher = DispatcherQueue.GetForCurrentThread();

            if (dispatcher.HasThreadAccess)
            {
                action();
                return Task.CompletedTask;
            }

            var tcs = new TaskCompletionSource<object>();
            dispatcher.TryEnqueue(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(result: null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
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
            //LapNumber = (l + 1).ToString();
            //RemainingSeconds = $"{remainingTime}s";
            //DisplayTime = $"{remainingTime / 60:D2}:{remainingTime % 60:D2}";
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LapNumber)));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RemainingSeconds)));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayTime)));
            LapNumber = (l + 1).ToString();
            RemainingSeconds = $"{remainingTime}s";
            DisplayTime = $"{remainingTime / 60:D2}:{remainingTime % 60:D2}";
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

        private static List<PhaseTime> GetDefaultPhases()
        {
            return
            [
                new("Instrukce", 30),
                new("Ètení", 150),
                new("Otázky 1", 90),
                new("Otázky 2", 90),
                new("Psaní plán", 60),
                new("Psaní 1", 300),
                new("Psaní 2", 180),
                new("Kontrola", 300),
            ];
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