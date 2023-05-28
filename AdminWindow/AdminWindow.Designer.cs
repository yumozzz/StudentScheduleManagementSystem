namespace StudentScheduleManagementSystem.UI
{
    public partial class AdminWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminWindow));
            this.header = new System.Windows.Forms.Panel();
            this.mainpage = new System.Windows.Forms.Panel();
            this.logListBox = new System.Windows.Forms.ListBox();
            this.exitButton = new System.Windows.Forms.PictureBox();
            this.courseButton = new System.Windows.Forms.PictureBox();
            this.activityButton = new System.Windows.Forms.PictureBox();
            this.mainpageButton = new System.Windows.Forms.PictureBox();
            this.logoutConfirm = new System.Windows.Forms.PictureBox();
            this.closeConfirm = new System.Windows.Forms.PictureBox();
            this.examButton = new System.Windows.Forms.PictureBox();
            this.mapEditButton = new System.Windows.Forms.PictureBox();
            this.logButton = new System.Windows.Forms.PictureBox();
            this.mainpage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exitButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.courseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainpageButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoutConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.examButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapEditButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logButton)).BeginInit();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.Transparent;
            this.header.ForeColor = System.Drawing.SystemColors.ControlText;
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(1300, 45);
            this.header.TabIndex = 23;
            this.header.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Header_MouseDown);
            this.header.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Header_MouseMove);
            // 
            // mainpage
            // 
            this.mainpage.BackColor = System.Drawing.Color.White;
            this.mainpage.Controls.Add(this.logListBox);
            this.mainpage.Location = new System.Drawing.Point(180, 58);
            this.mainpage.Name = "mainpage";
            this.mainpage.Size = new System.Drawing.Size(1050, 655);
            this.mainpage.TabIndex = 25;
            // 
            // logListBox
            // 
            this.logListBox.BackColor = System.Drawing.Color.White;
            this.logListBox.FormattingEnabled = true;
            this.logListBox.ItemHeight = 24;
            this.logListBox.Location = new System.Drawing.Point(10, 13);
            this.logListBox.Name = "logListBox";
            this.logListBox.Size = new System.Drawing.Size(1030, 628);
            this.logListBox.TabIndex = 0;
            // 
            // exitButton
            // 
            this.exitButton.BackColor = System.Drawing.Color.Transparent;
            this.exitButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exitButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.研究人员__ffffff_128_21601186;
            this.exitButton.Location = new System.Drawing.Point(65, 610);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(50, 50);
            this.exitButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitButton.TabIndex = 26;
            this.exitButton.TabStop = false;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // courseButton
            // 
            this.courseButton.BackColor = System.Drawing.Color.Transparent;
            this.courseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.courseButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.课表__ffffff_128_21601021;
            this.courseButton.Location = new System.Drawing.Point(65, 130);
            this.courseButton.Name = "courseButton";
            this.courseButton.Size = new System.Drawing.Size(50, 50);
            this.courseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.courseButton.TabIndex = 27;
            this.courseButton.TabStop = false;
            this.courseButton.Click += new System.EventHandler(this.CourseButton_Click);
            // 
            // activityButton
            // 
            this.activityButton.BackColor = System.Drawing.Color.Transparent;
            this.activityButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.activityButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.户外实践__ffffff_128_21601017;
            this.activityButton.Location = new System.Drawing.Point(65, 322);
            this.activityButton.Name = "activityButton";
            this.activityButton.Size = new System.Drawing.Size(50, 50);
            this.activityButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.activityButton.TabIndex = 28;
            this.activityButton.TabStop = false;
            this.activityButton.Click += new System.EventHandler(this.ActivityButton_Click);
            // 
            // mainpageButton
            // 
            this.mainpageButton.BackColor = System.Drawing.Color.Transparent;
            this.mainpageButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.SubPageBG;
            this.mainpageButton.Location = new System.Drawing.Point(175, 53);
            this.mainpageButton.Name = "mainpageButton";
            this.mainpageButton.Size = new System.Drawing.Size(1060, 665);
            this.mainpageButton.TabIndex = 31;
            this.mainpageButton.TabStop = false;
            // 
            // logoutConfirm
            // 
            this.logoutConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logoutConfirm.Image = global::StudentScheduleManagementSystem.Properties.Resources.退出__a880c2_128_3661364;
            this.logoutConfirm.Location = new System.Drawing.Point(500, 300);
            this.logoutConfirm.Name = "logoutConfirm";
            this.logoutConfirm.Size = new System.Drawing.Size(150, 150);
            this.logoutConfirm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoutConfirm.TabIndex = 33;
            this.logoutConfirm.TabStop = false;
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
            this.closeConfirm.TabIndex = 34;
            this.closeConfirm.TabStop = false;
            this.closeConfirm.Click += new System.EventHandler(this.CloseConfirm_Click);
            // 
            // examButton
            // 
            this.examButton.BackColor = System.Drawing.Color.Transparent;
            this.examButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.examButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.试卷__ffffff_128_21601018;
            this.examButton.Location = new System.Drawing.Point(65, 226);
            this.examButton.Name = "examButton";
            this.examButton.Size = new System.Drawing.Size(50, 50);
            this.examButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.examButton.TabIndex = 29;
            this.examButton.TabStop = false;
            this.examButton.Click += new System.EventHandler(this.ExamButton_Click);
            // 
            // mapEditButton
            // 
            this.mapEditButton.BackColor = System.Drawing.Color.Transparent;
            this.mapEditButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mapEditButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.地图__ffffff_128_21601004;
            this.mapEditButton.Location = new System.Drawing.Point(65, 418);
            this.mapEditButton.Name = "mapEditButton";
            this.mapEditButton.Size = new System.Drawing.Size(50, 50);
            this.mapEditButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mapEditButton.TabIndex = 35;
            this.mapEditButton.TabStop = false;
            this.mapEditButton.Click += new System.EventHandler(this.MapEditButton_Click);
            // 
            // logButton
            // 
            this.logButton.BackColor = System.Drawing.Color.Transparent;
            this.logButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.党员日志__ffffff_128_21601025;
            this.logButton.Location = new System.Drawing.Point(65, 514);
            this.logButton.Name = "logButton";
            this.logButton.Size = new System.Drawing.Size(50, 50);
            this.logButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logButton.TabIndex = 36;
            this.logButton.TabStop = false;
            this.logButton.Click += new System.EventHandler(this.LogButton_Click);
            // 
            // AdminWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1300, 770);
            this.Controls.Add(this.closeConfirm);
            this.Controls.Add(this.logButton);
            this.Controls.Add(this.mapEditButton);
            this.Controls.Add(this.examButton);
            this.Controls.Add(this.activityButton);
            this.Controls.Add(this.courseButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.header);
            this.Controls.Add(this.logoutConfirm);
            this.Controls.Add(this.mainpage);
            this.Controls.Add(this.mainpageButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminWindow";
            this.Text = "AdminWindow";
            this.mainpage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.exitButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.courseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainpageButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoutConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.examButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapEditButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel header;
        private Panel mainpage;
        private PictureBox exitButton;
        private PictureBox courseButton;
        private PictureBox activityButton;
        private PictureBox mainpageButton;
        private PictureBox logoutConfirm;
        private PictureBox closeConfirm;
        private PictureBox examButton;
        private PictureBox mapEditButton;
        private ListBox logListBox;
        private PictureBox logButton;
    }
}