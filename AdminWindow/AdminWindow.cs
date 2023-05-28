namespace StudentScheduleManagementSystem.UI
{
    public partial class AdminWindow : Form
    {
        private CourseSubwindow? _courseSubwindow;
        private ExamSubwindow? _examSubwindow;
        private GroupActivitySubwindow? _groupActivitySubwindow;
        private MapEditWindow? _mapEditWindow;
        public bool ShouldExitProgram { get; private set; } = false;

        public AdminWindow()
        {
            InitializeComponent();
            Log.LogBase.LogGenerated += OnLogGenerated;
            this.logoutConfirm.Hide();
            this.closeConfirm.Hide();
            this.logListBox.Hide();
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

        private void CloseConfirm_Click(object sender, EventArgs e)
        {
            LogoutConfirm_Click(sender, e);
            ShouldExitProgram = true;
        }

        private void CourseButton_Click(object sender, EventArgs e)
        {
            logListBox.Hide();
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
            logListBox.Hide();
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
            logListBox.Hide();
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
            logListBox.Hide();
            mainpage.Controls.Clear();
            this.logoutConfirm.Show();
            this.closeConfirm.Show();
        }

        private void LogButton_Click(object sender, EventArgs e)
        {
            this.logoutConfirm.Hide();
            this.closeConfirm.Hide();
            mainpage.Controls.Clear();
            mainpage.Controls.Add(logListBox);
            this.logListBox.Show();
            this.logListBox.BringToFront();
        }

        private void MapEditButton_Click(object sender, EventArgs e)
        {
            _mapEditWindow = new();
            _mapEditWindow.ShowDialog();
            _mapEditWindow.Dispose();
            GC.Collect();
        }

        private void OnLogGenerated(string message)
        {
            if (logListBox.Items.Count > 100)
            {
                logListBox.Items.RemoveAt(0);
            }
            logListBox.Items.Add(message);
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
