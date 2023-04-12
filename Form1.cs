using Microsoft.VisualBasic.Logging;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace English_Exam_Timer
{
    public partial class Form1 : Form
    {
        //Version 0.7.0
        //Developed by: Filip Lacina

        //Inicializing private variables
        private int currentLap;
        private int currentPhase = 1;
        private int remainingtime;
        private int lapCounts;
        private int showRemainingTime;
        private int[] Times;
        Boolean inicializationDone = false;
        ModifyTimer modifyTimer = new ModifyTimer();

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
                    lapCounts = modifyTimer.initialTimeArray.Length;
                    Times = new int[modifyTimer.initialTimeArray.Length];
                    for (int i = 0; i < modifyTimer.initialTimeArray.Length; i++)
                    {
                        Times[i] = modifyTimer.initialTimeArray[i];
                    }
                    inicializationDone = true;
                }
                catch (NullReferenceException exceptionThrow)
                {
                    MessageBox.Show("Import of default values failed! [Error: Iidt1]","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                /*var message = string.Join(Environment.NewLine, Times);
                MessageBox.Show(message);*/
            }
        }

        //Flash method
        async void flash(string ColorMode)
        {
            if (ColorMode == "yellow")
            {
                for (int a = 0; a < 16; a++)
                {
                    this.BackColor = Color.Yellow;
                    await Task.Delay(1000);
                    this.BackColor = Color.WhiteSmoke;
                    await Task.Delay(1000);
                }
            }
            if (ColorMode == "red")
            {
                for (int a = 0; a < 16; a++)
                {
                    this.BackColor = Color.Red;
                    await Task.Delay(100);
                    this.BackColor = Color.WhiteSmoke;
                    await Task.Delay(100);
                }
            }
        }
        private void BackgroundToRed_Change(object sender, EventArgs e)
        {
            flash("red");
        }

        //This timer shows current day and time
        private void realTimeTimer_Tick(object sender, EventArgs e)
        {
            labelTimeDate.Text = Convert.ToString("Nyní je: " + DateTime.Now);
        }

        //The real timer
        private void startTimerButton_Click(object sender, EventArgs e)
        {
            lapTimer.Start();

        }
        private void lapTimer_Tick(object sender, EventArgs e)
        {
            for (int l = 1; l < lapCounts; l++)
            {
                remainingtime = Times[l];
                for (int s = 0; s <= remainingtime; s++)
                {
                    label1.Text = Convert.ToString(showRemainingTime = remainingtime - s);
                }
            }
        }

        private void pauseTimerButton_Click(object sender, EventArgs e)
        {
            lapTimer.Stop();
        }

        private void stopAndResetTimerButton_Click(object sender, EventArgs e)
        {
            lapTimer.Stop();
            lapTimer.Dispose();
        }

        //Second form (form2) with user modifications to time and phases
        private void bModifyTimer_Click(object sender, EventArgs e)
        {
            modifyTimer = new ModifyTimer();
            DialogResult timerset = modifyTimer.ShowDialog();
            if (timerset == DialogResult.OK)
            {
                Times = new int[modifyTimer.TimesFromPhases.Length];
                for (int i = 0; i < modifyTimer.TimesFromPhases.Length; i++)
                {
                    Times[i] = modifyTimer.TimesFromPhases[i];
                }
            }
            else
            {
                MessageBox.Show("Timer was not changed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

    }
}