namespace English_Exam_Timer
{
    public partial class Form1 : Form
    {
        //Version 0.2.0
        //Developed by: Filip Lacina

        public Form1()
        {
            InitializeComponent();
        }

        //Inicializing variables
        int currentLap;
        int currentPhase;
        ModifyTimer modifyTimer = new ModifyTimer();

        //Form1 load action with timer start
        private void Form1_Load(object sender, EventArgs e)
        {
            realTimeTimer.Start();
        }

        //This timer shows current day and time
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = Convert.ToString("Nyní je: " + DateTime.Now);
        }

        private void startTimerButton_Click(object sender, EventArgs e)
        {
            currentPhase = 0;

        }

        private void pauseTimerButton_Click(object sender, EventArgs e)
        {

        }

        private void stopAndResetTimerButton_Click(object sender, EventArgs e)
        {

        }

        private void bModifyTimer_Click(object sender, EventArgs e)
        {
            DialogResult timerset = modifyTimer.ShowDialog();
            if (timerset == DialogResult.OK)
            {

            }
            else
            {
                MessageBox.Show("Timer was not changed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

    }
}