using StudentScheduleManagementSystem.Schedule;
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
            //CourseData.Rows[3].Selected = true;
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
            Data = Schedule.ScheduleBase.GetSharedByType(ScheduleType.Course);
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
            if (reviseCourse)
            {
                MessageBox.Show("请先修改课程！");
                return;
            }
            AddOneCourse(null);
        }

        private void DeleteCourse_Click(object sender, EventArgs e)
        {
            if (reviseCourse)
            {
                MessageBox.Show("请先修改课程！");
                return;
            }
            int index = FindValidIndex();
            if(index != -1)
            {
                DeleteOneCourse(index);
            }
        }

        private Boolean AddOneCourse(long? ID)
        {
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
                return false;
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
                    return true;
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
            return false;
        }

        private long DeleteOneCourse(int index)
        {
            StringBuilder courseDetail = UI.MainWindow.GenerateScheduleDetail(Data[index]);

            if (MessageBox.Show(courseDetail.ToString(), "课程信息", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    long ID = Data[index].Id;
                    ScheduleBase.DeleteShared(ID);
                    MessageBox.Show("已成功删除该课程");
                    Generate_Data();
                    return ID;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Log.Error.Log(null, ex);
                }
            }
            else
            {
                MessageBox.Show("已取消删除该课程");
            }
            return 0;
        }

        private int FindValidIndex()
        {
            int cnt = 0, index = -1;
            for (int i = 1; i < Data.Count(); i++)
            {
                if (Convert.ToBoolean(CourseData.Rows[i - 1].Cells[0].EditedFormattedValue))
                {
                    cnt++;
                    index = i;
                }
            }
            if (cnt == 0)
            {
                index = -1;
                MessageBox.Show("请选择要删除的考试！");
            }
            else if (cnt >= 2)
            {
                index = -1;
                MessageBox.Show("请一次选择一个考试删除！");
            }
            return index;
        }

        private Boolean reviseCourse = false;
        private long tempID;
        private void ReviseCourse_Click(object sender, EventArgs e)
        {
            if (!reviseCourse)
            {
                int index = FindValidIndex();
                if (index != -1)
                {
                    Schedule.ScheduleBase.SharedData data = Data[index];
                    tempID = DeleteOneCourse(index);

                    if (tempID != 0)
                    {
                        /*
                        this.NameBox.Text = data.Name;
                        this.WeekcomboBox.Text = "Week" + data.Timestamp.Week.ToString();
                        this.DaycomboBox.Text = data.Timestamp.Day.ToString();
                        this.HourcomboBox.Text = data.Timestamp.Hour.ToString() + ":00";
                        this.DurcomboBox.Text = data.Duration.ToString() + "小时";
                        reviseExam = true;
                        return;
                        */
                        this.NameBox.Text = data.Name;
                        this.HourcomboBox.Text = data.Timestamp.Hour.ToString() + ":00";
                        this.DurcomboBox.Text = data.Duration.ToString() + "小时";

                        if(data.RepetitiveType == RepetitiveType.Single)
                        {
                            this.WeekSelectBox.SelectCheckBox(data.Timestamp.Week - 1);
                            this.DaySelectBox.SelectCheckBox(((int)data.Timestamp.Day) - 1);
                        } 
                        else if(data.RepetitiveType == RepetitiveType.MultipleDays)
                        {
                            this.WeekSelectBox.SetAllValid();
                            int dayindex = 0;
                            for(int i = 0; i < Constants.AllDays.Length; i++)
                            {
                                if (data.ActiveDays[dayindex] == Constants.AllDays[i])
                                {
                                    DaySelectBox.SelectCheckBox(i);
                                    dayindex++;
                                    if(dayindex == data.ActiveDays.Length)
                                    {
                                        break;
                                    }
                                }
                            }
                        } 
                        else
                        {
                            int weekindex = 0;
                            for(int i = 0; i < data.ActiveWeeks.Length; i++)
                            {
                                if (data.ActiveWeeks[weekindex] == Constants.AllWeeks[i])
                                {
                                    WeekSelectBox.SelectCheckBox(i);
                                    weekindex++;
                                    if(weekindex == data.ActiveWeeks.Length)
                                    {
                                        break;
                                    }
                                }
                            }
                            int dayindex = 0;
                            for (int i = 0; i < Constants.AllDays.Length; i++)
                            {
                                if (data.ActiveDays[dayindex] == Constants.AllDays[i])
                                {
                                    DaySelectBox.SelectCheckBox(i);
                                    dayindex++;
                                    if (dayindex == data.ActiveDays.Length)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        reviseCourse = true;
                        return;
                    }
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (AddOneCourse(tempID))
                {
                    reviseCourse = false;
                }
                return;
            }
        }
    }
}
