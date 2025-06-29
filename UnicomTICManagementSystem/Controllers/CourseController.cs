using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class CourseController
    {
        // This method asks the DatabaseManager for all courses and returns them.
        public List<Course> GetAllCourses()
        {
            try
            {
                return DatabaseManager.GetAllCourses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching courses: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Course>(); // Return an empty list to prevent a crash
            }
        }

        // This method tells the DatabaseManager to add a new course.
        // In a more complex app, you could add validation here (e.g., check if the name is too short).
        public void AddCourse(string courseName)
        {
            try
            {
                DatabaseManager.AddCourse(courseName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding the course: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // This method tells the DatabaseManager to update a course.
        public void UpdateCourse(int courseId, string newCourseName)
        {
            try
            {
                DatabaseManager.UpdateCourse(courseId, newCourseName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the course: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // This method tells the DatabaseManager to delete a course.
        public void DeleteCourse(int courseId)
        {
            try
            {
                DatabaseManager.DeleteCourse(courseId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the course: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
