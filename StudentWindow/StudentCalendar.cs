using System.Diagnostics;

namespace StudentScheduleManagementSystem.UI
{
    public partial class StudentScheduleTable : Form
    {
        private int currentWeek = 0;
        private int showWeek = 1;

        public StudentScheduleTable()
        {
            InitializeComponent();
            Times.Timer.TimeChange += RefreshScheduleTable;
            GenerateScheduleTable(1);
        }

        private void RefreshScheduleTable(Times.Time time)
        {
            if (scheduleTable.InvokeRequired)
            {
                this.scheduleTable.Invoke(RefreshScheduleTable, time);
            }
            else if (time is { Day : Day.Monday, Hour : 0})
            {
                GenerateScheduleTable(time.Week);
            }
        }

        private void GenerateScheduleTable(int week)
        {
            showWeekLabel.Text = "第 " + week.ToString() + " 周";
            scheduleTable.Rows.Clear();
            int[] widths = { 70, 120, 120, 120, 120, 120, 120, 120 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleTable.Columns[i].Width = widths[i];
            }

            int offset = (week - 1) * 7 * 24;
            
            for (int i = offset + 7; i < offset + 21; i++)
            {
                string[] scheduleRecords = new string[7];
                bool[] alarmEnalbled = new bool[7];
                for (int j = 0; j < 7; j++)
                {
                    scheduleRecords[j] = "";
                    long id = Schedule.ScheduleBase.GetRecordAt(i + j * 24).Id;
                    if (id == 0)
                    {
                        continue;
                    }
                    Schedule.ScheduleBase? scheduleRecord = Schedule.ScheduleBase.GetScheduleById(id);
                    Debug.Assert(scheduleRecord != null);
                    scheduleRecords[j] = scheduleRecord.Name + "\n（" + TranslateScheduleType(scheduleRecord.ScheduleType) + "）" + "\n" + scheduleRecord.Description;
                    alarmEnalbled[j] = scheduleRecord.AlarmEnabled;
                }
                this.scheduleTable.Rows.Add((i - offset + 1).ToString() + ":00\n-\n" + (i - offset + 2).ToString() + ":00",
                                            scheduleRecords[0],
                                            scheduleRecords[1],
                                            scheduleRecords[2],
                                            scheduleRecords[3],
                                            scheduleRecords[4],
                                            scheduleRecords[5],
                                            scheduleRecords[6]);
                this.scheduleTable.Rows[i - offset - 7].Height = 100;
                for(int j = 0; j < 7; j++)
                {
                    if (alarmEnalbled[j])
                    {
                        this.scheduleTable.Rows[i - offset - 7].Cells[j + 1].Style.BackColor = Color.FromArgb(50, 168, 128, 194);
                    }
                }
            }
        }

        string TranslateScheduleType(ScheduleType scheduleType)
        {
            switch (scheduleType)
            {
                case ScheduleType.Course:
                    return "课程";
                case ScheduleType.Exam:
                    return "考试";
                case ScheduleType.Activity:
                    return "活动";
                case ScheduleType.TemporaryAffair:
                    return "临时事务";
            }
            return "未知";
        }

        private void ThisWeekScheduleTable_Click(object sender, EventArgs e)
        {
            showWeek = Times.Timer.Now.Week;
            GenerateScheduleTable(showWeek);
        }

        private void LastWeekScheduleTable_Click(object sender, EventArgs e)
        {
            if (showWeek - 1 > 0)
            {
                showWeek--;
                GenerateScheduleTable(showWeek);
            }
            else
            {
                MessageBox.Show("已经是最早的一周！");
                return;
            }
        }

        private void NextWeekScheduleTable_Click(object sender, EventArgs e)
        {
            if (showWeek + 1 <= 16)
            {
                showWeek++;
                GenerateScheduleTable(showWeek);
            }
            else
            {
                MessageBox.Show("已经是最晚的一周！");
                return;
            }
        }
    }


}
