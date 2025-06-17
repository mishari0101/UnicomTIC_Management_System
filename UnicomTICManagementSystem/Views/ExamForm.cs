using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Views
{
    public partial class ExamForm : Form
    {
        private readonly ExamController _controller;
        private List<Mark> _currentMarks; // A class-level list to hold the marks being edited

        public ExamForm()
        {
            InitializeComponent();
            _controller = new ExamController();
            _currentMarks = new List<Mark>();
        }

        private void ExamForm_Load(object sender, EventArgs e)
        {
            LoadSubjectsDropdown();
            LoadExamsGrid();
        }

        // --- Helper Methods for Loading Data ---

        private void LoadSubjectsDropdown()
        {
            cmbSubjects.DataSource = _controller.GetAllSubjects();
            cmbSubjects.DisplayMember = "SubjectName";
            cmbSubjects.ValueMember = "SubjectID";
        }

        private void LoadExamsGrid()
        {
            dgvExams.DataSource = _controller.GetAllExams();
            dgvExams.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Hide ID columns
            if (dgvExams.Columns["ExamID"] != null) dgvExams.Columns["ExamID"].Visible = false;
            if (dgvExams.Columns["SubjectID"] != null) dgvExams.Columns["SubjectID"].Visible = false;
        }

        // --- Event Handlers ---

        private void btnAddExam_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtExamName.Text) || cmbSubjects.SelectedValue == null)
            {
                MessageBox.Show("Please provide an exam name and select a subject.", "Error");
                return;
            }
            _controller.AddExam(txtExamName.Text, (int)cmbSubjects.SelectedValue);
            MessageBox.Show("Exam added successfully!");
            LoadExamsGrid(); // Refresh the list of exams
            txtExamName.Clear();
        }

        // This is the key event for the Master-Detail view.
        // When an exam is clicked, we load the marks for it.
        private void dgvExams_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvExams.CurrentRow != null)
            {
                int selectedExamId = Convert.ToInt32(dgvExams.CurrentRow.Cells["ExamID"].Value);

                // Get the marks data and store it in our class-level list
                _currentMarks = _controller.GetMarksForExam(selectedExamId);

                // Set this as the data source for the second grid
                dgvMarks.DataSource = _currentMarks;

                // Configure the marks grid for editing
                SetupMarksGrid();
            }
        }

        private void SetupMarksGrid()
        {
            dgvMarks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Hide ID columns
            if (dgvMarks.Columns["MarkID"] != null) dgvMarks.Columns["MarkID"].Visible = false;
            if (dgvMarks.Columns["StudentID"] != null) dgvMarks.Columns["StudentID"].Visible = false;
            if (dgvMarks.Columns["ExamID"] != null) dgvMarks.Columns["ExamID"].Visible = false;

            // Make columns read-only, except for the 'Score' column
            if (dgvMarks.Columns["StudentName"] != null) dgvMarks.Columns["StudentName"].ReadOnly = true;
            if (dgvMarks.Columns["Score"] != null) dgvMarks.Columns["Score"].ReadOnly = false;
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            // End any current edits in the grid
            dgvMarks.EndEdit();

            // Loop through each mark in our list and save it to the database
            foreach (DataGridViewRow row in dgvMarks.Rows)
            {
                // Get the student ID from the row
                int studentId = Convert.ToInt32(row.Cells["StudentID"].Value);
                int examId = Convert.ToInt32(row.Cells["ExamID"].Value);
                // Only save if a score has actually been entered
                if (row.Cells["Score"].Value != null && row.Cells["Score"].Value.ToString() != "")
                {
                    int score;
                    if (int.TryParse(row.Cells["Score"].Value.ToString(), out score))
                    {
                        // ...then save it.
                        _controller.SaveMark(studentId, examId, score);
                    }
                    else
                    {
                        // ...otherwise, warn the user about the bad data for that student.
                        string studentName = row.Cells["StudentName"].Value.ToString();
                        MessageBox.Show($"Invalid score entered for {studentName}. Please enter a valid number.", "Input Error");
                        return; // Stop the save process until they fix it.
                    }
                }
            }

            MessageBox.Show("All changes have been saved successfully!", "Success");

            // Optional: Refresh the grid to show the newly assigned MarkIDs, but not essential
            // If you want to refresh, you need to get the selected exam ID again and reload.
        }
    }
}