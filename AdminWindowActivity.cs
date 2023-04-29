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
    public partial class AdminWindowActivity : Form
    {
        public AdminWindowActivity()
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
            ActivityData.Rows.Clear();
            int[] widths = { 30, 150, 150, 150, 100, 100 };
            for (int i = 0; i < widths.Length; i++)
            {
                ActivityData.Columns[i].Width = widths[i];
            }
            Data = Schedule.ScheduleBase.GetSharedByType(ScheduleType.Activity);
            for (int i = 1; i < Data.Count(); i++)
            {
                if (Data[i].RepetitiveType == RepetitiveType.Single)
                {
                    this.ActivityData.Rows.Add(null,
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
                    this.ActivityData.Rows.Add(null,
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
                    this.ActivityData.Rows.Add(null,
                                                 Data[i].Name,
                                                 UI.MainWindow.Continuity(Data[i].ActiveWeeks),
                                                 days,
                                                 Data[i].Timestamp.Hour.ToString() + ":00",
                                                 Data[i].Duration.ToString() + "小时");
                }
            }
        }

        private void AddActivity_Click(object sender, EventArgs e)
        {
            AddOneActivity(null);
        }

        private void DeleteActivity_Click(object sender, EventArgs e)
        {
            DeleteOneActivity();
        }

        private Boolean AddOneActivity(long? ID)
        {
            //error
            StringBuilder ErrorMessage = new StringBuilder("");

            if (NameBox.Text.Equals(""))
            {
                ErrorMessage.AppendLine("请输入活动名！");
            }
            if (WeekSelectBox.valid == 0)
            {
                ErrorMessage.AppendLine("请输入活动周！");
            }
            if (DaySelectBox.valid == 0)
            {
                ErrorMessage.AppendLine("请输入活动日！");
            }
            if (HourcomboBox.Text == "")
            {
                ErrorMessage.AppendLine("请输入活动时间！");
            }
            if (DurcomboBox.Text == "")
            {
                ErrorMessage.AppendLine("请输入活动时长！");
            }
            if (!ErrorMessage.Equals(""))
            {
                MessageBox.Show(ErrorMessage.ToString());
                return false;
            }

            //Time info
            RepetitiveType activityType = new RepetitiveType();
            if (WeekSelectBox.valid == 1 && DaySelectBox.valid == 1)
            {
                activityType = RepetitiveType.Single;
            }
            else if (WeekSelectBox.valid == 16)
            {
                activityType = RepetitiveType.MultipleDays;
            }
            else
            {
                activityType = RepetitiveType.Designated;
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
            StringBuilder activityDetail = new StringBuilder("");
            activityDetail.Append("\n活动周：" + UI.MainWindow.Continuity(activeWeeks));
            activityDetail.Append("\n活动日：");
            for (int i = 0; i < aDcnt; i++)
            {
                activityDetail.Append(activeDays[i].ToString() + "; ");
            }
            activityDetail.Append("\n时间: " + HourcomboBox.Text + "\n时长: " + DurcomboBox.Text +
                                 "\n活动名称：" + NameBox.Text + "\n类型：" + activityType.ToString());


            if (MessageBox.Show(activityDetail.ToString(), "活动信息", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //加活动
                try
                {
                    if (activityType == RepetitiveType.Single)
                    {
                        Schedule.Activity activity = new(RepetitiveType.Single,
                                                            NameBox.Text,
                                                            new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginhour },
                                                            duration,
                                                            null,
                                                            new Map.Location.Building(1,
                                                                                       "default building",
                                                                                       new() { Id = -1, X = 0, Y = 0 }),
                                                            isGroupActivity: true,
                                                            Constants.EmptyIntArray,
                                                            Constants.EmptyDayArray,
                                                            null,
                                                            addOnTimeline: false);
                    }
                    else if (activityType == RepetitiveType.MultipleDays)
                    {
                        Schedule.Activity activity = new(RepetitiveType.MultipleDays,
                                                         NameBox.Text,
                                                         new() { Hour = beginhour },
                                                         duration,
                                                         null,
                                                         new Map.Location.Building(1,
                                                                                   "default building",
                                                                                   new() { Id = 0, X = 0, Y = 0 }),
                                                         isGroupActivity: true,
                                                         Constants.EmptyIntArray,
                                                         activeDays,
                                                         null,
                                                         addOnTimeline: false);
                    }
                    else
                    {
                        Schedule.Activity activity = new(RepetitiveType.Designated,
                                                         NameBox.Text,
                                                         new() { Hour = beginhour },
                                                         duration,
                                                         null,
                                                         new Map.Location.Building(1,
                                                                                   "default building",
                                                                                   new() { Id = 0, X = 0, Y = 0 }),
                                                         isGroupActivity: true,
                                                         activeWeeks,
                                                         activeDays,
                                                         null,
                                                         addOnTimeline: false);
                    }
                    MessageBox.Show("已成功添加该活动");
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
                //不加
                MessageBox.Show("已取消添加该活动");
            }
            return false;
        }

        private long DeleteOneActivity()
        {
            int cnt = 0, index = 0;
            for (int i = 1; i < Data.Count(); i++)
            {
                if (Convert.ToBoolean(ActivityData.Rows[i - 1].Cells[0].EditedFormattedValue))
                {
                    cnt++;
                    index = i;
                }
            }
            if (cnt == 0)
            {
                MessageBox.Show("请选择要删除的活动！");
                return 0;
            }
            else if (cnt >= 2)
            {
                MessageBox.Show("请一次选择一个活动删除！");
                return 0;
            }

            StringBuilder activityDetail = UI.MainWindow.GenerateScheduleDetail(Data[index]);

            if (MessageBox.Show(activityDetail.ToString(), "活动信息", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    long ID = Data[index].Id;
                    ScheduleBase.DeleteShared(ID);
                    MessageBox.Show("已成功删除该活动");
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
                MessageBox.Show("已取消删除该活动");
            }

            return 0;
        }

        private void ReviseActivity_Click(object sender, EventArgs e)
        {

        }
    }
}
