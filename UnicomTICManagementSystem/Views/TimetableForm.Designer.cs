namespace UnicomTICManagementSystem.Views
{
    partial class TimetableForm
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
            this.dgvTimetable = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTimeSlot = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSubjects = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbRooms = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.pnlAdminControls = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimetable)).BeginInit();
            this.pnlAdminControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTimetable
            // 
            this.dgvTimetable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTimetable.Location = new System.Drawing.Point(13, 13);
            this.dgvTimetable.Name = "dgvTimetable";
            this.dgvTimetable.RowHeadersWidth = 62;
            this.dgvTimetable.RowTemplate.Height = 28;
            this.dgvTimetable.Size = new System.Drawing.Size(775, 239);
            this.dgvTimetable.TabIndex = 0;
            this.dgvTimetable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTimetable_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Time Slot:";
            // 
            // txtTimeSlot
            // 
            this.txtTimeSlot.Location = new System.Drawing.Point(161, 20);
            this.txtTimeSlot.Name = "txtTimeSlot";
            this.txtTimeSlot.Size = new System.Drawing.Size(304, 26);
            this.txtTimeSlot.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Subject:";
            // 
            // cmbSubjects
            // 
            this.cmbSubjects.FormattingEnabled = true;
            this.cmbSubjects.Location = new System.Drawing.Point(161, 64);
            this.cmbSubjects.Name = "cmbSubjects";
            this.cmbSubjects.Size = new System.Drawing.Size(304, 28);
            this.cmbSubjects.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Room:";
            // 
            // cmbRooms
            // 
            this.cmbRooms.FormattingEnabled = true;
            this.cmbRooms.Location = new System.Drawing.Point(161, 119);
            this.cmbRooms.Name = "cmbRooms";
            this.cmbRooms.Size = new System.Drawing.Size(304, 28);
            this.cmbRooms.TabIndex = 6;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(157, 178);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(136, 41);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add Entry";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(319, 178);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(133, 41);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Update Entry";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(485, 178);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(149, 41);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete Entry";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnlAdminControls
            // 
            this.pnlAdminControls.Controls.Add(this.btnAdd);
            this.pnlAdminControls.Controls.Add(this.cmbSubjects);
            this.pnlAdminControls.Controls.Add(this.cmbRooms);
            this.pnlAdminControls.Controls.Add(this.label2);
            this.pnlAdminControls.Controls.Add(this.btnDelete);
            this.pnlAdminControls.Controls.Add(this.txtTimeSlot);
            this.pnlAdminControls.Controls.Add(this.label3);
            this.pnlAdminControls.Controls.Add(this.label1);
            this.pnlAdminControls.Controls.Add(this.btnUpdate);
            this.pnlAdminControls.Location = new System.Drawing.Point(13, 283);
            this.pnlAdminControls.Name = "pnlAdminControls";
            this.pnlAdminControls.Size = new System.Drawing.Size(776, 251);
            this.pnlAdminControls.TabIndex = 10;
            // 
            // TimetableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 546);
            this.Controls.Add(this.pnlAdminControls);
            this.Controls.Add(this.dgvTimetable);
            this.Name = "TimetableForm";
            this.Text = "TimetableForm";
            this.Load += new System.EventHandler(this.TimetableForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimetable)).EndInit();
            this.pnlAdminControls.ResumeLayout(false);
            this.pnlAdminControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTimetable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTimeSlot;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbSubjects;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbRooms;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel pnlAdminControls;
    }
}