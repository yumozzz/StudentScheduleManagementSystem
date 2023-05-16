namespace StudentScheduleManagementSystem.UI
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
            this.scheduleData = new System.Windows.Forms.DataGridView();
            this.courseCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weekTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.durationCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.switchData = new System.Windows.Forms.Button();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.weekBox = new System.Windows.Forms.TextBox();
            this.dayBox = new System.Windows.Forms.TextBox();
            this.hourBox = new System.Windows.Forms.TextBox();
            this.durationBox = new System.Windows.Forms.TextBox();
            this.buildingComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buildingRadioButton = new System.Windows.Forms.RadioButton();
            this.linkRadioButton = new System.Windows.Forms.RadioButton();
            this.searchCancel = new System.Windows.Forms.Button();
            this.searchOK = new System.Windows.Forms.Button();
            this.reviseCancel = new System.Windows.Forms.Button();
            this.reviseOK = new System.Windows.Forms.Button();
            this.reviseScheduleButton = new System.Windows.Forms.Button();
            this.deleteScheduleButton = new System.Windows.Forms.Button();
            this.addScheduleButton = new System.Windows.Forms.Button();
            this.searchByNameBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.detectCollision = new System.Windows.Forms.Button();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.linkBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleData)).BeginInit();
            this.SuspendLayout();
            // 
            // scheduleData
            // 
            this.scheduleData.AllowDrop = true;
            this.scheduleData.AllowUserToAddRows = false;
            this.scheduleData.AllowUserToResizeColumns = false;
            this.scheduleData.AllowUserToResizeRows = false;
            this.scheduleData.BackgroundColor = System.Drawing.Color.White;
            this.scheduleData.ColumnHeadersHeight = 34;
            this.scheduleData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.scheduleData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.courseCheckBoxColumn,
            this.nameTextBoxColumn,
            this.weekTextBoxColumn,
            this.dayCheckBoxColumn,
            this.timeCheckBoxColumn,
            this.durationCheckBoxColumn,
            this.descriptionColumn,
            this.locationColumn,
            this.idColumn});
            this.scheduleData.Location = new System.Drawing.Point(12, 48);
            this.scheduleData.Name = "scheduleData";
            this.scheduleData.RowHeadersVisible = false;
            this.scheduleData.RowHeadersWidth = 62;
            this.scheduleData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.scheduleData.RowTemplate.Height = 32;
            this.scheduleData.Size = new System.Drawing.Size(650, 600);
            this.scheduleData.TabIndex = 42;
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
            this.timeCheckBoxColumn.Frozen = true;
            this.timeCheckBoxColumn.HeaderText = "时间";
            this.timeCheckBoxColumn.MinimumWidth = 8;
            this.timeCheckBoxColumn.Name = "timeCheckBoxColumn";
            this.timeCheckBoxColumn.ReadOnly = true;
            this.timeCheckBoxColumn.Width = 150;
            // 
            // durationCheckBoxColumn
            // 
            this.durationCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.durationCheckBoxColumn.Frozen = true;
            this.durationCheckBoxColumn.HeaderText = "时长";
            this.durationCheckBoxColumn.MinimumWidth = 8;
            this.durationCheckBoxColumn.Name = "durationCheckBoxColumn";
            this.durationCheckBoxColumn.ReadOnly = true;
            this.durationCheckBoxColumn.Width = 150;
            // 
            // descriptionColumn
            // 
            this.descriptionColumn.Frozen = true;
            this.descriptionColumn.HeaderText = "描述";
            this.descriptionColumn.MinimumWidth = 8;
            this.descriptionColumn.Name = "descriptionColumn";
            this.descriptionColumn.Width = 150;
            // 
            // locationColumn
            // 
            this.locationColumn.Frozen = true;
            this.locationColumn.HeaderText = "地点/链接";
            this.locationColumn.MinimumWidth = 8;
            this.locationColumn.Name = "locationColumn";
            this.locationColumn.Width = 150;
            // 
            // idColumn
            // 
            this.idColumn.Frozen = true;
            this.idColumn.HeaderText = "ID";
            this.idColumn.MinimumWidth = 8;
            this.idColumn.Name = "idColumn";
            this.idColumn.Visible = false;
            this.idColumn.Width = 150;
            // 
            // switchData
            // 
            this.switchData.Location = new System.Drawing.Point(12, 8);
            this.switchData.Name = "switchData";
            this.switchData.Size = new System.Drawing.Size(112, 34);
            this.switchData.TabIndex = 43;
            this.switchData.Text = "Switch";
            this.switchData.UseVisualStyleBackColor = true;
            this.switchData.Click += new System.EventHandler(this.SwitchData_Click);
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(747, 10);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(300, 30);
            this.nameBox.TabIndex = 44;
            // 
            // weekBox
            // 
            this.weekBox.Location = new System.Drawing.Point(747, 47);
            this.weekBox.Name = "weekBox";
            this.weekBox.Size = new System.Drawing.Size(300, 30);
            this.weekBox.TabIndex = 45;
            // 
            // dayBox
            // 
            this.dayBox.Location = new System.Drawing.Point(747, 83);
            this.dayBox.Name = "dayBox";
            this.dayBox.Size = new System.Drawing.Size(300, 30);
            this.dayBox.TabIndex = 46;
            // 
            // hourBox
            // 
            this.hourBox.Location = new System.Drawing.Point(747, 120);
            this.hourBox.Name = "hourBox";
            this.hourBox.Size = new System.Drawing.Size(104, 30);
            this.hourBox.TabIndex = 47;
            // 
            // durationBox
            // 
            this.durationBox.Location = new System.Drawing.Point(944, 120);
            this.durationBox.Name = "durationBox";
            this.durationBox.Size = new System.Drawing.Size(103, 30);
            this.durationBox.TabIndex = 48;
            // 
            // buildingComboBox
            // 
            this.buildingComboBox.FormattingEnabled = true;
            this.buildingComboBox.Location = new System.Drawing.Point(803, 156);
            this.buildingComboBox.Name = "buildingComboBox";
            this.buildingComboBox.Size = new System.Drawing.Size(244, 32);
            this.buildingComboBox.TabIndex = 49;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(679, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 24);
            this.label1.TabIndex = 51;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(685, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 52;
            this.label2.Text = "Week";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(685, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 24);
            this.label3.TabIndex = 53;
            this.label3.Text = "Day";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(685, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 24);
            this.label4.TabIndex = 54;
            this.label4.Text = "Hour";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(871, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 24);
            this.label5.TabIndex = 55;
            this.label5.Text = "Dur";
            // 
            // buildingRadioButton
            // 
            this.buildingRadioButton.AutoSize = true;
            this.buildingRadioButton.Location = new System.Drawing.Point(690, 156);
            this.buildingRadioButton.Name = "buildingRadioButton";
            this.buildingRadioButton.Size = new System.Drawing.Size(107, 28);
            this.buildingRadioButton.TabIndex = 58;
            this.buildingRadioButton.TabStop = true;
            this.buildingRadioButton.Text = "Buliding";
            this.buildingRadioButton.UseVisualStyleBackColor = true;
            // 
            // linkRadioButton
            // 
            this.linkRadioButton.AutoSize = true;
            this.linkRadioButton.Location = new System.Drawing.Point(690, 199);
            this.linkRadioButton.Name = "linkRadioButton";
            this.linkRadioButton.Size = new System.Drawing.Size(70, 28);
            this.linkRadioButton.TabIndex = 59;
            this.linkRadioButton.TabStop = true;
            this.linkRadioButton.Text = "Link";
            this.linkRadioButton.UseVisualStyleBackColor = true;
            // 
            // searchCancel
            // 
            this.searchCancel.BackColor = System.Drawing.Color.White;
            this.searchCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchCancel.Location = new System.Drawing.Point(871, 523);
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
            this.searchOK.Location = new System.Drawing.Point(708, 523);
            this.searchOK.Name = "searchOK";
            this.searchOK.Size = new System.Drawing.Size(135, 34);
            this.searchOK.TabIndex = 65;
            this.searchOK.Text = "搜索";
            this.searchOK.UseVisualStyleBackColor = false;
            this.searchOK.Click += new System.EventHandler(this.SearchOK_Click);
            // 
            // reviseCancel
            // 
            this.reviseCancel.BackColor = System.Drawing.Color.White;
            this.reviseCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reviseCancel.Location = new System.Drawing.Point(873, 299);
            this.reviseCancel.Name = "reviseCancel";
            this.reviseCancel.Size = new System.Drawing.Size(135, 35);
            this.reviseCancel.TabIndex = 64;
            this.reviseCancel.Text = "取消";
            this.reviseCancel.UseVisualStyleBackColor = false;
            this.reviseCancel.Click += new System.EventHandler(this.ReviseCancel_Click);
            // 
            // reviseOK
            // 
            this.reviseOK.BackColor = System.Drawing.Color.White;
            this.reviseOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reviseOK.Location = new System.Drawing.Point(708, 299);
            this.reviseOK.Name = "reviseOK";
            this.reviseOK.Size = new System.Drawing.Size(135, 35);
            this.reviseOK.TabIndex = 63;
            this.reviseOK.Text = "确认";
            this.reviseOK.UseVisualStyleBackColor = false;
            this.reviseOK.Click += new System.EventHandler(this.ReviseOK_Click);
            // 
            // reviseScheduleButton
            // 
            this.reviseScheduleButton.BackColor = System.Drawing.Color.White;
            this.reviseScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reviseScheduleButton.Location = new System.Drawing.Point(708, 399);
            this.reviseScheduleButton.Name = "reviseScheduleButton";
            this.reviseScheduleButton.Size = new System.Drawing.Size(300, 34);
            this.reviseScheduleButton.TabIndex = 62;
            this.reviseScheduleButton.Text = "修改日程";
            this.reviseScheduleButton.UseVisualStyleBackColor = false;
            this.reviseScheduleButton.Click += new System.EventHandler(this.ReviseScheduleButton_Click);
            // 
            // deleteScheduleButton
            // 
            this.deleteScheduleButton.BackColor = System.Drawing.Color.White;
            this.deleteScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteScheduleButton.Location = new System.Drawing.Point(708, 350);
            this.deleteScheduleButton.Name = "deleteScheduleButton";
            this.deleteScheduleButton.Size = new System.Drawing.Size(300, 34);
            this.deleteScheduleButton.TabIndex = 61;
            this.deleteScheduleButton.Text = "删除日程";
            this.deleteScheduleButton.UseVisualStyleBackColor = false;
            this.deleteScheduleButton.Click += new System.EventHandler(this.DeleteScheduleButton_Click);
            // 
            // addScheduleButton
            // 
            this.addScheduleButton.BackColor = System.Drawing.Color.White;
            this.addScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addScheduleButton.Location = new System.Drawing.Point(708, 299);
            this.addScheduleButton.Name = "addScheduleButton";
            this.addScheduleButton.Size = new System.Drawing.Size(300, 35);
            this.addScheduleButton.TabIndex = 60;
            this.addScheduleButton.Text = "添加日程";
            this.addScheduleButton.UseVisualStyleBackColor = false;
            this.addScheduleButton.Click += new System.EventHandler(this.AddScheduleButton_Click);
            // 
            // searchByNameBox
            // 
            this.searchByNameBox.AcceptsReturn = true;
            this.searchByNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchByNameBox.Location = new System.Drawing.Point(708, 477);
            this.searchByNameBox.Name = "searchByNameBox";
            this.searchByNameBox.ShortcutsEnabled = false;
            this.searchByNameBox.Size = new System.Drawing.Size(300, 30);
            this.searchByNameBox.TabIndex = 67;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(708, 450);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 24);
            this.label6.TabIndex = 69;
            this.label6.Text = "Search by name";
            // 
            // detectCollision
            // 
            this.detectCollision.Location = new System.Drawing.Point(130, 8);
            this.detectCollision.Name = "detectCollision";
            this.detectCollision.Size = new System.Drawing.Size(112, 34);
            this.detectCollision.TabIndex = 71;
            this.detectCollision.Text = "检测冲突";
            this.detectCollision.UseVisualStyleBackColor = true;
            this.detectCollision.Click += new System.EventHandler(this.DetectCollision_Click);
            // 
            // descriptionBox
            // 
            this.descriptionBox.Location = new System.Drawing.Point(803, 230);
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.Size = new System.Drawing.Size(244, 30);
            this.descriptionBox.TabIndex = 72;
            // 
            // linkBox
            // 
            this.linkBox.Location = new System.Drawing.Point(803, 194);
            this.linkBox.Name = "linkBox";
            this.linkBox.Size = new System.Drawing.Size(244, 30);
            this.linkBox.TabIndex = 73;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(685, 236);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 24);
            this.label7.TabIndex = 74;
            this.label7.Text = "Description";
            // 
            // StudentSubwindowBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 655);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.linkBox);
            this.Controls.Add(this.descriptionBox);
            this.Controls.Add(this.detectCollision);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.searchByNameBox);
            this.Controls.Add(this.searchCancel);
            this.Controls.Add(this.searchOK);
            this.Controls.Add(this.reviseCancel);
            this.Controls.Add(this.reviseOK);
            this.Controls.Add(this.reviseScheduleButton);
            this.Controls.Add(this.deleteScheduleButton);
            this.Controls.Add(this.addScheduleButton);
            this.Controls.Add(this.linkRadioButton);
            this.Controls.Add(this.buildingRadioButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buildingComboBox);
            this.Controls.Add(this.durationBox);
            this.Controls.Add(this.hourBox);
            this.Controls.Add(this.dayBox);
            this.Controls.Add(this.weekBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.switchData);
            this.Controls.Add(this.scheduleData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StudentSubwindowBase";
            this.Text = "StudentSubwindow";
            ((System.ComponentModel.ISupportInitialize)(this.scheduleData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected DataGridView scheduleData;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button searchCancel;
        private Button searchOK;
        protected Button reviseCancel;
        protected Button reviseOK;
        protected Button reviseScheduleButton;
        protected Button deleteScheduleButton;
        protected Button addScheduleButton;
        private TextBox searchByNameBox;
        private Label label6;
        private Button detectCollision;
        private Label label7;
        private DataGridViewCheckBoxColumn courseCheckBoxColumn;
        private DataGridViewTextBoxColumn nameTextBoxColumn;
        private DataGridViewTextBoxColumn weekTextBoxColumn;
        private DataGridViewTextBoxColumn dayCheckBoxColumn;
        private DataGridViewTextBoxColumn timeCheckBoxColumn;
        private DataGridViewTextBoxColumn durationCheckBoxColumn;
        private DataGridViewTextBoxColumn descriptionColumn;
        private DataGridViewTextBoxColumn locationColumn;
        private DataGridViewTextBoxColumn idColumn;
        protected RadioButton buildingRadioButton;
        protected RadioButton linkRadioButton;
        protected TextBox descriptionBox;
        protected TextBox linkBox;
        protected TextBox nameBox;
        protected TextBox weekBox;
        protected TextBox dayBox;
        protected TextBox hourBox;
        protected TextBox durationBox;
        protected Button switchData;
        protected ComboBox buildingComboBox;
    }
}