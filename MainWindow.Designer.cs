namespace StudentScheduleManagementSystem.UI
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.login = new System.Windows.Forms.Button();
            this.username = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.Label();
            this.usernamebox = new System.Windows.Forms.TextBox();
            this.passwordbox = new System.Windows.Forms.TextBox();
            this.is_student = new System.Windows.Forms.RadioButton();
            this.is_administrator = new System.Windows.Forms.RadioButton();
            this.clear = new System.Windows.Forms.Button();
            this.welcome = new System.Windows.Forms.Label();
            this.close = new System.Windows.Forms.Button();
            this.move = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // login
            // 
            this.login.Location = new System.Drawing.Point(261, 395);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(277, 34);
            this.login.TabIndex = 0;
            this.login.Text = "Login";
            this.login.UseVisualStyleBackColor = true;
            // 
            // username
            // 
            this.username.AutoSize = true;
            this.username.Location = new System.Drawing.Point(261, 178);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(94, 24);
            this.username.TabIndex = 1;
            this.username.Text = "username";
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.Location = new System.Drawing.Point(261, 251);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(92, 24);
            this.password.TabIndex = 2;
            this.password.Text = "password";
            // 
            // usernamebox
            // 
            this.usernamebox.Location = new System.Drawing.Point(387, 178);
            this.usernamebox.Name = "usernamebox";
            this.usernamebox.Size = new System.Drawing.Size(150, 30);
            this.usernamebox.TabIndex = 3;
            // 
            // passwordbox
            // 
            this.passwordbox.Location = new System.Drawing.Point(387, 251);
            this.passwordbox.Name = "passwordbox";
            this.passwordbox.PasswordChar = '*';
            this.passwordbox.Size = new System.Drawing.Size(150, 30);
            this.passwordbox.TabIndex = 4;
            // 
            // is_student
            // 
            this.is_student.AutoSize = true;
            this.is_student.Checked = true;
            this.is_student.Location = new System.Drawing.Point(261, 329);
            this.is_student.Name = "is_student";
            this.is_student.Size = new System.Drawing.Size(103, 28);
            this.is_student.TabIndex = 6;
            this.is_student.TabStop = true;
            this.is_student.Text = "Student";
            this.is_student.UseVisualStyleBackColor = true;
            // 
            // is_administrator
            // 
            this.is_administrator.AutoSize = true;
            this.is_administrator.Location = new System.Drawing.Point(383, 329);
            this.is_administrator.Name = "is_administrator";
            this.is_administrator.Size = new System.Drawing.Size(155, 28);
            this.is_administrator.TabIndex = 7;
            this.is_administrator.TabStop = true;
            this.is_administrator.Text = "Administrator";
            this.is_administrator.UseVisualStyleBackColor = true;
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(261, 454);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(126, 34);
            this.clear.TabIndex = 8;
            this.clear.Text = "Clear";
            this.clear.UseVisualStyleBackColor = true;
            // 
            // welcome
            // 
            this.welcome.AutoSize = true;
            this.welcome.Location = new System.Drawing.Point(355, 121);
            this.welcome.Name = "welcome";
            this.welcome.Size = new System.Drawing.Size(96, 24);
            this.welcome.TabIndex = 9;
            this.welcome.Text = "Welcome!";
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(411, 454);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(126, 34);
            this.close.TabIndex = 11;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // move
            // 
            this.move.Location = new System.Drawing.Point(0, 0);
            this.move.Name = "move";
            this.move.Size = new System.Drawing.Size(800, 40);
            this.move.TabIndex = 12;
            this.move.Paint += new System.Windows.Forms.PaintEventHandler(this.move_Paint);
            this.move.MouseDown += new System.Windows.Forms.MouseEventHandler(this.move_MouseDown);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.move);
            this.Controls.Add(this.close);
            this.Controls.Add(this.welcome);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.is_administrator);
            this.Controls.Add(this.is_student);
            this.Controls.Add(this.passwordbox);
            this.Controls.Add(this.usernamebox);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.login);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainWindow";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button login;
        private Label username;
        private Label password;
        private TextBox usernamebox;
        private TextBox passwordbox;
        private RadioButton is_student;
        private RadioButton is_administrator;
        private Button clear;
        private Label welcome;
        private Button close;
        private Panel move;
        private System.Windows.Forms.Timer timer1;
    }
}