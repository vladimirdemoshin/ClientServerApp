namespace Client
{
    partial class MessageForm
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
            this.sendMessageButton = new System.Windows.Forms.Button();
            this.loginTextBox = new System.Windows.Forms.TextBox();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // sendMessageButton
            // 
            this.sendMessageButton.Location = new System.Drawing.Point(199, 186);
            this.sendMessageButton.Name = "sendMessageButton";
            this.sendMessageButton.Size = new System.Drawing.Size(121, 23);
            this.sendMessageButton.TabIndex = 2;
            this.sendMessageButton.Text = "Send message";
            this.sendMessageButton.UseVisualStyleBackColor = true;
            this.sendMessageButton.Click += new System.EventHandler(this.sendMessageButton_Click);
            // 
            // loginTextBox
            // 
            this.loginTextBox.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.loginTextBox.Location = new System.Drawing.Point(12, 12);
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.ReadOnly = true;
            this.loginTextBox.Size = new System.Drawing.Size(308, 20);
            this.loginTextBox.TabIndex = 3;
            this.loginTextBox.TabStop = false;
            // 
            // messageTextBox
            // 
            this.messageTextBox.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.messageTextBox.Location = new System.Drawing.Point(12, 38);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(308, 142);
            this.messageTextBox.TabIndex = 4;
            this.messageTextBox.TabStop = false;
            this.messageTextBox.Text = "Type your message here...";
            this.messageTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.messageTextBox_MouseClick);
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 217);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.loginTextBox);
            this.Controls.Add(this.sendMessageButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MessageForm";
            this.Text = "Messenger";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button sendMessageButton;
        private System.Windows.Forms.TextBox loginTextBox;
        private System.Windows.Forms.TextBox messageTextBox;
    }
}