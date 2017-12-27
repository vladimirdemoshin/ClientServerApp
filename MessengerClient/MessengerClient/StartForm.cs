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
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void registrationButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegistrationForm regForm = new RegistrationForm();
            regForm.ShowDialog();
            this.Show();
        }

        private void authenticationButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            AuthenticationForm authForm = new AuthenticationForm();
            authForm.ShowDialog();
            this.Show();
        }
    }
}
