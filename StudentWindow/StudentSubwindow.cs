using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace StudentScheduleManagementSystem.UI
{
    public abstract partial class StudentSubwindowBase : Form
    {
        protected List<Schedule.ScheduleBase.SharedData> _data;
        protected List<Schedule.ScheduleBase> _userData;
        protected ScheduleType _scheduleType;
        protected SubwindowState _subwindowState = SubwindowState.Viewing;
        protected SubwindowType _subwindowType;
        protected long? _originId = null;
        protected bool showAllData = true;

        private StudentSubwindowBase()
            : this(ScheduleType.Idle, SubwindowType.Default) { }
        protected StudentSubwindowBase(ScheduleType scheduleType, SubwindowType subwindowType)
        {
            InitializeComponent();
            if(scheduleType == ScheduleType.Idle || scheduleType == ScheduleType.Course ||
                scheduleType == ScheduleType.Exam || scheduleType == ScheduleType.Activity)
            {
                GenerateSharedData(scheduleType);
            }
            _scheduleType = scheduleType;
            _subwindowType = subwindowType;
            for (int i = 0; i < Map.Location.Buildings.Count; i++)
            {
                buildingComboBox.Items.Add(Map.Location.Buildings[i].Name);
            }
            this.reviseOK.Hide();
            this.reviseCancel.Hide();
        }

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

        private void SwitchData_Click(object sender, EventArgs e)
        {
            if (showAllData)
            {
                showAllData = false;
                GenerateUserData(_scheduleType);
            }
            else
            {
                showAllData = true;
                GenerateSharedData(_scheduleType);
            }
        }

        protected void GenerateSharedData(ScheduleType type)
        {
            _data = Schedule.ScheduleBase.GetSharedByType(type);
            GenerateSharedData(_data);
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

        abstract protected void GenerateUserData(List<Schedule.ScheduleBase> data);

        private void DetectCollision_Click(object sender, EventArgs e)
        {
            DetectCollision(true);
        }

        private long DetectCollision(bool showMessageBox)
        {
            int selectedCount = 0, index = 0;
            for (int i = 0; i < _data.Count; i++)
            {
                if (Convert.ToBoolean(scheduleData.Rows[i].Cells[0].EditedFormattedValue))
                {
                    selectedCount++;
                    index = i;
                }
            }
            if (selectedCount == 0)
            {
                MessageBox.Show("请选择日程！");
                return 0;
            }
            if (selectedCount >= 2)
            {
                MessageBox.Show("请一次选择一个日程！");
                return 0;
            }

            long id = long.Parse(scheduleData.Rows[index].Cells[8].Value.ToString()!);

            var selected = Schedule.ScheduleBase.GetSharedById(id);

            bool willCollide = Schedule.ScheduleBase.DetectCollision(
                                                                     selected!.RepetitiveType,
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

        private void ClearInput()
        {
            this.nameBox.Text = "";
            this.weekBox.Text = "";
            this.dayBox.Text = "";
            this.hourBox.Text = "";
            this.durationBox.Text = "";
        }

        private void AddScheduleButton_Click(object sender, EventArgs e)
        {
            if(_subwindowType == SubwindowType.PersonalActivity ||
                _subwindowType == SubwindowType.TemporaryAffair)
            {
                AddPersonalSchedule(null);
            }
            else
            {
                AddGroupSchedule();
            }
        }

        protected virtual void AddPersonalSchedule(long? id)
        {
            
        }

        protected void AddGroupSchedule()
        {
            if (!showAllData)
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
            this.reviseOK.Show();
            this.reviseCancel.Show();

            _originId = id;
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
        }

        private void DeleteScheduleButton_Click(object sender, EventArgs e)
        {
            if (_subwindowType == SubwindowType.PersonalActivity ||
                _subwindowType == SubwindowType.TemporaryAffair)
            {
                DeletePersonalSchedule();
            }
            else
            {
                DeleteGroupSchedule();
            }
        }

        protected virtual void DeletePersonalSchedule()
        {

        }

        protected void DeleteGroupSchedule()
        {
            if (showAllData)
            {
                MessageBox.Show("请在私有日程页面选择日程删除！");
                return;
            }

            int selectedCount = 0, index = 0;
            for (int i = 0; i < _userData.Count; i++)
            {
                if (Convert.ToBoolean(scheduleData.Rows[i].Cells[0].EditedFormattedValue))
                {
                    selectedCount++;
                    index = i;
                }
            }
            if (selectedCount == 0)
            {
                MessageBox.Show("请选择日程！");
                return;
            }
            if (selectedCount >= 2)
            {
                MessageBox.Show("请一次选择一个日程！");
                return;
            }

            long id = long.Parse(scheduleData.Rows[index].Cells[8].Value.ToString()!);

            if (MessageBox.Show(
                                "周次: " + scheduleData.Rows[index].Cells[2].Value.ToString() +
                                "\n天次: " + scheduleData.Rows[index].Cells[3].Value.ToString() +
                                "\n时间: " + scheduleData.Rows[index].Cells[5].Value.ToString() +
                                "\n时长: " + scheduleData.Rows[index].Cells[6].Value.ToString() +
                                "\n名称: " + scheduleData.Rows[index].Cells[1].Value.ToString(),
                                "日程信息",
                                MessageBoxButtons.OKCancel
                                ) == DialogResult.OK)
            {
                var selected = Schedule.ScheduleBase.GetScheduleById(id);
                selected!.DeleteSchedule();
            }
            GenerateUserData(_scheduleType);
        }

        private void ReviseScheduleButton_Click(object sender, EventArgs e)
        {
            if (_subwindowType == SubwindowType.PersonalActivity ||
                _subwindowType == SubwindowType.TemporaryAffair)
            {
                RevisePersonalSchedule();
            }
            else
            {
                ReviseGroupActivity();
            }
        }

        protected virtual void RevisePersonalSchedule()
        {

        }

        protected void ReviseGroupActivity()
        {
            if (showAllData)
            {
                MessageBox.Show("请在个人日程页面选择日程修改！");
                return;
            }

            int selectedCount = 0, index = 0;
            for (int i = 0; i < _userData.Count; i++)
            {
                if (Convert.ToBoolean(scheduleData.Rows[i].Cells[0].EditedFormattedValue))
                {
                    selectedCount++;
                    index = i;
                }
            }
            if (selectedCount == 0)
            {
                MessageBox.Show("请选择日程！");
                return;
            }
            if (selectedCount >= 2)
            {
                MessageBox.Show("请一次选择一个日程！");
                return;
            }

            this.addScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.reviseScheduleButton.Hide();
            this.reviseOK.Show();
            this.reviseCancel.Show();

            long id = long.Parse(scheduleData.Rows[index].Cells[8].Value.ToString()!);
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
        }

        private void ReviseOK_Click(object sender, EventArgs e)
        {
            if (_originId == null)
            {
                return;
            }

            if (_subwindowState == SubwindowState.ReviseUserSchedule)
            {
                var selected = Schedule.ScheduleBase.GetScheduleById((long)_originId);
                selected!.DeleteSchedule();
            }

            AddScheduleById((long)_originId);

            this.addScheduleButton.Show();
            this.deleteScheduleButton.Show();
            this.reviseScheduleButton.Show();
            this.reviseOK.Hide();
            this.reviseCancel.Hide();
            this._subwindowState = SubwindowState.Viewing;
        }

        abstract protected void AddScheduleById(long id);

        private void ReviseCancel_Click(object sender, EventArgs e)
        {
            ClearInput();
            this.addScheduleButton.Show();
            this.deleteScheduleButton.Show();
            this.reviseScheduleButton.Show();
            this.reviseOK.Hide();
            this.reviseCancel.Hide();
            this._subwindowState = SubwindowState.Viewing;
        }

        protected bool isSearching = false;

        private void SearchOK_Click(object sender, EventArgs e)
        {
            if (this.searchByNameBox.Text.Equals(""))
            {
                MessageBox.Show("请输入要搜索的日程名！");
                return;
            }

            if (showAllData)
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
            if (showAllData)
            {
                GenerateSharedData(_scheduleType);
            }
            else
            {
                GenerateUserData(_scheduleType);
            }
        }


    }

    public sealed class StudentCourseSubwindow : StudentSubwindowBase
    {
        public StudentCourseSubwindow()
            : base(ScheduleType.Course, SubwindowType.Course)
        { }

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
            var converted = list.Select(elem => (Schedule.Course)elem);
            foreach (var course in converted)
            {
                string days = new("");
                string location;
                
                location = course.OfflineLocation?.Name ?? course.OnlineLink!;

                string activeWeeks;
                if (course.RepetitiveType == RepetitiveType.Single)
                {
                    activeWeeks = course.BeginTime.Week.ToString();
                    days = course.BeginTime.Day.ToString();
                }
                else if (course.RepetitiveType == RepetitiveType.MultipleDays)
                {
                    activeWeeks = "1-16";
                    foreach (Day activeDay in course.ActiveDays)
                    {
                        days += activeDay.ToString()[..3] + ";";
                    }
                }
                else
                {
                    activeWeeks = GetBriefWeeks(course.ActiveWeeks).ToString();
                    foreach (Day activeDay in course.ActiveDays)
                    {
                        days += activeDay.ToString()[..3] + ";";
                    }
                }
                scheduleData.Rows.Add(null,
                                          course.Name,
                                          activeWeeks,
                                          days,
                                          course.BeginTime.Hour.ToString() + ":00",
                                          course.Duration.ToString() + "小时",
                                          course.Description ?? "",
                                          location,
                                          course.ScheduleId
                                          );
            }
        }

        protected override void AddScheduleById(long id)
        {
            var selected = Schedule.ScheduleBase.GetSharedById(id);

            if (buildingRadioButton.Checked)
            {
                _ = new Schedule.Course(selected!.RepetitiveType,
                                        selected.Name,
                                        selected.Timestamp,
                                        selected.Duration,
                                        descriptionBox.Text == "" ? null : descriptionBox.Text,
                                        Map.Location.GetBuildingsByName(buildingComboBox.Text)[0],
                                        selected.ActiveWeeks,
                                        selected.ActiveDays,
                                        ScheduleOperationType.UserOpration,
                                        id
                                       );
            }
            else
            {
                _ = new Schedule.Course(
                                        selected!.RepetitiveType,
                                        selected.Name,
                                        selected.Timestamp,
                                        selected.Duration,
                                        descriptionBox.Text == "" ? null : descriptionBox.Text,
                                        linkBox.Text,
                                        selected.ActiveWeeks,
                                        selected.ActiveDays,
                                        ScheduleOperationType.UserOpration,
                                        id
                                       );
            }
        }
    }

    public sealed class StudentExamSubwindow : StudentSubwindowBase
    {
        public StudentExamSubwindow()
            : base(ScheduleType.Exam, SubwindowType.Exam)
        { }

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
            var converted = list.Select(elem => (Schedule.Exam)elem);
            foreach (var exam in converted)
            {
                StringBuilder days = new("");
                string location;
                foreach (Day activeDay in exam.ActiveDays)
                {
                    days.Append(activeDay.ToString()[..3] + ";");
                }
                location = exam.OfflineLocation.Name;

                scheduleData.Rows.Add(null,
                                          exam.Name,
                                          exam.BeginTime.Week.ToString(),
                                          exam.BeginTime.Day.ToString(),
                                          exam.BeginTime.Hour.ToString() + ":00",
                                          exam.Duration.ToString() + "小时",
                                          exam.Description ?? "",
                                          location,
                                          exam.ScheduleId
                                          );
            }
        }

        protected override void AddScheduleById(long id)
        {
            var selected = Schedule.ScheduleBase.GetSharedById(id);

            _ = new Schedule.Exam(  selected!.Name,
                                    selected.Timestamp,
                                    selected.Duration,
                                    descriptionBox.Text == "" ? null : descriptionBox.Text,
                                    Map.Location.GetBuildingsByName(buildingComboBox.Text)[0],
                                    ScheduleOperationType.UserOpration,
                                    id
                                  );
        }
    }

    public sealed class StudentGroupActivitySubwindow : StudentSubwindowBase
    {
        public StudentGroupActivitySubwindow()
            : base(ScheduleType.Activity, SubwindowType.GroupActivity)
        { }

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
            var converted = list.Select(elem => (Schedule.Activity)elem);
            foreach (var activity in converted)
            {
                if (!activity.IsGroupActivity)
                {
                    continue;
                }
                StringBuilder days = new("");
                string location;
                foreach (Day activeDay in activity.ActiveDays)
                {
                    days.Append(activeDay.ToString()[..3] + ";");
                }
                location = activity.OfflineLocation?.Name ?? activity.OnlineLink!;

                string activeWeeks;
                if (activity.RepetitiveType == RepetitiveType.Single)
                {
                    activeWeeks = activity.ActiveWeeks[0].ToString();
                }
                else if (activity.RepetitiveType == RepetitiveType.MultipleDays)
                {
                    activeWeeks = "1-16";
                }
                else
                {
                    activeWeeks = GetBriefWeeks(activity.ActiveWeeks).ToString();
                }
                scheduleData.Rows.Add(null,
                                          activity.Name,
                                          activeWeeks,
                                          days,
                                          activity.BeginTime.Hour.ToString() + ":00",
                                          activity.Duration.ToString() + "小时",
                                          activity.Description ?? "",
                                          location,
                                          activity.ScheduleId
                                          );
            }
        }

        protected override void AddScheduleById(long id)
        {
            var selected = Schedule.ScheduleBase.GetSharedById(id);

            if (buildingRadioButton.Checked)
            {
                _ = new Schedule.Activity(selected!.RepetitiveType,
                                        selected.Name,
                                        selected.Timestamp,
                                        selected.Duration,
                                        descriptionBox.Text == "" ? null : descriptionBox.Text,
                                        Map.Location.GetBuildingsByName(buildingComboBox.Text)[0],
                                        true,
                                        selected.ActiveWeeks,
                                        selected.ActiveDays,
                                        ScheduleOperationType.UserOpration,
                                        id
                                       );
            }
            else
            {
                _ = new Schedule.Activity(
                                        selected!.RepetitiveType,
                                        selected.Name,
                                        selected.Timestamp,
                                        selected.Duration,
                                        descriptionBox.Text == "" ? null : descriptionBox.Text,
                                        linkBox.Text,
                                        true,
                                        selected.ActiveWeeks,
                                        selected.ActiveDays,
                                        ScheduleOperationType.UserOpration,
                                        id
                                       );
            }
        }
    }

    public sealed class StudentPersonalActivitySubwindow : StudentSubwindowBase
    {
        private MultiSelectBox weekSelectBox = new MultiSelectBox();
        private MultiSelectBox daySelectBox = new MultiSelectBox();
        private ComboBox hourComboBox = new ComboBox();
        private ComboBox durationComboBox = new ComboBox();

        public StudentPersonalActivitySubwindow()
            : base(ScheduleType.Activity, SubwindowType.PersonalActivity)
        {
            GenerateUserData(_scheduleType);
            GeneratePersonalActivitySubwindow();
            showAllData = false;
            _subwindowState = SubwindowState.Viewing;
        }

        private void GeneratePersonalActivitySubwindow()
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
            //TODO:整合Days等
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            this.daySelectBox.InitializeBox(days);

            
            hourComboBox.Size = hourBox.Size;
            hourComboBox.BackColor = Color.White;
            hourComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            hourComboBox.FlatStyle = FlatStyle.Flat;
            hourComboBox.ForeColor = SystemColors.WindowText;
            hourComboBox.FormattingEnabled = true;
            hourComboBox.Items.AddRange(new object[] {
            "8:00",
            "9:00",
            "10:00",
            "11:00",
            "12:00",
            "13:00",
            "14:00",
            "15:00",
            "16:00",
            "17:00",
            "18:00",
            "19:00",
            "20:00",
            "21:00"});
            hourComboBox.Location = hourBox.Location;
            hourComboBox.Name = "hourComboBox";
            hourComboBox.Size = hourBox.Size;
            Controls.Add(hourComboBox);

            durationComboBox.BackColor = Color.White;
            durationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            durationComboBox.DropDownWidth = 130;
            durationComboBox.FlatStyle = FlatStyle.Flat;
            durationComboBox.FormattingEnabled = true;
            durationComboBox.Items.AddRange(new object[] {
            "1小时",
            "2小时",
            "3小时"});
            durationComboBox.Location = durationBox.Location;
            durationComboBox.Name = "durationComboBox";
            durationComboBox.Size = durationBox.Size;
            Controls.Add(durationComboBox);

            weekBox.Hide();
            dayBox.Hide();
            hourBox.Hide();
            durationBox.Hide();
            switchData.Hide();
            daySelectBox.BringToFront();
            weekSelectBox.BringToFront();
        }
        
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
            var converted = list.Select(elem => (Schedule.Activity)elem);
            foreach (var activity in converted)
            {
                if (activity.IsGroupActivity)
                {
                    continue;
                }
                string days = new("");
                string location;
                
                location = activity.OfflineLocation?.Name ?? activity.OnlineLink!;

                string activeWeeks;
                if (activity.RepetitiveType == RepetitiveType.Single)
                {
                    activeWeeks = activity.BeginTime.Week.ToString();
                    days = activity.BeginTime.Day.ToString()[..3];
                }
                else if (activity.RepetitiveType == RepetitiveType.MultipleDays)
                {
                    activeWeeks = "1-16";
                    foreach (Day activeDay in activity.ActiveDays)
                    {
                        days = activity.BeginTime.Day.ToString()[..3];
                    }
                }
                else
                {
                    activeWeeks = GetBriefWeeks(activity.ActiveWeeks).ToString();
                    foreach (Day activeDay in activity.ActiveDays)
                    {
                        days = activity.BeginTime.Day.ToString()[..3];
                    }
                }
                scheduleData.Rows.Add(null,
                                          activity.Name,
                                          activeWeeks,
                                          days,
                                          activity.BeginTime.Hour.ToString() + ":00",
                                          activity.Duration.ToString() + "小时",
                                          activity.Description ?? "",
                                          location,
                                          activity.ScheduleId.ToString()
                                          );
            }
        }

        protected override void AddScheduleById(long id)
        {
            AddPersonalSchedule(id);
        }
        
        private bool GetScheduleInfo(bool showMessageBox,
                                       out string name,
                                       out RepetitiveType repetitiveType,
                                       out int[] activeWeeks,
                                       out Day[] activeDays,
                                       out int beginHour,
                                       out int duration//TODO:传出地点及在线链接
                                       )
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
      
        protected override void AddPersonalSchedule(long? id)
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
                                              Map.Location.GetBuildingsByName(buildingComboBox.Text)[0],
                                              false,
                                              Constants.EmptyIntArray,
                                              Constants.EmptyDayArray,
                                              ScheduleOperationType.UserOpration,
                                              id);
                }
                else
                {
                    _ = new Schedule.Activity(RepetitiveType.Single,
                                              name,
                                              new() { Week = activeWeeks[0], Day = activeDays[0], Hour = beginHour },
                                              duration,
                                              descriptionBox.Text == "" ? null : descriptionBox.Text,
                                              linkBox.Text,
                                              false,
                                              Constants.EmptyIntArray,
                                              Constants.EmptyDayArray,
                                              ScheduleOperationType.UserOpration,
                                              id);
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
                                              Map.Location.GetBuildingsByName(buildingComboBox.Text)[0],
                                              false,
                                              Constants.EmptyIntArray,
                                              activeDays,
                                              ScheduleOperationType.UserOpration,
                                              id);
                }
                else
                {
                    _ = new Schedule.Activity(RepetitiveType.MultipleDays,
                                              name,
                                              new() { Hour = beginHour },
                                              duration,
                                              descriptionBox.Text == "" ? null : descriptionBox.Text,
                                              linkBox.Text,
                                              false,
                                              Constants.EmptyIntArray,
                                              activeDays,
                                              ScheduleOperationType.UserOpration,
                                              id);
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
                                              Map.Location.GetBuildingsByName(buildingComboBox.Text)[0],
                                              false,
                                              activeWeeks,
                                              activeDays,
                                              ScheduleOperationType.UserOpration,
                                              id);
                }
                else
                {
                    _ = new Schedule.Activity(RepetitiveType.Designated,
                                              name,
                                              new() { Hour = beginHour },
                                              duration,
                                              descriptionBox.Text == "" ? null : descriptionBox.Text,
                                              linkBox.Text,
                                              false,
                                              activeWeeks,
                                              activeDays,
                                              ScheduleOperationType.UserOpration,
                                              id);
                }
            }
            if (id == null)
            {
                MessageBox.Show("已成功添加该课程");
            }
            GenerateUserData(_scheduleType);
            return;
        }

        protected override void DeletePersonalSchedule()
        {
            DeleteGroupSchedule();
        }

        protected override void RevisePersonalSchedule()
        {
            int selectedCount = 0, index = 0;
            for (int i = 0; i < _userData.Count; i++)
            {
                if (Convert.ToBoolean(scheduleData.Rows[i].Cells[0].EditedFormattedValue))
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
            
            long id = long.Parse(scheduleData.Rows[index].Cells[8].Value.ToString()!);
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
                linkBox.Text = ((Schedule.Activity)selected).OnlineLink;
                linkRadioButton.Checked = true;
            } 
            else
            {
                buildingComboBox.Text = ((Map.Location.Building)((Schedule.Activity)selected).OfflineLocation!).Name; ;
                buildingRadioButton.Checked = true;
            }
            descriptionBox.Text = selected.Description;
            _originId = selected.ScheduleId;
            this.reviseScheduleButton.Hide();
            this.addScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.reviseOK.Show();
            this.reviseCancel.Show();
            _subwindowState = SubwindowState.ReviseUserSchedule;
        }
    }


    public sealed class StudentTemporaryAffairSubwindow : StudentSubwindowBase
    {
        private ComboBox weekSelectBox = new();
        private ComboBox daySelectBox = new();
        private ComboBox hourComboBox = new();
        private string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

        public StudentTemporaryAffairSubwindow()
            : base(ScheduleType.TemporaryAffair, SubwindowType.TemporaryAffair)
        {
            GenerateUserData(_scheduleType);
            GenerateTemporaryAffairSubwindow();
            showAllData = false;
            _subwindowState = SubwindowState.Viewing;
        }

        private void GenerateTemporaryAffairSubwindow()
        {
            hourComboBox.BackColor = Color.White;
            hourComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            hourComboBox.DropDownWidth = 130;
            hourComboBox.FlatStyle = FlatStyle.Flat;
            hourComboBox.FormattingEnabled = true;
            hourComboBox.Items.AddRange(new object[] {
            "8:00",
            "9:00",
            "10:00",
            "11:00",
            "12:00",
            "13:00",
            "14:00",
            "15:00",
            "16:00",
            "17:00",
            "18:00",
            "19:00",
            "20:00",
            "21:00"});
            hourComboBox.Location = hourBox.Location;
            hourComboBox.Name = "hourComboBox";
            hourComboBox.Size = weekBox.Size;
            Controls.Add(hourComboBox);

            weekSelectBox.BackColor = Color.White;
            weekSelectBox.DropDownStyle = ComboBoxStyle.DropDownList;
            weekSelectBox.DropDownWidth = 130;
            weekSelectBox.FlatStyle = FlatStyle.Flat;
            weekSelectBox.FormattingEnabled = true;
            weekSelectBox.Items.AddRange(Shared.Weeks);
            weekSelectBox.Location = weekBox.Location;
            weekSelectBox.Name = "weekSelectBox";
            weekSelectBox.Size = weekBox.Size;
            Controls.Add(weekSelectBox);

            daySelectBox.BackColor = Color.White;
            daySelectBox.DropDownStyle = ComboBoxStyle.DropDownList;
            daySelectBox.DropDownWidth = 130;
            daySelectBox.FlatStyle = FlatStyle.Flat;
            daySelectBox.FormattingEnabled = true;
            daySelectBox.Items.AddRange(days);
            daySelectBox.Location = dayBox.Location;
            daySelectBox.Name = "daySelectBox";
            daySelectBox.Size = dayBox.Size;
            Controls.Add(daySelectBox);

            weekBox.Hide();
            dayBox.Hide();
            hourBox.Hide();
            durationBox.Hide();
            switchData.Hide();
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
                                          ((Map.Location.Building)(affair.OfflineLocation!)).Name,
                                          affair.ScheduleId.ToString()
                                          );
            }
        }

        protected override void AddScheduleById(long id)
        {
            AddPersonalSchedule(id);
        }

        protected override void AddPersonalSchedule(long? id)
        {
            StringBuilder errorMessage = new();
            int activeWeek;
            Day activeDay = Day.Monday;
            int beginHour;
            string buildingName;

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
            if (weekSelectBox.Text.Equals(""))
            {
                errorMessage.AppendLine("请输入日程周！");
            }
            if (daySelectBox.Text.Equals(""))
            {
                errorMessage.AppendLine("请输入日程日！");
            }
            if (hourComboBox.Text == "")
            {
                errorMessage.AppendLine("请输入日程时间！");
            }
            if (!errorMessage.Equals(""))
            {
                MessageBox.Show(errorMessage.ToString());
                return;
            }

            string name = nameBox.Text;

            activeWeek = int.Parse(weekSelectBox.Text[4..]);
            for(int i = 0; i < days.Length; i++)
            {
                if (days[i].Equals(daySelectBox.Text))
                {
                    activeDay = (Day)i;
                    break;
                }
            }

            if (hourComboBox.Text.Length == 5)
            {
                beginHour = (hourComboBox.Text[0] - '0') * 10 + hourComboBox.Text[1] - '0';
            }
            else
            {
                beginHour = hourComboBox.Text[0] - '0';
            }

            Times.Time beginTime = new() { Week = activeWeek, Day = activeDay, Hour = beginHour };


            if (MessageBox.Show("", "确认日程信息", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _ = new Schedule.TemporaryAffairs(name, beginTime, descriptionBox.Text == "" ? null : descriptionBox.Text, Map.Location.Buildings[0]);
                GenerateUserData(_scheduleType);
                return;
            }
        }

        protected override void DeletePersonalSchedule()
        {
            DeleteGroupSchedule();
        }

        protected override void RevisePersonalSchedule()
        {
            int selectedCount = 0, index = 0;
            for (int i = 0; i < _userData.Count; i++)
            {
                if (Convert.ToBoolean(scheduleData.Rows[i].Cells[0].EditedFormattedValue))
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

            long id = long.Parse(scheduleData.Rows[index].Cells[8].Value.ToString()!);
            var selected = Schedule.ScheduleBase.GetScheduleById(id);

            nameBox.Text = selected!.Name;
            weekSelectBox.Text = "Week" + selected.BeginTime.Week;
            daySelectBox.Text = days[(int)selected.BeginTime.Day];
            descriptionBox.Text = selected.Description ?? "";
            hourComboBox.Text = selected.BeginTime.Hour + ":00";
            buildingComboBox.Text = ((Map.Location.Building)((Schedule.TemporaryAffairs)selected).OfflineLocation).Name; ;
            _originId = selected.ScheduleId;
            this.reviseScheduleButton.Hide();
            this.addScheduleButton.Hide();
            this.deleteScheduleButton.Hide();
            this.reviseOK.Show();
            this.reviseCancel.Show();
            _subwindowState = SubwindowState.ReviseUserSchedule;
        }
    }
}

