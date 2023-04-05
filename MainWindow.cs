namespace StudentScheduleManagementSystem.UI
{
    public partial class MainWindow : Form
    {
        public static Point myPoint = new Point { };
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

        private void move_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                timer1.Enabled = true;

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}