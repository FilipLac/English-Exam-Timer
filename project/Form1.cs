using Microsoft.VisualBasic.Logging;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace English_Exam_Timer
{
    public partial class Form1 : Form
    {
        //Version 1.4.0
        //Developed by: Filip Lacina

        //Inicializing private variables
        ModifyTimer modifyTimer = new();
        private readonly int[] InitialTime = { 30, 150, 90, 90, 60, 300, 180, 300 };

        //Public variables for timer
        public int[] Times;

        //Private variables for timer (these will be modified only by timer)
        private int l = 0;
        private int remainingtime;
        private bool Paused = false;
        private bool Started = false;
        private bool WantLoop = true;


        public Form1()
        {
            InitializeComponent();
        }

        //Form1 load action with timer start
        private void Form1_Load(object sender, EventArgs e)
        {
            realTimeTimer.Start();
            try
            {
                Times = new int[8];
                for (int i = 0; i < 8; i++)
                {
                    Times[i] = InitialTime[i];
                }
            }
            catch (NullReferenceException exceptionThrow)
            {
                MessageBox.Show("Import of default values failed! [Error: It1 - " + exceptionThrow + "]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            /*var message = string.Join(Environment.NewLine, Times);
            MessageBox.Show(message);*/
        }

        //Import seconds and phases from second form (after user interaction)
        private void Import()
        {
            Times = new int[modifyTimer.TimesFromPhases.Length];
            for (int i = 0; i < modifyTimer.TimesFromPhases.Length; i++)
            {
                Times[i] = modifyTimer.TimesFromPhases[i];
            }
        }

        private void StartTimer()
        {
            if (Paused == false && Started == false)
            {
                Started = true;
                remainingtime = Times[l];
                lLapTime.Text = Convert.ToString(l + 1);
                ProgressBarTimer.Maximum = remainingtime;
                ProgressBarTimer.Value = remainingtime;
                lapTimer.Start();
            }
            else if (Paused == true && Started == false)
            {
                Paused = false;
                Started = true;
                lapTimer.Start();
            }
            else
            {

            }
        }

        private void ResetTimer()
        {
            lapTimer.Stop();
            lapTimer.Dispose();
            Started = false;
            Paused = false;
            l = 0;
            lLapTime.Text = Convert.ToString(l + 1);
            lRemainingTime.Text = "000s";
            LabelMinAndSec.Text = "00:00";
            ProgressBarTimer.Value = 0;
            this.BackColor = Color.WhiteSmoke;
        }

        //Flash method
        async void Flash(string ColorMode)
        {
            if (ColorMode == "yellow")
                this.BackColor = Color.Yellow;
            if (ColorMode == "yellow_blink")
            {
                this.BackColor = Color.WhiteSmoke;
                await Task.Delay(500);
                this.BackColor = Color.Yellow;
                await Task.Delay(500);
            }
            if (ColorMode == "red_blink")
            {
                this.BackColor = Color.Red;
                await Task.Delay(500);
                this.BackColor = Color.WhiteSmoke;
                await Task.Delay(500);
            }
            if (ColorMode == "red")
                this.BackColor = Color.Red;
        }

        //Calculating Minutes and seconds for second label
        private void CalculateMinutesAndSeconds(int secs)
        {
            int totalSeconds = secs;
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            if (minutes > 9 && seconds > 9)
                LabelMinAndSec.Text = minutes + " : " + seconds;
            else if (minutes > 9)
                LabelMinAndSec.Text = minutes + " : 0" + seconds;
            else if (seconds > 9)
                LabelMinAndSec.Text = "0" + minutes + " : " + seconds;
            else
                LabelMinAndSec.Text = "0" + minutes + " : 0" + seconds;

        }

        //This timer shows current day and time
        private void RealTimeTimer_Tick(object sender, EventArgs e)
        {
            labelTimeDate.Text = Convert.ToString("Nyní je: " + DateTime.Now);
        }

        //The real timer
        private void StartTimerButton_Click(object sender, EventArgs e)
        {
            StartTimer();
        }
        private void LapTimer_Tick(object sender, EventArgs e)
        {
            if (remainingtime > 0)
            {
                remainingtime--;
                try { ProgressBarTimer.Value--; }
                catch { }
            }
            else if (remainingtime == 0)
            {
                l++;
                if (l == Times.Length)
                {
                    if (WantLoop == false)
                    {
                        lapTimer.Stop();
                        lapTimer.Dispose();
                        MessageBox.Show("End", "Timer finished", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        ResetTimer();
                    }
                    else
                    {
                        lapTimer.Stop();
                        lapTimer.Dispose();
                        ResetTimer();
                        StartTimer();
                    }
                }
                else
                {
                    remainingtime = Times[l];
                    ProgressBarTimer.Maximum = remainingtime;
                    ProgressBarTimer.Value = remainingtime;
                    lLapTime.Text = Convert.ToString(l + 1);
                }
            }
            if (remainingtime == 0 && l == Times.Length)
                this.BackColor = Color.WhiteSmoke;
            else if (remainingtime < 10)
                Flash("red_blink");
            else if (remainingtime <= 15)
                Flash("red");
            //else if (remainingtime <= 25)
            //    flash("yellow_blink");
            else if (remainingtime <= 30)
                Flash("yellow");

            CalculateMinutesAndSeconds(remainingtime);
            lRemainingTime.Text = Convert.ToString(remainingtime + "s");
        }

        private void PauseTimerButton_Click(object sender, EventArgs e)
        {
            lapTimer.Stop();
            Paused = true;
            Started = false;
        }

        private void StopAndResetTimerButton_Click(object sender, EventArgs e)
        {
            ResetTimer();
        }

        //Second form (form2) with user modifications to time and phases
        private void ButtonModifyTimer_Click(object sender, EventArgs e)
        {
            modifyTimer = new ModifyTimer(this);
            DialogResult timerset = modifyTimer.ShowDialog();
            if (timerset == DialogResult.OK)
            {
                Import();
            }
            else
            {
                MessageBox.Show("Timer was not changed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ChbWantLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (ChbWantLoop.Checked == true)
            {
                WantLoop = true;
            }
            else
            {
                WantLoop = false;
            }
        }
    }
}