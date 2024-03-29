﻿namespace StudentScheduleManagementSystem.UI
{
    partial class StudentSubwindowBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentSubwindowBase));
            this.scheduleDataTable = new System.Windows.Forms.DataGridView();
            this.selectCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.alarmCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weekTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.durationTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.switchPageButton = new System.Windows.Forms.Button();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.weekBox = new System.Windows.Forms.TextBox();
            this.dayBox = new System.Windows.Forms.TextBox();
            this.hourBox = new System.Windows.Forms.TextBox();
            this.durationBox = new System.Windows.Forms.TextBox();
            this.buildingComboBox = new System.Windows.Forms.ComboBox();
            this.buildingRadioButton = new System.Windows.Forms.RadioButton();
            this.onlineLinkRadioButton = new System.Windows.Forms.RadioButton();
            this.searchCancel = new System.Windows.Forms.Button();
            this.searchOK = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.reviseScheduleButton = new System.Windows.Forms.Button();
            this.deleteScheduleButton = new System.Windows.Forms.Button();
            this.AddScheduleButton = new System.Windows.Forms.Button();
            this.searchByNameBox = new System.Windows.Forms.TextBox();
            this.detectCollisionButton = new System.Windows.Forms.Button();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.onlineLinkBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.locationPictureBox = new System.Windows.Forms.PictureBox();
            this.searchByNamePictureBox = new System.Windows.Forms.PictureBox();
            this.hideDurationPictureBox = new System.Windows.Forms.PictureBox();
            this.label = new System.Windows.Forms.Label();
            this.getAvailableTime = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDataTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchByNamePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hideDurationPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // scheduleDataTable
            // 
            this.scheduleDataTable.AllowUserToAddRows = false;
            this.scheduleDataTable.AllowUserToDeleteRows = false;
            this.scheduleDataTable.AllowUserToResizeColumns = false;
            this.scheduleDataTable.AllowUserToResizeRows = false;
            this.scheduleDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scheduleDataTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.scheduleDataTable.BackgroundColor = System.Drawing.Color.White;
            this.scheduleDataTable.ColumnHeadersHeight = 34;
            this.scheduleDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.scheduleDataTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectCheckBoxColumn,
            this.alarmCheckBoxColumn,
            this.nameTextBoxColumn,
            this.weekTextBoxColumn,
            this.dayTextBoxColumn,
            this.timeTextBoxColumn,
            this.durationTextBoxColumn,
            this.descriptionTextBoxColumn,
            this.locationTextBoxColumn,
            this.idColumn});
            this.scheduleDataTable.Location = new System.Drawing.Point(5, 45);
            this.scheduleDataTable.Name = "scheduleDataTable";
            this.scheduleDataTable.RowHeadersVisible = false;
            this.scheduleDataTable.RowHeadersWidth = 62;
            this.scheduleDataTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.scheduleDataTable.RowTemplate.Height = 32;
            this.scheduleDataTable.Size = new System.Drawing.Size(714, 605);
            this.scheduleDataTable.TabIndex = 42;
            // 
            // selectCheckBoxColumn
            // 
            this.selectCheckBoxColumn.Frozen = true;
            this.selectCheckBoxColumn.HeaderText = "";
            this.selectCheckBoxColumn.MinimumWidth = 8;
            this.selectCheckBoxColumn.Name = "selectCheckBoxColumn";
            this.selectCheckBoxColumn.Width = 30;
            // 
            // alarmCheckBoxColumn
            // 
            this.alarmCheckBoxColumn.HeaderText = "闹钟";
            this.alarmCheckBoxColumn.MinimumWidth = 8;
            this.alarmCheckBoxColumn.Name = "alarmCheckBoxColumn";
            this.alarmCheckBoxColumn.Width = 55;
            // 
            // nameTextBoxColumn
            // 
            this.nameTextBoxColumn.HeaderText = "日程名称";
            this.nameTextBoxColumn.MinimumWidth = 8;
            this.nameTextBoxColumn.Name = "nameTextBoxColumn";
            this.nameTextBoxColumn.ReadOnly = true;
            this.nameTextBoxColumn.Width = 180;
            // 
            // weekTextBoxColumn
            // 
            this.weekTextBoxColumn.HeaderText = "日程周";
            this.weekTextBoxColumn.MinimumWidth = 8;
            this.weekTextBoxColumn.Name = "weekTextBoxColumn";
            this.weekTextBoxColumn.ReadOnly = true;
            this.weekTextBoxColumn.Width = 120;
            // 
            // dayTextBoxColumn
            // 
            this.dayTextBoxColumn.HeaderText = "日程日";
            this.dayTextBoxColumn.MinimumWidth = 8;
            this.dayTextBoxColumn.Name = "dayTextBoxColumn";
            this.dayTextBoxColumn.ReadOnly = true;
            this.dayTextBoxColumn.Width = 120;
            // 
            // timeTextBoxColumn
            // 
            this.timeTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.timeTextBoxColumn.HeaderText = "时间";
            this.timeTextBoxColumn.MinimumWidth = 8;
            this.timeTextBoxColumn.Name = "timeTextBoxColumn";
            this.timeTextBoxColumn.ReadOnly = true;
            this.timeTextBoxColumn.Width = 150;
            // 
            // durationTextBoxColumn
            // 
            this.durationTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.durationTextBoxColumn.HeaderText = "时长";
            this.durationTextBoxColumn.MinimumWidth = 8;
            this.durationTextBoxColumn.Name = "durationTextBoxColumn";
            this.durationTextBoxColumn.ReadOnly = true;
            this.durationTextBoxColumn.Width = 150;
            // 
            // descriptionTextBoxColumn
            // 
            this.descriptionTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.descriptionTextBoxColumn.HeaderText = "描述";
            this.descriptionTextBoxColumn.MinimumWidth = 8;
            this.descriptionTextBoxColumn.Name = "descriptionTextBoxColumn";
            this.descriptionTextBoxColumn.ReadOnly = true;
            this.descriptionTextBoxColumn.Width = 150;
            // 
            // locationTextBoxColumn
            // 
            this.locationTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.locationTextBoxColumn.HeaderText = "地点/链接";
            this.locationTextBoxColumn.MinimumWidth = 8;
            this.locationTextBoxColumn.Name = "locationTextBoxColumn";
            this.locationTextBoxColumn.ReadOnly = true;
            this.locationTextBoxColumn.Width = 150;
            // 
            // idColumn
            // 
            this.idColumn.HeaderText = "ID";
            this.idColumn.MinimumWidth = 8;
            this.idColumn.Name = "idColumn";
            this.idColumn.Visible = false;
            this.idColumn.Width = 150;
            // 
            // switchPageButton
            // 
            this.switchPageButton.BackColor = System.Drawing.Color.White;
            this.switchPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.switchPageButton.Location = new System.Drawing.Point(607, 5);
            this.switchPageButton.Name = "switchPageButton";
            this.switchPageButton.Size = new System.Drawing.Size(112, 34);
            this.switchPageButton.TabIndex = 43;
            this.switchPageButton.Text = "切换";
            this.switchPageButton.UseVisualStyleBackColor = false;
            this.switchPageButton.Click += new System.EventHandler(this.SwitchData_Click);
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(736, 47);
            this.nameBox.Name = "nameBox";
            this.nameBox.ReadOnly = true;
            this.nameBox.Size = new System.Drawing.Size(300, 30);
            this.nameBox.TabIndex = 44;
            // 
            // weekBox
            // 
            this.weekBox.Location = new System.Drawing.Point(736, 107);
            this.weekBox.Name = "weekBox";
            this.weekBox.ReadOnly = true;
            this.weekBox.Size = new System.Drawing.Size(300, 30);
            this.weekBox.TabIndex = 45;
            // 
            // dayBox
            // 
            this.dayBox.Location = new System.Drawing.Point(736, 168);
            this.dayBox.Name = "dayBox";
            this.dayBox.ReadOnly = true;
            this.dayBox.Size = new System.Drawing.Size(300, 30);
            this.dayBox.TabIndex = 46;
            // 
            // hourBox
            // 
            this.hourBox.Location = new System.Drawing.Point(736, 227);
            this.hourBox.Name = "hourBox";
            this.hourBox.ReadOnly = true;
            this.hourBox.Size = new System.Drawing.Size(135, 30);
            this.hourBox.TabIndex = 47;
            // 
            // durationBox
            // 
            this.durationBox.Location = new System.Drawing.Point(901, 227);
            this.durationBox.Name = "durationBox";
            this.durationBox.ReadOnly = true;
            this.durationBox.Size = new System.Drawing.Size(135, 30);
            this.durationBox.TabIndex = 48;
            // 
            // buildingComboBox
            // 
            this.buildingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.buildingComboBox.FormattingEnabled = true;
            this.buildingComboBox.Location = new System.Drawing.Point(802, 269);
            this.buildingComboBox.Name = "buildingComboBox";
            this.buildingComboBox.Size = new System.Drawing.Size(234, 32);
            this.buildingComboBox.TabIndex = 49;
            // 
            // buildingRadioButton
            // 
            this.buildingRadioButton.AutoSize = true;
            this.buildingRadioButton.Location = new System.Drawing.Point(738, 273);
            this.buildingRadioButton.Name = "buildingRadioButton";
            this.buildingRadioButton.Size = new System.Drawing.Size(21, 20);
            this.buildingRadioButton.TabIndex = 58;
            this.buildingRadioButton.TabStop = true;
            this.buildingRadioButton.UseVisualStyleBackColor = true;
            // 
            // onlineLinkRadioButton
            // 
            this.onlineLinkRadioButton.AutoSize = true;
            this.onlineLinkRadioButton.Location = new System.Drawing.Point(738, 317);
            this.onlineLinkRadioButton.Name = "onlineLinkRadioButton";
            this.onlineLinkRadioButton.Size = new System.Drawing.Size(21, 20);
            this.onlineLinkRadioButton.TabIndex = 59;
            this.onlineLinkRadioButton.UseVisualStyleBackColor = true;
            // 
            // searchCancel
            // 
            this.searchCancel.BackColor = System.Drawing.Color.White;
            this.searchCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchCancel.Location = new System.Drawing.Point(897, 609);
            this.searchCancel.Name = "searchCancel";
            this.searchCancel.Size = new System.Drawing.Size(135, 34);
            this.searchCancel.TabIndex = 66;
            this.searchCancel.Text = "取消";
            this.searchCancel.UseVisualStyleBackColor = false;
            this.searchCancel.Click += new System.EventHandler(this.SearchCancel_Click);
            // 
            // searchOK
            // 
            this.searchOK.BackColor = System.Drawing.Color.White;
            this.searchOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchOK.Location = new System.Drawing.Point(734, 609);
            this.searchOK.Name = "searchOK";
            this.searchOK.Size = new System.Drawing.Size(135, 34);
            this.searchOK.TabIndex = 65;
            this.searchOK.Text = "搜索";
            this.searchOK.UseVisualStyleBackColor = false;
            this.searchOK.Click += new System.EventHandler(this.SearchOK_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.White;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(901, 395);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(135, 35);
            this.cancelButton.TabIndex = 64;
            this.cancelButton.Text = "取消";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.BackColor = System.Drawing.Color.White;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okButton.Location = new System.Drawing.Point(736, 395);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(135, 35);
            this.okButton.TabIndex = 63;
            this.okButton.Text = "确认";
            this.okButton.UseVisualStyleBackColor = false;
            // 
            // reviseScheduleButton
            // 
            this.reviseScheduleButton.BackColor = System.Drawing.Color.White;
            this.reviseScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reviseScheduleButton.Location = new System.Drawing.Point(736, 495);
            this.reviseScheduleButton.Name = "reviseScheduleButton";
            this.reviseScheduleButton.Size = new System.Drawing.Size(300, 34);
            this.reviseScheduleButton.TabIndex = 62;
            this.reviseScheduleButton.Text = "修改日程";
            this.reviseScheduleButton.UseVisualStyleBackColor = false;
            // 
            // deleteScheduleButton
            // 
            this.deleteScheduleButton.BackColor = System.Drawing.Color.White;
            this.deleteScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteScheduleButton.Location = new System.Drawing.Point(736, 446);
            this.deleteScheduleButton.Name = "deleteScheduleButton";
            this.deleteScheduleButton.Size = new System.Drawing.Size(300, 34);
            this.deleteScheduleButton.TabIndex = 61;
            this.deleteScheduleButton.Text = "删除日程";
            this.deleteScheduleButton.UseVisualStyleBackColor = false;
            // 
            // AddScheduleButton
            // 
            this.AddScheduleButton.BackColor = System.Drawing.Color.White;
            this.AddScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddScheduleButton.Location = new System.Drawing.Point(736, 395);
            this.AddScheduleButton.Name = "AddScheduleButton";
            this.AddScheduleButton.Size = new System.Drawing.Size(300, 35);
            this.AddScheduleButton.TabIndex = 60;
            this.AddScheduleButton.Text = "添加日程";
            this.AddScheduleButton.UseVisualStyleBackColor = false;
            // 
            // searchByNameBox
            // 
            this.searchByNameBox.AcceptsReturn = true;
            this.searchByNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchByNameBox.Location = new System.Drawing.Point(734, 568);
            this.searchByNameBox.Name = "searchByNameBox";
            this.searchByNameBox.ShortcutsEnabled = false;
            this.searchByNameBox.Size = new System.Drawing.Size(300, 30);
            this.searchByNameBox.TabIndex = 67;
            // 
            // detectCollisionButton
            // 
            this.detectCollisionButton.BackColor = System.Drawing.Color.White;
            this.detectCollisionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.detectCollisionButton.Location = new System.Drawing.Point(489, 5);
            this.detectCollisionButton.Name = "detectCollisionButton";
            this.detectCollisionButton.Size = new System.Drawing.Size(112, 34);
            this.detectCollisionButton.TabIndex = 71;
            this.detectCollisionButton.Text = "检测冲突";
            this.detectCollisionButton.UseVisualStyleBackColor = false;
            // 
            // descriptionBox
            // 
            this.descriptionBox.Location = new System.Drawing.Point(786, 355);
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.Size = new System.Drawing.Size(250, 30);
            this.descriptionBox.TabIndex = 72;
            // 
            // onlineLinkBox
            // 
            this.onlineLinkBox.Location = new System.Drawing.Point(802, 313);
            this.onlineLinkBox.Name = "onlineLinkBox";
            this.onlineLinkBox.Size = new System.Drawing.Size(234, 30);
            this.onlineLinkBox.TabIndex = 73;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::StudentScheduleManagementSystem.Properties.Resources.OperatingBackground;
            this.pictureBox1.Location = new System.Drawing.Point(725, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 255);
            this.pictureBox1.TabIndex = 75;
            this.pictureBox1.TabStop = false;
            // 
            // locationPictureBox
            // 
            this.locationPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("locationPictureBox.Image")));
            this.locationPictureBox.Location = new System.Drawing.Point(725, 250);
            this.locationPictureBox.Name = "locationPictureBox";
            this.locationPictureBox.Size = new System.Drawing.Size(320, 150);
            this.locationPictureBox.TabIndex = 76;
            this.locationPictureBox.TabStop = false;
            // 
            // searchByNamePictureBox
            // 
            this.searchByNamePictureBox.Image = global::StudentScheduleManagementSystem.Properties.Resources.SearchBG;
            this.searchByNamePictureBox.Location = new System.Drawing.Point(725, 534);
            this.searchByNamePictureBox.Name = "searchByNamePictureBox";
            this.searchByNamePictureBox.Size = new System.Drawing.Size(320, 70);
            this.searchByNamePictureBox.TabIndex = 77;
            this.searchByNamePictureBox.TabStop = false;
            this.searchByNameBox.KeyPress += this.SearchByNameBox_KeyPress;
            // 
            // hideDurationPictureBox
            // 
            this.hideDurationPictureBox.Location = new System.Drawing.Point(888, 204);
            this.hideDurationPictureBox.Name = "hideDurationPictureBox";
            this.hideDurationPictureBox.Size = new System.Drawing.Size(150, 58);
            this.hideDurationPictureBox.TabIndex = 78;
            this.hideDurationPictureBox.TabStop = false;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label.ForeColor = System.Drawing.Color.White;
            this.label.Location = new System.Drawing.Point(12, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(154, 24);
            this.label.TabIndex = 80;
            this.label.Text = "showPageType";
            // 
            // getAvailableTime
            // 
            this.getAvailableTime.BackColor = System.Drawing.Color.White;
            this.getAvailableTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.getAvailableTime.Location = new System.Drawing.Point(584, 5);
            this.getAvailableTime.Name = "getAvailableTime";
            this.getAvailableTime.Size = new System.Drawing.Size(135, 34);
            this.getAvailableTime.TabIndex = 81;
            this.getAvailableTime.Text = "检测可选时段";
            this.getAvailableTime.UseVisualStyleBackColor = false;
            this.getAvailableTime.Visible = false;
            // 
            // StudentSubwindowBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(128)))), ((int)(((byte)(194)))));
            this.ClientSize = new System.Drawing.Size(1050, 655);
            this.Controls.Add(this.getAvailableTime);
            this.Controls.Add(this.label);
            this.Controls.Add(this.onlineLinkBox);
            this.Controls.Add(this.descriptionBox);
            this.Controls.Add(this.detectCollisionButton);
            this.Controls.Add(this.searchByNameBox);
            this.Controls.Add(this.searchCancel);
            this.Controls.Add(this.searchOK);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.reviseScheduleButton);
            this.Controls.Add(this.deleteScheduleButton);
            this.Controls.Add(this.AddScheduleButton);
            this.Controls.Add(this.onlineLinkRadioButton);
            this.Controls.Add(this.buildingRadioButton);
            this.Controls.Add(this.buildingComboBox);
            this.Controls.Add(this.durationBox);
            this.Controls.Add(this.hourBox);
            this.Controls.Add(this.dayBox);
            this.Controls.Add(this.weekBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.switchPageButton);
            this.Controls.Add(this.scheduleDataTable);
            this.Controls.Add(this.searchByNamePictureBox);
            this.Controls.Add(this.hideDurationPictureBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.locationPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StudentSubwindowBase";
            this.Text = "StudentSubwindow";
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDataTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchByNamePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hideDurationPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected DataGridView scheduleDataTable;
        protected Button searchCancel;
        protected Button searchOK;
        protected Button cancelButton;
        protected Button okButton;
        protected Button reviseScheduleButton;
        protected Button deleteScheduleButton;
        protected Button AddScheduleButton;
        protected Button detectCollisionButton;
        protected Button switchPageButton;

        protected RadioButton buildingRadioButton;
        protected RadioButton onlineLinkRadioButton;
        protected ComboBox buildingComboBox;

        protected TextBox searchByNameBox;
        protected TextBox descriptionBox;
        protected TextBox onlineLinkBox;
        protected TextBox nameBox;
        protected TextBox weekBox;
        protected TextBox dayBox;
        protected TextBox hourBox;
        protected TextBox durationBox;
        
        protected PictureBox pictureBox1;
        protected PictureBox locationPictureBox;
        protected PictureBox searchByNamePictureBox;
        protected PictureBox hideDurationPictureBox;
        protected Label label;
        private DataGridViewCheckBoxColumn selectCheckBoxColumn;
        private DataGridViewCheckBoxColumn alarmCheckBoxColumn;
        private DataGridViewTextBoxColumn nameTextBoxColumn;
        private DataGridViewTextBoxColumn weekTextBoxColumn;
        private DataGridViewTextBoxColumn dayTextBoxColumn;
        private DataGridViewTextBoxColumn timeTextBoxColumn;
        private DataGridViewTextBoxColumn durationTextBoxColumn;
        private DataGridViewTextBoxColumn descriptionTextBoxColumn;
        private DataGridViewTextBoxColumn locationTextBoxColumn;
        private DataGridViewTextBoxColumn idColumn;
        protected Button getAvailableTime;
    }
}