using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace StudentScheduleManagementSystem.UI
{
    public abstract partial class StudentSubwindowBase : Form
    {
        protected List<Schedule.SharedData> _sharedData;
        protected List<Schedule.Schedule> _userData;
        protected ScheduleType _scheduleType;
        protected SubwindowState _subwindowState = SubwindowState.Viewing;
        protected SubwindowType _subwindowType;
        protected readonly HashSet<long> _selectedIds = new();
        protected long? _originId = null;
        protected bool _showAllData = true;

        #region ctor

        protected StudentSubwindowBase()
            : this(ScheduleType.Idle, SubwindowType.Default) { }

        protected StudentSubwindowBase(ScheduleType scheduleType, SubwindowType subwindowType)
        {
            InitializeComponent();
            _scheduleType = scheduleType;
            _subwindowType = subwindowType;
            if (_subwindowType != SubwindowType.PersonalActivity && _subwindowType != SubwindowType.TemporaryAffair)
            {
                foreach (var schedule in Schedule.Schedule.GetScheduleByType(_scheduleType))
                {
                    _selectedIds.Add(schedule.ScheduleId);
                }
                GenerateSharedData(scheduleType);
            }
            foreach (var building in Map.Location.Buildings)
            {
                buildingComboBox.Items.Add(building.Name);
            }
            this.detectCollisionButton.Click += (sender, e) => DetectCollision(true);
            this.deleteScheduleButton.Click += DeleteScheduleButton_Click;
            this.scheduleDataTable.SortCompare += TableSortCompare;
            //occurs when the cell lost focus
            this.scheduleDataTable.CellValueChanged += OnSwitchAlarm;

            this.okButton.Hide();
            this.cancelButton.Hide();
            this.hideDurationPictureBox.Hide();
        }

        #endregion

        #region tool methods

        protected static StringBuilder GetBriefWeeks(int[] activeWeeks, bool measureIsHour)
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
                        if (measureIsHour)
                        {
                            ret.Append(":00");
                        }
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
                    if (measureIsHour)
                    {
                        ret.Append(":00");
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
            if (measureIsHour)
            {
                ret.Append(":00");
            }

            return ret;
        }

        private void TableSortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            int column = e.Column.Index;
            SortOrder order = scheduleDataTable.Columns[column].HeaderCell.SortGlyphDirection;

            if (_showAllData)
            {
                Schedule.SharedData sharedData1 =
                    Schedule.Schedule.GetSharedById(long.Parse(scheduleDataTable.Rows[e.RowIndex1]
                                                                  .Cells[9]
                                                                  .Value.ToString()!))!;
                Schedule.SharedData sharedData2 =
                    Schedule.Schedule.GetSharedById(long.Parse(scheduleDataTable.Rows[e.RowIndex2]
                                                                  .Cells[9]
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
                    e.SortResult = sharedData1.ScheduleId.CompareTo(sharedData1.ScheduleId);
                }
            }
            else
            {
                Schedule.Schedule schedule1 =
                    Schedule.Schedule.GetScheduleById(long.Parse(scheduleDataTable.Rows[e.RowIndex1]
                                                                    .Cells[9]
                                                                    .Value.ToString()!))!;
                Schedule.Schedule schedule2 =
                    Schedule.Schedule.GetScheduleById(long.Parse(scheduleDataTable.Rows[e.RowIndex2]
                                                                    .Cells[9]
                                                                    .Value.ToString()!))!;
                //week
                if (column == 3)
                {
                    e.SortResult =
                        new ArrayComparer().Compare(schedule1.RepetitiveType == RepetitiveType.Single
                                                        ? new[] { schedule1.BeginTime.Week }
                                                        : schedule1.ActiveWeeks,
                                                    schedule2.RepetitiveType == RepetitiveType.Single
                                                        ? new[] { schedule2.BeginTime.Week }
                                                        : schedule2.ActiveWeeks);
                }
                else if (column == 4)
                {
                    e.SortResult =
                        new ArrayComparer().Compare(schedule1.RepetitiveType == RepetitiveType.Single
                                                        ? new[] { schedule1.BeginTime.Day.ToInt() }
                                                        : Array.ConvertAll(schedule1.ActiveDays, day => day.ToInt()),
                                                    schedule1.RepetitiveType == RepetitiveType.Single
                                                        ? new[] { schedule1.BeginTime.Day.ToInt() }
                                                        : Array.ConvertAll(schedule2.ActiveDays, day => day.ToInt()));
                }
                if (e.SortResult == 0)
                {
                    e.SortResult = schedule1.ScheduleId.CompareTo(schedule2.ScheduleId);
                }
            }

            if (order == SortOrder.Descending)
            {
                e.SortResult = -e.SortResult;
            }
        }

        protected long DetectCollision(bool showMessageBox)
        {
            int[] selectedRows = scheduleDataTable.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("能且只能选择一个日程！", "提示");
                return 0;
            }

            long id = long.Parse(scheduleDataTable.Rows[selectedRows[0]].Cells[9].Value.ToString()!);

            var selected = Schedule.Schedule.GetSharedById(id);

            bool willCollide = Schedule.Schedule.DetectCollision(selected!.RepetitiveType,
                                                                 selected.ScheduleType,
                                                                 selected.Timestamp,
                                                                 selected.Duration,
                                                                 selected.ActiveWeeks,
                                                                 selected.ActiveDays,
                                                                 out RepetitiveType collisionRepType,
                                                                 out ScheduleType collisionSchType,
                                                                 out long[] collisionIds);

            if (willCollide)
            {
                StringBuilder types = new();
                if (collisionSchType.HasFlag(ScheduleType.Course))
                {
                    types.Append("课程、");
                }
                if (collisionSchType.HasFlag(ScheduleType.Exam))
                {
                    types.Append("考试、");
                }
                if (collisionSchType.HasFlag(ScheduleType.Activity))
                {
                    types.Append("活动、");
                }
                if (collisionSchType.HasFlag(ScheduleType.TemporaryAffair))
                {
                    types.Append("临时事务、");
                }
                Debug.Assert(types.Length != 0);
                types.Remove(types.Length - 1, 1);
                MessageBox.Show("该日程与您的类型为" + types.ToString() + "的日程冲突", "提示");
                return 0;
            }
            else
            {
                if (showMessageBox)
                {
                    MessageBox.Show("该日程与您的日程没有冲突", "提示");
                }
                return id;
            }
        }

        protected void ClearInformation()
        {
            this.nameBox.Text = "";
            this.weekBox.Text = "";
            this.dayBox.Text = "";
            this.hourBox.Text = "";
            this.durationBox.Text = "";
        }

        #endregion

        #region table content generator

        protected void GenerateSharedData(ScheduleType type)
        {
            _sharedData = Schedule.Schedule.GetSharedByType(type);
            GenerateSharedData(_sharedData.ToArray());
        }

        protected void GenerateSharedData(Schedule.SharedData[] data)
        {
            MergeSort.Sort(ref data, (data1, data2) => data1.ScheduleId.CompareTo(data2.ScheduleId));
            scheduleDataTable.Rows.Clear();
            scheduleDataTable.Columns[1].Visible = false;
            scheduleDataTable.Columns[6].Visible = false;
            scheduleDataTable.Columns[7].Visible = false;
            scheduleDataTable.Columns[8].Visible = false;

            int[] widths = { 30, 55, 130, 120, 130, 60, 60 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleDataTable.Columns[i].Width = widths[i];
            }

            foreach (var sharedData in data)
            {
                if (sharedData.RepetitiveType == RepetitiveType.Single)
                {
                    this.scheduleDataTable.Rows.Add(null,
                                                    false,
                                                    sharedData.Name,
                                                    sharedData.Timestamp.Week.ToString(),
                                                    sharedData.Timestamp.Day.ToString()[..3],
                                                    sharedData.Timestamp.Hour.ToString() + ":00",
                                                    sharedData.Duration.ToString() + "小时",
                                                    null,
                                                    null,
                                                    sharedData.ScheduleId);
                }
                else if (sharedData.RepetitiveType == RepetitiveType.MultipleDays)
                {
                    StringBuilder days = new();
                    foreach (Day activeDay in sharedData.ActiveDays)
                    {
                        days.Append(activeDay.ToString()[..3] + ";");
                    }
                    this.scheduleDataTable.Rows.Add(null,
                                                    false,
                                                    sharedData.Name,
                                                    "1-16",
                                                    days.ToString(),
                                                    sharedData.Timestamp.Hour.ToString() + ":00",
                                                    sharedData.Duration.ToString() + "小时",
                                                    null,
                                                    null,
                                                    sharedData.ScheduleId);
                }
                else
                {
                    StringBuilder days = new();
                    foreach (Day activeDay in sharedData.ActiveDays)
                    {
                        days.Append(activeDay.ToString()[..3] + ";");
                    }
                    this.scheduleDataTable.Rows.Add(null,
                                                    false,
                                                    sharedData.Name,
                                                    GetBriefWeeks(sharedData.ActiveWeeks, false).ToString(),
                                                    days.ToString(),
                                                    sharedData.Timestamp.Hour.ToString() + ":00",
                                                    sharedData.Duration.ToString() + "小时",
                                                    null,
                                                    null,
                                                    sharedData.ScheduleId);
                }
            }

            long[] ids = new long[scheduleDataTable.RowCount];
            for (int i = 0; i < scheduleDataTable.RowCount; i++)
            {
                ids[i] = long.Parse(scheduleDataTable.Rows[i].Cells[9].Value.ToString()!);
            }
            foreach (var selected in _selectedIds)
            {
                int left = 0;
                int right = ids.Length;
                int middle = 0;
                while (left < right)
                {
                    middle = left + ((right - left) / 2);
                    if (ids[middle] > selected)
                    {
                        right = middle;
                    }
                    else if (ids[middle] < selected)
                    {
                        left = middle + 1;
                    }
                    else
                    {
                        var cell = (DataGridViewCheckBoxCell)scheduleDataTable.Rows[middle].Cells[0];
                        var row = (DataGridViewRow)scheduleDataTable.Rows[middle];
                        for (int j = 0; j < row.Cells.Count; j++)
                        {
                            row.Cells[j].Style.BackColor = Color.LightGray;
                        }
                        cell.ReadOnly = true;
                        break;
                    }
                }
            }
        }

        protected void GenerateUserData(ScheduleType type)
        {
            _userData = Schedule.Schedule.GetScheduleByType(type);
            GenerateUserData(_userData.ToArray());
        }

        protected abstract void GenerateUserData(Schedule.Schedule[] data);

        #endregion

        #region envent handler

        private void SwitchData_Click(object sender, EventArgs e)
        {
            if (_showAllData)
            {
                _showAllData = false;
                detectCollisionButton.Visible = false;
                GenerateUserData(_scheduleType);
                switch (_subwindowType)
                {
                    case SubwindowType.Course:
                        label.Text = "个人课程";
                        break;
                    case SubwindowType.Exam:
                        label.Text = "个人考试";
                        break;
                    case SubwindowType.GroupActivity:
                        label.Text = "个人集体活动";
                        break;
                }
            }
            else
            {
                _showAllData = true;
                detectCollisionButton.Visible = true;
                GenerateSharedData(_scheduleType);
                switch (_subwindowType)
                {
                    case SubwindowType.Course:
                        label.Text = "全部课程";
                        break;
                    case SubwindowType.Exam:
                        label.Text = "全部考试";
                        break;
                    case SubwindowType.GroupActivity:
                        label.Text = "全部集体活动";
                        break;
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            ClearInformation();
            this.AddScheduleButton.Show();
            this.deleteScheduleButton.Show();
            this.reviseScheduleButton.Show();
            this.okButton.Hide();
            this.cancelButton.Hide();
            this.buildingRadioButton.Checked = false;
            this.onlineLinkRadioButton.Checked = false;
            this.buildingComboBox.SelectedIndex = -1;
            this.buildingComboBox.Text = "";
            this.onlineLinkBox.Text = "";
            this.descriptionBox.Text = "";
            this._subwindowState = SubwindowState.Viewing;
            _originId = null;
            foreach (DataGridViewRow row in scheduleDataTable.Rows)
            {
                ((DataGridViewCheckBoxCell)row.Cells[0]).ReadOnly = false;
            }
            Times.Timer.Pause = false;
        }

        protected void DeleteScheduleButton_Click(object sender, EventArgs e)
        {
            Times.Timer.Pause = true;
            if (_showAllData)
            {
                MessageBox.Show("请在个人日程页面选择日程删除！", "提示");
                return;
            }

            int[] selectedRows = scheduleDataTable.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("只能选择一个日程！", "提示");
                return;
            }

            long id = long.Parse(scheduleDataTable.Rows[selectedRows[0]].Cells[9].Value.ToString()!);

            if (MessageBox.Show("名称: " + scheduleDataTable.Rows[selectedRows[0]].Cells[1].Value.ToString() + "\n周次: " +
                                scheduleDataTable.Rows[selectedRows[0]].Cells[2].Value.ToString() + "\n天次: " +
                                scheduleDataTable.Rows[selectedRows[0]].Cells[3].Value.ToString() + "\n时间: " +
                                scheduleDataTable.Rows[selectedRows[0]].Cells[4].Value.ToString() + "\n时长: " +
                                scheduleDataTable.Rows[selectedRows[0]].Cells[5].Value.ToString() + "\n地点/链接：" +
                                scheduleDataTable.Rows[selectedRows[0]].Cells[6].Value.ToString(),
                                "确认日程信息",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                var selected = Schedule.Schedule.GetScheduleById(id);
                selected!.DeleteSchedule();
                MessageBox.Show("已成功删除该日程", "提示");
                Log.Information.Log($"成功删除id为{id}的日程");
                _selectedIds.Remove(id);
                GenerateUserData(_scheduleType);
            }
            Times.Timer.Pause = false;
        }


        private void SearchOK_Click(object sender, EventArgs e)
        {
            if (this.searchByNameBox.Text.Equals(""))
            {
                MessageBox.Show("请输入要搜索的日程名！", "提示");
                return;
            }

            if (_showAllData)
            {
                var result = Schedule.Schedule.GetSharedByName(this.searchByNameBox.Text)
                                     .Where(schedule => schedule.ScheduleType == _scheduleType)
                                     .ToArray();
                if (result.Length == 0)
                {
                    MessageBox.Show("未搜索到日程！", "提示");
                    return;
                }
                GenerateSharedData(result);
            }
            else
            {
                var result = Schedule.Schedule.GetSchedulesByName(this.searchByNameBox.Text)
                                     .Where(schedule => schedule.ScheduleType == _scheduleType)
                                     .ToArray();
                if (result.Length == 0)
                {
                    MessageBox.Show("未搜索到日程！", "提示");
                    return;
                }
                GenerateUserData(result);
            }
        }

        private void SearchCancel_Click(object sender, EventArgs e)
        {
            searchByNameBox.Text = "";
            if (_showAllData)
            {
                GenerateSharedData(_scheduleType);
            }
            else
            {
                GenerateUserData(_scheduleType);
            }
        }

        protected void OnSwitchAlarm(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1 || _showAllData)
            {
                return;
            }
            long id = long.Parse(scheduleDataTable.Rows[e.RowIndex].Cells[9].Value.ToString()!);
            Schedule.Schedule selected = Schedule.Schedule.GetScheduleById(id)!;
            Debug.Assert(selected.ScheduleType is not (ScheduleType.Course or ScheduleType.Exam));
            switch (selected.ScheduleType,
                    Convert.ToBoolean(scheduleDataTable.Rows[e.RowIndex].Cells[1].EditedFormattedValue))
            {
                case (ScheduleType.Activity, true):
                    selected.EnableAlarm(Schedule.Activity.Notify,
                                         new Times.Alarm.SpecifiedAlarmParam { scheduleId = id });
                    break;
                case (ScheduleType.Activity, false):
                    selected.DisableAlarm();
                    break;
                case (ScheduleType.TemporaryAffair, true):
                    selected.EnableAlarm(Schedule.TemporaryAffair.Notify,
                                         new Times.Alarm.TemporaryAffairParam
                                         {
                                             timestamp = selected.BeginTime,
                                         });
                    GenerateUserData(_scheduleType);
                    break;
                case (ScheduleType.TemporaryAffair, false):
                    selected.DisableAlarm();
                    GenerateUserData(_scheduleType);
                    break;
            }
        }

        #endregion
    }

    public abstract class SharedScheduleSubwindowBase : StudentSubwindowBase
    {
        protected SharedScheduleSubwindowBase(ScheduleType scheduleType, SubwindowType subwindowType)
            : base(scheduleType, subwindowType)
        {
            this.AddScheduleButton.Click += AddScheduleButton_Click;
            this.okButton.Click += OkButton_Click;
            this.cancelButton.Click += CancelButton_Click;
            this.reviseScheduleButton.Click += ReviseButton_Click;
        }

        #region table content generator

        protected override void GenerateUserData(Schedule.Schedule[] data)
        {
            scheduleDataTable.Rows.Clear();
            int[] widths = { 30, 55, 130, 120, 130, 60, 60, 150, 150 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleDataTable.Columns[i].Width = widths[i];
            }
            scheduleDataTable.Columns[1].Visible = _subwindowType == SubwindowType.GroupActivity;
            scheduleDataTable.Columns[6].Visible = true;
            scheduleDataTable.Columns[7].Visible = true;
            scheduleDataTable.Columns[8].Visible = true;

            foreach (var schedule in data)
            {
                Debug.Assert(schedule is Schedule.Course or Schedule.Exam or Schedule.Activity);
                if (schedule is Schedule.Activity { IsGroupActivity : false })
                {
                    continue;
                }

                StringBuilder days = new("");
                foreach (Day activeDay in schedule.ActiveDays)
                {
                    days.Append(activeDay.ToString()[..3] + ";");
                }
                if (days.Length == 0)
                {
                    days = new(schedule.BeginTime.Day.ToString()[..3] + ";");
                }

                string location = "";
                if (schedule is Schedule.Course course)
                {
                    location = course.OfflineLocation?.Name ?? course.OnlineLink!;
                }
                else if (schedule is Schedule.Exam exam)
                {
                    location = exam.OfflineLocation.Name;
                }
                else if (schedule is Schedule.Activity activity)
                {
                    location = activity.OfflineLocation?.Name ?? activity.OnlineLink!;
                }

                string activeWeeks;
                if (schedule.RepetitiveType == RepetitiveType.Single)
                {
                    activeWeeks = schedule.BeginTime.Week.ToString();
                }
                else if (schedule.RepetitiveType == RepetitiveType.MultipleDays)
                {
                    activeWeeks = "1-16";
                }
                else
                {
                    activeWeeks = GetBriefWeeks(schedule.ActiveWeeks, false).ToString();
                }

                scheduleDataTable.Rows.Add(null,
                                           schedule.AlarmEnabled,
                                           schedule.Name,
                                           activeWeeks,
                                           days,
                                           schedule.BeginTime.Hour.ToString() + ":00",
                                           schedule.Duration.ToString() + "小时",
                                           schedule.Description ?? "",
                                           location,
                                           schedule.ScheduleId);
            }
        }

        #endregion

        #region tool methods

        private bool GetScheduleInfo(bool showMessageBox, DataGridViewRow selected)
        {
            StringBuilder errorMessage = new();

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
            string location = "";
            if (buildingRadioButton.Checked && buildingRadioButton.Visible)
            {
                if (buildingComboBox.Text == "")
                {
                    errorMessage.AppendLine("请输入日程地址！");
                }
                else
                {
                    location = $"\n地址: {buildingComboBox.Text}";
                }
            }
            else if (onlineLinkRadioButton.Checked && onlineLinkRadioButton.Visible)
            {
                if (onlineLinkBox.Text == "")
                {
                    errorMessage.AppendLine("请输入日程链接！");
                }
                else
                {
                    location = $"\n链接: {onlineLinkBox.Text}";
                }
            }
            else if (!buildingRadioButton.Visible && !onlineLinkRadioButton.Visible) { }
            else
            {
                errorMessage.AppendLine("未选择线下地址或线上链接！");
            }
            if (!errorMessage.Equals(""))
            {
                MessageBox.Show(errorMessage.ToString(), "错误");
                return false;
            }

            return showMessageBox
                       ? MessageBox.Show("名称: " + selected.Cells[2].Value.ToString() + "\n周次: " +
                                         selected.Cells[3].Value.ToString() + "\n天次: " +
                                         selected.Cells[4].Value.ToString() + "\n时间: " +
                                         selected.Cells[5].Value.ToString() + "\n时长: " +
                                         selected.Cells[6].Value.ToString() + location,
                                         "确认日程信息",
                                         MessageBoxButtons.OKCancel) == DialogResult.OK
                       : true;
        }

        protected void AddSchedule(long id)
        {
            var selected = Schedule.Schedule.GetSharedById(id);
            Type scheduleType = _scheduleType switch
            {
                ScheduleType.Course => typeof(Schedule.Course), ScheduleType.Exam => typeof(Schedule.Exam),
                ScheduleType.Activity => typeof(Schedule.Activity),
                _ => throw new ArgumentException(null, nameof(_scheduleType)),
            };
            var ctors = scheduleType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            List<object?> args = new()
            {
                selected!.RepetitiveType,
                selected.Name,
                selected.Timestamp,
                selected.Duration,
                descriptionBox.Text == "" ? null : descriptionBox.Text,
                null,
                true,
                selected.ActiveWeeks,
                selected.ActiveDays,
                ScheduleOperationType.UserOpration,
                id
            };
            Schedule.Schedule obj;

            //offline
            if (buildingRadioButton.Checked)
            {
                var ctor = ctors.FirstOrDefault(ctor => ctor.GetParameters()
                                                            .FirstOrDefault(info => info.ParameterType ==
                                                                                typeof(Map.Location.Building)) != null);
                args[5] = Map.Location.GetBuildingsByName(buildingComboBox.Text)[0];

                if (_scheduleType == ScheduleType.Course)
                {
                    args.RemoveAt(6);
                }
                else if (_scheduleType == ScheduleType.Exam)
                {
                    args.RemoveAt(0);
                    args.RemoveRange(5, 3);
                }

                if (ctor == null)
                {
                    throw new InvalidOperationException("specific ctor could not be found");
                }
                obj = (Schedule.Schedule)ctor.Invoke(args.ToArray());
            }
            //online
            else
            {
                var ctor = ctors.FirstOrDefault(ctor => ctor.GetParameters()
                                                            .FirstOrDefault(info => info.ParameterType ==
                                                                                typeof(Map.Location.Building)) == null);
                args[5] = onlineLinkBox.Text == "" ? null : onlineLinkBox.Text;
                if (_scheduleType == ScheduleType.Course)
                {
                    args.RemoveAt(6);
                }
                else if (_scheduleType == ScheduleType.Exam)
                {
                    args.RemoveAt(0);
                    args.RemoveRange(5, 3);
                }

                if (ctor == null)
                {
                    throw new InvalidOperationException("specific ctor could not be found");
                }
                obj = (Schedule.Schedule)ctor.Invoke(args.ToArray());
            }
            if (obj.ScheduleType == ScheduleType.Course)
            {
                obj.EnableAlarm(Schedule.Course.Notify,
                                new Times.Alarm.SpecifiedAlarmParam { scheduleId = obj.ScheduleId });
                Log.Information.Log("已自动配置课程闹钟");
            }
            else if (obj.ScheduleType == ScheduleType.Exam)
            {
                obj.EnableAlarm(Schedule.Exam.Notify,
                                new Times.Alarm.SpecifiedAlarmParam { scheduleId = obj.ScheduleId });
                Log.Information.Log("已自动配置考试闹钟");
            }
            _selectedIds.Add(id);
            Debug.Assert(_showAllData);
            GenerateSharedData(_scheduleType);
        }

        #endregion

        #region event handler

        protected void AddScheduleButton_Click(object sender, EventArgs e)
        {
            Times.Timer.Pause = true;

            if (!_showAllData)
            {
                MessageBox.Show("请在全部日程的页面选择日程添加！", "提示");
                return;
            }

            long id = DetectCollision(false);
            if (id == 0)
            {
                return;
            }

            _subwindowState = SubwindowState.AddUserSchedule;

            this.AddScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.reviseScheduleButton.Hide();
            this.okButton.Show();
            this.cancelButton.Show();

            var selected = Schedule.Schedule.GetSharedById(id);
            ClearInformation();
            this.nameBox.Text = selected!.Name;
            if (selected.RepetitiveType == RepetitiveType.Single)
            {
                this.weekBox.Text = selected.Timestamp.Week.ToString();
                this.dayBox.Text = selected.Timestamp.Day.ToString()[..3];
            }
            else if (selected.RepetitiveType == RepetitiveType.MultipleDays)
            {
                this.weekBox.Text = "1-16";
                foreach (Day activeDay in selected.ActiveDays)
                {
                    this.dayBox.Text += activeDay.ToString()[..3] + "; ";
                }
            }
            else
            {
                this.weekBox.Text = GetBriefWeeks(selected.ActiveWeeks, false).ToString();
                foreach (Day activeDay in selected.ActiveDays)
                {
                    this.dayBox.Text += activeDay.ToString()[..3] + "; ";
                }
            }
            this.hourBox.Text = selected.Timestamp.Hour.ToString() + ":00";
            this.durationBox.Text = selected.Duration.ToString() + "小时";

            _subwindowState = SubwindowState.AddUserSchedule;
            foreach (DataGridViewRow row in scheduleDataTable.Rows)
            {
                ((DataGridViewCheckBoxCell)row.Cells[0]).ReadOnly = true;
            }
        }

        protected void ReviseButton_Click(object sender, EventArgs e)
        {
            Times.Timer.Pause = true;

            if (_showAllData)
            {
                MessageBox.Show("请在个人日程页面选择日程修改！", "提示");
                return;
            }

            int[] selectedRows = scheduleDataTable.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("只能选择一个日程！", "提示");
                return;
            }

            this.AddScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.reviseScheduleButton.Hide();
            this.okButton.Show();
            this.cancelButton.Show();

            long id = long.Parse(scheduleDataTable.Rows[selectedRows[0]].Cells[9].Value.ToString()!);
            _originId = id;
            var selected = Schedule.Schedule.GetScheduleById(id);
            ClearInformation();
            this.nameBox.Text = selected!.Name;
            if (selected.ScheduleType == ScheduleType.Exam || selected.RepetitiveType == RepetitiveType.Single)
            {
                this.weekBox.Text = selected.ActiveWeeks[0].ToString();
                this.dayBox.Text = selected.ActiveDays[0].ToString()[..3];
            }
            else if (selected.RepetitiveType == RepetitiveType.MultipleDays)
            {
                this.weekBox.Text = "1-16";
                foreach (Day activeDay in selected.ActiveDays)
                {
                    this.dayBox.Text += activeDay.ToString()[..3] + "; ";
                }
            }
            else
            {
                this.weekBox.Text = GetBriefWeeks(selected.ActiveWeeks, false).ToString();
                foreach (Day activeDay in selected.ActiveDays)
                {
                    this.dayBox.Text += activeDay.ToString()[..3] + "; ";
                }
            }
            this.hourBox.Text = selected.BeginTime.ToString() + ":00";
            this.durationBox.Text = selected.Duration.ToString() + "小时";
            this.descriptionBox.Text = selected.Description;

            if (_subwindowType == SubwindowType.Course)
            {
                if (((Schedule.Course)selected).IsOnline)
                {
                    onlineLinkRadioButton.Checked = true;
                    onlineLinkBox.Text = ((Schedule.Course)selected).OnlineLink;
                }
                else
                {
                    buildingRadioButton.Checked = true;
                    buildingComboBox.Text = ((Schedule.Course)selected).OfflineLocation!.Value.Name;
                }
            }
            else if (_subwindowType == SubwindowType.GroupActivity)
            {
                if (((Schedule.Activity)selected).IsOnline)
                {
                    onlineLinkRadioButton.Checked = true;
                    onlineLinkBox.Text = ((Schedule.Activity)selected).OnlineLink;
                }
                else
                {
                    buildingRadioButton.Checked = true;
                    buildingComboBox.Text = ((Schedule.Activity)selected).OfflineLocation!.Value.Name;
                }
            }
            else
            {
                buildingRadioButton.Checked = true;
                buildingComboBox.Text = ((Schedule.Exam)selected).OfflineLocation.Name;
            }

            _subwindowState = SubwindowState.ReviseUserSchedule;
            foreach (DataGridViewRow row in scheduleDataTable.Rows)
            {
                ((DataGridViewCheckBoxCell)row.Cells[0]).ReadOnly = true;
            }
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            Debug.Assert((_subwindowState, _originId) is (SubwindowState.ReviseUserSchedule, > (long)1e9) or
                                                         (SubwindowState.AddUserSchedule, null));

            int[] selectedRows = scheduleDataTable.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("只能选择一个日程！", "提示");
                return;
            }

            long id = long.Parse(scheduleDataTable.Rows[selectedRows[0]].Cells[9].Value.ToString()!);
            bool confirm = GetScheduleInfo(true, scheduleDataTable.Rows[selectedRows[0]]);
            if (!confirm)
            {
                return;
            }
            if (_subwindowState == SubwindowState.ReviseUserSchedule)
            {
                var selected = Schedule.Schedule.GetScheduleById(id);
                selected!.DeleteSchedule();
            }
            AddSchedule(_originId ?? id);

            this.AddScheduleButton.Show();
            this.deleteScheduleButton.Show();
            this.reviseScheduleButton.Show();
            this.okButton.Hide();
            this.cancelButton.Hide();
            this.buildingRadioButton.Checked = false;
            this.onlineLinkRadioButton.Checked = false;
            this.buildingComboBox.SelectedIndex = -1;
            this.buildingComboBox.Text = "";
            this.onlineLinkBox.Text = "";
            this.descriptionBox.Text = "";
            if (_subwindowState == SubwindowState.ReviseUserSchedule)
            {
                Log.Information.Log($"成功修改id为{_originId!.Value}的共享日程");
                GenerateUserData(_scheduleType);
            }
            _subwindowState = SubwindowState.Viewing;
            ClearInformation();
            foreach (DataGridViewRow row in scheduleDataTable.Rows)
            {
                ((DataGridViewCheckBoxCell)row.Cells[0]).ReadOnly = false;
            }
            Times.Timer.Pause = false;
        }

        #endregion
    }

    public abstract class PersonalScheduleSubwinowBase : StudentSubwindowBase
    {
        protected PersonalScheduleSubwinowBase(ScheduleType scheduleType, SubwindowType subwindowType)
            : base(scheduleType, subwindowType)
        {
            this.reviseScheduleButton.Click += ReviseScheduleButton_Click;
            this.okButton.Click += OkButton_Click;
            this.getAvailableTime.Click += GetAvailableTime_Click;
            this.nameBox.ReadOnly = false;
            this.getAvailableTime.Visible = true;
        }

        #region table content generator

        protected override void GenerateUserData(Schedule.Schedule[] data)
        {
            scheduleDataTable.Rows.Clear();
            int[] widths = { 30, 55, 130, 120, 130, 60, 60, 150, 150 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleDataTable.Columns[i].Width = widths[i];
            }
            scheduleDataTable.Columns[1].Visible = true;
            scheduleDataTable.Columns[6].Visible = true;
            scheduleDataTable.Columns[7].Visible = true;

            foreach (var schedule in data)
            {
                Debug.Assert(schedule is Schedule.Course or Schedule.Exam or Schedule.Activity);
                if (schedule is Schedule.Activity { IsGroupActivity : true })
                {
                    continue;
                }

                StringBuilder days = new("");
                foreach (Day activeDay in schedule.ActiveDays)
                {
                    days.Append(activeDay.ToString()[..3] + ";");
                }
                if (days.Length == 0)
                {
                    days = new(schedule.BeginTime.Day.ToString()[..3] + ";");
                }

                string location = "";
                if (schedule is Schedule.Activity activity)
                {
                    location = activity.OfflineLocation?.Name ?? activity.OnlineLink!;
                }
                else if (schedule is Schedule.TemporaryAffair temp)
                {
                    location = temp.OfflineLocation.Name;
                }

                string activeWeeks;
                if (schedule.RepetitiveType == RepetitiveType.Single)
                {
                    activeWeeks = schedule.BeginTime.Week.ToString();
                }
                else if (schedule.RepetitiveType == RepetitiveType.MultipleDays)
                {
                    activeWeeks = "1-16";
                }
                else
                {
                    activeWeeks = GetBriefWeeks(schedule.ActiveWeeks, false).ToString();
                }

                scheduleDataTable.Rows.Add(null,
                                           schedule.AlarmEnabled,
                                           schedule.Name,
                                           activeWeeks,
                                           days,
                                           schedule.BeginTime.Hour.ToString() + ":00",
                                           schedule.Duration.ToString() + "小时",
                                           schedule.Description ?? "",
                                           location,
                                           schedule.ScheduleId);
            }
        }

        #endregion

        #region tool methods

        public bool GetScheduleInfo(bool showMessageBox,
                                    int earliest,
                                    int latest,
                                    MultiSelectBox? weekSelectBox,
                                    MultiSelectBox? daySelectBox,
                                    ComboBox? weekComboBox,
                                    ComboBox? dayComboBox,
                                    ComboBox hourComboBox,
                                    ComboBox? durationComboBox,
                                     out string name,
                                     out RepetitiveType repetitiveType,
                                     out int[] activeWeeks,
                                     out Day[] activeDays,
                                     out Times.Time beginTime,
                                     out int duration,
                                     out string offlineLocationName,
                                     out string onlineLink)
        {
            if (!(weekSelectBox == null ^ weekComboBox == null) || !(daySelectBox != null ^ dayComboBox != null))
            {
                throw new ArgumentNullException();
            }

            StringBuilder errorMessage = new();
            name = "";
            repetitiveType = RepetitiveType.Null;
            activeWeeks = Constants.EmptyIntArray;
            activeDays = Constants.EmptyDayArray;
            beginTime = new();
            duration = 0;
            offlineLocationName = buildingComboBox.Text;
            onlineLink = onlineLinkBox.Text;

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
            if (weekSelectBox is {ValidCount : 0} || weekComboBox is {SelectedIndex : -1})
            {
                errorMessage.AppendLine("请输入日程周！");
            }
            if (daySelectBox is { ValidCount: 0 } || dayComboBox is {SelectedIndex : -1})
            {
                errorMessage.AppendLine("请输入日程日！");
            }
            if (hourComboBox.Text == "")
            {
                errorMessage.AppendLine("请输入日程时间！");
            }
            if (durationComboBox is { Text : ""})
            {
                errorMessage.AppendLine("请输入日程时长！");
            }

            if (buildingRadioButton.Checked && buildingRadioButton.Visible)
            {
                if (buildingComboBox.Text == "")
                {
                    errorMessage.AppendLine("请输入日程地址！");
                }
            }
            else if (onlineLinkRadioButton.Checked && onlineLinkRadioButton.Visible)
            {
                if (onlineLinkBox.Text == "")
                {
                    errorMessage.AppendLine("请输入日程链接！");
                }
            }
            else if (!buildingRadioButton.Visible && !onlineLinkRadioButton.Visible) { }
            else
            {
                errorMessage.AppendLine("未选择线下地址或线上链接！");
            }

            name = nameBox.Text;
            if ((weekSelectBox is { ValidCount : 1 } && daySelectBox is { ValidCount : 1 }) || (weekComboBox, dayComboBox) is not (null, null))
            {
                repetitiveType = RepetitiveType.Single;
            }
            else if (weekSelectBox!.ValidCount == 16)
            {
                repetitiveType = RepetitiveType.MultipleDays;
            }
            else
            {
                repetitiveType = RepetitiveType.Designated;
            }

            activeWeeks = new int[weekSelectBox?.ValidCount ?? 1];
            if (weekSelectBox != null)
            {
                int activeWeekCount = 0;
                for (int i = 0; i < weekSelectBox.TotalCount; i++)
                {
                    if (weekSelectBox.Selects[i])
                    {
                        activeWeeks[activeWeekCount] = i + 1;
                        activeWeekCount++;
                    }
                }
            }
            else
            {
                activeWeeks[0] = weekComboBox!.SelectedIndex + 1;
            }

            activeDays = new Day[daySelectBox?.ValidCount ?? 1];
            if (daySelectBox != null)
            {
                int activeDayCount = 0;
                for (int i = 0; i < daySelectBox.TotalCount; i++)
                {
                    if (daySelectBox.Selects[i])
                    {
                        activeDays[activeDayCount] = (Day)i;
                        activeDayCount++;
                    }
                }
            }
            else
            {
                activeDays[0] = (Day)dayComboBox!.SelectedIndex;
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
                                                                  buildingComboBox.Text,
                                                                  onlineLinkBox.Text),
                                         "确认日程信息",
                                         MessageBoxButtons.OKCancel) == DialogResult.OK
                       : true;
        }

        protected abstract void AddSchedule();

        protected abstract bool GetTargetTimeDetail(out int[] activeWeeks, out Day[] activeDays, out int duration);

        #endregion

        #region event handler

        protected void GetAvailableTime_Click(object sender, EventArgs e)
        {
            if (!GetTargetTimeDetail(out int[] activeWeeks, out Day[] activeDays, out int duration))
            {
                return;
            }

            List<int> availableTime = new();
            if (_subwindowType == SubwindowType.TemporaryAffair)
            {
                int startTime = (activeWeeks[0] - 1) * 7 * 24 + (int)activeDays[0] * 24;
                for (int i = startTime + Schedule.TemporaryAffair.Earliest;
                     i < startTime + Schedule.TemporaryAffair.Latest; i++)
                {
                    ScheduleType scheduleType = Schedule.Schedule.GetRecordAt(i).ScheduleType;
                    if (scheduleType is ScheduleType.TemporaryAffair or ScheduleType.Idle)
                    {
                        availableTime.Add(i - startTime);
                    }
                }
            }
            else
            {
                bool[] availableHourArray = new bool[Schedule.Activity.Latest - Schedule.Activity.Earliest];
                bool[] availableTimeArray = new bool[Schedule.Activity.Latest - Schedule.Activity.Earliest];
                Array.Fill(availableHourArray, true);
                Array.Fill(availableTimeArray, true);
                foreach (var activeWeek in activeWeeks)
                {
                    foreach (var activeDay in activeDays)
                    {
                        int startTime = (activeWeek - 1) * 7 * 24 + activeDay.ToInt() * 24;
                        for (int i = 0; i < availableHourArray.Length; i++)
                        {
                            availableHourArray[i] = availableHourArray[i] &&
                                                    Schedule.Schedule
                                                            .GetRecordAt(startTime + Schedule.Activity.Earliest + i)
                                                            .ScheduleType == ScheduleType.Idle;
                        }
                    }
                }
                for (int i = 0; i < availableTimeArray.Length - duration + 1; i++)
                {
                    for (int j = 0; j < duration; j++)
                    {
                        availableTimeArray[i] = availableTimeArray[i] && availableHourArray[i + j];
                    }
                    if (availableTimeArray[i])
                    {
                        availableTime.Add(i + Schedule.Activity.Earliest);
                    }
                }
            }
            if (availableTime.Count > 0)
            {
                MessageBox.Show("日程可选起始时间：" + GetBriefWeeks(availableTime.ToArray(), true), "提示");
            }
            else
            {
                MessageBox.Show("所选时段没有空闲时间！", "提示");
            }
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(_subwindowState is SubwindowState.ReviseUserSchedule && _originId != null);

            var selected = Schedule.Schedule.GetScheduleById(_originId.Value);
            selected!.DeleteSchedule();
            AddSchedule();

            this.AddScheduleButton.Show();
            this.deleteScheduleButton.Show();
            this.reviseScheduleButton.Show();
            this.okButton.Hide();
            this.cancelButton.Hide();
            this.buildingRadioButton.Checked = false;
            this.onlineLinkRadioButton.Checked = false;
            this.buildingComboBox.SelectedIndex = -1;
            this.buildingComboBox.Text = "";
            this.onlineLinkBox.Text = "";
            this.descriptionBox.Text = "";
            this._subwindowState = SubwindowState.Viewing;
            ClearInformation();

            Times.Timer.Pause = false;
        }

        protected abstract void ReviseScheduleButton_Click(object sender, EventArgs e);

        #endregion
    }

    public sealed class StudentCourseSubwindow : SharedScheduleSubwindowBase
    {
        public StudentCourseSubwindow()
            : base(ScheduleType.Course, SubwindowType.Course)
        {
            label.Text = "全部课程";
        }
    }

    public sealed class StudentExamSubwindow : SharedScheduleSubwindowBase
    {
        public StudentExamSubwindow()
            : base(ScheduleType.Exam, SubwindowType.Exam)
        {
            onlineLinkRadioButton.Enabled = false;
            onlineLinkBox.Enabled = false;
            label.Text = "全部考试";
        }
    }

    public sealed class StudentGroupActivitySubwindow : SharedScheduleSubwindowBase
    {
        public StudentGroupActivitySubwindow()
            : base(ScheduleType.Activity, SubwindowType.GroupActivity)
        {
            label.Text = "全部集体活动";
        }
    }

    public sealed class StudentPersonalActivitySubwindow : PersonalScheduleSubwinowBase
    {
        private MultiSelectBox weekSelectBox = new();
        private MultiSelectBox daySelectBox = new();
        private ComboBox hourComboBox = new();
        private ComboBox durationComboBox = new();

        public StudentPersonalActivitySubwindow()
            : base(ScheduleType.Activity, SubwindowType.PersonalActivity)
        {
            GenerateUserData(_scheduleType);
            this.detectCollisionButton.Hide();
            GenerateSubwindow();
            _showAllData = false;
            _subwindowState = SubwindowState.Viewing;
            this.AddScheduleButton.Click += (sender, e) => AddSchedule();
            label.Text = "个人活动";
        }

        #region UI generator

        private void GenerateSubwindow()
        {
            weekSelectBox.BackColor = Color.White;
            weekSelectBox.BorderStyle = BorderStyle.FixedSingle;
            weekSelectBox.ShowComboBox = false;
            weekSelectBox.Size = new Size(300, 30);
            weekSelectBox.Location = weekBox.Location;
            weekSelectBox.Name = "weekSelectBox";
            Controls.Add(weekSelectBox);
            this.weekSelectBox.InitializeBox(Shared.Weeks);

            daySelectBox.BackColor = Color.White;
            daySelectBox.BorderStyle = BorderStyle.FixedSingle;
            daySelectBox.ShowComboBox = false;
            daySelectBox.Size = new Size(300, 30);
            daySelectBox.Location = dayBox.Location;
            daySelectBox.Name = "daySelectBox";
            Controls.Add(daySelectBox);
            this.daySelectBox.InitializeBox(Shared.Days);

            hourComboBox.Size = hourBox.Size;
            hourComboBox.BackColor = Color.White;
            hourComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            hourComboBox.FlatStyle = FlatStyle.Flat;
            hourComboBox.ForeColor = SystemColors.WindowText;
            hourComboBox.FormattingEnabled = true;
            hourComboBox.Items.AddRange(Shared.Hours.ToArray<object>());
            hourComboBox.Location = hourBox.Location;
            hourComboBox.Name = "hourComboBox";
            hourComboBox.Size = hourBox.Size;
            Controls.Add(hourComboBox);

            durationComboBox.BackColor = Color.White;
            durationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            durationComboBox.DropDownWidth = 130;
            durationComboBox.FlatStyle = FlatStyle.Flat;
            durationComboBox.FormattingEnabled = true;
            durationComboBox.Items.AddRange(new object[] { "1小时", "2小时", "3小时" });
            durationComboBox.Location = durationBox.Location;
            durationComboBox.Name = "durationComboBox";
            durationComboBox.Size = durationBox.Size;
            Controls.Add(durationComboBox);

            weekBox.Hide();
            dayBox.Hide();
            hourBox.Hide();
            durationBox.Hide();
            switchPageButton.Hide();
            hourComboBox.BringToFront();
            durationComboBox.BringToFront();
            daySelectBox.BringToFront();
            weekSelectBox.BringToFront();
        }

        #endregion

        #region tool methods

        protected override bool GetTargetTimeDetail(out int[] activeWeeks, out Day[] activeDays, out int duration)
        {
            activeWeeks = new int[weekSelectBox.ValidCount];
            activeDays = new Day[daySelectBox.ValidCount];
            duration = 0;
            StringBuilder errorMessage = new("");
            if (weekSelectBox.ValidCount == 0)
            {
                errorMessage.Append("请输入日程周！\n");
            }
            if (daySelectBox.ValidCount == 0)
            {
                errorMessage.Append("请输入日程日！\n");
            }
            if (durationComboBox.Text == "")
            {
                errorMessage.Append("请输入日程时长！");
            }
            if (errorMessage.Length != 0)
            {
                MessageBox.Show(errorMessage.ToString(), "错误");
                return false;
            }

            int j = 0;
            for (int i = 0; i < 16; i++)
            {
                if (weekSelectBox.Selects[i])
                {
                    activeWeeks[j] = i + 1;
                    j++;
                }
            }
            j = 0;
            for (int i = 0; i < 7; i++)
            {
                if (daySelectBox.Selects[i])
                {
                    activeDays[j] = (Day)i;
                    j++;
                }
            }
            duration = durationComboBox.Text[0] - '0';

            return true;
        }

        protected override void AddSchedule()
        {
            bool confirm = GetScheduleInfo(true,
                                           Schedule.Activity.Earliest,
                                           Schedule.Activity.Latest,
                                           weekSelectBox,
                                           daySelectBox,
                                           null,
                                           null,
                                           hourComboBox,
                                           durationComboBox,
                                           out string name,
                                           out RepetitiveType repetitiveType,
                                           out int[] activeWeeks,
                                           out Day[] activeDays,
                                           out Times.Time beginTime,
                                           out int duration,
                                           out string buildingName,
                                           out string onlineLink);

            if (!confirm)
            {
                return;
            }

            if (buildingRadioButton.Checked)
            {
                _ = new Schedule.Activity(repetitiveType,
                                          name,
                                          beginTime,
                                          duration,
                                          descriptionBox.Text == "" ? null : descriptionBox.Text,
                                          Map.Location.GetBuildingsByName(buildingName)[0],
                                          false,
                                          activeWeeks,
                                          activeDays,
                                          ScheduleOperationType.UserOpration);
            }
            else
            {
                _ = new Schedule.Activity(repetitiveType,
                                          name,
                                          beginTime,
                                          duration,
                                          descriptionBox.Text == "" ? null : descriptionBox.Text,
                                          onlineLink,
                                          false,
                                          activeWeeks,
                                          activeDays,
                                          ScheduleOperationType.UserOpration);
            }
            GenerateUserData(_scheduleType);
            ClearInformation();
            this.weekSelectBox.ClearBox();
            this.daySelectBox.ClearBox();
            this.hourComboBox.Text = "";
            this.durationComboBox.Text = "";
        }

        #endregion

        #region event handler

        protected override void ReviseScheduleButton_Click(object sender, EventArgs e)
        {
            Times.Timer.Pause = true;

            int[] selectedRows = scheduleDataTable.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("只能选择一个日程！", "提示");
                return;
            }

            long id = long.Parse(scheduleDataTable.Rows[selectedRows[0]].Cells[9].Value.ToString()!);
            var selected = Schedule.Schedule.GetScheduleById(id);

            this.nameBox.Text = selected!.Name;
            RepetitiveType repetitiveType = selected.RepetitiveType;
            if (repetitiveType == RepetitiveType.Single)
            {
                this.weekSelectBox.SelectCheckBox(selected.BeginTime.Week - 1);
                this.daySelectBox.SelectCheckBox((int)selected.BeginTime.Day);
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

            this.hourComboBox.Text = selected.BeginTime.Hour + ":00";
            this.durationComboBox.Text = selected.Duration + "小时";
            if (selected.IsOnline)
            {
                onlineLinkBox.Text = ((Schedule.Activity)selected).OnlineLink;
                onlineLinkRadioButton.Checked = true;
            }
            else
            {
                buildingComboBox.Text = ((Schedule.Activity)selected).OfflineLocation!.Value.Name;
                buildingRadioButton.Checked = true;
            }
            descriptionBox.Text = selected.Description;
            _originId = selected.ScheduleId;
            this.reviseScheduleButton.Hide();
            this.AddScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.okButton.Show();
            this.cancelButton.Show();
            _subwindowState = SubwindowState.ReviseUserSchedule;
        }

        #endregion
    }


    public sealed class StudentTemporaryAffairSubwindow : PersonalScheduleSubwinowBase
    {
        private ComboBox weekSelectBox = new();
        private ComboBox daySelectBox = new();
        private ComboBox hourComboBox = new();

        public StudentTemporaryAffairSubwindow()
            : base(ScheduleType.TemporaryAffair, SubwindowType.TemporaryAffair)
        {
            GenerateUserData(_scheduleType);
            this.detectCollisionButton.Hide();
            GenerateSubwindow();
            _showAllData = false;
            _subwindowState = SubwindowState.Viewing;
            this.AddScheduleButton.Click += (sender, e) => AddSchedule();
            this.onlineLinkRadioButton.Enabled = false;
            this.onlineLinkBox.Enabled = false;
            this.hideDurationPictureBox.Show();
            label.Text = "临时事务";
        }

        #region UI generator

        private void GenerateSubwindow()
        {
            hourComboBox.BackColor = Color.White;
            hourComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            hourComboBox.DropDownWidth = 130;
            hourComboBox.FlatStyle = FlatStyle.Flat;
            hourComboBox.FormattingEnabled = true;
            hourComboBox.Items.AddRange(Shared.Hours.ToArray<object>());
            hourComboBox.Location = hourBox.Location;
            hourComboBox.Name = "hourComboBox";
            hourComboBox.Size = weekBox.Size;
            Controls.Add(hourComboBox);

            weekSelectBox.BackColor = Color.White;
            weekSelectBox.DropDownStyle = ComboBoxStyle.DropDownList;
            weekSelectBox.DropDownWidth = 130;
            weekSelectBox.FlatStyle = FlatStyle.Flat;
            weekSelectBox.FormattingEnabled = true;
            weekSelectBox.Items.AddRange(Shared.Weeks.ToArray<object>());
            weekSelectBox.Location = weekBox.Location;
            weekSelectBox.Name = "weekSelectBox";
            weekSelectBox.Size = weekBox.Size;
            Controls.Add(weekSelectBox);

            daySelectBox.BackColor = Color.White;
            daySelectBox.DropDownStyle = ComboBoxStyle.DropDownList;
            daySelectBox.DropDownWidth = 130;
            daySelectBox.FlatStyle = FlatStyle.Flat;
            daySelectBox.FormattingEnabled = true;
            daySelectBox.Items.AddRange(Shared.Days.ToArray<object>());
            daySelectBox.Location = dayBox.Location;
            daySelectBox.Name = "daySelectBox";
            daySelectBox.Size = dayBox.Size;
            Controls.Add(daySelectBox);

            weekBox.Hide();
            dayBox.Hide();
            hourBox.Hide();
            durationBox.Hide();
            hideDurationPictureBox.Show();
            switchPageButton.Hide();
            hourComboBox.BringToFront();
            daySelectBox.BringToFront();
            weekSelectBox.BringToFront();
        }

        protected override void GenerateUserData(Schedule.Schedule[] data)
        {
            scheduleDataTable.Rows.Clear();
            int[] widths = { 30, 55, 130, 80, 80, 60, 60, 150, 150 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleDataTable.Columns[i].Width = widths[i];
            }
            scheduleDataTable.Columns[5].Visible = true;
            scheduleDataTable.Columns[6].Visible = false;
            scheduleDataTable.Columns[7].Visible = true;
            var converted = data.Select(elem => (Schedule.TemporaryAffair)elem);
            foreach (var affair in converted)
            {
                scheduleDataTable.Rows.Add(null,
                                           affair.AlarmEnabled,
                                           affair.Name,
                                           affair.BeginTime.Week,
                                           affair.BeginTime.Day.ToString()[..3],
                                           affair.BeginTime.Hour.ToString() + ":00",
                                           "",
                                           affair.Description ?? "",
                                           affair.OfflineLocation.Name,
                                           affair.ScheduleId.ToString());
            }
        }

        #endregion

        #region tool methods

        protected override bool GetTargetTimeDetail(out int[] activeWeeks, out Day[] activeDays, out int duration)
        {
            activeWeeks = Constants.EmptyIntArray;
            activeDays = Constants.EmptyDayArray;
            duration = 1;
            StringBuilder errorMessage = new("");
            if (weekSelectBox.Text == "")
            {
                errorMessage.Append("请输入日程周！\n");
            }
            if (daySelectBox.Text == "")
            {
                errorMessage.Append("请输入日程日！\n");
            }

            if (errorMessage.Length != 0)
            {
                MessageBox.Show(errorMessage.ToString(), "错误");
                return false;
            }

            activeWeeks = new[] { int.Parse(weekSelectBox.Text[4..]) };
            for (int i = 0; i < 7; i++)
            {
                if (daySelectBox.Text == Shared.Days[i])
                {
                    activeDays = new[] { (Day)i };
                    break;
                }
            }

            return true;
        }

        protected override void AddSchedule()
        {
            bool confirm = GetScheduleInfo(true,
                                           Schedule.Activity.Earliest,
                                           Schedule.Activity.Latest,
                                           null,
                                           null,
                                           weekSelectBox,
                                           daySelectBox,
                                           hourComboBox,
                                           null,
                                           out string name,
                                           out _,
                                           out _,
                                           out _,
                                           out Times.Time beginTime,
                                           out _,
                                           out string buildingName,
                                           out _);

            if (!confirm)
            {
                return;
            }

            {
                _ = new Schedule.TemporaryAffair(name,
                                                 beginTime,
                                                 descriptionBox.Text == "" ? null : descriptionBox.Text,
                                                 Map.Location.GetBuildingsByName(buildingName)[0]);
                GenerateUserData(_scheduleType);
                ClearInformation();
                this.hourComboBox.Text = "";
                return;
            }
        }

        #endregion

        #region event handler

        protected override void ReviseScheduleButton_Click(object sender, EventArgs e)
        {
            Times.Timer.Pause = true;

            int[] selectedRows = scheduleDataTable.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("只能选择一个日程！", "提示");
                return;
            }

            long id = long.Parse(scheduleDataTable.Rows[selectedRows[0]].Cells[9].Value.ToString()!);
            var selected = Schedule.Schedule.GetScheduleById(id);

            nameBox.Text = selected!.Name;
            weekSelectBox.Text = "Week" + selected.BeginTime.Week;
            daySelectBox.Text = Shared.Days[(int)selected.BeginTime.Day];
            descriptionBox.Text = selected.Description ?? "";
            hourComboBox.Text = selected.BeginTime.Hour + ":00";
            buildingComboBox.Text = ((Schedule.TemporaryAffair)selected).OfflineLocation.Name;
            _originId = selected.ScheduleId;
            this.reviseScheduleButton.Hide();
            this.AddScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.okButton.Show();
            this.cancelButton.Show();
            _subwindowState = SubwindowState.ReviseUserSchedule;
        }

        #endregion
    }
}