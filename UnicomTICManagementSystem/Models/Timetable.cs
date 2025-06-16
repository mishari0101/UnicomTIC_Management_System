using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class Timetable
    {
        public int TimetableID { get; set; }
        public string TimeSlot { get; set; } // e.g., "Monday 9:00 AM - 11:00 AM"
        public int SubjectID { get; set; }
        public int RoomID { get; set; }

        // --- Helper properties for display ---
        public string SubjectName { get; set; }
        public string RoomName { get; set; }
    }
}
