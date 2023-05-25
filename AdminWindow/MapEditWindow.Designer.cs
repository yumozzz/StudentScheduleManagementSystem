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
            lock (_lock)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditWindow));
            pictureBox1 = new PictureBox();
            imageList = new ImageList(components);
            warnPictureBox = new PictureBox();
            helpPictureBox = new PictureBox();
            helpButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)warnPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)helpPictureBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.SchoolMap;
            pictureBox1.Location = new Point(0, 30);
            pictureBox1.Margin = new Padding(2, 2, 2, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(519, 731);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // imageList
            // 
            imageList.ColorDepth = ColorDepth.Depth8Bit;
            imageList.ImageStream = (ImageListStreamer)resources.GetObject("imageList.ImageStream");
            imageList.TransparentColor = Color.Transparent;
            imageList.Images.SetKeyName(0, "xLockPicture.png");
            imageList.Images.SetKeyName(1, "yLockPicture.png");
            // 
            // warnPictureBox
            // 
            warnPictureBox.Location = new Point(159, 3);
            warnPictureBox.Margin = new Padding(2, 2, 2, 2);
            warnPictureBox.Name = "warnPictureBox";
            warnPictureBox.Size = new Size(200, 33);
            warnPictureBox.TabIndex = 1;
            warnPictureBox.TabStop = false;
            // 
            // helpPictureBox
            // 
            helpPictureBox.BackColor = Color.Transparent;
            helpPictureBox.Image = (Image)resources.GetObject("helpPictureBox.Image");
            helpPictureBox.InitialImage = null;
            helpPictureBox.Location = new Point(126, 183);
            helpPictureBox.Margin = new Padding(2, 2, 2, 2);
            helpPictureBox.Name = "helpPictureBox";
            helpPictureBox.Size = new Size(267, 200);
            helpPictureBox.TabIndex = 3;
            helpPictureBox.TabStop = false;
            // 
            // helpButton
            // 
            helpButton.Location = new Point(363, 3);
            helpButton.Margin = new Padding(2, 2, 2, 2);
            helpButton.Name = "helpButton";
            helpButton.Size = new Size(67, 30);
            helpButton.TabIndex = 4;
            helpButton.Text = "Help";
            helpButton.UseVisualStyleBackColor = true;
            helpButton.Click += button1_Click;
            // 
            // MapEditWindow
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(519, 699);
            Controls.Add(helpButton);
            Controls.Add(helpPictureBox);
            Controls.Add(warnPictureBox);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            Margin = new Padding(2, 2, 2, 2);
            Name = "MapEditWindow";
            Text = "MapEdit";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)warnPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)helpPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private ImageList imageList;
        private PictureBox warnPictureBox;
        private PictureBox helpPictureBox;
        private Button helpButton;
    }
}