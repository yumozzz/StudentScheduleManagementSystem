using System.Diagnostics;

namespace StudentScheduleManagementSystem.UI
{
    /// <summary>
    /// 日程表类，提供表格生成的相关方法
    /// </summary>
    public partial class StudentScheduleTable : Form
    {
        private int _displayedWeek = 1;
        private long _selectedId;
        public StudentWindow MainWindow { get; init; }

        public StudentScheduleTable()
        {
            InitializeComponent();
            Times.Timer.TimeChange += RefreshScheduleTable;
            GenerateScheduleTable(1);
        }

        /// <summary>
        /// 若<paramref name="time"/>是某一周的第一个小时，则刷新表格
        /// </summary>
        private void RefreshScheduleTable(Times.Time time)
        {
            if (scheduleTable.InvokeRequired)
            {
                this.scheduleTable.Invoke(RefreshScheduleTable, time);
            }
            else if (time is { Day : Day.Monday, Hour : 0 })
            {
                GenerateScheduleTable(time.Week);
            }
        }

        /// <summary>
        /// 按周数<paramref name="week"/>刷新表格
        /// </summary>
        private void GenerateScheduleTable(int week)
        {
            showWeekLabel.Text = $"第 {week.ToString()} 周";
            scheduleTable.Rows.Clear();
            int[] widths = { 70, 120, 120, 120, 120, 120, 120, 120 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleTable.Columns[i].Width = widths[i];
            }

            int offset = (week - 1) * 7 * 24;

            for (int i = offset + 5; i < offset + 21; i++)
            {
                string[] scheduleRecords = new string[7];
                bool[] alarmEnabled = new bool[7];
                for (int j = 0; j < 7; j++)
                {
                    scheduleRecords[j] = "";
                    long id = Schedule.Schedule.GetRecordAt(i + 1 + j * 24).Id;
                    if (id == 0)
                    {
                        continue;
                    }
                    Schedule.Schedule? scheduleRecord = Schedule.Schedule.GetScheduleById(id);
                    string scheduleName;
                    string description;
                    if (scheduleRecord!.ScheduleType == ScheduleType.TemporaryAffair)
                    {
                        scheduleName = Array
                                      .ConvertAll(Schedule.TemporaryAffair.GetAllAt(scheduleRecord.BeginTime),
                                                  affair => affair.Name)
                                      .Aggregate((str, elem) => str = str + "、\n" + elem);
                        description = "";
                    }
                    else
                    {
                        scheduleName = scheduleRecord.Name;
                        description = scheduleRecord.Description!;
                    }
                    Debug.Assert(scheduleRecord != null);
                    string typeName = scheduleRecord.ScheduleType switch
                    {
                        ScheduleType.Course => "课程", ScheduleType.Exam => "考试", ScheduleType.Activity => "活动",
                        ScheduleType.TemporaryAffair => "临时事务",
                        _ => throw new ArgumentException(null, nameof(scheduleRecord.ScheduleType)),
                    };
                    scheduleRecords[j] = scheduleName + $"\n（{typeName}）\n" +
                                         description;
                    alarmEnabled[j] = scheduleRecord.AlarmEnabled;
                }
                this.scheduleTable.Rows.Add($"{(i - offset + 1).ToString()}:00\n-\n{(i - offset + 2).ToString()}:00",
                                            scheduleRecords[0],
                                            scheduleRecords[1],
                                            scheduleRecords[2],
                                            scheduleRecords[3],
                                            scheduleRecords[4],
                                            scheduleRecords[5],
                                            scheduleRecords[6]);
                this.scheduleTable.Rows[i - offset - 5].Height = 150;
                for(int j = 0; j < 7; j++)
                {
                    if (alarmEnabled[j])
                    {
                        this.scheduleTable.Rows[i - offset - 5].Cells[j + 1].Style.ForeColor = Color.FromArgb(50, 168, 128, 194);
                    }
                }
            }
        }

        private void ThisWeekScheduleTable_Click(object sender, EventArgs e)
        {
            _displayedWeek = Times.Timer.Now.Week;
            GenerateScheduleTable(_displayedWeek);
        }

        /// <summary>
        /// 用户点击上一周按钮
        /// </summary>
        private void LastWeekScheduleTable_Click(object sender, EventArgs e)
        {
            if (_displayedWeek - 1 > 0)
            {
                _displayedWeek--;
                GenerateScheduleTable(_displayedWeek);
            }
            else
            {
                MessageBox.Show("已经是最早的一周！");
                return;
            }
        }

        /// <summary>
        /// 用户点击下一周按钮
        /// </summary>
        private void NextWeekScheduleTable_Click(object sender, EventArgs e)
        {
            if (_displayedWeek + 1 <= 16)
            {
                _displayedWeek++;
                GenerateScheduleTable(_displayedWeek);
            }
            else
            {
                MessageBox.Show("已经是最晚的一周！");
                return;
            }
        }

        /// <summary>
        /// 双击单元格，转到对应日程
        /// </summary>
        private void ScheduleTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex > 0)
            {
                int doubleClickCell = (_displayedWeek - 1) * 7 * 24 + (e.ColumnIndex - 1) * 24 + e.RowIndex + 6;
                _selectedId = Schedule.Schedule.GetRecordAt(doubleClickCell).Id;
                if (_selectedId == 0)
                {
                    return;
                }
                Schedule.Schedule selected = Schedule.Schedule.GetScheduleById(_selectedId)!;
                ScheduleType scheduleType = selected!.ScheduleType;
                switch (scheduleType)
                {
                    case ScheduleType.Course:
                        MainWindow.CourseButton_Click(this, EventArgs.Empty);
                        MainWindow.StudentCourseSubwindow!.SelectScheduleWithId(_selectedId);
                        break;
                    case ScheduleType.Exam:
                        MainWindow.ExamButton_Click(this, EventArgs.Empty);
                        MainWindow.StudentExamSubwindow!.SelectScheduleWithId(_selectedId);
                        break;
                    case ScheduleType.Activity:
                        Schedule.Activity activity = (Schedule.Activity)selected;
                        if (activity.IsGroupActivity)
                        {
                            MainWindow.GroupActivityButton_Click(this, EventArgs.Empty);
                            MainWindow.StudentGroupActivitySubwindow!.SelectScheduleWithId(_selectedId);
                        }
                        else
                        {
                            MainWindow.PersonalActivityButton_Click(this, EventArgs.Empty);
                            MainWindow.StudentPersonalActivitySubwindow!.SelectScheduleWithId(_selectedId);
                        }
                        break;
                    case ScheduleType.TemporaryAffair:
                        MainWindow.TemporaryAffairButton_Click(this, EventArgs.Empty);
                        MainWindow.StudentTemporaryAffairSubwindow!.SelectScheduleWithId(_selectedId);
                        break;
                }
            }
        }
    }
}