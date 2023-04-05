using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentScheduleManagementSystem
{
    public partial class StudentWindow : Form
    {
        public StudentWindow()
        {
            InitializeComponent();
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

        private void logout_Click(object sender, EventArgs e)
        {
            this.Close();
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


    }
}
