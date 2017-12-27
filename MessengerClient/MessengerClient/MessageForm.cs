using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MessageForm : Form
    {
        private String login;
        private bool firstClickMessage = true;
        public MessageForm(String login)
        {
            InitializeComponent();
            this.login = login;
            loginTextBox.Text = login;
        }

        private void messageTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (firstClickMessage) { messageTextBox.Text = ""; firstClickMessage = false; messageTextBox.ForeColor = Color.Black; }
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            String login = loginTextBox.Text;
            String message = messageTextBox.Text;
            if (firstClickMessage || message == "")
            {
                MessageBox.Show("Type message before", "Sending failed");
                return;
            }
            byte answer = Connection.SendMessage(login, message);
            String status = "Message has been send successfully.";
            String statusTitle = "Sending done";
            if (answer == Connection.MESSAGE_FAILED_CODE)
            {
                status = "Try again, please.";
                statusTitle = "Sending failed";
            }
            MessageBox.Show(status, statusTitle);
        }
    }
}
