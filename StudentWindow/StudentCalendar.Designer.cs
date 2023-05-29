namespace StudentScheduleManagementSystem.UI
{
    partial class StudentScheduleTable : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Times.Timer.TimeChange -= RefreshScheduleTable;
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.scheduleTable = new System.Windows.Forms.DataGridView();
            this.hourColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mondayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tuesdayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wednesdayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thursdayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fridayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saturdayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sundayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nextWeekScheduleTable = new System.Windows.Forms.Button();
            this.thisWeekScheduleTable = new System.Windows.Forms.Button();
            this.lastWeekScheduleTable = new System.Windows.Forms.Button();
            this.showWeekLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleTable)).BeginInit();
            this.SuspendLayout();
            // 
            // scheduleTable
            // 
            this.scheduleTable.AllowUserToAddRows = false;
            this.scheduleTable.AllowUserToResizeColumns = false;
            this.scheduleTable.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scheduleTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.scheduleTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.scheduleTable.BackgroundColor = System.Drawing.Color.White;
            this.scheduleTable.ColumnHeadersHeight = 34;
            this.scheduleTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.scheduleTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hourColumn,
            this.mondayColumn,
            this.tuesdayColumn,
            this.wednesdayColumn,
            this.thursdayColumn,
            this.fridayColumn,
            this.saturdayColumn,
            this.sundayColumn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scheduleTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.scheduleTable.Location = new System.Drawing.Point(12, 42);
            this.scheduleTable.Name = "scheduleTable";
            this.scheduleTable.ReadOnly = true;
            this.scheduleTable.RowHeadersVisible = false;
            this.scheduleTable.RowHeadersWidth = 62;
            this.scheduleTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scheduleTable.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.scheduleTable.RowTemplate.Height = 32;
            this.scheduleTable.Size = new System.Drawing.Size(1026, 601);
            this.scheduleTable.TabIndex = 42;
            this.scheduleTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ScheduleTable_CellDoubleClick);
            // 
            // hourColumn
            // 
            this.hourColumn.HeaderText = "时间";
            this.hourColumn.MinimumWidth = 8;
            this.hourColumn.Name = "hourColumn";
            this.hourColumn.ReadOnly = true;
            this.hourColumn.Width = 82;
            // 
            // mondayColumn
            // 
            this.mondayColumn.HeaderText = "Monday";
            this.mondayColumn.MinimumWidth = 8;
            this.mondayColumn.Name = "mondayColumn";
            this.mondayColumn.ReadOnly = true;
            this.mondayColumn.Width = 118;
            // 
            // tuesdayColumn
            // 
            this.tuesdayColumn.HeaderText = "Tuesday";
            this.tuesdayColumn.MinimumWidth = 8;
            this.tuesdayColumn.Name = "tuesdayColumn";
            this.tuesdayColumn.ReadOnly = true;
            this.tuesdayColumn.Width = 117;
            // 
            // wednesdayColumn
            // 
            this.wednesdayColumn.HeaderText = "Wednesday";
            this.wednesdayColumn.MinimumWidth = 8;
            this.wednesdayColumn.Name = "wednesdayColumn";
            this.wednesdayColumn.ReadOnly = true;
            this.wednesdayColumn.Width = 147;
            // 
            // thursdayColumn
            // 
            this.thursdayColumn.HeaderText = "Thursday";
            this.thursdayColumn.MinimumWidth = 8;
            this.thursdayColumn.Name = "thursdayColumn";
            this.thursdayColumn.ReadOnly = true;
            this.thursdayColumn.Width = 125;
            // 
            // fridayColumn
            // 
            this.fridayColumn.HeaderText = "Friday";
            this.fridayColumn.MinimumWidth = 8;
            this.fridayColumn.Name = "fridayColumn";
            this.fridayColumn.ReadOnly = true;
            this.fridayColumn.Width = 150;
            // 
            // saturdayColumn
            // 
            this.saturdayColumn.HeaderText = "Saturday";
            this.saturdayColumn.MinimumWidth = 8;
            this.saturdayColumn.Name = "saturdayColumn";
            this.saturdayColumn.ReadOnly = true;
            this.saturdayColumn.Width = 123;
            // 
            // sundayColumn
            // 
            this.sundayColumn.HeaderText = "Sunday";
            this.sundayColumn.MinimumWidth = 8;
            this.sundayColumn.Name = "sundayColumn";
            this.sundayColumn.ReadOnly = true;
            this.sundayColumn.Width = 110;
            // 
            // nextWeekScheduleTable
            // 
            this.nextWeekScheduleTable.BackColor = System.Drawing.Color.White;
            this.nextWeekScheduleTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextWeekScheduleTable.Location = new System.Drawing.Point(926, 4);
            this.nextWeekScheduleTable.Name = "nextWeekScheduleTable";
            this.nextWeekScheduleTable.Size = new System.Drawing.Size(112, 34);
            this.nextWeekScheduleTable.TabIndex = 43;
            this.nextWeekScheduleTable.Text = ">>";
            this.nextWeekScheduleTable.UseVisualStyleBackColor = false;
            this.nextWeekScheduleTable.Click += new System.EventHandler(this.NextWeekScheduleTable_Click);
            // 
            // thisWeekScheduleTable
            // 
            this.thisWeekScheduleTable.BackColor = System.Drawing.Color.White;
            this.thisWeekScheduleTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.thisWeekScheduleTable.Location = new System.Drawing.Point(808, 4);
            this.thisWeekScheduleTable.Name = "thisWeekScheduleTable";
            this.thisWeekScheduleTable.Size = new System.Drawing.Size(112, 34);
            this.thisWeekScheduleTable.TabIndex = 44;
            this.thisWeekScheduleTable.Text = "本周";
            this.thisWeekScheduleTable.UseVisualStyleBackColor = false;
            this.thisWeekScheduleTable.Click += new System.EventHandler(this.ThisWeekScheduleTable_Click);
            // 
            // lastWeekScheduleTable
            // 
            this.lastWeekScheduleTable.BackColor = System.Drawing.Color.White;
            this.lastWeekScheduleTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lastWeekScheduleTable.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lastWeekScheduleTable.Location = new System.Drawing.Point(690, 4);
            this.lastWeekScheduleTable.Name = "lastWeekScheduleTable";
            this.lastWeekScheduleTable.Size = new System.Drawing.Size(112, 34);
            this.lastWeekScheduleTable.TabIndex = 45;
            this.lastWeekScheduleTable.Text = "<<";
            this.lastWeekScheduleTable.UseVisualStyleBackColor = false;
            this.lastWeekScheduleTable.Click += new System.EventHandler(this.LastWeekScheduleTable_Click);
            // 
            // showWeekLabel
            // 
            this.showWeekLabel.AutoSize = true;
            this.showWeekLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.showWeekLabel.ForeColor = System.Drawing.Color.White;
            this.showWeekLabel.Location = new System.Drawing.Point(12, 8);
            this.showWeekLabel.Name = "showWeekLabel";
            this.showWeekLabel.Size = new System.Drawing.Size(175, 28);
            this.showWeekLabel.TabIndex = 46;
            this.showWeekLabel.Text = "showWeekLabel";
            // 
            // StudentScheduleTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(128)))), ((int)(((byte)(194)))));
            this.ClientSize = new System.Drawing.Size(1050, 655);
            this.Controls.Add(this.showWeekLabel);
            this.Controls.Add(this.lastWeekScheduleTable);
            this.Controls.Add(this.thisWeekScheduleTable);
            this.Controls.Add(this.nextWeekScheduleTable);
            this.Controls.Add(this.scheduleTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StudentScheduleTable";
            this.Text = "StudentCalender";
            ((System.ComponentModel.ISupportInitialize)(this.scheduleTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected DataGridView scheduleTable;
        private DataGridViewTextBoxColumn hourColumn;
        private DataGridViewTextBoxColumn mondayColumn;
        private DataGridViewTextBoxColumn tuesdayColumn;
        private DataGridViewTextBoxColumn wednesdayColumn;
        private DataGridViewTextBoxColumn thursdayColumn;
        private DataGridViewTextBoxColumn fridayColumn;
        private DataGridViewTextBoxColumn saturdayColumn;
        private DataGridViewTextBoxColumn sundayColumn;
        private Button nextWeekScheduleTable;
        private Button thisWeekScheduleTable;
        private Button lastWeekScheduleTable;
        private Label showWeekLabel;
    }
}