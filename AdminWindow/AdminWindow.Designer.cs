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
            header = new Panel();
            mainpage = new Panel();
            logListBox = new ListBox();
            exitButton = new PictureBox();
            courseButton = new PictureBox();
            activityButton = new PictureBox();
            mainpageButton = new PictureBox();
            logoutConfirm = new PictureBox();
            closeConfirm = new PictureBox();
            examButton = new PictureBox();
            mapEditButton = new PictureBox();
            logButton = new PictureBox();
            mainpage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)exitButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)courseButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)activityButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mainpageButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)logoutConfirm).BeginInit();
            ((System.ComponentModel.ISupportInitialize)closeConfirm).BeginInit();
            ((System.ComponentModel.ISupportInitialize)examButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mapEditButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)logButton).BeginInit();
            SuspendLayout();
            // 
            // header
            // 
            header.BackColor = Color.Transparent;
            header.ForeColor = SystemColors.ControlText;
            header.Location = new Point(0, 0);
            header.Name = "header";
            header.Size = new Size(1300, 45);
            header.TabIndex = 23;
            header.MouseDown += Header_MouseDown;
            header.MouseMove += Header_MouseMove;
            // 
            // mainpage
            // 
            mainpage.BackColor = Color.White;
            mainpage.Location = new Point(180, 58);
            mainpage.Name = "mainpage";
            mainpage.Size = new Size(1050, 655);
            mainpage.TabIndex = 25;
            // 
            // logListBox
            // 
            logListBox.FormattingEnabled = true;
            logListBox.HorizontalScrollbar = true;
            logListBox.ItemHeight = 24;
            logListBox.Location = new Point(10, 13);
            logListBox.Name = "logListBox";
            logListBox.ScrollAlwaysVisible = true;
            logListBox.SelectionMode = SelectionMode.None;
            logListBox.Size = new Size(1030, 628);
            logListBox.TabIndex = 0;
            // 
            // exitButton
            // 
            exitButton.BackColor = Color.Transparent;
            exitButton.Cursor = Cursors.Hand;
            exitButton.Image = Properties.Resources.研究人员__ffffff_128_21601186;
            exitButton.Location = new Point(65, 610);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(50, 50);
            exitButton.SizeMode = PictureBoxSizeMode.Zoom;
            exitButton.TabIndex = 26;
            exitButton.TabStop = false;
            exitButton.Click += ExitButton_Click;
            // 
            // courseButton
            // 
            courseButton.BackColor = Color.Transparent;
            courseButton.Cursor = Cursors.Hand;
            courseButton.Image = Properties.Resources.课表__ffffff_128_21601021;
            courseButton.Location = new Point(65, 130);
            courseButton.Name = "courseButton";
            courseButton.Size = new Size(50, 50);
            courseButton.SizeMode = PictureBoxSizeMode.Zoom;
            courseButton.TabIndex = 27;
            courseButton.TabStop = false;
            courseButton.Click += CourseButton_Click;
            // 
            // activityButton
            // 
            activityButton.BackColor = Color.Transparent;
            activityButton.Cursor = Cursors.Hand;
            activityButton.Image = Properties.Resources.户外实践__ffffff_128_21601017;
            activityButton.Location = new Point(65, 322);
            activityButton.Name = "activityButton";
            activityButton.Size = new Size(50, 50);
            activityButton.SizeMode = PictureBoxSizeMode.Zoom;
            activityButton.TabIndex = 28;
            activityButton.TabStop = false;
            activityButton.Click += ActivityButton_Click;
            // 
            // mainpageButton
            // 
            mainpageButton.BackColor = Color.Transparent;
            mainpageButton.Image = Properties.Resources.SubPageBG;
            mainpageButton.Location = new Point(175, 53);
            mainpageButton.Name = "mainpageButton";
            mainpageButton.Size = new Size(1060, 665);
            mainpageButton.TabIndex = 31;
            mainpageButton.TabStop = false;
            // 
            // logoutConfirm
            // 
            logoutConfirm.Cursor = Cursors.Hand;
            logoutConfirm.Image = Properties.Resources.退出__a880c2_128_3661364;
            logoutConfirm.Location = new Point(500, 300);
            logoutConfirm.Name = "logoutConfirm";
            logoutConfirm.Size = new Size(150, 150);
            logoutConfirm.SizeMode = PictureBoxSizeMode.Zoom;
            logoutConfirm.TabIndex = 33;
            logoutConfirm.TabStop = false;
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
            closeConfirm.TabIndex = 34;
            closeConfirm.TabStop = false;
            closeConfirm.Click += CloseConfirm_Click;
            // 
            // examButton
            // 
            examButton.BackColor = Color.Transparent;
            examButton.Cursor = Cursors.Hand;
            examButton.Image = Properties.Resources.试卷__ffffff_128_21601018;
            examButton.Location = new Point(65, 226);
            examButton.Name = "examButton";
            examButton.Size = new Size(50, 50);
            examButton.SizeMode = PictureBoxSizeMode.Zoom;
            examButton.TabIndex = 29;
            examButton.TabStop = false;
            examButton.Click += ExamButton_Click;
            // 
            // mapEditButton
            // 
            mapEditButton.BackColor = Color.Transparent;
            mapEditButton.Cursor = Cursors.Hand;
            mapEditButton.Image = Properties.Resources.地图__ffffff_128_21601004;
            mapEditButton.Location = new Point(65, 418);
            mapEditButton.Name = "mapEditButton";
            mapEditButton.Size = new Size(50, 50);
            mapEditButton.SizeMode = PictureBoxSizeMode.Zoom;
            mapEditButton.TabIndex = 35;
            mapEditButton.TabStop = false;
            mapEditButton.Click += MapEditButton_Click;
            // 
            // logButton
            // 
            logButton.BackColor = Color.Transparent;
            logButton.Cursor = Cursors.Hand;
            logButton.Image = Properties.Resources.党员日志__ffffff_128_21601025;
            logButton.Location = new Point(65, 514);
            logButton.Name = "logButton";
            logButton.Size = new Size(50, 50);
            logButton.SizeMode = PictureBoxSizeMode.Zoom;
            logButton.TabIndex = 36;
            logButton.TabStop = false;
            logButton.Click += LogButton_Click;
            // 
            // AdminWindow
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1300, 770);
            Controls.Add(logButton);
            Controls.Add(mapEditButton);
            Controls.Add(examButton);
            Controls.Add(activityButton);
            Controls.Add(courseButton);
            Controls.Add(exitButton);
            Controls.Add(header);
            Controls.Add(logoutConfirm);
            Controls.Add(closeConfirm);
            Controls.Add(mainpage);
            Controls.Add(mainpageButton);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AdminWindow";
            Text = "AdminWindow";
            mainpage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)exitButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)courseButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)activityButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)mainpageButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)logoutConfirm).EndInit();
            ((System.ComponentModel.ISupportInitialize)closeConfirm).EndInit();
            ((System.ComponentModel.ISupportInitialize)examButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)mapEditButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)logButton).EndInit();
            ResumeLayout(false);
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