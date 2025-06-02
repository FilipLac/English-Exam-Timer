
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Microsoft.UI;
//offline
    using System.Text.Json;
    using System.Linq;
    using Windows.UI;

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

            // inicializace stavù pøepínaèù
            LoopTS.IsOn = ViewModel.LoopEnabled;
            FlashTS.IsOn = ViewModel.FlashEnabled;

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
                    if (LoopTS.Header is string headerText)
                        LoopTS.Header = new TextBlock { Text = headerText, Foreground = new SolidColorBrush(foreground) };
                    if (FlashTS != null)
                    {
                        FlashTS.Foreground = new SolidColorBrush(foreground);
                        if (FlashTS.Header is string flashHeader)
                            FlashTS.Header = new TextBlock { Text = flashHeader, Foreground = new SolidColorBrush(foreground) };
                    }
                    AdjustPanelBackground(color);
                });
            };

            ViewModel.TimerFinished += async () =>
            {
                await ShowMessageDialog("Konec", "Èasovaè dokonèen."/*, this.XamlRoot*/);
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
        private void FlashTS_IsOn(object sender, RoutedEventArgs e) => ViewModel.SetFlash(FlashTS.IsOn == true);

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
                e.PropertyName == nameof(ViewModel.LapNumber))
            {
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            TimerText.Text = ViewModel.DisplayTime;
            UpdateGauge();
            PlayPhaseTransitionAnimation();
        }

        private void UpdateGauge()
        {
            int total = ViewModel.Times.Length > ViewModel.CurrentPhaseIndex ? ViewModel.Times[ViewModel.CurrentPhaseIndex] : 1;
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

            var figure = new PathFigure { StartPoint = new Point(100, 0), IsClosed = false };
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

        private static async Task ShowMessageDialog(string title, string content)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
            };
            await dialog.ShowAsync();
        }
    }

    public partial class TimerViewModel : INotifyPropertyChanged
    {
        //Declarations
        public event Action? TimerFinished;
        private int remainingTime;

        private int l = 0;
        
        public Action<Brush>? SetBackgroundAction { get; set; }


        private SolidColorBrush _currentBrush = new(Microsoft.UI.Colors.WhiteSmoke);

        public event PropertyChangedEventHandler? PropertyChanged;
        
        //Declaration for Pause and Stop
        private bool paused = false;
        private bool started = false;

        //Multirun
        private readonly DispatcherTimer lapTimer;
        private CancellationTokenSource flashCts;

        //------------------Declaration for ToggleSwitches------------------//
        private bool enableFlash = true;
        private bool wantLoop = true;
        public bool FlashEnabled => enableFlash;
        public bool LoopEnabled => wantLoop;
        public void SetFlash(bool enabled) => enableFlash = enabled;
        public void SetLoop(bool enabled) => wantLoop = enabled;
        //------------------------------------------------------------------//


        public int[] Times { get; private set; }
        public List<PhaseTime> Phases { get; private set; } = [];
        public int RemainingSecondsInt => remainingTime;
        public int CurrentPhaseIndex => l;
        public string LapNumber { get; private set; } = "1";
        public string DisplayTime { get; private set; } = "00:00";
        private const string FileName = "phases.json";

        public TimerViewModel()
        {
            Times = new int[8];
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

        public void PauseTimer()
        {
            if (started)
            {
                paused = true;
                started = false;
                lapTimer.Stop();
                flashCts?.Cancel();
                flashCts = null;
                SetBackgroundAction?.Invoke(new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke));
            }
        }

        public void ResetTimer()
        {
            lapTimer.Stop();
            started = false;
            paused = false;
            l = 0;
            remainingTime = 0;
            flashCts?.Cancel();
            flashCts = null;
            SetBackgroundAction?.Invoke(new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke));
            UpdateUI();
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
                        TimerFinished?.Invoke();
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
            if (enableFlash)
                await StartFlashing(remainingTime);
            else
                await ResetBackground();
            UpdateUI();
        }

        private void UpdateUI()
        {
            LapNumber = (l + 1).ToString();
            DisplayTime = $"{remainingTime / 60:D2}:{remainingTime % 60:D2}";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LapNumber)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayTime)));
        }

        private async Task StartFlashing(int secondsLeft)
        {
            flashCts?.Cancel();
            flashCts = new CancellationTokenSource();
            try
            {
                if (secondsLeft <= 5)
                    await SetSolidColor(Microsoft.UI.Colors.Red);
                else if (secondsLeft < 10)
                    await FlashColor(Microsoft.UI.Colors.Red, 500);
                else if (secondsLeft <= 30)
                    await FlashColor(Microsoft.UI.Colors.Yellow, 500);
                else
                    await ResetBackground();
            }
            catch (TaskCanceledException) { }
        }

        private async Task FlashColor(Color color, int intervalMs)
        {
            var targetBrush = new SolidColorBrush(color);
            var originalBrush = new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke);
            while (!flashCts.IsCancellationRequested)
            {
                SetBackgroundAction?.Invoke(targetBrush);
                await Task.Delay(intervalMs, flashCts.Token);
                SetBackgroundAction?.Invoke(originalBrush);
                await Task.Delay(intervalMs, flashCts.Token);
            }
        }

        private async Task SetSolidColor(Color color)
        {
            SetBackgroundAction?.Invoke(new SolidColorBrush(color));
            await Task.CompletedTask;
        }

        private async Task ResetBackground()
        {
            SetBackgroundAction?.Invoke(new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke));
            await Task.CompletedTask;
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

        public async Task SavePhasesAsync()
        {
            string json = JsonSerializer.Serialize(Phases);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }

        private static List<PhaseTime> GetDefaultPhases() =>
        [
            //new("Instrukce", 30), new("Ètení", 150), new("Otázky 1", 90), new("Otázky 2", 90),
            //new("Psaní plán", 60), new("Psaní 1", 300), new("Psaní 2", 180), new("Kontrola", 300)
            new("Debug_Phase1", 40)
        ];

        public void ApplyPhasesToTimes() => Times = Phases.Select(p => p.DurationSeconds).ToArray();
    }
}
