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
            this.move = new System.Windows.Forms.Panel();
            this.Logout = new System.Windows.Forms.Button();
            this.CurrentTime = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.Button();
            this.multiSelectBox1 = new StudentScheduleManagementSystem.MultiSelectBox();
            this.multiSelectBox2 = new StudentScheduleManagementSystem.MultiSelectBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.multiSelectBox3 = new StudentScheduleManagementSystem.MultiSelectBox();
            this.move.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // move
            // 
            this.move.Controls.Add(this.Logout);
            this.move.Location = new System.Drawing.Point(0, 0);
            this.move.Name = "move";
            this.move.Size = new System.Drawing.Size(1200, 40);
            this.move.TabIndex = 1;
            this.move.MouseDown += new System.Windows.Forms.MouseEventHandler(this.move_MouseDown);
            this.move.MouseMove += new System.Windows.Forms.MouseEventHandler(this.move_MouseMove);
            // 
            // Logout
            // 
            this.Logout.Location = new System.Drawing.Point(1076, 3);
            this.Logout.Name = "Logout";
            this.Logout.Size = new System.Drawing.Size(112, 34);
            this.Logout.TabIndex = 7;
            this.Logout.Text = "Logout";
            this.Logout.UseVisualStyleBackColor = true;
            this.Logout.Click += new System.EventHandler(this.Logout_Click);
            // 
            // CurrentTime
            // 
            this.CurrentTime.AutoSize = true;
            this.CurrentTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CurrentTime.Location = new System.Drawing.Point(12, 53);
            this.CurrentTime.Name = "CurrentTime";
            this.CurrentTime.Size = new System.Drawing.Size(70, 26);
            this.CurrentTime.TabIndex = 2;
            this.CurrentTime.Text = "Week i";
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(203, 114);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(112, 34);
            this.search.TabIndex = 6;
            this.search.Text = "Search";
            this.search.UseVisualStyleBackColor = true;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // multiSelectBox1
            // 
            this.multiSelectBox1.Location = new System.Drawing.Point(140, 3);
            this.multiSelectBox1.Name = "multiSelectBox1";
            this.multiSelectBox1.Size = new System.Drawing.Size(184, 30);
            this.multiSelectBox1.TabIndex = 9;
            // 
            // multiSelectBox2
            // 
            this.multiSelectBox2.Location = new System.Drawing.Point(140, 39);
            this.multiSelectBox2.Name = "multiSelectBox2";
            this.multiSelectBox2.Size = new System.Drawing.Size(184, 30);
            this.multiSelectBox2.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.search);
            this.panel1.Controls.Add(this.multiSelectBox3);
            this.panel1.Controls.Add(this.multiSelectBox1);
            this.panel1.Controls.Add(this.multiSelectBox2);
            this.panel1.Location = new System.Drawing.Point(873, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(327, 224);
            this.panel1.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 24);
            this.label3.TabIndex = 15;
            this.label3.Text = "Hours";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 24);
            this.label2.TabIndex = 14;
            this.label2.Text = "Days";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 24);
            this.label1.TabIndex = 13;
            this.label1.Text = "Weeks";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(203, 154);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 34);
            this.button1.TabIndex = 12;
            this.button1.Text = "Insert";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // multiSelectBox3
            // 
            this.multiSelectBox3.Location = new System.Drawing.Point(140, 76);
            this.multiSelectBox3.Name = "multiSelectBox3";
            this.multiSelectBox3.Size = new System.Drawing.Size(184, 30);
            this.multiSelectBox3.TabIndex = 11;
            // 
            // StudentWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.CurrentTime);
            this.Controls.Add(this.move);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StudentWindow";
            this.Text = "StudentWindow";
            this.move.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Panel move;
        private Label CurrentTime;
        private Button search;
        private Button Logout;
        private MultiSelectBox multiSelectBox1;
        private MultiSelectBox multiSelectBox2;
        private Panel panel1;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button button1;
        private MultiSelectBox multiSelectBox3;
    }
}