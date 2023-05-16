using System.Diagnostics;
using System.Text;

namespace StudentScheduleManagementSystem.UI
{ 
    public abstract partial class AdminSubwindowBase : Form
    {
        protected List<Schedule.ScheduleBase.SharedData> _data;
        protected ScheduleType _type;
        protected long? _originId = null;


        private AdminSubwindowBase()
            : this(ScheduleType.Idle) { }

        protected AdminSubwindowBase(ScheduleType type)
        {
            InitializeComponent();
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
            this.weekSelectBox.InitializeBox(weeks);
            GenerateFormData(type);
            _type = type;
            this.reviseOK.Hide();
            this.reviseCancel.Hide();
            this.searchByNameBox.ForeColor = Color.Black;
            this.searchByIdBox.ForeColor = Color.Gray;
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

        protected void GenerateFormData(ScheduleType type)
        {
            _data = Schedule.ScheduleBase.GetSharedByType(type);
            GenerateFormData(_data);
        }

        protected void GenerateFormData(List<Schedule.ScheduleBase.SharedData> data)
        {
            scheduleData.Rows.Clear();
            int[] widths = { 30, 150, 130, 120, 130, 60, 60 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleData.Columns[i].Width = widths[i];
            }

            foreach (var sharedData in data)
            {
                if (sharedData.RepetitiveType == RepetitiveType.Single)
                {
                    this.scheduleData.Rows.Add(null,
                                               sharedData.Name,
                                               sharedData.Id,
                                               sharedData.Timestamp.Week.ToString(),
                                               sharedData.Timestamp.Day.ToString()[..3],
                                               sharedData.Timestamp.Hour.ToString() + ":00",
                                               sharedData.Duration.ToString() + "小时");
                }
                else if (sharedData.RepetitiveType == RepetitiveType.MultipleDays)
                {
                    StringBuilder days = new();
                    foreach (Day activeDay in sharedData.ActiveDays)
                    {
                        days.Append(activeDay.ToString()[..3] + ";");
                    }
                    this.scheduleData.Rows.Add(null,
                                               sharedData.Name,
                                               sharedData.Id,
                                               "1-16",
                                               days.ToString(),
                                               sharedData.Timestamp.Hour.ToString() + ":00",
                                               sharedData.Duration.ToString() + "小时");
                }
                else
                {
                    StringBuilder days = new();
                    foreach (Day activeDay in sharedData.ActiveDays)
                    {
                        days.Append(activeDay.ToString()[..3] + ";");
                    }
                    this.scheduleData.Rows.Add(null,
                                               sharedData.Name,
                                               sharedData.Id,
                                               GetBriefWeeks(sharedData.ActiveWeeks).ToString(),
                                               days.ToString(),
                                               sharedData.Timestamp.Hour.ToString() + ":00",
                                               sharedData.Duration.ToString() + "小时");
                }
            }
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
            if (weekSelectBox.ValidCount == 0)
            {
                errorMessage.AppendLine("请输入日程周！");
            }
            if (daySelectBox.ValidCount == 0)
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
            if (weekSelectBox.ValidCount == 1 && daySelectBox.ValidCount == 1)
            {
                repetitiveType = RepetitiveType.Single;
            }
            else if (weekSelectBox.ValidCount == 16)
            {
                repetitiveType = RepetitiveType.MultipleDays;
            }
            else
            {
                repetitiveType = RepetitiveType.Designated;
            }
            activeWeeks = new int[weekSelectBox.ValidCount];
            int activeWeekCount = 0;
            for (int i = 0; i < weekSelectBox.TotalCount; i++)
            {
                if (weekSelectBox.Selects[i])
                {
                    activeWeeks[activeWeekCount] = i + 1;
                    activeWeekCount++;
                }
            }
            activeDays = new Day[daySelectBox.ValidCount];
            int activeDayCount = 0;
            for (int i = 0; i < daySelectBox.TotalCount; i++)
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
            AddOneSchedule(null, true);
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
            StringBuilder scheduleDetail = GetScheduleDetail(selected.Name,
                                                             selected.RepetitiveType,
                                                             selected.ActiveWeeks,
                                                             selected.ActiveDays,
                                                             selected.Timestamp,
                                                             selected.Duration);

            if (MessageBox.Show(scheduleDetail.ToString(), "日程信息", MessageBoxButtons.OKCancel) != DialogResult.OK)
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

        protected abstract bool AddOneSchedule(long? id, bool showMessageBox);

        private void ReviseOK_Click(object sender, EventArgs e)
        {
            bool success = true;
            try
            {
                success = AddOneSchedule(_originId, false);
            }
            catch (ArgumentException) { }
            if (!success)
            {
                return;
            }
            Schedule.ScheduleBase.DeleteShared(_originId!.Value);
            AddOneSchedule(_originId, true);
            MessageBox.Show("已成功修改该日程");
            Log.Information.Log($"成功修改id为{_originId.Value}的共享日程");
            GenerateFormData(_type);

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

        private void ScheduleData_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            int column = e.Column.Index;
            if (column == 3)
            {
                _data.Sort(new ActiveWeekComparer());
                if (this.scheduleData.Columns[3].HeaderCell.SortGlyphDirection==SortOrder.Descending)
                {
                    _data.Reverse(0, _data.Count);
                }
                e.Handled = true;
                GenerateFormData(_data);
            }
            else if (column == 4)
            {
                _data.Sort(new ActiveDayComparer());
                if (this.scheduleData.Columns[4].HeaderCell.SortGlyphDirection == SortOrder.Descending)
                {
                    _data.Reverse(0, _data.Count);
                }
                e.Handled = true;
                GenerateFormData(_data);
            }
        }

        private bool _searchByName = true;

        private void SearchByNameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            _searchByName = true;
            if (e.KeyChar is not ((>= (char)0x4e00 and <= (char)0x9fbb) or
                              (>= '0' and <= '9') or
                              (>= 'A' and <= 'Z') or
                              (>= 'a' and <= 'z') or
                              '_' or
                              '-' or
                              ' '))
            {
                e.Handled = true;
            }
            else
            {
                this.searchByNameBox.ForeColor = Color.Black;
                this.searchByIdBox.ForeColor = Color.Gray;
            }
        }
        private void SearchByIdBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            _searchByName = false;
            if (e.KeyChar is not (>= '0' and <= '9'))
            {
                e.Handled = true;
            }
            else
            {
                this.searchByNameBox.ForeColor = Color.Gray;
                this.searchByIdBox.ForeColor = Color.Black;
            }
        }

        private void SearchOK_Click(object sender, EventArgs e)
        {
            if (_searchByName)
            {
                if (this.searchByNameBox.Text.Equals(""))
                {
                    MessageBox.Show("请输入要搜索的日程名！");
                    return;
                }
                var result = Schedule.ScheduleBase.GetSharedByName(this.searchByNameBox.Text);
                if (result.Count == 0)
                {
                    MessageBox.Show("未搜索到日程！");
                    return;
                }
                GenerateFormData(result);
            }
            else
            {
                if (this.searchByIdBox.Text.Equals(""))
                {
                    MessageBox.Show("请输入要搜索的日程ID！");
                    return;
                }
                var result = Schedule.ScheduleBase.GetSharedById(long.Parse(searchByIdBox.Text));
                if (result == null)
                {
                    MessageBox.Show("未搜索到日程！");
                    return;
                }
                GenerateFormData(new List<Schedule.ScheduleBase.SharedData> { result });
            }
        }

        private void SearchCancel_Click(object sender, EventArgs e)
        {
            GenerateFormData(_type);
        }

    }

    public sealed class CourseSubwindow : AdminSubwindowBase
    {
        public CourseSubwindow()
            : base(ScheduleType.Course) {
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri" };
            this.daySelectBox.InitializeBox(days);
        }


        protected override bool AddOneSchedule(long? id, bool showMessageBox)
        {
            bool confirm = GetScheduleInfo(showMessageBox,
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

            if (beginHour + duration >= Schedule.Course.Latest)
            {
                MessageBox.Show("课程结束时间不得晚于规定时间！");
                return false;
            }

            if (repetitiveType == RepetitiveType.Single)
            {
                _ = new Schedule.Course(RepetitiveType.Single,
                                        name,
                                        new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginHour },
                                        duration,
                                        null,
                                        Constants.DefaultBuilding,
                                        Constants.EmptyIntArray,
                                        Constants.EmptyDayArray,
                                        id,
                                        false);
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                _ = new Schedule.Course(RepetitiveType.MultipleDays,
                                        name,
                                        new() { Hour = beginHour },
                                        duration,
                                        null,
                                        Constants.DefaultBuilding,
                                        Constants.EmptyIntArray,
                                        activeDays,
                                        id,
                                        false);
            }
            else
            {
                _ = new Schedule.Course(RepetitiveType.Designated,
                                        name,
                                        new() { Hour = beginHour },
                                        duration,
                                        null,
                                        Constants.DefaultBuilding,
                                        activeWeeks,
                                        activeDays,
                                        id,
                                        false);
            }
            if (id == null)
            {
                MessageBox.Show("已成功添加该课程");
            }
            GenerateFormData(_type);
            return true;
        }
    }

    public sealed class ExamSubwindow : AdminSubwindowBase
    {
        public ExamSubwindow()
            : base(ScheduleType.Exam) {
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            this.daySelectBox.InitializeBox(days);
        }

        protected override bool AddOneSchedule(long? id, bool showMessageBox)
        {
            bool confirm = GetScheduleInfo(showMessageBox,
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

            if (beginHour + duration >= Schedule.Exam.Latest)
            {
                MessageBox.Show("考试结束时间不得晚于规定时间！");
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
                                  Constants.DefaultBuilding,
                                  id,
                                  false);
            if (id == null)
            {
                MessageBox.Show("已成功添加该课程");
            }
            GenerateFormData(_type);
            return true;
        }
    }

    public sealed class GroupActivitySubwindow : AdminSubwindowBase
    {
        public GroupActivitySubwindow()
            : base(ScheduleType.Activity) {
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            this.daySelectBox.InitializeBox(days);
        }


        protected override bool AddOneSchedule(long? id, bool showMessageBox)
        {
            bool confirm = GetScheduleInfo(showMessageBox,
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

            if (beginHour + duration >= Schedule.Activity.Latest)
            {
                MessageBox.Show("活动结束时间不得晚于规定时间！");
                return false;
            }

            if (repetitiveType == RepetitiveType.Single)
            {
                _ = new Schedule.Activity(RepetitiveType.Single,
                                          name,
                                          new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginHour },
                                          duration,
                                          null,
                                          Constants.DefaultBuilding,
                                          true,
                                          Constants.EmptyIntArray,
                                          Constants.EmptyDayArray,
                                          id,
                                          false);
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                _ = new Schedule.Activity(RepetitiveType.MultipleDays,
                                          nameBox.Text,
                                          new() { Hour = beginHour },
                                          duration,
                                          null,
                                          Constants.DefaultBuilding,
                                          true,
                                          Constants.EmptyIntArray,
                                          activeDays,
                                          id,
                                          false);
            }
            else
            {
                _ = new Schedule.Activity(RepetitiveType.Designated,
                                          nameBox.Text,
                                          new() { Hour = beginHour },
                                          duration,
                                          null,
                                          Constants.DefaultBuilding,
                                          true,
                                          activeWeeks,
                                          activeDays,
                                          id,
                                          false);
            }
            if (id == null)
            {
                MessageBox.Show("已成功添加该课程");
            }
            GenerateFormData(_type);
            return true;
        }
    }
}