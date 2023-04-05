namespace StudentScheduleManagementSystem.UI
{
    public partial class MapWindow : Form
    {
        private List<(Map.Location.Vertex, Map.Location.Vertex)> _lineEndPointPairs;
        private List<(Map.Location.Vertex, Map.Location.Vertex, Map.Location.Vertex, Map.Location.Vertex)>
            _bezCurveControlPointTuples;

        public MapWindow(List<(Map.Location.Vertex, Map.Location.Vertex)> lineEndPointPairs,
                         List<(Map.Location.Vertex, Map.Location.Vertex, Map.Location.Vertex, Map.Location.Vertex)>
                             bezCurveControlPointTuples)
        {
            _lineEndPointPairs = lineEndPointPairs;
            _bezCurveControlPointTuples = bezCurveControlPointTuples;
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;　　　　
            Pen pen = new(Color.Red, 2);
            foreach (var endPoints in _lineEndPointPairs)
            {
                graphics.DrawLine(pen,
                                  new(endPoints.Item1.X, endPoints.Item1.Y),
                                  new(endPoints.Item2.X, endPoints.Item2.Y));
            }
            foreach (var controlPoints in _bezCurveControlPointTuples)
            {
                graphics.DrawBezier(pen,
                                    new(controlPoints.Item1.X, controlPoints.Item1.Y),
                                    new(controlPoints.Item2.X, controlPoints.Item2.Y),
                                    new(controlPoints.Item3.X, controlPoints.Item3.Y),
                                    new(controlPoints.Item4.X, controlPoints.Item4.Y));
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}