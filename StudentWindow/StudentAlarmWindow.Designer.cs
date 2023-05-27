namespace StudentScheduleManagementSystem.UI
{
    partial class StudentAlarmWindow
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
            alarmName = new Label();
            buildingComboBox = new ComboBox();
            currentBuildingLabel = new Label();
            showMapButton = new Button();
            onlineLinkLinkLabel = new LinkLabel();
            onlineLinkLabel = new Label();
            SuspendLayout();
            // 
            // alarmName
            // 
            alarmName.AutoEllipsis = true;
            alarmName.Font = new Font("黑体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            alarmName.ForeColor = Color.White;
            alarmName.Location = new Point(40, 35);
            alarmName.Name = "alarmName";
            alarmName.Size = new Size(400, 60);
            alarmName.TabIndex = 0;
            alarmName.Text = "alarmName";
            alarmName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // buildingComboBox
            // 
            buildingComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            buildingComboBox.FormattingEnabled = true;
            buildingComboBox.Location = new Point(147, 136);
            buildingComboBox.Name = "buildingComboBox";
            buildingComboBox.Size = new Size(182, 32);
            buildingComboBox.TabIndex = 1;
            // 
            // currentBuildingLabel
            // 
            currentBuildingLabel.AutoSize = true;
            currentBuildingLabel.Font = new Font("黑体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            currentBuildingLabel.ForeColor = Color.White;
            currentBuildingLabel.Location = new Point(38, 139);
            currentBuildingLabel.Name = "currentBuildingLabel";
            currentBuildingLabel.Size = new Size(106, 24);
            currentBuildingLabel.TabIndex = 2;
            currentBuildingLabel.Text = "当前地点";
            // 
            // showMapButton
            // 
            showMapButton.BackColor = Color.White;
            showMapButton.FlatStyle = FlatStyle.Flat;
            showMapButton.Location = new Point(332, 136);
            showMapButton.Name = "showMapButton";
            showMapButton.Size = new Size(112, 34);
            showMapButton.TabIndex = 3;
            showMapButton.Text = "显示导航";
            showMapButton.UseVisualStyleBackColor = false;
            showMapButton.Click += ShowMapButton_Click;
            // 
            // onlineLinkLinkLabel
            // 
            onlineLinkLinkLabel.AutoSize = true;
            onlineLinkLinkLabel.Location = new Point(147, 140);
            onlineLinkLinkLabel.Name = "onlineLinkLinkLabel";
            onlineLinkLinkLabel.Size = new Size(98, 24);
            onlineLinkLinkLabel.TabIndex = 4;
            onlineLinkLinkLabel.TabStop = true;
            onlineLinkLinkLabel.Text = "linkLabel1";
            // 
            // onlineLinkLabel
            // 
            onlineLinkLabel.AutoSize = true;
            onlineLinkLabel.Font = new Font("黑体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            onlineLinkLabel.ForeColor = Color.White;
            onlineLinkLabel.Location = new Point(38, 139);
            onlineLinkLabel.Name = "onlineLinkLabel";
            onlineLinkLabel.Size = new Size(106, 24);
            onlineLinkLabel.TabIndex = 5;
            onlineLinkLabel.Text = "课程链接";
            // 
            // StudentAlarmWindow
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 128, 194);
            ClientSize = new Size(478, 244);
            Controls.Add(buildingComboBox);
            Controls.Add(onlineLinkLinkLabel);
            Controls.Add(showMapButton);
            Controls.Add(currentBuildingLabel);
            Controls.Add(alarmName);
            Controls.Add(onlineLinkLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "StudentAlarmWindow";
            Text = "闹钟";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label alarmName;
        private ComboBox buildingComboBox;
        private Label currentBuildingLabel;
        private Button showMapButton;
        private LinkLabel onlineLinkLinkLabel;
        private Label onlineLinkLabel;
    }
}