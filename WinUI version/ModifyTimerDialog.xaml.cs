using System;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Text.Json;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRT.Interop; // nutné pro HWND
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace English_Exam_Timer
{
    public sealed partial class ModifyTimerDialog : ContentDialog
    {
        public ObservableCollection<PhaseTime> Phases { get; set; } = [];

        private readonly TimerViewModel _viewModel;
        public event EventHandler<PhaseTime>? EditPhaseRequested;
        public event EventHandler? AddPhaseRequested;

        public ModifyTimerDialog(TimerViewModel viewModel)
        {
            this.InitializeComponent();
            _viewModel = viewModel;

            foreach (var phase in viewModel.Phases)
            {
                Phases.Add(new PhaseTime(phase.Name, phase.DurationSeconds));
            }

            PhasesListView.ItemsSource = Phases;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddPhaseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void PhasesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is PhaseTime selected)
            {
                EditPhaseRequested?.Invoke(this, selected);
            }
        }

        private void Phase_ContextRequested(UIElement sender, ContextRequestedEventArgs args)
        {
            if (sender is FrameworkElement fe && fe.DataContext is PhaseTime toDelete)
            {
                var menu = new MenuFlyout();
                var deleteItem = new MenuFlyoutItem { Text = "Smazat" };
                deleteItem.Click += (s, e) => Phases.Remove(toDelete);
                menu.Items.Add(deleteItem);
                menu.ShowAt(fe);
            }
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (Phases.Count == 0)
            {
                args.Cancel = true;
                ErrorTextBlock.Text = "Musíte mít alespoň jednu fázi.";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }

            ErrorTextBlock.Visibility = Visibility.Collapsed;

            _viewModel.Phases.Clear();
            foreach (var phase in Phases)
                _viewModel.Phases.Add(phase);

            _viewModel.ApplyPhasesToTimes();
            await _viewModel.SavePhasesAsync();
        }
    


    private async void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".json");
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // Získání HWND pro WinUI3 dialog
            var hwnd = WindowNative.GetWindowHandle(App.MainAppWindow);
            InitializeWithWindow.Initialize(picker, hwnd);

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                try
                {
                    string json = await FileIO.ReadTextAsync(file);
                    var importedPhases = JsonSerializer.Deserialize<List<PhaseTime>>(json);
                    if (importedPhases != null)
                    {
                        Phases.Clear();
                        foreach (var phase in importedPhases)
                            Phases.Add(phase);
                    }
                }
                catch (Exception ex) 
                {
                    ImportErrorTextBlock.Text = "Chyba importu: " + ex.Message;
                    ImportErrorTextBlock.Visibility = Visibility.Visible;
                }
            }
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
                #pragma warning disable IDE0028 // Zjednodušit inicializaci kolekce
            picker.FileTypeChoices.Add("JSON", new List<string>() { ".json" });
                #pragma warning restore IDE0028 // Zjednodušit inicializaci kolekce
            picker.SuggestedFileName = "phases";

            var hwnd = WindowNative.GetWindowHandle(App.MainAppWindow);
            InitializeWithWindow.Initialize(picker, hwnd);

            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                try
                {
                    string json = JsonSerializer.Serialize(Phases);
                    await FileIO.WriteTextAsync(file, json);
                }
                catch
                {
                    // Chybu můžeš ošetřit např. zobrazením dialogu
                }
            }
        }
    }
}
