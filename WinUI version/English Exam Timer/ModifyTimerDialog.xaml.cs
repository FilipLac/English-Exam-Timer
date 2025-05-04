using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Microsoft.UI.Xaml.Input;

namespace English_Exam_Timer
{
    public sealed partial class ModifyTimerDialog : ContentDialog
    {
        private readonly SemaphoreSlim dialogSemaphore = new SemaphoreSlim(1, 1);

        public ObservableCollection<PhaseTime> Phases { get; set; } = new();

        private TimerViewModel _viewModel;

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

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new PhaseInputDialog
            {
                XamlRoot = this.XamlRoot
            };
            var result = await inputDialog.ShowAsync();
            if (result == ContentDialogResult.Primary && inputDialog.Phase != null)
            {
                Phases.Add(inputDialog.Phase);
            }
        }

        private async void PhasesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!await dialogSemaphore.WaitAsync(0)) return;

            try
            {
                if (e.ClickedItem is PhaseTime selected)
                {
                    var inputDialog = new PhaseInputDialog(selected)
                    {
                        XamlRoot = this.XamlRoot
                    };

                    var result = await inputDialog.ShowAsync();

                    if (result == ContentDialogResult.Primary && inputDialog.Phase != null)
                    {
                        int index = Phases.IndexOf(selected);
                        Phases[index] = inputDialog.Phase;
                    }
                }
            }
            finally
            {
                dialogSemaphore.Release();
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
                await new ContentDialog
                {
                    Title = "Chyba",
                    Content = "Musíte mít alespoň jednu fázi.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            _viewModel.Phases.Clear();
            foreach (var phase in Phases)
                _viewModel.Phases.Add(phase);

            _viewModel.ApplyPhasesToTimes();
            await _viewModel.SavePhasesAsync();
        }
    }
}
