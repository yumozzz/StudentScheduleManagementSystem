namespace StudentScheduleManagementSystem.UI
{
    public partial class MapSubwindow : Form
    {
        private List<(Map.Location.Vertex, Map.Location.Vertex)> _lineEndPointPairs;
        private List<(Map.Location.Vertex, Point, Point, Map.Location.Vertex)> _bezCurveControlPointTuples;

        public MapSubwindow(List<(Map.Location.Vertex, Map.Location.Vertex)> lineEndPointPairs,
                         List<(Map.Location.Vertex, Point, Point, Map.Location.Vertex)>
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
                                    controlPoints.Item2,
                                    controlPoints.Item3,
                                    new(controlPoints.Item4.X, controlPoints.Item4.Y));
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}