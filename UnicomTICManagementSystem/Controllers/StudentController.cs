using System.Collections.Generic;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class StudentController
    {
        public List<Student> GetAllStudents()
        {
            return DatabaseManager.GetAllStudents();
        }

        // We need a way to get all courses to populate our dropdown menu.
        public List<Course> GetAllCourses()
        {
            return DatabaseManager.GetAllCourses();
        }

        public void AddStudent(string studentName, int courseId)
        {
            DatabaseManager.AddStudent(studentName, courseId);
        }

        public void UpdateStudent(int studentId, string newName, int newCourseId)
        {
            DatabaseManager.UpdateStudent(studentId, newName, newCourseId);
        }

        public void DeleteStudent(int studentId)
        {
            DatabaseManager.DeleteStudent(studentId);
        }
    }
}
