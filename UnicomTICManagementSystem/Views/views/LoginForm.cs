using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;

namespace UnicomTICManagementSystem.Views
{
    public partial class LoginForm : Form
    {
        private LoginController loginController;

        public LoginForm()
        {
            InitializeComponent();
            loginController = new LoginController();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var user = loginController.AuthenticateUser(username, password);

            if (user != null)
            {
                MessageBox.Show($"Login successful! Welcome, {user.Username}. Your role is {user.Role}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // We will open the main dashboard here later
                MainForm mainForm = new MainForm(user); // Pass the user object
                mainForm.Show();
                this.Hide(); // Hide the login form
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
