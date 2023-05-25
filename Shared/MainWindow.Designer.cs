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
            login = new Button();
            usernameBox = new TextBox();
            passwordBox = new TextBox();
            clear = new Button();
            close = new Button();
            header = new Panel();
            register = new Button();
            SuspendLayout();
            // 
            // login
            // 
            login.BackgroundImageLayout = ImageLayout.None;
            login.FlatAppearance.BorderColor = Color.White;
            login.FlatAppearance.BorderSize = 0;
            login.FlatStyle = FlatStyle.Popup;
            login.Font = new Font("Cambria", 12F, FontStyle.Regular, GraphicsUnit.Point);
            login.Location = new Point(210, 347);
            login.Name = "login";
            login.Size = new Size(378, 40);
            login.TabIndex = 0;
            login.Text = "Login";
            login.UseVisualStyleBackColor = true;
            login.Click += LoginButton_Click;
            // 
            // usernameBox
            // 
            usernameBox.BackColor = Color.White;
            usernameBox.BorderStyle = BorderStyle.None;
            usernameBox.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            usernameBox.Location = new Point(373, 225);
            usernameBox.Name = "usernameBox";
            usernameBox.Size = new Size(215, 33);
            usernameBox.TabIndex = 3;
            usernameBox.Text = "2021210001";
            // 
            // passwordBox
            // 
            passwordBox.Font = new Font("Arial Narrow", 12F, FontStyle.Regular, GraphicsUnit.Point);
            passwordBox.Location = new Point(373, 287);
            passwordBox.Name = "passwordBox";
            passwordBox.PasswordChar = '*';
            passwordBox.Size = new Size(215, 35);
            passwordBox.TabIndex = 4;
            passwordBox.Text = "123456";
            // 
            // clear
            // 
            clear.FlatStyle = FlatStyle.Popup;
            clear.Font = new Font("Cambria", 12F, FontStyle.Regular, GraphicsUnit.Point);
            clear.Location = new Point(413, 467);
            clear.Name = "clear";
            clear.Size = new Size(175, 40);
            clear.TabIndex = 8;
            clear.Text = "Clear";
            clear.UseVisualStyleBackColor = true;
            clear.Click += ClearButton_Click;
            // 
            // close
            // 
            close.FlatStyle = FlatStyle.Popup;
            close.Font = new Font("Cambria", 12F, FontStyle.Regular, GraphicsUnit.Point);
            close.Location = new Point(210, 467);
            close.Name = "close";
            close.Size = new Size(175, 40);
            close.TabIndex = 11;
            close.Text = "Close";
            close.UseVisualStyleBackColor = true;
            // 
            // header
            // 
            header.BackgroundImage = Properties.Resources.LoginBackground;
            header.Location = new Point(0, 0);
            header.Name = "header";
            header.Size = new Size(800, 40);
            header.TabIndex = 12;
            header.MouseDown += Header_MouseDown;
            header.MouseMove += Header_MouseMove;
            // 
            // register
            // 
            register.FlatStyle = FlatStyle.Popup;
            register.Font = new Font("Cambria", 12F, FontStyle.Regular, GraphicsUnit.Point);
            register.Location = new Point(210, 407);
            register.Name = "register";
            register.Size = new Size(378, 40);
            register.TabIndex = 13;
            register.Text = "Register";
            register.UseVisualStyleBackColor = true;
            register.Click += RegisterButton_Click;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackgroundImage = Properties.Resources.LoginBackground1;
            ClientSize = new Size(800, 600);
            Controls.Add(register);
            Controls.Add(header);
            Controls.Add(close);
            Controls.Add(clear);
            Controls.Add(passwordBox);
            Controls.Add(usernameBox);
            Controls.Add(login);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MainWindow";
            RightToLeft = RightToLeft.No;
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button login;
        private TextBox usernameBox;
        private TextBox passwordBox;
        private Button clear;
        private Button close;
        private Panel header;
        private Button register;
    }
}