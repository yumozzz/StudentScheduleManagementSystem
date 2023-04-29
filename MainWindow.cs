using System.Runtime.InteropServices;
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

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
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
                    else if(MainProgram.Program.Identity == Identity.Administrator)
                    {
                        AdminSubwindow = new AdminWindow();
                        AdminSubwindow.ShowDialog();
                    }
                    
                    this.Show();
                }
                else
                {
                    MessageBox.Show("fail!");
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
                    MessageBox.Show("fail!");
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

        public static StringBuilder Continuity(int[] activeWeeks)
        {
            if (activeWeeks.Length == 1)
            {
                return new StringBuilder(activeWeeks[0].ToString());
            }

            int continuity = 0;
            StringBuilder ret = new("");
            for (int i = 1; i < activeWeeks.Length; i++)
            {
                if (activeWeeks[i] == activeWeeks[i - 1] + 1)
                {
                    if (continuity == 0)
                    {
                        if (i != 1)
                        {
                            ret.Append(", ");
                        }
                        ret.Append(activeWeeks[i - 1].ToString());
                    }
                    continuity++;
                }
                else
                {
                    if (continuity == 0)
                    {
                        if (i != 1)
                        {
                            ret.Append(", ");
                        }
                        ret.Append(activeWeeks[i - 1].ToString());
                    }
                    else
                    {
                        ret.Append("-" + activeWeeks[i - 1].ToString());
                    }
                    continuity = 0;
                }
            }
            
            if (continuity == 0)
            {
                ret.Append(", " + activeWeeks[activeWeeks.Length - 1].ToString());
            }
            else
            {
                ret.Append("-" + activeWeeks[activeWeeks.Length - 1].ToString());
            }

            return ret;
        }

        
        public static StringBuilder GenerateScheduleDetail(Schedule.ScheduleBase.SharedData data)
        {
            StringBuilder ret = new("");
            StringBuilder week = new("");
            StringBuilder day = new("");
            //ScheduleType.Course

            if (data.RepetitiveType == RepetitiveType.Single)
            {
                week.Append(data.Timestamp.Week.ToString());
                day.Append(data.Timestamp.Day.ToString());
            }
            else if (data.RepetitiveType == RepetitiveType.MultipleDays)
            {
                week.Append("1-16");
                for (int j = 0; j < data.ActiveDays.Count(); j++)
                {
                    day.Append(data.ActiveDays[j].ToString().Substring(0, 3) + ";");
                }
            }
            else
            {
                week = Continuity(data.ActiveWeeks);
                for (int j = 0; j < data.ActiveDays.Count(); j++)
                {
                    day.Append(data.ActiveDays[j].ToString().Substring(0, 3) + ";");
                }
            }

            String[] type = new String[2];
            if(data.ScheduleType == ScheduleType.Course)
            {
                type[0] = "课程";
                type[1] = "上课";
            }
            else if (data.ScheduleType == ScheduleType.Activity)
            {
                type[0] = "活动";
                type[1] = "活动";
            }
            else
            {
                type[0] = "考试";
                type[1] = "考试";
            }

            ret.Append(type[0] + "名称：" + data.Name + "\n" + 
                       type[1] + "周：" + week + "\n" +
                       type[1] + "日：" + day + "\n" +
                       "时间: " + data.Timestamp.Hour.ToString() + ":00" + "\n" +
                       "时长: " + data.Duration.ToString() + "小时" + "\n" +
                       "ID: " + data.Id.ToString());

            return ret;
        }
        
        /*
        public void Delete_Click(DataGridView ActivityData)
        {

        }
        */
    }
}