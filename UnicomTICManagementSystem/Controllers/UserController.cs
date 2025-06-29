using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnicomTICManagementSystem.Helpers; // This line now works!
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class UserController
    {
        public List<User> GetAllUsers()
        {
            try { return DatabaseManager.GetAllUsers(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching users: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<User>();
            }
        }

        // This method now uses the PasswordHelper before calling the database.
        public void AddUser(string username, string password, string role)
        {
            try
            {
                // Hash the plain-text password from the form.
                string passwordHash = PasswordHelper.HashPassword(password);
                // Send the HASH to the database manager.
                DatabaseManager.AddUser(username, passwordHash, role);
            }
            catch (Exception ex) { MessageBox.Show("Error adding user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // This method also uses the PasswordHelper.
        public void UpdateUser(int id, string username, string password, string role)
        {
            try
            {
                // Hash the new plain-text password from the form.
                string passwordHash = PasswordHelper.HashPassword(password);
                // Send the new HASH to the database manager.
                DatabaseManager.UpdateUser(id, username, passwordHash, role);
            }
            catch (Exception ex) { MessageBox.Show("Error updating user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void DeleteUser(int id)
        {
            try { DatabaseManager.DeleteUser(id); }
            catch (Exception ex) { MessageBox.Show("Error deleting user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}