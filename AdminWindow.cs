using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    SubWindows: 1050, 655
    180, 58    
    175, 53
*/


namespace StudentScheduleManagementSystem.UI
{
    public partial class AdminWindow : Form
    {
        public static AdminWindowCourse? AdminWindowCourse { get; set; } = null;
        public static AdminWindowExam? AdminWindowExam { get; set; } = null;
        public static AdminWindowActivity? AdminWindowActivity { get; set; } = null;

        public AdminWindow()
        {
            InitializeComponent();
            AdminWindowCourse = new AdminWindowCourse();
            AdminWindowExam = new AdminWindowExam();
            AdminWindowActivity = new AdminWindowActivity();
            this.Logout.Hide();
            this.ClosePage.Hide();
        }

        //窗口拖动
        int oldX, oldY;
        private void Header_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.Location.X - this.oldX;
                this.Top += e.Location.Y - this.oldY;
            }
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            MainProgram.Program.Logout();
            AdminWindowCourse.Dispose();
            AdminWindowCourse.Close();
            AdminWindowExam.Dispose();
            AdminWindowExam.Close();
            AdminWindowActivity.Dispose();
            AdminWindowActivity.Close();
            this.Dispose();
            this.Close();
        }

        private void ClosePage_Click(object sender, EventArgs e)
        {
            MainProgram.Program.Logout();
            AdminWindowCourse.Close();
            AdminWindowCourse.Dispose();
            AdminWindowExam.Close();
            AdminWindowExam.Dispose();
            AdminWindowActivity.Close();
            AdminWindowActivity.Dispose();
            this.Close();
            this.Dispose();
        }

        private void CourseOP_Click(object sender, EventArgs e)
        {
            this.Logout.Hide();
            this.ClosePage.Hide();
            SubPage.Controls.Clear();
            AdminWindowCourse.TopLevel = false;
            SubPage.Controls.Add(AdminWindowCourse);
            AdminWindowCourse.Show();
        }

        private void TestOP_Click(object sender, EventArgs e)
        {
            this.Logout.Hide();
            this.ClosePage.Hide();
            SubPage.Controls.Clear();
            AdminWindowExam.TopLevel = false;
            SubPage.Controls.Add(AdminWindowExam);
            AdminWindowExam.Show();
        }

        private void ActivityOP_Click(object sender, EventArgs e)
        {
            this.Logout.Hide();
            this.ClosePage.Hide();
            SubPage.Controls.Clear();
            AdminWindowActivity.TopLevel = false;
            SubPage.Controls.Add(AdminWindowActivity);
            AdminWindowActivity.Show();
        }

        private void LogoutClose_Click(object sender, EventArgs e)
        {
            SubPage.Controls.Clear();
            this.Logout.Show();
            this.ClosePage.Show();
        }


        private void Header_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.oldX = e.Location.X;
                this.oldY = e.Location.Y;
            }
        }

        /*
        public static List<SharedData> GetShared(ScheduleType type)
        {
            int i = type switch
            {
                ScheduleType.Course => 1, ScheduleType.Exam => 2, ScheduleType.Activity => 3,
                _ => throw new ArgumentException(null, nameof(type))
            };
            List<SharedData> ret = new();
            foreach (var id in _correspondenceDictionary.Keys)
            {
                if (id % (long)1e9 == i)
                {
                    ret.Add(_correspondenceDictionary[id]);
                }
            }
            return ret;
        }
        public class SharedData
        {
            [JsonConverter(typeof(StringEnumConverter))]
            public ScheduleType @ScheduleType { get; set; }
            public long Id { get; set; }
            public string Name { get; set; }
            [JsonConverter(typeof(StringEnumConverter))]
            public RepetitiveType @RepetitiveType { get; set; }
            [JsonProperty] public int[] ActiveWeeks { get; set; }
            [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
            public Day[] ActiveDays { get; set; }
            public Times.Time Timestamp { get; set; }
            public int Duration { get; set; }
        }
        */


    }
}
