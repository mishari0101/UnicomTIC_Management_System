using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class Mark
    {
        public int MarkID { get; set; } // The ID of the mark entry itself
        public int StudentID { get; set; }
        public string StudentName { get; set; } // Helper property
        public int ExamID { get; set; }

        // The score. We use 'int?' (a nullable integer) so it can be empty if a mark hasn't been entered yet.
        public int? Score { get; set; }
    }
}
