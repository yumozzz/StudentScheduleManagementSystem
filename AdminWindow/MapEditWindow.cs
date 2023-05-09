namespace StudentScheduleManagementSystem.UI
{
    public partial class MapEditWindow : MapWindow
    {
        public MapEditWindow(List<(Map.Location.Vertex, Map.Location.Vertex)> lineEndPointPairs,
                             List<(Map.Location.Vertex, Point, Point, Map.Location.Vertex)> bezCurveControlPointTuples)
            : base(lineEndPointPairs, bezCurveControlPointTuples)
        {
            InitializeComponent();
            Button buttonOK = new() { Text = "OK", Name = "OK", Location = new(0, 0),Size=new(150, 45) };
            Controls.Add(buttonOK);
            Button buttonCancel = new() { Text = "Cancel", Name = "Cancel", Location = new(628, 0), Size = new(150, 45) };
            Controls.Add(buttonCancel);
        }
    }
}