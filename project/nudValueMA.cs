using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace English_Exam_Timer
{
    public partial class nudValueMA : Form
    {
        public nudValueMA()
        {
            InitializeComponent();
        }

        public nudValueMA(int evoke)
        {
            InitializeComponent();
            if (evoke == 1)
            {
                label2.Visible = true;
                label2.Text = "Enter number that will be added at the end of previous form.";
            }
            else if (evoke == 2)
            {
                label2.Visible = true;
                label2.Text = "Enter number that will replace selected number from previous form.";
            }
            else
            {
                MessageBox.Show("Error occured: Envma" + evoke);
                Close();
            }
        }

        //just local variable from this form for changing/adding value to previous form
        private decimal nudvalue;

        //public variable for passing value in and out this form
        public decimal DecValue
        {
            get { return this.nudvalue; }
            set { this.nudvalue = value; }
        }

        //Load form and set value (only if user pressed modify in previous form)
        private void nudValueMA_Load(object sender, EventArgs e)
        {
            nudvalue = DecValue;
            if (DecValue != 0)
            {
                numericUpDown1.Value = nudvalue;
            }
        }

        //Changing value - user action
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            DecValue = numericUpDown1.Value;
        }

        //Buttons - user actions
        private void bSet_Click(object sender, EventArgs e)
        {
            DecValue = numericUpDown1.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
