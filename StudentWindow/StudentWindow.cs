namespace StudentScheduleManagementSystem.UI
{

    public partial class StudentWindow : Form
    {
        private static StudentScheduleTable? _studentScheduleTable;
        private static StudentCourseSubwindow? _studentCourseSubwindow;
        private static StudentExamSubwindow? _studentExamSubwindow;
        private static StudentGroupActivitySubwindow? _studentGroupActivitySubwindow;
        private static StudentPersonalActivitySubwindow? _studentPersonalActivitySubwindow;
        private static StudentTemporaryAffairSubwindow? _studentTemporaryAffairSubwindow;

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

        private void ExamButton_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            _studentExamSubwindow = new ();
            _studentExamSubwindow.TopLevel = false;
            mainpage.Controls.Add(_studentExamSubwindow);
            _studentExamSubwindow.Show();
        }

        private void GroupActivityButton_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            _studentGroupActivitySubwindow = new ();
            _studentGroupActivitySubwindow.TopLevel = false;
            mainpage.Controls.Add(_studentGroupActivitySubwindow);
            _studentGroupActivitySubwindow.Show();
        }

        private void PersonalActivityButton_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            _studentPersonalActivitySubwindow = new();
            _studentPersonalActivitySubwindow.TopLevel = false;
            mainpage.Controls.Add(_studentPersonalActivitySubwindow);
            _studentPersonalActivitySubwindow.Show();
        }

        private void TemporaryAffairButton_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            _studentTemporaryAffairSubwindow = new();
            _studentTemporaryAffairSubwindow.TopLevel = false;
            mainpage.Controls.Add(_studentTemporaryAffairSubwindow);
            _studentTemporaryAffairSubwindow.Show();
        }

        public void SetLocalTime(Times.Time time)
        {
            if (currentTime.InvokeRequired)
            {
                this.currentTime.Invoke(SetLocalTime, time);
            }
            else
            {
                this.currentTime.Text = time.ToString();
            }
        }
    }
}
