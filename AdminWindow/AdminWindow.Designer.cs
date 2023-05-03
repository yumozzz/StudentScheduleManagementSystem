namespace StudentScheduleManagementSystem.UI
{
    partial class AdminWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminWindow));
            this.header = new System.Windows.Forms.Panel();
            this.mainpage = new System.Windows.Forms.Panel();
            this.logoutButton = new System.Windows.Forms.PictureBox();
            this.courseButton = new System.Windows.Forms.PictureBox();
            this.activityButton = new System.Windows.Forms.PictureBox();
            this.mainpageButton = new System.Windows.Forms.PictureBox();
            this.logoutConfirm = new System.Windows.Forms.PictureBox();
            this.closeConfirm = new System.Windows.Forms.PictureBox();
            this.testOperation = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.logoutButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.courseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainpageButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoutConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testOperation)).BeginInit();
            this.SuspendLayout();
            // 
            // Header
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
            // SubPage
            // 
            this.mainpage.BackColor = System.Drawing.Color.White;
            this.mainpage.Location = new System.Drawing.Point(180, 58);
            this.mainpage.Name = "mainpage";
            this.mainpage.Size = new System.Drawing.Size(1050, 655);
            this.mainpage.TabIndex = 25;
            // 
            // LogoutClose
            // 
            this.logoutButton.BackColor = System.Drawing.Color.Transparent;
            this.logoutButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logoutButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.研究人员__ffffff_128_21601186;
            this.logoutButton.Location = new System.Drawing.Point(65, 540);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(50, 50);
            this.logoutButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoutButton.TabIndex = 26;
            this.logoutButton.TabStop = false;
            this.logoutButton.Click += new System.EventHandler(this.LogoutClose_Click);
            // 
            // CourseOP
            // 
            this.courseButton.BackColor = System.Drawing.Color.Transparent;
            this.courseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.courseButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.课表__ffffff_128_21601021;
            this.courseButton.Location = new System.Drawing.Point(65, 180);
            this.courseButton.Name = "courseButton";
            this.courseButton.Size = new System.Drawing.Size(50, 50);
            this.courseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.courseButton.TabIndex = 27;
            this.courseButton.TabStop = false;
            this.courseButton.Click += new System.EventHandler(this.CourseManagement_Click);
            // 
            // ActivityOP
            // 
            this.activityButton.BackColor = System.Drawing.Color.Transparent;
            this.activityButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.activityButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.户外实践__ffffff_128_21601017;
            this.activityButton.Location = new System.Drawing.Point(65, 420);
            this.activityButton.Name = "activityButton";
            this.activityButton.Size = new System.Drawing.Size(50, 50);
            this.activityButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.activityButton.TabIndex = 28;
            this.activityButton.TabStop = false;
            this.activityButton.Click += new System.EventHandler(this.ActivityManagement_Click);
            // 
            // SubPageBG
            // 
            this.mainpageButton.BackColor = System.Drawing.Color.Transparent;
            this.mainpageButton.Image = global::StudentScheduleManagementSystem.Properties.Resources.SubPageBG;
            this.mainpageButton.Location = new System.Drawing.Point(175, 53);
            this.mainpageButton.Name = "mainpageButton";
            this.mainpageButton.Size = new System.Drawing.Size(1060, 665);
            this.mainpageButton.TabIndex = 31;
            this.mainpageButton.TabStop = false;
            // 
            // Logout
            // 
            this.logoutConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logoutConfirm.Image = global::StudentScheduleManagementSystem.Properties.Resources.退出__a880c2_128_3661364;
            this.logoutConfirm.Location = new System.Drawing.Point(500, 300);
            this.logoutConfirm.Name = "logoutConfirm";
            this.logoutConfirm.Size = new System.Drawing.Size(150, 150);
            this.logoutConfirm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoutConfirm.TabIndex = 33;
            this.logoutConfirm.TabStop = false;
            this.logoutConfirm.Click += new System.EventHandler(this.Logout_Click);
            // 
            // ClosePage
            // 
            this.closeConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeConfirm.Image = global::StudentScheduleManagementSystem.Properties.Resources.退出__a880c2_128_3656183;
            this.closeConfirm.Location = new System.Drawing.Point(750, 300);
            this.closeConfirm.Name = "closeConfirm";
            this.closeConfirm.Size = new System.Drawing.Size(150, 150);
            this.closeConfirm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.closeConfirm.TabIndex = 34;
            this.closeConfirm.TabStop = false;
            this.closeConfirm.Click += new System.EventHandler(this.ClosePage_Click);
            // 
            // TestOP
            // 
            this.testOperation.BackColor = System.Drawing.Color.Transparent;
            this.testOperation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.testOperation.Image = global::StudentScheduleManagementSystem.Properties.Resources.试卷__ffffff_128_21601018;
            this.testOperation.Location = new System.Drawing.Point(65, 300);
            this.testOperation.Name = "testOperation";
            this.testOperation.Size = new System.Drawing.Size(50, 50);
            this.testOperation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.testOperation.TabIndex = 29;
            this.testOperation.TabStop = false;
            this.testOperation.Click += new System.EventHandler(this.TestManagement_Click);
            // 
            // AdminWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1300, 770);
            this.Controls.Add(this.testOperation);
            this.Controls.Add(this.activityButton);
            this.Controls.Add(this.courseButton);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.header);
            this.Controls.Add(this.logoutConfirm);
            this.Controls.Add(this.closeConfirm);
            this.Controls.Add(this.mainpage);
            this.Controls.Add(this.mainpageButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminWindow";
            this.Text = "AdminWindow";
            ((System.ComponentModel.ISupportInitialize)(this.logoutButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.courseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainpageButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoutConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testOperation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel header;
        private Panel mainpage;
        private PictureBox logoutButton;
        private PictureBox courseButton;
        private PictureBox activityButton;
        private PictureBox mainpageButton;
        private PictureBox logoutConfirm;
        private PictureBox closeConfirm;
        private PictureBox testOperation;
    }
}