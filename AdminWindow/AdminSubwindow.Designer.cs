namespace StudentScheduleManagementSystem.UI
{
    partial class AdminSubwindowBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        protected System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
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
        protected void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.scheduleDataTable = new System.Windows.Forms.DataGridView();
            this.courseCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weekTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.durationCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weekSelectBox = new StudentScheduleManagementSystem.UI.MultiSelectBox();
            this.daySelectBox = new StudentScheduleManagementSystem.UI.MultiSelectBox();
            this.durationComboBox = new System.Windows.Forms.ComboBox();
            this.hourComboBox = new System.Windows.Forms.ComboBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.addScheduleButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.deleteScheduleButton = new System.Windows.Forms.Button();
            this.reviseScheduleButton = new System.Windows.Forms.Button();
            this.reviseOK = new System.Windows.Forms.Button();
            this.reviseCancel = new System.Windows.Forms.Button();
            this.searchOK = new System.Windows.Forms.Button();
            this.searchByNameBox = new System.Windows.Forms.TextBox();
            this.searchCancel = new System.Windows.Forms.Button();
            this.searchByIdBox = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDataTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // scheduleDataTable
            // 
            this.scheduleDataTable.AllowUserToAddRows = false;
            this.scheduleDataTable.AllowUserToResizeColumns = false;
            this.scheduleDataTable.AllowUserToResizeRows = false;
            this.scheduleDataTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.scheduleDataTable.BackgroundColor = System.Drawing.Color.White;
            this.scheduleDataTable.ColumnHeadersHeight = 34;
            this.scheduleDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.scheduleDataTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.courseCheckBoxColumn,
            this.nameTextBoxColumn,
            this.idTextBoxColumn,
            this.weekTextBoxColumn,
            this.dayCheckBoxColumn,
            this.timeCheckBoxColumn,
            this.durationCheckBoxColumn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scheduleDataTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.scheduleDataTable.Location = new System.Drawing.Point(5, 45);
            this.scheduleDataTable.Name = "scheduleDataTable";
            this.scheduleDataTable.RowHeadersVisible = false;
            this.scheduleDataTable.RowHeadersWidth = 62;
            this.scheduleDataTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.scheduleDataTable.RowTemplate.Height = 32;
            this.scheduleDataTable.Size = new System.Drawing.Size(714, 605);
            this.scheduleDataTable.TabIndex = 41;
            // 
            // courseCheckBoxColumn
            // 
            this.courseCheckBoxColumn.Frozen = true;
            this.courseCheckBoxColumn.HeaderText = "";
            this.courseCheckBoxColumn.MinimumWidth = 8;
            this.courseCheckBoxColumn.Name = "courseCheckBoxColumn";
            this.courseCheckBoxColumn.Width = 30;
            // 
            // nameTextBoxColumn
            // 
            this.nameTextBoxColumn.HeaderText = "日程名称";
            this.nameTextBoxColumn.MinimumWidth = 8;
            this.nameTextBoxColumn.Name = "nameTextBoxColumn";
            this.nameTextBoxColumn.ReadOnly = true;
            this.nameTextBoxColumn.Width = 180;
            // 
            // idTextBoxColumn
            // 
            this.idTextBoxColumn.HeaderText = "日程ID";
            this.idTextBoxColumn.MinimumWidth = 8;
            this.idTextBoxColumn.Name = "idTextBoxColumn";
            this.idTextBoxColumn.Width = 150;
            // 
            // weekTextBoxColumn
            // 
            this.weekTextBoxColumn.HeaderText = "日程周";
            this.weekTextBoxColumn.MinimumWidth = 8;
            this.weekTextBoxColumn.Name = "weekTextBoxColumn";
            this.weekTextBoxColumn.ReadOnly = true;
            this.weekTextBoxColumn.Width = 120;
            // 
            // dayCheckBoxColumn
            // 
            this.dayCheckBoxColumn.HeaderText = "日程日";
            this.dayCheckBoxColumn.MinimumWidth = 8;
            this.dayCheckBoxColumn.Name = "dayCheckBoxColumn";
            this.dayCheckBoxColumn.ReadOnly = true;
            this.dayCheckBoxColumn.Width = 120;
            // 
            // timeCheckBoxColumn
            // 
            this.timeCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.timeCheckBoxColumn.HeaderText = "时间";
            this.timeCheckBoxColumn.MinimumWidth = 8;
            this.timeCheckBoxColumn.Name = "timeCheckBoxColumn";
            this.timeCheckBoxColumn.ReadOnly = true;
            this.timeCheckBoxColumn.Width = 150;
            // 
            // durationCheckBoxColumn
            // 
            this.durationCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.durationCheckBoxColumn.HeaderText = "时长";
            this.durationCheckBoxColumn.MinimumWidth = 8;
            this.durationCheckBoxColumn.Name = "durationCheckBoxColumn";
            this.durationCheckBoxColumn.ReadOnly = true;
            this.durationCheckBoxColumn.Width = 150;
            // 
            // weekSelectBox
            // 
            this.weekSelectBox.BackColor = System.Drawing.Color.White;
            this.weekSelectBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.weekSelectBox.Location = new System.Drawing.Point(735, 106);
            this.weekSelectBox.Name = "weekSelectBox";
            this.weekSelectBox.ShowComboBox = false;
            this.weekSelectBox.Size = new System.Drawing.Size(300, 30);
            this.weekSelectBox.TabIndex = 36;
            // 
            // daySelectBox
            // 
            this.daySelectBox.BackColor = System.Drawing.Color.White;
            this.daySelectBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.daySelectBox.Location = new System.Drawing.Point(735, 166);
            this.daySelectBox.Name = "daySelectBox";
            this.daySelectBox.ShowComboBox = false;
            this.daySelectBox.Size = new System.Drawing.Size(300, 30);
            this.daySelectBox.TabIndex = 37;
            // 
            // durationComboBox
            // 
            this.durationComboBox.BackColor = System.Drawing.Color.White;
            this.durationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.durationComboBox.DropDownWidth = 130;
            this.durationComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.durationComboBox.FormattingEnabled = true;
            this.durationComboBox.Items.AddRange(new object[] {
            "1小时",
            "2小时",
            "3小时"});
            this.durationComboBox.Location = new System.Drawing.Point(900, 228);
            this.durationComboBox.Name = "durationComboBox";
            this.durationComboBox.Size = new System.Drawing.Size(135, 32);
            this.durationComboBox.TabIndex = 40;
            // 
            // hourComboBox
            // 
            this.hourComboBox.BackColor = System.Drawing.Color.White;
            this.hourComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hourComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hourComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.hourComboBox.FormattingEnabled = true;
            this.hourComboBox.Location = new System.Drawing.Point(735, 228);
            this.hourComboBox.Name = "hourComboBox";
            this.hourComboBox.Size = new System.Drawing.Size(135, 32);
            this.hourComboBox.TabIndex = 39;
            // 
            // nameBox
            // 
            this.nameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nameBox.Location = new System.Drawing.Point(735, 46);
            this.nameBox.Name = "nameBox";
            this.nameBox.ShortcutsEnabled = false;
            this.nameBox.Size = new System.Drawing.Size(300, 30);
            this.nameBox.TabIndex = 38;
            // 
            // addScheduleButton
            // 
            this.addScheduleButton.BackColor = System.Drawing.Color.White;
            this.addScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addScheduleButton.Location = new System.Drawing.Point(735, 276);
            this.addScheduleButton.Name = "addScheduleButton";
            this.addScheduleButton.Size = new System.Drawing.Size(300, 35);
            this.addScheduleButton.TabIndex = 35;
            this.addScheduleButton.Text = "添加日程";
            this.addScheduleButton.UseVisualStyleBackColor = false;
            this.addScheduleButton.Click += new System.EventHandler(this.AddSchedule_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::StudentScheduleManagementSystem.Properties.Resources.OperatingBackground;
            this.pictureBox1.Location = new System.Drawing.Point(725, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 420);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // deleteScheduleButton
            // 
            this.deleteScheduleButton.BackColor = System.Drawing.Color.White;
            this.deleteScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteScheduleButton.Location = new System.Drawing.Point(735, 327);
            this.deleteScheduleButton.Name = "deleteScheduleButton";
            this.deleteScheduleButton.Size = new System.Drawing.Size(300, 34);
            this.deleteScheduleButton.TabIndex = 53;
            this.deleteScheduleButton.Text = "删除日程";
            this.deleteScheduleButton.UseVisualStyleBackColor = false;
            this.deleteScheduleButton.Click += new System.EventHandler(this.DeleteSchedule_Click);
            // 
            // reviseScheduleButton
            // 
            this.reviseScheduleButton.BackColor = System.Drawing.Color.White;
            this.reviseScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reviseScheduleButton.Location = new System.Drawing.Point(735, 376);
            this.reviseScheduleButton.Name = "reviseScheduleButton";
            this.reviseScheduleButton.Size = new System.Drawing.Size(300, 34);
            this.reviseScheduleButton.TabIndex = 54;
            this.reviseScheduleButton.Text = "修改日程";
            this.reviseScheduleButton.UseVisualStyleBackColor = false;
            this.reviseScheduleButton.Click += new System.EventHandler(this.ReviseSchedule_Click);
            // 
            // reviseOK
            // 
            this.reviseOK.BackColor = System.Drawing.Color.White;
            this.reviseOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reviseOK.Location = new System.Drawing.Point(735, 276);
            this.reviseOK.Name = "reviseOK";
            this.reviseOK.Size = new System.Drawing.Size(135, 35);
            this.reviseOK.TabIndex = 55;
            this.reviseOK.Text = "确认";
            this.reviseOK.UseVisualStyleBackColor = false;
            this.reviseOK.Click += new System.EventHandler(this.ReviseOK_Click);
            // 
            // reviseCancel
            // 
            this.reviseCancel.BackColor = System.Drawing.Color.White;
            this.reviseCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reviseCancel.Location = new System.Drawing.Point(900, 276);
            this.reviseCancel.Name = "reviseCancel";
            this.reviseCancel.Size = new System.Drawing.Size(135, 35);
            this.reviseCancel.TabIndex = 56;
            this.reviseCancel.Text = "取消";
            this.reviseCancel.UseVisualStyleBackColor = false;
            this.reviseCancel.Click += new System.EventHandler(this.ReviseCancel_Click);
            // 
            // searchOK
            // 
            this.searchOK.BackColor = System.Drawing.Color.White;
            this.searchOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchOK.Location = new System.Drawing.Point(735, 582);
            this.searchOK.Name = "searchOK";
            this.searchOK.Size = new System.Drawing.Size(135, 34);
            this.searchOK.TabIndex = 57;
            this.searchOK.Text = "搜索";
            this.searchOK.UseVisualStyleBackColor = false;
            this.searchOK.Click += new System.EventHandler(this.SearchOK_Click);
            // 
            // searchByNameBox
            // 
            this.searchByNameBox.AcceptsReturn = true;
            this.searchByNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchByNameBox.Location = new System.Drawing.Point(735, 468);
            this.searchByNameBox.Name = "searchByNameBox";
            this.searchByNameBox.ShortcutsEnabled = false;
            this.searchByNameBox.Size = new System.Drawing.Size(300, 30);
            this.searchByNameBox.TabIndex = 58;
            this.searchByNameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchByNameBox_KeyPress);
            // 
            // searchCancel
            // 
            this.searchCancel.BackColor = System.Drawing.Color.White;
            this.searchCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchCancel.Location = new System.Drawing.Point(900, 582);
            this.searchCancel.Name = "searchCancel";
            this.searchCancel.Size = new System.Drawing.Size(135, 34);
            this.searchCancel.TabIndex = 59;
            this.searchCancel.Text = "取消";
            this.searchCancel.UseVisualStyleBackColor = false;
            this.searchCancel.Click += new System.EventHandler(this.SearchCancel_Click);
            // 
            // searchByIdBox
            // 
            this.searchByIdBox.AcceptsReturn = true;
            this.searchByIdBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchByIdBox.Location = new System.Drawing.Point(735, 533);
            this.searchByIdBox.Name = "searchByIdBox";
            this.searchByIdBox.ShortcutsEnabled = false;
            this.searchByIdBox.Size = new System.Drawing.Size(300, 30);
            this.searchByIdBox.TabIndex = 60;
            this.searchByIdBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchByIdBox_KeyPress);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::StudentScheduleManagementSystem.Properties.Resources.SearchBG;
            this.pictureBox2.Location = new System.Drawing.Point(725, 434);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(320, 216);
            this.pictureBox2.TabIndex = 61;
            this.pictureBox2.TabStop = false;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label.ForeColor = System.Drawing.Color.White;
            this.label.Location = new System.Drawing.Point(12, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(154, 24);
            this.label.TabIndex = 81;
            this.label.Text = "showPageType";
            // 
            // AdminSubwindowBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(128)))), ((int)(((byte)(194)))));
            this.ClientSize = new System.Drawing.Size(1050, 655);
            this.Controls.Add(this.label);
            this.Controls.Add(this.weekSelectBox);
            this.Controls.Add(this.daySelectBox);
            this.Controls.Add(this.searchByIdBox);
            this.Controls.Add(this.searchCancel);
            this.Controls.Add(this.searchByNameBox);
            this.Controls.Add(this.searchOK);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.reviseCancel);
            this.Controls.Add(this.reviseOK);
            this.Controls.Add(this.reviseScheduleButton);
            this.Controls.Add(this.deleteScheduleButton);
            this.Controls.Add(this.scheduleDataTable);
            this.Controls.Add(this.durationComboBox);
            this.Controls.Add(this.hourComboBox);
            this.Controls.Add(this.addScheduleButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminSubwindowBase";
            this.Text = ",";
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDataTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected DataGridView scheduleDataTable;
        protected MultiSelectBox weekSelectBox;
        protected MultiSelectBox daySelectBox;
        protected ComboBox durationComboBox;
        protected ComboBox hourComboBox;
        protected TextBox nameBox;
        protected Button addScheduleButton;
        protected Button deleteScheduleButton;
        protected Button reviseScheduleButton;
        protected PictureBox pictureBox1;
        protected Button reviseOK;
        protected Button reviseCancel;
        private Button searchOK;
        private TextBox searchByNameBox;
        private Button searchCancel;
        private TextBox searchByIdBox;
        private PictureBox pictureBox2;
        private DataGridViewCheckBoxColumn courseCheckBoxColumn;
        private DataGridViewTextBoxColumn nameTextBoxColumn;
        private DataGridViewTextBoxColumn idTextBoxColumn;
        private DataGridViewTextBoxColumn weekTextBoxColumn;
        private DataGridViewTextBoxColumn dayCheckBoxColumn;
        private DataGridViewTextBoxColumn timeCheckBoxColumn;
        private DataGridViewTextBoxColumn durationCheckBoxColumn;
        protected Label label;
    }
}