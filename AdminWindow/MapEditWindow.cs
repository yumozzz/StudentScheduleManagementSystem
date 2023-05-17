using System.Drawing;
using System.Net;

namespace StudentScheduleManagementSystem.UI
{
    public partial class MapEditWindow : Form
    {
        private const int SmallCircRad = 5;
        private const int BigCircRad = 6;

        private HashSet<(Point, Point)> _lineEndPointPairs;
        private HashSet<Point> _points;
        private int? _xLock = null, _yLock = null;
        private Point? _mouseOver = null, _selected = null;

        public MapEditWindow()
        {
            /*_lineEndPointPairs = Map.Location.GetLineEndPoints()
                                    .Select(pair => (pair.Item1.ToPoint(), pair.Item2.ToPoint()))
                                    .ToHashSet();*/
            _lineEndPointPairs = new();
            _points = new();
            foreach (var pair in _lineEndPointPairs)
            {
                _points.Add(pair.Item1);
                _points.Add(pair.Item2);
            }
            InitializeComponent();
            Button buttonOK = new() { Text = "OK", Name = "OK", Location = new(0, 0), Size = new(150, 45) };
            Controls.Add(buttonOK);
            Button buttonCancel = new() { Text = "Cancel", Name = "Cancel", Location = new(628, 0), Size = new(150, 45) };
            Controls.Add(buttonCancel);
            this.KeyDown += OnKeyDown;
            pictureBox1.MouseDown += OnMouseDown;
            Thread thread = new(() =>
            {
                while (true)
                {
                    if (pictureBox1.IsDisposed)
                    {
                        return;
                    }
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

            foreach (var center in _points)
            {
                graphics.FillEllipse(brush, new() { Location = new(center.X - SmallCircRad, center.Y - SmallCircRad), Size = new(2 * SmallCircRad, 2 * SmallCircRad) });
            }
            if (_selected.HasValue)
            {
                graphics.FillEllipse(new SolidBrush(Color.Green), new() { Location = new(_selected.Value.X - SmallCircRad, _selected.Value.Y - SmallCircRad), Size = new(2 * SmallCircRad, 2 * SmallCircRad) });
            }
            foreach (var endPoints in _lineEndPointPairs)
            {
                graphics.DrawLine(pen,
                                  new(endPoints.Item1.X, endPoints.Item1.Y),
                                  new(endPoints.Item2.X, endPoints.Item2.Y));
            }
            Point circleCenter = pictureBox1.PointToClient(Control.MousePosition);
            circleCenter.X -= BigCircRad;
            circleCenter.Y -= BigCircRad;
            foreach (var center in _points)
            {
                if (Distance(circleCenter, center) < 14.0)
                {
                    graphics.FillEllipse(new SolidBrush(Color.Blue), new() { Location = new(center.X - BigCircRad, center.Y - BigCircRad), Size = new(2 * BigCircRad, 2 * BigCircRad) });
                    _mouseOver = center;
                    return;
                }
            }
            _mouseOver = null;
            graphics.FillEllipse(brush,
                                 new()
                                 {
                                     X = _xLock == null ? circleCenter.X : _xLock.Value - BigCircRad,
                                     Y = _yLock == null ? circleCenter.Y : _yLock.Value - BigCircRad,
                                     Size = new(2 * BigCircRad, 2 * BigCircRad)
                                 }); ;
        }

        private void OnMouseDown(object sender, EventArgs e)
        {
            Point mousePositionToPicture = pictureBox1.PointToClient(Control.MousePosition);
            if (_xLock.HasValue)
            {
                mousePositionToPicture.X = _xLock.Value;
            }
            if (_yLock.HasValue)
            {
                mousePositionToPicture.Y = _yLock.Value;
            }
            if (_mouseOver == null)
            {
                
                _points.Add(mousePositionToPicture);
                Console.WriteLine("add point");
            }
            else
            {
                if (_selected == null)
                {
                    _selected = _mouseOver;
                    Console.WriteLine("select");
                }
                else
                {
                    Console.WriteLine("add line");
                    _lineEndPointPairs.Add((_selected.Value,
                                            _mouseOver.Value));
                    _selected = null;
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    //TODO:改变窗口显示
                    _xLock = _xLock.HasValue ? null : pictureBox1.PointToClient(Control.MousePosition).X;
                    e.Handled = true;
                    break;
                case Keys.ControlKey:
                    _yLock = _yLock.HasValue ? null : pictureBox1.PointToClient(Control.MousePosition).Y;
                    e.Handled = true;
                    break;
                case Keys.Delete:
                    if (_selected == null)
                    {
                        break;
                    }
                    _points.Remove(_selected.Value);
                    int i = 0;
                    var list = _lineEndPointPairs.ToList();
                    while (true)
                    {
                        if (i == list.Count)
                        {
                            break;
                        }
                        if (list[i].Item1 == _selected.Value || list[i].Item2 == _selected.Value)
                        {
                            list.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                    _lineEndPointPairs = list.ToHashSet();
                    _selected = null;
                    UpdateGraphics();
                    break;
            }
        }
    }
}