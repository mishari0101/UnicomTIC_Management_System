using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public int CourseID { get; set; } // The ID of the course the student is enrolled in

        // This property is for display purposes only. It's not a column in the Students table.
        public string CourseName { get; set; }
    }
}
