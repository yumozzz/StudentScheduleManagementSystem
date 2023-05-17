namespace StudentScheduleManagementSystem.UI
{
    partial class MapEditWindow : Form
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditWindow));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.warmPictureBox = new System.Windows.Forms.PictureBox();
            this.helpPictureBox = new System.Windows.Forms.PictureBox();
            this.helpButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warmPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::StudentScheduleManagementSystem.Properties.Resources.SchoolMap;
            this.pictureBox1.Location = new System.Drawing.Point(0, 45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(779, 1097);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "xLockPicture.png");
            this.imageList.Images.SetKeyName(1, "yLockPicture.png");
            // 
            // warmPictureBox
            // 
            this.warmPictureBox.Location = new System.Drawing.Point(239, 5);
            this.warmPictureBox.Name = "warmPictureBox";
            this.warmPictureBox.Size = new System.Drawing.Size(300, 50);
            this.warmPictureBox.TabIndex = 1;
            this.warmPictureBox.TabStop = false;
            // 
            // helpPictureBox
            // 
            this.helpPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.helpPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("helpPictureBox.Image")));
            this.helpPictureBox.InitialImage = null;
            this.helpPictureBox.Location = new System.Drawing.Point(189, 275);
            this.helpPictureBox.Name = "helpPictureBox";
            this.helpPictureBox.Size = new System.Drawing.Size(400, 300);
            this.helpPictureBox.TabIndex = 3;
            this.helpPictureBox.TabStop = false;
            // 
            // helpButton
            // 
            this.helpButton.Location = new System.Drawing.Point(545, 5);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(100, 45);
            this.helpButton.TabIndex = 4;
            this.helpButton.Text = "Help";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // MapEditWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 1144);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.helpPictureBox);
            this.Controls.Add(this.warmPictureBox);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "MapEditWindow";
            this.Text = "MapEdit";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warmPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private ImageList imageList;
        private PictureBox warmPictureBox;
        private PictureBox helpPictureBox;
        private Button helpButton;
    }
}