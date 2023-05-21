using System.Text;

namespace StudentScheduleManagementSystem.UI
{
    public partial class MainWindow : Form
    {
        public static StudentWindow? StudentSubwindow { get; private set; }
        public static AdminWindow? AdminSubwindow { get; private set; }

        public MainWindow()
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

        private void ClearButton_Click(object sender, EventArgs e)
        {
            this.usernamebox.Text = "";
            this.passwordbox.Text = "";
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (usernamebox.Text == "" || passwordbox.Text == "")
            {
                MessageBox.Show("Empty!");
            }
            else
            {
                if (MainProgram.Program.Login(usernamebox.Text, passwordbox.Text))
                {
                    MessageBox.Show("Successfully login!");
                    this.Hide();
                    Thread windowThread, clockThread = new(Times.Timer.Start);
                    MainProgram.Program.Cts = new();
                    if (MainProgram.Program.Identity == Identity.User)
                    {
                        StudentSubwindow = new();
                        windowThread = new(() => StudentSubwindow.ShowDialog());
                        windowThread.SetApartmentState(ApartmentState.STA);
                        windowThread.Start();
                        clockThread.Start();
                        windowThread.Join();
                        MainProgram.Program.Cts.Cancel();
                        StudentSubwindow.Dispose();
                    }
                    else
                    {
                        AdminSubwindow = new();
                        windowThread = new(() => AdminSubwindow.ShowDialog());
                        windowThread.SetApartmentState(ApartmentState.STA);
                        windowThread.Start();
                        clockThread.Start();
                        windowThread.Join();
                        MainProgram.Program.Cts.Cancel();
                        AdminSubwindow.Dispose();
                    }
                    GC.Collect();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Fail!");
                }
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (usernamebox.Text == "" || passwordbox.Text == "")
            {
                MessageBox.Show("Empty!");
            }
            else
            {
                if (MainProgram.Program.Register(usernamebox.Text, passwordbox.Text))
                {
                    MessageBox.Show("Successfully register!");

                    this.Hide();
                    StudentSubwindow = new();
                    Thread thread = new(() => StudentSubwindow.ShowDialog());
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    thread.Join();
                    StudentSubwindow.Dispose();
                    GC.Collect();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Fail!");
                }
            }
        }

        private void UsernameBox_TextChanged(object sender, EventArgs e)
        {
            passwordbox.Text = "";
        }
    }
}