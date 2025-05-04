using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace English_Exam_Timer
{
    public sealed partial class PhaseInputDialog : ContentDialog
    {
        public PhaseTime? Phase { get; private set; }

        public PhaseInputDialog()
        {
            this.InitializeComponent();
        }

        public PhaseInputDialog(PhaseTime existing) : this()
        {
            NameBox.Text = existing.Name;
            DurationBox.Text = existing.DurationSeconds.ToString();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) ||
                !int.TryParse(DurationBox.Text, out int seconds) || seconds <= 0)
            {
                args.Cancel = true;
                return;
            }

            Phase = new PhaseTime(NameBox.Text.Trim(), seconds);
        }
    }
}
