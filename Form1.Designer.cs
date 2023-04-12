namespace English_Exam_Timer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            realTimeTimer = new System.Windows.Forms.Timer(components);
            labelTimeDate = new Label();
            startTimerButton = new Button();
            pauseTimerButton = new Button();
            stopAndResetTimerButton = new Button();
            bModifyTimer = new Button();
            lapTimer = new System.Windows.Forms.Timer(components);
            label2 = new Label();
            label1 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // realTimeTimer
            // 
            realTimeTimer.Interval = 1000;
            realTimeTimer.Tick += realTimeTimer_Tick;
            // 
            // labelTimeDate
            // 
            labelTimeDate.AutoSize = true;
            labelTimeDate.BackColor = Color.Transparent;
            labelTimeDate.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            labelTimeDate.Location = new Point(150, 19);
            labelTimeDate.Name = "labelTimeDate";
            labelTimeDate.Size = new Size(214, 25);
            labelTimeDate.TabIndex = 0;
            labelTimeDate.Text = "Current Date and Time";
            // 
            // startTimerButton
            // 
            startTimerButton.Location = new Point(12, 399);
            startTimerButton.Name = "startTimerButton";
            startTimerButton.Size = new Size(174, 39);
            startTimerButton.TabIndex = 1;
            startTimerButton.Text = "Start Timer";
            startTimerButton.UseVisualStyleBackColor = true;
            startTimerButton.Click += startTimerButton_Click;
            // 
            // pauseTimerButton
            // 
            pauseTimerButton.Location = new Point(192, 399);
            pauseTimerButton.Name = "pauseTimerButton";
            pauseTimerButton.Size = new Size(155, 39);
            pauseTimerButton.TabIndex = 1;
            pauseTimerButton.Text = "Pause Timer";
            pauseTimerButton.UseVisualStyleBackColor = true;
            pauseTimerButton.Click += pauseTimerButton_Click;
            // 
            // stopAndResetTimerButton
            // 
            stopAndResetTimerButton.Location = new Point(353, 399);
            stopAndResetTimerButton.Name = "stopAndResetTimerButton";
            stopAndResetTimerButton.Size = new Size(155, 39);
            stopAndResetTimerButton.TabIndex = 1;
            stopAndResetTimerButton.Text = "Stop and Reset Timer";
            stopAndResetTimerButton.UseVisualStyleBackColor = true;
            stopAndResetTimerButton.Click += stopAndResetTimerButton_Click;
            // 
            // bModifyTimer
            // 
            bModifyTimer.BackColor = Color.DarkGray;
            bModifyTimer.FlatStyle = FlatStyle.Popup;
            bModifyTimer.Location = new Point(514, 399);
            bModifyTimer.Name = "bModifyTimer";
            bModifyTimer.Size = new Size(45, 39);
            bModifyTimer.TabIndex = 2;
            bModifyTimer.Text = "⚙️";
            bModifyTimer.UseVisualStyleBackColor = false;
            bModifyTimer.Click += bModifyTimer_Click;
            // 
            // lapTimer
            // 
            lapTimer.Interval = 1000;
            lapTimer.Tick += lapTimer_Tick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(24, 221);
            label2.Name = "label2";
            label2.Size = new Size(194, 32);
            label2.TabIndex = 1;
            label2.Text = "Remaining time: ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 132F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(175, 108);
            label1.Name = "label1";
            label1.Size = new Size(384, 235);
            label1.TabIndex = 0;
            label1.Text = "000";
            // 
            // button1
            // 
            button1.Location = new Point(65, 84);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 3;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += BackgroundToRed_Change;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(571, 450);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(bModifyTimer);
            Controls.Add(stopAndResetTimerButton);
            Controls.Add(pauseTimerButton);
            Controls.Add(startTimerButton);
            Controls.Add(labelTimeDate);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            HelpButton = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Graduation Exam Timer";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer realTimeTimer;
        private Label labelTimeDate;
        private Button startTimerButton;
        private Button pauseTimerButton;
        private Button stopAndResetTimerButton;
        private Button bModifyTimer;
        private System.Windows.Forms.Timer lapTimer;
        private Label label1;
        private Label label2;
        private Button button1;
    }
}