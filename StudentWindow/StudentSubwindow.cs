using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentScheduleManagementSystem.UI
{
    public abstract partial class StudentSubwindowBase : Form
    {
        protected List<Schedule.ScheduleBase.SharedData> _data;
        protected List<Schedule.ScheduleBase> _myData;
        protected ScheduleType _type;
        protected long? _originId = null;

        private StudentSubwindowBase()
            : this(ScheduleType.Idle) { }
        protected StudentSubwindowBase(ScheduleType type)
        {
            InitializeComponent();
            GenerateSharedData(type);
            _type = type;
            for(int i = 0; i < Map.Location.Buildings.Count; i++)
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

        bool showAllData = true;

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
                    this.scheduleData.Rows.Add( null,
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
                    this.scheduleData.Rows.Add( null,
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
                    this.scheduleData.Rows.Add( null,
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

            linkBox.Text = scheduleData.Rows[index].Cells[8].Value.ToString();
            long id = long.Parse(scheduleData.Rows[index].Cells[8].Value.ToString());

            var selected = Schedule.ScheduleBase.GetSharedById(id);

            bool willCollide = Schedule.ScheduleBase.DetectCollision(
                                                                     selected!.RepetitiveType,
                                                                     selected.ScheduleType,
                                                                     selected.Timestamp,
                                                                     selected.Duration,
                                                                     selected.ActiveWeeks,
                                                                     selected.ActiveDays,
                                                                     out RepetitiveType collisionRepType,
                                                                     out ScheduleType collisionSchType
                                                                     );

            if (willCollide)
            {
                MessageBox.Show("该日程与您的类型为" + collisionSchType.ToString() + "的日程冲突");
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
            long id = DetectCollision(false);
            if (id == 0)
            {
                return;
            }
            else
            {
                _originId = id;
                var selected = Schedule.ScheduleBase.GetSharedById(id);
                ClearInput();
                this.nameBox.Text = selected!.Name;
                if(selected.RepetitiveType == RepetitiveType.Single)
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

            }
        }

        private void DeleteScheduleButton_Click(object sender, EventArgs e)
        {

        }

        private void ReviseScheduleButton_Click(object sender, EventArgs e)
        {

        }

        private void ReviseOK_Click(object sender, EventArgs e)
        {
            //TODO 确认修改、删除、添加
        }

        private void ReviseCancel_Click(object sender, EventArgs e)
        {
            //TODO 取消
        }

        private void SearchOK_Click(object sender, EventArgs e)
        {

        }

        private void SearchCancel_Click(object sender, EventArgs e)
        {

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
            foreach(var course in converted)
            {
                StringBuilder days = new("");
                string location;
                foreach (Day activeDay in course.ActiveDays)
                {
                    days.Append(activeDay.ToString()[..3] + ";");
                }
                if(course.OfflineLocation != null)
                {
                    location = course.OfflineLocation.ToString();
                }
                else
                {
                    location = course.OnlineLink!.ToString();
                }
                scheduleData.Rows.Add(
                                      course.Name,
                                      GetBriefWeeks(course.ActiveWeeks),
                                      days, 
                                      course.BeginTime.ToString(),
                                      course.Duration.ToString(),
                                      course.Description,
                                      location
                                      );
                
            }
        }
    }
}
