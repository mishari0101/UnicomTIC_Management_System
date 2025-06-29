using System;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories; // We can call the repository directly for this simple view

namespace UnicomTICManagementSystem.Views
{
    public partial class StudentMarksForm : Form
    {
        private readonly User _loggedInUser;

        // The form is created with the student's user information
        public StudentMarksForm(User user)
        {
            InitializeComponent();
            _loggedInUser = user;
        }

        // When the form loads, get the marks and display them
        private void StudentMarksForm_Load(object sender, EventArgs e)
        {
            // Use the student's username to get their specific marks
            var myMarks = DatabaseManager.GetMarksForStudent(_loggedInUser.Username);

            dgvMyMarks.DataSource = myMarks;
            dgvMyMarks.ReadOnly = true; // Make the grid completely read-only
            dgvMyMarks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}