using System.Diagnostics;
using System.Text;

namespace StudentScheduleManagementSystem.UI
{
    public abstract partial class AdminSubwindowBase : Form
    {
        protected List<Schedule.SharedData> _data;
        protected ScheduleType _type;
        protected long? _originId = null;
        protected SubwindowType _subwindowType;

        private AdminSubwindowBase()
            : this(ScheduleType.Idle) { }

        protected AdminSubwindowBase(ScheduleType type)
        {
            InitializeComponent();
            this.weekSelectBox.InitializeBox(Shared.Weeks);
            GenerateFormData(type);
            _type = type;
            this.reviseOK.Hide();
            this.reviseCancel.Hide();
            this.searchByNameBox.ForeColor = Color.Black;
            this.searchByIdBox.ForeColor = Color.Gray;
            this.scheduleDataTable.SortCompare += TableSortCompare;
            this.searchCancel.Click += (sender, e) => GenerateFormData(_type);
        }

        protected void GenerateFormData(ScheduleType type)
        {
            _data = Schedule.Schedule.GetSharedByType(type);
            GenerateFormData(_data);
        }

        protected void GenerateFormData(List<Schedule.SharedData> data)
        {
            scheduleDataTable.Rows.Clear();
            int[] widths = { 30, 155, 130, 135, 110, 60, 60 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleDataTable.Columns[i].Width = widths[i];
            }

            foreach (var sharedData in data)
            {
                if (sharedData.RepetitiveType == RepetitiveType.Single)
                {
                    this.scheduleDataTable.Rows.Add(null,
                                                    sharedData.Name,
                                                    sharedData.ScheduleId,
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
                    this.scheduleDataTable.Rows.Add(null,
                                                    sharedData.Name,
                                                    sharedData.ScheduleId,
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
                    this.scheduleDataTable.Rows.Add(null,
                                                    sharedData.Name,
                                                    sharedData.ScheduleId,
                                                    Shared.GetBriefWeeks(sharedData.ActiveWeeks).ToString(),
                                                    days.ToString(),
                                                    sharedData.Timestamp.Hour.ToString() + ":00",
                                                    sharedData.Duration.ToString() + "小时");
                }
            }
        }

        protected bool GetScheduleInfo(bool showMessageBox,
                                       int earliest,
                                       int latest,
                                       out string name,
                                       out RepetitiveType repetitiveType,
                                       out int[] activeWeeks,
                                       out Day[] activeDays,
                                       out Times.Time beginTime,
                                       out int duration)
        {
            StringBuilder errorMessage = new();
            name = "";
            repetitiveType = RepetitiveType.Null;
            activeWeeks = Constants.EmptyIntArray;
            activeDays = Constants.EmptyDayArray;
            beginTime = new();
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
            if (repetitiveType != RepetitiveType.Single && _type == ScheduleType.Exam)
            {
                errorMessage.AppendLine("考试不能为重复日程！");
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
                    activeDays[activeDayCount] = (Day)i;
                    activeDayCount++;
                }
            }

            duration = durationComboBox?.SelectedIndex + 1 ?? 1;
            int beginHour = hourComboBox.SelectedIndex + earliest;
            if (repetitiveType == RepetitiveType.Single)
            {
                beginTime = new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginHour };
                activeWeeks = Constants.EmptyIntArray;
                activeDays = Constants.EmptyDayArray;
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                beginTime = new() { Hour = beginHour };
                activeWeeks = Constants.EmptyIntArray;
            }
            else
            {
                beginTime = new() { Hour = beginHour };
            }

            if (beginHour + duration >= latest)
            {
                errorMessage.AppendLine("日程结束时间不得晚于最晚时间！");
            }
            if (!errorMessage.Equals(""))
            {
                MessageBox.Show(errorMessage.ToString(), "错误");
                return false;
            }

            return showMessageBox
                       ? MessageBox.Show(Shared.GetScheduleDetail(name,
                                                                  repetitiveType,
                                                                  activeWeeks,
                                                                  activeDays,
                                                                  beginTime,
                                                                  duration,
                                                                  null,
                                                                  null),
                                         "确认日程信息",
                                         MessageBoxButtons.OKCancel) == DialogResult.OK
                       : true;
        }

        protected void AddSchedule_Click(object sender, EventArgs e)
        {
            bool confirm = GetScheduleInfo(true,
                                           Schedule.Activity.Earliest,
                                           Schedule.Activity.Latest,
                                           out string name,
                                           out RepetitiveType repetitiveType,
                                           out int[] activeWeeks,
                                           out Day[] activeDays,
                                           out Times.Time beginTime,
                                           out int duration);
            if (!confirm)
            {
                return;
            }

            AddSchedule(null, name, repetitiveType, activeWeeks, activeDays, beginTime, duration);
            Log.Information.Log($"成功添加共享日程");
            ClearInput();
        }

        protected void DeleteSchedule_Click(object sender, EventArgs e)
        {
            int selectedCount = 0, index = 0;
            for (int i = 0; i < _data.Count; i++)
            {
                if (Convert.ToBoolean(scheduleDataTable.Rows[i].Cells[0].EditedFormattedValue))
                {
                    selectedCount++;
                    index = i;
                }
            }
            if (selectedCount == 0)
            {
                MessageBox.Show("请选择要删除的日程！", "提示");
                return;
            }
            if (selectedCount >= 2)
            {
                MessageBox.Show("请一次选择一个日程删除！", "提示");
                return;
            }

            long id = (long)scheduleDataTable.Rows[index].Cells[2].Value;

            if (MessageBox.Show("周次: " + scheduleDataTable.Rows[index].Cells[3].Value.ToString() + "\n天次: " +
                                scheduleDataTable.Rows[index].Cells[4].Value.ToString() + "\n时间: " +
                                scheduleDataTable.Rows[index].Cells[5].Value.ToString() + "\n时长: " +
                                scheduleDataTable.Rows[index].Cells[6].Value.ToString() + "\n名称: " +
                                scheduleDataTable.Rows[index].Cells[1].Value.ToString(),
                                "日程信息",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Schedule.Schedule.DeleteShared(id);
                MessageBox.Show("已成功删除该日程", "提示");
                Log.Information.Log($"成功删除id为{id}的共享日程");
                GenerateFormData(_type);
            }
        }

        protected void ReviseSchedule_Click(object sender, EventArgs e)
        {
            Debug.Assert(!_originId.HasValue);
            int selectedCount = 0, index = 0;
            for (int i = 0; i < _data.Count; i++)
            {
                if (Convert.ToBoolean(scheduleDataTable.Rows[i].Cells[0].EditedFormattedValue))
                {
                    selectedCount++;
                    index = i;
                }
            }
            if (selectedCount == 0)
            {
                MessageBox.Show("请选择要修改的日程！", "提示");
                return;
            }
            if (selectedCount >= 2)
            {
                MessageBox.Show("请一次选择一个日程修改！", "提示");
                return;
            }

            long id = (long)scheduleDataTable.Rows[index].Cells[2].Value;
            var selected = Schedule.Schedule.GetSharedById(id);

            this.nameBox.Text = selected!.Name;
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
            _originId = selected.ScheduleId;
            this.reviseScheduleButton.Hide();
            this.addScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.reviseOK.Show();
            this.reviseCancel.Show();
        }

        protected abstract bool AddSchedule(long? id,
                                            string name,
                                            RepetitiveType repetitiveType,
                                            int[] activeWeeks,
                                            Day[] activeDays,
                                            Times.Time beginTime,
                                            int duration);

        private void ReviseOK_Click(object sender, EventArgs e)
        {
            int earliest = 0;
            int latest = 0;

            switch (_subwindowType)
            {
                case SubwindowType.Course:
                    earliest = Schedule.Course.Earliest;
                    latest = Schedule.Course.Latest;
                    break;
                case SubwindowType.Exam:
                    earliest = Schedule.Exam.Earliest;
                    latest = Schedule.Exam.Latest;
                    break;
                case SubwindowType.GroupActivity:
                    earliest = Schedule.Activity.Earliest;
                    latest = Schedule.Activity.Latest;
                    break;
            }

            bool confirm = GetScheduleInfo(true,
                                           earliest,
                                           latest,
                                           out string name,
                                           out RepetitiveType repetitiveType,
                                           out int[] activeWeeks,
                                           out Day[] activeDays,
                                           out Times.Time beginTime,
                                           out int duration);
            if (!confirm)
            {
                return;
            }

            Schedule.Schedule.DeleteShared(_originId!.Value);
            AddSchedule(_originId, name, repetitiveType, activeWeeks, activeDays, beginTime, duration);
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
            this.hourComboBox.SelectedIndex = -1;
            this.durationComboBox.SelectedIndex = -1;
        }

        private void TableSortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            int column = e.Column.Index;
            SortOrder order = scheduleDataTable.Columns[column].HeaderCell.SortGlyphDirection;
            Schedule.SharedData sharedData1 =
                Schedule.Schedule.GetSharedById(long.Parse(scheduleDataTable.Rows[e.RowIndex1]
                                                                            .Cells[2]
                                                                            .Value.ToString()!))!;
            Schedule.SharedData sharedData2 =
                Schedule.Schedule.GetSharedById(long.Parse(scheduleDataTable.Rows[e.RowIndex2]
                                                                            .Cells[2]
                                                                            .Value.ToString()!))!;
            //week
            if (column == 3)
            {
                e.SortResult =
                    new ArrayComparer().Compare(sharedData1.RepetitiveType == RepetitiveType.Single
                                                    ? new[] { sharedData1.Timestamp.Week }
                                                    : sharedData1.ActiveWeeks,
                                                sharedData2.RepetitiveType == RepetitiveType.Single
                                                    ? new[] { sharedData2.Timestamp.Week }
                                                    : sharedData2.ActiveWeeks);
            }
            else if (column == 4)
            {
                e.SortResult =
                    new ArrayComparer().Compare(sharedData1.RepetitiveType == RepetitiveType.Single
                                                    ? new[] { sharedData1.Timestamp.Day.ToInt() }
                                                    : Array.ConvertAll(sharedData1.ActiveDays, day => day.ToInt()),
                                                sharedData1.RepetitiveType == RepetitiveType.Single
                                                    ? new[] { sharedData1.Timestamp.Day.ToInt() }
                                                    : Array.ConvertAll(sharedData2.ActiveDays, day => day.ToInt()));
            }
            if (e.SortResult == 0)
            {
                e.SortResult = sharedData1.ScheduleId.CompareTo(sharedData2.ScheduleId);
            }
            if (order == SortOrder.Descending)
            {
                e.SortResult = -e.SortResult;
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
                                  ' ' or
                                  '\b'))
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
            if (e.KeyChar is not ((>= '0' and <= '9') or '\b'))
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
                    MessageBox.Show("请输入要搜索的日程名！", "提示");
                    return;
                }
                var result = Schedule.Schedule.GetSharedByName(this.searchByNameBox.Text);
                if (result.Count == 0)
                {
                    MessageBox.Show("未搜索到日程！", "提示");
                    return;
                }
                GenerateFormData(result);
            }
            else
            {
                if (this.searchByIdBox.Text.Equals(""))
                {
                    MessageBox.Show("请输入要搜索的日程ID！", "提示");
                    return;
                }
                var result = Schedule.Schedule.GetSharedById(long.Parse(searchByIdBox.Text));
                if (result == null)
                {
                    MessageBox.Show("未搜索到日程！", "提示");
                    return;
                }
                GenerateFormData(new List<Schedule.SharedData> { result });
            }
        }
    }

    public sealed class CourseSubwindow : AdminSubwindowBase
    {
        public CourseSubwindow()
            : base(ScheduleType.Course)
        {
            this.daySelectBox.InitializeBox(Shared.Days[..^2]);
            this.hourComboBox.Items.AddRange(Shared.Hours.ToArray<object>()[2..^2]);
            this.label.Text = "课程";
            this._subwindowType = SubwindowType.Course;
        }


        protected override bool AddSchedule(long? id,
                                            string name,
                                            RepetitiveType repetitiveType,
                                            int[] activeWeeks,
                                            Day[] activeDays,
                                            Times.Time beginTime,
                                            int duration)
        {
            _ = new Schedule.Course(repetitiveType,
                                    name,
                                    beginTime,
                                    duration,
                                    null,
                                    Constants.DefaultBuilding,
                                    activeWeeks,
                                    activeDays,
                                    ScheduleOperationType.AdminOperation,
                                    id);
            if (id == null)
            {
                MessageBox.Show("已成功添加该课程", "提示");
            }
            GenerateFormData(_type);
            return true;
        }
    }

    public sealed class ExamSubwindow : AdminSubwindowBase
    {
        public ExamSubwindow()
            : base(ScheduleType.Exam)
        {
            this.daySelectBox.InitializeBox(Shared.Days[..^2]);
            this.hourComboBox.Items.AddRange(Shared.Hours.ToArray<object>()[2..^2]);
            this.label.Text = "考试";
            this._subwindowType = SubwindowType.Exam;
        }

        protected override bool AddSchedule(long? id,
                                            string name,
                                            RepetitiveType repetitiveType,
                                            int[] activeWeeks,
                                            Day[] activeDays,
                                            Times.Time beginTime,
                                            int duration)
        {
            _ = new Schedule.Exam(name,
                                  beginTime,
                                  duration,
                                  null,
                                  Constants.DefaultBuilding,
                                  ScheduleOperationType.AdminOperation,
                                  id);
            if (id == null)
            {
                MessageBox.Show("已成功添加该课程", "提示");
            }
            GenerateFormData(_type);
            return true;
        }
    }

    public sealed class GroupActivitySubwindow : AdminSubwindowBase
    {
        public GroupActivitySubwindow()
            : base(ScheduleType.Activity)
        {
            this.daySelectBox.InitializeBox(Shared.Days);
            this.hourComboBox.Items.AddRange(Shared.Hours.ToArray<object>());
            this.label.Text = "集体活动";
            this._subwindowType = SubwindowType.GroupActivity;
        }


        protected override bool AddSchedule(long? id,
                                            string name,
                                            RepetitiveType repetitiveType,
                                            int[] activeWeeks,
                                            Day[] activeDays,
                                            Times.Time beginTime,
                                            int duration)
        {
            _ = new Schedule.Activity(repetitiveType,
                                      name,
                                      beginTime,
                                      duration,
                                      null,
                                      Constants.DefaultBuilding,
                                      true,
                                      activeWeeks,
                                      activeDays,
                                      ScheduleOperationType.AdminOperation,
                                      id);
            if (id == null)
            {
                MessageBox.Show("已成功添加该活动", "提示");
            }
            GenerateFormData(_type);
            return true;
        }
    }
}