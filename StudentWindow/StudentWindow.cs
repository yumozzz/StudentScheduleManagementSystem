using System.Text;

namespace StudentScheduleManagementSystem.UI
{
    public partial class StudentWindow : Form
    {
        private StudentScheduleTable? _studentScheduleTable;
        private StudentCourseSubwindow? _studentCourseSubwindow;
        private StudentExamSubwindow? _studentExamSubwindow;
        private StudentGroupActivitySubwindow? _studentGroupActivitySubwindow;
        private StudentPersonalActivitySubwindow? _studentPersonalActivitySubwindow;
        private StudentTemporaryAffairSubwindow? _studentTemporaryAffairSubwindow;
        public bool ShouldExitProgram { get; private set; } = false;

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
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            _studentScheduleTable = new();
            _studentScheduleTable.TopLevel = false;
            mainpage.Controls.Add(_studentScheduleTable);
            _studentScheduleTable.Show();
        }

        private void CourseButton_Click(object sender, EventArgs e)
        {
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            _studentCourseSubwindow = new();
            _studentCourseSubwindow.TopLevel = false;
            _studentCourseSubwindow.PauseTimeDelegate = PauseTime;
            mainpage.Controls.Add(_studentCourseSubwindow);
            _studentCourseSubwindow.Show();
        }

        private void ExamButton_Click(object sender, EventArgs e)
        {
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            _studentExamSubwindow = new();
            _studentExamSubwindow.TopLevel = false;
            _studentExamSubwindow.PauseTimeDelegate = PauseTime;
            mainpage.Controls.Add(_studentExamSubwindow);
            _studentExamSubwindow.Show();
        }

        private void GroupActivityButton_Click(object sender, EventArgs e)
        {
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            _studentGroupActivitySubwindow = new();
            _studentGroupActivitySubwindow.TopLevel = false;
            _studentGroupActivitySubwindow.PauseTimeDelegate = PauseTime;
            mainpage.Controls.Add(_studentGroupActivitySubwindow);
            _studentGroupActivitySubwindow.Show();
        }

        private void PersonalActivityButton_Click(object sender, EventArgs e)
        {
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            _studentPersonalActivitySubwindow = new();
            _studentPersonalActivitySubwindow.TopLevel = false;
            _studentPersonalActivitySubwindow.PauseTimeDelegate = PauseTime;
            mainpage.Controls.Add(_studentPersonalActivitySubwindow);
            _studentPersonalActivitySubwindow.Show();
        }

        private void TemporaryAffairButton_Click(object sender, EventArgs e)
        {
            logoutConfirm.Hide();
            closeConfirm.Hide();
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

            int week = weekBox.SelectedIndex + 1;
            Day day = (Day)dayBox.SelectedIndex;
            int hour = hourBox.SelectedIndex;

            if (MessageBox.Show("周次: " + week.ToString() + "\n日次: " + day.ToString() + "\n时间: " + hour.ToString() +
                                ":00",
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

        private void LogoutConfirm_Click(object sender, EventArgs e)
        {
            MainProgram.Program.Logout();
            _studentScheduleTable?.Close();
            _studentCourseSubwindow?.Close();
            _studentExamSubwindow?.Close();
            _studentGroupActivitySubwindow?.Close();
            _studentPersonalActivitySubwindow?.Close();
            _studentTemporaryAffairSubwindow?.Close();
            _studentScheduleTable = null;
            _studentCourseSubwindow = null;
            _studentExamSubwindow = null;
            _studentGroupActivitySubwindow = null;
            _studentPersonalActivitySubwindow = null;
            _studentTemporaryAffairSubwindow = null;
            this.Close();
        }

        private void CloseConfirm_Click(object sender, EventArgs e)
        {
            LogoutConfirm_Click(sender, e);
            ShouldExitProgram = true;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            _studentScheduleTable?.Hide();
            _studentCourseSubwindow?.Hide();
            _studentExamSubwindow?.Hide();
            _studentGroupActivitySubwindow?.Hide();
            _studentPersonalActivitySubwindow?.Hide();
            _studentTemporaryAffairSubwindow?.Hide();
            logoutConfirm.Show();
            closeConfirm.Show();
        }
    }
}