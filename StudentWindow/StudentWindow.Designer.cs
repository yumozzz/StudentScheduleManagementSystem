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
            header = new Panel();
            hourBox = new ComboBox();
            label1 = new Label();
            dayBox = new ComboBox();
            weekBox = new ComboBox();
            pauseButton = new Button();
            speedButton = new Button();
            setTime = new Button();
            daySetBox = new Label();
            weekSetBox = new Label();
            currentTime = new Label();
            mainpageButton = new PictureBox();
            mainpage = new Panel();
            ScheduleTableButton = new PictureBox();
            courseButton = new PictureBox();
            examButton = new PictureBox();
            GroupActivityButton = new PictureBox();
            personalActivityButton = new PictureBox();
            temporaryAffairButton = new PictureBox();
            exitButton = new PictureBox();
            logoutConfirm = new PictureBox();
            closeConfirm = new PictureBox();
            header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mainpageButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScheduleTableButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)courseButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)examButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GroupActivityButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)personalActivityButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)temporaryAffairButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)exitButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)logoutConfirm).BeginInit();
            ((System.ComponentModel.ISupportInitialize)closeConfirm).BeginInit();
            SuspendLayout();
            // 
            // header
            // 
            header.BackColor = Color.Transparent;
            header.Controls.Add(hourBox);
            header.Controls.Add(label1);
            header.Controls.Add(dayBox);
            header.Controls.Add(weekBox);
            header.Controls.Add(pauseButton);
            header.Controls.Add(speedButton);
            header.Controls.Add(setTime);
            header.Controls.Add(daySetBox);
            header.Controls.Add(weekSetBox);
            header.Controls.Add(currentTime);
            header.Location = new Point(0, 0);
            header.Name = "header";
            header.Size = new Size(1300, 45);
            header.TabIndex = 1;
            header.MouseDown += Header_MouseDown;
            header.MouseMove += Header_MouseMove;
            // 
            // hourBox
            // 
            hourBox.DropDownStyle = ComboBoxStyle.DropDownList;
            hourBox.FormattingEnabled = true;
            hourBox.Items.AddRange(new object[] { "0:00", "1:00", "2:00", "3:00", "4:00", "5:00", "6:00", "7:00", "8:00", "9:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00" });
            hourBox.Location = new Point(640, 5);
            hourBox.Name = "hourBox";
            hourBox.Size = new Size(120, 32);
            hourBox.TabIndex = 12;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("黑体", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(610, 12);
            label1.Name = "label1";
            label1.Size = new Size(32, 21);
            label1.TabIndex = 19;
            label1.Text = "时";
            // 
            // dayBox
            // 
            dayBox.DropDownStyle = ComboBoxStyle.DropDownList;
            dayBox.FormattingEnabled = true;
            dayBox.Items.AddRange(new object[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" });
            dayBox.Location = new Point(480, 5);
            dayBox.Name = "dayBox";
            dayBox.Size = new Size(120, 32);
            dayBox.TabIndex = 10;
            // 
            // weekBox
            // 
            weekBox.DropDownStyle = ComboBoxStyle.DropDownList;
            weekBox.FormattingEnabled = true;
            weekBox.Items.AddRange(new object[] { "Week1", "Week2", "Week3", "Week4", "Week5", "Week6", "Week7", "Week8", "Week9", "Week10", "Week11", "Week12", "Week13", "Week14", "Week15", "Week16" });
            weekBox.Location = new Point(320, 5);
            weekBox.Name = "weekBox";
            weekBox.Size = new Size(120, 32);
            weekBox.TabIndex = 8;
            // 
            // pauseButton
            // 
            pauseButton.BackColor = Color.White;
            pauseButton.FlatStyle = FlatStyle.Flat;
            pauseButton.Location = new Point(1000, 3);
            pauseButton.Name = "pauseButton";
            pauseButton.Size = new Size(100, 35);
            pauseButton.TabIndex = 18;
            pauseButton.Text = "暂停";
            pauseButton.UseVisualStyleBackColor = false;
            pauseButton.Click += PauseButton_Click;
            // 
            // speedButton
            // 
            speedButton.BackColor = Color.White;
            speedButton.FlatStyle = FlatStyle.Flat;
            speedButton.Location = new Point(890, 3);
            speedButton.Name = "speedButton";
            speedButton.Size = new Size(100, 35);
            speedButton.TabIndex = 17;
            speedButton.Text = "快进";
            speedButton.UseVisualStyleBackColor = false;
            speedButton.Click += SpeedButton_Click;
            // 
            // setTime
            // 
            setTime.BackColor = Color.White;
            setTime.FlatStyle = FlatStyle.Flat;
            setTime.Location = new Point(780, 3);
            setTime.Name = "setTime";
            setTime.Size = new Size(100, 35);
            setTime.TabIndex = 16;
            setTime.Text = "设置时间";
            setTime.UseVisualStyleBackColor = false;
            setTime.Click += SetTime_Click;
            // 
            // daySetBox
            // 
            daySetBox.AutoSize = true;
            daySetBox.Font = new Font("黑体", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            daySetBox.ForeColor = Color.White;
            daySetBox.Location = new Point(450, 12);
            daySetBox.Name = "daySetBox";
            daySetBox.Size = new Size(32, 21);
            daySetBox.TabIndex = 11;
            daySetBox.Text = "日";
            // 
            // weekSetBox
            // 
            weekSetBox.AutoSize = true;
            weekSetBox.Font = new Font("黑体", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            weekSetBox.ForeColor = Color.White;
            weekSetBox.Location = new Point(290, 12);
            weekSetBox.Name = "weekSetBox";
            weekSetBox.Size = new Size(32, 21);
            weekSetBox.TabIndex = 9;
            weekSetBox.Text = "周";
            // 
            // currentTime
            // 
            currentTime.AutoSize = true;
            currentTime.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            currentTime.ForeColor = Color.Transparent;
            currentTime.Location = new Point(12, 8);
            currentTime.Name = "currentTime";
            currentTime.Size = new Size(80, 28);
            currentTime.TabIndex = 2;
            currentTime.Text = "Week i";
            // 
            // mainpageButton
            // 
            mainpageButton.BackColor = Color.Transparent;
            mainpageButton.Image = Properties.Resources.SubPageBG;
            mainpageButton.Location = new Point(175, 53);
            mainpageButton.Name = "mainpageButton";
            mainpageButton.Size = new Size(1060, 665);
            mainpageButton.TabIndex = 32;
            mainpageButton.TabStop = false;
            // 
            // mainpage
            // 
            mainpage.BackColor = Color.White;
            mainpage.Location = new Point(180, 58);
            mainpage.Name = "mainpage";
            mainpage.Size = new Size(1050, 655);
            mainpage.TabIndex = 33;
            // 
            // ScheduleTableButton
            // 
            ScheduleTableButton.BackColor = Color.Transparent;
            ScheduleTableButton.Cursor = Cursors.Hand;
            ScheduleTableButton.Image = Properties.Resources.日历时间__ffffff_128_20286020;
            ScheduleTableButton.Location = new Point(65, 100);
            ScheduleTableButton.Name = "ScheduleTableButton";
            ScheduleTableButton.Size = new Size(50, 50);
            ScheduleTableButton.SizeMode = PictureBoxSizeMode.Zoom;
            ScheduleTableButton.TabIndex = 40;
            ScheduleTableButton.TabStop = false;
            ScheduleTableButton.Click += ScheduleTableButton_Click;
            // 
            // courseButton
            // 
            courseButton.BackColor = Color.Transparent;
            courseButton.Cursor = Cursors.Hand;
            courseButton.Image = Properties.Resources.课表__ffffff_128_21601021;
            courseButton.Location = new Point(65, 190);
            courseButton.Name = "courseButton";
            courseButton.Size = new Size(50, 50);
            courseButton.SizeMode = PictureBoxSizeMode.Zoom;
            courseButton.TabIndex = 41;
            courseButton.TabStop = false;
            courseButton.Click += CourseButton_Click;
            // 
            // examButton
            // 
            examButton.BackColor = Color.Transparent;
            examButton.Cursor = Cursors.Hand;
            examButton.Image = Properties.Resources.试卷__ffffff_128_21601018;
            examButton.Location = new Point(65, 280);
            examButton.Name = "examButton";
            examButton.Size = new Size(50, 50);
            examButton.SizeMode = PictureBoxSizeMode.Zoom;
            examButton.TabIndex = 42;
            examButton.TabStop = false;
            examButton.Click += ExamButton_Click;
            // 
            // GroupActivityButton
            // 
            GroupActivityButton.BackColor = Color.Transparent;
            GroupActivityButton.Cursor = Cursors.Hand;
            GroupActivityButton.Image = Properties.Resources.户外实践__ffffff_128_21601017;
            GroupActivityButton.Location = new Point(65, 370);
            GroupActivityButton.Name = "GroupActivityButton";
            GroupActivityButton.Size = new Size(50, 50);
            GroupActivityButton.SizeMode = PictureBoxSizeMode.Zoom;
            GroupActivityButton.TabIndex = 43;
            GroupActivityButton.TabStop = false;
            GroupActivityButton.Click += GroupActivityButton_Click;
            // 
            // personalActivityButton
            // 
            personalActivityButton.BackColor = Color.Transparent;
            personalActivityButton.Cursor = Cursors.Hand;
            personalActivityButton.Image = Properties.Resources.跑步__ffffff_128_21590258;
            personalActivityButton.Location = new Point(65, 460);
            personalActivityButton.Name = "personalActivityButton";
            personalActivityButton.Size = new Size(50, 50);
            personalActivityButton.SizeMode = PictureBoxSizeMode.Zoom;
            personalActivityButton.TabIndex = 44;
            personalActivityButton.TabStop = false;
            personalActivityButton.Click += PersonalActivityButton_Click;
            // 
            // temporaryAffairButton
            // 
            temporaryAffairButton.BackColor = Color.Transparent;
            temporaryAffairButton.Cursor = Cursors.Hand;
            temporaryAffairButton.Image = Properties.Resources.个人账户__ffffff_128_21601167;
            temporaryAffairButton.Location = new Point(65, 550);
            temporaryAffairButton.Name = "temporaryAffairButton";
            temporaryAffairButton.Size = new Size(50, 50);
            temporaryAffairButton.SizeMode = PictureBoxSizeMode.Zoom;
            temporaryAffairButton.TabIndex = 45;
            temporaryAffairButton.TabStop = false;
            temporaryAffairButton.Click += TemporaryAffairButton_Click;
            // 
            // exitButton
            // 
            exitButton.BackColor = Color.Transparent;
            exitButton.Cursor = Cursors.Hand;
            exitButton.Image = Properties.Resources.研究人员__ffffff_128_21601186;
            exitButton.Location = new Point(65, 640);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(50, 50);
            exitButton.SizeMode = PictureBoxSizeMode.Zoom;
            exitButton.TabIndex = 46;
            exitButton.TabStop = false;
            exitButton.Click += ExitButton_Click;
            // 
            // logoutConfirm
            // 
            logoutConfirm.Cursor = Cursors.Hand;
            logoutConfirm.Image = Properties.Resources.退出__a880c2_128_3661364;
            logoutConfirm.Location = new Point(500, 300);
            logoutConfirm.Name = "logoutConfirm";
            logoutConfirm.Size = new Size(150, 150);
            logoutConfirm.SizeMode = PictureBoxSizeMode.Zoom;
            logoutConfirm.TabIndex = 47;
            logoutConfirm.TabStop = false;
            logoutConfirm.Visible = false;
            logoutConfirm.Click += LogoutConfirm_Click;
            // 
            // closeConfirm
            // 
            closeConfirm.Cursor = Cursors.Hand;
            closeConfirm.Image = Properties.Resources.退出__a880c2_128_3656183;
            closeConfirm.Location = new Point(750, 300);
            closeConfirm.Name = "closeConfirm";
            closeConfirm.Size = new Size(150, 150);
            closeConfirm.SizeMode = PictureBoxSizeMode.Zoom;
            closeConfirm.TabIndex = 48;
            closeConfirm.TabStop = false;
            closeConfirm.Visible = false;
            closeConfirm.Click += CloseConfirm_Click;
            // 
            // StudentWindow
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1300, 770);
            Controls.Add(logoutConfirm);
            Controls.Add(closeConfirm);
            Controls.Add(exitButton);
            Controls.Add(temporaryAffairButton);
            Controls.Add(personalActivityButton);
            Controls.Add(GroupActivityButton);
            Controls.Add(examButton);
            Controls.Add(courseButton);
            Controls.Add(ScheduleTableButton);
            Controls.Add(mainpage);
            Controls.Add(header);
            Controls.Add(mainpageButton);
            FormBorderStyle = FormBorderStyle.None;
            Name = "StudentWindow";
            Text = "StudentWindow";
            header.ResumeLayout(false);
            header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)mainpageButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScheduleTableButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)courseButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)examButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)GroupActivityButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)personalActivityButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)temporaryAffairButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)exitButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)logoutConfirm).EndInit();
            ((System.ComponentModel.ISupportInitialize)closeConfirm).EndInit();
            ResumeLayout(false);
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