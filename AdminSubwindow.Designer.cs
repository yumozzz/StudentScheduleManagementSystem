namespace StudentScheduleManagementSystem.UI
{
    partial class AdminSubwindowBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        protected System.ComponentModel.IContainer components = null;

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
        protected void InitializeComponent()
        {
            this.ScheduleData = new System.Windows.Forms.DataGridView();
            this.CourseDGVCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ScheduleDGVName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScheduleDGVWeek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScheduleDGVDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScheduleDGVTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScheduleDGVDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WeekSelectBox = new StudentScheduleManagementSystem.MultiSelectBox();
            this.DaySelectBox = new StudentScheduleManagementSystem.MultiSelectBox();
            this.DurcomboBox = new System.Windows.Forms.ComboBox();
            this.HourcomboBox = new System.Windows.Forms.ComboBox();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.AddSchedule = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DeleteSchedule = new System.Windows.Forms.Button();
            this.ReviseSchedule = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ScheduleData
            // 
            this.ScheduleData.AllowUserToAddRows = false;
            this.ScheduleData.BackgroundColor = System.Drawing.Color.White;
            this.ScheduleData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ScheduleData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CourseDGVCheck,
            this.ScheduleDGVName,
            this.ScheduleDGVWeek,
            this.ScheduleDGVDay,
            this.ScheduleDGVTime,
            this.ScheduleDGVDuration});
            this.ScheduleData.Location = new System.Drawing.Point(5, 2);
            this.ScheduleData.Name = "ScheduleData";
            this.ScheduleData.RowHeadersVisible = false;
            this.ScheduleData.RowHeadersWidth = 62;
            this.ScheduleData.RowTemplate.Height = 32;
            this.ScheduleData.Size = new System.Drawing.Size(714, 651);
            this.ScheduleData.TabIndex = 41;
            // 
            // CourseDGVCheck
            // 
            this.CourseDGVCheck.Frozen = true;
            this.CourseDGVCheck.HeaderText = "";
            this.CourseDGVCheck.MinimumWidth = 8;
            this.CourseDGVCheck.Name = "CourseDGVCheck";
            this.CourseDGVCheck.Width = 30;
            // 
            // ScheduleDGVName
            // 
            this.ScheduleDGVName.Frozen = true;
            this.ScheduleDGVName.HeaderText = "日程名称";
            this.ScheduleDGVName.MinimumWidth = 8;
            this.ScheduleDGVName.Name = "ScheduleDGVName";
            this.ScheduleDGVName.ReadOnly = true;
            this.ScheduleDGVName.Width = 180;
            // 
            // ScheduleDGVWeek
            // 
            this.ScheduleDGVWeek.Frozen = true;
            this.ScheduleDGVWeek.HeaderText = "日程周";
            this.ScheduleDGVWeek.MinimumWidth = 8;
            this.ScheduleDGVWeek.Name = "ScheduleDGVWeek";
            this.ScheduleDGVWeek.ReadOnly = true;
            this.ScheduleDGVWeek.Width = 120;
            // 
            // ScheduleDGVDay
            // 
            this.ScheduleDGVDay.Frozen = true;
            this.ScheduleDGVDay.HeaderText = "日程日";
            this.ScheduleDGVDay.MinimumWidth = 8;
            this.ScheduleDGVDay.Name = "ScheduleDGVDay";
            this.ScheduleDGVDay.ReadOnly = true;
            this.ScheduleDGVDay.Width = 120;
            // 
            // ScheduleDGVTime
            // 
            this.ScheduleDGVTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ScheduleDGVTime.HeaderText = "时间";
            this.ScheduleDGVTime.MinimumWidth = 8;
            this.ScheduleDGVTime.Name = "ScheduleDGVTime";
            this.ScheduleDGVTime.ReadOnly = true;
            this.ScheduleDGVTime.Width = 150;
            // 
            // ScheduleDGVDuration
            // 
            this.ScheduleDGVDuration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ScheduleDGVDuration.HeaderText = "时长";
            this.ScheduleDGVDuration.MinimumWidth = 8;
            this.ScheduleDGVDuration.Name = "ScheduleDGVDuration";
            this.ScheduleDGVDuration.ReadOnly = true;
            this.ScheduleDGVDuration.Width = 150;
            // 
            // WeekSelectBox
            // 
            this.WeekSelectBox.BackColor = System.Drawing.Color.White;
            this.WeekSelectBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WeekSelectBox.Location = new System.Drawing.Point(735, 104);
            this.WeekSelectBox.Name = "WeekSelectBox";
            this.WeekSelectBox.Size = new System.Drawing.Size(300, 30);
            this.WeekSelectBox.TabIndex = 36;
            // 
            // DaySelectBox
            // 
            this.DaySelectBox.BackColor = System.Drawing.Color.White;
            this.DaySelectBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DaySelectBox.Location = new System.Drawing.Point(735, 164);
            this.DaySelectBox.Name = "DaySelectBox";
            this.DaySelectBox.Size = new System.Drawing.Size(300, 30);
            this.DaySelectBox.TabIndex = 37;
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
            "3小时"});
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
            // AddSchedule
            // 
            this.AddSchedule.BackColor = System.Drawing.Color.White;
            this.AddSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddSchedule.Location = new System.Drawing.Point(735, 272);
            this.AddSchedule.Name = "AddSchedule";
            this.AddSchedule.Size = new System.Drawing.Size(300, 35);
            this.AddSchedule.TabIndex = 35;
            this.AddSchedule.Text = "AddSchedule";
            this.AddSchedule.UseVisualStyleBackColor = false;
            this.AddSchedule.Click += new System.EventHandler(this.AddSchedule_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::StudentScheduleManagementSystem.Properties.Resources.OPBG_Format_1;
            this.pictureBox1.Location = new System.Drawing.Point(725, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 320);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(900, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 24);
            this.label5.TabIndex = 52;
            this.label5.Text = "Duration";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(735, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 24);
            this.label4.TabIndex = 51;
            this.label4.Text = "Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(735, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 24);
            this.label3.TabIndex = 50;
            this.label3.Text = "Day";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(735, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 49;
            this.label2.Text = "Week";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(735, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 24);
            this.label1.TabIndex = 48;
            this.label1.Text = "Name";
            // 
            // DeleteSchedule
            // 
            this.DeleteSchedule.Location = new System.Drawing.Point(735, 353);
            this.DeleteSchedule.Name = "DeleteSchedule";
            this.DeleteSchedule.Size = new System.Drawing.Size(300, 34);
            this.DeleteSchedule.TabIndex = 53;
            this.DeleteSchedule.Text = "DeleteSchedule";
            this.DeleteSchedule.UseVisualStyleBackColor = true;
            this.DeleteSchedule.Click += new System.EventHandler(this.DeleteSchedule_Click);
            // 
            // ReviseSchedule
            // 
            this.ReviseSchedule.Location = new System.Drawing.Point(735, 410);
            this.ReviseSchedule.Name = "ReviseSchedule";
            this.ReviseSchedule.Size = new System.Drawing.Size(300, 34);
            this.ReviseSchedule.TabIndex = 54;
            this.ReviseSchedule.Text = "ReviseSchedule";
            this.ReviseSchedule.UseVisualStyleBackColor = true;
            this.ReviseSchedule.Click += new System.EventHandler(this.ReviseSchedule_Click);
            // 
            // AdminWindowSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 655);
            this.Controls.Add(this.ReviseSchedule);
            this.Controls.Add(this.DeleteSchedule);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ScheduleData);
            this.Controls.Add(this.WeekSelectBox);
            this.Controls.Add(this.DaySelectBox);
            this.Controls.Add(this.DurcomboBox);
            this.Controls.Add(this.HourcomboBox);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.AddSchedule);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminWindowSchedule";
            this.Text = ",";
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected DataGridView ScheduleData;
        protected MultiSelectBox WeekSelectBox;
        protected MultiSelectBox DaySelectBox;
        protected ComboBox DurcomboBox;
        protected ComboBox HourcomboBox;
        protected TextBox NameBox;
        protected Button AddSchedule;
        protected PictureBox pictureBox1;
        protected Label label5;
        protected Label label4;
        protected Label label3;
        protected Label label2;
        protected Label label1;
        protected DataGridViewCheckBoxColumn CourseDGVCheck;
        protected DataGridViewTextBoxColumn ScheduleDGVName;
        protected DataGridViewTextBoxColumn ScheduleDGVWeek;
        protected DataGridViewTextBoxColumn ScheduleDGVDay;
        protected DataGridViewTextBoxColumn ScheduleDGVTime;
        protected DataGridViewTextBoxColumn ScheduleDGVDuration;
        protected Button DeleteSchedule;
        protected Button ReviseSchedule;
    }
}