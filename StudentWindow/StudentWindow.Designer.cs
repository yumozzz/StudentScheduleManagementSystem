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
            this.header.Controls.Add(this.currentTime);
            this.header.Controls.Add(this.logoutButton);
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(1300, 45);
            this.header.TabIndex = 1;
            this.header.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Header_MouseDown);
            this.header.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Header_MouseMove);
            // 
            // currentTime
            // 
            this.currentTime.AutoSize = true;
            this.currentTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.currentTime.Location = new System.Drawing.Point(12, 11);
            this.currentTime.Name = "currentTime";
            this.currentTime.Size = new System.Drawing.Size(70, 26);
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
            // exam
            // 
            this.examButton.Location = new System.Drawing.Point(12, 187);
            this.examButton.Name = "examButton";
            this.examButton.Size = new System.Drawing.Size(112, 34);
            this.examButton.TabIndex = 36;
            this.examButton.Text = "Exam";
            this.examButton.UseVisualStyleBackColor = true;
            this.examButton.Click += new System.EventHandler(this.ExamButton_Click);
            // 
            // GroupActivity
            // 
            this.GroupActivityButton.Location = new System.Drawing.Point(12, 240);
            this.GroupActivityButton.Name = "GroupActivityButton";
            this.GroupActivityButton.Size = new System.Drawing.Size(112, 34);
            this.GroupActivityButton.TabIndex = 37;
            this.GroupActivityButton.Text = "GroupActivity";
            this.GroupActivityButton.UseVisualStyleBackColor = true;
            this.GroupActivityButton.Click += new System.EventHandler(this.GroupActivityButton_Click);
            // 
            // personalActivity
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
    }
}