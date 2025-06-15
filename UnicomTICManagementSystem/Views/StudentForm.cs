using System;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;

namespace UnicomTICManagementSystem.Views
{
    public partial class StudentForm : Form
    {
        private readonly StudentController _studentController;

        public StudentForm()
        {
            InitializeComponent();
            _studentController = new StudentController();
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            LoadCoursesDropdown(); // Load dropdown first
            LoadStudentsGrid();   // Then load the grid
        }

        // --- Helper Methods to Load Data ---
        private void LoadStudentsGrid()
        {
            dgvStudents.DataSource = _studentController.GetAllStudents();
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Hide the ID columns as they are not for the user
            if (dgvStudents.Columns["StudentID"] != null)
                dgvStudents.Columns["StudentID"].Visible = false;
            if (dgvStudents.Columns["CourseID"] != null)
                dgvStudents.Columns["CourseID"].Visible = false;
        }

        private void LoadCoursesDropdown()
        {
            var courses = _studentController.GetAllCourses();
            cmbCourses.DataSource = courses;
            cmbCourses.DisplayMember = "CourseName"; // What the user sees
            cmbCourses.ValueMember = "CourseID";     // The hidden value we use in our code
        }

        // --- Button and Grid Events ---
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentName.Text) || cmbCourses.SelectedValue == null)
            {
                MessageBox.Show("Please enter a student name and select a course.", "Error");
                return;
            }

            string studentName = txtStudentName.Text;
            int courseId = (int)cmbCourses.SelectedValue; // Get the selected course's ID

            _studentController.AddStudent(studentName, courseId);
            MessageBox.Show("Student added successfully!", "Success");

            LoadStudentsGrid(); // Refresh grid
            txtStudentName.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Please select a student to update.", "Error");
                return;
            }

            int studentId = Convert.ToInt32(dgvStudents.CurrentRow.Cells["StudentID"].Value);
            string newName = txtStudentName.Text;
            int newCourseId = (int)cmbCourses.SelectedValue;

            _studentController.UpdateStudent(studentId, newName, newCourseId);
            MessageBox.Show("Student updated successfully!", "Success");

            LoadStudentsGrid();
            txtStudentName.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Please select a student to delete.", "Error");
                return;
            }

            var confirm = MessageBox.Show("Are you sure?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                int studentId = Convert.ToInt32(dgvStudents.CurrentRow.Cells["StudentID"].Value);
                _studentController.DeleteStudent(studentId);
                LoadStudentsGrid();
                txtStudentName.Clear();
            }
        }

        // When a user clicks a row in the grid, fill the controls with that student's data.
        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvStudents.CurrentRow != null)
            {
                txtStudentName.Text = dgvStudents.CurrentRow.Cells["Name"].Value.ToString();
                // This will automatically select the correct course in the dropdown
                cmbCourses.SelectedValue = dgvStudents.CurrentRow.Cells["CourseID"].Value;
            }
        }
    }
}