namespace English_Exam_Timer
{
    partial class ModifyTimer
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
            components = new System.ComponentModel.Container();
            bSet = new Button();
            bCancel = new Button();
            listBox1 = new ListBox();
            listboxContentMenuStrip = new ContextMenuStrip(components);
            addToolStripMenuItem = new ToolStripMenuItem();
            modifyToolStripMenuItem = new ToolStripMenuItem();
            removeToolStripMenuItem = new ToolStripMenuItem();
            listboxContentMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // bSet
            // 
            bSet.BackColor = Color.FromArgb(192, 255, 192);
            bSet.FlatStyle = FlatStyle.Flat;
            bSet.Location = new Point(12, 247);
            bSet.Name = "bSet";
            bSet.Size = new Size(112, 37);
            bSet.TabIndex = 2;
            bSet.Text = "Set";
            bSet.UseVisualStyleBackColor = false;
            bSet.Click += bSet_Click;
            // 
            // bCancel
            // 
            bCancel.BackColor = Color.FromArgb(255, 192, 192);
            bCancel.FlatStyle = FlatStyle.Flat;
            bCancel.Location = new Point(130, 247);
            bCancel.Name = "bCancel";
            bCancel.Size = new Size(119, 37);
            bCancel.TabIndex = 2;
            bCancel.Text = "Cancel";
            bCancel.UseVisualStyleBackColor = false;
            bCancel.Click += bCancel_Click;
            // 
            // listBox1
            // 
            listBox1.ContextMenuStrip = listboxContentMenuStrip;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(237, 229);
            listBox1.TabIndex = 3;
            // 
            // listboxContentMenuStrip
            // 
            listboxContentMenuStrip.Items.AddRange(new ToolStripItem[] { addToolStripMenuItem, modifyToolStripMenuItem, removeToolStripMenuItem });
            listboxContentMenuStrip.Name = "listboxContentMenuStrip";
            listboxContentMenuStrip.Size = new Size(118, 70);
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new Size(117, 22);
            addToolStripMenuItem.Text = "Add";
            addToolStripMenuItem.Click += lbAddValue_Click;
            // 
            // modifyToolStripMenuItem
            // 
            modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            modifyToolStripMenuItem.Size = new Size(117, 22);
            modifyToolStripMenuItem.Text = "Modify";
            modifyToolStripMenuItem.Click += lbModifyValue_Click;
            // 
            // removeToolStripMenuItem
            // 
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new Size(117, 22);
            removeToolStripMenuItem.Text = "Remove";
            removeToolStripMenuItem.Click += lbRemoveValue_Click;
            // 
            // ModifyTimer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(264, 296);
            Controls.Add(listBox1);
            Controls.Add(bCancel);
            Controls.Add(bSet);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ModifyTimer";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ModifyTimer";
            Load += ModifyTimer_Load;
            listboxContentMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button bSet;
        private Button bCancel;
        private ListBox listBox1;
        private ContextMenuStrip listboxContentMenuStrip;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem modifyToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
    }
}