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
            this.login = new System.Windows.Forms.Button();
            this.usernamebox = new System.Windows.Forms.TextBox();
            this.passwordbox = new System.Windows.Forms.TextBox();
            this.clear = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.move = new System.Windows.Forms.Panel();
            this.register = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // login
            // 
            this.login.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.login.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.login.FlatAppearance.BorderSize = 0;
            this.login.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.login.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.login.Location = new System.Drawing.Point(210, 347);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(378, 40);
            this.login.TabIndex = 0;
            this.login.Text = "Login";
            this.login.UseVisualStyleBackColor = true;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // usernamebox
            // 
            this.usernamebox.BackColor = System.Drawing.Color.White;
            this.usernamebox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.usernamebox.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.usernamebox.Location = new System.Drawing.Point(373, 225);
            this.usernamebox.Name = "usernamebox";
            this.usernamebox.Size = new System.Drawing.Size(215, 33);
            this.usernamebox.TabIndex = 3;
            this.usernamebox.Text = "2021219999";
            this.usernamebox.TextChanged += new System.EventHandler(this.usernamebox_TextChanged);
            // 
            // passwordbox
            // 
            this.passwordbox.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.passwordbox.Location = new System.Drawing.Point(373, 287);
            this.passwordbox.Name = "passwordbox";
            this.passwordbox.PasswordChar = '*';
            this.passwordbox.Size = new System.Drawing.Size(215, 35);
            this.passwordbox.TabIndex = 4;
            this.passwordbox.Text = "administrator";
            // 
            // clear
            // 
            this.clear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.clear.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.clear.Location = new System.Drawing.Point(413, 467);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(175, 40);
            this.clear.TabIndex = 8;
            this.clear.Text = "Clear";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // close
            // 
            this.close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.close.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.close.Location = new System.Drawing.Point(210, 467);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(175, 40);
            this.close.TabIndex = 11;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // move
            // 
            this.move.BackgroundImage = global::StudentScheduleManagementSystem.Properties.Resources.LoginBackground;
            this.move.Location = new System.Drawing.Point(0, 0);
            this.move.Name = "move";
            this.move.Size = new System.Drawing.Size(800, 40);
            this.move.TabIndex = 12;
            this.move.Paint += new System.Windows.Forms.PaintEventHandler(this.move_Paint);
            this.move.MouseDown += new System.Windows.Forms.MouseEventHandler(this.move_MouseDown);
            this.move.MouseMove += new System.Windows.Forms.MouseEventHandler(this.move_MouseMove);
            // 
            // register
            // 
            this.register.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.register.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.register.Location = new System.Drawing.Point(210, 407);
            this.register.Name = "register";
            this.register.Size = new System.Drawing.Size(378, 40);
            this.register.TabIndex = 13;
            this.register.Text = "Register";
            this.register.UseVisualStyleBackColor = true;
            this.register.Click += new System.EventHandler(this.register_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::StudentScheduleManagementSystem.Properties.Resources.LoginBackground1;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.register);
            this.Controls.Add(this.move);
            this.Controls.Add(this.close);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.passwordbox);
            this.Controls.Add(this.usernamebox);
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
        private TextBox usernamebox;
        private TextBox passwordbox;
        private Button clear;
        private Button close;
        private Panel move;
        private Button register;
    }
}