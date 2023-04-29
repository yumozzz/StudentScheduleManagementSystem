﻿using StudentScheduleManagementSystem.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentScheduleManagementSystem.UI
{
    public partial class MapOPWindow : StudentScheduleManagementSystem.UI.MapWindow
    {
        /*
        public MapOPWindow()
        {
            InitializeComponent();
        }
        */
        public MapOPWindow(List<(Location.Vertex, Location.Vertex)> lineEndPointPairs, List<(Location.Vertex, Point, Point, Location.Vertex)> bezCurveControlPointTuples) : base(lineEndPointPairs, bezCurveControlPointTuples)
        {
            InitializeComponent();
        }
    }
}