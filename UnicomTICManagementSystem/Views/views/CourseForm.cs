using System;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers; // We need this to use our controller

namespace UnicomTICManagementSystem.Views
{
    public partial class CourseForm : Form
    {
        // A variable to hold our controller
        private readonly CourseController _courseController;

        public CourseForm()
        {
            InitializeComponent();
            _courseController = new CourseController(); // Create a new instance of our controller
        }

        // This event runs automatically when the form first loads
        private void CourseForm_Load(object sender, EventArgs e)
        {
            LoadCourses();
        }

        // A helper method to load/reload all courses into the grid
        private void LoadCourses()
        {
            // Get all courses from the controller and set them as the data source for the grid
            dgvCourses.DataSource = _courseController.GetAllCourses();
            // Optional: Auto-size columns to fit content
            dgvCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            // --- VALIDATION START ---
            if (string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                MessageBox.Show("Please enter a course name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop the method here
            }
            // --- VALIDATION END ---

            _courseController.AddCourse(txtCourseName.Text);
            MessageBox.Show("Course added successfully!", "Success");
            LoadCourses();
            txtCourseName.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow == null)
            {
                MessageBox.Show("Please select a course from the grid to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- VALIDATION START ---
            if (string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                MessageBox.Show("Please enter a course name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop the method here
            }
            // --- VALIDATION END ---

            int courseId = Convert.ToInt32(dgvCourses.CurrentRow.Cells["CourseID"].Value);
            _courseController.UpdateCourse(courseId, txtCourseName.Text);
            MessageBox.Show("Course updated successfully!", "Success");
            LoadCourses();
            txtCourseName.Clear();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow != null)
            {
                var confirmResult = MessageBox.Show("Are you sure you want to delete this course?", "Confirm Delete", MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    // FIX: Use Convert.ToInt32 here as well for safety and consistency.
                    int courseId = Convert.ToInt32(dgvCourses.CurrentRow.Cells["CourseID"].Value);
                    _courseController.DeleteCourse(courseId);
                    LoadCourses();
                    txtCourseName.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please select a course to delete.", "Error");
            }
        }
        private void dgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCourses.CurrentRow != null)
            {
                txtCourseName.Text = dgvCourses.CurrentRow.Cells["CourseName"].Value.ToString();
            }
        }
    }

}
