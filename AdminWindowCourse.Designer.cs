namespace StudentScheduleManagementSystem
{
    partial class AdminWindowCourse
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
            this.CourseData = new System.Windows.Forms.DataGridView();
            this.WeekSelectBox = new StudentScheduleManagementSystem.MultiSelectBox();
            this.DaySelectBox = new StudentScheduleManagementSystem.MultiSelectBox();
            this.DurcomboBox = new System.Windows.Forms.ComboBox();
            this.HourcomboBox = new System.Windows.Forms.ComboBox();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.AddCourse = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CourseDGVCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CourseDGVName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CourseDGVWeek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CourseDGVDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CourseDGVTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CourseDGVDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeleteCourse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CourseData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // CourseData
            // 
            this.CourseData.AllowUserToAddRows = false;
            this.CourseData.BackgroundColor = System.Drawing.Color.White;
            this.CourseData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CourseData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CourseDGVCheck,
            this.CourseDGVName,
            this.CourseDGVWeek,
            this.CourseDGVDay,
            this.CourseDGVTime,
            this.CourseDGVDuration});
            this.CourseData.Location = new System.Drawing.Point(5, 2);
            this.CourseData.Name = "CourseData";
            this.CourseData.RowHeadersVisible = false;
            this.CourseData.RowHeadersWidth = 62;
            this.CourseData.RowTemplate.Height = 32;
            this.CourseData.Size = new System.Drawing.Size(714, 651);
            this.CourseData.TabIndex = 33;
            // 
            // WeekSelectBox
            // 
            this.WeekSelectBox.BackColor = System.Drawing.Color.White;
            this.WeekSelectBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WeekSelectBox.Location = new System.Drawing.Point(735, 104);
            this.WeekSelectBox.Name = "WeekSelectBox";
            this.WeekSelectBox.Size = new System.Drawing.Size(300, 30);
            this.WeekSelectBox.TabIndex = 28;
            // 
            // DaySelectBox
            // 
            this.DaySelectBox.BackColor = System.Drawing.Color.White;
            this.DaySelectBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DaySelectBox.Location = new System.Drawing.Point(735, 164);
            this.DaySelectBox.Name = "DaySelectBox";
            this.DaySelectBox.Size = new System.Drawing.Size(300, 30);
            this.DaySelectBox.TabIndex = 29;
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
            this.DurcomboBox.TabIndex = 32;
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
            this.HourcomboBox.TabIndex = 31;
            // 
            // NameBox
            // 
            this.NameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NameBox.Location = new System.Drawing.Point(735, 44);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(300, 30);
            this.NameBox.TabIndex = 30;
            // 
            // AddCourse
            // 
            this.AddCourse.BackColor = System.Drawing.Color.White;
            this.AddCourse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddCourse.Location = new System.Drawing.Point(735, 274);
            this.AddCourse.Name = "AddCourse";
            this.AddCourse.Size = new System.Drawing.Size(300, 35);
            this.AddCourse.TabIndex = 27;
            this.AddCourse.Text = "AddCourse";
            this.AddCourse.UseVisualStyleBackColor = false;
            this.AddCourse.Click += new System.EventHandler(this.AddCourse_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::StudentScheduleManagementSystem.Properties.Resources.OPBG_Format_1;
            this.pictureBox1.Location = new System.Drawing.Point(725, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 320);
            this.pictureBox1.TabIndex = 34;
            this.pictureBox1.TabStop = false;
            // 
            // CourseDGVCheck
            // 
            this.CourseDGVCheck.Frozen = true;
            this.CourseDGVCheck.HeaderText = "";
            this.CourseDGVCheck.MinimumWidth = 8;
            this.CourseDGVCheck.Name = "CourseDGVCheck";
            this.CourseDGVCheck.Width = 30;
            // 
            // CourseDGVName
            // 
            this.CourseDGVName.Frozen = true;
            this.CourseDGVName.HeaderText = "课程名称";
            this.CourseDGVName.MinimumWidth = 8;
            this.CourseDGVName.Name = "CourseDGVName";
            this.CourseDGVName.ReadOnly = true;
            this.CourseDGVName.Width = 180;
            // 
            // CourseDGVWeek
            // 
            this.CourseDGVWeek.Frozen = true;
            this.CourseDGVWeek.HeaderText = "上课周";
            this.CourseDGVWeek.MinimumWidth = 8;
            this.CourseDGVWeek.Name = "CourseDGVWeek";
            this.CourseDGVWeek.ReadOnly = true;
            this.CourseDGVWeek.Width = 120;
            // 
            // CourseDGVDay
            // 
            this.CourseDGVDay.Frozen = true;
            this.CourseDGVDay.HeaderText = "上课日";
            this.CourseDGVDay.MinimumWidth = 8;
            this.CourseDGVDay.Name = "CourseDGVDay";
            this.CourseDGVDay.ReadOnly = true;
            this.CourseDGVDay.Width = 120;
            // 
            // CourseDGVTime
            // 
            this.CourseDGVTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CourseDGVTime.HeaderText = "时间";
            this.CourseDGVTime.MinimumWidth = 8;
            this.CourseDGVTime.Name = "CourseDGVTime";
            this.CourseDGVTime.ReadOnly = true;
            this.CourseDGVTime.Width = 150;
            // 
            // CourseDGVDuration
            // 
            this.CourseDGVDuration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CourseDGVDuration.HeaderText = "时长";
            this.CourseDGVDuration.MinimumWidth = 8;
            this.CourseDGVDuration.Name = "CourseDGVDuration";
            this.CourseDGVDuration.ReadOnly = true;
            this.CourseDGVDuration.Width = 150;
            // 
            // DeleteCourse
            // 
            this.DeleteCourse.Location = new System.Drawing.Point(735, 364);
            this.DeleteCourse.Name = "DeleteCourse";
            this.DeleteCourse.Size = new System.Drawing.Size(300, 34);
            this.DeleteCourse.TabIndex = 35;
            this.DeleteCourse.Text = "DeleteCourse";
            this.DeleteCourse.UseVisualStyleBackColor = true;
            this.DeleteCourse.Click += new System.EventHandler(this.DeleteCourse_Click);
            // 
            // AdminWindowCourse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 655);
            this.Controls.Add(this.DeleteCourse);
            this.Controls.Add(this.CourseData);
            this.Controls.Add(this.WeekSelectBox);
            this.Controls.Add(this.DaySelectBox);
            this.Controls.Add(this.DurcomboBox);
            this.Controls.Add(this.HourcomboBox);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.AddCourse);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminWindowCourse";
            this.Text = "AdminWindowCourse";
            ((System.ComponentModel.ISupportInitialize)(this.CourseData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView CourseData;
        private MultiSelectBox WeekSelectBox;
        private MultiSelectBox DaySelectBox;
        private ComboBox DurcomboBox;
        private ComboBox HourcomboBox;
        private TextBox NameBox;
        private Button AddCourse;
        private PictureBox pictureBox1;
        private DataGridViewCheckBoxColumn CourseDGVCheck;
        private DataGridViewTextBoxColumn CourseDGVName;
        private DataGridViewTextBoxColumn CourseDGVWeek;
        private DataGridViewTextBoxColumn CourseDGVDay;
        private DataGridViewTextBoxColumn CourseDGVTime;
        private DataGridViewTextBoxColumn CourseDGVDuration;
        private Button DeleteCourse;
    }
}