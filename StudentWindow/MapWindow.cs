using System.Net;

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

        private void DrawPoint(Graphics graphics, Point point)
        {
            graphics.FillEllipse(new SolidBrush(Color.Red),
                                 new()
                                 {
                                     Location = new(point.X - 5, point.Y - 5),
                                     Size = new(10, 10)
                                 });
        }

        /// <summary>
        /// 绘制路径
        /// </summary>
        protected void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            using Pen penLine = new(Color.Red, 4);

            foreach (var endPoints in _lineEndPointPairs)
            {
                graphics.DrawLine(penLine,
                                  endPoints.Item1.ToPoint(),
                                  endPoints.Item2.ToPoint());
                DrawPoint(graphics, endPoints.Item1.ToPoint());
                DrawPoint(graphics, endPoints.Item2.ToPoint());
            }
            foreach (var controlPoints in _bezCurveControlPointTuples)
            {
                graphics.DrawBezier(penLine,
                                    controlPoints.Item1.ToPoint(),
                                    controlPoints.Item2,
                                    controlPoints.Item3,
                                    controlPoints.Item4.ToPoint());
                DrawPoint(graphics, controlPoints.Item1.ToPoint());
                DrawPoint(graphics, controlPoints.Item4.ToPoint());
            }
        }
    }
}