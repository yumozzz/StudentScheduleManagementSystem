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
            this.alarmName = new System.Windows.Forms.Label();
            this.buildingComboBox = new System.Windows.Forms.ComboBox();
            this.currentBuildingLabel = new System.Windows.Forms.Label();
            this.showMapButton = new System.Windows.Forms.Button();
            this.onlineLinkLinkLabel = new System.Windows.Forms.LinkLabel();
            this.onlineLinkLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // alarmName
            // 
            this.alarmName.AutoSize = true;
            this.alarmName.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.alarmName.ForeColor = System.Drawing.Color.White;
            this.alarmName.Location = new System.Drawing.Point(175, 62);
            this.alarmName.Name = "alarmName";
            this.alarmName.Size = new System.Drawing.Size(118, 24);
            this.alarmName.TabIndex = 0;
            this.alarmName.Text = "alarmName";
            // 
            // buildingComboBox
            // 
            this.buildingComboBox.FormattingEnabled = true;
            this.buildingComboBox.Location = new System.Drawing.Point(147, 136);
            this.buildingComboBox.Name = "buildingComboBox";
            this.buildingComboBox.Size = new System.Drawing.Size(182, 32);
            this.buildingComboBox.TabIndex = 1;
            // 
            // currentBuildingLabel
            // 
            this.currentBuildingLabel.AutoSize = true;
            this.currentBuildingLabel.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.currentBuildingLabel.ForeColor = System.Drawing.Color.White;
            this.currentBuildingLabel.Location = new System.Drawing.Point(38, 139);
            this.currentBuildingLabel.Name = "currentBuildingLabel";
            this.currentBuildingLabel.Size = new System.Drawing.Size(106, 24);
            this.currentBuildingLabel.TabIndex = 2;
            this.currentBuildingLabel.Text = "当前地点";
            // 
            // showMapButton
            // 
            this.showMapButton.BackColor = System.Drawing.Color.White;
            this.showMapButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showMapButton.Location = new System.Drawing.Point(332, 136);
            this.showMapButton.Name = "showMapButton";
            this.showMapButton.Size = new System.Drawing.Size(112, 34);
            this.showMapButton.TabIndex = 3;
            this.showMapButton.Text = "显示导航";
            this.showMapButton.UseVisualStyleBackColor = false;
            this.showMapButton.Click += new System.EventHandler(this.ShowMapButton_Click);
            // 
            // onlineLinkLinkLabel
            // 
            this.onlineLinkLinkLabel.AutoSize = true;
            this.onlineLinkLinkLabel.Location = new System.Drawing.Point(147, 140);
            this.onlineLinkLinkLabel.Name = "onlineLinkLinkLabel";
            this.onlineLinkLinkLabel.Size = new System.Drawing.Size(98, 24);
            this.onlineLinkLinkLabel.TabIndex = 4;
            this.onlineLinkLinkLabel.TabStop = true;
            this.onlineLinkLinkLabel.Text = "linkLabel1";
            // 
            // onlineLinkLabel
            // 
            this.onlineLinkLabel.AutoSize = true;
            this.onlineLinkLabel.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.onlineLinkLabel.ForeColor = System.Drawing.Color.White;
            this.onlineLinkLabel.Location = new System.Drawing.Point(38, 139);
            this.onlineLinkLabel.Name = "onlineLinkLabel";
            this.onlineLinkLabel.Size = new System.Drawing.Size(106, 24);
            this.onlineLinkLabel.TabIndex = 5;
            this.onlineLinkLabel.Text = "课程链接";
            // 
            // StudentAlarmWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(128)))), ((int)(((byte)(194)))));
            this.ClientSize = new System.Drawing.Size(478, 244);
            this.Controls.Add(this.buildingComboBox);
            this.Controls.Add(this.onlineLinkLinkLabel);
            this.Controls.Add(this.showMapButton);
            this.Controls.Add(this.currentBuildingLabel);
            this.Controls.Add(this.alarmName);
            this.Controls.Add(this.onlineLinkLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "StudentAlarmWindow";
            this.Text = "闹钟";
            this.ResumeLayout(false);
            this.PerformLayout();

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