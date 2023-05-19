namespace StudentScheduleManagementSystem.UI
{
    partial class StudentWindow
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
            this.header = new System.Windows.Forms.Panel();
            this.speedButton = new System.Windows.Forms.Button();
            this.setTime = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.hourBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dayBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.weekBox = new System.Windows.Forms.ComboBox();
            this.currentTime = new System.Windows.Forms.Label();
            this.logoutButton = new System.Windows.Forms.Button();
            this.mainpageButton = new System.Windows.Forms.PictureBox();
            this.mainpage = new System.Windows.Forms.Panel();
            this.ScheduleTableButton = new System.Windows.Forms.Button();
            this.courseButton = new System.Windows.Forms.Button();
            this.examButton = new System.Windows.Forms.Button();
            this.GroupActivityButton = new System.Windows.Forms.Button();
            this.personalActivityButton = new System.Windows.Forms.Button();
            this.temporaryAffairButton = new System.Windows.Forms.Button();
            this.header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainpageButton)).BeginInit();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.Controls.Add(this.speedButton);
            this.header.Controls.Add(this.setTime);
            this.header.Controls.Add(this.label3);
            this.header.Controls.Add(this.hourBox);
            this.header.Controls.Add(this.label2);
            this.header.Controls.Add(this.dayBox);
            this.header.Controls.Add(this.label1);
            this.header.Controls.Add(this.weekBox);
            this.header.Controls.Add(this.currentTime);
            this.header.Controls.Add(this.logoutButton);
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(1300, 45);
            this.header.TabIndex = 1;
            this.header.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Header_MouseDown);
            this.header.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Header_MouseMove);
            // 
            // speedButton
            // 
            this.speedButton.Location = new System.Drawing.Point(849, 3);
            this.speedButton.Name = "speedButton";
            this.speedButton.Size = new System.Drawing.Size(112, 34);
            this.speedButton.TabIndex = 17;
            this.speedButton.Text = "快进";
            this.speedButton.UseVisualStyleBackColor = true;
            this.speedButton.Click += new System.EventHandler(this.SpeedButton_Click);
            // 
            // setTime
            // 
            this.setTime.Location = new System.Drawing.Point(731, 3);
            this.setTime.Name = "setTime";
            this.setTime.Size = new System.Drawing.Size(112, 34);
            this.setTime.TabIndex = 16;
            this.setTime.Text = "SetTime";
            this.setTime.UseVisualStyleBackColor = true;
            this.setTime.Click += new System.EventHandler(this.SetTime_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(567, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 24);
            this.label3.TabIndex = 13;
            this.label3.Text = "Hour";
            // 
            // hourBox
            // 
            this.hourBox.FormattingEnabled = true;
            this.hourBox.Items.AddRange(new object[] {
            "0:00",
            "1:00",
            "2:00",
            "3:00",
            "4:00",
            "5:00",
            "6:00",
            "7:00",
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
            "21:00",
            "22:00",
            "23:00"});
            this.hourBox.Location = new System.Drawing.Point(626, 5);
            this.hourBox.Name = "hourBox";
            this.hourBox.Size = new System.Drawing.Size(99, 32);
            this.hourBox.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(402, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 24);
            this.label2.TabIndex = 11;
            this.label2.Text = "Day";
            // 
            // dayBox
            // 
            this.dayBox.FormattingEnabled = true;
            this.dayBox.Items.AddRange(new object[] {
            "Mon",
            "Tue",
            "Wed",
            "Thu",
            "Fri",
            "Sat",
            "Sun"});
            this.dayBox.Location = new System.Drawing.Point(452, 5);
            this.dayBox.Name = "dayBox";
            this.dayBox.Size = new System.Drawing.Size(109, 32);
            this.dayBox.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(210, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 24);
            this.label1.TabIndex = 9;
            this.label1.Text = "Week";
            // 
            // weekBox
            // 
            this.weekBox.FormattingEnabled = true;
            this.weekBox.Items.AddRange(new object[] {
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
            this.weekBox.Location = new System.Drawing.Point(274, 5);
            this.weekBox.Name = "weekBox";
            this.weekBox.Size = new System.Drawing.Size(122, 32);
            this.weekBox.TabIndex = 8;
            // 
            // currentTime
            // 
            this.currentTime.AutoSize = true;
            this.currentTime.Location = new System.Drawing.Point(12, 11);
            this.currentTime.Name = "currentTime";
            this.currentTime.Size = new System.Drawing.Size(68, 24);
            this.currentTime.TabIndex = 2;
            this.currentTime.Text = "Week i";
            // 
            // logoutButton
            // 
            this.logoutButton.Location = new System.Drawing.Point(1185, 3);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(112, 34);
            this.logoutButton.TabIndex = 7;
            this.logoutButton.Text = "Logout";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.Logout_Click);
            // 
            // mainpageButton
            // 
            this.mainpageButton.BackColor = System.Drawing.Color.Transparent;
            this.mainpageButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.SubPageBG;
            this.mainpageButton.Location = new System.Drawing.Point(171, 53);
            this.mainpageButton.Name = "mainpageButton";
            this.mainpageButton.Size = new System.Drawing.Size(1060, 665);
            this.mainpageButton.TabIndex = 32;
            this.mainpageButton.TabStop = false;
            // 
            // mainpage
            // 
            this.mainpage.BackColor = System.Drawing.Color.White;
            this.mainpage.Location = new System.Drawing.Point(176, 58);
            this.mainpage.Name = "mainpage";
            this.mainpage.Size = new System.Drawing.Size(1050, 655);
            this.mainpage.TabIndex = 33;
            // 
            // ScheduleTableButton
            // 
            this.ScheduleTableButton.Location = new System.Drawing.Point(12, 80);
            this.ScheduleTableButton.Name = "ScheduleTableButton";
            this.ScheduleTableButton.Size = new System.Drawing.Size(112, 34);
            this.ScheduleTableButton.TabIndex = 34;
            this.ScheduleTableButton.Text = "ScheduleTable";
            this.ScheduleTableButton.UseVisualStyleBackColor = true;
            this.ScheduleTableButton.Click += new System.EventHandler(this.ScheduleTableButton_Click);
            // 
            // courseButton
            // 
            this.courseButton.Location = new System.Drawing.Point(12, 134);
            this.courseButton.Name = "courseButton";
            this.courseButton.Size = new System.Drawing.Size(112, 34);
            this.courseButton.TabIndex = 35;
            this.courseButton.Text = "course";
            this.courseButton.UseVisualStyleBackColor = true;
            this.courseButton.Click += new System.EventHandler(this.CourseButton_Click);
            // 
            // examButton
            // 
            this.examButton.Location = new System.Drawing.Point(12, 187);
            this.examButton.Name = "examButton";
            this.examButton.Size = new System.Drawing.Size(112, 34);
            this.examButton.TabIndex = 36;
            this.examButton.Text = "Exam";
            this.examButton.UseVisualStyleBackColor = true;
            this.examButton.Click += new System.EventHandler(this.ExamButton_Click);
            // 
            // GroupActivityButton
            // 
            this.GroupActivityButton.Location = new System.Drawing.Point(12, 240);
            this.GroupActivityButton.Name = "GroupActivityButton";
            this.GroupActivityButton.Size = new System.Drawing.Size(112, 34);
            this.GroupActivityButton.TabIndex = 37;
            this.GroupActivityButton.Text = "GroupActivity";
            this.GroupActivityButton.UseVisualStyleBackColor = true;
            this.GroupActivityButton.Click += new System.EventHandler(this.GroupActivityButton_Click);
            // 
            // personalActivityButton
            // 
            this.personalActivityButton.Location = new System.Drawing.Point(12, 293);
            this.personalActivityButton.Name = "personalActivityButton";
            this.personalActivityButton.Size = new System.Drawing.Size(112, 34);
            this.personalActivityButton.TabIndex = 38;
            this.personalActivityButton.Text = "PersonalActivity";
            this.personalActivityButton.UseVisualStyleBackColor = true;
            this.personalActivityButton.Click += new System.EventHandler(this.PersonalActivityButton_Click);
            // 
            // temporaryAffairButton
            // 
            this.temporaryAffairButton.Location = new System.Drawing.Point(12, 344);
            this.temporaryAffairButton.Name = "temporaryAffairButton";
            this.temporaryAffairButton.Size = new System.Drawing.Size(112, 34);
            this.temporaryAffairButton.TabIndex = 39;
            this.temporaryAffairButton.Text = "TemporaryAffair";
            this.temporaryAffairButton.UseVisualStyleBackColor = true;
            this.temporaryAffairButton.Click += new System.EventHandler(this.TemporaryAffairButton_Click);
            // 
            // StudentWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 770);
            this.Controls.Add(this.temporaryAffairButton);
            this.Controls.Add(this.personalActivityButton);
            this.Controls.Add(this.GroupActivityButton);
            this.Controls.Add(this.examButton);
            this.Controls.Add(this.courseButton);
            this.Controls.Add(this.ScheduleTableButton);
            this.Controls.Add(this.mainpage);
            this.Controls.Add(this.header);
            this.Controls.Add(this.mainpageButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StudentWindow";
            this.Text = "StudentWindow";
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainpageButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel header;
        private Label currentTime;
        private Button logoutButton;
        private PictureBox mainpageButton;
        private Panel mainpage;
        private Button ScheduleTableButton;
        private Button courseButton;
        private Button examButton;
        private Button GroupActivityButton;
        private Button personalActivityButton;
        private Button temporaryAffairButton;
        private ComboBox weekBox;
        private Button setTime;
        private Label label3;
        private ComboBox hourBox;
        private Label label2;
        private ComboBox dayBox;
        private Label label1;
        private Button speedButton;
    }
}