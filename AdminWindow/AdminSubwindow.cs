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
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace StudentScheduleManagementSystem.UI
{
    public abstract partial class AdminSubwindowBase : Form
    {
        private List<Schedule.ScheduleBase.SharedData> _data;
        private ScheduleType _type;
        private long? _originId = null;


        private AdminSubwindowBase()
            : this(ScheduleType.Idle) { }

        protected AdminSubwindowBase(ScheduleType type)
        {
            InitializeComponent();
            GenerateMultiSelectBox();
            GenerateFormData(type);
            _type = type;
            this.reviseOK.Hide();
            this.reviseCancel.Hide();
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

        private StringBuilder GetScheduleDetail(string name,
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
                scheduleDetail.Append("\n天次：");
                foreach (Day activeDay in activeDays)
                {
                    scheduleDetail.Append(activeDay.ToString() + "; ");
                }
            }
            else
            {
                scheduleDetail.Append("\n周次：" + GetBriefWeeks(activeWeeks));
                scheduleDetail.Append("\n天次：");
                foreach (Day activeDay in activeDays)
                {
                    scheduleDetail.Append(activeDay.ToString() + "; ");
                }
            }

            scheduleDetail.Append("\n时间: " + timestamp.Hour + "\n时长: " + duration + "\n名称：" + name + "\n类型：" +
                                  repetitiveType.ToString());
            return scheduleDetail;
        }

        protected void AddSchedule_Click(object sender, EventArgs e)
        {
            AddOneSchedule(null);
            Log.Information.Log($"成功添加共享日程");
        }

        protected void DeleteSchedule_Click(object sender, EventArgs e)
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
                MessageBox.Show("请选择要删除的日程！");
                return;
            }
            if (selectedCount >= 2)
            {
                MessageBox.Show("请一次选择一个日程删除！");
                return;
            }

            var selected = _data[index];
            StringBuilder ScheduleDetail = GetScheduleDetail(selected.Name,
                                                             selected.RepetitiveType,
                                                             selected.ActiveWeeks,
                                                             selected.ActiveDays,
                                                             selected.Timestamp,
                                                             selected.Duration);

            if (MessageBox.Show(ScheduleDetail.ToString(), "日程信息", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }
            long id = _data[index].Id;
            Schedule.ScheduleBase.DeleteShared(id);
            MessageBox.Show("已成功删除该日程");
            Log.Information.Log($"成功删除id为{id}的共享日程");
            GenerateFormData(_type);
        }

        protected void ReviseSchedule_Click(object sender, EventArgs e)
        {
            Debug.Assert(!_originId.HasValue);
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
                MessageBox.Show("请选择要修改的日程！");
                return;
            }
            if (selectedCount >= 2)
            {
                MessageBox.Show("请一次选择一个日程修改！");
                return;
            }

            var selected = _data[index];
            this.nameBox.Text = selected.Name;
            RepetitiveType repetitiveType = selected.RepetitiveType;
            if (repetitiveType == RepetitiveType.Single)
            {
                this.weekSelectBox.SelectCheckBox(selected.Timestamp.Week);
                this.daySelectBox.SelectCheckBox((int)selected.Timestamp.Day);
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                for (int i = 0; i < 16; i++)
                {
                    this.weekSelectBox.SelectCheckBox(i);
                }
                foreach (Day activeDay in selected.ActiveDays)
                {
                    this.daySelectBox.SelectCheckBox(activeDay.ToInt());
                }
            }
            else
            {
                foreach (int activeWeek in selected.ActiveWeeks)
                {
                    this.weekSelectBox.SelectCheckBox(activeWeek - 1);
                }
                foreach (Day activeDay in selected.ActiveDays)
                {
                    this.daySelectBox.SelectCheckBox(activeDay.ToInt());
                }
            }

            this.hourComboBox.Text = selected.Timestamp.Hour + ":00";
            this.durationComboBox.Text = selected.Duration + "小时";
            _originId = selected.Id;
            this.reviseScheduleButton.Hide();
            this.addScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.reviseOK.Show();
            this.reviseCancel.Show();
        }

        protected bool GetScheduleInfo(bool showMessageBox,
                                       out string name,
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

            name = nameBox.Text;
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

            return showMessageBox
                       ? MessageBox.Show(GetScheduleDetail(name,
                                                           repetitiveType,
                                                           activeWeeks,
                                                           activeDays,
                                                           timestamp,
                                                           duration)
                                            .ToString(),
                                         "确认日程信息",
                                         MessageBoxButtons.OKCancel) == DialogResult.OK
                       : true;
        }

        protected abstract bool AddOneSchedule(long? id);

        private void ReviseOK_Click(object sender, EventArgs e)
        {
            Schedule.ScheduleBase.DeleteShared(_originId!.Value);
            AddOneSchedule(_originId);
            MessageBox.Show("已成功修改该日程");
            Log.Information.Log($"成功修改id为{_originId.Value}的共享日程");
            GenerateFormData();

            this.ClearInput();
            _originId = null;
            this.reviseOK.Hide();
            this.reviseCancel.Hide();
            this.reviseScheduleButton.Show();
            this.addScheduleButton.Show();
            this.deleteScheduleButton.Show();
        }

        private void ReviseCancel_Click(object sender, EventArgs e)
        {
            this.ClearInput();
            _originId = null;
            this.reviseOK.Hide();
            this.reviseCancel.Hide();
            this.reviseScheduleButton.Show();
            this.addScheduleButton.Show();
            this.deleteScheduleButton.Show();
        }

        private void ClearInput()
        {
            this.nameBox.Text = "";
            this.weekSelectBox.ClearBox();
            this.daySelectBox.ClearBox();
            this.hourComboBox.Text = "";
            this.durationComboBox.Text = "";
        }
    }

    public sealed class CourseSubwindow : AdminSubwindowBase
    {
        public CourseSubwindow()
            : base(ScheduleType.Course) { }


        protected override bool AddOneSchedule(long? id)
        {
            bool confirm = GetScheduleInfo(true,
                                           out string name,
                                           out RepetitiveType repetitiveType,
                                           out int[] activeWeeks,
                                           out Day[] activeDays,
                                           out int beginHour,
                                           out int duration);

            if (!confirm)
            {
                return false;
            }

            if (repetitiveType == RepetitiveType.Single)
            {
                _ = new Schedule.Course(RepetitiveType.Single,
                                        name,
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
                                        name,
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
                                        name,
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
            if(id == null)
            {
                MessageBox.Show("已成功添加该课程");
            }
            GenerateFormData();
            return true;
        }
    }

    public sealed class ExamSubwindow : AdminSubwindowBase
    {
        public ExamSubwindow()
            : base(ScheduleType.Exam) { }

        protected override bool AddOneSchedule(long? id)
        {
            bool confirm = GetScheduleInfo(true,
                                           out string name,
                                           out RepetitiveType repetitiveType,
                                           out int[] activeWeeks,
                                           out Day[] activeDays,
                                           out int beginHour,
                                           out int duration);

            if (!confirm)
            {
                return false;
            }
            if (repetitiveType != RepetitiveType.Single)
            {
                MessageBox.Show("只能选择一个周次和一个天次！");
                return false;
            }

            _ = new Schedule.Exam(name,
                                  new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginHour },
                                  duration,
                                  null,
                                  new Map.Location.Building(1, "default building", new() { Id = -1, X = 0, Y = 0 }));
            if (id == null)
            {
                MessageBox.Show("已成功添加该课程");
            }
            GenerateFormData();
            return true;
        }
    }

    public sealed class GroupActivitySubwindow : AdminSubwindowBase
    {
        public GroupActivitySubwindow()
            : base(ScheduleType.Activity) { }


        protected override bool AddOneSchedule(long? id)
        {
            bool confirm = GetScheduleInfo(true,
                                           out string name,
                                           out RepetitiveType repetitiveType,
                                           out int[] activeWeeks,
                                           out Day[] activeDays,
                                           out int beginHour,
                                           out int duration);

            if (!confirm)
            {
                return false;
            }

            if (repetitiveType == RepetitiveType.Single)
            {
                _ = new Schedule.Activity(RepetitiveType.Single,
                                          name,
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
            if (id == null)
            {
                MessageBox.Show("已成功添加该课程");
            }
            GenerateFormData();
            return true;
        }
    }
}