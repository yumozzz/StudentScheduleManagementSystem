namespace StudentScheduleManagementSystem.UI
{
    public partial class MainWindow : Form
    {
        public String username_;
        public String password_;
        public Boolean usertype_;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void move_Paint(object sender, PaintEventArgs e)
        {

        }

        int oldX, oldY;
        private void move_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.oldX = e.Location.X;
                this.oldY = e.Location.Y;
            }
        }

        private void login_Click(object sender, EventArgs e)
        {
            if (username.Text == "" || password.Text == "")
            {
                MessageBox.Show("Empty!");
            }
            else
            {
                log_in();
            }
        }

        private void log_in()
        {
            if (is_student.Checked == true && 
                MainProgram.Program.dicstu.ContainsKey(usernamebox.Text) &&
                MainProgram.Program.dicstu[usernamebox.Text] == passwordbox.Text)
            {
                //MessageBox.Show("Successfully login!");
                username_ = usernamebox.Text;
                password_ = passwordbox.Text;
                usertype_ = is_student.Checked; //student true

                StudentWindow studentwindow = new StudentWindow();
                this.Hide();
                studentwindow.ShowDialog();
                this.Show();
            } else if (is_administrator.Checked == true && 
                       MainProgram.Program.dicadmin.ContainsKey(usernamebox.Text) &&
                       MainProgram.Program.dicadmin[usernamebox.Text] == passwordbox.Text)
            {
                //MessageBox.Show("Successfully login!");
                username_ = usernamebox.Text;
                password_ = passwordbox.Text;
                usertype_ = is_student.Checked; //admin false

                AdminWindow adminwindow = new AdminWindow();
                this.Hide();
                adminwindow.ShowDialog();
                this.Show();
            } else
            {
                MessageBox.Show("fail!");
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            this.usernamebox.Text = "";
            this.passwordbox.Text = "";
        }

        private void move_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.Location.X - this.oldX;
                this.Top += e.Location.Y - this.oldY;
            }
        }
    }
}