using static StudentScheduleManagementSystem.UI.StudentWindow;

namespace StudentScheduleManagementSystem.UI
{
    public partial class MapEditWindow : Form
    {
        private HashSet<(Map.Location.Vertex, Map.Location.Vertex)> _lineEndPointPairs;
        private HashSet<Point> _points;
        private int? _xLock = null, _yLock = null;
        private Point? mouseOver = null, selected = null;

        public MapEditWindow()
        {
            //_lineEndPointPairs = Map.Location.GetLineEndPoints().ToHashSet();
            _lineEndPointPairs = new();
            _points = new();
            foreach (var pair in _lineEndPointPairs)
            {
                _points.Add(pair.Item1.ToPoint());
                _points.Add(pair.Item2.ToPoint());
            }
            InitializeComponent();
            Button buttonOK = new() { Text = "OK", Name = "OK", Location = new(0, 0), Size = new(150, 45) };
            Controls.Add(buttonOK);
            Button buttonCancel = new() { Text = "Cancel", Name = "Cancel", Location = new(628, 0), Size = new(150, 45) };
            Controls.Add(buttonCancel);
            this.KeyDown += switchXLock;
            this.KeyDown += switchYLock;
            pictureBox1.MouseDown += OnMouseDown;
            Thread thread = new(() =>
            {
                while (true)
                {
                    if (pictureBox1.InvokeRequired)
                    {
                        this.pictureBox1.Invoke(UpdateGraphics);
                    }
                    else
                    {
                        UpdateGraphics();
                    }
                    Thread.Sleep(100);
                }
            });
            thread.Start();
        }

        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        private void UpdateGraphics()
        {
            pictureBox1.Invalidate();
            Update();
            using Graphics graphics = pictureBox1.CreateGraphics();
            Pen pen = new(Color.Red, 2);
            Brush brush = new SolidBrush(Color.Red);

            foreach (var endPoints in _lineEndPointPairs)
            {
                graphics.DrawLine(pen,
                                  new(endPoints.Item1.X, endPoints.Item1.Y),
                                  new(endPoints.Item2.X, endPoints.Item2.Y));
                graphics.FillEllipse(brush, new() { X = endPoints.Item1.X, Y = endPoints.Item1.Y, Size = new(10, 10) });
                graphics.FillEllipse(brush, new() { X = endPoints.Item2.X, Y = endPoints.Item2.Y, Size = new(10, 10) });
            }
            Point mousePosition = this.PointToClient(Control.MousePosition);
            foreach (var point in _points)
            {
                if (Distance(mousePosition, point) < 8.0)
                {
                    graphics.FillEllipse(brush, new() { X = point.X, Y = point.Y, Size = new(20, 20) });
                    mouseOver = point;
                    return;
                }
            }
            graphics.FillEllipse(brush,
                                 new()
                                 {
                                     X = _xLock ?? mousePosition.X - 10,
                                     Y = _yLock ?? mousePosition.Y - 55,
                                     Size = new(20, 20)
                                 });
        }

        private void OnMouseDown(object sender, EventArgs e)
        {

        }

        private void switchXLock(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.LShiftKey)
            {
                e.Handled = true;
            }
            _xLock = _xLock.HasValue ? null : Control.MousePosition.X;
            //TODO:改变窗口显示
        }

        private void switchYLock(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.LControlKey)
            {
                e.Handled = true;
            }
            _yLock = _yLock.HasValue ? null : Control.MousePosition.Y;
            //TODO:改变窗口显示
        }
    }
}