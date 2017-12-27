using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public partial class RegistrationForm : Form
    {
        private bool firstClickLogin = true;
        private bool firstClickPassword = true;
        public RegistrationForm()
        {
            InitializeComponent();
        }
        private void loginTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (firstClickLogin) { loginTextBox.Text = ""; firstClickLogin = false; loginTextBox.ForeColor = Color.Black; }
        }
        private void passwordTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (firstClickPassword) { passwordTextBox.Text = ""; firstClickPassword = false; passwordTextBox.ForeColor = Color.Black; }
        }
        private void submitButton_Click(object sender, EventArgs e)
        {
            String login = loginTextBox.Text;
            String password = passwordTextBox.Text;
            if (firstClickLogin || firstClickPassword || login == "" || password == "")
            {
                MessageBox.Show("Insert login and password before", "Registration failed");
                return;
            }
            byte answer = Connection.Registration(login, password);
            String status = "Registration has done successfully. Now you can log in.";
            String statusTitle = "Registration done";
            if (answer == Connection.REGISTRATION_FAILED_CODE)
            {
                status = "This login is used already. Try again.";
                statusTitle = "Registration failed";
            }
            MessageBox.Show(status, statusTitle);
            if (answer == Connection.REGISTRATION_DONE_CODE)
            {
                this.Close();
            }
        }
    }
}
