using System.Collections.Generic;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class CourseController
    {
        // This method asks the DatabaseManager for all courses and returns them.
        public List<Course> GetAllCourses()
        {
            return DatabaseManager.GetAllCourses();
        }

        // This method tells the DatabaseManager to add a new course.
        // In a more complex app, you could add validation here (e.g., check if the name is too short).
        public void AddCourse(string courseName)
        {
            DatabaseManager.AddCourse(courseName);
        }

        // This method tells the DatabaseManager to update a course.
        public void UpdateCourse(int courseId, string newCourseName)
        {
            DatabaseManager.UpdateCourse(courseId, newCourseName);
        }

        // This method tells the DatabaseManager to delete a course.
        public void DeleteCourse(int courseId)
        {
            DatabaseManager.DeleteCourse(courseId);
        }
    }
}
