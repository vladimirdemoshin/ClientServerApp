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
    public partial class AuthenticationForm : Form
    {
        private bool firstClickLogin = true;
        private bool firstClickPassword = true;
        public AuthenticationForm()
        {
            InitializeComponent();
        }
        private void submitButton_Click(object sender, EventArgs e)
        {
            String login = loginTextBox.Text;
            String password = passwordTextBox.Text;
            if (firstClickLogin || firstClickPassword || login == "" || password == "")
            {
                MessageBox.Show("Insert login and password before", "Autentication failed");
                return;
            }
            byte answer = Connection.Authentication(login, password);
            String status = "Authentication has done successfully. Now you can log in.";
            String statusTitle = "Authentication done";
            if (answer == Connection.AUTHENTICATION_FAILED_CODE)
            {
                status = "This login is used already or password is incorrect. Try again.";
                statusTitle = "Authentication failed";
            }
            MessageBox.Show(status, statusTitle);
            if (answer == Connection.AUTHENTICATION_DONE_CODE)
            {
                this.Hide();
                MessageForm messageForm = new MessageForm(login);
                messageForm.ShowDialog();
                this.Close();
            }
        }
        private void loginTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (firstClickLogin) { loginTextBox.Text = ""; firstClickLogin = false; loginTextBox.ForeColor = Color.Black; }
        }
        private void passwordTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (firstClickPassword) { passwordTextBox.Text = ""; firstClickPassword = false; passwordTextBox.ForeColor = Color.Black; }
        }
    }
}
