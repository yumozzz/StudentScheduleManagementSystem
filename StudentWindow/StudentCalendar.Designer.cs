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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.scheduleTable = new System.Windows.Forms.DataGridView();
            this.hourColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mondayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tuesdayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wednesdayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thursdayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fridayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saturdayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sundayColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleTable)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(898, 100);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(150, 30);
            this.textBox1.TabIndex = 0;
            // 
            // scheduleTable
            // 
            this.scheduleTable.AllowUserToAddRows = false;
            this.scheduleTable.AllowUserToResizeColumns = false;
            this.scheduleTable.AllowUserToResizeRows = false;
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
            this.scheduleTable.Location = new System.Drawing.Point(12, 12);
            this.scheduleTable.Name = "scheduleTable";
            this.scheduleTable.RowHeadersVisible = false;
            this.scheduleTable.RowHeadersWidth = 62;
            this.scheduleTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.scheduleTable.RowTemplate.Height = 32;
            this.scheduleTable.Size = new System.Drawing.Size(780, 645);
            this.scheduleTable.TabIndex = 42;
            // 
            // hourColumn
            // 
            this.hourColumn.HeaderText = "时间";
            this.hourColumn.MinimumWidth = 8;
            this.hourColumn.Name = "hourColumn";
            this.hourColumn.Width = 150;
            // 
            // mondayColumn
            // 
            this.mondayColumn.HeaderText = "Monday";
            this.mondayColumn.MinimumWidth = 8;
            this.mondayColumn.Name = "mondayColumn";
            this.mondayColumn.Width = 150;
            // 
            // tuesdayColumn
            // 
            this.tuesdayColumn.HeaderText = "Tuesday";
            this.tuesdayColumn.MinimumWidth = 8;
            this.tuesdayColumn.Name = "tuesdayColumn";
            this.tuesdayColumn.Width = 150;
            // 
            // wednesdayColumn
            // 
            this.wednesdayColumn.HeaderText = "Wednesday";
            this.wednesdayColumn.MinimumWidth = 8;
            this.wednesdayColumn.Name = "wednesdayColumn";
            this.wednesdayColumn.Width = 150;
            // 
            // thursdayColumn
            // 
            this.thursdayColumn.HeaderText = "Thursday";
            this.thursdayColumn.MinimumWidth = 8;
            this.thursdayColumn.Name = "thursdayColumn";
            this.thursdayColumn.Width = 150;
            // 
            // fridayColumn
            // 
            this.fridayColumn.HeaderText = "Friday";
            this.fridayColumn.MinimumWidth = 8;
            this.fridayColumn.Name = "fridayColumn";
            this.fridayColumn.Width = 150;
            // 
            // saturdayColumn
            // 
            this.saturdayColumn.HeaderText = "Saturday";
            this.saturdayColumn.MinimumWidth = 8;
            this.saturdayColumn.Name = "saturdayColumn";
            this.saturdayColumn.Width = 150;
            // 
            // sundayColumn
            // 
            this.sundayColumn.HeaderText = "Sunday";
            this.sundayColumn.MinimumWidth = 8;
            this.sundayColumn.Name = "sundayColumn";
            this.sundayColumn.Width = 150;
            // 
            // StudentScheduleTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 655);
            this.Controls.Add(this.scheduleTable);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StudentScheduleTable";
            this.Text = "StudentCalender";
            ((System.ComponentModel.ISupportInitialize)(this.scheduleTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBox1;
        protected DataGridView scheduleTable;
        private DataGridViewTextBoxColumn hourColumn;
        private DataGridViewTextBoxColumn mondayColumn;
        private DataGridViewTextBoxColumn tuesdayColumn;
        private DataGridViewTextBoxColumn wednesdayColumn;
        private DataGridViewTextBoxColumn thursdayColumn;
        private DataGridViewTextBoxColumn fridayColumn;
        private DataGridViewTextBoxColumn saturdayColumn;
        private DataGridViewTextBoxColumn sundayColumn;
    }
}