namespace StudentScheduleManagementSystem
{
    partial class AdminWindowActivity
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
            this.ActivityData = new System.Windows.Forms.DataGridView();
            this.CourseDGVCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ActivityDGVName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivityDGVWeek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivityDGVDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivityDGVTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivityDGVDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WeekSelectBox = new StudentScheduleManagementSystem.MultiSelectBox();
            this.DaySelectBox = new StudentScheduleManagementSystem.MultiSelectBox();
            this.DurcomboBox = new System.Windows.Forms.ComboBox();
            this.HourcomboBox = new System.Windows.Forms.ComboBox();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.AddActivity = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DeleteActivity = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ActivityData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ActivityData
            // 
            this.ActivityData.AllowUserToAddRows = false;
            this.ActivityData.BackgroundColor = System.Drawing.Color.White;
            this.ActivityData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ActivityData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CourseDGVCheck,
            this.ActivityDGVName,
            this.ActivityDGVWeek,
            this.ActivityDGVDay,
            this.ActivityDGVTime,
            this.ActivityDGVDuration});
            this.ActivityData.Location = new System.Drawing.Point(5, 2);
            this.ActivityData.Name = "ActivityData";
            this.ActivityData.RowHeadersVisible = false;
            this.ActivityData.RowHeadersWidth = 62;
            this.ActivityData.RowTemplate.Height = 32;
            this.ActivityData.Size = new System.Drawing.Size(714, 651);
            this.ActivityData.TabIndex = 41;
            // 
            // CourseDGVCheck
            // 
            this.CourseDGVCheck.Frozen = true;
            this.CourseDGVCheck.HeaderText = "";
            this.CourseDGVCheck.MinimumWidth = 8;
            this.CourseDGVCheck.Name = "CourseDGVCheck";
            this.CourseDGVCheck.Width = 30;
            // 
            // ActivityDGVName
            // 
            this.ActivityDGVName.Frozen = true;
            this.ActivityDGVName.HeaderText = "活动名称";
            this.ActivityDGVName.MinimumWidth = 8;
            this.ActivityDGVName.Name = "ActivityDGVName";
            this.ActivityDGVName.ReadOnly = true;
            this.ActivityDGVName.Width = 180;
            // 
            // ActivityDGVWeek
            // 
            this.ActivityDGVWeek.Frozen = true;
            this.ActivityDGVWeek.HeaderText = "活动周";
            this.ActivityDGVWeek.MinimumWidth = 8;
            this.ActivityDGVWeek.Name = "ActivityDGVWeek";
            this.ActivityDGVWeek.ReadOnly = true;
            this.ActivityDGVWeek.Width = 120;
            // 
            // ActivityDGVDay
            // 
            this.ActivityDGVDay.Frozen = true;
            this.ActivityDGVDay.HeaderText = "活动日";
            this.ActivityDGVDay.MinimumWidth = 8;
            this.ActivityDGVDay.Name = "ActivityDGVDay";
            this.ActivityDGVDay.ReadOnly = true;
            this.ActivityDGVDay.Width = 120;
            // 
            // ActivityDGVTime
            // 
            this.ActivityDGVTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ActivityDGVTime.HeaderText = "时间";
            this.ActivityDGVTime.MinimumWidth = 8;
            this.ActivityDGVTime.Name = "ActivityDGVTime";
            this.ActivityDGVTime.ReadOnly = true;
            this.ActivityDGVTime.Width = 150;
            // 
            // ActivityDGVDuration
            // 
            this.ActivityDGVDuration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ActivityDGVDuration.HeaderText = "时长";
            this.ActivityDGVDuration.MinimumWidth = 8;
            this.ActivityDGVDuration.Name = "ActivityDGVDuration";
            this.ActivityDGVDuration.ReadOnly = true;
            this.ActivityDGVDuration.Width = 150;
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
            // AddActivity
            // 
            this.AddActivity.BackColor = System.Drawing.Color.White;
            this.AddActivity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddActivity.Location = new System.Drawing.Point(735, 272);
            this.AddActivity.Name = "AddActivity";
            this.AddActivity.Size = new System.Drawing.Size(300, 35);
            this.AddActivity.TabIndex = 35;
            this.AddActivity.Text = "AddActivity";
            this.AddActivity.UseVisualStyleBackColor = false;
            this.AddActivity.Click += new System.EventHandler(this.AddActivity_Click);
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
            // DeleteActivity
            // 
            this.DeleteActivity.Location = new System.Drawing.Point(735, 353);
            this.DeleteActivity.Name = "DeleteActivity";
            this.DeleteActivity.Size = new System.Drawing.Size(300, 34);
            this.DeleteActivity.TabIndex = 53;
            this.DeleteActivity.Text = "DeleteActivity";
            this.DeleteActivity.UseVisualStyleBackColor = true;
            this.DeleteActivity.Click += new System.EventHandler(this.DeleteActivity_Click);
            // 
            // AdminWindowActivity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 655);
            this.Controls.Add(this.DeleteActivity);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ActivityData);
            this.Controls.Add(this.WeekSelectBox);
            this.Controls.Add(this.DaySelectBox);
            this.Controls.Add(this.DurcomboBox);
            this.Controls.Add(this.HourcomboBox);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.AddActivity);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminWindowActivity";
            this.Text = ",";
            ((System.ComponentModel.ISupportInitialize)(this.ActivityData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView ActivityData;
        private MultiSelectBox WeekSelectBox;
        private MultiSelectBox DaySelectBox;
        private ComboBox DurcomboBox;
        private ComboBox HourcomboBox;
        private TextBox NameBox;
        private Button AddActivity;
        private PictureBox pictureBox1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private DataGridViewCheckBoxColumn CourseDGVCheck;
        private DataGridViewTextBoxColumn ActivityDGVName;
        private DataGridViewTextBoxColumn ActivityDGVWeek;
        private DataGridViewTextBoxColumn ActivityDGVDay;
        private DataGridViewTextBoxColumn ActivityDGVTime;
        private DataGridViewTextBoxColumn ActivityDGVDuration;
        private Button DeleteActivity;
    }
}