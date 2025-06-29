using System;
using System.Collections.Generic;
using System.Drawing; // We need this for using the 'Color' class
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Views
{
    public partial class UserForm : Form
    {
        private readonly UserController _controller;
        private readonly User _adminUser;
        private readonly string passwordPlaceholder = "Enter new password to change";

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
            ResetPasswordBox(); // Set the initial state of the password box.
        }

        private void LoadRolesDropdown()
        {
            var roles = new List<string> { "Admin", "Student", "Staff", "Lecturer" };
            cmbRole.DataSource = roles;
        }

        private void LoadUsersGrid()
        {
            dgvUsers.DataSource = _controller.GetAllUsers();
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvUsers.Columns["Password"] != null) dgvUsers.Columns["Password"].Visible = false;
            if (dgvUsers.Columns["UserID"] != null) dgvUsers.Columns["UserID"].Visible = false;
        }

        // This is our new, simple method to reset the password box.
        private void ResetPasswordBox()
        {
            txtPassword.Text = passwordPlaceholder;
            txtPassword.ForeColor = Color.Gray;
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string roleFromGrid = dgvUsers.CurrentRow.Cells["Role"].Value.ToString();
            MessageBox.Show($"The role from the grid is: '{roleFromGrid}'");
            // -----------------------------

            if (e.RowIndex >= 0 && dgvUsers.CurrentRow != null)
            {
                txtUsername.Text = dgvUsers.CurrentRow.Cells["Username"].Value.ToString();

                // This is the line that is failing.
                cmbRole.SelectedItem = roleFromGrid;

                ResetPasswordBox();
            }
        }

        // --- Placeholder Event Handlers ---

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == passwordPlaceholder)
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            // If the user clicks out and the box is empty, reset it.
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ResetPasswordBox();
            }
        }

        // --- Button Click Methods ---

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // --- VALIDATION START ---
            // Check for empty username or password, AND check if the password is still the placeholder.
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text == passwordPlaceholder)
            {
                MessageBox.Show("Username and a valid Password cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role for the user.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // --- VALIDATION END ---

            _controller.AddUser(txtUsername.Text, txtPassword.Text, cmbRole.SelectedItem.ToString());
            MessageBox.Show("User added successfully!");
            LoadUsersGrid();
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user from the grid to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- VALIDATION START ---
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text == passwordPlaceholder)
            {
                MessageBox.Show("Username and a new Password are required for an update.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role for the user.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // --- VALIDATION END ---

            int userId = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);
            _controller.UpdateUser(userId, txtUsername.Text, txtPassword.Text, cmbRole.SelectedItem.ToString());
            MessageBox.Show("User updated successfully!");
            LoadUsersGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            int userIdToDelete = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);
            if (userIdToDelete == _adminUser.UserID)
            {
                MessageBox.Show("You cannot delete your own account while you are logged in.", "Action Forbidden", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var confirm = MessageBox.Show("Are you sure you want to delete this user permanently?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                _controller.DeleteUser(userIdToDelete);
                LoadUsersGrid();
            }
        }

        // This can be left empty or deleted.
        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}