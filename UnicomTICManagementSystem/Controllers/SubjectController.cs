using System.Collections.Generic;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class SubjectController
    {
        // This method asks the DatabaseManager for all subjects.
        public List<Subject> GetAllSubjects()
        {
            return DatabaseManager.GetAllSubjects();
        }

        // We need this method again to fill our 'Course' dropdown menu on the form.
        public List<Course> GetAllCourses()
        {
            return DatabaseManager.GetAllCourses();
        }

        // This method tells the DatabaseManager to add a subject.
        public void AddSubject(string subjectName, int courseId)
        {
            DatabaseManager.AddSubject(subjectName, courseId);
        }

        // This method tells the DatabaseManager to update a subject.
        public void UpdateSubject(int subjectId, string newName, int newCourseId)
        {
            DatabaseManager.UpdateSubject(subjectId, newName, newCourseId);
        }

        // This method tells the DatabaseManager to delete a subject.
        public void DeleteSubject(int subjectId)
        {
            DatabaseManager.DeleteSubject(subjectId);
        }
    }
}