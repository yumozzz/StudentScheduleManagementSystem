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
    public partial class StudentScheduleTable : Form
    {
        public delegate void RefreshTimeTableDelegate(Times.Time time);

        private RefreshTimeTableDelegate _resreshTimeTableDelegate;

        public StudentScheduleTable()
        {
            InitializeComponent();
            Times.Timer.TimeChange += RefreshScheduleTable;
            GenerateScheduleTable(1);
        }

        private int previousWeek = 0;
        private void RefreshScheduleTable(Times.Time time)
        {
            if (scheduleTable.InvokeRequired)
            {
                _resreshTimeTableDelegate = new(RefreshScheduleTable);
                this.scheduleTable.Invoke(RefreshScheduleTable, time);
                return;
            }

            if (time.Week != previousWeek)
            {
                GenerateScheduleTable(time.Week);
            }
        }

        private void GenerateScheduleTable(int week)
        {
            scheduleTable.Rows.Clear();
            int[] widths = { 70, 100, 100, 100, 100, 100, 100, 100 };
            for (int i = 0; i < widths.Length; i++)
            {
                scheduleTable.Columns[i].Width = widths[i];
            }

            int offset = (week - 1) * 7 * 24;
            
            for (int i = offset + 7; i < offset + 21; i++)
            {
                string[] scheduleRecords = new string[7];
                for (int j = 0; j < 7; j++)
                {
                    scheduleRecords[j] = "";
                    long id = Schedule.ScheduleBase.GetRecordAt(i + j * 24).Id;
                    if (id != 0)
                    {
                        Schedule.ScheduleBase? scheduleRecord = Schedule.ScheduleBase.GetScheduleById(id);
                        if (scheduleRecord != null)
                        {
                            scheduleRecords[j] = scheduleRecord.Name + "\n" + scheduleRecord.Description;
                        }
                    }
                }
                this.scheduleTable.Rows.Add((i - offset + 1).ToString() + ":00",
                                            scheduleRecords[0],
                                            scheduleRecords[1],
                                            scheduleRecords[2],
                                            scheduleRecords[3],
                                            scheduleRecords[4],
                                            scheduleRecords[5],
                                            scheduleRecords[6]);
                this.scheduleTable.Rows[i - offset - 7].Height = 100;
            }
        }
    }
}
