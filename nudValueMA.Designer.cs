namespace English_Exam_Timer
{
    partial class nudValueMA
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            numericUpDown1 = new NumericUpDown();
            label1 = new Label();
            bSet = new Button();
            bCancel = new Button();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // numericUpDown1
            // 
            numericUpDown1.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown1.Location = new Point(146, 46);
            numericUpDown1.Maximum = new decimal(new int[] { -1530494976, 232830, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(120, 23);
            numericUpDown1.TabIndex = 0;
            numericUpDown1.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(102, 48);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 1;
            label1.Text = "Value:";
            // 
            // bSet
            // 
            bSet.BackColor = Color.FromArgb(192, 255, 192);
            bSet.FlatStyle = FlatStyle.Flat;
            bSet.Location = new Point(52, 92);
            bSet.Name = "bSet";
            bSet.Size = new Size(130, 43);
            bSet.TabIndex = 2;
            bSet.Text = "Set";
            bSet.UseVisualStyleBackColor = false;
            bSet.Click += bSet_Click;
            // 
            // bCancel
            // 
            bCancel.BackColor = Color.FromArgb(255, 192, 192);
            bCancel.FlatStyle = FlatStyle.Flat;
            bCancel.Location = new Point(188, 92);
            bCancel.Name = "bCancel";
            bCancel.Size = new Size(145, 43);
            bCancel.TabIndex = 2;
            bCancel.Text = "Cancel";
            bCancel.UseVisualStyleBackColor = false;
            bCancel.Click += bCancel_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 9);
            label2.Name = "label2";
            label2.Size = new Size(326, 15);
            label2.TabIndex = 3;
            label2.Text = "Enter number that will be added at the end of previous form.";
            label2.Visible = false;
            // 
            // nudValueMA
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(377, 147);
            Controls.Add(label2);
            Controls.Add(bCancel);
            Controls.Add(bSet);
            Controls.Add(label1);
            Controls.Add(numericUpDown1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "nudValueMA";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "nudValueMA";
            Load += nudValueMA_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown numericUpDown1;
        private Label label1;
        private Button bSet;
        private Button bCancel;
        private Label label2;
    }
}