using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentScheduleManagementSystem.UI
{
    public abstract partial class AdminSubwindowBase : Form
    {
        private List<Schedule.ScheduleBase.SharedData> _data;
        private ScheduleType _type;


        private AdminSubwindowBase() : this(ScheduleType.Idle)
        { }

        protected AdminSubwindowBase(ScheduleType type)
        {
            InitializeComponent();
            GenerateMultiSelectBox();
            GenerateFormData(type);
            _type= type;
        }

        private static StringBuilder GetBriefWeeks(int[] activeWeeks)
        {
            if (activeWeeks.Length == 1)
            {
                return new StringBuilder(activeWeeks[0].ToString());
            }

            int continuity = 0;
            StringBuilder ret = new("");
            for (int i = 1; i < activeWeeks.Length; i++)
            {
                if (activeWeeks[i] == activeWeeks[i - 1] + 1)
                {
                    if (continuity == 0)
                    {
                        if (i != 1)
                        {
                            ret.Append(", ");
                        }
                        ret.Append(activeWeeks[i - 1]);
                    }
                    continuity++;
                }
                else
                {
                    if (continuity == 0)
                    {
                        if (i != 1)
                        {
                            ret.Append(", ");
                        }
                        ret.Append(activeWeeks[i - 1]);
                    }
                    else
                    {
                        ret.Append("-" + activeWeeks[i - 1].ToString());
                    }
                    continuity = 0;
                }
            }

            if (continuity == 0)
            {
                ret.Append(", " + activeWeeks[^1].ToString());
            }
            else
            {
                ret.Append("-" + activeWeeks[^1].ToString());
            }

            return ret;
        }

        private void GenerateMultiSelectBox()
        {
            string[] days = { "Mon", "Tue", "Wen", "Thu", "Fri", "Sat", "Sun" };
            this.DaySelectBox.InitializeBox(7, days);

            string[] weeks =
            {
                "Week1",
                "Week2",
                "Week3",
                "Week4",
                "Week5",
                "Week6",
                "Week7",
                "Week8",
                "Week9",
                "Week10",
                "Week11",
                "Week12",
                "Week13",
                "Week14",
                "Week15",
                "Week16",
            };
            this.WeekSelectBox.InitializeBox(16, weeks);
        }

        protected void GenerateFormData()
        {
            GenerateFormData(_type);
        }

        private void GenerateFormData(ScheduleType type)
        {
            ScheduleData.Rows.Clear();
            int[] widths = { 30, 150, 150, 150, 100, 100 };
            for (int i = 0; i < widths.Length; i++)
            {
                ScheduleData.Columns[i].Width = widths[i];
            }
            _data = Schedule.ScheduleBase.GetSharedByType(type);
            for (int i = 1; i < _data.Count(); i++)
            {
                if (_data[i].RepetitiveType == RepetitiveType.Single)
                {
                    this.ScheduleData.Rows.Add(null,
                                               _data[i].Name,
                                               _data[i].Timestamp.Week.ToString(),
                                               _data[i].Timestamp.Day.ToString(),
                                               _data[i].Timestamp.Hour.ToString() + ":00",
                                               _data[i].Duration.ToString() + "小时");
                }
                else if (_data[i].RepetitiveType == RepetitiveType.MultipleDays)
                {
                    StringBuilder days = new();
                    for (int j = 0; j < _data[i].ActiveDays.Length; j++)
                    {
                        days.Append(_data[i].ActiveDays[j].ToString()[..3] + ";");
                    }
                    this.ScheduleData.Rows.Add(null,
                                               _data[i].Name,
                                               "1-16",
                                               days,
                                               _data[i].Timestamp.Hour.ToString() + ":00",
                                               _data[i].Duration.ToString() + "小时");
                }
                else
                {
                    StringBuilder days = new();
                    for (int j = 0; j < _data[i].ActiveDays.Length; j++)
                    {
                        days.Append(_data[i].ActiveDays[j].ToString()[..3] + ";");
                    }
                    this.ScheduleData.Rows.Add(null,
                                               _data[i].Name,
                                               GetBriefWeeks(_data[i].ActiveWeeks),
                                               days,
                                               _data[i].Timestamp.Hour.ToString() + ":00",
                                               _data[i].Duration.ToString() + "小时");
                }
            }
        }

        private StringBuilder GetScheduleDatail(string name,
                                                RepetitiveType repetitiveType,
                                                int[] activeWeeks,
                                                Day[] activeDays,
                                                int beginHour,
                                                int duration)
        {
            StringBuilder scheduleDetail = new("");
            scheduleDetail.Append("\n周次：" + GetBriefWeeks(activeWeeks));
            scheduleDetail.Append("\n天次：");
            foreach (Day activeDay in activeDays)
            {
                scheduleDetail.Append(activeDay.ToString() + "; ");
            }
            scheduleDetail.Append("\n时间: " + beginHour + "\n时长: " + duration + "\n名称：" + name + "\n类型：" +
                                  repetitiveType.ToString());
            return scheduleDetail;
        }

        protected void AddSchedule_Click(object sender, EventArgs e)
        {
            AddOneSchedule(null);
        }

        protected void DeleteSchedule_Click(object sender, EventArgs e)
        {
            //TODO
            DeleteOneSchedule();
        }

        protected bool GetScheduleInfo(out string name,
                                       out RepetitiveType repetitiveType,
                                       out int[] activeWeeks,
                                       out Day[] activeDays,
                                       out int beginHour,
                                       out int duration)
        {
            StringBuilder errorMessage = new();
            name = "";
            repetitiveType = RepetitiveType.Null;
            activeWeeks = Constants.EmptyIntArray;
            activeDays = Constants.EmptyDayArray;
            beginHour = 0;
            duration = 0;

            if (NameBox.Text.Equals(""))
            {
                errorMessage.AppendLine("请输入日程名！");
            }
            if (WeekSelectBox.valid == 0)
            {
                errorMessage.AppendLine("请输入日程周！");
            }
            if (DaySelectBox.valid == 0)
            {
                errorMessage.AppendLine("请输入日程日！");
            }
            if (HourcomboBox.Text == "")
            {
                errorMessage.AppendLine("请输入日程时间！");
            }
            if (DurcomboBox.Text == "")
            {
                errorMessage.AppendLine("请输入日程时长！");
            }
            if (!errorMessage.Equals(""))
            {
                MessageBox.Show(errorMessage.ToString());
                return false;
            }

            if (WeekSelectBox.valid == 1 && DaySelectBox.valid == 1)
            {
                repetitiveType = RepetitiveType.Single;
            }
            else if (WeekSelectBox.valid == 16)
            {
                repetitiveType = RepetitiveType.MultipleDays;
            }
            else
            {
                repetitiveType = RepetitiveType.Designated;
            }
            activeWeeks = new int[WeekSelectBox.valid];
            int activeWeekCount = 0;
            for (int i = 0; i < WeekSelectBox.items_cnt; i++)
            {
                if (WeekSelectBox.Selects[i])
                {
                    activeWeeks[activeWeekCount] = i + 1;
                    activeWeekCount++;
                }
            }
            activeDays = new Day[DaySelectBox.valid];
            int activeDayCount = 0;
            for (int i = 0; i < DaySelectBox.items_cnt; i++)
            {
                if (DaySelectBox.Selects[i])
                {
                    activeDays[activeDayCount] = Constants.AllDays[i];
                    activeDayCount++;
                }
            }

            duration = DurcomboBox.Text[0] - '0';

            if (HourcomboBox.Text.Length == 5)
            {
                beginHour = (HourcomboBox.Text[0] - '0') * 10 + HourcomboBox.Text[1] - '0';
            }
            else
            {
                beginHour = HourcomboBox.Text[0] - '0';
            }

            return MessageBox.Show(GetScheduleDatail(name, repetitiveType, activeWeeks, activeDays, beginHour, duration)
                                      .ToString(),
                                   "确认日程信息",
                                   MessageBoxButtons.OKCancel) == DialogResult.OK;
        }

        protected abstract void AddOneSchedule(long? id);

        protected long DeleteOneSchedule()
        {
            int selectedCount = 0, index = 0;
            for (int i = 1; i < _data.Count; i++)
            {
                if (Convert.ToBoolean(ScheduleData.Rows[i - 1].Cells[0].EditedFormattedValue))
                {
                    selectedCount++;
                    index = i;
                }
            }
            if (selectedCount == 0)
            {
                MessageBox.Show("请选择要删除的活动！");
                return -1;
            }
            if (selectedCount >= 2)
            {
                MessageBox.Show("请一次选择一个活动删除！");
                return 0;
            }

            var selected = _data[index];
            StringBuilder ScheduleDetail = GetScheduleDatail(selected.Name,
                                                             selected.RepetitiveType,
                                                             selected.ActiveWeeks,
                                                             selected.ActiveDays,
                                                             selected.Timestamp.Hour,
                                                             selected.Duration);

            if (MessageBox.Show(ScheduleDetail.ToString(), "活动信息", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                long id = _data[index].Id;
                Schedule.ScheduleBase.DeleteShared(id);
                MessageBox.Show("已成功删除该活动");
                GenerateFormData(_type);
                return id;
            }
            else
            {
                return 0;
            }
        }

        protected void ReviseSchedule_Click(object sender, EventArgs e) { }
    }

    public sealed class CourseSubwindow : AdminSubwindowBase
    {
        public CourseSubwindow()
            : base(ScheduleType.Course)
        {

        }


        protected override void AddOneSchedule(long? id)
        {
            bool confirm = GetScheduleInfo(out string name,
                                           out RepetitiveType repetitiveType,
                                           out int[] activeWeeks,
                                           out Day[] activeDays,
                                           out int beginHour,
                                           out int duration);
            Debug.Assert(repetitiveType == RepetitiveType.Single);

            if (repetitiveType == RepetitiveType.Single)
            {
                _ = new Schedule.Course(RepetitiveType.Single,
                                             NameBox.Text,
                                             new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginHour },
                                             duration,
                                             null,
                                             new Map.Location.Building(1,
                                                                       "default building",
                                                                       new() { Id = -1, X = 0, Y = 0 }),
                                             Constants.EmptyIntArray,
                                             Constants.EmptyDayArray,
                                             addOnTimeline: false);
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                _ = new Schedule.Course(RepetitiveType.MultipleDays,
                                          NameBox.Text,
                                          new() { Hour = beginHour },
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
                _ = new Schedule.Course(RepetitiveType.Designated,
                                          NameBox.Text,
                                          new() { Hour = beginHour },
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
            GenerateFormData();
        }
    }

    public sealed class ExamSubwindow : AdminSubwindowBase
    {
        public ExamSubwindow()
            : base(ScheduleType.Exam)
        {

        }

        protected override void AddOneSchedule(long? id)
        {
            bool confirm = GetScheduleInfo(out string name,
                                           out RepetitiveType repetitiveType,
                                           out int[] activeWeeks,
                                           out Day[] activeDays,
                                           out int beginHour,
                                           out int duration);
            Debug.Assert(repetitiveType == RepetitiveType.Single);
            _ = new Schedule.Exam(name,
                                  new()
                                  {
                                      Week = activeWeeks[0],
                                      Day = activeDays[0],
                                      Hour = beginHour
                                  },
                                  duration,
                                  null,
                                  new Map.Location.Building(1,
                                                            "default building",
                                                            new() { Id = -1, X = 0, Y = 0 }));
            MessageBox.Show("已成功添加该考试");
            GenerateFormData();
        }
    }

    public sealed class GroupActivitySubwindow : AdminSubwindowBase
    {
        public GroupActivitySubwindow()
            : base(ScheduleType.Activity)
        {

        }


        protected override void AddOneSchedule(long? id)
        {
            bool confirm = GetScheduleInfo(out string name,
                                           out RepetitiveType repetitiveType,
                                           out int[] activeWeeks,
                                           out Day[] activeDays,
                                           out int beginHour,
                                           out int duration);
            Debug.Assert(repetitiveType == RepetitiveType.Single);

            if (repetitiveType == RepetitiveType.Single)
            {
                _ = new Schedule.Activity(RepetitiveType.Single,
                                             NameBox.Text,
                                             new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginHour },
                                             duration,
                                             null,
                                             new Map.Location.Building(1,
                                                                       "default building",
                                                                       new() { Id = -1, X = 0, Y = 0 }),
                                             true,
                                             Constants.EmptyIntArray,
                                             Constants.EmptyDayArray,
                                             addOnTimeline: false);
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                _ = new Schedule.Activity(RepetitiveType.MultipleDays,
                                          NameBox.Text,
                                          new() { Hour = beginHour },
                                          duration,
                                          null,
                                          new Map.Location.Building(1,
                                                                    "default building",
                                                                    new() { Id = 0, X = 0, Y = 0 }),
                                          true,
                                          Constants.EmptyIntArray,
                                          activeDays,
                                          addOnTimeline: false);
            }
            else
            {
                _ = new Schedule.Activity(RepetitiveType.Designated,
                                          NameBox.Text,
                                          new() { Hour = beginHour },
                                          duration,
                                          null,
                                          new Map.Location.Building(1,
                                                                    "default building",
                                                                    new() { Id = 0, X = 0, Y = 0 }),
                                          true,
                                          activeWeeks,
                                          activeDays,
                                          addOnTimeline: false);
            }
            MessageBox.Show("已成功添加该集体活动");
            GenerateFormData();
        }
    }
}