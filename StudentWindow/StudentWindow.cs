using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace StudentScheduleManagementSystem.UI
{
    public partial class StudentWindow : Form
    {
        public StudentScheduleTable? StudentScheduleTable { get; private set; }
        public StudentCourseSubwindow? StudentCourseSubwindow { get; private set; }
        public StudentExamSubwindow? StudentExamSubwindow { get; private set; }
        public StudentGroupActivitySubwindow? StudentGroupActivitySubwindow { get; private set; }
        public StudentPersonalActivitySubwindow? StudentPersonalActivitySubwindow { get; private set; }
        public StudentTemporaryAffairSubwindow? StudentTemporaryAffairSubwindow { get; private set; }
        public bool ShouldExitProgram { get; private set; } = false;

        public StudentWindow()
        {
            InitializeComponent();
            Times.Timer.TimeChange += SetLocalTime;
            pauseButton.Click += (sender, e) => { Times.Timer.Pause = !Times.Timer.Pause; };
            speedButton.Click += (sender, e) => Times.Timer.SetSpeed();
            Log.LogBase.LogGenerated += (message) =>
            {
                if (logListBox.InvokeRequired)
                {
                    logListBox.Invoke(OnLogGenerated, message);
                }
                else
                {
                    OnLogGenerated(message);
                }
            };
            Times.Timer.SetPauseState += (pause) =>
            { 
                if (pauseButton.InvokeRequired)
                {
                    pauseButton.Invoke(() => Text = pause ? "继续" : "暂停");
                }
                else
                {
                    pauseButton.Text = pause ? "继续" : "暂停";
                }
            };
            logListBox.Hide();
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

        private void ScheduleTableButton_Click(object sender, EventArgs e)
        {
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            StudentScheduleTable = new() { MainWindow = this };
            StudentScheduleTable.TopLevel = false;
            mainpage.Controls.Add(StudentScheduleTable);
            StudentScheduleTable.Show();
        }

        public void CourseButton_Click(object sender, EventArgs e)
        {
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            StudentCourseSubwindow = new();
            StudentCourseSubwindow.TopLevel = false;
            mainpage.Controls.Add(StudentCourseSubwindow);
            StudentCourseSubwindow.Show();
        }

        public void ExamButton_Click(object sender, EventArgs e)
        {
            logListBox.Hide();
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            StudentExamSubwindow = new();
            StudentExamSubwindow.TopLevel = false;
            mainpage.Controls.Add(StudentExamSubwindow);
            StudentExamSubwindow.Show();
        }

        public void GroupActivityButton_Click(object sender, EventArgs e)
        {
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            StudentGroupActivitySubwindow = new();
            StudentGroupActivitySubwindow.TopLevel = false;
            mainpage.Controls.Add(StudentGroupActivitySubwindow);
            StudentGroupActivitySubwindow.Show();
        }

        public void PersonalActivityButton_Click(object sender, EventArgs e)
        {
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            StudentPersonalActivitySubwindow = new();
            StudentPersonalActivitySubwindow.TopLevel = false;
            mainpage.Controls.Add(StudentPersonalActivitySubwindow);
            StudentPersonalActivitySubwindow.Show();
        }

        public void TemporaryAffairButton_Click(object sender, EventArgs e)
        {
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            StudentTemporaryAffairSubwindow = new();
            StudentTemporaryAffairSubwindow.TopLevel = false;
            mainpage.Controls.Add(StudentTemporaryAffairSubwindow);
            StudentTemporaryAffairSubwindow.Show();
        }

        private void SetTimeButton_Click(object sender, EventArgs e)
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

            if (MessageBox.Show($"周次：{week.ToString()}\n日次：{day.ToString()}\n时间：{hour.ToString()}:00",
                                "确认时间修改",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Times.Time time = new() { Week = week, Day = day, Hour = hour };
                Times.Timer.SetTime(time);
            }
            weekBox.SelectedIndex = -1;
            dayBox.SelectedIndex = -1;
            hourBox.SelectedIndex = -1;
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
            StudentScheduleTable?.Close();
            StudentCourseSubwindow?.Close();
            StudentExamSubwindow?.Close();
            StudentGroupActivitySubwindow?.Close();
            StudentPersonalActivitySubwindow?.Close();
            StudentTemporaryAffairSubwindow?.Close();
            StudentScheduleTable = null;
            StudentCourseSubwindow = null;
            StudentExamSubwindow = null;
            StudentGroupActivitySubwindow = null;
            StudentPersonalActivitySubwindow = null;
            StudentTemporaryAffairSubwindow = null;
            this.Close();
        }

        private void CloseConfirm_Click(object sender, EventArgs e)
        {
            LogoutConfirm_Click(sender, e);
            ShouldExitProgram = true;
        }

        private void LogButton_Click(object sender, EventArgs e)
        {
            StudentScheduleTable?.Hide();
            StudentCourseSubwindow?.Hide();
            StudentExamSubwindow?.Hide();
            StudentGroupActivitySubwindow?.Hide();
            StudentPersonalActivitySubwindow?.Hide();
            StudentTemporaryAffairSubwindow?.Hide();
            logoutConfirm.Hide();
            closeConfirm.Hide();
            mainpage.Controls.Clear();
            if (logListBox.InvokeRequired)
            {
                logListBox.Invoke(() =>
                {
                    mainpage.Controls.Add(logListBox);
                    Show();
                });
            }
            else
            {
                mainpage.Controls.Add(logListBox);
                logListBox.Show();
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            logListBox.Hide();
            StudentScheduleTable?.Hide();
            StudentCourseSubwindow?.Hide();
            StudentExamSubwindow?.Hide();
            StudentGroupActivitySubwindow?.Hide();
            StudentPersonalActivitySubwindow?.Hide();
            StudentTemporaryAffairSubwindow?.Hide();
            logoutConfirm.Show();
            closeConfirm.Show();
        }

        private void OnLogGenerated(string message)
        {
            if (logListBox.Items.Count > 100)
            {
                logListBox.Items.RemoveAt(0);
            }
            logListBox.Items.Add(message);
            if (logListBox.Visible)
            {
                Graphics graphics = logListBox.CreateGraphics();
                float width = 0f;
                foreach (var item in logListBox.Items)
                {
                    width = Math.Max(width, graphics.MeasureString(item.ToString()!.Replace("\r\n", " ").Replace('\n', ' '), logListBox.Font).Width);
                }
                logListBox.HorizontalExtent = Convert.ToInt32(width) + 20;
            }
        }
    }
}