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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentWindow));
            this.header = new System.Windows.Forms.Panel();
            this.hourBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dayBox = new System.Windows.Forms.ComboBox();
            this.weekBox = new System.Windows.Forms.ComboBox();
            this.pauseButton = new System.Windows.Forms.Button();
            this.speedButton = new System.Windows.Forms.Button();
            this.setTime = new System.Windows.Forms.Button();
            this.daySetBox = new System.Windows.Forms.Label();
            this.weekSetBox = new System.Windows.Forms.Label();
            this.currentTime = new System.Windows.Forms.Label();
            this.mainpageButton = new System.Windows.Forms.PictureBox();
            this.mainpage = new System.Windows.Forms.Panel();
            this.ScheduleTableButton = new System.Windows.Forms.PictureBox();
            this.courseButton = new System.Windows.Forms.PictureBox();
            this.examButton = new System.Windows.Forms.PictureBox();
            this.GroupActivityButton = new System.Windows.Forms.PictureBox();
            this.personalActivityButton = new System.Windows.Forms.PictureBox();
            this.temporaryAffairButton = new System.Windows.Forms.PictureBox();
            this.exitButton = new System.Windows.Forms.PictureBox();
            this.logoutConfirm = new System.Windows.Forms.PictureBox();
            this.closeConfirm = new System.Windows.Forms.PictureBox();
            this.header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainpageButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleTableButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.courseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.examButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupActivityButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.personalActivityButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.temporaryAffairButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoutConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeConfirm)).BeginInit();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.Transparent;
            this.header.Controls.Add(this.hourBox);
            this.header.Controls.Add(this.label1);
            this.header.Controls.Add(this.dayBox);
            this.header.Controls.Add(this.weekBox);
            this.header.Controls.Add(this.pauseButton);
            this.header.Controls.Add(this.speedButton);
            this.header.Controls.Add(this.setTime);
            this.header.Controls.Add(this.daySetBox);
            this.header.Controls.Add(this.weekSetBox);
            this.header.Controls.Add(this.currentTime);
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(1300, 45);
            this.header.TabIndex = 1;
            this.header.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Header_MouseDown);
            this.header.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Header_MouseMove);
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
            this.hourBox.Location = new System.Drawing.Point(640, 5);
            this.hourBox.Name = "hourBox";
            this.hourBox.Size = new System.Drawing.Size(120, 32);
            this.hourBox.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(610, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 21);
            this.label1.TabIndex = 19;
            this.label1.Text = "时";
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
            this.dayBox.Location = new System.Drawing.Point(480, 5);
            this.dayBox.Name = "dayBox";
            this.dayBox.Size = new System.Drawing.Size(120, 32);
            this.dayBox.TabIndex = 10;
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
            this.weekBox.Location = new System.Drawing.Point(320, 5);
            this.weekBox.Name = "weekBox";
            this.weekBox.Size = new System.Drawing.Size(120, 32);
            this.weekBox.TabIndex = 8;
            // 
            // pauseButton
            // 
            this.pauseButton.BackColor = System.Drawing.Color.White;
            this.pauseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pauseButton.Location = new System.Drawing.Point(1000, 3);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(100, 35);
            this.pauseButton.TabIndex = 18;
            this.pauseButton.Text = "暂停";
            this.pauseButton.UseVisualStyleBackColor = false;
            this.pauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // speedButton
            // 
            this.speedButton.BackColor = System.Drawing.Color.White;
            this.speedButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.speedButton.Location = new System.Drawing.Point(890, 3);
            this.speedButton.Name = "speedButton";
            this.speedButton.Size = new System.Drawing.Size(100, 35);
            this.speedButton.TabIndex = 17;
            this.speedButton.Text = "快进";
            this.speedButton.UseVisualStyleBackColor = false;
            this.speedButton.Click += new System.EventHandler(this.SpeedButton_Click);
            // 
            // setTime
            // 
            this.setTime.BackColor = System.Drawing.Color.White;
            this.setTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.setTime.Location = new System.Drawing.Point(780, 3);
            this.setTime.Name = "setTime";
            this.setTime.Size = new System.Drawing.Size(100, 35);
            this.setTime.TabIndex = 16;
            this.setTime.Text = "SetTime";
            this.setTime.UseVisualStyleBackColor = false;
            this.setTime.Click += new System.EventHandler(this.SetTime_Click);
            // 
            // daySetBox
            // 
            this.daySetBox.AutoSize = true;
            this.daySetBox.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.daySetBox.ForeColor = System.Drawing.Color.White;
            this.daySetBox.Location = new System.Drawing.Point(450, 12);
            this.daySetBox.Name = "daySetBox";
            this.daySetBox.Size = new System.Drawing.Size(32, 21);
            this.daySetBox.TabIndex = 11;
            this.daySetBox.Text = "日";
            // 
            // weekSetBox
            // 
            this.weekSetBox.AutoSize = true;
            this.weekSetBox.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.weekSetBox.ForeColor = System.Drawing.Color.White;
            this.weekSetBox.Location = new System.Drawing.Point(290, 12);
            this.weekSetBox.Name = "weekSetBox";
            this.weekSetBox.Size = new System.Drawing.Size(32, 21);
            this.weekSetBox.TabIndex = 9;
            this.weekSetBox.Text = "周";
            // 
            // currentTime
            // 
            this.currentTime.AutoSize = true;
            this.currentTime.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.currentTime.ForeColor = System.Drawing.Color.Transparent;
            this.currentTime.Location = new System.Drawing.Point(12, 8);
            this.currentTime.Name = "currentTime";
            this.currentTime.Size = new System.Drawing.Size(80, 28);
            this.currentTime.TabIndex = 2;
            this.currentTime.Text = "Week i";
            // 
            // mainpageButton
            // 
            this.mainpageButton.BackColor = System.Drawing.Color.Transparent;
            this.mainpageButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.SubPageBG;
            this.mainpageButton.Location = new System.Drawing.Point(175, 53);
            this.mainpageButton.Name = "mainpageButton";
            this.mainpageButton.Size = new System.Drawing.Size(1060, 665);
            this.mainpageButton.TabIndex = 32;
            this.mainpageButton.TabStop = false;
            // 
            // mainpage
            // 
            this.mainpage.BackColor = System.Drawing.Color.White;
            this.mainpage.Location = new System.Drawing.Point(180, 58);
            this.mainpage.Name = "mainpage";
            this.mainpage.Size = new System.Drawing.Size(1050, 655);
            this.mainpage.TabIndex = 33;
            // 
            // ScheduleTableButton
            // 
            this.ScheduleTableButton.BackColor = System.Drawing.Color.Transparent;
            this.ScheduleTableButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ScheduleTableButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.日历时间__ffffff_128_20286020;
            this.ScheduleTableButton.Location = new System.Drawing.Point(65, 100);
            this.ScheduleTableButton.Name = "ScheduleTableButton";
            this.ScheduleTableButton.Size = new System.Drawing.Size(50, 50);
            this.ScheduleTableButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ScheduleTableButton.TabIndex = 40;
            this.ScheduleTableButton.TabStop = false;
            this.ScheduleTableButton.Click += new System.EventHandler(this.ScheduleTableButton_Click);
            // 
            // courseButton
            // 
            this.courseButton.BackColor = System.Drawing.Color.Transparent;
            this.courseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.courseButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.课表__ffffff_128_21601021;
            this.courseButton.Location = new System.Drawing.Point(65, 190);
            this.courseButton.Name = "courseButton";
            this.courseButton.Size = new System.Drawing.Size(50, 50);
            this.courseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.courseButton.TabIndex = 41;
            this.courseButton.TabStop = false;
            this.courseButton.Click += new System.EventHandler(this.CourseButton_Click);
            // 
            // examButton
            // 
            this.examButton.BackColor = System.Drawing.Color.Transparent;
            this.examButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.examButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.试卷__ffffff_128_21601018;
            this.examButton.Location = new System.Drawing.Point(65, 280);
            this.examButton.Name = "examButton";
            this.examButton.Size = new System.Drawing.Size(50, 50);
            this.examButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.examButton.TabIndex = 42;
            this.examButton.TabStop = false;
            this.examButton.Click += new System.EventHandler(this.ExamButton_Click);
            // 
            // GroupActivityButton
            // 
            this.GroupActivityButton.BackColor = System.Drawing.Color.Transparent;
            this.GroupActivityButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GroupActivityButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.户外实践__ffffff_128_21601017;
            this.GroupActivityButton.Location = new System.Drawing.Point(65, 370);
            this.GroupActivityButton.Name = "GroupActivityButton";
            this.GroupActivityButton.Size = new System.Drawing.Size(50, 50);
            this.GroupActivityButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.GroupActivityButton.TabIndex = 43;
            this.GroupActivityButton.TabStop = false;
            this.GroupActivityButton.Click += new System.EventHandler(this.GroupActivityButton_Click);
            // 
            // personalActivityButton
            // 
            this.personalActivityButton.BackColor = System.Drawing.Color.Transparent;
            this.personalActivityButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.personalActivityButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.跑步__ffffff_128_21590258;
            this.personalActivityButton.Location = new System.Drawing.Point(65, 460);
            this.personalActivityButton.Name = "personalActivityButton";
            this.personalActivityButton.Size = new System.Drawing.Size(50, 50);
            this.personalActivityButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.personalActivityButton.TabIndex = 44;
            this.personalActivityButton.TabStop = false;
            this.personalActivityButton.Click += new System.EventHandler(this.PersonalActivityButton_Click);
            // 
            // temporaryAffairButton
            // 
            this.temporaryAffairButton.BackColor = System.Drawing.Color.Transparent;
            this.temporaryAffairButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.temporaryAffairButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.个人账户__ffffff_128_21601167;
            this.temporaryAffairButton.Location = new System.Drawing.Point(65, 550);
            this.temporaryAffairButton.Name = "temporaryAffairButton";
            this.temporaryAffairButton.Size = new System.Drawing.Size(50, 50);
            this.temporaryAffairButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.temporaryAffairButton.TabIndex = 45;
            this.temporaryAffairButton.TabStop = false;
            this.temporaryAffairButton.Click += new System.EventHandler(this.PersonalActivityButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.BackColor = System.Drawing.Color.Transparent;
            this.exitButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exitButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.研究人员__ffffff_128_21601186;
            this.exitButton.Location = new System.Drawing.Point(65, 640);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(50, 50);
            this.exitButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitButton.TabIndex = 46;
            this.exitButton.TabStop = false;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // logoutConfirm
            // 
            this.logoutConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logoutConfirm.Image = global::StudentScheduleManagementSystem.Properties.Resources.退出__a880c2_128_3661364;
            this.logoutConfirm.Location = new System.Drawing.Point(500, 300);
            this.logoutConfirm.Name = "logoutConfirm";
            this.logoutConfirm.Size = new System.Drawing.Size(150, 150);
            this.logoutConfirm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoutConfirm.TabIndex = 47;
            this.logoutConfirm.TabStop = false;
            this.logoutConfirm.Visible = false;
            this.logoutConfirm.Click += new System.EventHandler(this.LogoutConfirm_Click);
            // 
            // closeConfirm
            // 
            this.closeConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeConfirm.Image = global::StudentScheduleManagementSystem.Properties.Resources.退出__a880c2_128_3656183;
            this.closeConfirm.Location = new System.Drawing.Point(750, 300);
            this.closeConfirm.Name = "closeConfirm";
            this.closeConfirm.Size = new System.Drawing.Size(150, 150);
            this.closeConfirm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.closeConfirm.TabIndex = 48;
            this.closeConfirm.TabStop = false;
            this.closeConfirm.Visible = false;
            this.closeConfirm.Click += new System.EventHandler(this.CloseConfirm_Click);
            // 
            // StudentWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1300, 770);
            this.Controls.Add(this.logoutConfirm);
            this.Controls.Add(this.closeConfirm);
            this.Controls.Add(this.exitButton);
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
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleTableButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.courseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.examButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupActivityButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.personalActivityButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.temporaryAffairButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoutConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeConfirm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel header;
        private Label currentTime;
        private PictureBox mainpageButton;
        private Panel mainpage;
        private ComboBox weekBox;
        private Button setTime;
        private ComboBox hourBox;
        private Label daySetBox;
        private ComboBox dayBox;
        private Label weekSetBox;
        private Button speedButton;
        private Button pauseButton;
        private PictureBox ScheduleTableButton;
        private PictureBox courseButton;
        private PictureBox examButton;
        private PictureBox GroupActivityButton;
        private PictureBox personalActivityButton;
        private PictureBox temporaryAffairButton;
        private Label label1;
        private PictureBox exitButton;
        private PictureBox logoutConfirm;
        private PictureBox closeConfirm;
    }
}