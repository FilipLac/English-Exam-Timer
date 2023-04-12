using Microsoft.VisualBasic.Logging;
using System.Windows.Forms;

namespace English_Exam_Timer
{
    public partial class Form1 : Form
    {
        //Version 0.5.1
        //Developed by: Filip Lacina

        //Inicializing private variables
        private int currentLap;
        private int currentPhase = 1;
        private int remainingtime;
        private int lapCounts;
        private int showRemainingTime;
        private int[] Times;
        ModifyTimer modifyTimer = new ModifyTimer();

        public Form1()
        {
            InitializeComponent();
            modifyTimer = new ModifyTimer();
            try
            {
                lapCounts = modifyTimer.initialTimeArray.Length;
                Times = new int[modifyTimer.initialTimeArray.Length];
                for (int i = 0; i < modifyTimer.initialTimeArray.Length; i++)
                {
                    Times[i] = modifyTimer.initialTimeArray[i];
                }
            }
            catch(NullReferenceException e)
            {
                //Log = "Importing values failed";
            }
            /*var message = string.Join(Environment.NewLine, Times);
            MessageBox.Show(message);*/
        }

        //Form1 load action with timer start
        private void Form1_Load(object sender, EventArgs e)
        {
            realTimeTimer.Start();
        }

        //This timer shows current day and time
        private void realTimeTimer_Tick(object sender, EventArgs e)
        {
            labelTimeDate.Text = Convert.ToString("Nyní je: " + DateTime.Now);
        }

        private void startTimerButton_Click(object sender, EventArgs e)
        {
            lapTimer.Start();

        }
        private void lapTimer_Tick(object sender, EventArgs e)
        {
            for (int l = 1; l<lapCounts;  l++)
            {
                remainingtime = Times[l];
                for (int s = 0;  s<= remainingtime; s++)
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
                /*var message = string.Join(Environment.NewLine, Times);
                MessageBox.Show(message);*/
            }
            else
            {
                MessageBox.Show("Timer was not changed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

    }
}