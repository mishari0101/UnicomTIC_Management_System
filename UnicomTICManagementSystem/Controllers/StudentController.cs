using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class StudentController
    {
        public List<Student> GetAllStudents()
        {
            try { return DatabaseManager.GetAllStudents(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching students: " + ex.Message, "Database Error");
                return new List<Student>();
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

        public void AddStudent(string studentName, int courseId)
        {
            try { DatabaseManager.AddStudent(studentName, courseId); }
            catch (Exception ex) { MessageBox.Show("Error adding student: " + ex.Message, "Database Error"); }
        }

        public void UpdateStudent(int studentId, string newName, int newCourseId)
        {
            try { DatabaseManager.UpdateStudent(studentId, newName, newCourseId); }
            catch (Exception ex) { MessageBox.Show("Error updating student: " + ex.Message, "Database Error"); }
        }

        public void DeleteStudent(int studentId)
        {
            try { DatabaseManager.DeleteStudent(studentId); }
            catch (Exception ex) { MessageBox.Show("Error deleting student: " + ex.Message, "Database Error"); }
        }
    }
}