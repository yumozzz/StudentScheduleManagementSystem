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
    public partial class ShowMap : Form
    {
        Map.Location.Point point1;
        Map.Location.Point point2;
        public ShowMap(Map.Location.Point point1, Map.Location.Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.　　　　　　
            Pen p = new Pen(Color.Blue, 2);//定义了一个蓝色,宽度为2的画笔　
            g.DrawLine(p, point1.X, point1.Y, point2.X, point2.Y);//在画板上画直线,起始坐标为(10,10),终点坐标为(100,100)        
            g.DrawRectangle(p, point1.X, point1.Y, point2.X, point2.Y);//在画板上画矩形,起始坐标为(10,10),宽为,高为　        
            g.DrawEllipse(p, point1.X, point1.Y, point2.X, point2.Y);//在画板上画椭圆,起始坐标为(10,10),外接矩形的宽为,高为100　　　
            //g.DrawBeziers(pen, );
        }
    }
}
