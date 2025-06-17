using System;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Views
{
    public partial class MainForm : Form
    {
        private User loggedInUser;

        public MainForm(User user)
        {
            InitializeComponent();
            loggedInUser = user;
            SetupDashboard();
        }

        private void SetupDashboard()
        {
            // Default to the most secure view: hide the entire "Manage" menu.
            manageToolStripMenuItem.Visible = false;

            // Grant permissions based on role.
            if (loggedInUser != null && string.Equals(loggedInUser.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                // If the user is an Admin, show the "Manage" menu.
                manageToolStripMenuItem.Visible = true;
            }
        }

        // --- Menu Item Event Handlers ---

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CourseForm courseForm = new CourseForm();
            courseForm.Show();
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentForm studentForm = new StudentForm();
            studentForm.Show();
        }

        private void subjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubjectForm subjectForm = new SubjectForm();
            subjectForm.Show();
        }



        private void roomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoomForm roomForm = new RoomForm();
            roomForm.Show();
        }

        private void examsMarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExamForm examForm = new ExamForm();
            examForm.Show();
        }

        private void timetableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Pass the user information to the TimetableForm so it knows what to show.
            TimetableForm timetableForm = new TimetableForm(loggedInUser);
            timetableForm.Show();
        }

        // --- Form Event Handlers ---

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ensures the entire application quits when this form is closed.
            Application.Exit();
        }
    }
}