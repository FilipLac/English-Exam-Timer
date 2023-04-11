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
            realTimeTimer = new System.Windows.Forms.Timer(components);
            label1 = new Label();
            startTimerButton = new Button();
            pauseTimerButton = new Button();
            stopAndResetTimerButton = new Button();
            bModifyTimer = new Button();
            lapTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // realTimeTimer
            // 
            realTimeTimer.Interval = 1000;
            realTimeTimer.Tick += timer1_Tick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(216, 24);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // startTimerButton
            // 
            startTimerButton.Location = new Point(135, 415);
            startTimerButton.Name = "startTimerButton";
            startTimerButton.Size = new Size(75, 23);
            startTimerButton.TabIndex = 1;
            startTimerButton.Text = "Start Timer";
            startTimerButton.UseVisualStyleBackColor = true;
            startTimerButton.Click += startTimerButton_Click;
            // 
            // pauseTimerButton
            // 
            pauseTimerButton.Location = new Point(216, 415);
            pauseTimerButton.Name = "pauseTimerButton";
            pauseTimerButton.Size = new Size(111, 23);
            pauseTimerButton.TabIndex = 1;
            pauseTimerButton.Text = "Pause Timer";
            pauseTimerButton.UseVisualStyleBackColor = true;
            pauseTimerButton.Click += pauseTimerButton_Click;
            // 
            // stopAndResetTimerButton
            // 
            stopAndResetTimerButton.Location = new Point(333, 415);
            stopAndResetTimerButton.Name = "stopAndResetTimerButton";
            stopAndResetTimerButton.Size = new Size(128, 23);
            stopAndResetTimerButton.TabIndex = 1;
            stopAndResetTimerButton.Text = "Stop and Reset Timer";
            stopAndResetTimerButton.UseVisualStyleBackColor = true;
            stopAndResetTimerButton.Click += stopAndResetTimerButton_Click;
            // 
            // bModifyTimer
            // 
            bModifyTimer.BackColor = Color.DarkGray;
            bModifyTimer.FlatStyle = FlatStyle.Popup;
            bModifyTimer.Location = new Point(528, 415);
            bModifyTimer.Name = "bModifyTimer";
            bModifyTimer.Size = new Size(28, 23);
            bModifyTimer.TabIndex = 2;
            bModifyTimer.Text = "⚙️";
            bModifyTimer.UseVisualStyleBackColor = false;
            bModifyTimer.Click += bModifyTimer_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(568, 450);
            Controls.Add(bModifyTimer);
            Controls.Add(stopAndResetTimerButton);
            Controls.Add(pauseTimerButton);
            Controls.Add(startTimerButton);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer realTimeTimer;
        private Label label1;
        private Button startTimerButton;
        private Button pauseTimerButton;
        private Button stopAndResetTimerButton;
        private Button bModifyTimer;
        private System.Windows.Forms.Timer lapTimer;
    }
}