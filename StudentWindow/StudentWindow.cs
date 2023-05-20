using System.Text;

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
            Times.Timer.TimeChange += SetLocalTime;
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
            _studentCourseSubwindow.PauseTimeDelegate = PauseTime;
            mainpage.Controls.Add(_studentCourseSubwindow);
            _studentCourseSubwindow.Show();
        }

        private void ExamButton_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            _studentExamSubwindow = new ();
            _studentExamSubwindow.TopLevel = false;
            _studentExamSubwindow.PauseTimeDelegate = PauseTime;
            mainpage.Controls.Add(_studentExamSubwindow);
            _studentExamSubwindow.Show();
        }

        private void GroupActivityButton_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            _studentGroupActivitySubwindow = new ();
            _studentGroupActivitySubwindow.TopLevel = false;
            _studentGroupActivitySubwindow.PauseTimeDelegate = PauseTime;
            mainpage.Controls.Add(_studentGroupActivitySubwindow);
            _studentGroupActivitySubwindow.Show();
        }

        private void PersonalActivityButton_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            _studentPersonalActivitySubwindow = new();
            _studentPersonalActivitySubwindow.TopLevel = false;
            _studentPersonalActivitySubwindow.PauseTimeDelegate = PauseTime;
            mainpage.Controls.Add(_studentPersonalActivitySubwindow);
            _studentPersonalActivitySubwindow.Show();
        }

        private void TemporaryAffairButton_Click(object sender, EventArgs e)
        {
            mainpage.Controls.Clear();
            _studentTemporaryAffairSubwindow = new();
            _studentTemporaryAffairSubwindow.TopLevel = false;
            _studentTemporaryAffairSubwindow.PauseTimeDelegate = PauseTime;
            mainpage.Controls.Add(_studentTemporaryAffairSubwindow);
            _studentTemporaryAffairSubwindow.Show();
        }

        private void SetTime_Click(object sender, EventArgs e)
        {
            StringBuilder errorMessage = new();
            if (weekBox.Text.Equals(""))
            {
                errorMessage.AppendLine("请输入周！\n");
            }
            if (dayBox.Text.Equals(""))
            {
                errorMessage.AppendLine("请输入日！\n");
            }
            if (hourBox.Text.Equals(""))
            {
                errorMessage.AppendLine("请输入时间！\n");
            }
            if (!errorMessage.Equals(""))
            {
                MessageBox.Show(errorMessage.ToString());
                return;
            }

            int week = weekBox.Text[4] - '0';
            Day day = Day.Monday;
            int hour = hourBox.Text[0] - '0';
            if (weekBox.Text.Length == 6)
            {
                week = week * 10 + weekBox.Text[5] - '0';
            }
            for(int i = 0; i < 6; i++)
            {
                if (dayBox.Text.Equals(Shared.Days[i]))
                {
                    day = (Day)i;
                }
            }
            if (hourBox.Text.Length == 5)
            {
                hour = hour * 10 + hourBox.Text[1] - '0';
            }

            if (MessageBox.Show("周次: " + week.ToString() + 
                                "\n日次: " + day.ToString() + 
                                "\n时间: " + hour.ToString() + ":00", 
                                "确认时间修改",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Times.Time time = new() { Week = week, Day = day, Hour = hour };
                Times.Timer.SetTime(time);
                Log.Information.Log("时间设置为: " + time.ToString());
            }
        }

        private void SpeedButton_Click(object sender, EventArgs e)
        {
            Times.Timer.SetSpeed();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            PauseTime(null);
        }

        public void PauseTime(bool? pauseTime)
        {
            Times.Timer.Pause = pauseTime.HasValue ? pauseTime.Value : !Times.Timer.Pause;
            if (Times.Timer.Pause)
            {
                pauseButton.Text = "继续";
                Log.Information.Log("时间暂停: " + currentTime.Text);
            }
            else
            {
                pauseButton.Text = "暂停";
                Log.Information.Log("时间恢复: " + currentTime.Text);
            }
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
