﻿using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace StudentScheduleManagementSystem.UI
{
    public abstract partial class StudentSubwindowBase : Form
    {
        protected List<Schedule.ScheduleBase.SharedData> _sharedData;
        protected List<Schedule.ScheduleBase> _userData;
        protected ScheduleType _scheduleType;
        protected SubwindowState _subwindowState = SubwindowState.Viewing;
        protected SubwindowType _subwindowType;
        protected long? _originId = null;
        protected bool _showAllData = true;

        #region ctor

        protected StudentSubwindowBase()
            : this(ScheduleType.Idle, SubwindowType.Default) { }

        protected StudentSubwindowBase(ScheduleType scheduleType, SubwindowType subwindowType)
        {
            InitializeComponent();
            if (scheduleType != ScheduleType.TemporaryAffair)
            {
                GenerateSharedData(scheduleType);
            }
            _scheduleType = scheduleType;
            _subwindowType = subwindowType;
            foreach (var building in Map.Location.Buildings)
            {
                buildingComboBox.Items.Add(building.Name);
            }
            this.detectCollisionButton.Click += (sender, e) => DetectCollision(true);
            this.deleteScheduleButton.Click += DeleteScheduleButton_Click;
            this.okButton.Hide();
            this.cancelButton.Hide();
        }

        #endregion

        #region tool methods

        protected static StringBuilder GetBriefWeeks(int[] activeWeeks)
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

        #endregion

        #region table content generator

        protected void GenerateSharedData(ScheduleType type)
        {
            _sharedData = Schedule.ScheduleBase.GetSharedByType(type);
            GenerateSharedData(_sharedData);
        }

        protected void GenerateSharedData(List<Schedule.ScheduleBase.SharedData> data)
        {
            scheduleData.Rows.Clear();
            scheduleData.Columns[6].Visible = false;
            scheduleData.Columns[7].Visible = false;
            int[] widths = { 30, 130, 120, 130, 60, 60 };
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
                                               sharedData.Timestamp.Week.ToString(),
                                               sharedData.Timestamp.Day.ToString()[..3],
                                               sharedData.Timestamp.Hour.ToString() + ":00",
                                               sharedData.Duration.ToString() + "小时",
                                               null,
                                               null,
                                               sharedData.Id);
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
                                               "1-16",
                                               days.ToString(),
                                               sharedData.Timestamp.Hour.ToString() + ":00",
                                               sharedData.Duration.ToString() + "小时",
                                               null,
                                               null,
                                               sharedData.Id);
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
                                               GetBriefWeeks(sharedData.ActiveWeeks).ToString(),
                                               days.ToString(),
                                               sharedData.Timestamp.Hour.ToString() + ":00",
                                               sharedData.Duration.ToString() + "小时",
                                               null,
                                               null,
                                               sharedData.Id);
                }
            }
        }

        protected void GenerateUserData(ScheduleType type)
        {
            _userData = Schedule.ScheduleBase.GetScheduleByType(type);
            GenerateUserData(_userData);
        }

        protected abstract void GenerateUserData(List<Schedule.ScheduleBase> data);

        protected long DetectCollision(bool showMessageBox)
        {
            int[] selectedRows = scheduleData.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("只能选择一个日程！");
                return 0;
            }

            long id = long.Parse(scheduleData.Rows[selectedRows[0]].Cells[8].Value.ToString()!);

            var selected = Schedule.ScheduleBase.GetSharedById(id);

            bool willCollide = Schedule.ScheduleBase.DetectCollision(selected!.RepetitiveType,
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
                MessageBox.Show("该日程与您的类型为" + types.ToString() + "的日程冲突");
                return 0;
            }
            else
            {
                if (showMessageBox)
                {
                    MessageBox.Show("该日程与您的日程没有冲突");
                }
                return id;
            }
        }

        protected void ClearInput()
        {
            this.nameBox.Text = "";
            this.weekBox.Text = "";
            this.dayBox.Text = "";
            this.hourBox.Text = "";
            this.durationBox.Text = "";
        }

        #endregion

        #region envent handler

        private void SwitchData_Click(object sender, EventArgs e)
        {
            if (_showAllData)
            {
                _showAllData = false;
                GenerateUserData(_scheduleType);
            }
            else
            {
                _showAllData = true;
                GenerateSharedData(_scheduleType);
            }
        }

        protected void ReviseCancel_Click(object sender, EventArgs e)
        {
            ClearInput();
            this.addScheduleButton.Show();
            this.deleteScheduleButton.Show();
            this.reviseScheduleButton.Show();
            this.okButton.Hide();
            this.cancelButton.Hide();
            this._subwindowState = SubwindowState.Viewing;
            foreach (DataGridViewRow row in scheduleData.Rows)
            {
                ((DataGridViewCheckBoxCell)row.Cells[0]).ReadOnly = false;
            }
        }

        protected void DeleteScheduleButton_Click(object sender, EventArgs e)
        {
            if (_showAllData)
            {
                MessageBox.Show("请在个人日程页面选择日程删除！");
                return;
            }

            int[] selectedRows = scheduleData.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("只能选择一个日程！");
                return;
            }

            long id = long.Parse(scheduleData.Rows[selectedRows[0]].Cells[8].Value.ToString()!);

            if (MessageBox.Show("名称: " + scheduleData.Rows[selectedRows[0]].Cells[1].Value.ToString() + "\n周次: " +
                                scheduleData.Rows[selectedRows[0]].Cells[2].Value.ToString() + "\n天次: " +
                                scheduleData.Rows[selectedRows[0]].Cells[3].Value.ToString() + "\n时间: " +
                                scheduleData.Rows[selectedRows[0]].Cells[4].Value.ToString() + "\n时长: " +
                                scheduleData.Rows[selectedRows[0]].Cells[5].Value.ToString() + "\n地点/链接：" +
                                scheduleData.Rows[selectedRows[0]].Cells[6].Value.ToString(),
                                "确认日程信息",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                var selected = Schedule.ScheduleBase.GetScheduleById(id);
                selected!.DeleteSchedule();
            }
            GenerateUserData(_scheduleType);
        }


        private void SearchOK_Click(object sender, EventArgs e)
        {
            if (this.searchByNameBox.Text.Equals(""))
            {
                MessageBox.Show("请输入要搜索的日程名！");
                return;
            }

            if (_showAllData)
            {
                var result = Schedule.ScheduleBase.GetSharedByName(this.searchByNameBox.Text);
                if (result.Count == 0)
                {
                    MessageBox.Show("未搜索到日程！");
                    return;
                }
                GenerateSharedData(result);
            }
            else
            {
                var result = Schedule.ScheduleBase.GetSchedulesByName(this.searchByNameBox.Text);
                if (result.Count == 0)
                {
                    MessageBox.Show("未搜索到日程！");
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

        #endregion
    }

    public abstract class SharedScheduleSubwindowBase : StudentSubwindowBase
    {
        protected SharedScheduleSubwindowBase(ScheduleType scheduleType, SubwindowType subwindowType)
            : base(scheduleType, subwindowType)
        {
            this.addScheduleButton.Click += AddScheduleButton_Click;
            this.okButton.Click += OkButton_Click;
            this.reviseScheduleButton.Click += ReviseButton_Click;
        }

        #region table content generator

        protected override void GenerateUserData(List<Schedule.ScheduleBase> data)
        {
            scheduleData.Rows.Clear();
            int[] widths = { 30, 130, 120, 130, 60, 60, 150, 150 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleData.Columns[i].Width = widths[i];
            }
            scheduleData.Columns[6].Visible = true;
            scheduleData.Columns[7].Visible = true;

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
                    activeWeeks = schedule.ActiveWeeks[0].ToString();
                }
                else if (schedule.RepetitiveType == RepetitiveType.MultipleDays)
                {
                    activeWeeks = "1-16";
                }
                else
                {
                    activeWeeks = GetBriefWeeks(schedule.ActiveWeeks).ToString();
                }

                scheduleData.Rows.Add(null,
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

        private bool GetScheduleInfo(bool showMessageBox, DataGridViewRow selected, out string offlineLocationName, out string onlineLink)
        {
            StringBuilder errorMessage = new();
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
            else if (linkRadioButton.Checked && linkRadioButton.Visible)
            {
                if (onlineLinkBox.Text == "")
                {
                    errorMessage.AppendLine("请输入日程链接！");
                }
                else
                {
                    location = $"\n链接: {buildingComboBox.Text}";
                }
            }
            else if (!buildingRadioButton.Visible && !linkRadioButton.Visible)
            { }
            else
            {
                errorMessage.AppendLine("未选择线下地址或线上链接，或程序遇到错误");
            }
            if (!errorMessage.Equals(""))
            {
                MessageBox.Show(errorMessage.ToString());
                return false;
            }

            return showMessageBox
                       ? MessageBox.Show("名称: " + selected.Cells[1].Value.ToString() + "\n周次: " +
                                         selected.Cells[2].Value.ToString() + "\n天次: " +
                                         selected.Cells[3].Value.ToString() + "\n时间: " +
                                         selected.Cells[4].Value.ToString() + "\n时长: " +
                                         selected.Cells[5].Value.ToString() + location,
                                         "确认日程信息",
                                         MessageBoxButtons.OKCancel) == DialogResult.OK
                       : true;
        }

        protected void AddSchedule(long id)
        {
            var selected = Schedule.ScheduleBase.GetSharedById(id);
            Type scheduleType = _scheduleType switch
            {
                ScheduleType.Course => typeof(Schedule.Course),
                ScheduleType.Exam => typeof(Schedule.Exam),
                ScheduleType.Activity => typeof(Schedule.Activity),
                _ => throw new ArgumentException(null, nameof(_scheduleType)),
            };
            var ctors = scheduleType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            //offline
            if (buildingRadioButton.Checked)
            {
                var ctor = ctors.FirstOrDefault(ctor => ctor.GetParameters()
                                                            .FirstOrDefault(info => info.ParameterType ==
                                                                                typeof(Map.Location.Building)) != null);
                if (ctor == null)
                {
                    throw new InvalidOperationException("specific ctor could not be found");
                }
                _ = ctor.Invoke(new object?[]
                {
                    selected!.RepetitiveType,
                    selected.Name,
                    selected.Timestamp,
                    selected.Duration,
                    descriptionBox.Text == "" ? null : descriptionBox.Text,
                    Map.Location.GetBuildingsByName(buildingComboBox.Text)[0],
                    selected.ActiveWeeks,
                    selected.ActiveDays,
                    ScheduleOperationType.UserOpration,
                    id
                });
            }
            //online
            else
            {
                var ctor = ctors.FirstOrDefault(ctor => ctor.GetParameters()
                                                            .FirstOrDefault(info => info.ParameterType ==
                                                                                typeof(Map.Location.Building)) == null);
                if (ctor == null)
                {
                    throw new InvalidOperationException("specific ctor could not be found");
                }
                _ = ctor.Invoke(new object?[]
                {
                    selected!.RepetitiveType,
                    selected.Name,
                    selected.Timestamp,
                    selected.Duration,
                    descriptionBox.Text == "" ? null : descriptionBox.Text,
                    onlineLinkBox,
                    selected.ActiveWeeks,
                    selected.ActiveDays,
                    ScheduleOperationType.UserOpration,
                    id
                });
            }
        }

        #endregion

        #region event handler

        protected void AddScheduleButton_Click(object sender, EventArgs e)
        {
            if (!_showAllData)
            {
                MessageBox.Show("请在公有日程页面选择日程添加！");
                return;
            }

            long id = DetectCollision(false);
            if (id == 0)
            {
                return;
            }

            _subwindowState = SubwindowState.AddUserSchedule;

            this.addScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.reviseScheduleButton.Hide();
            this.okButton.Show();
            this.cancelButton.Show();

            var selected = Schedule.ScheduleBase.GetSharedById(id);
            ClearInput();
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
                this.weekBox.Text = GetBriefWeeks(selected.ActiveWeeks).ToString();
                foreach (Day activeDay in selected.ActiveDays)
                {
                    this.dayBox.Text += activeDay.ToString()[..3] + "; ";
                }
            }
            this.hourBox.Text = selected.Timestamp.Hour.ToString() + ":00";
            this.durationBox.Text = selected.Duration.ToString() + "小时";

            _subwindowState = SubwindowState.AddUserSchedule;
            foreach (DataGridViewRow row in scheduleData.Rows)
            {
                ((DataGridViewCheckBoxCell)row.Cells[0]).ReadOnly = true;
            }
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            Debug.Assert((_subwindowState, _originId) is (SubwindowState.ReviseUserSchedule, > (long)1e9) or
                                                         (SubwindowState.AddUserSchedule, null));

            int[] selectedRows = scheduleData.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("只能选择一个日程！");
                return;
            }

            //TODO:冲突检测
            long id = long.Parse(scheduleData.Rows[selectedRows[0]].Cells[8].Value.ToString()!);
            bool confirm = GetScheduleInfo(true, scheduleData.Rows[selectedRows[0]], out _, out _);
            if (!confirm)
            {
                return;
            }
            if (_subwindowState == SubwindowState.ReviseUserSchedule)
            {
                var selected = Schedule.ScheduleBase.GetScheduleById(id);
                selected!.DeleteSchedule();
            }
            AddSchedule(_originId ?? id);

            this.addScheduleButton.Show();
            this.deleteScheduleButton.Show();
            this.reviseScheduleButton.Show();
            this.okButton.Hide();
            this.cancelButton.Hide();
            this._subwindowState = SubwindowState.Viewing;
            foreach (DataGridViewRow row in scheduleData.Rows)
            {
                ((DataGridViewCheckBoxCell)row.Cells[0]).ReadOnly = false;
            }
        }

        protected void ReviseButton_Click(object sender, EventArgs e)
        {
            if (_showAllData)
            {
                MessageBox.Show("请在个人日程页面选择日程修改！");
                return;
            }

            int[] selectedRows = scheduleData.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("只能选择一个日程！");
                return;
            }

            this.addScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.reviseScheduleButton.Hide();
            this.okButton.Show();
            this.cancelButton.Show();

            long id = long.Parse(scheduleData.Rows[selectedRows[0]].Cells[8].Value.ToString()!);
            _originId = id;
            var selected = Schedule.ScheduleBase.GetScheduleById(id);
            ClearInput();
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
                this.weekBox.Text = GetBriefWeeks(selected.ActiveWeeks).ToString();
                foreach (Day activeDay in selected.ActiveDays)
                {
                    this.dayBox.Text += activeDay.ToString()[..3] + "; ";
                }
            }
            this.hourBox.Text = selected.BeginTime.ToString() + ":00";
            this.durationBox.Text = selected.Duration.ToString() + "小时";
            this.descriptionBox.Text = selected.Description;

            _subwindowState = SubwindowState.ReviseUserSchedule;
            foreach (DataGridViewRow row in scheduleData.Rows)
            {
                ((DataGridViewCheckBoxCell)row.Cells[0]).ReadOnly = true;
            }
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
        }

        #region table content generator

        protected override void GenerateUserData(List<Schedule.ScheduleBase> list)
        {
            scheduleData.Rows.Clear();
            int[] widths = { 30, 130, 120, 130, 60, 60, 150, 150 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleData.Columns[i].Width = widths[i];
            }
            scheduleData.Columns[6].Visible = true;
            scheduleData.Columns[7].Visible = true;

            foreach (var schedule in list)
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
                else if (schedule is Schedule.TemporaryAffairs temp)
                {
                    location = temp.OfflineLocation.Name;
                }

                string activeWeeks;
                if (schedule.RepetitiveType == RepetitiveType.Single)
                {
                    activeWeeks = schedule.ActiveWeeks[0].ToString();
                }
                else if (schedule.RepetitiveType == RepetitiveType.MultipleDays)
                {
                    activeWeeks = "1-16";
                }
                else
                {
                    activeWeeks = GetBriefWeeks(schedule.ActiveWeeks).ToString();
                }

                scheduleData.Rows.Add(null,
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

        protected abstract void AddSchedule();

        #endregion

        #region event handler

        protected void OkButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(_subwindowState is SubwindowState.ReviseUserSchedule && _originId != null);

            //TODO:冲突检测
            var selected = Schedule.ScheduleBase.GetScheduleById(_originId.Value);
            selected!.DeleteSchedule();
            AddSchedule();

            this.addScheduleButton.Show();
            this.deleteScheduleButton.Show();
            this.reviseScheduleButton.Show();
            this.okButton.Hide();
            this.cancelButton.Hide();
            this._subwindowState = SubwindowState.Viewing;
        }

        protected abstract void ReviseScheduleButton_Click(object sender, EventArgs e);

        #endregion
    }

    public sealed class StudentCourseSubwindow : SharedScheduleSubwindowBase
    {
        public StudentCourseSubwindow()
            : base(ScheduleType.Course, SubwindowType.Course) { }
    }

    public sealed class StudentExamSubwindow : SharedScheduleSubwindowBase
    {
        public StudentExamSubwindow()
            : base(ScheduleType.Exam, SubwindowType.Exam) { }
    }

    public sealed class StudentGroupActivitySubwindow : SharedScheduleSubwindowBase
    {
        public StudentGroupActivitySubwindow()
            : base(ScheduleType.Activity, SubwindowType.GroupActivity) { }
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
            GenerateSubwindow();
            _showAllData = false;
            _subwindowState = SubwindowState.Viewing;
            this.addScheduleButton.Click += (sender, e) => AddSchedule();
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
            switchDataButton.Hide();
            daySelectBox.BringToFront();
            weekSelectBox.BringToFront();
        }

        #endregion

        #region tool methods

        private bool GetScheduleInfo(bool showMessageBox,
                                     out string name,
                                     out RepetitiveType repetitiveType,
                                     out int[] activeWeeks,
                                     out Day[] activeDays,
                                     out int beginHour,
                                     out int duration,
                                     out string offlineLocationName,
                                     out string onlineLink
        )
        {
            StringBuilder errorMessage = new();
            name = "";
            repetitiveType = RepetitiveType.Null;
            activeWeeks = Constants.EmptyIntArray;
            activeDays = Constants.EmptyDayArray;
            beginHour = 0;
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
            if (buildingRadioButton.Checked && buildingRadioButton.Visible && buildingComboBox.Text == "")
            {
                errorMessage.AppendLine("请输入日程地址！");
            }
            else if (linkRadioButton.Checked && linkRadioButton.Visible && onlineLinkBox.Text == "")
            {
                errorMessage.AppendLine("请输入日程链接！");
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
                       ? MessageBox.Show(Shared.GetScheduleDetail(name,
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

        //TODO:简化if
        protected override void AddSchedule()
        {
            bool confirm = GetScheduleInfo(true,
                                           out string name,
                                           out RepetitiveType repetitiveType,
                                           out int[] activeWeeks,
                                           out Day[] activeDays,
                                           out int beginHour,
                                           out int duration,
                                           out string buildingName,
                                           out string onlineLink);

            if (!confirm)
            {
                return;
            }

            if (beginHour + duration >= Schedule.Activity.Latest)
            {
                MessageBox.Show("活动结束时间不得晚于规定时间！");
                return;
            }

            if (repetitiveType == RepetitiveType.Single)
            {
                if (buildingRadioButton.Checked)
                {
                    _ = new Schedule.Activity(RepetitiveType.Single,
                                              name,
                                              new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginHour },
                                              duration,
                                              descriptionBox.Text == "" ? null : descriptionBox.Text,
                                              Map.Location.GetBuildingsByName(buildingName)[0],
                                              false,
                                              Constants.EmptyIntArray,
                                              Constants.EmptyDayArray,
                                              ScheduleOperationType.UserOpration);
                }
                else
                {
                    _ = new Schedule.Activity(RepetitiveType.Single,
                                              name,
                                              new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginHour },
                                              duration,
                                              descriptionBox.Text == "" ? null : descriptionBox.Text,
                                              onlineLink,
                                              false,
                                              Constants.EmptyIntArray,
                                              Constants.EmptyDayArray,
                                              ScheduleOperationType.UserOpration);
                }
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                if (buildingRadioButton.Checked)
                {
                    _ = new Schedule.Activity(RepetitiveType.MultipleDays,
                                              name,
                                              new() { Hour = beginHour },
                                              duration,
                                              descriptionBox.Text == "" ? null : descriptionBox.Text,
                                              Map.Location.GetBuildingsByName(buildingName)[0],
                                              false,
                                              Constants.EmptyIntArray,
                                              activeDays,
                                              ScheduleOperationType.UserOpration);
                }
                else
                {
                    _ = new Schedule.Activity(RepetitiveType.MultipleDays,
                                              name,
                                              new() { Hour = beginHour },
                                              duration,
                                              descriptionBox.Text == "" ? null : descriptionBox.Text,
                                              onlineLink,
                                              false,
                                              Constants.EmptyIntArray,
                                              activeDays,
                                              ScheduleOperationType.UserOpration);
                }
            }
            else
            {
                if (buildingRadioButton.Checked)
                {
                    _ = new Schedule.Activity(RepetitiveType.Designated,
                                              name,
                                              new() { Hour = beginHour },
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
                    _ = new Schedule.Activity(RepetitiveType.Designated,
                                              name,
                                              new() { Hour = beginHour },
                                              duration,
                                              descriptionBox.Text == "" ? null : descriptionBox.Text,
                                              onlineLink,
                                              false,
                                              activeWeeks,
                                              activeDays,
                                              ScheduleOperationType.UserOpration);
                }
            }

            GenerateUserData(_scheduleType);
            return;
        }

        #endregion

        #region event handler

        protected override void ReviseScheduleButton_Click(object sender, EventArgs e)
        {
            int[] selectedRows = scheduleData.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("只能选择一个日程！");
                return;
            }

            long id = long.Parse(scheduleData.Rows[selectedRows[0]].Cells[8].Value.ToString()!);
            var selected = Schedule.ScheduleBase.GetScheduleById(id);

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
                linkRadioButton.Checked = true;
            }
            else
            {
                buildingComboBox.Text = ((Schedule.Activity)selected).OfflineLocation!.Value.Name;
                buildingRadioButton.Checked = true;
            }
            descriptionBox.Text = selected.Description;
            _originId = selected.ScheduleId;
            this.reviseScheduleButton.Hide();
            this.addScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.okButton.Show();
            this.cancelButton.Show();
            _subwindowState = SubwindowState.ReviseUserSchedule;
        }

        #endregion
    }


    public sealed class StudentTemporaryAffairSubwindow : PersonalScheduleSubwinowBase
    {
        private ComboBox _weekSelectBox = new();
        private ComboBox _daySelectBox = new();
        private ComboBox _hourComboBox = new();

        public StudentTemporaryAffairSubwindow()
            : base(ScheduleType.TemporaryAffair, SubwindowType.TemporaryAffair)
        {
            GenerateUserData(_scheduleType);
            GenerateSubwindow();
            _showAllData = false;
            _subwindowState = SubwindowState.Viewing;
            this.addScheduleButton.Click += (sender, e) => AddSchedule();
        }

        #region UI generator

        private void GenerateSubwindow()
        {
            _hourComboBox.BackColor = Color.White;
            _hourComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _hourComboBox.DropDownWidth = 130;
            _hourComboBox.FlatStyle = FlatStyle.Flat;
            _hourComboBox.FormattingEnabled = true;
            _hourComboBox.Items.AddRange(Shared.Hours.ToArray<object>());
            _hourComboBox.Location = hourBox.Location;
            _hourComboBox.Name = "hourComboBox";
            _hourComboBox.Size = weekBox.Size;
            Controls.Add(_hourComboBox);

            _weekSelectBox.BackColor = Color.White;
            _weekSelectBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _weekSelectBox.DropDownWidth = 130;
            _weekSelectBox.FlatStyle = FlatStyle.Flat;
            _weekSelectBox.FormattingEnabled = true;
            _weekSelectBox.Items.AddRange(Shared.Weeks.ToArray<object>());
            _weekSelectBox.Location = weekBox.Location;
            _weekSelectBox.Name = "weekSelectBox";
            _weekSelectBox.Size = weekBox.Size;
            Controls.Add(_weekSelectBox);

            _daySelectBox.BackColor = Color.White;
            _daySelectBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _daySelectBox.DropDownWidth = 130;
            _daySelectBox.FlatStyle = FlatStyle.Flat;
            _daySelectBox.FormattingEnabled = true;
            _daySelectBox.Items.AddRange(Shared.Days.ToArray<object>());
            _daySelectBox.Location = dayBox.Location;
            _daySelectBox.Name = "daySelectBox";
            _daySelectBox.Size = dayBox.Size;
            Controls.Add(_daySelectBox);

            weekBox.Hide();
            dayBox.Hide();
            hourBox.Hide();
            durationBox.Hide();
            switchDataButton.Hide();
        }

        protected override void GenerateUserData(List<Schedule.ScheduleBase> list)
        {
            scheduleData.Rows.Clear();
            int[] widths = { 30, 130, 50, 80, 60, 60, 150, 150 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleData.Columns[i].Width = widths[i];
            }
            scheduleData.Columns[5].Visible = false;
            scheduleData.Columns[6].Visible = true;
            scheduleData.Columns[7].Visible = true;
            var converted = list.Select(elem => (Schedule.TemporaryAffairs)elem);
            foreach (var affair in converted)
            {
                scheduleData.Rows.Add(null,
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

        protected override void AddSchedule()
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
            if (nameBox.Text is "Default" or "default")
            {
                errorMessage.AppendLine("日程名不能为保留值！");
            }
            if (_weekSelectBox.Text.Equals(""))
            {
                errorMessage.AppendLine("请输入日程周！");
            }
            if (_daySelectBox.Text.Equals(""))
            {
                errorMessage.AppendLine("请输入日程日！");
            }
            if (_hourComboBox.Text == "")
            {
                errorMessage.AppendLine("请输入日程时间！");
            }
            if (buildingComboBox.Text == "")
            {
                errorMessage.AppendLine("请输入日程地址！");
            }
            if (!errorMessage.Equals(""))
            {
                MessageBox.Show(errorMessage.ToString());
                return;
            }

            string name = nameBox.Text;
            int beginHour;
            int activeWeek = int.Parse(_weekSelectBox.Text[4..]);
            Day activeDay = Day.Monday;

            for (int i = 0; i < Shared.Days.Length; i++)
            {
                if (Shared.Days[i].Equals(_daySelectBox.Text))
                {
                    activeDay = (Day)i;
                    break;
                }
            }
            if (_hourComboBox.Text.Length == 5)
            {
                beginHour = (_hourComboBox.Text[0] - '0') * 10 + _hourComboBox.Text[1] - '0';
            }
            else
            {
                beginHour = _hourComboBox.Text[0] - '0';
            }

            Times.Time beginTime = new() { Week = activeWeek, Day = activeDay, Hour = beginHour };
            if (MessageBox.Show(Shared.GetScheduleDetail(name,
                                                         RepetitiveType.Single,
                                                         new int[] { activeWeek },
                                                         new Day[] { activeDay },
                                                         beginTime,
                                                         1)
                                      .ToString(),
                                "确认日程信息",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _ = new Schedule.TemporaryAffairs(name,
                                                  beginTime,
                                                  descriptionBox.Text == "" ? null : descriptionBox.Text,
                                                  Map.Location.Buildings[0]);
                GenerateUserData(_scheduleType);
                return;
            }
        }

        #endregion

        #region event handler

        protected override void ReviseScheduleButton_Click(object sender, EventArgs e)
        {
            int[] selectedRows = scheduleData.GetSelectedRowsCount(0);
            if (selectedRows.Length != 1)
            {
                MessageBox.Show("只能选择一个日程！");
                return;
            }

            long id = long.Parse(scheduleData.Rows[selectedRows[0]].Cells[8].Value.ToString()!);
            var selected = Schedule.ScheduleBase.GetScheduleById(id);

            nameBox.Text = selected!.Name;
            _weekSelectBox.Text = "Week" + selected.BeginTime.Week;
            _daySelectBox.Text = Shared.Days[(int)selected.BeginTime.Day];
            descriptionBox.Text = selected.Description ?? "";
            _hourComboBox.Text = selected.BeginTime.Hour + ":00";
            buildingComboBox.Text = ((Schedule.TemporaryAffairs)selected).OfflineLocation.Name;
            _originId = selected.ScheduleId;
            this.reviseScheduleButton.Hide();
            this.addScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.okButton.Show();
            this.cancelButton.Show();
            _subwindowState = SubwindowState.ReviseUserSchedule;
        }

        #endregion
    }
}