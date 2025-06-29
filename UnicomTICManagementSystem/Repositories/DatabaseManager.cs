using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using UnicomTICManagementSystem.Helpers; // For PasswordHelper
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Repositories
{
    public class DatabaseManager
    {

        private static readonly string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "Database", "unicomtic.db");
        //private static readonly string dbPath = "";
        public static string ConnectionString = $"Data Source={dbPath};Version=3;";

        public static void InitializeDatabase()
        {
            try
            {
                string dbDirectory = Path.GetDirectoryName(dbPath);
                if (!Directory.Exists(dbDirectory))
                {
                    Directory.CreateDirectory(dbDirectory);
                    Logger.Log("Database directory created: " + dbDirectory);
                }

                if (!File.Exists(dbPath))
                {
                    SQLiteConnection.CreateFile(dbPath);
                    Logger.Log("Database file created at: " + dbPath);

                    using (var connection = new SQLiteConnection(ConnectionString))
                    {
                        connection.Open();
                        Logger.Log("Database connection opened.");

                        try
                        {
                            string createTablesQuery = @"
                        CREATE TABLE Users (UserID INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT NOT NULL UNIQUE, Password TEXT NOT NULL, Role TEXT NOT NULL);
                        CREATE TABLE Courses (CourseID INTEGER PRIMARY KEY AUTOINCREMENT, CourseName TEXT NOT NULL UNIQUE);
                        CREATE TABLE Subjects (SubjectID INTEGER PRIMARY KEY AUTOINCREMENT, SubjectName TEXT NOT NULL, CourseID INTEGER, FOREIGN KEY(CourseID) REFERENCES Courses(CourseID));
                        CREATE TABLE Students (StudentID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, CourseID INTEGER, FOREIGN KEY(CourseID) REFERENCES Courses(CourseID));
                        CREATE TABLE Rooms (RoomID INTEGER PRIMARY KEY AUTOINCREMENT, RoomName TEXT NOT NULL, RoomType TEXT NOT NULL);
                        CREATE TABLE Exams (ExamID INTEGER PRIMARY KEY AUTOINCREMENT, ExamName TEXT NOT NULL, SubjectID INTEGER, FOREIGN KEY(SubjectID) REFERENCES Subjects(SubjectID));
                        CREATE TABLE Marks (MarkID INTEGER PRIMARY KEY AUTOINCREMENT, StudentID INTEGER, ExamID INTEGER, Score INTEGER, FOREIGN KEY(StudentID) REFERENCES Students(StudentID), FOREIGN KEY(ExamID) REFERENCES Exams(ExamID));
                        CREATE TABLE Timetables (TimetableID INTEGER PRIMARY KEY AUTOINCREMENT, SubjectID INTEGER, TimeSlot TEXT NOT NULL, RoomID INTEGER, FOREIGN KEY(SubjectID) REFERENCES Subjects(SubjectID), FOREIGN KEY(RoomID) REFERENCES Rooms(RoomID));";

                            new SQLiteCommand(createTablesQuery, connection).ExecuteNonQuery();
                            Logger.Log("Database tables created successfully.");

                            string adminPasswordHash = PasswordHelper.HashPassword("admin123");
                            string studentPasswordHash = PasswordHelper.HashPassword("student123");

                            string insertUsersQuery = @"
                        INSERT INTO Users (Username, Password, Role) VALUES (@adminUser, @adminPass, 'Admin');
                        INSERT INTO Users (Username, Password, Role) VALUES (@studentUser, @studentPass, 'Student');";

                            using (var command = new SQLiteCommand(insertUsersQuery, connection))
                            {
                                command.Parameters.AddWithValue("@adminUser", "admin");
                                command.Parameters.AddWithValue("@adminPass", adminPasswordHash);
                                command.Parameters.AddWithValue("@studentUser", "student1");
                                command.Parameters.AddWithValue("@studentPass", studentPasswordHash);
                                command.ExecuteNonQuery();
                            }

                            Logger.Log("Default users inserted.");
                            //MessageBox.Show("Database created and initialized successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (SQLiteException ex)
                        {
                            Logger.Log("SQLite table creation error: " + ex);
                            MessageBox.Show("Database setup failed. Please contact support.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException dirEx)
            {
                Logger.Log("Directory error: " + dirEx);
                MessageBox.Show("App folder not found. Please check installation.", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (IOException ioEx)
            {
                Logger.Log("File I/O error: " + ioEx);
                MessageBox.Show("File system error. Try restarting the app.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (SQLiteException sqlEx)
            {
                Logger.Log("Database connection error: " + sqlEx);
                MessageBox.Show("Unable to connect to the database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Logger.Log("Unexpected error: " + ex);
                MessageBox.Show("An unexpected error occurred. Please contact support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static User GetUserByCredentials(string username, string passwordHash)
        {
            User user = null;
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @username AND Password = @passwordHash";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@passwordHash", passwordHash);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User { UserID = reader.GetInt32(0), Username = reader.GetString(1), Password = reader.GetString(2), Role = reader.GetString(3) };
                        }
                    }
                }
            }
            return user;
        }

        // --- Course Methods ---
        public static List<Course> GetAllCourses()
        {
            var list = new List<Course>();
            using (var conn = new SQLiteConnection(ConnectionString)) { conn.Open(); using (var cmd = new SQLiteCommand("SELECT CourseID, CourseName FROM Courses", conn)) using (var rdr = cmd.ExecuteReader()) { while (rdr.Read()) { list.Add(new Course { CourseID = rdr.GetInt32(0), CourseName = rdr.GetString(1) }); } } }
            return list;
        }
        public static void AddCourse(string name) { ExecuteNonQuery("INSERT INTO Courses (CourseName) VALUES (@name)", new SQLiteParameter("@name", name)); }
        public static void UpdateCourse(int id, string name) { ExecuteNonQuery("UPDATE Courses SET CourseName = @name WHERE CourseID = @id", new SQLiteParameter("@name", name), new SQLiteParameter("@id", id)); }
        public static void DeleteCourse(int id) { ExecuteNonQuery("DELETE FROM Courses WHERE CourseID = @id", new SQLiteParameter("@id", id)); }

        // --- Student Methods ---
        public static List<Student> GetAllStudents()
        {
            var list = new List<Student>();
            string query = "SELECT s.StudentID, s.Name, s.CourseID, c.CourseName FROM Students s LEFT JOIN Courses c ON s.CourseID = c.CourseID";
            using (var conn = new SQLiteConnection(ConnectionString)) { conn.Open(); using (var cmd = new SQLiteCommand(query, conn)) using (var rdr = cmd.ExecuteReader()) { while (rdr.Read()) { list.Add(new Student { StudentID = rdr.GetInt32(0), Name = rdr.GetString(1), CourseID = rdr.IsDBNull(2) ? 0 : rdr.GetInt32(2), CourseName = rdr.IsDBNull(3) ? "N/A" : rdr.GetString(3) }); } } }
            return list;
        }
        public static void AddStudent(string name, int courseId) { ExecuteNonQuery("INSERT INTO Students (Name, CourseID) VALUES (@name, @courseId)", new SQLiteParameter("@name", name), new SQLiteParameter("@courseId", courseId)); }
        public static void UpdateStudent(int id, string name, int courseId) { ExecuteNonQuery("UPDATE Students SET Name = @name, CourseID = @courseId WHERE StudentID = @id", new SQLiteParameter("@name", name), new SQLiteParameter("@courseId", courseId), new SQLiteParameter("@id", id)); }
        public static void DeleteStudent(int id) { ExecuteNonQuery("DELETE FROM Students WHERE StudentID = @id", new SQLiteParameter("@id", id)); }

        // --- Subject Methods ---
        public static List<Subject> GetAllSubjects()
        {
            var list = new List<Subject>();
            string query = "SELECT s.SubjectID, s.SubjectName, s.CourseID, c.CourseName FROM Subjects s LEFT JOIN Courses c ON s.CourseID = c.CourseID";
            using (var conn = new SQLiteConnection(ConnectionString)) { conn.Open(); using (var cmd = new SQLiteCommand(query, conn)) using (var rdr = cmd.ExecuteReader()) { while (rdr.Read()) { list.Add(new Subject { SubjectID = rdr.GetInt32(0), SubjectName = rdr.GetString(1), CourseID = rdr.IsDBNull(2) ? 0 : rdr.GetInt32(2), CourseName = rdr.IsDBNull(3) ? "N/A" : rdr.GetString(3) }); } } }
            return list;
        }
        public static void AddSubject(string name, int courseId) { ExecuteNonQuery("INSERT INTO Subjects (SubjectName, CourseID) VALUES (@name, @courseId)", new SQLiteParameter("@name", name), new SQLiteParameter("@courseId", courseId)); }
        public static void UpdateSubject(int id, string name, int courseId) { ExecuteNonQuery("UPDATE Subjects SET SubjectName = @name, CourseID = @courseId WHERE SubjectID = @id", new SQLiteParameter("@name", name), new SQLiteParameter("@courseId", courseId), new SQLiteParameter("@id", id)); }
        public static void DeleteSubject(int id) { ExecuteNonQuery("DELETE FROM Subjects WHERE SubjectID = @id", new SQLiteParameter("@id", id)); }

        // --- Room Methods ---
        public static List<Room> GetAllRooms()
        {
            var list = new List<Room>();
            using (var conn = new SQLiteConnection(ConnectionString)) { conn.Open(); using (var cmd = new SQLiteCommand("SELECT RoomID, RoomName, RoomType FROM Rooms", conn)) using (var rdr = cmd.ExecuteReader()) { while (rdr.Read()) { list.Add(new Room { RoomID = rdr.GetInt32(0), RoomName = rdr.GetString(1), RoomType = rdr.GetString(2) }); } } }
            return list;
        }
        public static void AddRoom(string name, string type) { ExecuteNonQuery("INSERT INTO Rooms (RoomName, RoomType) VALUES (@name, @type)", new SQLiteParameter("@name", name), new SQLiteParameter("@type", type)); }
        public static void UpdateRoom(int id, string name, string type) { ExecuteNonQuery("UPDATE Rooms SET RoomName = @name, RoomType = @type WHERE RoomID = @id", new SQLiteParameter("@name", name), new SQLiteParameter("@type", type), new SQLiteParameter("@id", id)); }
        public static void DeleteRoom(int id) { ExecuteNonQuery("DELETE FROM Rooms WHERE RoomID = @id", new SQLiteParameter("@id", id)); }

        // --- Exam Methods ---
        public static List<Exam> GetAllExams()
        {
            var list = new List<Exam>();
            string query = "SELECT e.ExamID, e.ExamName, e.SubjectID, s.SubjectName FROM Exams e LEFT JOIN Subjects s ON e.SubjectID = s.SubjectID";
            using (var conn = new SQLiteConnection(ConnectionString)) { conn.Open(); using (var cmd = new SQLiteCommand(query, conn)) using (var rdr = cmd.ExecuteReader()) { while (rdr.Read()) { list.Add(new Exam { ExamID = rdr.GetInt32(0), ExamName = rdr.GetString(1), SubjectID = rdr.GetInt32(2), SubjectName = rdr.IsDBNull(3) ? "N/A" : rdr.GetString(3) }); } } }
            return list;
        }
        public static void AddExam(string name, int subjectId) { ExecuteNonQuery("INSERT INTO Exams (ExamName, SubjectID) VALUES (@name, @subjectId)", new SQLiteParameter("@name", name), new SQLiteParameter("@subjectId", subjectId)); }

        // --- Marks Methods ---
        public static List<Mark> GetMarksForExam(int examId)
        {
            var marks = new List<Mark>();
            int subjectId;
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT SubjectID FROM Exams WHERE ExamID = @examId", connection))
                {
                    command.Parameters.AddWithValue("@examId", examId);
                    var result = command.ExecuteScalar();
                    if (result == null || result == DBNull.Value) return marks;
                    subjectId = Convert.ToInt32(result);
                }
                string query = @"SELECT s.StudentID, s.Name, m.MarkID, m.Score FROM Students s
                                 LEFT JOIN Marks m ON s.StudentID = m.StudentID AND m.ExamID = @examId
                                 WHERE s.CourseID = (SELECT CourseID FROM Subjects WHERE SubjectID = @subjectId)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@examId", examId);
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) { marks.Add(new Mark { StudentID = reader.GetInt32(0), StudentName = reader.GetString(1), MarkID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2), ExamID = examId, Score = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3) }); }
                    }
                }
            }
            return marks;
        }
        public static void SaveOrUpdateMark(int studentId, int examId, int score)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var checkCommand = new SQLiteCommand("SELECT MarkID FROM Marks WHERE StudentID = @studentId AND ExamID = @examId", connection))
                {
                    checkCommand.Parameters.AddWithValue("@studentId", studentId);
                    checkCommand.Parameters.AddWithValue("@examId", examId);
                    object markIdResult = checkCommand.ExecuteScalar();
                    if (markIdResult != null && markIdResult != DBNull.Value)
                    {
                        ExecuteNonQuery("UPDATE Marks SET Score = @score WHERE MarkID = @markId", new SQLiteParameter("@score", score), new SQLiteParameter("@markId", Convert.ToInt32(markIdResult)));
                    }
                    else
                    {
                        ExecuteNonQuery("INSERT INTO Marks (StudentID, ExamID, Score) VALUES (@studentId, @examId, @score)", new SQLiteParameter("@studentId", studentId), new SQLiteParameter("@examId", examId), new SQLiteParameter("@score", score));
                    }
                }
            }
        }
        public static List<StudentMarkRecord> GetMarksForStudent(string username)
        {
            var results = new List<StudentMarkRecord>();
            string query = @"SELECT s.SubjectName, e.ExamName, m.Score FROM Marks m
                             JOIN Students st ON m.StudentID = st.StudentID
                             JOIN Exams e ON m.ExamID = e.ExamID
                             JOIN Subjects s ON e.SubjectID = s.SubjectID
                             WHERE st.Name = @username";
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new StudentMarkRecord { SubjectName = reader.GetString(0), ExamName = reader.GetString(1), Score = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2) });
                        }
                    }
                }
            }
            return results;
        }

        // --- Timetable Methods ---
        public static List<Timetable> GetAllTimetableEntries()
        {
            var entries = new List<Timetable>();
            string query = @"SELECT t.TimetableID, t.TimeSlot, s.SubjectName, r.RoomName, t.SubjectID, t.RoomID
                             FROM Timetables t
                             LEFT JOIN Subjects s ON t.SubjectID = s.SubjectID
                             LEFT JOIN Rooms r ON t.RoomID = r.RoomID";
            using (var conn = new SQLiteConnection(ConnectionString)) { conn.Open(); using (var cmd = new SQLiteCommand(query, conn)) using (var rdr = cmd.ExecuteReader()) { while (rdr.Read()) { entries.Add(new Timetable { TimetableID = rdr.GetInt32(0), TimeSlot = rdr.GetString(1), SubjectName = rdr.IsDBNull(2) ? "N/A" : rdr.GetString(2), RoomName = rdr.IsDBNull(3) ? "N/A" : rdr.GetString(3), SubjectID = rdr.GetInt32(4), RoomID = rdr.GetInt32(5) }); } } }
            return entries;
        }
        public static void AddTimetableEntry(string timeSlot, int subjectId, int roomId) { ExecuteNonQuery("INSERT INTO Timetables (TimeSlot, SubjectID, RoomID) VALUES (@ts, @sid, @rid)", new SQLiteParameter("@ts", timeSlot), new SQLiteParameter("@sid", subjectId), new SQLiteParameter("@rid", roomId)); }
        public static void UpdateTimetableEntry(int id, string timeSlot, int subjectId, int roomId) { ExecuteNonQuery("UPDATE Timetables SET TimeSlot = @ts, SubjectID = @sid, RoomID = @rid WHERE TimetableID = @id", new SQLiteParameter("@ts", timeSlot), new SQLiteParameter("@sid", subjectId), new SQLiteParameter("@rid", roomId), new SQLiteParameter("@id", id)); }
        public static void DeleteTimetableEntry(int id) { ExecuteNonQuery("DELETE FROM Timetables WHERE TimetableID = @id", new SQLiteParameter("@id", id)); }

        // --- User Methods ---
        public static List<User> GetAllUsers()
        {
            var users = new List<User>();
            using (var conn = new SQLiteConnection(ConnectionString)) { conn.Open(); using (var cmd = new SQLiteCommand("SELECT UserID, Username, Role, Password FROM Users", conn)) using (var rdr = cmd.ExecuteReader()) { while (rdr.Read()) { users.Add(new User { UserID = rdr.GetInt32(0), Username = rdr.GetString(1), Role = rdr.GetString(2), Password = rdr.GetString(3) }); } } }
            return users;
        }
        public static void AddUser(string username, string password, string role) { ExecuteNonQuery("INSERT INTO Users (Username, Password, Role) VALUES (@u, @p, @r)", new SQLiteParameter("@u", username), new SQLiteParameter("@p", password), new SQLiteParameter("@r", role)); }
        public static void UpdateUser(int userId, string username, string password, string role)
        {
            if (!string.IsNullOrEmpty(password))
            {
                string hashedPassword = PasswordHelper.HashPassword(password);
                ExecuteNonQuery("UPDATE Users SET Username = @u, Password = @p, Role = @r WHERE UserID = @id", new SQLiteParameter("@u", username), new SQLiteParameter("@p", hashedPassword), new SQLiteParameter("@r", role), new SQLiteParameter("@id", userId));
            }
            else
            {
                ExecuteNonQuery("UPDATE Users SET Username = @u, Role = @r WHERE UserID = @id", new SQLiteParameter("@u", username), new SQLiteParameter("@r", role), new SQLiteParameter("@id", userId));
            }
        }
        public static void DeleteUser(int userId) { ExecuteNonQuery("DELETE FROM Users WHERE UserID = @id", new SQLiteParameter("@id", userId)); }

        // --- Helper method to reduce code duplication ---
        private static void ExecuteNonQuery(string query, params SQLiteParameter[] parameters)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}