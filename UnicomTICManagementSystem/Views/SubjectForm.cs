﻿using System;
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
            // The order here is very important!
            LoadCoursesDropdown(); // Must load courses first for the dropdown.
            LoadSubjectsGrid();    // Then load the subjects into the grid.
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

        // This is the critical method for your problem.
        private void LoadCoursesDropdown()
        {
            // It gets the list of courses from the controller...
            var courses = _subjectController.GetAllCourses();

            // ...and sets them as the data source for the dropdown.
            cmbCourses.DataSource = courses;
            cmbCourses.DisplayMember = "CourseName"; // What the user sees
            cmbCourses.ValueMember = "CourseID";     // The hidden ID value we use
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // --- VALIDATION START ---
            if (string.IsNullOrWhiteSpace(txtSubjectName.Text))
            {
                MessageBox.Show("Please enter a subject name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbCourses.SelectedValue == null)
            {
                MessageBox.Show("Please select a course for this subject.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // --- VALIDATION END ---

            _subjectController.AddSubject(txtSubjectName.Text, (int)cmbCourses.SelectedValue);
            MessageBox.Show("Subject added successfully!", "Success");
            LoadSubjectsGrid();
            txtSubjectName.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.CurrentRow == null)
            {
                MessageBox.Show("Please select a subject from the grid to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- VALIDATION START ---
            if (string.IsNullOrWhiteSpace(txtSubjectName.Text))
            {
                MessageBox.Show("Please enter a subject name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbCourses.SelectedValue == null)
            {
                MessageBox.Show("Please select a course for this subject.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // --- VALIDATION END ---

            int subjectId = Convert.ToInt32(dgvSubjects.CurrentRow.Cells["SubjectID"].Value);
            _subjectController.UpdateSubject(subjectId, txtSubjectName.Text, (int)cmbCourses.SelectedValue);
            MessageBox.Show("Subject updated successfully!", "Success");
            LoadSubjectsGrid();
            txtSubjectName.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.CurrentRow == null) return;

            var confirm = MessageBox.Show("Are you sure?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                int subjectId = Convert.ToInt32(dgvSubjects.CurrentRow.Cells["SubjectID"].Value);
                _subjectController.DeleteSubject(subjectId);
                LoadSubjectsGrid();
                txtSubjectName.Clear();
            }
        }

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