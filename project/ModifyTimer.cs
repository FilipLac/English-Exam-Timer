using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace English_Exam_Timer
{
    public partial class ModifyTimer : Form
    {
        /*  1 = 0,5 min - 30s   2 = 2,5 min - 150s  3 = 1,5 min - 90s   4 = 1,5 min - 90s   5 = 1 min - 60s    6 = 5 min - 300s    7 = 3 min - 180s    8 = 5 min - 300s */
        public ModifyTimer()
        {
            InitializeComponent();
        }

        private Form1 form1;
        public ModifyTimer(Form1 ParentForm)
        {
            InitializeComponent();
            form1 = ParentForm;
        }

        public int[] TimesFromPhases { get; private set; }

        private void ButtonSet_Click(object sender, EventArgs e)
        {
            TimesFromPhases = new int[listBox1.Items.Count];
            for (int inx = 0; inx < listBox1.Items.Count; inx++)
            {
                TimesFromPhases[inx] = Convert.ToInt32(listBox1.Items[inx]);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        //This will open nudValueMA form which will allow user to add new value at end
        private void ListboxAddValue_Click(object sender, EventArgs e)
        {
            ModifyValue valuema = new(1);
            DialogResult timerset = valuema.ShowDialog();
            if (timerset == DialogResult.OK)
            {
                listBox1.Items.Add(valuema.DecValue);
                listBox1.Refresh();
            }
            else
            {
                MessageBox.Show("Value was not added!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //This will open nudValueMA form which will allow user to modify selected value
        private void ListboxModifyValue_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ModifyValue valuema = new(2)
                {
                    DecValue = Convert.ToDecimal(listBox1.SelectedItem)
                };
                DialogResult timerset = valuema.ShowDialog();
                if (timerset == DialogResult.OK)
                {
                    listBox1.Items[listBox1.SelectedIndex] = valuema.DecValue;
                    listBox1.Refresh();
                }
                else
                {
                    MessageBox.Show("Value was not changed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("You must first select some item!", "Warning item was not selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Remove value from listbox -> it will be excluded when timer runs
        private void LbRemoveValue_Click(object sender, EventArgs e)
        {
            RemoveValue();
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                RemoveValue();
            }
        }

        private void RemoveValue()
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            else
            {
                MessageBox.Show("You must first select some item!", "Warning item was not selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //When this form loads, it will add all values to listbox for modification
        private void ModifyTimer_Load(object sender, EventArgs e)
        {
            foreach (int number in form1.Times)
            {
                listBox1.Items.Add(number);
            }
        }
    }
}
