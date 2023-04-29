namespace StudentScheduleManagementSystem.UI
{
    public partial class MapEditWindow : MapWindow
    {
        public MapEditWindow(List<(Map.Location.Vertex, Map.Location.Vertex)> lineEndPointPairs,
                             List<(Map.Location.Vertex, Point, Point, Map.Location.Vertex)> bezCurveControlPointTuples)
            : base(lineEndPointPairs, bezCurveControlPointTuples)
        {
            InitializeComponent();
        }
    }
}