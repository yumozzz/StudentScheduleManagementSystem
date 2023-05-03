﻿namespace StudentScheduleManagementSystem.UI
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
            this.logoutButton = new System.Windows.Forms.Button();
            this.currentTime = new System.Windows.Forms.Label();
            this.searchButton = new System.Windows.Forms.Button();
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
            this.move.Controls.Add(this.logoutButton);
            this.move.Location = new System.Drawing.Point(0, 0);
            this.move.Name = "move";
            this.move.Size = new System.Drawing.Size(1200, 40);
            this.move.TabIndex = 1;
            this.move.MouseDown += new System.Windows.Forms.MouseEventHandler(this.move_MouseDown);
            this.move.MouseMove += new System.Windows.Forms.MouseEventHandler(this.move_MouseMove);
            // 
            // Logout
            // 
            this.logoutButton.Location = new System.Drawing.Point(1076, 3);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(112, 34);
            this.logoutButton.TabIndex = 7;
            this.logoutButton.Text = "Logout";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.Logout_Click);
            // 
            // CurrentTime
            // 
            this.currentTime.AutoSize = true;
            this.currentTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.currentTime.Location = new System.Drawing.Point(12, 53);
            this.currentTime.Name = "currentTime";
            this.currentTime.Size = new System.Drawing.Size(70, 26);
            this.currentTime.TabIndex = 2;
            this.currentTime.Text = "Week i";
            // 
            // search
            // 
            this.searchButton.Location = new System.Drawing.Point(203, 114);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(112, 34);
            this.searchButton.TabIndex = 6;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.search_Click);
            // 
            // multiSelectBox1
            // 
            this.multiSelectBox1.BackColor = System.Drawing.Color.White;
            this.multiSelectBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.multiSelectBox1.Location = new System.Drawing.Point(140, 3);
            this.multiSelectBox1.Name = "multiSelectBox1";
            this.multiSelectBox1.Size = new System.Drawing.Size(184, 30);
            this.multiSelectBox1.TabIndex = 9;
            // 
            // multiSelectBox2
            // 
            this.multiSelectBox2.BackColor = System.Drawing.Color.White;
            this.multiSelectBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.panel1.Controls.Add(this.searchButton);
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
            this.multiSelectBox3.BackColor = System.Drawing.Color.White;
            this.multiSelectBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.Controls.Add(this.currentTime);
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
        private Label currentTime;
        private Button searchButton;
        private Button logoutButton;
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