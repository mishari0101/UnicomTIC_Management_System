using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers; // Assuming you have a UserController
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Views
{
    public partial class UserForm : Form
    {
        private readonly UserController _controller;
        private readonly User _adminUser;
        private readonly string _passwordPlaceholder = "Enter new password to change";

        public UserForm(User adminUser)
        {
            InitializeComponent();
            _controller = new UserController();
            _adminUser = adminUser;
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            LoadRolesDropdown();
            LoadUsersGrid();
            ResetInputFields(); // Set the initial state of the input fields.
        }

        private void LoadRolesDropdown()
        {
            // The order here will be the order in the dropdown.
            var roles = new List<string> { "Admin", "Staff", "Lecturer", "Student" };
            cmbRole.DataSource = roles;
        }

        private void LoadUsersGrid()
        {
            // Store the currently selected row's ID if it exists
            int? selectedUserId = dgvUsers.CurrentRow?.Cells["UserID"].Value as int?;

            dgvUsers.DataSource = _controller.GetAllUsers();
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // --- IMPORTANT CHANGES HERE ---
            if (dgvUsers.Columns["Password"] != null)
            {
                dgvUsers.Columns["Password"].Visible = false; // Always hide password hash
            }
            if (dgvUsers.Columns["UserID"] != null)
            {
                dgvUsers.Columns["UserID"].Visible = true; // <<<<< MAKE THE ID VISIBLE
                dgvUsers.Columns["UserID"].HeaderText = "ID";
                dgvUsers.Columns["UserID"].Width = 50;
                dgvUsers.Columns["UserID"].ReadOnly = true; // Prevent users from editing the ID in the grid
            }

            // Re-select the row that was selected before the refresh
            if (selectedUserId.HasValue)
            {
                foreach (DataGridViewRow row in dgvUsers.Rows)
                {
                    if (row.Cells["UserID"].Value as int? == selectedUserId)
                    {
                        row.Selected = true;
                        dgvUsers.CurrentCell = row.Cells[1]; // Select the username cell
                        break;
                    }
                }
            }
        }

        private void ResetInputFields()
        {
            txtUsername.Clear();
            cmbRole.SelectedIndex = -1; // Deselect any role
            txtPassword.Text = _passwordPlaceholder;
            txtPassword.ForeColor = Color.Gray;
            dgvUsers.ClearSelection();
        }

        private void dgvUsers_CellClick(object sender, EventArgs e)
        {
            // Use SelectionChanged instead of CellClick for better reliability
            if (dgvUsers.CurrentRow == null || dgvUsers.CurrentRow.DataBoundItem == null)
            {
                return;
            }

            // Get the User object directly from the selected row
            User selectedUser = (User)dgvUsers.CurrentRow.DataBoundItem;

            txtUsername.Text = selectedUser.Username;
            cmbRole.SelectedItem = selectedUser.Role;

            // Reset password field to placeholder for security
            txtPassword.Text = _passwordPlaceholder;
            txtPassword.ForeColor = Color.Gray;
        }

        // --- Placeholder Text Event Handlers ---
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == _passwordPlaceholder)
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ResetPasswordBox();
            }
        }

        private void ResetPasswordBox()
        {
            txtPassword.Text = _passwordPlaceholder;
            txtPassword.ForeColor = Color.Gray;
        }

        // --- Button Click Methods ---
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text == _passwordPlaceholder)
            {
                MessageBox.Show("Username and a valid Password cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role for the user.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _controller.AddUser(txtUsername.Text, txtPassword.Text, cmbRole.SelectedItem.ToString());
            MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUsersGrid();
            ResetInputFields();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user from the grid to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Username and Role cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);

            // If the password box still has the placeholder, pass an empty string to the controller.
            // This tells the DatabaseManager to NOT update the password.
            string newPassword = (txtPassword.Text == _passwordPlaceholder) ? "" : txtPassword.Text;

            _controller.UpdateUser(userId, txtUsername.Text, newPassword, cmbRole.SelectedItem.ToString());
            MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUsersGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user to delete.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userIdToDelete = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);

            if (userIdToDelete == _adminUser.UserID)
            {
                MessageBox.Show("You cannot delete your own account while you are logged in.", "Action Forbidden", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirmResult = MessageBox.Show("Are you sure you want to delete this user permanently?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                _controller.DeleteUser(userIdToDelete);
                LoadUsersGrid();
                ResetInputFields();
            }
        }
    }
}