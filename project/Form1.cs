using Microsoft.VisualBasic.Logging;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace English_Exam_Timer
{
    public partial class Form1 : Form
    {
        //Version 0.9.0
        //Developed by: Filip Lacina

        //Inicializing private variables
        ModifyTimer modifyTimer = new ModifyTimer();
        private bool inicializationDone = false;

        //Public variables for timer
        public int[] Times;
        
        public bool InicializationDone
        {
            get { return inicializationDone; }
            private set { inicializationDone = value; }
        }

        //Private variables for timer (these will be modified only by timer)
        private int l = 0;
        private int remainingtime;

        public Form1()
        {
            InitializeComponent();
        }

        //Form1 load action with timer start
        private void Form1_Load(object sender, EventArgs e)
        {
            if (inicializationDone == false)
            {
                realTimeTimer.Start();
                try
                {
                    Times = new int[8];
                    for (int i = 0; i < 8; i++)
                    {
                        Times[i] = modifyTimer.initialTimeArray[i];
                    }
                }
                catch (NullReferenceException exceptionThrow)
                {
                    MessageBox.Show("Import of default values failed! [Error: Iidt1 - "+exceptionThrow+"]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                /*var message = string.Join(Environment.NewLine, Times);
                MessageBox.Show(message);*/
            }
        }

        private void Import()
        {
            Times = new int[modifyTimer.TimesFromPhases.Length];
            for (int i = 0; i < modifyTimer.TimesFromPhases.Length; i++)
            {
                Times[i] = modifyTimer.TimesFromPhases[i];
            }
        }

        //Flash method
        async void flash(string ColorMode)
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

        //This timer shows current day and time
        private void realTimeTimer_Tick(object sender, EventArgs e)
        {
            labelTimeDate.Text = Convert.ToString("Nyní je: " + DateTime.Now);
        }

        //The real timer
        private void startTimerButton_Click(object sender, EventArgs e)
        {
            remainingtime = Times[l];
            lLapTime.Text = Convert.ToString(l + 1);
            lapTimer.Start();

        }
        private void lapTimer_Tick(object sender, EventArgs e)
        {
            if (remainingtime < 0 || remainingtime == 0 && (l == modifyTimer.TimesFromPhases.Length) || (l == modifyTimer.initialTimeArray.Length))
            {
                lapTimer.Stop();
                lapTimer.Dispose();
                lLapTime.Text = "Timer Ended!";
                MessageBox.Show("End", "Timer finished", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }    
            if (remainingtime > 0)
            {
                remainingtime = remainingtime - 1;
            }
            else if (remainingtime == 0)
            {
                l++;
                remainingtime = Times[l];
                lLapTime.Text = Convert.ToString(l + 1);
            }

            if (remainingtime < 10)
                flash("red_blink");
            else if (remainingtime <= 15)
                flash("red");
            //else if (remainingtime <= 25)
            //    flash("yellow_blink");
            else if (remainingtime <= 30)
                flash("yellow");

            lRemainingTime.Text = Convert.ToString(remainingtime);
        }

        private void pauseTimerButton_Click(object sender, EventArgs e)
        {
            lapTimer.Stop();
        }

        private void stopAndResetTimerButton_Click(object sender, EventArgs e)
        {
            lapTimer.Stop();
            lapTimer.Dispose();
            lRemainingTime.Text = "000";
            this.BackColor = Color.WhiteSmoke;
        }

        //Second form (form2) with user modifications to time and phases
        private void bModifyTimer_Click(object sender, EventArgs e)
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

    }
}