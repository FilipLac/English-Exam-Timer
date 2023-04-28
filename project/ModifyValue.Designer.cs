namespace English_Exam_Timer
{
    partial class ModifyValue
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
            numericUpDown1.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            numericUpDown1.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown1.Location = new Point(299, 70);
            numericUpDown1.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(120, 50);
            numericUpDown1.TabIndex = 0;
            numericUpDown1.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(189, 72);
            label1.Name = "label1";
            label1.Size = new Size(104, 45);
            label1.TabIndex = 1;
            label1.Text = "Value:";
            // 
            // bSet
            // 
            bSet.BackColor = Color.FromArgb(192, 255, 192);
            bSet.FlatStyle = FlatStyle.Flat;
            bSet.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
            bSet.Location = new Point(7, 145);
            bSet.Name = "bSet";
            bSet.Size = new Size(286, 78);
            bSet.TabIndex = 2;
            bSet.Text = "Set";
            bSet.UseVisualStyleBackColor = false;
            bSet.Click += ButtonSet_Click;
            // 
            // bCancel
            // 
            bCancel.BackColor = Color.FromArgb(255, 192, 192);
            bCancel.FlatStyle = FlatStyle.Flat;
            bCancel.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
            bCancel.Location = new Point(299, 145);
            bCancel.Name = "bCancel";
            bCancel.Size = new Size(283, 78);
            bCancel.TabIndex = 2;
            bCancel.Text = "Cancel";
            bCancel.UseVisualStyleBackColor = false;
            bCancel.Click += ButtonCancel_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(105, 28);
            label2.Name = "label2";
            label2.Size = new Size(381, 19);
            label2.TabIndex = 3;
            label2.Text = "Enter number that will be added at the end of previous form.";
            label2.Visible = false;
            // 
            // ModifyValue
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(594, 235);
            Controls.Add(label2);
            Controls.Add(bCancel);
            Controls.Add(bSet);
            Controls.Add(label1);
            Controls.Add(numericUpDown1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ModifyValue";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Modify or Add Value";
            Load += NudValueMA_Load;
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