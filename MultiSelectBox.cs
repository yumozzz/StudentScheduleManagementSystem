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
    public partial class MultiSelectBox : UserControl
    {
        private const int gapHeight = 24;
        private const int height_hide = 30;
        private int height_show;

        public int items_cnt = 0;
        public int valid = 0;
        public Boolean[] Selects;
        public String[] NameofCkbox;
        public Boolean browse_show = false;

        public CheckBox[] CBs;
        public MultiSelectBox()
        {
            InitializeComponent();
        }

        public void InitializeBox(int items_cnt, String[] Names)
        {

            this.items_cnt = items_cnt;
            Selects = new Boolean[items_cnt];
            NameofCkbox = new String[items_cnt];
            CBs = new CheckBox[items_cnt];
            for (int i = 0; i < items_cnt; i++)
            {
                NameofCkbox[i] = Names[i];
                Selects[i] = false;
                CBs[i] = new CheckBox();
            }

            int cnt = 0;
            foreach (CheckBox CB in CBs)
            {
                CB.Location = new System.Drawing.Point(0, height_hide + gapHeight + cnt * 28);
                CB.Name = NameofCkbox[cnt];
                CB.Size = new System.Drawing.Size(300, 28);
                CB.TabIndex = 1;
                CB.Text = NameofCkbox[cnt];
                CB.UseVisualStyleBackColor = true;
                CB.CheckedChanged += new System.EventHandler((sender, e) =>
                {
                    Update_textBox();
                });
                CB.MouseLeave += new System.EventHandler((sender, e) =>
                {
                    CB.BackColor = Color.White;
                });
                CB.MouseMove += new System.Windows.Forms.MouseEventHandler((sender, e) =>
                {
                    CB.BackColor = Color.DodgerBlue;
                });
                Controls.Add(CB);
                cnt++;
            }

            height_show = height_hide + items_cnt * CBs[0].Height + gapHeight;

            pictureBox2.Hide();
        }

        public void Update_textBox()
        {
            valid = 0;
            textBox.Text = "";
            for (int i = 0; i < items_cnt; i++)
            {
                this.Selects[i] = CBs[i].Checked;
                if (this.Selects[i])
                {
                    textBox.Text += NameofCkbox[i] + ";";
                    valid++;
                }
            }
        }

        public void Select_All_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < items_cnt; i++)
            {
                CBs[i].Checked = true;
            }
            Update_textBox();
        }

        public void Cancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < items_cnt; i++)
            {
                CBs[i].Checked = false;
            }
            Update_textBox();
        }

        private void MultiSelectBox_Load(object sender, EventArgs e)
        {
            this.Height = height_hide;
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            if (browse_show == false)
            {
                pictureBox1_Click(sender, e);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (browse_show)
            {
                this.Height = height_hide;
                browse_show = false;
                pictureBox2.Hide();
            }
            else
            {
                this.Height = height_show;
                browse_show = true;
                pictureBox2.Show();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (browse_show)
            {
                this.Height = height_hide;
                browse_show = false;
                pictureBox2.Hide();
            }
            else
            {
                this.Height = height_show;
                browse_show = true;
                pictureBox2.Show();
            }
        }
    }
}