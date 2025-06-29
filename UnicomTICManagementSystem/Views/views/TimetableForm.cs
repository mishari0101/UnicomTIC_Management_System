using System;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;
using UnicomTICManagementSystem.Models; // We need this for the User model

namespace UnicomTICManagementSystem.Views
{
    public partial class TimetableForm : Form
    {
        private readonly TimetableController _controller;
        private readonly User _loggedInUser; // Store the logged-in user

        // The constructor now accepts a User object
        public TimetableForm(User user)
        {
            InitializeComponent();
            _controller = new TimetableController();
            _loggedInUser = user; // Save the user
        }

        private void TimetableForm_Load(object sender, EventArgs e)
        {
            SetupFormForRole(); // Set up the form based on user role
            LoadTimetableGrid();
        }

        private void SetupFormForRole()
        {
            // By default, assume it's a student (read-only view)
            pnlAdminControls.Visible = false;
            dgvTimetable.ReadOnly = true;

            // If the user is an Admin, show the controls
            if (string.Equals(_loggedInUser.Role, "Admin", StringComparison.OrdinalIgnoreCase))

            {
                pnlAdminControls.Visible = true;
                dgvTimetable.ReadOnly = false;
                // Load the dropdowns only if they are an admin
                LoadDropdowns();
            }
        }

        private void LoadTimetableGrid()
        {
            dgvTimetable.DataSource = _controller.GetAllTimetableEntries();
            dgvTimetable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Hide ID columns
            if (dgvTimetable.Columns["TimetableID"] != null) dgvTimetable.Columns["TimetableID"].Visible = false;
            if (dgvTimetable.Columns["SubjectID"] != null) dgvTimetable.Columns["SubjectID"].Visible = false;
            if (dgvTimetable.Columns["RoomID"] != null) dgvTimetable.Columns["RoomID"].Visible = false;
        }

        private void LoadDropdowns()
        {
            cmbSubjects.DataSource = _controller.GetAllSubjects();
            cmbSubjects.DisplayMember = "SubjectName";
            cmbSubjects.ValueMember = "SubjectID";

            cmbRooms.DataSource = _controller.GetAllRooms();
            cmbRooms.DisplayMember = "RoomName";
            cmbRooms.ValueMember = "RoomID";
        }

        // --- ALL THE CODE BELOW IS ONLY USED BY ADMINS ---

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _controller.AddEntry(txtTimeSlot.Text, (int)cmbSubjects.SelectedValue, (int)cmbRooms.SelectedValue);
            LoadTimetableGrid();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvTimetable.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvTimetable.CurrentRow.Cells["TimetableID"].Value);
            _controller.UpdateEntry(id, txtTimeSlot.Text, (int)cmbSubjects.SelectedValue, (int)cmbRooms.SelectedValue);
            LoadTimetableGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTimetable.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvTimetable.CurrentRow.Cells["TimetableID"].Value);
            _controller.DeleteEntry(id);
            LoadTimetableGrid();
        }

        private void dgvTimetable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // This code should only run if the panel is visible (i.e., for an admin)
            if (pnlAdminControls.Visible && e.RowIndex >= 0 && dgvTimetable.CurrentRow != null)
            {
                txtTimeSlot.Text = dgvTimetable.CurrentRow.Cells["TimeSlot"].Value.ToString();
                cmbSubjects.SelectedValue = dgvTimetable.CurrentRow.Cells["SubjectID"].Value;
                cmbRooms.SelectedValue = dgvTimetable.CurrentRow.Cells["RoomID"].Value;
            }
        }
    }
}