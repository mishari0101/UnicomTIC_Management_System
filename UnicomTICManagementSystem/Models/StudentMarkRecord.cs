using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    // This class is a "ViewModel". It's specifically designed to hold data
    // for one particular view (the student's marks grid).
    public class StudentMarkRecord
    {
        public string SubjectName { get; set; }
        public string ExamName { get; set; }
        public int? Score { get; set; } // The score can be null if not yet entered.
    }
}
