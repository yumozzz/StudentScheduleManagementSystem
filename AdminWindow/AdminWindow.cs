﻿namespace StudentScheduleManagementSystem.UI
{
    public partial class AdminWindow : Form
    {
        private static CourseSubwindow? _courseSubwindow;
        private static ExamSubwindow? _examSubwindow;
        private static GroupActivitySubwindow? _groupActivitySubwindow;
        private static MapEditWindow? _mapEditWindow;

        public AdminWindow()
        {
            InitializeComponent();
            this.logoutConfirm.Hide();
            this.closeConfirm.Hide();
        }

        //窗口拖动
        private int _x, _y;

        private void Header_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.Location.X - this._x;
                this.Top += e.Location.Y - this._y;
            }
        }

        private void Header_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this._x = e.Location.X;
                this._y = e.Location.Y;
            }
        }

        private void LogoutConfirm_Click(object sender, EventArgs e)
        {
            MainProgram.Program.Logout();
            _courseSubwindow?.Close();
            _examSubwindow?.Close();
            _groupActivitySubwindow?.Close();
            _courseSubwindow = null;
            _examSubwindow = null;
            _groupActivitySubwindow = null;
            this.Close();
        }

        //TODO:
        private void CloseConfirm_Click(object sender, EventArgs e)
        {
            LogoutConfirm_Click(sender, e);
        }

        private void CourseButton_Click(object sender, EventArgs e)
        {
            this.logoutConfirm.Hide();
            this.closeConfirm.Hide();
            mainpage.Controls.Clear();
            _courseSubwindow = new();
            _courseSubwindow.TopLevel = false;
            mainpage.Controls.Add(_courseSubwindow);
            _courseSubwindow.Show();
        }

        private void ExamButton_Click(object sender, EventArgs e)
        {
            this.logoutConfirm.Hide();
            this.closeConfirm.Hide();
            mainpage.Controls.Clear();
            _examSubwindow = new();
            _examSubwindow.TopLevel = false;
            mainpage.Controls.Add(_examSubwindow);
            _examSubwindow.Show();
        }

        private void ActivityButton_Click(object sender, EventArgs e)
        {
            this.logoutConfirm.Hide();
            this.closeConfirm.Hide();
            mainpage.Controls.Clear();
            _groupActivitySubwindow = new();
            _groupActivitySubwindow.TopLevel = false;
            mainpage.Controls.Add(_groupActivitySubwindow);
            _groupActivitySubwindow.Show();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            this.logoutConfirm.Show();
            this.closeConfirm.Show();
        }

        private void MapEditButton_Click(object sender, EventArgs e)
        {
            //TODO
            _mapEditWindow = new(new List<(Map.Location.Vertex, Map.Location.Vertex)>(),
                                 new List<(Map.Location.Vertex, Point, Point, Map.Location.Vertex)>());
            _mapEditWindow.ShowDialog();
            _mapEditWindow.Close();
            _mapEditWindow.Dispose();
            GC.Collect();
        }
    }
}
