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
            _type = type;
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
            this.daySelectBox.InitializeBox(7, days);

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
            this.weekSelectBox.InitializeBox(16, weeks);
        }

        protected void GenerateFormData()
        {
            GenerateFormData(_type);
        }

        private void GenerateFormData(ScheduleType type)
        {
            scheduleData.Rows.Clear();
            int[] widths = { 30, 150, 150, 150, 100, 100 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleData.Columns[i].Width = widths[i];
            }
            _data = Schedule.ScheduleBase.GetSharedByType(type);
            for (int i = 1; i < _data.Count(); i++)
            {
                if (_data[i].RepetitiveType == RepetitiveType.Single)
                {
                    this.scheduleData.Rows.Add(null,
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
                    this.scheduleData.Rows.Add(null,
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
                    this.scheduleData.Rows.Add(null,
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
                                                Times.Time timestamp,
                                                int duration)
        {
            StringBuilder scheduleDetail = new("");
            if (repetitiveType == RepetitiveType.Single)
            {
                scheduleDetail.Append("\n周次：" + timestamp.Week);
                scheduleDetail.Append("\n天次：" + timestamp.Day);
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                scheduleDetail.Append("\n周次：" + "1-16");
                foreach (Day activeDay in activeDays)
                {
                    scheduleDetail.Append(activeDay.ToString() + "; ");
                }
            }
            else
            {
                scheduleDetail.Append("\n周次：" + GetBriefWeeks(activeWeeks));
                foreach (Day activeDay in activeDays)
                {
                    scheduleDetail.Append(activeDay.ToString() + "; ");
                }
            }
            scheduleDetail.Append("\n天次：");
            foreach (Day activeDay in activeDays)
            {
                scheduleDetail.Append(activeDay.ToString() + "; ");
            }
            scheduleDetail.Append("\n时间: " + timestamp.Hour + "\n时长: " + duration + "\n名称：" + name + "\n类型：" +
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

            char[] arr = nameBox.Text.ToCharArray();
            if (arr.Length == 0)
            {
                errorMessage.AppendLine("请输入日程名！");
            }
            foreach (char c in arr)
            {
                if (c is not ((>= (char)0x4e00 and <= (char)0x9fbb) or
                              (>= '0' and <= '9') or
                              (>= 'A' and <= 'Z') or
                              (>= 'a' and <= 'z') or
                              '_' or
                              '-' or
                              ' '))
                {
                    errorMessage.AppendLine("日程名包含非法字符！");
                    break;
                }
            }
            if (name is "Default" or "default")
            {
                errorMessage.AppendLine("日程名不能为保留值！");
            }
            if (weekSelectBox.valid == 0)
            {
                errorMessage.AppendLine("请输入日程周！");
            }
            if (daySelectBox.valid == 0)
            {
                errorMessage.AppendLine("请输入日程日！");
            }
            if (hourComboBox.Text == "")
            {
                errorMessage.AppendLine("请输入日程时间！");
            }
            if (durationComboBox.Text == "")
            {
                errorMessage.AppendLine("请输入日程时长！");
            }
            if (!errorMessage.Equals(""))
            {
                MessageBox.Show(errorMessage.ToString());
                return false;
            }

            if (weekSelectBox.valid == 1 && daySelectBox.valid == 1)
            {
                repetitiveType = RepetitiveType.Single;
            }
            else if (weekSelectBox.valid == 16)
            {
                repetitiveType = RepetitiveType.MultipleDays;
            }
            else
            {
                repetitiveType = RepetitiveType.Designated;
            }
            activeWeeks = new int[weekSelectBox.valid];
            int activeWeekCount = 0;
            for (int i = 0; i < weekSelectBox.items_cnt; i++)
            {
                if (weekSelectBox.Selects[i])
                {
                    activeWeeks[activeWeekCount] = i + 1;
                    activeWeekCount++;
                }
            }
            activeDays = new Day[daySelectBox.valid];
            int activeDayCount = 0;
            for (int i = 0; i < daySelectBox.items_cnt; i++)
            {
                if (daySelectBox.Selects[i])
                {
                    activeDays[activeDayCount] = Constants.AllDays[i];
                    activeDayCount++;
                }
            }

            duration = durationComboBox.Text[0] - '0';

            if (hourComboBox.Text.Length == 5)
            {
                beginHour = (hourComboBox.Text[0] - '0') * 10 + hourComboBox.Text[1] - '0';
            }
            else
            {
                beginHour = hourComboBox.Text[0] - '0';
            }

            Times.Time timestamp;

            if (repetitiveType == RepetitiveType.Single)
            {
                timestamp = new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginHour };
            }
            else
            {
                timestamp = new() { Hour = beginHour };
            }

            return MessageBox.Show(GetScheduleDatail(name, repetitiveType, activeWeeks, activeDays, timestamp, duration)
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
                if (Convert.ToBoolean(scheduleData.Rows[i - 1].Cells[0].EditedFormattedValue))
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
                                                             selected.Timestamp,
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

            if (!confirm)
            {
                return;
            }
            if (repetitiveType == RepetitiveType.Single)
            {
                _ = new Schedule.Course(RepetitiveType.Single,
                                             nameBox.Text,
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
                                          nameBox.Text,
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
                                          nameBox.Text,
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
            if (!confirm)
            {
                return;
            }
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
            if (!confirm)
            {
                return;
            }
            if (repetitiveType == RepetitiveType.Single)
            {
                _ = new Schedule.Activity(RepetitiveType.Single,
                                             nameBox.Text,
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
                                          nameBox.Text,
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
                                          nameBox.Text,
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