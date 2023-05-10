using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentScheduleManagementSystem.UI
{

    public partial class StudentWindow : Form
    {
        public delegate void ChangeLocalTimeCallback(Times.Time time);

        private ChangeLocalTimeCallback _changeLocalTimeCallback;

        private static StudentScheduleTable? _studentScheduleTable;

        private static StudentCourseSubwindow? _studentCourseSubwindow;
        //private static ExamSubwindow? _examSubwindow;
        //private static GroupActivitySubwindow? _groupActivitySubwindow;
        //private static MapEditWindow? _mapEditWindow;
        public StudentWindow()
        {
            InitializeComponent();
        }

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

        private void Logout_Click(object sender, EventArgs e)
        {
            MainProgram.Program.Logout();
            this.Close();
        }

        private void ScheduleTableButton_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            _studentScheduleTable = new();
            _studentScheduleTable.TopLevel = false;
            mainpage.Controls.Add(_studentScheduleTable);
            _studentScheduleTable.Show();
        }

        private void CourseButton_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            _studentCourseSubwindow = new();
            _studentCourseSubwindow.TopLevel = false;
            mainpage.Controls.Add(_studentCourseSubwindow);
            _studentCourseSubwindow.Show();
        }

        public void SetLocalTime(Times.Time time)
        {
            if (currentTime.InvokeRequired)
            {
                _changeLocalTimeCallback = new(SetLocalTime);
                this.currentTime.Invoke(SetLocalTime, time);
            }
            else
            {
                this.currentTime.Text = time.ToString();
            }
        }
    }
}
