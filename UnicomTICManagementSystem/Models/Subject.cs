using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class Subject
    {
        // The unique ID for each subject in the database table.
        public int SubjectID { get; set; }

        // The name of the subject, like "Introduction to Programming".
        public string SubjectName { get; set; }

        // This is the 'foreign key'. It links this subject to a specific course.
        public int CourseID { get; set; }

        // This is a helper property for display purposes only. It is NOT in the
        // database table. We will fill this ourselves using a special SQL query.
        public string CourseName { get; set; }
    }
}
