﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    SubWindows: 1050, 655
    180, 58    
    175, 53
*/


namespace StudentScheduleManagementSystem.UI
{
    public partial class AdminWindow : Form
    {
        private static CourseSubwindow _courseSubwindow { get; set; }
        private static ExamSubwindow _examSubwindow { get; set; }
        private static GroupActivitySubwindow _groupActivitySubwindow { get; set; }

        public AdminWindow()
        {
            InitializeComponent();
            _courseSubwindow = new CourseSubwindow();
            _examSubwindow = new ExamSubwindow();
            _groupActivitySubwindow = new GroupActivitySubwindow();
            this.logoutConfirm.Hide();
            this.closeConfirm.Hide();
        }

        //窗口拖动
        int oldX, oldY;
        private void Header_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.Location.X - this.oldX;
                this.Top += e.Location.Y - this.oldY;
            }
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            MainProgram.Program.Logout();
            _courseSubwindow.Dispose();
            _courseSubwindow.Close();
            _examSubwindow.Dispose();
            _examSubwindow.Close();
            _groupActivitySubwindow.Dispose();
            _groupActivitySubwindow.Close();
            this.Dispose();
            this.Close();
        }

        private void ClosePage_Click(object sender, EventArgs e)
        {
            MainProgram.Program.Logout();
            _courseSubwindow.Close();
            _courseSubwindow.Dispose();
            _examSubwindow.Close();
            _examSubwindow.Dispose();
            _groupActivitySubwindow.Close();
            _groupActivitySubwindow.Dispose();
            this.Close();
            this.Dispose();
        }

        private void CourseManagement_Click(object sender, EventArgs e)
        {
            this.logoutConfirm.Hide();
            this.closeConfirm.Hide();
            mainpage.Controls.Clear();
            _courseSubwindow.TopLevel = false;
            mainpage.Controls.Add(_courseSubwindow);
            _courseSubwindow.Show();
        }

        private void TestManagement_Click(object sender, EventArgs e)
        {
            this.logoutConfirm.Hide();
            this.closeConfirm.Hide();
            mainpage.Controls.Clear();
            _examSubwindow.TopLevel = false;
            mainpage.Controls.Add(_examSubwindow);
            _examSubwindow.Show();
        }

        private void ActivityManagement_Click(object sender, EventArgs e)
        {
            this.logoutConfirm.Hide();
            this.closeConfirm.Hide();
            mainpage.Controls.Clear();
            _groupActivitySubwindow.TopLevel = false;
            mainpage.Controls.Add(_groupActivitySubwindow);
            _groupActivitySubwindow.Show();
        }

        private void LogoutClose_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            this.logoutConfirm.Show();
            this.closeConfirm.Show();
        }


        private void Header_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.oldX = e.Location.X;
                this.oldY = e.Location.Y;
            }
        }
    }
}