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
            this.allScheduleData = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.buildingComboBox = new System.Windows.Forms.ComboBox();
            this.linkComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.searchCancel = new System.Windows.Forms.Button();
            this.searchOK = new System.Windows.Forms.Button();
            this.reviseCancel = new System.Windows.Forms.Button();
            this.reviseOK = new System.Windows.Forms.Button();
            this.reviseScheduleButton = new System.Windows.Forms.Button();
            this.deleteScheduleButton = new System.Windows.Forms.Button();
            this.addScheduleButton = new System.Windows.Forms.Button();
            this.searchByIdBox = new System.Windows.Forms.TextBox();
            this.searchByNameBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.detectCollision = new System.Windows.Forms.Button();
            this.courseCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weekTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.durationCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.allScheduleData)).BeginInit();
            this.SuspendLayout();
            // 
            // allScheduleData
            // 
            this.allScheduleData.AllowUserToAddRows = false;
            this.allScheduleData.AllowUserToResizeColumns = false;
            this.allScheduleData.AllowUserToResizeRows = false;
            this.allScheduleData.BackgroundColor = System.Drawing.Color.White;
            this.allScheduleData.ColumnHeadersHeight = 34;
            this.allScheduleData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.allScheduleData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.courseCheckBoxColumn,
            this.nameTextBoxColumn,
            this.idTextBoxColumn,
            this.weekTextBoxColumn,
            this.dayCheckBoxColumn,
            this.timeCheckBoxColumn,
            this.durationCheckBoxColumn});
            this.allScheduleData.Location = new System.Drawing.Point(12, 48);
            this.allScheduleData.Name = "allScheduleData";
            this.allScheduleData.RowHeadersVisible = false;
            this.allScheduleData.RowHeadersWidth = 62;
            this.allScheduleData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.allScheduleData.RowTemplate.Height = 32;
            this.allScheduleData.Size = new System.Drawing.Size(714, 595);
            this.allScheduleData.TabIndex = 42;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 34);
            this.button1.TabIndex = 43;
            this.button1.Text = "Switch";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(797, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(250, 30);
            this.textBox1.TabIndex = 44;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(797, 50);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(250, 30);
            this.textBox2.TabIndex = 45;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(797, 86);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(250, 30);
            this.textBox3.TabIndex = 46;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(797, 123);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(92, 30);
            this.textBox4.TabIndex = 47;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(955, 123);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(92, 30);
            this.textBox5.TabIndex = 48;
            // 
            // buildingComboBox
            // 
            this.buildingComboBox.FormattingEnabled = true;
            this.buildingComboBox.Location = new System.Drawing.Point(735, 193);
            this.buildingComboBox.Name = "buildingComboBox";
            this.buildingComboBox.Size = new System.Drawing.Size(312, 32);
            this.buildingComboBox.TabIndex = 49;
            // 
            // linkComboBox
            // 
            this.linkComboBox.FormattingEnabled = true;
            this.linkComboBox.Location = new System.Drawing.Point(735, 265);
            this.linkComboBox.Name = "linkComboBox";
            this.linkComboBox.Size = new System.Drawing.Size(312, 32);
            this.linkComboBox.TabIndex = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(735, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 24);
            this.label1.TabIndex = 51;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(735, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 52;
            this.label2.Text = "Week";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(735, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 24);
            this.label3.TabIndex = 53;
            this.label3.Text = "Day";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(735, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 24);
            this.label4.TabIndex = 54;
            this.label4.Text = "Hour";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(907, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 24);
            this.label5.TabIndex = 55;
            this.label5.Text = "Dur";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(740, 159);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(107, 28);
            this.radioButton1.TabIndex = 58;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Buliding";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(740, 231);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(70, 28);
            this.radioButton2.TabIndex = 59;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Link";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // searchCancel
            // 
            this.searchCancel.BackColor = System.Drawing.Color.White;
            this.searchCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchCancel.Location = new System.Drawing.Point(905, 591);
            this.searchCancel.Name = "searchCancel";
            this.searchCancel.Size = new System.Drawing.Size(135, 34);
            this.searchCancel.TabIndex = 66;
            this.searchCancel.Text = "取消";
            this.searchCancel.UseVisualStyleBackColor = false;
            // 
            // searchOK
            // 
            this.searchOK.BackColor = System.Drawing.Color.White;
            this.searchOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchOK.Location = new System.Drawing.Point(740, 591);
            this.searchOK.Name = "searchOK";
            this.searchOK.Size = new System.Drawing.Size(135, 34);
            this.searchOK.TabIndex = 65;
            this.searchOK.Text = "搜索";
            this.searchOK.UseVisualStyleBackColor = false;
            // 
            // reviseCancel
            // 
            this.reviseCancel.BackColor = System.Drawing.Color.White;
            this.reviseCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reviseCancel.Location = new System.Drawing.Point(905, 303);
            this.reviseCancel.Name = "reviseCancel";
            this.reviseCancel.Size = new System.Drawing.Size(135, 35);
            this.reviseCancel.TabIndex = 64;
            this.reviseCancel.Text = "取消";
            this.reviseCancel.UseVisualStyleBackColor = false;
            // 
            // reviseOK
            // 
            this.reviseOK.BackColor = System.Drawing.Color.White;
            this.reviseOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reviseOK.Location = new System.Drawing.Point(740, 303);
            this.reviseOK.Name = "reviseOK";
            this.reviseOK.Size = new System.Drawing.Size(135, 35);
            this.reviseOK.TabIndex = 63;
            this.reviseOK.Text = "确认";
            this.reviseOK.UseVisualStyleBackColor = false;
            // 
            // reviseScheduleButton
            // 
            this.reviseScheduleButton.BackColor = System.Drawing.Color.White;
            this.reviseScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reviseScheduleButton.Location = new System.Drawing.Point(740, 403);
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
            this.deleteScheduleButton.Location = new System.Drawing.Point(740, 354);
            this.deleteScheduleButton.Name = "deleteScheduleButton";
            this.deleteScheduleButton.Size = new System.Drawing.Size(300, 34);
            this.deleteScheduleButton.TabIndex = 61;
            this.deleteScheduleButton.Text = "删除日程";
            this.deleteScheduleButton.UseVisualStyleBackColor = false;
            // 
            // addScheduleButton
            // 
            this.addScheduleButton.BackColor = System.Drawing.Color.White;
            this.addScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addScheduleButton.Location = new System.Drawing.Point(740, 303);
            this.addScheduleButton.Name = "addScheduleButton";
            this.addScheduleButton.Size = new System.Drawing.Size(300, 35);
            this.addScheduleButton.TabIndex = 60;
            this.addScheduleButton.Text = "添加日程";
            this.addScheduleButton.UseVisualStyleBackColor = false;
            // 
            // searchByIdBox
            // 
            this.searchByIdBox.AcceptsReturn = true;
            this.searchByIdBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchByIdBox.Location = new System.Drawing.Point(740, 546);
            this.searchByIdBox.Name = "searchByIdBox";
            this.searchByIdBox.ShortcutsEnabled = false;
            this.searchByIdBox.Size = new System.Drawing.Size(300, 30);
            this.searchByIdBox.TabIndex = 68;
            // 
            // searchByNameBox
            // 
            this.searchByNameBox.AcceptsReturn = true;
            this.searchByNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchByNameBox.Location = new System.Drawing.Point(740, 481);
            this.searchByNameBox.Name = "searchByNameBox";
            this.searchByNameBox.ShortcutsEnabled = false;
            this.searchByNameBox.Size = new System.Drawing.Size(300, 30);
            this.searchByNameBox.TabIndex = 67;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(740, 454);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 24);
            this.label6.TabIndex = 69;
            this.label6.Text = "Search by name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(740, 519);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 24);
            this.label7.TabIndex = 70;
            this.label7.Text = "Search by ID";
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
            // idTextBoxColumn
            // 
            this.idTextBoxColumn.Frozen = true;
            this.idTextBoxColumn.HeaderText = "日程ID";
            this.idTextBoxColumn.MinimumWidth = 8;
            this.idTextBoxColumn.Name = "idTextBoxColumn";
            this.idTextBoxColumn.Width = 150;
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
            // StudentSubwindowBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 655);
            this.Controls.Add(this.detectCollision);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.searchByIdBox);
            this.Controls.Add(this.searchByNameBox);
            this.Controls.Add(this.searchCancel);
            this.Controls.Add(this.searchOK);
            this.Controls.Add(this.reviseCancel);
            this.Controls.Add(this.reviseOK);
            this.Controls.Add(this.reviseScheduleButton);
            this.Controls.Add(this.deleteScheduleButton);
            this.Controls.Add(this.addScheduleButton);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkComboBox);
            this.Controls.Add(this.buildingComboBox);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.allScheduleData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StudentSubwindowBase";
            this.Text = "StudentSubwindow";
            ((System.ComponentModel.ISupportInitialize)(this.allScheduleData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected DataGridView allScheduleData;
        private Button button1;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private ComboBox buildingComboBox;
        private ComboBox linkComboBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Button searchCancel;
        private Button searchOK;
        protected Button reviseCancel;
        protected Button reviseOK;
        protected Button reviseScheduleButton;
        protected Button deleteScheduleButton;
        protected Button addScheduleButton;
        private TextBox searchByIdBox;
        private TextBox searchByNameBox;
        private Label label6;
        private Label label7;
        private DataGridViewCheckBoxColumn courseCheckBoxColumn;
        private DataGridViewTextBoxColumn nameTextBoxColumn;
        private DataGridViewTextBoxColumn idTextBoxColumn;
        private DataGridViewTextBoxColumn weekTextBoxColumn;
        private DataGridViewTextBoxColumn dayCheckBoxColumn;
        private DataGridViewTextBoxColumn timeCheckBoxColumn;
        private DataGridViewTextBoxColumn durationCheckBoxColumn;
        private Button detectCollision;
    }
}