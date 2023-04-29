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
            this.Header = new System.Windows.Forms.Panel();
            this.SubPage = new System.Windows.Forms.Panel();
            this.LogoutClose = new System.Windows.Forms.PictureBox();
            this.CourseOP = new System.Windows.Forms.PictureBox();
            this.ActivityOP = new System.Windows.Forms.PictureBox();
            this.SubPageBG = new System.Windows.Forms.PictureBox();
            this.Logout = new System.Windows.Forms.PictureBox();
            this.ClosePage = new System.Windows.Forms.PictureBox();
            this.TestOP = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LogoutClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CourseOP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActivityOP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubPageBG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TestOP)).BeginInit();
            this.SuspendLayout();
            // 
            // Header
            // 
            this.Header.BackColor = System.Drawing.Color.Transparent;
            this.Header.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(1300, 45);
            this.Header.TabIndex = 23;
            this.Header.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Header_MouseDown);
            this.Header.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Header_MouseMove);
            // 
            // SubPage
            // 
            this.SubPage.BackColor = System.Drawing.Color.White;
            this.SubPage.Location = new System.Drawing.Point(180, 58);
            this.SubPage.Name = "SubPage";
            this.SubPage.Size = new System.Drawing.Size(1050, 655);
            this.SubPage.TabIndex = 25;
            // 
            // LogoutClose
            // 
            this.LogoutClose.BackColor = System.Drawing.Color.Transparent;
            this.LogoutClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LogoutClose.Image = global::StudentScheduleManagementSystem.Properties.Resources.研究人员__ffffff_128_21601186;
            this.LogoutClose.Location = new System.Drawing.Point(65, 540);
            this.LogoutClose.Name = "LogoutClose";
            this.LogoutClose.Size = new System.Drawing.Size(50, 50);
            this.LogoutClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.LogoutClose.TabIndex = 26;
            this.LogoutClose.TabStop = false;
            this.LogoutClose.Click += new System.EventHandler(this.LogoutClose_Click);
            // 
            // CourseOP
            // 
            this.CourseOP.BackColor = System.Drawing.Color.Transparent;
            this.CourseOP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CourseOP.Image = global::StudentScheduleManagementSystem.Properties.Resources.课表__ffffff_128_21601021;
            this.CourseOP.Location = new System.Drawing.Point(65, 180);
            this.CourseOP.Name = "CourseOP";
            this.CourseOP.Size = new System.Drawing.Size(50, 50);
            this.CourseOP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CourseOP.TabIndex = 27;
            this.CourseOP.TabStop = false;
            this.CourseOP.Click += new System.EventHandler(this.CourseOP_Click);
            // 
            // ActivityOP
            // 
            this.ActivityOP.BackColor = System.Drawing.Color.Transparent;
            this.ActivityOP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ActivityOP.Image = global::StudentScheduleManagementSystem.Properties.Resources.户外实践__ffffff_128_21601017;
            this.ActivityOP.Location = new System.Drawing.Point(65, 420);
            this.ActivityOP.Name = "ActivityOP";
            this.ActivityOP.Size = new System.Drawing.Size(50, 50);
            this.ActivityOP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ActivityOP.TabIndex = 28;
            this.ActivityOP.TabStop = false;
            this.ActivityOP.Click += new System.EventHandler(this.ActivityOP_Click);
            // 
            // SubPageBG
            // 
            this.SubPageBG.BackColor = System.Drawing.Color.Transparent;
            this.SubPageBG.Image = global::StudentScheduleManagementSystem.Properties.Resources.SubPageBG;
            this.SubPageBG.Location = new System.Drawing.Point(175, 53);
            this.SubPageBG.Name = "SubPageBG";
            this.SubPageBG.Size = new System.Drawing.Size(1060, 665);
            this.SubPageBG.TabIndex = 31;
            this.SubPageBG.TabStop = false;
            // 
            // Logout
            // 
            this.Logout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Logout.Image = global::StudentScheduleManagementSystem.Properties.Resources.退出__a880c2_128_3661364;
            this.Logout.Location = new System.Drawing.Point(500, 300);
            this.Logout.Name = "Logout";
            this.Logout.Size = new System.Drawing.Size(150, 150);
            this.Logout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Logout.TabIndex = 33;
            this.Logout.TabStop = false;
            this.Logout.Click += new System.EventHandler(this.Logout_Click);
            // 
            // ClosePage
            // 
            this.ClosePage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ClosePage.Image = global::StudentScheduleManagementSystem.Properties.Resources.退出__a880c2_128_3656183;
            this.ClosePage.Location = new System.Drawing.Point(750, 300);
            this.ClosePage.Name = "ClosePage";
            this.ClosePage.Size = new System.Drawing.Size(150, 150);
            this.ClosePage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ClosePage.TabIndex = 34;
            this.ClosePage.TabStop = false;
            this.ClosePage.Click += new System.EventHandler(this.ClosePage_Click);
            // 
            // TestOP
            // 
            this.TestOP.BackColor = System.Drawing.Color.Transparent;
            this.TestOP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TestOP.Image = global::StudentScheduleManagementSystem.Properties.Resources.试卷__ffffff_128_21601018;
            this.TestOP.Location = new System.Drawing.Point(65, 300);
            this.TestOP.Name = "TestOP";
            this.TestOP.Size = new System.Drawing.Size(50, 50);
            this.TestOP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.TestOP.TabIndex = 29;
            this.TestOP.TabStop = false;
            this.TestOP.Click += new System.EventHandler(this.TestOP_Click);
            // 
            // AdminWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1300, 770);
            this.Controls.Add(this.TestOP);
            this.Controls.Add(this.ActivityOP);
            this.Controls.Add(this.CourseOP);
            this.Controls.Add(this.LogoutClose);
            this.Controls.Add(this.Header);
            this.Controls.Add(this.Logout);
            this.Controls.Add(this.ClosePage);
            this.Controls.Add(this.SubPage);
            this.Controls.Add(this.SubPageBG);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminWindow";
            this.Text = "AdminWindow";
            ((System.ComponentModel.ISupportInitialize)(this.LogoutClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CourseOP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActivityOP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubPageBG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TestOP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Panel Header;
        private Panel SubPage;
        private PictureBox LogoutClose;
        private PictureBox CourseOP;
        private PictureBox ActivityOP;
        private PictureBox SubPageBG;
        private PictureBox Logout;
        private PictureBox ClosePage;
        private PictureBox TestOP;
    }
}