using System;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;

namespace UnicomTICManagementSystem.Views
{
    public partial class SubjectForm : Form
    {
        private readonly SubjectController _subjectController;

        public SubjectForm()
        {
            InitializeComponent();
            _subjectController = new SubjectController();
        }

        // This method runs automatically when the form is opened.
        private void SubjectForm_Load(object sender, EventArgs e)
        {
            LoadCoursesDropdown(); // Must load courses first for the dropdown
            LoadSubjectsGrid();    // Then load the subjects into the grid
        }

        private void LoadSubjectsGrid()
        {
            dgvSubjects.DataSource = _subjectController.GetAllSubjects();
            dgvSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Hide ID columns from the user to keep the interface clean
            if (dgvSubjects.Columns["SubjectID"] != null)
                dgvSubjects.Columns["SubjectID"].Visible = false;
            if (dgvSubjects.Columns["CourseID"] != null)
                dgvSubjects.Columns["CourseID"].Visible = false;
        }

        private void LoadCoursesDropdown()
        {
            cmbCourses.DataSource = _subjectController.GetAllCourses();
            cmbCourses.DisplayMember = "CourseName"; // The text the user sees
            cmbCourses.ValueMember = "CourseID";     // The hidden ID value we use
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSubjectName.Text) || cmbCourses.SelectedValue == null)
            {
                MessageBox.Show("Please enter a subject name and select a course.", "Error");
                return;
            }
            _subjectController.AddSubject(txtSubjectName.Text, (int)cmbCourses.SelectedValue);
            MessageBox.Show("Subject added successfully!", "Success");
            LoadSubjectsGrid();
            txtSubjectName.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.CurrentRow == null) return; // Nothing selected

            int subjectId = Convert.ToInt32(dgvSubjects.CurrentRow.Cells["SubjectID"].Value);
            _subjectController.UpdateSubject(subjectId, txtSubjectName.Text, (int)cmbCourses.SelectedValue);
            MessageBox.Show("Subject updated successfully!", "Success");
            LoadSubjectsGrid();
            txtSubjectName.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.CurrentRow == null) return; // Nothing selected

            var confirm = MessageBox.Show("Are you sure?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                int subjectId = Convert.ToInt32(dgvSubjects.CurrentRow.Cells["SubjectID"].Value);
                _subjectController.DeleteSubject(subjectId);
                LoadSubjectsGrid();
                txtSubjectName.Clear();
            }
        }

        // Fills the textboxes when you click on a row in the grid.
        private void dgvSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSubjects.CurrentRow != null)
            {
                txtSubjectName.Text = dgvSubjects.CurrentRow.Cells["SubjectName"].Value.ToString();
                cmbCourses.SelectedValue = dgvSubjects.CurrentRow.Cells["CourseID"].Value;
            }
        }
    }
}