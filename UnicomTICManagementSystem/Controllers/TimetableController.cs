using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class TimetableController
    {
        public List<Timetable> GetAllTimetableEntries()
        {
            try { return DatabaseManager.GetAllTimetableEntries(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching timetable entries: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Timetable>();
            }
        }

        public List<Subject> GetAllSubjects()
        {
            try { return DatabaseManager.GetAllSubjects(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching subjects: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Subject>();
            }
        }

        public List<Room> GetAllRooms()
        {
            try { return DatabaseManager.GetAllRooms(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching rooms: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Room>();
            }
        }

        public void AddEntry(string timeSlot, int subjectId, int roomId)
        {
            try { DatabaseManager.AddTimetableEntry(timeSlot, subjectId, roomId); }
            catch (Exception ex) { MessageBox.Show("Error adding timetable entry: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void UpdateEntry(int id, string timeSlot, int subjectId, int roomId)
        {
            try { DatabaseManager.UpdateTimetableEntry(id, timeSlot, subjectId, roomId); }
            catch (Exception ex) { MessageBox.Show("Error updating timetable entry: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void DeleteEntry(int id)
        {
            try { DatabaseManager.DeleteTimetableEntry(id); }
            catch (Exception ex) { MessageBox.Show("Error deleting timetable entry: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}