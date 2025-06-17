using System.Collections.Generic;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class UserController
    {
        public List<User> GetAllUsers()
        {
            return DatabaseManager.GetAllUsers();
        }

        public void AddUser(string username, string password, string role)
        {
            DatabaseManager.AddUser(username, password, role);
        }

        public void UpdateUser(int id, string username, string password, string role)
        {
            DatabaseManager.UpdateUser(id, username, password, role);
        }

        public void DeleteUser(int id)
        {
            DatabaseManager.DeleteUser(id);
        }
    }
}