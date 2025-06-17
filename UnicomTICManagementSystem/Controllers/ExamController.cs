using System.Collections.Generic;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class ExamController
    {
        // --- Methods for managing Exams ---

        public List<Exam> GetAllExams()
        {
            return DatabaseManager.GetAllExams();
        }

        public void AddExam(string examName, int subjectId)
        {
            DatabaseManager.AddExam(examName, subjectId);
        }

        // --- Methods for managing Marks ---

        // Gets the list of students and their scores for a selected exam
        public List<Mark> GetMarksForExam(int examId)
        {
            return DatabaseManager.GetMarksForExam(examId);
        }

        // Tells the database to save a student's score
        public void SaveMark(int studentId, int examId, int score)
        {
            DatabaseManager.SaveOrUpdateMark(studentId, examId, score);
        }

        // --- Helper methods to get data for dropdown menus ---

        public List<Subject> GetAllSubjects()
        {
            return DatabaseManager.GetAllSubjects();
        }
    }
}
