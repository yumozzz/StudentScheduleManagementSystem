using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentScheduleManagementSystem.UI
{

    public partial class StudentWindow : Form
    {
        public delegate void ChangeLocalTimeCallback(Times.Time time);

        public ChangeLocalTimeCallback changeLocalTimeCallback;

        public StudentWindow()
        {
            InitializeComponent();
            this.multiSelectBox1.Add("wk1");
            this.multiSelectBox1.Add("wk2");
            this.multiSelectBox1.Add("wk3");
            this.multiSelectBox1.Add("wk4");
            this.multiSelectBox1.Add("wk5");
            this.multiSelectBox1.Add("wk6");
            this.multiSelectBox1.Add("wk7");
            this.multiSelectBox1.Add("wk8");
            this.multiSelectBox1.Add("wk9");
            this.multiSelectBox1.Add("wk10");
            this.multiSelectBox1.Add("wk11");
            this.multiSelectBox1.Add("wk12");
            this.multiSelectBox1.Add("wk13");
            this.multiSelectBox1.Add("wk14");
            this.multiSelectBox1.Add("wk15");
            this.multiSelectBox1.Add("wk16");
            this.multiSelectBox2.Add("d1");
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

        private void search_Click(object sender, EventArgs e)
        {
            
        }

        private void move_MouseMove(object sender, MouseEventArgs e)
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
            this.Close();
        }

        public void SetLocalTime(Times.Time time)
        {
            if (CurrentTime.InvokeRequired)
            {
                changeLocalTimeCallback = new(SetLocalTime);
                this.CurrentTime.Invoke(SetLocalTime, time);
            }
            else
            {
                this.CurrentTime.Text = time.ToString();
            }
        }
    }
}
