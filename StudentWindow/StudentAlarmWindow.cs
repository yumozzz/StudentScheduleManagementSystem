namespace StudentScheduleManagementSystem.UI
{
    public partial class StudentAlarmWindow : Form
    {
        private List<Map.Location.Building> _targetBuildings;

        public StudentAlarmWindow()
        {
            InitializeComponent();
        }

        public StudentAlarmWindow(string scheduleName, string onlineLink)
        {
            InitializeComponent();
            this.showMapButton.Hide();
            this.buildingComboBox.Hide();
            this.currentBuildingLabel.Hide();
            this.alarmName.Text = scheduleName;
            this.onlineLinkLinkLabel.Text = onlineLink;
        }

        public StudentAlarmWindow(string scheduleName, Map.Location.Building targetBuilding)
        {
            InitializeComponent();
            foreach (var building in Map.Location.Buildings)
            {
                buildingComboBox.Items.Add(building.Name);
            }
            this.alarmName.Text = "闹钟：" + scheduleName;
            this.onlineLinkLinkLabel.Hide();
            this.onlineLinkLabel.Hide();
            _targetBuildings = new() { targetBuilding };
        }

        public StudentAlarmWindow(string scheduleName, List<Map.Location.Building> targetBuildings)
        {
            InitializeComponent();
            foreach (var building in Map.Location.Buildings)
            {
                buildingComboBox.Items.Add(building.Name);
            }
            this.alarmName.Text = "闹钟：" + scheduleName;
            this.onlineLinkLinkLabel.Hide();
            this.onlineLinkLabel.Hide();
            _targetBuildings = targetBuildings;
        }

        /// <summary>
        /// 处理关键点，调用寻路函数
        /// </summary>
        private void ShowMapButton_Click(object sender, EventArgs e)
        {
            if (buildingComboBox.Text == "")
            {
                MessageBox.Show("请选择当前地点！");
                return;
            }
            Map.Location.Building currentBuilding = Map.Location.GetBuildingsByName(buildingComboBox.Text)[0];
            _targetBuildings.Add(currentBuilding);
            List<int> points = _targetBuildings.Count == 2 ? Map.Location.GetClosestPath(currentBuilding, _targetBuildings[0]) : Map.Location.GetClosestCircuit(_targetBuildings);
            this.Hide();
            Map.Navigate.Show(points);
            this.Close();
        }
    }
}
