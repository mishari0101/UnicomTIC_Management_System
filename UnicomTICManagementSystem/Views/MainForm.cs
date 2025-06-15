using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Views;

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
            // By default, hide all management buttons
            btnCourses.Visible = false;
            btnStudents.Visible = false;
            btnExams.Visible = false;
            // Timetable is visible to all, so we don't hide it

            // Show buttons based on role
            switch (loggedInUser.Role)
            {
                case "Admin":
                    // Admin sees everything
                    btnCourses.Visible = true;
                    btnStudents.Visible = true;
                    btnExams.Visible = true;
                    break;
                case "Staff":
                    // Staff can manage exams and view timetable
                    btnExams.Visible = true;
                    break;
                case "Lecturer":
                    // Lecturer can manage exams and view timetable
                    btnExams.Visible = true;
                    break;
                case "Student":
                    // Student can only view their stuff (we'll handle this inside the forms)
                    break;
            }
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            // Create a new instance of our CourseForm
            CourseForm courseManagementForm = new CourseForm();
            // Show it to the user
            courseManagementForm.Show();
        }
        private void btnStudents_Click(object sender, EventArgs e)
        {
            StudentForm studentForm = new StudentForm();
            studentForm.Show();
        }

    }
}



