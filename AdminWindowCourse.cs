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
    public partial class AdminWindowCourse : Form
    {
        public AdminWindowCourse()
        {
            InitializeComponent();
            Generate_MultiSelectBox();
            Generate_Data();
        }

        private List<Schedule.ScheduleBase.SharedData> Data = null;
        public void Generate_MultiSelectBox()
        {
            String[] Days = { "Mon", "Tue", "Wen", "Thu", "Fri", "Sat", "Sun" };
            this.DaySelectBox.InitializeBox(7, Days);

            String[] Weeks = { "Week1", "Week2", "Week3", "Week4", "Week5", "Week6",
                               "Week7", "Week8", "Week9", "Week10", "Week11", "Week12",
                               "Week13","Week14","Week15","Week16",};
            this.WeekSelectBox.InitializeBox(16, Weeks);
        }

        public void Generate_Data()
        {
            CourseData.Rows.Clear();
            int[] widths = { 30, 150, 150, 150, 100, 100 };
            for (int i = 0; i < widths.Length; i++)
            {
                CourseData.Columns[i].Width = widths[i];
            }
            Data = Schedule.ScheduleBase.GetShared(ScheduleType.Course);
            for (int i = 1; i < Data.Count(); i++)
            {
                if(Data[i].RepetitiveType == RepetitiveType.Single)
                {
                    this.CourseData.Rows.Add(null,
                                             Data[i].Name,
                                             Data[i].Timestamp.Week.ToString(),
                                             Data[i].Timestamp.Day.ToString(),
                                             Data[i].Timestamp.Hour.ToString() + ":00",
                                             Data[i].Duration.ToString() + "小时");
                }
                else if (Data[i].RepetitiveType == RepetitiveType.MultipleDays)
                {
                    StringBuilder days = new StringBuilder();
                    for (int j = 0; j < Data[i].ActiveDays.Count(); j++)
                    {
                        days.Append(Data[i].ActiveDays[j].ToString().Substring(0, 3) + ";");
                    }
                    this.CourseData.Rows.Add(null,
                                             Data[i].Name,
                                             "1-16",
                                             days,
                                             Data[i].Timestamp.Hour.ToString() + ":00",
                                             Data[i].Duration.ToString() + "小时");
                }
                else
                {
                    StringBuilder days = new StringBuilder();
                    for (int j = 0; j < Data[i].ActiveDays.Count(); j++)
                    {
                        days.Append(Data[i].ActiveDays[j].ToString().Substring(0, 3) + ";");
                    }
                    this.CourseData.Rows.Add(null,
                                             Data[i].Name,
                                             UI.MainWindow.Continuity(Data[i].ActiveWeeks),
                                             days,
                                             Data[i].Timestamp.Hour.ToString() + ":00",
                                             Data[i].Duration.ToString() + "小时");
                }
            }
        }

        private void AddCourse_Click(object sender, EventArgs e)
        {
            //error
            StringBuilder error_message = new StringBuilder("");

            if (NameBox.Text.Equals(""))
            {
                error_message.AppendLine("请输入课程名！");
            }
            if (WeekSelectBox.valid == 0)
            {
                error_message.AppendLine("请输入上课周！");
            }
            if (DaySelectBox.valid == 0)
            {
                error_message.AppendLine("请输入上课日！");
            }
            if (HourcomboBox.Text == "")
            {
                error_message.AppendLine("请输入上课时间！");
            }
            if (DurcomboBox.Text == "")
            {
                error_message.AppendLine("请输入上课时长！");
            }
            if (!error_message.Equals(""))
            {
                MessageBox.Show(error_message.ToString());
                return;
            }

            //Time info
            RepetitiveType courseType = new RepetitiveType();
            if (WeekSelectBox.valid == 1 && DaySelectBox.valid == 1)
            {
                courseType = RepetitiveType.Single;
            }
            else if (WeekSelectBox.valid == 16)
            {
                courseType = RepetitiveType.MultipleDays;
            }
            else
            {
                courseType = RepetitiveType.Designated;
            }
            int[] activeWeeks = new int[WeekSelectBox.valid];
            int aWcnt = 0;
            for (int i = 0; i < WeekSelectBox.items_cnt; i++)
            {
                if (WeekSelectBox.Selects[i])
                {
                    activeWeeks[aWcnt] = i + 1;
                    aWcnt++;
                }
            }
            Day[] activeDays = new Day[DaySelectBox.valid];
            int aDcnt = 0;
            //activeDays = activeDays.Append();
            for (int i = 0; i < DaySelectBox.items_cnt; i++)
            {
                if (DaySelectBox.Selects[i])
                {
                    activeDays[aDcnt] = Constants.AllDays[i];
                    aDcnt++;
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
            StringBuilder courseDetail = new StringBuilder("");
            courseDetail.Append("\n上课周：" + UI.MainWindow.Continuity(activeWeeks));
            courseDetail.Append("\n上课日：");
            for (int i = 0; i < aDcnt; i++)
            {
                courseDetail.Append(activeDays[i].ToString() + "; ");
            }
            courseDetail.Append("\n时间: " + HourcomboBox.Text + "\n时长: " + DurcomboBox.Text +
                                 "\n课程名称：" + NameBox.Text + "\n类型：" + courseType.ToString());

            if (MessageBox.Show(courseDetail.ToString(), "课程信息", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //加课程
                try
                {
                    if (courseType == RepetitiveType.Single)
                    {
                        Schedule.Course course = new(RepetitiveType.Single,
                                                     NameBox.Text,
                                                     new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginhour },
                                                     duration,
                                                     null,
                                                     new Map.Location.Building(1,
                                                                               "default building",
                                                                               new() { Id = -1, X = 0, Y = 0 }),
                                                     Constants.EmptyIntArray,
                                                     Constants.EmptyDayArray,
                                                     addOnTimeline: false);
                    }
                    else if (courseType == RepetitiveType.MultipleDays)
                    {
                        Schedule.Course course = new(RepetitiveType.MultipleDays,
                                                     NameBox.Text,
                                                     new() { Hour = beginhour },
                                                     duration,
                                                     null,
                                                     new Map.Location.Building(1,
                                                                               "default building",
                                                                               new() { Id = 0, X = 0, Y = 0 }),
                                                     Constants.EmptyIntArray,
                                                     activeDays,
                                                     addOnTimeline: false);
                    }
                    else
                    {
                        Schedule.Course course = new(RepetitiveType.Designated,
                                                     NameBox.Text,
                                                     new() { Hour = beginhour },
                                                     duration,
                                                     null,
                                                     new Map.Location.Building(1,
                                                                               "default building",
                                                                               new() { Id = 0, X = 0, Y = 0 }),
                                                     activeWeeks,
                                                     activeDays,
                                                     addOnTimeline: false);
                    }
                    MessageBox.Show("已成功添加该课程");
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
                //不加课程
                MessageBox.Show("已取消添加该课程");
            }

            //Schedule.Course course
            /*
            Schedule.Course course = new(null,
                                             RepetitiveType.Designated,
                                             "test course*",
                                             new() { Week = 1, Day = Day.Monday, Hour = 12 },
                                             2,
                                             null,
                                             new Map.Location.Building(1,
                                                                       "test building",
                                                                       new() { Id = 0, X = 0, Y = 0 }),
                                             new[] { 1, 2, 3 },
                                             new[] { Day.Monday, Day.Tuesday });
            
            public Course(long? specifiedId,
                      RepetitiveType repetitiveType,
                      string name,
                      Times.Time beginTime,
                      int duration,
                      string? description,
                      string onlineLink,
                      int[] activeWeeks,
                      Day[] activeDays)
             
             */
            //Constants.EmptyDayArray;
        }


    }
}
