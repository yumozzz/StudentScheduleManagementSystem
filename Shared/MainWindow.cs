namespace StudentScheduleManagementSystem.UI
{
    public partial class MainWindow : Form
    {
        public static StudentWindow? StudentWindow { get; private set; }
        public static AdminWindow? AdminWindow { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            this.close.Click += (sender, e) => Exit();
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

        private void ClearButton_Click(object sender, EventArgs e)
        {
            this.usernameBox.Text = "";
            this.passwordBox.Text = "";
        }

        private void Exit()
        {
            this.Close();
            this.Dispose();
            MainProgram.Program.Exit();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (usernameBox.Text == "" || passwordBox.Text == "")
            {
                MessageBox.Show("输入为空!");
                return;
            }
            if (!MainProgram.Program.Login(usernameBox.Text, passwordBox.Text))
            {
                return;
            }
            this.usernameBox.Text = "";
            this.passwordBox.Text = "";
            MessageBox.Show("登录成功!");
            this.Hide();
            Thread windowThread, clockThread = new(Times.Timer.Start);
            MainProgram.Program.Cts = new();
            if (MainProgram.Program.Identity == Identity.User)
            {
                StudentWindow = new();
                windowThread = new(() => StudentWindow.ShowDialog());
                windowThread.SetApartmentState(ApartmentState.STA);
                windowThread.Start();
                clockThread.Start();
                windowThread.Join();
                MainProgram.Program.Cts.Cancel();
                if (StudentWindow.ShouldExitProgram)
                {
                    Exit();
                    return;
                }
                StudentWindow.Dispose();
            }
            else
            {
                AdminWindow = new();
                windowThread = new(() => AdminWindow.ShowDialog());
                windowThread.SetApartmentState(ApartmentState.STA);
                windowThread.Start();
                clockThread.Start();
                windowThread.Join();
                MainProgram.Program.Cts.Cancel();
                if (AdminWindow.ShouldExitProgram)
                {
                    Exit();
                    return;
                }
                AdminWindow.Dispose();
            }
            GC.Collect();
            this.Show();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (usernameBox.Text == "" || passwordBox.Text == "")
            {
                MessageBox.Show("输入为空!");
                return;
            }
            if (!MainProgram.Program.Register(usernameBox.Text, passwordBox.Text))
            {
                return;
            }
            MessageBox.Show("注册成功!");
            this.Hide();
            StudentWindow = new();
            Thread thread = new(() => StudentWindow.ShowDialog());
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            Thread.Sleep(100);
            StudentWindow.Dispose();
            GC.Collect();
            this.Show();
        }
    }
}