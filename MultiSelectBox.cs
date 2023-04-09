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
        private const int gapHeight = 6;
        public int items_cnt = 0;
        private int height_hide = 30;
        private int height_show;
        public Boolean[] Selects = new Boolean[25];
        public String[] NameofCkbox = new String[25];
        private Boolean browse_show = false;
        public MultiSelectBox()
        {
            InitializeComponent();
            for(int i = 0; i <= items_cnt; i++)
            {
                this.Selects[i] = false;
            }
        }

        public void Add(String item)
        {
            items_cnt++;
            height_show = height_hide + items_cnt * checkBox1.Height +
                          Select_All.Height + (1 + items_cnt) * gapHeight;
            this.NameofCkbox[items_cnt] = item;
            switch (items_cnt)
            {
                case 1:
                    checkBox1.Text = item;
                    break;
                case 2:
                    checkBox2.Text = item;
                    break;
                case 3:
                    checkBox3.Text = item;
                    break;
                case 4:
                    checkBox4.Text = item;
                    break;
                case 5:
                    checkBox5.Text = item;
                    break;
                case 6:
                    checkBox6.Text = item;
                    break;
                case 7:
                    checkBox7.Text = item;
                    break;
                case 8:
                    checkBox8.Text = item;
                    break;
                case 9:
                    checkBox9.Text = item;
                    break;
                case 10:
                    checkBox10.Text = item;
                    break;
                case 11:
                    checkBox11.Text = item;
                    break;
                case 12:
                    checkBox12.Text = item;
                    break;
                case 13:
                    checkBox13.Text = item;
                    break;
                case 14:
                    checkBox14.Text = item;
                    break;
                case 15:
                    checkBox15.Text = item;
                    break;
                case 16:
                    checkBox16.Text = item;
                    break;
                case 17:
                    checkBox17.Text = item;
                    break;
                case 18:
                    checkBox18.Text = item;
                    break;
                case 19:
                    checkBox19.Text = item;
                    break;
                case 20:
                    checkBox20.Text = item;
                    break;
                case 21:
                    checkBox21.Text = item;
                    break;
                case 22:
                    checkBox22.Text = item;
                    break;
                case 23:
                    checkBox23.Text = item;
                    break;
                case 24:
                    checkBox24.Text = item;
                    break;
            }
        }

        private void Update_textBox()
        {
            textBox.Text = "";
            for(int i = 1; i <= items_cnt; i++)
            {
                if (Selects[i])
                {
                    textBox.Text += NameofCkbox[i] + ";";
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Selects[1] = true;
            }
            else
            {
                Selects[1] = false;
            }
            Update_textBox();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Selects[2] = true;
            }
            else
            {
                Selects[2] = false;
            }
            Update_textBox();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                Selects[3] = true;
            }
            else
            {
                Selects[3] = false;
            }
            Update_textBox();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                Selects[4] = true;
            }
            else
            {
                Selects[4] = false;
            }
            Update_textBox();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                Selects[5] = true;
            }
            else
            {
                Selects[5] = false;
            }
            Update_textBox();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                Selects[6] = true;
            }
            else
            {
                Selects[6] = false;
            }
            Update_textBox();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                Selects[7] = true;
            }
            else
            {
                Selects[7] = false;
            }
            Update_textBox();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                Selects[8] = true;
            }
            else
            {
                Selects[8] = false;
            }
            Update_textBox();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
            {
                Selects[9] = true;
            }
            else
            {
                Selects[9] = false;
            }
            Update_textBox();
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                Selects[10] = true;
            }
            else
            {
                Selects[10] = false;
            }
            Update_textBox();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked)
            {
                Selects[11] = true;
            }
            else
            {
                Selects[11] = false;
            }
            Update_textBox();
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked)
            {
                Selects[12] = true;
            }
            else
            {
                Selects[12] = false;
            }
            Update_textBox();
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked)
            {
                Selects[13] = true;
            }
            else
            {
                Selects[13] = false;
            }
            Update_textBox();
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox14.Checked)
            {
                Selects[14] = true;
            }
            else
            {
                Selects[14] = false;
            }
            Update_textBox();
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox15.Checked)
            {
                Selects[15] = true;
            }
            else
            {
                Selects[15] = false;
            }
            Update_textBox();
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox16.Checked)
            {
                Selects[16] = true;
            }
            else
            {
                Selects[16] = false;
            }
            Update_textBox();
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox17.Checked)
            {
                Selects[17] = true;
            }
            else
            {
                Selects[17] = false;
            }
            Update_textBox();
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox18.Checked)
            {
                Selects[18] = true;
            }
            else
            {
                Selects[18] = false;
            }
            Update_textBox();
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox19.Checked)
            {
                Selects[19] = true;
            }
            else
            {
                Selects[19] = false;
            }
            Update_textBox();
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox20.Checked)
            {
                Selects[20] = true;
            }
            else
            {
                Selects[20] = false;
            }
            Update_textBox();
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox21.Checked)
            {
                Selects[21] = true;
            }
            else
            {
                Selects[21] = false;
            }
            Update_textBox();
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox22.Checked)
            {
                Selects[22] = true;
            }
            else
            {
                Selects[22] = false;
            }
            Update_textBox();
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox23.Checked)
            {
                Selects[23] = true;
            }
            else
            {
                Selects[23] = false;
            }
            Update_textBox();
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox24.Checked)
            {
                Selects[24] = true;
            }
            else
            {
                Selects[24] = false;
            }
            Update_textBox();
        }

        private void Select_All_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;
            checkBox7.Checked = true;
            checkBox8.Checked = true;
            checkBox9.Checked = true;
            checkBox10.Checked = true;
            checkBox11.Checked = true;
            checkBox12.Checked = true;
            checkBox13.Checked = true;
            checkBox14.Checked = true;
            checkBox15.Checked = true;
            checkBox16.Checked = true;
            checkBox17.Checked = true;
            checkBox18.Checked = true;
            checkBox19.Checked = true;
            checkBox20.Checked = true;
            checkBox21.Checked = true;
            checkBox22.Checked = true;
            checkBox23.Checked = true;
            checkBox24.Checked = true;
        }

        private void Cancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;
            checkBox11.Checked = false;
            checkBox12.Checked = false;
            checkBox13.Checked = false;
            checkBox14.Checked = false;
            checkBox15.Checked = false;
            checkBox16.Checked = false;
            checkBox17.Checked = false;
            checkBox18.Checked = false;
            checkBox19.Checked = false;
            checkBox20.Checked = false;
            checkBox21.Checked = false;
            checkBox22.Checked = false;
            checkBox23.Checked = false;
            checkBox24.Checked = false;
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            if (browse_show)
            {
                this.Height = height_hide;
                browse_show = false;
            }
            else
            {
                this.Height = height_show;
                browse_show = true;
            }
        }

        private void MultiSelectBox_Load(object sender, EventArgs e)
        {
            this.Height = height_hide;
        }
    }
}
