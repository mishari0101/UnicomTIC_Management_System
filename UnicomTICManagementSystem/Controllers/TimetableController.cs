using System.Collections.Generic;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class TimetableController
    {
        public List<Timetable> GetAllTimetableEntries()
        {
            return DatabaseManager.GetAllTimetableEntries();
        }

        // Methods to populate our dropdown menus
        public List<Subject> GetAllSubjects()
        {
            return DatabaseManager.GetAllSubjects();
        }

        public List<Room> GetAllRooms()
        {
            return DatabaseManager.GetAllRooms();
        }

        // Methods for admin actions
        public void AddEntry(string timeSlot, int subjectId, int roomId)
        {
            DatabaseManager.AddTimetableEntry(timeSlot, subjectId, roomId);
        }

        public void UpdateEntry(int id, string timeSlot, int subjectId, int roomId)
        {
            DatabaseManager.UpdateTimetableEntry(id, timeSlot, subjectId, roomId);
        }

        public void DeleteEntry(int id)
        {
            DatabaseManager.DeleteTimetableEntry(id);
        }
    }
}