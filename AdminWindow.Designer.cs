namespace StudentScheduleManagementSystem.UI
{
    partial class AdminWindow
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
            this.AddSchedule = new System.Windows.Forms.Button();
            this.MapOperator = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddSchedule
            // 
            this.AddSchedule.Location = new System.Drawing.Point(62, 77);
            this.AddSchedule.Name = "AddSchedule";
            this.AddSchedule.Size = new System.Drawing.Size(144, 34);
            this.AddSchedule.TabIndex = 0;
            this.AddSchedule.Text = "AddSchedule";
            this.AddSchedule.UseVisualStyleBackColor = true;
            // 
            // MapOperator
            // 
            this.MapOperator.Location = new System.Drawing.Point(62, 309);
            this.MapOperator.Name = "MapOperator";
            this.MapOperator.Size = new System.Drawing.Size(144, 34);
            this.MapOperator.TabIndex = 1;
            this.MapOperator.Text = "MapOperator";
            this.MapOperator.UseVisualStyleBackColor = true;
            this.MapOperator.Click += new System.EventHandler(this.MapOperator_Click);
            // 
            // AdminWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 669);
            this.Controls.Add(this.MapOperator);
            this.Controls.Add(this.AddSchedule);
            this.Name = "AdminWindow";
            this.Text = "AdminWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private Button AddSchedule;
        private Button MapOperator;
    }
}