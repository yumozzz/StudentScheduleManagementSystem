using StudentScheduleManagementSystem.Schedule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentScheduleManagementSystem
{
    public partial class AdminWindowExam : Form
    {
        public AdminWindowExam()
        {
            InitializeComponent();
            Generate_Data();
        }

        private List<Schedule.ScheduleBase.SharedData> Data = null;
        public void Generate_Data()
        {
            ExamData.Rows.Clear();
            int[] widths = { 30, 150, 150, 150, 150, 100, 100 };
            for (int i = 0; i < widths.Length; i++)
            {
                ExamData.Columns[i].Width = widths[i];
            }
            Data = Schedule.ScheduleBase.GetSharedByType(ScheduleType.Exam);
            for (int i = 1; i < Data.Count(); i++)
            {
                this.ExamData.Rows.Add(null,
                                       Data[i].Name,
                                       Data[i].Id,
                                       Data[i].Timestamp.Week.ToString(),
                                       Data[i].Timestamp.Day.ToString(),
                                       Data[i].Timestamp.Hour.ToString() + ":00",
                                       Data[i].Duration.ToString() + "小时");
            }
        }

        private void AddExam_Click(object sender, EventArgs e)
        {
            if (reviseExam)
            {
                MessageBox.Show("请先修改课程！");
                return;
            }
            AddOneExam(null);
        }

        private Boolean AddOneExam(long? ID){
            StringBuilder ErrorMessage = new StringBuilder("");

            if (NameBox.Text.Equals(""))
            {
                ErrorMessage.AppendLine("请输入考试科目！");
            }
            if (WeekcomboBox.Text.Equals(""))
            {
                ErrorMessage.AppendLine("请输入考试周！");
            }
            if (DaycomboBox.Text.Equals(""))
            {
                ErrorMessage.AppendLine("请输入考试日！");
            }
            if (HourcomboBox.Text == "")
            {
                ErrorMessage.AppendLine("请输入考试时间！");
            }
            if (DurcomboBox.Text == "")
            {
                ErrorMessage.AppendLine("请输入考试时长！");
            }
            if (!ErrorMessage.Equals(""))
            {
                MessageBox.Show(ErrorMessage.ToString());
                return false;
            }

            int activeWeek = 0;
            if (WeekcomboBox.Text.Length == 5)
            {
                activeWeek = WeekcomboBox.Text[4] - '0';
            }
            else
            {
                activeWeek = (WeekcomboBox.Text[4] - '0') * 10 + WeekcomboBox.Text[5];
            }

            Day activeDay = Day.Monday;
            for (int i = 0; i < Constants.AllDays.Length; i++)
            {
                if (DaycomboBox.Text.Equals(Constants.AllDays[i].ToString()))
                {
                    activeDay = Constants.AllDays[i];
                    break;
                }
            }

            int beginhour;
            int duration = DurcomboBox.Text[0] - '0';

            if (HourcomboBox.Text.Length == 5)
            {
                beginhour = (HourcomboBox.Text[0] - '0') * 10 + HourcomboBox.Text[1] - '0';
            }
            else
            {
                beginhour = HourcomboBox.Text[0] - '0';
            }

            //Double check
            StringBuilder examDetail = new StringBuilder("");
            examDetail.Append("考试周：" + WeekcomboBox.Text + "\n考试日：" + DaycomboBox.Text +
                              "\n时间: " + HourcomboBox.Text + "\n时长: " + DurcomboBox.Text +
                              "\n考试科目：" + NameBox.Text + "\nID: " + ID.ToString());

            if (MessageBox.Show(examDetail.ToString(), "考试信息", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    Schedule.Exam exam = new(NameBox.Text,
                                             new() { Week = activeWeek, Day = activeDay, Hour = beginhour },
                                             duration,
                                             null,
                                             new Map.Location.Building(1,
                                                                       "default building",
                                                                       new() { Id = -1, X = 0, Y = 0 }),
                                             ID,
                                             addOnTimeline: false);

                    MessageBox.Show("已成功添加该考试");
                    Generate_Data();

                    this.NameBox.Text = "";
                    this.WeekcomboBox.Text = "";
                    this.DaycomboBox.Text = "";
                    this.HourcomboBox.Text = "";
                    this.DurcomboBox.Text = "";

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Log.Error.Log(null, ex);
                }
            }
            else
            {
                MessageBox.Show("已取消添加该考试");
            }
            return false;
        }

        private void DeleteExam_Click(object sender, EventArgs e)
        {
            if (reviseExam)
            {
                MessageBox.Show("请先修改课程！");
                return;
            }
            int index = FindValidIndex();
            if (index != -1)
            {
                DeleteOneExam(index);
            }
        }

        private long DeleteOneExam(int index)
        {
            StringBuilder examDetail = UI.MainWindow.GenerateScheduleDetail(Data[index]);

            if (MessageBox.Show(examDetail.ToString(), "考试信息", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    long ID = Data[index].Id;
                    ScheduleBase.DeleteShared(ID);
                    MessageBox.Show("已成功删除该考试");
                    Generate_Data();
                    return ID;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Log.Error.Log(null, ex);
                }
            }
            else
            {
                MessageBox.Show("已取消删除该考试");
            }
            return 0;
        }

        private int FindValidIndex()
        {
            int cnt = 0, index = -1;
            for (int i = 1; i < Data.Count(); i++)
            {
                if (Convert.ToBoolean(ExamData.Rows[i - 1].Cells[0].EditedFormattedValue))
                {
                    cnt++;
                    index = i;
                }
            }
            if (cnt == 0)
            {
                index = -1;
                MessageBox.Show("请选择要删除的考试！");
            }
            else if (cnt >= 2)
            {
                index = -1;
                MessageBox.Show("请一次选择一个考试删除！");
            }
            return index;
        }

        private Boolean reviseExam = false;
        private long tempID;
        private void ReviseExam_Click(object sender, EventArgs e)
        {
            if (!reviseExam)
            {
                int index = FindValidIndex();
                if(index != -1)
                {
                    Schedule.ScheduleBase.SharedData data = Data[index];
                    tempID =  DeleteOneExam(index);

                    if (tempID != 0)
                    {
                        this.NameBox.Text = data.Name;
                        this.WeekcomboBox.Text = "Week" + data.Timestamp.Week.ToString();
                        this.DaycomboBox.Text = data.Timestamp.Day.ToString();
                        this.HourcomboBox.Text = data.Timestamp.Hour.ToString() + ":00";
                        this.DurcomboBox.Text = data.Duration.ToString() + "小时";
                        reviseExam = true;
                        return;
                    }
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (AddOneExam(tempID))
                {
                    reviseExam = false;
                }
                return;
            }
        }
    }
}
