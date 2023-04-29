namespace StudentScheduleManagementSystem
{
    partial class AdminWindowExam
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
            this.ExamData = new System.Windows.Forms.DataGridView();
            this.DurcomboBox = new System.Windows.Forms.ComboBox();
            this.HourcomboBox = new System.Windows.Forms.ComboBox();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.AddExam = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.WeekcomboBox = new System.Windows.Forms.ComboBox();
            this.DaycomboBox = new System.Windows.Forms.ComboBox();
            this.DeleteExam = new System.Windows.Forms.Button();
            this.ReviseExam = new System.Windows.Forms.Button();
            this.CourseDGVCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ExamDGVName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IDDGVtextBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExamDGVWeek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExamDGVDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExamDGVTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExamDGVDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ExamData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ExamData
            // 
            this.ExamData.AllowUserToAddRows = false;
            this.ExamData.BackgroundColor = System.Drawing.Color.White;
            this.ExamData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ExamData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CourseDGVCheck,
            this.ExamDGVName,
            this.IDDGVtextBox,
            this.ExamDGVWeek,
            this.ExamDGVDay,
            this.ExamDGVTime,
            this.ExamDGVDuration});
            this.ExamData.Location = new System.Drawing.Point(5, 2);
            this.ExamData.Name = "ExamData";
            this.ExamData.RowHeadersVisible = false;
            this.ExamData.RowHeadersWidth = 62;
            this.ExamData.RowTemplate.Height = 32;
            this.ExamData.Size = new System.Drawing.Size(714, 651);
            this.ExamData.TabIndex = 41;
            // 
            // DurcomboBox
            // 
            this.DurcomboBox.BackColor = System.Drawing.Color.White;
            this.DurcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DurcomboBox.DropDownWidth = 130;
            this.DurcomboBox.FormattingEnabled = true;
            this.DurcomboBox.Items.AddRange(new object[] {
            "1小时",
            "2小时",
            "3小时",
            "4小时",
            "5小时",
            "6小时",
            "7小时"});
            this.DurcomboBox.Location = new System.Drawing.Point(900, 224);
            this.DurcomboBox.Name = "DurcomboBox";
            this.DurcomboBox.Size = new System.Drawing.Size(135, 32);
            this.DurcomboBox.TabIndex = 40;
            // 
            // HourcomboBox
            // 
            this.HourcomboBox.BackColor = System.Drawing.Color.White;
            this.HourcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HourcomboBox.FormattingEnabled = true;
            this.HourcomboBox.Items.AddRange(new object[] {
            "8:00",
            "9:00",
            "10:00",
            "11:00",
            "12:00",
            "13:00",
            "14:00",
            "15:00",
            "16:00",
            "17:00",
            "18:00",
            "19:00",
            "20:00",
            "21:00"});
            this.HourcomboBox.Location = new System.Drawing.Point(735, 224);
            this.HourcomboBox.Name = "HourcomboBox";
            this.HourcomboBox.Size = new System.Drawing.Size(135, 32);
            this.HourcomboBox.TabIndex = 39;
            // 
            // NameBox
            // 
            this.NameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NameBox.Location = new System.Drawing.Point(735, 44);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(300, 30);
            this.NameBox.TabIndex = 38;
            // 
            // AddExam
            // 
            this.AddExam.BackColor = System.Drawing.Color.White;
            this.AddExam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddExam.Location = new System.Drawing.Point(735, 274);
            this.AddExam.Name = "AddExam";
            this.AddExam.Size = new System.Drawing.Size(300, 35);
            this.AddExam.TabIndex = 35;
            this.AddExam.Text = "AddExam";
            this.AddExam.UseVisualStyleBackColor = false;
            this.AddExam.Click += new System.EventHandler(this.AddExam_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::StudentScheduleManagementSystem.Properties.Resources.OPBG_Format_1;
            this.pictureBox1.Location = new System.Drawing.Point(725, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 320);
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(735, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 24);
            this.label1.TabIndex = 43;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(735, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 44;
            this.label2.Text = "Week";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(735, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 24);
            this.label3.TabIndex = 45;
            this.label3.Text = "Day";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(735, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 24);
            this.label4.TabIndex = 46;
            this.label4.Text = "Time";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(900, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 24);
            this.label5.TabIndex = 47;
            this.label5.Text = "Duration";
            // 
            // WeekcomboBox
            // 
            this.WeekcomboBox.BackColor = System.Drawing.Color.White;
            this.WeekcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WeekcomboBox.FormattingEnabled = true;
            this.WeekcomboBox.Items.AddRange(new object[] {
            "Week1",
            "Week2",
            "Week3",
            "Week4",
            "Week5",
            "Week6",
            "Week7",
            "Week8",
            "Week9",
            "Week10",
            "Week11",
            "Week12",
            "Week13",
            "Week14",
            "Week15",
            "Week16"});
            this.WeekcomboBox.Location = new System.Drawing.Point(735, 104);
            this.WeekcomboBox.Name = "WeekcomboBox";
            this.WeekcomboBox.Size = new System.Drawing.Size(135, 32);
            this.WeekcomboBox.TabIndex = 48;
            // 
            // DaycomboBox
            // 
            this.DaycomboBox.BackColor = System.Drawing.Color.White;
            this.DaycomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DaycomboBox.FormattingEnabled = true;
            this.DaycomboBox.Items.AddRange(new object[] {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"});
            this.DaycomboBox.Location = new System.Drawing.Point(735, 162);
            this.DaycomboBox.Name = "DaycomboBox";
            this.DaycomboBox.Size = new System.Drawing.Size(135, 32);
            this.DaycomboBox.TabIndex = 49;
            // 
            // DeleteExam
            // 
            this.DeleteExam.Location = new System.Drawing.Point(735, 344);
            this.DeleteExam.Name = "DeleteExam";
            this.DeleteExam.Size = new System.Drawing.Size(300, 34);
            this.DeleteExam.TabIndex = 54;
            this.DeleteExam.Text = "DeleteExam";
            this.DeleteExam.UseVisualStyleBackColor = true;
            this.DeleteExam.Click += new System.EventHandler(this.DeleteExam_Click);
            // 
            // ReviseExam
            // 
            this.ReviseExam.Location = new System.Drawing.Point(735, 405);
            this.ReviseExam.Name = "ReviseExam";
            this.ReviseExam.Size = new System.Drawing.Size(300, 34);
            this.ReviseExam.TabIndex = 55;
            this.ReviseExam.Text = "ReviseExam";
            this.ReviseExam.UseVisualStyleBackColor = true;
            this.ReviseExam.Click += new System.EventHandler(this.ReviseExam_Click);
            // 
            // CourseDGVCheck
            // 
            this.CourseDGVCheck.Frozen = true;
            this.CourseDGVCheck.HeaderText = "";
            this.CourseDGVCheck.MinimumWidth = 8;
            this.CourseDGVCheck.Name = "CourseDGVCheck";
            this.CourseDGVCheck.Width = 30;
            // 
            // ExamDGVName
            // 
            this.ExamDGVName.Frozen = true;
            this.ExamDGVName.HeaderText = "考试名称";
            this.ExamDGVName.MinimumWidth = 8;
            this.ExamDGVName.Name = "ExamDGVName";
            this.ExamDGVName.ReadOnly = true;
            this.ExamDGVName.Width = 180;
            // 
            // IDDGVtextBox
            // 
            this.IDDGVtextBox.Frozen = true;
            this.IDDGVtextBox.HeaderText = "ID";
            this.IDDGVtextBox.MinimumWidth = 8;
            this.IDDGVtextBox.Name = "IDDGVtextBox";
            this.IDDGVtextBox.Width = 150;
            // 
            // ExamDGVWeek
            // 
            this.ExamDGVWeek.Frozen = true;
            this.ExamDGVWeek.HeaderText = "考试周";
            this.ExamDGVWeek.MinimumWidth = 8;
            this.ExamDGVWeek.Name = "ExamDGVWeek";
            this.ExamDGVWeek.ReadOnly = true;
            this.ExamDGVWeek.Width = 120;
            // 
            // ExamDGVDay
            // 
            this.ExamDGVDay.Frozen = true;
            this.ExamDGVDay.HeaderText = "考试日";
            this.ExamDGVDay.MinimumWidth = 8;
            this.ExamDGVDay.Name = "ExamDGVDay";
            this.ExamDGVDay.ReadOnly = true;
            this.ExamDGVDay.Width = 120;
            // 
            // ExamDGVTime
            // 
            this.ExamDGVTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ExamDGVTime.HeaderText = "时间";
            this.ExamDGVTime.MinimumWidth = 8;
            this.ExamDGVTime.Name = "ExamDGVTime";
            this.ExamDGVTime.ReadOnly = true;
            this.ExamDGVTime.Width = 150;
            // 
            // ExamDGVDuration
            // 
            this.ExamDGVDuration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ExamDGVDuration.HeaderText = "时长";
            this.ExamDGVDuration.MinimumWidth = 8;
            this.ExamDGVDuration.Name = "ExamDGVDuration";
            this.ExamDGVDuration.ReadOnly = true;
            this.ExamDGVDuration.Width = 150;
            // 
            // AdminWindowExam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 655);
            this.Controls.Add(this.ReviseExam);
            this.Controls.Add(this.DeleteExam);
            this.Controls.Add(this.DaycomboBox);
            this.Controls.Add(this.WeekcomboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ExamData);
            this.Controls.Add(this.DurcomboBox);
            this.Controls.Add(this.HourcomboBox);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.AddExam);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminWindowExam";
            this.Text = "AdminWindowTest";
            ((System.ComponentModel.ISupportInitialize)(this.ExamData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView ExamData;
        private ComboBox DurcomboBox;
        private ComboBox HourcomboBox;
        private TextBox NameBox;
        private Button AddExam;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private ComboBox WeekcomboBox;
        private ComboBox DaycomboBox;
        private Button DeleteExam;
        private Button ReviseExam;
        private DataGridViewCheckBoxColumn CourseDGVCheck;
        private DataGridViewTextBoxColumn ExamDGVName;
        private DataGridViewTextBoxColumn IDDGVtextBox;
        private DataGridViewTextBoxColumn ExamDGVWeek;
        private DataGridViewTextBoxColumn ExamDGVDay;
        private DataGridViewTextBoxColumn ExamDGVTime;
        private DataGridViewTextBoxColumn ExamDGVDuration;
    }
}