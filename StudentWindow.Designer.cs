namespace StudentScheduleManagementSystem
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
            this.logout = new System.Windows.Forms.Button();
            this.move = new System.Windows.Forms.Panel();
            this.week = new System.Windows.Forms.Label();
            this.day = new System.Windows.Forms.Label();
            this.currenttime = new System.Windows.Forms.Label();
            this.searchclass = new System.Windows.Forms.TextBox();
            this.search = new System.Windows.Forms.Button();
            this.move.SuspendLayout();
            this.SuspendLayout();
            // 
            // logout
            // 
            this.logout.Location = new System.Drawing.Point(1076, 6);
            this.logout.Name = "logout";
            this.logout.Size = new System.Drawing.Size(112, 34);
            this.logout.TabIndex = 0;
            this.logout.Text = "Logout";
            this.logout.UseVisualStyleBackColor = true;
            this.logout.Click += new System.EventHandler(this.logout_Click);
            // 
            // move
            // 
            this.move.Controls.Add(this.logout);
            this.move.Location = new System.Drawing.Point(0, 0);
            this.move.Name = "move";
            this.move.Size = new System.Drawing.Size(1200, 40);
            this.move.TabIndex = 1;
            this.move.MouseDown += new System.Windows.Forms.MouseEventHandler(this.move_MouseDown);
            this.move.MouseMove += new System.Windows.Forms.MouseEventHandler(this.move_MouseMove);
            // 
            // week
            // 
            this.week.AutoSize = true;
            this.week.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.week.Location = new System.Drawing.Point(12, 82);
            this.week.Name = "week";
            this.week.Size = new System.Drawing.Size(70, 26);
            this.week.TabIndex = 2;
            this.week.Text = "Week i";
            // 
            // day
            // 
            this.day.AutoSize = true;
            this.day.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.day.Location = new System.Drawing.Point(88, 82);
            this.day.Name = "day";
            this.day.Size = new System.Drawing.Size(54, 26);
            this.day.TabIndex = 3;
            this.day.Text = "day j";
            // 
            // currenttime
            // 
            this.currenttime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.currenttime.Location = new System.Drawing.Point(12, 122);
            this.currenttime.Name = "currenttime";
            this.currenttime.Size = new System.Drawing.Size(94, 36);
            this.currenttime.TabIndex = 4;
            this.currenttime.Text = "hh:mm:ss";
            // 
            // searchclass
            // 
            this.searchclass.Location = new System.Drawing.Point(12, 222);
            this.searchclass.Name = "searchclass";
            this.searchclass.Size = new System.Drawing.Size(150, 30);
            this.searchclass.TabIndex = 5;
            this.searchclass.Text = "searchclass";
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(30, 258);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(112, 34);
            this.search.TabIndex = 6;
            this.search.Text = "Search";
            this.search.UseVisualStyleBackColor = true;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // StudentWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.search);
            this.Controls.Add(this.searchclass);
            this.Controls.Add(this.currenttime);
            this.Controls.Add(this.day);
            this.Controls.Add(this.week);
            this.Controls.Add(this.move);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StudentWindow";
            this.Text = "StudentWindow";
            this.move.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button logout;
        private Panel move;
        private Label week;
        private Label day;
        private Label currenttime;
        private TextBox searchclass;
        private Button search;
    }
}