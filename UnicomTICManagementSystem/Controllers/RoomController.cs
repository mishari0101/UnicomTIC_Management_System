using System.Collections.Generic;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class RoomController
    {
        public List<Room> GetAllRooms()
        {
            return DatabaseManager.GetAllRooms();
        }

        public void AddRoom(string name, string type)
        {
            DatabaseManager.AddRoom(name, type);
        }

        public void UpdateRoom(int id, string name, string type)
        {
            DatabaseManager.UpdateRoom(id, name, type);
        }

        public void DeleteRoom(int id)
        {
            DatabaseManager.DeleteRoom(id);
        }
    }
}