namespace StudentScheduleManagementSystem.UI
{
    public partial class MapWindow : Form
    {
        private List<(Map.Location.Vertex, Map.Location.Vertex)> _lineEndPointPairs;
        private List<(Map.Location.Vertex, Point, Point, Map.Location.Vertex)> _bezCurveControlPointTuples;

        public MapWindow()
        {
            _lineEndPointPairs = new();
            _bezCurveControlPointTuples = new();
            InitializeComponent();
        }

        public MapWindow(List<(Map.Location.Vertex, Map.Location.Vertex)> lineEndPointPairs,
                         List<(Map.Location.Vertex, Point, Point, Map.Location.Vertex)>
                             bezCurveControlPointTuples)
        {
            _lineEndPointPairs = lineEndPointPairs;
            _bezCurveControlPointTuples = bezCurveControlPointTuples;
            InitializeComponent();
        }

        protected void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            using Pen pen = new(Color.Red, 2);

            foreach (var endPoints in _lineEndPointPairs)
            {
                graphics.DrawLine(pen,
                                  endPoints.Item1.ToPoint(),
                                  endPoints.Item2.ToPoint());
            }
            foreach (var controlPoints in _bezCurveControlPointTuples)
            {
                graphics.DrawBezier(pen,
                                    controlPoints.Item1.ToPoint(),
                                    controlPoints.Item2,
                                    controlPoints.Item3,
                                    controlPoints.Item4.ToPoint());
            }
        }
    }
}