using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class RoomController
    {
        public List<Room> GetAllRooms()
        {
            try { return DatabaseManager.GetAllRooms(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching rooms: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Room>();
            }
        }

        public void AddRoom(string name, string type)
        {
            try { DatabaseManager.AddRoom(name, type); }
            catch (Exception ex) { MessageBox.Show("Error adding room: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void UpdateRoom(int id, string name, string type)
        {
            try { DatabaseManager.UpdateRoom(id, name, type); }
            catch (Exception ex) { MessageBox.Show("Error updating room: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void DeleteRoom(int id)
        {
            try { DatabaseManager.DeleteRoom(id); }
            catch (Exception ex) { MessageBox.Show("Error deleting room: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}