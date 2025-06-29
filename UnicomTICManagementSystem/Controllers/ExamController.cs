using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;

namespace UnicomTICManagementSystem.Controllers
{
    public class ExamController
    {
        public List<Exam> GetAllExams()
        {
            try { return DatabaseManager.GetAllExams(); }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching exams: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Exam>();
            }
        }

        public void AddExam(string examName, int subjectId)
        {
            try { DatabaseManager.AddExam(examName, subjectId); }
            catch (Exception ex) { MessageBox.Show("Error adding exam: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public List<Mark> GetMarksForExam(int examId)
        {
            try { return DatabaseManager.GetMarksForExam(examId); }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching marks for exam: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Mark>();
            }
        }

        public void SaveMark(int studentId, int examId, int score)
        {
            try { DatabaseManager.SaveOrUpdateMark(studentId, examId, score); }
            catch (Exception ex) { MessageBox.Show("Error saving mark: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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
    }
}