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
    public partial class AdminWindowExam : Form
    {
        public AdminWindowExam()
        {
            InitializeComponent();
            Generate_Data();
        }

        private List<Schedule.ScheduleBase.SharedData> Data = null;
        public void Generate_Data()
        {
            ExamData.Rows.Clear();
            int[] widths = { 30, 150, 150, 150, 100, 100 };
            for (int i = 0; i < widths.Length; i++)
            {
                ExamData.Columns[i].Width = widths[i];
            }
            Data = Schedule.ScheduleBase.GetShared(ScheduleType.Exam);
            for (int i = 1; i < Data.Count(); i++)
            {
                this.ExamData.Rows.Add(null,
                                       Data[i].Name,
                                       Data[i].Timestamp.Week.ToString(),
                                       Data[i].Timestamp.Day.ToString(),
                                       Data[i].Timestamp.Hour.ToString() + ":00",
                                       Data[i].Duration.ToString() + "小时");
            }
        }

        private void AddTest_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder("");

            if (NameBox.Text.Equals(""))
            {
                ErrorMessage.AppendLine("请输入考试科目！");
            }
            if (WeekcomboBox.Text.Equals(""))
            {
                ErrorMessage.AppendLine("请输入考试周！");
            }
            if (DaycomboBox.Text.Equals(""))
            {
                ErrorMessage.AppendLine("请输入考试日！");
            }
            if (HourcomboBox.Text == "")
            {
                ErrorMessage.AppendLine("请输入考试时间！");
            }
            if (DurcomboBox.Text == "")
            {
                ErrorMessage.AppendLine("请输入考试时长！");
            }
            if (!ErrorMessage.Equals(""))
            {
                MessageBox.Show(ErrorMessage.ToString());
                return;
            }

            int activeWeek = 0;
            if(WeekcomboBox.Text.Length == 5)
            {
                activeWeek = WeekcomboBox.Text[4] - '0';
            }
            else
            {
                activeWeek = (WeekcomboBox.Text[4] - '0') * 10 + WeekcomboBox.Text[5];
            }

            Day activeDay = Day.Monday;
            for(int i = 0; i < Constants.AllDays.Length; i++)
            {
                if (DaycomboBox.Text.Equals(Constants.AllDays[i].ToString()))
                {
                    activeDay = Constants.AllDays[i];
                    break;
                }
            }

            int beginhour;
            int duration = DurcomboBox.Text[0] - '0';

            if (HourcomboBox.Text.Length == 5)
            {
                beginhour = (HourcomboBox.Text[0] - '0') * 10 + HourcomboBox.Text[1] - '0';
            }
            else
            {
                beginhour = HourcomboBox.Text[0] - '0';
            }

            //Double check
            StringBuilder examDetail = new StringBuilder("");
            examDetail.Append("考试周：" + WeekcomboBox.Text + "\n考试日：" + DaycomboBox.Text + 
                              "\n时间: " + HourcomboBox.Text + "\n时长: " + DurcomboBox.Text +
                              "\n考试科目：" + NameBox.Text);

            if (MessageBox.Show(examDetail.ToString(), "考试信息", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    Schedule.Exam exam = new(NameBox.Text,
                                             new() { Week = activeWeek, Day = activeDay, Hour = beginhour },
                                             duration,
                                             null,
                                             new Map.Location.Building(1,
                                                                       "default building",
                                                                       new() { Id = -1, X = 0, Y = 0 }),
                                             null,
                                             addOnTimeline: false);
                    
                    MessageBox.Show("已成功添加该考试");
                    Generate_Data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Log.Error.Log(null, ex);
                }
            }
            else
            {
                MessageBox.Show("已取消添加该考试");
            }
            /*public Exam(string name,
                    Times.Time beginTime,
                    int duration,
                    string? description,
                    Map.Location.Building offlineLocation,
                    long? specifiedId = null,
                    bool addOnTimeline = true)
            */

        }
    }
}
