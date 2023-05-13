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
        protected List<Schedule.ScheduleBase> _myData;
        protected ScheduleType _type;
        protected long? _originId = null;
        protected int studentSubwindowState = Constants.NormalState;
        protected bool showAllData = true;

        private StudentSubwindowBase()
            : this(ScheduleType.Idle) { }
        protected StudentSubwindowBase(ScheduleType type)
        {
            InitializeComponent();
            GenerateSharedData(type);
            _type = type;
            for (int i = 0; i < Map.Location.Buildings.Count; i++)
            {
                buildingComboBox.Items.Add(Map.Location.Buildings[i].Name);
            }
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
                GenerateUserData(_type);
            }
            else
            {
                showAllData = true;
                GenerateSharedData(_type);
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
            _myData = Schedule.ScheduleBase.GetScheduleByType(type);

            GenerateUserData(_myData);
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

            studentSubwindowState = Constants.AddUserScheduleState;

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

            studentSubwindowState = Constants.AddUserScheduleState;
        }

        private void DeleteScheduleButton_Click(object sender, EventArgs e)
        {
            if (showAllData)
            {
                MessageBox.Show("请在私有日程页面选择日程删除！");
                return;
            }

            int selectedCount = 0, index = 0;
            for (int i = 0; i < _myData.Count; i++)
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
                                "周次: " + scheduleData.Rows[index].Cells[3].Value.ToString() +
                                "\n天次: " + scheduleData.Rows[index].Cells[4].Value.ToString() +
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
            GenerateUserData(_type);
        }

        private void ReviseScheduleButton_Click(object sender, EventArgs e)
        {
            if (showAllData)
            {
                MessageBox.Show("请在个人日程页面选择日程修改！");
                return;
            }

            int selectedCount = 0, index = 0;
            for (int i = 0; i < _myData.Count; i++)
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

            studentSubwindowState = Constants.ReviseUserScheduleState;
        }

        private void ReviseOK_Click(object sender, EventArgs e)
        {
            if (_originId == null)
            {
                return;
            }

            if (studentSubwindowState == Constants.ReviseUserScheduleState)
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
                GenerateSharedData(_type);
            }
            else
            {
                GenerateUserData(_type);
            }
        }


    }

    public sealed class StudentCourseSubwindow : StudentSubwindowBase
    {
        public StudentCourseSubwindow()
            : base(ScheduleType.Course)
        {

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
                                        Map.Location.Buildings[1],
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
            : base(ScheduleType.Exam)
        {

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
                                    Map.Location.Buildings[1],
                                    ScheduleOperationType.UserOpration,
                                    id
                                  );
        }
    }

    public sealed class StudentGroupActivitySubwindow : StudentSubwindowBase
    {
        public StudentGroupActivitySubwindow()
            : base(ScheduleType.Activity)
        {

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
                                        Map.Location.Buildings[1],
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
        public StudentPersonalActivitySubwindow()
            : base(ScheduleType.Activity)
        {
            GenerateUserData(_type);
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
            //TODO
        }
    }
}

