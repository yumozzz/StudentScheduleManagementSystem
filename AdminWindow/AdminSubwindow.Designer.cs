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
            this.scheduleData = new System.Windows.Forms.DataGridView();
            this.courseCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weekTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.durationCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weekSelectBox = new StudentScheduleManagementSystem.MultiSelectBox();
            this.daySelectBox = new StudentScheduleManagementSystem.MultiSelectBox();
            this.durationComboBox = new System.Windows.Forms.ComboBox();
            this.hourComboBox = new System.Windows.Forms.ComboBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.addScheduleButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.deleteScheduleButton = new System.Windows.Forms.Button();
            this.reviseScheduleButton = new System.Windows.Forms.Button();
            this.reviseOK = new System.Windows.Forms.Button();
            this.reviseCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // scheduleData
            // 
            this.scheduleData.AllowUserToAddRows = false;
            this.scheduleData.BackgroundColor = System.Drawing.Color.White;
            this.scheduleData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scheduleData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.courseCheckBoxColumn,
            this.nameTextBoxColumn,
            this.weekTextBoxColumn,
            this.dayCheckBoxColumn,
            this.timeCheckBoxColumn,
            this.durationCheckBoxColumn});
            this.scheduleData.Location = new System.Drawing.Point(5, 2);
            this.scheduleData.Name = "scheduleData";
            this.scheduleData.RowHeadersVisible = false;
            this.scheduleData.RowHeadersWidth = 62;
            this.scheduleData.RowTemplate.Height = 32;
            this.scheduleData.Size = new System.Drawing.Size(714, 651);
            this.scheduleData.TabIndex = 41;
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
            this.nameTextBoxColumn.Frozen = true;
            this.nameTextBoxColumn.HeaderText = "日程名称";
            this.nameTextBoxColumn.MinimumWidth = 8;
            this.nameTextBoxColumn.Name = "nameTextBoxColumn";
            this.nameTextBoxColumn.ReadOnly = true;
            this.nameTextBoxColumn.Width = 180;
            // 
            // weekTextBoxColumn
            // 
            this.weekTextBoxColumn.Frozen = true;
            this.weekTextBoxColumn.HeaderText = "日程周";
            this.weekTextBoxColumn.MinimumWidth = 8;
            this.weekTextBoxColumn.Name = "weekTextBoxColumn";
            this.weekTextBoxColumn.ReadOnly = true;
            this.weekTextBoxColumn.Width = 120;
            // 
            // dayCheckBoxColumn
            // 
            this.dayCheckBoxColumn.Frozen = true;
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
            this.weekSelectBox.Location = new System.Drawing.Point(735, 104);
            this.weekSelectBox.Name = "weekSelectBox";
            this.weekSelectBox.Size = new System.Drawing.Size(300, 30);
            this.weekSelectBox.TabIndex = 36;
            // 
            // daySelectBox
            // 
            this.daySelectBox.BackColor = System.Drawing.Color.White;
            this.daySelectBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.daySelectBox.Location = new System.Drawing.Point(735, 164);
            this.daySelectBox.Name = "daySelectBox";
            this.daySelectBox.Size = new System.Drawing.Size(300, 30);
            this.daySelectBox.TabIndex = 37;
            // 
            // durationComboBox
            // 
            this.durationComboBox.BackColor = System.Drawing.Color.White;
            this.durationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.durationComboBox.DropDownWidth = 130;
            this.durationComboBox.FormattingEnabled = true;
            this.durationComboBox.Items.AddRange(new object[] {
            "1小时",
            "2小时",
            "3小时"});
            this.durationComboBox.Location = new System.Drawing.Point(900, 224);
            this.durationComboBox.Name = "durationComboBox";
            this.durationComboBox.Size = new System.Drawing.Size(135, 32);
            this.durationComboBox.TabIndex = 40;
            // 
            // hourComboBox
            // 
            this.hourComboBox.BackColor = System.Drawing.Color.White;
            this.hourComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hourComboBox.FormattingEnabled = true;
            this.hourComboBox.Items.AddRange(new object[] {
            "8:00",
            "9:00",
            "10:00",
            "11:00",
            "12:00",
            "13:00",
            "14:00",
            "15:00",
            "16:00",
            "17:00",
            "18:00",
            "19:00",
            "20:00",
            "21:00"});
            this.hourComboBox.Location = new System.Drawing.Point(735, 224);
            this.hourComboBox.Name = "hourComboBox";
            this.hourComboBox.Size = new System.Drawing.Size(135, 32);
            this.hourComboBox.TabIndex = 39;
            // 
            // nameBox
            // 
            this.nameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nameBox.Location = new System.Drawing.Point(735, 44);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(300, 30);
            this.nameBox.TabIndex = 38;
            // 
            // addScheduleButton
            // 
            this.addScheduleButton.BackColor = System.Drawing.Color.White;
            this.addScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addScheduleButton.Location = new System.Drawing.Point(735, 272);
            this.addScheduleButton.Name = "addScheduleButton";
            this.addScheduleButton.Size = new System.Drawing.Size(300, 35);
            this.addScheduleButton.TabIndex = 35;
            this.addScheduleButton.Text = "AddSchedule";
            this.addScheduleButton.UseVisualStyleBackColor = false;
            this.addScheduleButton.Click += new System.EventHandler(this.AddSchedule_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::StudentScheduleManagementSystem.Properties.Resources.OPBG_Format_1;
            this.pictureBox1.Location = new System.Drawing.Point(725, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 416);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(900, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 24);
            this.label5.TabIndex = 52;
            this.label5.Text = "Duration";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(735, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 24);
            this.label4.TabIndex = 51;
            this.label4.Text = "Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(735, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 24);
            this.label3.TabIndex = 50;
            this.label3.Text = "Day";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(735, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 49;
            this.label2.Text = "Week";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(735, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 24);
            this.label1.TabIndex = 48;
            this.label1.Text = "Name";
            // 
            // deleteScheduleButton
            // 
            this.deleteScheduleButton.Location = new System.Drawing.Point(735, 325);
            this.deleteScheduleButton.Name = "deleteScheduleButton";
            this.deleteScheduleButton.Size = new System.Drawing.Size(300, 34);
            this.deleteScheduleButton.TabIndex = 53;
            this.deleteScheduleButton.Text = "DeleteSchedule";
            this.deleteScheduleButton.UseVisualStyleBackColor = true;
            this.deleteScheduleButton.Click += new System.EventHandler(this.DeleteSchedule_Click);
            // 
            // reviseScheduleButton
            // 
            this.reviseScheduleButton.Location = new System.Drawing.Point(735, 374);
            this.reviseScheduleButton.Name = "reviseScheduleButton";
            this.reviseScheduleButton.Size = new System.Drawing.Size(300, 34);
            this.reviseScheduleButton.TabIndex = 54;
            this.reviseScheduleButton.Text = "ReviseSchedule";
            this.reviseScheduleButton.UseVisualStyleBackColor = true;
            this.reviseScheduleButton.Click += new System.EventHandler(this.ReviseSchedule_Click);
            // 
            // reviseYes
            // 
            this.reviseOK.Location = new System.Drawing.Point(735, 273);
            this.reviseOK.Name = "reviseOK";
            this.reviseOK.Size = new System.Drawing.Size(135, 34);
            this.reviseOK.TabIndex = 55;
            this.reviseOK.Text = "确认";
            this.reviseOK.UseVisualStyleBackColor = true;
            this.reviseOK.Click += new System.EventHandler(this.ReviseOK_Click);
            // 
            // reviseCancel
            // 
            this.reviseCancel.Location = new System.Drawing.Point(900, 273);
            this.reviseCancel.Name = "reviseCancel";
            this.reviseCancel.Size = new System.Drawing.Size(135, 34);
            this.reviseCancel.TabIndex = 56;
            this.reviseCancel.Text = "取消";
            this.reviseCancel.UseVisualStyleBackColor = true;
            this.reviseCancel.Click += new System.EventHandler(this.ReviseCancel_Click);
            // 
            // AdminSubwindowBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 655);
            this.Controls.Add(this.reviseCancel);
            this.Controls.Add(this.reviseOK);
            this.Controls.Add(this.reviseScheduleButton);
            this.Controls.Add(this.deleteScheduleButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scheduleData);
            this.Controls.Add(this.weekSelectBox);
            this.Controls.Add(this.daySelectBox);
            this.Controls.Add(this.durationComboBox);
            this.Controls.Add(this.hourComboBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.addScheduleButton);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminSubwindowBase";
            this.Text = ",";
            ((System.ComponentModel.ISupportInitialize)(this.scheduleData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected DataGridView scheduleData;
        protected MultiSelectBox weekSelectBox;
        protected MultiSelectBox daySelectBox;
        protected ComboBox durationComboBox;
        protected ComboBox hourComboBox;
        protected TextBox nameBox;
        protected Button addScheduleButton;
        protected Button deleteScheduleButton;
        protected Button reviseScheduleButton;
        protected PictureBox pictureBox1;
        protected Label label5;
        protected Label label4;
        protected Label label3;
        protected Label label2;
        protected Label label1;
        protected DataGridViewCheckBoxColumn courseCheckBoxColumn;
        protected DataGridViewTextBoxColumn nameTextBoxColumn;
        protected DataGridViewTextBoxColumn weekTextBoxColumn;
        protected DataGridViewTextBoxColumn dayCheckBoxColumn;
        protected DataGridViewTextBoxColumn timeCheckBoxColumn;
        protected DataGridViewTextBoxColumn durationCheckBoxColumn;
        protected Button reviseOK;
        protected Button reviseCancel;
    }
}