using System.Windows.Forms;

namespace English_Exam_Timer
{
    public partial class Form1 : Form
    {
        //Version 0.5.0
        //Developed by: Filip Lacina

        public Form1()
        {
            InitializeComponent();
        }

        //Inicializing private variables
        private int currentLap;
        private int currentPhase;
        private int[] Times;

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
            currentPhase = 0;
            lapTimer.Start();
        }
        private void lapTimer_Tick(object sender, EventArgs e)
        {

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
            ModifyTimer modifyTimer = new ModifyTimer();
            DialogResult timerset = modifyTimer.ShowDialog();
            if (timerset == DialogResult.OK)
            {
                Times = new int[modifyTimer.TimesFromPhases.Length];
                for (int i = 0; i < modifyTimer.TimesFromPhases.Length; i++)
                {
                    Times[i] = modifyTimer.TimesFromPhases[i];
                }
                var message = string.Join(Environment.NewLine, Times);
                MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show("Timer was not changed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

    }
}