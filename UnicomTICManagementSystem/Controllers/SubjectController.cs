using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class SubjectController
    {
        public List<Subject> GetAllSubjects()
        {
            try { return DatabaseManager.GetAllSubjects(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching subjects: " + ex.Message, "Database Error");
                return new List<Subject>();
            }
        }

        public List<Course> GetAllCourses()
        {
            try { return DatabaseManager.GetAllCourses(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching courses: " + ex.Message, "Database Error");
                return new List<Course>();
            }
        }

        public void AddSubject(string subjectName, int courseId)
        {
            try { DatabaseManager.AddSubject(subjectName, courseId); }
            catch (Exception ex) { MessageBox.Show("Error adding subject: " + ex.Message, "Database Error"); }
        }

        public void UpdateSubject(int subjectId, string newName, int newCourseId)
        {
            try { DatabaseManager.UpdateSubject(subjectId, newName, newCourseId); }
            catch (Exception ex) { MessageBox.Show("Error updating subject: " + ex.Message, "Database Error"); }
        }

        public void DeleteSubject(int subjectId)
        {
            try { DatabaseManager.DeleteSubject(subjectId); }
            catch (Exception ex) { MessageBox.Show("Error deleting subject: " + ex.Message, "Database Error"); }
        }
    }
}