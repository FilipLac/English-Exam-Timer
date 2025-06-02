using System;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
//offline
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.DataTransfer;

namespace English_Exam_Timer
{
    public sealed partial class ModifyTimerDialog : ContentDialog
    {
        public ObservableCollection<PhaseTime> Phases { get; set; } = new();

        private TimerViewModel _viewModel;
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
    }
}
