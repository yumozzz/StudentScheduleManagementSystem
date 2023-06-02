namespace StudentScheduleManagementSystem.UI
{
    public partial class MultiSelectBox : UserControl
    {
        private const int GapHeight = 24;
        private const int HideHeight = 30;
        private int _showHeight;

        /// <summary>
        /// 选项的总个数
        /// </summary>
        public int TotalCount { get; private set; } = 0;
        /// <summary>
        /// 选中项的总个数
        /// </summary>
        public int ValidCount { get; private set; } = 0;
        /// <summary>
        /// 每个选项的选中状态
        /// </summary>
        public bool[] Selects { get; private set; }
        /// <summary>
        /// 当前是否展示下拉框
        /// </summary>
        public bool ShowComboBox { get; set; } = false;

        private CheckBox[] _boxes;

        public MultiSelectBox()
        {
            InitializeComponent();
            this.selectAll.LinkClicked += (sender, e) => SetAllValid();
            this.cancel.LinkClicked += (sender, e) => ClearBox();
            this.textBox.Click += (sender, e) =>
            {
                if (ShowComboBox == false)
                {
                    pictureBox1_Click(sender, e);
                }
            };
            this.Load += (sender, e) => this.Height = HideHeight;
        }

        public void InitializeBox(string[] texts)
        {
            this.TotalCount = texts.Length;
            Selects = new bool[texts.Length];
            _boxes = new CheckBox[texts.Length];

            for (int i = 0; i < texts.Length; i++)
            {
                CheckBox box = new();
                box.Location = new(3, HideHeight + GapHeight + i * 28);
                box.Name = texts[i];
                box.Size = new(300, 28);
                box.TabIndex = 1;
                box.Text = texts[i];
                box.UseVisualStyleBackColor = true;
                box.CheckedChanged += (sender, e) => { UpdateTextBox(); };
                box.MouseLeave += (sender, e) => { box.BackColor = Color.White; };
                box.MouseMove += (sender, e) => { box.BackColor = Color.DodgerBlue; };
                Controls.Add(box);
                _boxes[i] = box;
            }

            _showHeight = HideHeight + texts.Length * _boxes[0].Height + GapHeight;
            pictureBox2.Hide();
        }

        private void UpdateTextBox()
        {
            ValidCount = 0;
            textBox.Text = "";
            for (int i = 0; i < TotalCount; i++)
            {
                this.Selects[i] = _boxes[i].Checked;
                if (this.Selects[i])
                {
                    textBox.Text += _boxes[i].Text + ";";
                    ValidCount++;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (ShowComboBox)
            {
                this.Height = HideHeight;
                ShowComboBox = false;
                pictureBox2.Hide();
            }
            else
            {
                this.Height = _showHeight;
                ShowComboBox = true;
                pictureBox2.Show();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (ShowComboBox)
            {
                this.Height = HideHeight;
                ShowComboBox = false;
                pictureBox2.Hide();
            }
            else
            {
                this.Height = _showHeight;
                ShowComboBox = true;
                pictureBox2.Show();
            }
        }

        /// <summary>
        /// 选中一个选项
        /// </summary>
        public bool SelectCheckBox(int index)
        {
            if (index >= this.TotalCount || index < 0)
            {
                return false;
            }

            Selects[index] = true;
            _boxes[index].Checked = true;
            UpdateTextBox();

            return true;
        }

        /// <summary>
        /// 清空选中
        /// </summary>
        public void ClearBox()
        {
            foreach (var box in _boxes)
            {
                box.Checked = false;
            }
            UpdateTextBox();
        }

        /// <summary>
        /// 设置所有项为选中
        /// </summary>
        public void SetAllValid()
        {
            foreach (var box in _boxes)
            {
                box.Checked = true;
            }
            UpdateTextBox();
        }
    }
}