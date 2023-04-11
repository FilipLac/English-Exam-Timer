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
    public partial class ModifyTimer : Form
    {
        public ModifyTimer()
        {
            InitializeComponent();
        }
        
        /*  1 = 0,5 min - 30s
            2 = 2,5 min - 150s
            3 = 1,5 min - 90s
            4 = 1,5 min - 90s
            5 = 1 min - 60s
            6 = 5 min - 300s
            7 = 3 min - 180s
            8 = 5 min - 300s */
        public int[] PhaseTimes = { 30, 150, 90, 90, 60, 300, 180, 300 };

        private void bSet_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        //This will open nudValueMA form which will allow user to add new value at end
        private void lbAddValue_Click(object sender, EventArgs e)
        {
            nudValueMA valuema = new nudValueMA(1);
            DialogResult timerset = valuema.ShowDialog();
            if (timerset == DialogResult.OK)
            {
                listBox1.SelectedItem = valuema.DecValue;
            }
            else
            {
                MessageBox.Show("Value was not added!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //This will open nudValueMA form which will allow user to modify selected value
        private void lbModifyValue_Click(object sender, EventArgs e)
        {
            nudValueMA valuema = new nudValueMA(2);
            valuema.DecValue = Convert.ToDecimal(listBox1.SelectedItem);
            DialogResult timerset = valuema.ShowDialog();
            if (timerset == DialogResult.OK)
            {
                listBox1.SelectedItem = valuema.DecValue;
            }
            else
            {
                MessageBox.Show("Value was not changed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Remove value from listbox -> it will be excluded when timer runs
        private void lbRemoveValue_Click(object sender, EventArgs e)
        {
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        //When this form loads, it will add all values to listbox for modification
        private void ModifyTimer_Load(object sender, EventArgs e)
        {
            foreach (int number in PhaseTimes)
            {
                listBox1.Items.Add(number);
            }
        }
    }
}
