namespace StudentScheduleManagementSystem.UI
{
    public partial class MultiSelectBox : UserControl
    {
        private const int GapHeight = 24;
        private const int HideHeight = 30;
        private int _showHeight;

        public int TotalCount { get; private set; } = 0;
        public int ValidCount { get; private set; } = 0;
        public bool[] Selects { get; private set; }
        public bool ShowComboBox { get; set; } = false;

        private CheckBox[] _boxes;

        public MultiSelectBox()
        {
            InitializeComponent();
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

        private void SelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetAllValid();
        }

        private void Cancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClearBox();
        }

        private void MultiSelectBox_Load(object sender, EventArgs e)
        {
            this.Height = HideHeight;
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            if (ShowComboBox == false)
            {
                pictureBox1_Click(sender, e);
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

        public void ClearBox()
        {
            foreach (var box in _boxes)
            {
                box.Checked = false;
            }
            UpdateTextBox();
        }

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