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
            menuStrip1 = new MenuStrip();
            addToolStripMenuItem1 = new ToolStripMenuItem();
            modifyToolStripMenuItem1 = new ToolStripMenuItem();
            removeToolStripMenuItem1 = new ToolStripMenuItem();
            listboxContentMenuStrip.SuspendLayout();
            menuStrip1.SuspendLayout();
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
            bSet.Click += ButtonSet_Click;
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
            bCancel.Click += ButtonCancel_Click;
            // 
            // listBox1
            // 
            listBox1.ContextMenuStrip = listboxContentMenuStrip;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 27);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(237, 214);
            listBox1.TabIndex = 3;
            listBox1.KeyDown += listBox1_KeyDown;
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
            addToolStripMenuItem.Click += ListboxAddValue_Click;
            // 
            // modifyToolStripMenuItem
            // 
            modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            modifyToolStripMenuItem.Size = new Size(117, 22);
            modifyToolStripMenuItem.Text = "Modify";
            modifyToolStripMenuItem.Click += ListboxModifyValue_Click;
            // 
            // removeToolStripMenuItem
            // 
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new Size(117, 22);
            removeToolStripMenuItem.Text = "Remove";
            removeToolStripMenuItem.Click += LbRemoveValue_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { addToolStripMenuItem1, modifyToolStripMenuItem1, removeToolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(264, 24);
            menuStrip1.TabIndex = 5;
            menuStrip1.Text = "menuStrip1";
            // 
            // addToolStripMenuItem1
            // 
            addToolStripMenuItem1.Name = "addToolStripMenuItem1";
            addToolStripMenuItem1.Size = new Size(41, 20);
            addToolStripMenuItem1.Text = "Add";
            addToolStripMenuItem1.Click += ListboxAddValue_Click;
            // 
            // modifyToolStripMenuItem1
            // 
            modifyToolStripMenuItem1.Name = "modifyToolStripMenuItem1";
            modifyToolStripMenuItem1.Size = new Size(57, 20);
            modifyToolStripMenuItem1.Text = "Modify";
            modifyToolStripMenuItem1.Click += ListboxModifyValue_Click;
            // 
            // removeToolStripMenuItem1
            // 
            removeToolStripMenuItem1.Name = "removeToolStripMenuItem1";
            removeToolStripMenuItem1.Size = new Size(62, 20);
            removeToolStripMenuItem1.Text = "Remove";
            removeToolStripMenuItem1.Click += LbRemoveValue_Click;
            // 
            // ModifyTimer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(264, 296);
            Controls.Add(menuStrip1);
            Controls.Add(listBox1);
            Controls.Add(bCancel);
            Controls.Add(bSet);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ModifyTimer";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ModifyTimer";
            Load += ModifyTimer_Load;
            listboxContentMenuStrip.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button bSet;
        private Button bCancel;
        private ListBox listBox1;
        private ContextMenuStrip listboxContentMenuStrip;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem modifyToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem addToolStripMenuItem1;
        private ToolStripMenuItem modifyToolStripMenuItem1;
        private ToolStripMenuItem removeToolStripMenuItem1;
    }
}