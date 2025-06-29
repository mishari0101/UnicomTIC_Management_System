using System;
using System.Windows.Forms;
using UnicomTICManagementSystem.Helpers; // Make sure this is here
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class LoginController
    {
        public User AuthenticateUser(string username, string password)
        {
            try
            {
                string providedPasswordHash = PasswordHelper.HashPassword(password);
                // The actual authentication logic happens in the repository now, so we just call it.
                // To keep the pattern clean, we should ideally have a method in DatabaseManager
                // like GetUserByUsernameAndPasswordHash. For now, we'll assume the old way is there.

                // Let's create a dedicated method in DatabaseManager to keep logic separate
                return DatabaseManager.GetUserByCredentials(username, providedPasswordHash);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during login: " + ex.Message, "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null; // Return null to indicate login failure
            }
        }
    }
}