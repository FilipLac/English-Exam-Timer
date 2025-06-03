using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Windows.Foundation;
using Windows.Storage;
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

            //Inicialization of ToggleSwitches
            LoopTS.IsOn = ViewModel.LoopEnabled;
            FlashTS.IsOn = ViewModel.FlashEnabled;

            _ = ViewModel.LoadPhasesAsync();
            ViewModel.SetBackgroundAction = brush =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    FlashLayer.Background = brush;
                    var color = (brush as SolidColorBrush)?.Color ?? Colors.White;
                    var foregroundBrush = new SolidColorBrush(GetContrastingForeground(color));
                    TimerText.Foreground = foregroundBrush;
                    LoopTS.Foreground = foregroundBrush;
                    if (LoopTS.Header is string headerText)
                        LoopTS.Header = new TextBlock { Text = headerText, Foreground = foregroundBrush };
                    if (FlashTS != null)
                    {
                        FlashTS.Foreground = foregroundBrush;
                        if (FlashTS.Header is string flashHeader)
                            FlashTS.Header = new TextBlock { Text = flashHeader, Foreground = foregroundBrush };
                    }
                    AdjustPanelBackground(color);
                });
            };

            ViewModel.TimerFinished += async () =>{await ShowMessageDialog("Konec", "Èasovaè dokonèen.", this.XamlRoot);};
        }

        private void AdjustPanelBackground(Color backgroundColor)
        {
            static byte adjust(byte c) => (byte)Math.Max(0, c - 30);
            Color darker = Color.FromArgb(255, adjust(backgroundColor.R), adjust(backgroundColor.G), adjust(backgroundColor.B));
            BottomPanel.Background = new SolidColorBrush(darker);
        }

        private static Color GetContrastingForeground(Color bg)
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

            //Add Phase
            dialog.AddPhaseRequested += async (s, _) =>
            {
                //Hide main dialog
                dialog.Hide();

                //Opens dialog for user, so that they can add phases
                var inputDialog = new PhaseInputDialog { XamlRoot = this.Content.XamlRoot };
                var result = await inputDialog.ShowAsync();

                if (result == ContentDialogResult.Primary && inputDialog.Phase != null){dialog.Phases.Add(inputDialog.Phase);}

                //Show back the main dialog
                await dialog.ShowAsync();
            };

            //Phases edit
            dialog.EditPhaseRequested += async (s, phase) =>
            {
                if (phase is PhaseTime selectedPhase)
                {
                    //Hide main dialog
                    dialog.Hide();

                    //Opens dialog for user, so that they can edit phases
                    var inputDialog = new PhaseInputDialog(selectedPhase) { XamlRoot = this.Content.XamlRoot };
                    var result = await inputDialog.ShowAsync();

                    if (result == ContentDialogResult.Primary && inputDialog.Phase != null)
                    {
                        int index = dialog.Phases.IndexOf(selectedPhase);
                        if (index >= 0)
                            dialog.Phases[index] = inputDialog.Phase;
                    }

                    //Show back the main dialog
                    await dialog.ShowAsync();
                }
            };

            //Show main dialog
            await dialog.ShowAsync();
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            //Changes in UI based on ViewModel
            if (e.PropertyName == nameof(ViewModel.DisplayTime) || e.PropertyName == nameof(ViewModel.LapNumber))
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

        private void PlayPhaseTransitionAnimation(){if (this.Resources.TryGetValue("PhaseTransitionStoryboard", out var storyboardObj) && storyboardObj is Storyboard storyboard){storyboard.Begin();}}

        private static async Task ShowMessageDialog(string title, string content, XamlRoot xamlRoot)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = xamlRoot
            };
            await dialog.ShowAsync();
        }
    }

    public partial class TimerViewModel : INotifyPropertyChanged
    {
        //Declarations
        public event Action? TimerFinished;
        private int remainingTime;

        private int currentPhaseIndex = 0;
        
        public Action<Brush>? SetBackgroundAction { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        //Declaration for Pause and Stop
        private bool paused = false;
        private bool started = false;

        //Multirun
        private readonly System.Timers.Timer lapTimer;
        private readonly DispatcherQueue dispatcherQueue;
        private CancellationTokenSource? flashCts = new();

        //------------------Declaration for ToggleSwitches------------------//
        private bool enableFlash = true;
        private bool wantLoop = true;
        public bool FlashEnabled => enableFlash;
        public bool LoopEnabled => wantLoop;
        public void SetLoop(bool enabled) => wantLoop = enabled;
        public void SetFlash(bool enabled)
        {
            enableFlash = enabled;
            if (!enableFlash)
            {
                flashCts?.Cancel();
                flashCts = null;
                ResetBackground();
            }
        }
        //------------------------------------------------------------------//

        public int[] Times { get; private set; }
        public ObservableCollection<PhaseTime> Phases { get; } = [];
        public int RemainingSecondsInt => remainingTime;
        public int CurrentPhaseIndex => currentPhaseIndex;
        public string LapNumber { get; private set; } = "1";
        public string DisplayTime { get; private set; } = "00:00";
        private const string FileName = "phases.json";

        public TimerViewModel()
        {
            Times = new int[8];
            lapTimer = new System.Timers.Timer(1000); //1s
            lapTimer.Elapsed += LapTimer_Elapsed;
            lapTimer.AutoReset = true;
            dispatcherQueue = DispatcherQueue.GetForCurrentThread(); //setting dispatcherQueue
        }

        public void StartTimer()
        {
            if (!paused && !started)
            {
                started = true;
                remainingTime = Times[currentPhaseIndex];
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
            currentPhaseIndex = 0;
            remainingTime = 0;
            flashCts?.Cancel();
            flashCts = null;
            SetBackgroundAction?.Invoke(new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke));
            UpdateUI();
        }

        private void LapTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            //All UI changes must be done using dispatcherQueue
            dispatcherQueue.TryEnqueue(()=>
            {
                if (remainingTime > 0){remainingTime--;}
                else
                {
                    currentPhaseIndex++;
                    if (currentPhaseIndex == Times.Length)
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
                        remainingTime = Times[currentPhaseIndex];
                    }
                }
                if (enableFlash)
                    StartFlashing(remainingTime);
                else
                    ResetBackground();
                UpdateUI();
            });
        }

        private void UpdateUI()
        {
            LapNumber = (currentPhaseIndex + 1).ToString();
            DisplayTime = $"{remainingTime / 60:D2}:{remainingTime % 60:D2}";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LapNumber)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayTime)));
        }

        public void StartFlashing(int secondsLeft)
        {
            flashCts?.Cancel(); //WILL cancel all flashes, if they are still running
            flashCts = new CancellationTokenSource();

            if (secondsLeft <= 5)
                SetSolidColor(Microsoft.UI.Colors.Red);
            else if (secondsLeft < 10)
                _ = FlashColor(Microsoft.UI.Colors.Red, 500, flashCts.Token);
            else if (secondsLeft <= 30)
                _ = FlashColor(Microsoft.UI.Colors.Yellow, 500, flashCts.Token); //_= means that code won't wait untill it finishes, instead it goes on -> timer has priority over UI flash
            else
                ResetBackground();
        }

        private async Task FlashColor(Color color, int intervalMs, CancellationToken token)
        {
            var targetBrush = new SolidColorBrush(color);
            var originalBrush = new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke);
            if (flashCts == null)
                return;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    SetBackgroundAction?.Invoke(targetBrush);
                    await Task.Delay(intervalMs, token);
                    SetBackgroundAction?.Invoke(originalBrush);
                    await Task.Delay(intervalMs, token);
                }
            }
            catch (TaskCanceledException) {/*Blikání bylo zrušeno, nic dalšího není potøeba dìlat*/}
        }

        private void SetSolidColor(Color color){SetBackgroundAction?.Invoke(new SolidColorBrush(color));}

        private void ResetBackground(){SetBackgroundAction?.Invoke(new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke));}

        public async Task LoadPhasesAsync()
        {
            List<PhaseTime> loaded;
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(FileName);
                string json = await FileIO.ReadTextAsync(file);
                loaded = JsonSerializer.Deserialize<List<PhaseTime>>(json) ?? GetDefaultPhases();
            }
            catch
            {
                loaded = GetDefaultPhases();
            }

            Phases.Clear();
            foreach (var phase in loaded)
                Phases.Add(phase);

            ApplyPhasesToTimes();
        }

        public async Task SavePhasesAsync()
        {
            // Converts ObservableCollection to List because of serialization
            string json = JsonSerializer.Serialize(Phases.ToList());
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }

        private static List<PhaseTime> GetDefaultPhases() =>
        [
            new("Instrukce", 30), new("Ètení", 150), new("Otázky 1", 90), new("Otázky 2", 90), new("Psaní plán", 60), new("Psaní 1", 300), new("Psaní 2", 180), new("Kontrola", 300)
        ];

        #pragma warning disable IDE0305 // Zjednodušit inicializaci kolekce
        public void ApplyPhasesToTimes() => Times = Phases.Select(p => p.DurationSeconds).ToArray();
        #pragma warning restore IDE0305 // Zjednodušit inicializaci kolekce
    }
}