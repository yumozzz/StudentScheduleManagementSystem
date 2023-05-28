#define SHOWPOINTID

namespace StudentScheduleManagementSystem.UI
{
    public partial class MapEditWindow : Form
    {
        private const int SmallCircRad = 5;
        private const int BigCircRad = 6;

        private HashSet<(Point, Point)> _lineEndPointPairs;
        private Dictionary<Point, (string, Label)?> _points;
        private TextBox _textBox = new() { Name = "TextBox", Size = new Size(100, 30) };

        private int? _xLock = null, _yLock = null;
        private Point? _mouseOver = null, _selected = null;
        private bool _showLabels = false;
        private bool _isInputting = false;
        private bool _needHelp = false;
        private bool _isAlive = true;
        private static readonly object _lock = new();

        public MapEditWindow()
        {
            InitializeComponent();
            _lineEndPointPairs = Map.Location.GetEdges()
                                    .ToHashSet();
            _points = new();
            #if SHOWPOINTID
            foreach (var vertex in Map.Location.GetVertices())
            {
                try
                {
                    _points.Add(vertex.ToPoint(), null);
                    Label label = new()
                    {
                        Location = new((vertex.X + 5), (vertex.Y + 20)),
                        Size = new(50, 24),
                        Text = vertex.Id.ToString(),
                        Font = fontLabel.Font,
                        AutoSize = true,
                        BackColor = Color.FromArgb(50, 168, 128, 194)
                    };
                    Controls.Add(label);
                    label.BringToFront();
                    label.Show();
                }
                catch (ArgumentException) { }
            }
            #else
            foreach (var building in Map.Location.Buildings)
            {
                Label label = new()
                {
                    Location = new((int)((building.Center.ToPoint().X + 20) / 1.5), (int)((building.Center.ToPoint().Y + 20) / 1.5)),
                    Size = new(100, 24),
                    Text = building.Name,
                    AutoSize = true,
                    BackColor = Color.FromArgb(50, 168, 128, 194)
                };
                _points.Add(building.Center.ToPoint(), (building.Name, label));
                label.BringToFront();
                label.Hide();
                Controls.Add(label);
            }
            foreach (var point in Map.Location.GetPoints())
            {
                try
                {
                    _points.Add(point, null);
                }
                catch (ArgumentException) { }
            }
            #endif
            Button buttonOk = new() { Text = "OK", Name = "OK", Location = new(0, 0), Size = new(150, 45) };
            Button buttonCancel =
                new() { Text = "Cancel", Name = "Cancel", Location = new(628, 0), Size = new(150, 45) };
            buttonOk.Click += (sender, e) =>
            {
                lock (_lock)
                {
                    Map.Location.GlobalMap = new(GetVertices(), GetEdges());
                    Map.Location.Buildings = GetBuildings();
                    this.Close();
                }
            };
            buttonCancel.Click += (sender, e) =>
            {
                lock (_lock)
                {
                    this.Close();
                }
            };
            Controls.Add(buttonOk);
            Controls.Add(buttonCancel);
            helpButton.Location = new(528, 0);
            this.KeyDown += OnKeyDown;
            pictureBox1.MouseDown += OnMouseDown;
            Thread thread = new(() =>
            {
                Thread.Sleep(100);
                while (_isAlive)
                {
                    DateTime begin = DateTime.Now;
                    lock (_lock)
                    {
                        if (pictureBox1.InvokeRequired)
                        {
                            this.pictureBox1.Invoke(UpdateGraphics);
                        }
                        else
                        {
                            UpdateGraphics();
                        }
                    }
                    DateTime end = DateTime.Now;
                    Thread.Sleep((end - begin) / 2);
                }
            });
            Controls.Add(_textBox);
            _textBox.BringToFront();
            _textBox.Hide();
            warnPictureBox.Hide();
            helpPictureBox.Hide();
            helpPictureBox.SendToBack();
            thread.Start();
        }

        public new void Close()
        {
            _isAlive = false;
            base.Close();
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

            foreach ((var center, _) in _points)
            {
                graphics.FillEllipse(brush,
                                     new()
                                     {
                                         Location = new(center.X - SmallCircRad, center.Y - SmallCircRad),
                                         Size = new(2 * SmallCircRad, 2 * SmallCircRad)
                                     });
            }
            if (_selected.HasValue)
            {
                graphics.FillEllipse(new SolidBrush(Color.Green),
                                     new()
                                     {
                                         Location = new(_selected.Value.X - SmallCircRad,
                                                        _selected.Value.Y - SmallCircRad),
                                         Size = new(2 * SmallCircRad, 2 * SmallCircRad)
                                     });
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
            foreach ((var center, _) in _points)
            {
                if (Distance(circleCenter, center) < 12.0)
                {
                    graphics.FillEllipse(new SolidBrush(Color.Blue),
                                         new()
                                         {
                                             Location = new(center.X - BigCircRad, center.Y - BigCircRad),
                                             Size = new(2 * BigCircRad, 2 * BigCircRad)
                                         });
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
                                 });
        }

        private void OnMouseDown(object sender, EventArgs e)
        {
            if (_isInputting)
            {
                return;
            }
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
                _points.Add(mousePositionToPicture, null);
            }
            else
            {
                if (_selected == null)
                {
                    _selected = _mouseOver;
                    foreach (var tuple in _points.Values)
                    {
                        tuple?.Item2.Hide();
                    }
                }
                else if (_selected != _mouseOver)
                {
                    bool success;
                    if ((_selected.Value.X, _selected.Value.Y).CompareTo((_mouseOver.Value.X, _mouseOver.Value.Y)) <= 0)
                    {
                        success = _lineEndPointPairs.Add((_selected.Value, _mouseOver.Value));
                    }
                    else
                    {
                        success = _lineEndPointPairs.Add((_mouseOver.Value, _selected.Value));
                    }
                    _selected = null;
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    if (_textBox.Visible)
                    {
                        return;
                    }
                    _xLock = _xLock.HasValue
                                 ? null
                                 : (_selected?.X ?? pictureBox1.PointToClient(Control.MousePosition).X);
                    _selected = null;
                    e.Handled = true;
                    if (_xLock.HasValue)
                    {
                        _selected = null;
                        warnPictureBox.Image = imageList.Images[0];
                        warnPictureBox.Show();
                        warnPictureBox.BringToFront();
                    }
                    else if (_yLock.HasValue)
                    {
                        _selected = null;
                        warnPictureBox.Image = imageList.Images[1];
                        warnPictureBox.Show();
                        warnPictureBox.BringToFront();
                    }
                    else
                    {
                        warnPictureBox.Hide();
                    }
                    break;
                case Keys.ControlKey:
                    if (_textBox.Visible)
                    {
                        return;
                    }
                    _yLock = _yLock.HasValue
                                 ? null
                                 : (_selected?.Y ?? pictureBox1.PointToClient(Control.MousePosition).Y);
                    _selected = null;
                    e.Handled = true;
                    if (_yLock.HasValue)
                    {
                        warnPictureBox.Image = imageList.Images[1];
                        warnPictureBox.Show();
                        warnPictureBox.BringToFront();
                    }
                    else if (_xLock.HasValue)
                    {
                        warnPictureBox.Image = imageList.Images[0];
                        warnPictureBox.Show();
                        warnPictureBox.BringToFront();
                    }
                    else
                    {
                        warnPictureBox.Hide();
                    }
                    break;
                case Keys.Delete:
                    if (!_selected.HasValue)
                    {
                        break;
                    }
                    Console.WriteLine("delete point");
                    _points[_selected.Value]?.Item2.Dispose();
                    Controls.Remove(_points[_selected.Value]?.Item2!);
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
                    e.Handled = true;
                    break;
                case Keys.Escape:
                    _selected = null;
                    _textBox.Text = "";
                    _textBox.Hide();
                    _isInputting = false;
                    e.Handled = true;
                    break;
                case Keys.Insert:
                    if (_selected == null)
                    {
                        break;
                    }
                    _isInputting = true;
                    _textBox.Location = new(_selected.Value.X + 20, _selected.Value.Y + 20);
                    _textBox.Text = _points[_selected.Value]?.Item1 ?? string.Empty;
                    _textBox.Show();
                    e.Handled = true;
                    break;
                case Keys.Return:
                    if (!_isInputting)
                    {
                        break;
                    }
                    Label information = new()
                    {
                        Location = _textBox.Location,
                        Size = new(100, 24),
                        Text = _textBox.Text,
                        AutoSize = true,
                        BackColor = Color.FromArgb(50, 168, 128, 194)
                    };
                    _points[_selected!.Value] = (_textBox.Text, information);
                    Controls.Add(information);
                    information.BringToFront();
                    information.Show();
                    _isInputting = false;
                    _selected = null;
                    _textBox.Hide();
                    goto alt; //identical to a missing break in C/C++
                case Keys.Menu:
                alt:
                    if (_isInputting)
                    {
                        return;
                    }
                    foreach (var tuple in _points.Values)
                    {
                        if (!tuple.HasValue)
                        {
                            continue;
                        }
                        if (_showLabels)
                        {
                            tuple.Value.Item2.Show();
                        }
                        else
                        {
                            tuple.Value.Item2.Hide();
                        }
                    }
                    _showLabels = !_showLabels;
                    e.Handled = true;
                    break;
            }
        }

        private Dictionary<Point, int> _pointIds;

        private void GenerateIdDictionary()
        {
            _pointIds = new();
            int index = 0;
            foreach (var point in _points.Keys)
            {
                _pointIds.Add(point, index++);
            }
        }

        public List<Map.Location.Vertex> GetVertices()
        {
            GenerateIdDictionary();
            return _pointIds.Select(kvPair => kvPair.Key.ToVertex(kvPair.Value)).ToList();
        }

        public List<(Map.Location.Vertex, Map.Location.Vertex)> GetEdges()
        {
            GenerateIdDictionary();
            return _lineEndPointPairs.Select(tuple => (tuple.Item1.ToVertex(_pointIds[tuple.Item1]),
                                                       tuple.Item2.ToVertex(_pointIds[tuple.Item2])))
                                     .ToList();
        }

        public List<Map.Location.Building> GetBuildings()
        {
            GenerateIdDictionary();
            List<Map.Location.Building> ret = new();
            int index = 0;
            foreach (var kvPair in _points)
            {

                if (!kvPair.Value.HasValue)
                {
                    continue;
                }
                ret.Add(new(index++, kvPair.Value.Value.Item1, kvPair.Key.ToVertex(_pointIds[kvPair.Key])));
            }
            return ret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_needHelp)
            {
                helpPictureBox.Hide();
                _needHelp = false;
            }
            else
            {
                helpPictureBox.Show();
                helpPictureBox.BringToFront();
                _needHelp = true;
            }
        }
    }
}