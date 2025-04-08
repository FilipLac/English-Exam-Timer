using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace English_Exam_Timer
{
    public sealed partial class ModifyTimerDialog : ContentDialog
    {
        public int Lap1 { get; private set; }
        public int Lap2 { get; private set; }
        public int Lap3 { get; private set; }
        public int Lap4 { get; private set; }

        public ModifyTimerDialog(TimerViewModel viewModel)
        {
            this.InitializeComponent();

            // Předvyplní aktuální hodnoty
            Lap1Box.Text = viewModel.Lap1Seconds.ToString();
            Lap2Box.Text = viewModel.Lap2Seconds.ToString();
            Lap3Box.Text = viewModel.Lap3Seconds.ToString();
            Lap4Box.Text = viewModel.Lap4Seconds.ToString();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Validace a převod na čísla
            if (int.TryParse(Lap1Box.Text, out int lap1) &&
                int.TryParse(Lap2Box.Text, out int lap2) &&
                int.TryParse(Lap3Box.Text, out int lap3) &&
                int.TryParse(Lap4Box.Text, out int lap4))
            {
                Lap1 = lap1;
                Lap2 = lap2;
                Lap3 = lap3;
                Lap4 = lap4;
            }
            else
            {
                args.Cancel = true; // Zůstane otevřené, pokud nejsou čísla validní
            }
        }
    }
}
