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
    public partial class StudentSubwindowBase : Form
    {
        protected List<Schedule.ScheduleBase.SharedData> _data;
        protected ScheduleType _type;

        private StudentSubwindowBase()
            : this(ScheduleType.Idle) { }
        protected StudentSubwindowBase(ScheduleType type)
        {
            InitializeComponent();
            GenerateAllData(type);
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

        protected void GenerateAllData(ScheduleType type)
        {
            _data = Schedule.ScheduleBase.GetSharedByType(type);
            GenerateAllData(_data);
        }

        protected void GenerateAllData(List<Schedule.ScheduleBase.SharedData> data)
        {
            allScheduleData.Rows.Clear();
            int[] widths = { 30, 100, 150, 130, 120, 130, 60, 60 };
            for (int i = 0; i < widths.Length; i++)
            {
                allScheduleData.Columns[i].Width = widths[i];
            }

            foreach (var sharedData in data)
            {
                if (sharedData.RepetitiveType == RepetitiveType.Single)
                {
                    this.allScheduleData.Rows.Add( null,
                                                   "检测冲突",
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
                    this.allScheduleData.Rows.Add( null,
                                                   "检测冲突",
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
                    this.allScheduleData.Rows.Add( null,
                                                   "检测冲突",
                                                   sharedData.Name,
                                                   sharedData.Id,
                                                   GetBriefWeeks(sharedData.ActiveWeeks).ToString(),
                                                   days.ToString(),
                                                   sharedData.Timestamp.Hour.ToString() + ":00",
                                                   sharedData.Duration.ToString() + "小时");
                }
            }
        }

        private void DetectCollision_Click(object sender, EventArgs e)
        {
            //TODO:检测是否只选择了一个

            bool collide =
        }
    }



    public sealed class StudentCourseSubwindow : StudentSubwindowBase
    {
        public StudentCourseSubwindow()
            : base(ScheduleType.Course)
        {

        }
    }
}
