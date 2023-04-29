using System.Text;

namespace StudentScheduleManagementSystem.UI
{
    public partial class MainWindow : Form
    {
        public static StudentWindow? StudentSubwindow { get; private set; } = null;
        public static AdminWindow? AdminSubwindow { get; private set; } = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        //private void MainWindow_Load(object sender, EventArgs e)
        //{

        //}

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        /*private void move_Paint(object sender, PaintEventArgs e)
        {

        }*/

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
                    if (MainProgram.Program.Identity == Identity.User)
                    {
                        /*
                        StudentSubwindow = new StudentSubwindow();
                        StudentSubwindow.ShowDialog();
                        //debug
                        */
                        AdminSubwindow = new AdminWindow();
                        AdminSubwindow.ShowDialog();
                        
                    }
                    else
                    {
                        AdminSubwindow = new AdminWindow();
                        AdminSubwindow.ShowDialog();
                    }
                    
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Fail!");
                }
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            this.usernamebox.Text = "";
            this.passwordbox.Text = "";
        }

        private void register_Click(object sender, EventArgs e)
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

                    StudentSubwindow = new StudentWindow();
                    this.Hide();
                    StudentSubwindow.ShowDialog();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Fail!");
                }
            }
        }

        private void usernamebox_TextChanged(object sender, EventArgs e)
        {
            passwordbox.Text = "";
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