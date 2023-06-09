﻿namespace English_Exam_Timer
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
            lRemainingTime = new Label();
            label3 = new Label();
            lLapTime = new Label();
            ProgressBarTimer = new ProgressBar();
            LabelMinAndSec = new Label();
            label1 = new Label();
            ChbWantLoop = new CheckBox();
            SuspendLayout();
            // 
            // realTimeTimer
            // 
            realTimeTimer.Interval = 1000;
            realTimeTimer.Tick += RealTimeTimer_Tick;
            // 
            // labelTimeDate
            // 
            labelTimeDate.AutoSize = true;
            labelTimeDate.BackColor = Color.Transparent;
            labelTimeDate.Font = new Font("Segoe UI", 28F, FontStyle.Regular, GraphicsUnit.Point);
            labelTimeDate.Location = new Point(24, 19);
            labelTimeDate.Name = "labelTimeDate";
            labelTimeDate.Size = new Size(403, 51);
            labelTimeDate.TabIndex = 0;
            labelTimeDate.Text = "Current Date and Time";
            // 
            // startTimerButton
            // 
            startTimerButton.BackColor = Color.FromArgb(192, 255, 192);
            startTimerButton.Font = new Font("Segoe UI", 28F, FontStyle.Regular, GraphicsUnit.Point);
            startTimerButton.Location = new Point(15, 451);
            startTimerButton.Name = "startTimerButton";
            startTimerButton.Size = new Size(296, 134);
            startTimerButton.TabIndex = 1;
            startTimerButton.Text = "Start Timer";
            startTimerButton.UseVisualStyleBackColor = false;
            startTimerButton.Click += StartTimerButton_Click;
            // 
            // pauseTimerButton
            // 
            pauseTimerButton.BackColor = Color.Linen;
            pauseTimerButton.Font = new Font("Segoe UI", 28F, FontStyle.Regular, GraphicsUnit.Point);
            pauseTimerButton.Location = new Point(317, 451);
            pauseTimerButton.Name = "pauseTimerButton";
            pauseTimerButton.Size = new Size(304, 134);
            pauseTimerButton.TabIndex = 1;
            pauseTimerButton.Text = "Pause Timer";
            pauseTimerButton.UseVisualStyleBackColor = false;
            pauseTimerButton.Click += PauseTimerButton_Click;
            // 
            // stopAndResetTimerButton
            // 
            stopAndResetTimerButton.BackColor = Color.MistyRose;
            stopAndResetTimerButton.Font = new Font("Segoe UI", 22F, FontStyle.Regular, GraphicsUnit.Point);
            stopAndResetTimerButton.Location = new Point(627, 451);
            stopAndResetTimerButton.Name = "stopAndResetTimerButton";
            stopAndResetTimerButton.Size = new Size(284, 133);
            stopAndResetTimerButton.TabIndex = 1;
            stopAndResetTimerButton.Text = "Stop and Reset Timer";
            stopAndResetTimerButton.UseVisualStyleBackColor = false;
            stopAndResetTimerButton.Click += StopAndResetTimerButton_Click;
            // 
            // bModifyTimer
            // 
            bModifyTimer.BackColor = Color.Silver;
            bModifyTimer.Font = new Font("Segoe UI", 42F, FontStyle.Regular, GraphicsUnit.Point);
            bModifyTimer.Location = new Point(917, 450);
            bModifyTimer.Name = "bModifyTimer";
            bModifyTimer.Size = new Size(184, 134);
            bModifyTimer.TabIndex = 2;
            bModifyTimer.Text = "⚙️";
            bModifyTimer.UseVisualStyleBackColor = false;
            bModifyTimer.Click += ButtonModifyTimer_Click;
            // 
            // lapTimer
            // 
            lapTimer.Interval = 1000;
            lapTimer.Tick += LapTimer_Tick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.FlatStyle = FlatStyle.Popup;
            label2.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(24, 89);
            label2.Name = "label2";
            label2.Size = new Size(259, 45);
            label2.TabIndex = 1;
            label2.Text = "Remaining time: ";
            // 
            // lRemainingTime
            // 
            lRemainingTime.AutoSize = true;
            lRemainingTime.BackColor = Color.Transparent;
            lRemainingTime.FlatStyle = FlatStyle.Flat;
            lRemainingTime.Font = new Font("Segoe UI", 25F, FontStyle.Regular, GraphicsUnit.Point);
            lRemainingTime.Location = new Point(289, 89);
            lRemainingTime.Name = "lRemainingTime";
            lRemainingTime.Size = new Size(74, 46);
            lRemainingTime.TabIndex = 0;
            lRemainingTime.Text = "000";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 28F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(759, 19);
            label3.Name = "label3";
            label3.Size = new Size(226, 51);
            label3.TabIndex = 4;
            label3.Text = "Current Lap:";
            // 
            // lLapTime
            // 
            lLapTime.AutoSize = true;
            lLapTime.BackColor = Color.Transparent;
            lLapTime.Font = new Font("Segoe UI", 28F, FontStyle.Bold, GraphicsUnit.Point);
            lLapTime.Location = new Point(998, 19);
            lLapTime.Name = "lLapTime";
            lLapTime.Size = new Size(77, 51);
            lLapTime.TabIndex = 5;
            lLapTime.Text = "lap";
            // 
            // ProgressBarTimer
            // 
            ProgressBarTimer.Location = new Point(120, 350);
            ProgressBarTimer.Name = "ProgressBarTimer";
            ProgressBarTimer.Size = new Size(854, 23);
            ProgressBarTimer.TabIndex = 6;
            // 
            // LabelMinAndSec
            // 
            LabelMinAndSec.AutoSize = true;
            LabelMinAndSec.BackColor = Color.Transparent;
            LabelMinAndSec.FlatStyle = FlatStyle.Flat;
            LabelMinAndSec.Font = new Font("Segoe UI", 96F, FontStyle.Bold, GraphicsUnit.Point);
            LabelMinAndSec.Location = new Point(582, 177);
            LabelMinAndSec.Name = "LabelMinAndSec";
            LabelMinAndSec.Size = new Size(403, 170);
            LabelMinAndSec.TabIndex = 7;
            LabelMinAndSec.Text = "00:00";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 38F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(120, 229);
            label1.Margin = new Padding(0, 0, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(407, 68);
            label1.TabIndex = 1;
            label1.Text = "Remaining time: ";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ChbWantLoop
            // 
            ChbWantLoop.AutoSize = true;
            ChbWantLoop.Checked = true;
            ChbWantLoop.CheckState = CheckState.Checked;
            ChbWantLoop.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point);
            ChbWantLoop.Location = new Point(934, 379);
            ChbWantLoop.Name = "ChbWantLoop";
            ChbWantLoop.Size = new Size(154, 69);
            ChbWantLoop.TabIndex = 8;
            ChbWantLoop.Text = "Loop";
            ChbWantLoop.UseVisualStyleBackColor = true;
            ChbWantLoop.CheckedChanged += ChbWantLoop_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1113, 597);
            Controls.Add(ChbWantLoop);
            Controls.Add(label1);
            Controls.Add(lRemainingTime);
            Controls.Add(LabelMinAndSec);
            Controls.Add(ProgressBarTimer);
            Controls.Add(label3);
            Controls.Add(lLapTime);
            Controls.Add(label2);
            Controls.Add(bModifyTimer);
            Controls.Add(stopAndResetTimerButton);
            Controls.Add(pauseTimerButton);
            Controls.Add(startTimerButton);
            Controls.Add(labelTimeDate);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Graduation Exam Timer by: Filip Lacina(v1.4.0.)";
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
        private Label lRemainingTime;
        private Label label2;
        private Label label3;
        private Label lLapTime;
        private ProgressBar ProgressBarTimer;
        private Label LabelMinAndSec;
        private Label label1;
        private CheckBox ChbWantLoop;
    }
}