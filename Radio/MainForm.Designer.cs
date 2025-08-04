namespace VHF_Controller
{
    partial class MainForm
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
            btn1 = new Button();
            lblFrequencyDisplay = new Label();
            btn2 = new Button();
            btn3 = new Button();
            btn6 = new Button();
            btn5 = new Button();
            btn4 = new Button();
            btn9 = new Button();
            btn8 = new Button();
            btn7 = new Button();
            btn0 = new Button();
            btnReset = new Button();
            btnEnter = new Button();
            feedbackTimer = new System.Windows.Forms.Timer(components);
            lblFeedback = new Label();
            lblStatus = new Label();
            btnReconnect = new Button();
            SuspendLayout();
            // 
            // btn1
            // 
            btn1.Font = new Font("Audiowide", 16F, FontStyle.Bold);
            btn1.Location = new Point(19, 53);
            btn1.Name = "btn1";
            btn1.Size = new Size(91, 48);
            btn1.TabIndex = 0;
            btn1.Text = "1";
            btn1.UseVisualStyleBackColor = true;
            btn1.Click += btn1_Click;
            // 
            // lblFrequencyDisplay
            // 
            lblFrequencyDisplay.AutoSize = true;
            lblFrequencyDisplay.Font = new Font("Audiowide", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFrequencyDisplay.Location = new Point(76, 9);
            lblFrequencyDisplay.Name = "lblFrequencyDisplay";
            lblFrequencyDisplay.Size = new Size(164, 41);
            lblFrequencyDisplay.TabIndex = 3;
            lblFrequencyDisplay.Text = "___.___";
            lblFrequencyDisplay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btn2
            // 
            btn2.Font = new Font("Audiowide", 16F, FontStyle.Bold);
            btn2.Location = new Point(112, 53);
            btn2.Name = "btn2";
            btn2.Size = new Size(91, 48);
            btn2.TabIndex = 4;
            btn2.Text = "2";
            btn2.UseVisualStyleBackColor = true;
            btn2.Click += btn2_Click;
            // 
            // btn3
            // 
            btn3.Font = new Font("Audiowide", 16F, FontStyle.Bold);
            btn3.Location = new Point(204, 53);
            btn3.Name = "btn3";
            btn3.Size = new Size(91, 48);
            btn3.TabIndex = 5;
            btn3.Text = "3";
            btn3.UseVisualStyleBackColor = true;
            btn3.Click += btn3_Click;
            // 
            // btn6
            // 
            btn6.Font = new Font("Audiowide", 16F, FontStyle.Bold);
            btn6.Location = new Point(204, 107);
            btn6.Name = "btn6";
            btn6.Size = new Size(91, 48);
            btn6.TabIndex = 8;
            btn6.Text = "6";
            btn6.UseVisualStyleBackColor = true;
            btn6.Click += btn6_Click;
            // 
            // btn5
            // 
            btn5.Font = new Font("Audiowide", 16F, FontStyle.Bold);
            btn5.Location = new Point(112, 107);
            btn5.Name = "btn5";
            btn5.Size = new Size(91, 48);
            btn5.TabIndex = 7;
            btn5.Text = "5";
            btn5.UseVisualStyleBackColor = true;
            btn5.Click += btn5_Click;
            // 
            // btn4
            // 
            btn4.Font = new Font("Audiowide", 16F, FontStyle.Bold);
            btn4.Location = new Point(19, 107);
            btn4.Name = "btn4";
            btn4.Size = new Size(91, 48);
            btn4.TabIndex = 6;
            btn4.Text = "4";
            btn4.UseVisualStyleBackColor = true;
            btn4.Click += btn4_Click;
            // 
            // btn9
            // 
            btn9.Font = new Font("Audiowide", 16F, FontStyle.Bold);
            btn9.Location = new Point(204, 161);
            btn9.Name = "btn9";
            btn9.Size = new Size(91, 48);
            btn9.TabIndex = 11;
            btn9.Text = "9";
            btn9.UseVisualStyleBackColor = true;
            btn9.Click += btn9_Click;
            // 
            // btn8
            // 
            btn8.Font = new Font("Audiowide", 16F, FontStyle.Bold);
            btn8.Location = new Point(112, 161);
            btn8.Name = "btn8";
            btn8.Size = new Size(91, 48);
            btn8.TabIndex = 10;
            btn8.Text = "8";
            btn8.UseVisualStyleBackColor = true;
            btn8.Click += btn8_Click;
            // 
            // btn7
            // 
            btn7.Font = new Font("Audiowide", 16F, FontStyle.Bold);
            btn7.Location = new Point(19, 161);
            btn7.Name = "btn7";
            btn7.Size = new Size(91, 48);
            btn7.TabIndex = 9;
            btn7.Text = "7";
            btn7.UseVisualStyleBackColor = true;
            btn7.Click += btn7_Click;
            // 
            // btn0
            // 
            btn0.Font = new Font("Audiowide", 16F, FontStyle.Bold);
            btn0.Location = new Point(112, 215);
            btn0.Name = "btn0";
            btn0.Size = new Size(91, 48);
            btn0.TabIndex = 12;
            btn0.Text = "0";
            btn0.UseVisualStyleBackColor = true;
            btn0.Click += btn0_Click;
            // 
            // btnReset
            // 
            btnReset.Font = new Font("Audiowide", 12F, FontStyle.Bold);
            btnReset.Location = new Point(19, 215);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(91, 48);
            btnReset.TabIndex = 13;
            btnReset.Text = "RST";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnEnter
            // 
            btnEnter.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEnter.Location = new Point(204, 215);
            btnEnter.Name = "btnEnter";
            btnEnter.Size = new Size(91, 48);
            btnEnter.TabIndex = 14;
            btnEnter.Text = "ADV";
            btnEnter.UseVisualStyleBackColor = true;
            btnEnter.Click += btnEnter_Click;
            // 
            // feedbackTimer
            // 
            feedbackTimer.Interval = 1000;
            // 
            // lblFeedback
            // 
            lblFeedback.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblFeedback.AutoSize = true;
            lblFeedback.BackColor = Color.Transparent;
            lblFeedback.Font = new Font("Microsoft Sans Serif", 48F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFeedback.ForeColor = Color.LimeGreen;
            lblFeedback.Location = new Point(24, 118);
            lblFeedback.Name = "lblFeedback";
            lblFeedback.Size = new Size(261, 73);
            lblFeedback.TabIndex = 17;
            lblFeedback.Text = "CHECK";
            lblFeedback.Visible = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = Color.Red;
            lblStatus.Location = new Point(19, 273);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(79, 15);
            lblStatus.TabIndex = 18;
            lblStatus.Text = "Disconnected";
            // 
            // btnReconnect
            // 
            btnReconnect.Location = new Point(220, 269);
            btnReconnect.Name = "btnReconnect";
            btnReconnect.Size = new Size(75, 23);
            btnReconnect.TabIndex = 19;
            btnReconnect.Text = "Reconnect";
            btnReconnect.UseVisualStyleBackColor = true;
            btnReconnect.Click += btnReconnect_Click;
            // 
            // MainForm
            // 
            ClientSize = new Size(323, 299);
            Controls.Add(btnReconnect);
            Controls.Add(lblStatus);
            Controls.Add(lblFeedback);
            Controls.Add(btnEnter);
            Controls.Add(btnReset);
            Controls.Add(btn0);
            Controls.Add(btn9);
            Controls.Add(btn8);
            Controls.Add(btn7);
            Controls.Add(btn6);
            Controls.Add(btn5);
            Controls.Add(btn4);
            Controls.Add(btn3);
            Controls.Add(btn2);
            Controls.Add(lblFrequencyDisplay);
            Controls.Add(btn1);
            Name = "MainForm";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Label lblFrequencyDisplay;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn9;
        private System.Windows.Forms.Button btn8;
        private System.Windows.Forms.Button btn7;
        private System.Windows.Forms.Button btn0;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Timer feedbackTimer;
        private System.Windows.Forms.Label lblFeedback;
        private Label lblStatus;
        private Button btnReconnect;
    }
}

