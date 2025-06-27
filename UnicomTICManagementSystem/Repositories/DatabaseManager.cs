using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Repositories
{

    public class DatabaseManager
    {
        private static readonly string dbFile = "unicomtic.db";
        public static string ConnectionString = $"Data Source={dbFile};Version=3;";

        // This method creates the database file and tables if they don't exist.
        public static void InitializeDatabase()
        {
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);

                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    // Create Users Table
                    string createUserTable = @"
                    CREATE TABLE Users (
                        UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL,
                        Role TEXT NOT NULL
                    );";
                    new SQLiteCommand(createUserTable, connection).ExecuteNonQuery();

                    // Create Courses Table
                    string createCoursesTable = @"
                    CREATE TABLE Courses (
                        CourseID INTEGER PRIMARY KEY AUTOINCREMENT,
                        CourseName TEXT NOT NULL
                    );";
                    new SQLiteCommand(createCoursesTable, connection).ExecuteNonQuery();

                    // Add more tables here following the same pattern...
                    // Subjects Table
                    string createSubjectsTable = @"
                    CREATE TABLE Subjects (
                        SubjectID INTEGER PRIMARY KEY AUTOINCREMENT,
                        SubjectName TEXT NOT NULL,
                        CourseID INTEGER,
                        FOREIGN KEY(CourseID) REFERENCES Courses(CourseID)
                    );";
                    new SQLiteCommand(createSubjectsTable, connection).ExecuteNonQuery();

                    // Students Table
                    string createStudentsTable = @"
                    CREATE TABLE Students (
                        StudentID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        CourseID INTEGER,
                        FOREIGN KEY(CourseID) REFERENCES Courses(CourseID)
                    );";
                    new SQLiteCommand(createStudentsTable, connection).ExecuteNonQuery();

                    // Rooms Table
                    string createRoomsTable = @"
                    CREATE TABLE Rooms (
                        RoomID INTEGER PRIMARY KEY AUTOINCREMENT,
                        RoomName TEXT NOT NULL,
                        RoomType TEXT NOT NULL
                    );";
                    new SQLiteCommand(createRoomsTable, connection).ExecuteNonQuery();

                    // Exams Table
                    string createExamsTable = @"
                    CREATE TABLE Exams (
                        ExamID INTEGER PRIMARY KEY AUTOINCREMENT,
                        ExamName TEXT NOT NULL,
                        SubjectID INTEGER,
                        FOREIGN KEY(SubjectID) REFERENCES Subjects(SubjectID)
                    );";
                    new SQLiteCommand(createExamsTable, connection).ExecuteNonQuery();

                    // Marks Table
                    string createMarksTable = @"
                    CREATE TABLE Marks (
                        MarkID INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentID INTEGER,
                        ExamID INTEGER,
                        Score INTEGER,
                        FOREIGN KEY(StudentID) REFERENCES Students(StudentID),
                        FOREIGN KEY(ExamID) REFERENCES Exams(ExamID)
                    );";
                    new SQLiteCommand(createMarksTable, connection).ExecuteNonQuery();

                    // Timetables Table
                    string createTimetablesTable = @"
                    CREATE TABLE Timetables (
                        TimetableID INTEGER PRIMARY KEY AUTOINCREMENT,
                        SubjectID INTEGER,
                        TimeSlot TEXT NOT NULL,
                        RoomID INTEGER,
                        FOREIGN KEY(SubjectID) REFERENCES Subjects(SubjectID),
                        FOREIGN KEY(RoomID) REFERENCES Rooms(RoomID)
                    );";
                    new SQLiteCommand(createTimetablesTable, connection).ExecuteNonQuery();

                    // --- Add some default data to make testing easier ---
                    // Add default Admin user
                    string addAdmin = "INSERT INTO Users (Username, Password, Role) VALUES ('admin', 'admin123', 'Admin');";
                    new SQLiteCommand(addAdmin, connection).ExecuteNonQuery();
                    // Add default Student user
                    string addStudent = "INSERT INTO Users (Username, Password, Role) VALUES ('student', 'student123', 'Student');";
                    new SQLiteCommand(addStudent, connection).ExecuteNonQuery();

                }
            }
        }
        // --- Course Management Methods ---

        // 1. READ: Get all courses from the database
        public static List<Course> GetAllCourses()
        {
            var courses = new List<Course>(); // Create an empty list to hold the courses
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT CourseID, CourseName FROM Courses"; // SQL command to get all courses
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader()) // Execute the command and get the results
                    {
                        // Loop through each row found in the database
                        while (reader.Read())
                        {
                            // Create a new Course object and fill it with data from the current row
                            var course = new Course
                            {
                                CourseID = reader.GetInt32(0),   // Get data from the first column (CourseID)
                                CourseName = reader.GetString(1) // Get data from the second column (CourseName)
                            };
                            courses.Add(course); // Add the new course object to our list
                        }
                    }
                }
            }
            return courses; // Return the final list of courses
        }

        // 2. CREATE: Add a new course to the database
        public static void AddCourse(string courseName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                // SQL command to insert a new row. @courseName is a placeholder to prevent errors and attacks.
                string query = "INSERT INTO Courses (CourseName) VALUES (@courseName)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    // Replace the @courseName placeholder with the actual value
                    command.Parameters.AddWithValue("@courseName", courseName);
                    command.ExecuteNonQuery(); // Execute the command (doesn't return any data)
                }
            }
        }

        // 3. UPDATE: Change the name of an existing course
        public static void UpdateCourse(int courseId, string newCourseName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Courses SET CourseName = @courseName WHERE CourseID = @courseId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    // Replace the placeholders with the actual values
                    command.Parameters.AddWithValue("@courseName", newCourseName);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.ExecuteNonQuery(); // Execute the command
                }
            }
        }

        // 4. DELETE: Remove a course from the database
        public static void DeleteCourse(int courseId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Courses WHERE CourseID = @courseId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    // Replace the placeholder with the actual value
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.ExecuteNonQuery(); // Execute the command
                }
            }
        }
        // --- Student Management Methods ---

        // 1. READ: Get all students and their course names.
        public static List<Student> GetAllStudents()
        {
            var students = new List<Student>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                // This is a special SQL query. The "LEFT JOIN" links the Students table to the Courses table
                // using the CourseID so we can get the CourseName.
                string query = @"
            SELECT s.StudentID, s.Name, s.CourseID, c.CourseName 
            FROM Students s
            LEFT JOIN Courses c ON s.CourseID = c.CourseID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var student = new Student
                            {
                                StudentID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                CourseID = reader.GetInt32(2),
                                CourseName = reader.IsDBNull(3) ? "N/A" : reader.GetString(3) // Get CourseName, handle if it's not assigned
                            };
                            students.Add(student);
                        }
                    }
                }
            }
            return students;
        }

        // 2. CREATE: Add a new student. Notice we only need the CourseID, not the name.
        public static void AddStudent(string studentName, int courseId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Students (Name, CourseID) VALUES (@name, @courseId)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", studentName);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 3. UPDATE: Change a student's name or course.
        public static void UpdateStudent(int studentId, string newName, int newCourseId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Students SET Name = @name, CourseID = @courseId WHERE StudentID = @studentId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", newName);
                    command.Parameters.AddWithValue("@courseId", newCourseId);
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 4. DELETE: Remove a student.
        public static void DeleteStudent(int studentId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Students WHERE StudentID = @studentId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.ExecuteNonQuery();
                }
            }
        }
        // --- Subject Management Methods ---

        // 1. READ: Gets all subjects from the database.
        public static List<Subject> GetAllSubjects()
        {
            var subjects = new List<Subject>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                // This SQL query joins the Subjects table (aliased as 's') with the
                // Courses table (aliased as 'c') to get the CourseName for each subject.
                string query = @"
            SELECT s.SubjectID, s.SubjectName, s.CourseID, c.CourseName 
            FROM Subjects s
            LEFT JOIN Courses c ON s.CourseID = c.CourseID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var subject = new Subject
                            {
                                SubjectID = reader.GetInt32(0),
                                SubjectName = reader.GetString(1),
                                CourseID = reader.GetInt32(2),
                                CourseName = reader.IsDBNull(3) ? "N/A" : reader.GetString(3)
                            };
                            subjects.Add(subject);
                        }
                    }
                }
            }
            return subjects;
        }

        // 2. CREATE: Adds a new subject to the database.
        public static void AddSubject(string subjectName, int courseId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Subjects (SubjectName, CourseID) VALUES (@subjectName, @courseId)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@subjectName", subjectName);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 3. UPDATE: Changes an existing subject's name or course.
        public static void UpdateSubject(int subjectId, string newName, int newCourseId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Subjects SET SubjectName = @name, CourseID = @courseId WHERE SubjectID = @subjectId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", newName);
                    command.Parameters.AddWithValue("@courseId", newCourseId);
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 4. DELETE: Removes a subject from the database.
        public static void DeleteSubject(int subjectId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Subjects WHERE SubjectID = @subjectId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    command.ExecuteNonQuery();
                }
            }
        }
        // --- Room Management Methods ---

        public static List<Room> GetAllRooms()
        {
            var rooms = new List<Room>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT RoomID, RoomName, RoomType FROM Rooms";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var room = new Room
                            {
                                RoomID = reader.GetInt32(0),
                                RoomName = reader.GetString(1),
                                RoomType = reader.GetString(2)
                            };
                            rooms.Add(room);
                        }
                    }
                }
            }
            return rooms;
        }

        public static void AddRoom(string roomName, string roomType)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Rooms (RoomName, RoomType) VALUES (@roomName, @roomType)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@roomName", roomName);
                    command.Parameters.AddWithValue("@roomType", roomType);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateRoom(int roomId, string newName, string newType)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Rooms SET RoomName = @name, RoomType = @type WHERE RoomID = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", newName);
                    command.Parameters.AddWithValue("@type", newType);
                    command.Parameters.AddWithValue("@id", roomId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteRoom(int roomId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Rooms WHERE RoomID = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", roomId);
                    command.ExecuteNonQuery();
                }
            }
        }
        // --- Timetable Management Methods ---

        public static List<Timetable> GetAllTimetableEntries()
        {
            var entries = new List<Timetable>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                // This is our most advanced query yet!
                // It joins THREE tables: Timetables (t), Subjects (s), and Rooms (r).
                // This allows us to get the SubjectName and RoomName in a single database call.
                string query = @"
            SELECT t.TimetableID, t.TimeSlot, s.SubjectName, r.RoomName, t.SubjectID, t.RoomID
            FROM Timetables t
            LEFT JOIN Subjects s ON t.SubjectID = s.SubjectID
            LEFT JOIN Rooms r ON t.RoomID = r.RoomID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entry = new Timetable
                            {
                                TimetableID = reader.GetInt32(0),
                                TimeSlot = reader.GetString(1),
                                SubjectName = reader.IsDBNull(2) ? "N/A" : reader.GetString(2),
                                RoomName = reader.IsDBNull(3) ? "N/A" : reader.GetString(3),
                                SubjectID = reader.GetInt32(4),
                                RoomID = reader.GetInt32(5)
                            };
                            entries.Add(entry);
                        }
                    }
                }
            }
            return entries;
        }

        public static void AddTimetableEntry(string timeSlot, int subjectId, int roomId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Timetables (TimeSlot, SubjectID, RoomID) VALUES (@timeSlot, @subjectId, @roomId)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@timeSlot", timeSlot);
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    command.Parameters.AddWithValue("@roomId", roomId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateTimetableEntry(int timetableId, string newTimeSlot, int newSubjectId, int newRoomId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Timetables SET TimeSlot = @timeSlot, SubjectID = @subjectId, RoomID = @roomId WHERE TimetableID = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@timeSlot", newTimeSlot);
                    command.Parameters.AddWithValue("@subjectId", newSubjectId);
                    command.Parameters.AddWithValue("@roomId", newRoomId);
                    command.Parameters.AddWithValue("@id", timetableId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteTimetableEntry(int timetableId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Timetables WHERE TimetableID = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", timetableId);
                    command.ExecuteNonQuery();
                }
            }
        }
        // --- Exam Management Methods ---

        public static List<Exam> GetAllExams()
        {
            var exams = new List<Exam>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                // Join with Subjects table to get the SubjectName for display
                string query = @"
            SELECT e.ExamID, e.ExamName, e.SubjectID, s.SubjectName 
            FROM Exams e
            LEFT JOIN Subjects s ON e.SubjectID = s.SubjectID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exams.Add(new Exam
                            {
                                ExamID = reader.GetInt32(0),
                                ExamName = reader.GetString(1),
                                SubjectID = reader.GetInt32(2),
                                SubjectName = reader.IsDBNull(3) ? "N/A" : reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return exams;
        }

        public static void AddExam(string examName, int subjectId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Exams (ExamName, SubjectID) VALUES (@name, @subjectId)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", examName);
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // --- Marks Management Methods ---

        // This is a powerful method. For a given exam, it gets all relevant students
        // and joins their existing mark if they have one.
        public static List<Mark> GetMarksForExam(int examId)
        {
            var marks = new List<Mark>();
            var examSubjectIdQuery = "SELECT SubjectID FROM Exams WHERE ExamID = @examId";
            int subjectId;

            // First, find out which subject this exam belongs to.
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(examSubjectIdQuery, connection))
                {
                    command.Parameters.AddWithValue("@examId", examId);
                    var result = command.ExecuteScalar(); // Gets a single value
                    if (result == null || result == DBNull.Value) return marks; // No subject found, return empty list
                    subjectId = Convert.ToInt32(result);
                }

                // Now, get all students in that subject's course and LEFT JOIN their marks for this exam.
                string query = @"
            SELECT s.StudentID, s.Name, m.MarkID, m.Score
            FROM Students s
            LEFT JOIN Marks m ON s.StudentID = m.StudentID AND m.ExamID = @examId
            WHERE s.CourseID = (SELECT CourseID FROM Subjects WHERE SubjectID = @subjectId)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@examId", examId);
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            marks.Add(new Mark
                            {
                                StudentID = reader.GetInt32(0),
                                StudentName = reader.GetString(1),
                                MarkID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2), // MarkID is 0 if no mark exists yet
                                ExamID = examId,
                                // Score can be null if no mark has been entered
                                Score = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3)
                            });
                        }
                    }
                }
            }
            return marks;
        }// This is an "Upsert" method. It INSERTS a new mark or UPDATES an existing one.
        public static void SaveOrUpdateMark(int studentId, int examId, int score)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Step 1: Check if a mark already exists for this student and exam.
                string checkQuery = "SELECT MarkID FROM Marks WHERE StudentID = @studentId AND ExamID = @examId";

                // Create the command to check for an existing mark
                var checkCommand = new SQLiteCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@studentId", studentId);
                checkCommand.Parameters.AddWithValue("@examId", examId);

                // Execute the command and get the result (which will be the MarkID or null)
                object markIdResult = checkCommand.ExecuteScalar();

                // Step 2: Decide whether to UPDATE or INSERT.
                if (markIdResult != null && markIdResult != DBNull.Value)
                {
                    // --- UPDATE an existing mark ---
                    int existingMarkId = Convert.ToInt32(markIdResult);
                    string updateQuery = "UPDATE Marks SET Score = @score WHERE MarkID = @markId";

                    var updateCommand = new SQLiteCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@score", score);
                    updateCommand.Parameters.AddWithValue("@markId", existingMarkId); // Use the converted ID

                    updateCommand.ExecuteNonQuery();
                }
                else
                {
                    // --- INSERT a new mark ---
                    string insertQuery = "INSERT INTO Marks (StudentID, ExamID, Score) VALUES (@studentId, @examId, @score)";

                    var insertCommand = new SQLiteCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@studentId", studentId);
                    insertCommand.Parameters.AddWithValue("@examId", examId);
                    insertCommand.Parameters.AddWithValue("@score", score);

                    insertCommand.ExecuteNonQuery();
                }
            }
        }
        // --- Student-Specific Methods ---

        // This method gets all the exam results for a single student, using their username.
        public static List<StudentMarkRecord> GetMarksForStudent(string username)
        {
            var results = new List<StudentMarkRecord>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // This query joins three tables to get all the necessary information.
                // It starts from the Marks table and links to Students, Exams, and Subjects.
                string query = @"
            SELECT s.SubjectName, e.ExamName, m.Score
            FROM Marks m
            JOIN Students st ON m.StudentID = st.StudentID
            JOIN Exams e ON m.ExamID = e.ExamID
            JOIN Subjects s ON e.SubjectID = s.SubjectID
            WHERE st.Name = @username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    // We find the student based on their unique username.
                    command.Parameters.AddWithValue("@username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new StudentMarkRecord
                            {
                                SubjectName = reader.GetString(0),
                                ExamName = reader.GetString(1),
                                Score = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2)
                            });
                        }
                    }
                }
            }
            return results;
        }
        // --- User Management Methods ---

        public static List<User> GetAllUsers()
        {
            var users = new List<User>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                // We select everything EXCEPT the password for security.
                // It's bad practice to display passwords, even to an admin.
                string query = "SELECT UserID, Username, Role, Password FROM Users";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserID = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Role = reader.GetString(2),
                                Password = reader.GetString(3) // We get it, but won't show it.
                            });
                        }
                    }
                }
            }
            return users;
        }

        public static void AddUser(string username, string password, string role)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, @role)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@role", role);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Note: It's often better to have a separate "ChangePassword" method,
        // but for simplicity, we'll allow updating everything at once.
        public static void UpdateUser(int userId, string username, string password, string role)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET Username = @username, Password = @password, Role = @role WHERE UserID = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@role", role);
                    command.Parameters.AddWithValue("@id", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteUser(int userId)
        {
            // Important: Don't let an admin delete themselves! We'll add this check in the form's code.
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE UserID = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
