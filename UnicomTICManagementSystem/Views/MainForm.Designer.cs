using System;

namespace UnicomTICManagementSystem.Views
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCourses = new System.Windows.Forms.Button();
            this.btnStudents = new System.Windows.Forms.Button();
            this.btnTimetable = new System.Windows.Forms.Button();
            this.btnExams = new System.Windows.Forms.Button();
            this.btnSubjects = new System.Windows.Forms.Button();
            this.btnRooms = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCourses
            // 
            this.btnCourses.Location = new System.Drawing.Point(95, 29);
            this.btnCourses.Name = "btnCourses";
            this.btnCourses.Size = new System.Drawing.Size(208, 39);
            this.btnCourses.TabIndex = 0;
            this.btnCourses.Text = "Manage Courses";
            this.btnCourses.UseVisualStyleBackColor = true;
            this.btnCourses.Click += new System.EventHandler(this.btnCourses_Click);
            // 
            // btnStudents
            // 
            this.btnStudents.Location = new System.Drawing.Point(95, 87);
            this.btnStudents.Name = "btnStudents";
            this.btnStudents.Size = new System.Drawing.Size(208, 41);
            this.btnStudents.TabIndex = 1;
            this.btnStudents.Text = "Manage Students";
            this.btnStudents.UseVisualStyleBackColor = true;
            this.btnStudents.Click += new System.EventHandler(this.btnStudents_Click);
            // 
            // btnTimetable
            // 
            this.btnTimetable.Location = new System.Drawing.Point(95, 225);
            this.btnTimetable.Name = "btnTimetable";
            this.btnTimetable.Size = new System.Drawing.Size(208, 39);
            this.btnTimetable.TabIndex = 2;
            this.btnTimetable.Text = "View Timetable";
            this.btnTimetable.UseVisualStyleBackColor = true;
            this.btnTimetable.Click += new System.EventHandler(this.btnTimetable_Click);
            // 
            // btnExams
            // 
            this.btnExams.Location = new System.Drawing.Point(95, 294);
            this.btnExams.Name = "btnExams";
            this.btnExams.Size = new System.Drawing.Size(208, 42);
            this.btnExams.TabIndex = 3;
            this.btnExams.Text = "Manage Exams & Marks";
            this.btnExams.UseVisualStyleBackColor = true;
            // 
            // btnSubjects
            // 
            this.btnSubjects.Location = new System.Drawing.Point(95, 153);
            this.btnSubjects.Name = "btnSubjects";
            this.btnSubjects.Size = new System.Drawing.Size(208, 41);
            this.btnSubjects.TabIndex = 4;
            this.btnSubjects.Text = "Manage Subjects";
            this.btnSubjects.UseVisualStyleBackColor = true;
            this.btnSubjects.Click += new System.EventHandler(this.btnSubjects_Click);
            // 
            // btnRooms
            // 
            this.btnRooms.Location = new System.Drawing.Point(95, 370);
            this.btnRooms.Name = "btnRooms";
            this.btnRooms.Size = new System.Drawing.Size(208, 39);
            this.btnRooms.TabIndex = 5;
            this.btnRooms.Text = "Manage Rooms";
            this.btnRooms.UseVisualStyleBackColor = true;
            this.btnRooms.Click += new System.EventHandler(this.btnRooms_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.btnRooms);
            this.Controls.Add(this.btnSubjects);
            this.Controls.Add(this.btnExams);
            this.Controls.Add(this.btnTimetable);
            this.Controls.Add(this.btnStudents);
            this.Controls.Add(this.btnCourses);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button btnCourses;
        private System.Windows.Forms.Button btnStudents;
        private System.Windows.Forms.Button btnTimetable;
        private System.Windows.Forms.Button btnExams;
        private System.Windows.Forms.Button btnSubjects;
        private System.Windows.Forms.Button btnRooms;
    }
}