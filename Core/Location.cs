using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace StudentScheduleManagementSystem.Map
{
    public static class Location
    {
        private static List<Building> _buildings = new() { Constants.DefaultBuilding };

        public static List<Building> Buildings
        {
            get => _buildings.GetRange(1, _buildings.Count - 1);
            set => _buildings = new List<Building> { Constants.DefaultBuilding }.Concat(value).ToList();
        }
        public static AdjacencyTable? GlobalMap { get; set; }

        static Location()
        {
            (JArray, JArray) information;
            try
            {
                information = FileManagement.FileManager.ReadFromMapFile(FileManagement.FileManager.MapFileDirectory);
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException or FileNotFoundException)
            {
                return;
            }
            GlobalMap = new(information.Item1);
            foreach (JObject obj in information.Item2)
            {
                int id = obj["Id"]!.Value<int>();
                string name = obj["Name"]!.Value<string>()!;
                Building building = new(id, name, GlobalMap.GetVertex(obj["CenterId"]!.Value<int>()));
                Buildings.Add(building);
            }
        }

        #region structs, classes and enums

        public struct Edge //边
        {
            public EdgeType Type { get; set; }
            public (Point, Point)? Controls { get; set; }
            public int Weight { get; set; }

            public Edge(EdgeType type, (Point, Point)? controls, int weight)
            {
                Type = type;
                Controls = controls;
                Weight = weight;
            }
        }

        public struct Vertex //点
        {
            public int Id { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public Vertex(int id, int x, int y)
            {
                Id = id;
                X = x;
                Y = y;
            }

            public Point ToPoint()
            {
                return new Point(X, Y);
            }

            public override bool Equals(object? obj)
            {
                if (obj is Vertex v)
                {
                    return Id == v.Id && X == v.X && Y == v.Y;
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int result = 37;
                    result *= 397;
                    result += Id.GetHashCode();
                    result *= 397;
                    result += X.GetHashCode();
                    result *= 397;
                    result += Y.GetHashCode();
                    return result;
                }
            }
        }

        public struct Building //建筑
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Vertex Center { get; set; }

            public Building(int id, string name, Vertex center)
            {
                Id = id;
                Name = name;
                Center = center;
            }

            public Building()
            {
                throw new InvalidOperationException("default constructor for struct Building should not be called");
            }

            public static bool operator ==(Building left, Building right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Building left, Building right)
            {
                return !left.Equals(right);
            }

            public override bool Equals(object? obj)
            {
                if (obj is Building b)
                {
                    return Id == b.Id && Name == b.Name && Center.Equals(b.Center);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int result = 37;
                    result *= 397;
                    result += Id.GetHashCode();
                    result *= 397;
                    result += Name.GetHashCode();
                    result *= 397;
                    result += Center.GetHashCode();
                    return result;
                }
            }
        }

        public class AdjacencyTable
        {
            private struct Node
            {
                public int pointId;
                public Edge edge;
            }

            public int Size { get; init; }
            private readonly List<Node>[] _adjArray;
            private readonly Point[] _locations;

            public AdjacencyTable(List<Vertex> vertices, List<(Vertex, Vertex)> edges)
            {
                Size = vertices.Count;
                _adjArray = new List<Node>[Size];
                for (int i = 0; i < Size; i++)
                {
                    _adjArray[i] = new();
                }
                _locations = vertices.ConvertAll(vertex => new Point(vertex.X, vertex.Y)).ToArray();

                foreach (var (from, to) in edges)
                {
                    int dist = (int)Math.Sqrt((from.X - to.X) * (from.X - to.X) + (from.Y - to.Y) * (from.Y - to.Y));
                    Edge edge = new(EdgeType.Line, null, dist);

                    Node node = new Node() { pointId = to.Id, edge = edge }; //from->to
                    _adjArray[from.Id].Add(node);

                    node.pointId = from.Id; //to->from
                    _adjArray[to.Id].Add(node);
                }
                foreach (var nodes in _adjArray)
                {
                    nodes.TrimExcess();
                }
            }

            public AdjacencyTable(JArray vertexArray)
            {
                Size = vertexArray.Count;
                _adjArray = new List<Node>[Size];
                for (int i = 0; i < Size; i++)
                {
                    _adjArray[i] = new();
                }
                _locations = new Point[Size];

                try
                {
                    foreach (JObject vertex in vertexArray)
                    {
                        int id = vertex["Id"]!.Value<int>();
                        int x = vertex["X"]!.Value<int>();
                        int y = vertex["Y"]!.Value<int>();
                        JArray nexts = (JArray)vertex["Nexts"]!;
                        if (nexts.Count == 0)
                        {
                            continue;
                        }
                        _locations[id] = new(x, y);
                        foreach (JObject next in nexts)
                        {
                            Node node = new()
                            {
                                pointId = next["Id"]!.Value<int>(),
                                edge = new()
                                {
                                    Weight = next["Distance"]!.Value<int>(),
                                    Type = next["EdgeType"]!.Value<string>() switch
                                    {
                                        "Line" => EdgeType.Line,
                                        "QuadraticBezierCurve" => EdgeType.QuadraticBezierCurve,
                                        _ => throw new JsonFormatException("Wrong EdgeType")
                                    },
                                    Controls = null,
                                }
                            };
                            if (node.pointId >= Size || node.pointId < 0)
                            {
                                throw new JsonFormatException("Wrong vertex id");
                            }
                            if (node.edge.Type == EdgeType.QuadraticBezierCurve)
                            {
                                JArray controls = (JArray)next["Controls"]!;
                                if (controls.Count != 2)
                                {
                                    throw new JsonFormatException("Too many control points");
                                }
                                var control1 = JsonConvert.DeserializeObject<Point>(controls.ElementAt(1).ToString());
                                var control2 = JsonConvert.DeserializeObject<Point>(controls.ElementAt(2).ToString());
                                node.edge.Controls = (control1, control2);
                            }
                            _adjArray[id].Add(node);
                        }
                    }
                    foreach (var nodes in _adjArray)
                    {
                        if (nodes.Count == 0)
                        {
                            throw new JsonFormatException("Id is not continuous");
                        }
                        nodes.TrimExcess();
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new JsonFormatException("Wrong vertex id");
                }
            }

            #if false
            public AdjacencyTable(int[,] adjMatrix, (int, int)[] location)
            {
                if (adjMatrix.GetLength(0) != adjMatrix.GetLength(1))
                {
                    throw new ArgumentException("adjacency matrix is not a square matrix");
                }
                if (adjMatrix.GetLength(0) != location.Length)
                {
                    throw new
                        ArgumentException("the size of adjacency matrix and the length of location array is not equal");
                }
                int id = 0;
                Size = adjMatrix.GetLength(0);
                _adjArray = new Node*[Size];
                _locations = new (int, int)[Size];
                try
                {
                    for (int i = 0; i < Size; i++)
                    {
                        _locations[id] = (location[i].Item1, location[i].Item2);
                        Node* cur = _adjArray[i];
                        for (int j = 0; j < Size; j++)
                        {
                            if (adjMatrix[i, j] == 0) //i到j的边不存在
                            {
                                continue;
                            }
                            Node node = new()
                            {
                                adjVexDist = adjMatrix[i, j],
                                adjVexId = id++,
                                edgeType = EdgeType.Line,
                                controls = null
                            };
                            if (_adjArray[i] == null) //头结点
                            {
                                _adjArray[i] = &node;
                                continue;
                            }
                            cur->next = &node; //非头结点
                            cur = cur->next;
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new JsonFormatException("Wrong vertex id");
                }
            }

            #endif

            public List<(int, int)> this[int id]
            {
                get => _adjArray[id].Select(node => (id, node.pointId)).ToList();
            }

            public Edge? this[int fromId, int toId]
            {
                get
                {
                    Node ans = default;
                    bool find = false;
                    foreach (var node in _adjArray[fromId])
                    {
                        if (node.pointId == toId)
                        {
                            find = true;
                            ans = node;
                            break;
                        }
                    }
                    if (find)
                    {
                        return ans.edge;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public Vertex GetVertex(int id) => new(id, _locations[id].X, _locations[id].Y);

            public JArray SaveInstance()
            {
                JArray root = new();
                for (int i = 0; i < Size; i++)
                {
                    JObject obj = new() { { "Id", i }, { "X", _locations[i].X }, { "Y", _locations[i].Y } };
                    JArray nexts = new();
                    foreach(var node in _adjArray[i])
                    {
                        JObject next = new()
                        {
                            { "Id", node.pointId },
                            { "Distance", node.edge.Weight },
                            { "EdgeType", node.edge.Type == EdgeType.Line ? "Line" : "QuadraticBezierCurve" }
                        };
                        JArray controls = new();
                        if (node.edge.Type == EdgeType.QuadraticBezierCurve)
                        {
                            JObject control1 = JObject.FromObject(node.edge.Controls!.Value.Item1);
                            JObject control2 = JObject.FromObject(node.edge.Controls!.Value.Item2);
                            controls.Add(control1);
                            controls.Add(control2);
                        }
                        next.Add(controls);
                        nexts.Add(next);
                    }
                    obj.Add("Nexts", nexts);
                    root.Add(obj);
                }
                return root;
            }
        }

        public enum EdgeType
        {
            Line,
            QuadraticBezierCurve,
        }

        #endregion

        #region API on pathfinding

        //重载
        public static List<int> GetClosestPath(Building startBuilding, Building endBuilding)
        {
            return GetClosestPath(startBuilding.Center.Id, endBuilding.Center.Id);
        }

        public static List<int> GetClosestCircuit(List<Building> buildings)
        {
            List<int> points = buildings.ConvertAll(building => building.Center.Id);
            if (points.Count <= 2 || points.Count >= 10) //建筑太多或太少，报错
            {
                throw new ArgumentException("too many or too few items in parameter \"buildings\"");
            }
            (int[,] submap, int[] correspondence) = CreateSubMap(points);
            int row = submap.GetLength(0), column = 1 << (row - 1);
            int[,] dp = new int[20, 1 << 20];
            List<int> res = new() { points[0] };

            for (int i = 0; i < row; i++)
            {
                dp[i, 0] = submap[i, 0];
            }

            for (int j = 1; j < column; j++)
            {
                for (int i = 0; i < row; i++)
                {
                    dp[i, j] = int.MaxValue;
                    int bit = j >> (i - 1);
                    if ((bit & 1) == 1)
                    {
                        continue;
                    }
                    for (int k = 1; k < row; k++)
                    {
                        int bit2 = j >> (k - 1);
                        int bit3 = j ^ (1 << (k - 1));
                        if ((bit2 & 1) == 1 && dp[i, j] > submap[i, k] + dp[k, bit3])
                        {
                            dp[i, j] = submap[i, k] + dp[k, bit3];
                        }
                    }
                }
            }
            bool[] hasVisited = new bool[GlobalMap!.Size];
            int p = 0, e = column - 1, t = 0, cyc = 0;
            while (cyc++ < 10)
            {
                int min = int.MaxValue;
                for (int i = 0; i < row; i++)
                {
                    int bit = 1 << (i - 1);
                    if (!hasVisited[i] && (e & bit) == 1 &&
                        min > (GlobalMap[correspondence[i], p]?.Weight ?? int.MaxValue) + dp[i, e ^ bit])
                    {
                        min = GlobalMap[correspondence[i], p]!.Value.Weight + dp[i, e ^ bit];
                        t = correspondence[i];
                    }
                }
                if (p == t)
                {
                    break;
                }
                p = t;
                res.Add(p);
                hasVisited[p] = true;
                e ^= (1 << (p - 1));
            }
            res.Add(points[0]); //在建筑的关键点的最后添加一个出发点
            //res只是关键的点的路径，并不包含所有点，下面这个finalCircuit才包含所有点
            List<int> finalCircuit = new();
            for (int i = 1; i < res.Count; i++)
            {
                List<int> sector = GetClosestPath(res[i - 1], res[i]);
                foreach (var vertex in sector)
                {
                    finalCircuit.Add(vertex);
                }
            }
            return finalCircuit;
        }

        //重载，寻找两点最短路径的路径长
        public static int GetClosestPathLength(Building startBuilding, Building endBuilding)
        {
            return GetClosestPathLength(startBuilding.Center.Id, endBuilding.Center.Id);
        }

        #endregion

        #region API on searching

        public static List<Building> GetBuildingsByName(string name)
        {
            List<Building> ret = new();
            if (name == "default building")
            {
                return new List<Building>() { Constants.DefaultBuilding };
            }
            foreach (var building in Buildings)
            {
                if (building.Name.Contains(name))
                {
                    ret.Add(building);
                }
            }
            //return ret;
            return new() { new(-1, "random building", new()) };
        }

        public static List<(Vertex, Vertex)> GetLineEndPoints()
        {
            HashSet<(Vertex, Vertex)> ret = new();
            if (GlobalMap == null)
            {
                return ret.ToList();
            }
            for (int i = 0; i < GlobalMap!.Size; i++)
            {
                foreach (var pair in GlobalMap[i])
                {
                    var smallerId = pair.Item1 <= pair.Item2 ? pair.Item1 : pair.Item2;
                    var biggerId = smallerId == pair.Item1 ? pair.Item2 : pair.Item1;
                    if (GlobalMap[smallerId, biggerId]!.Value.Type == EdgeType.Line)
                    {
                        ret.Add((GlobalMap.GetVertex(smallerId), GlobalMap.GetVertex(biggerId)));
                    }
                }
            }
            return ret.ToList();
        }

        public static List<(Vertex, Point, Point, Vertex)> GetBezierCurveControlPoint()
        {
            HashSet<(Vertex, Vertex)> set = new();
            List<(Vertex, Point, Point, Vertex)> ret = new();
            if (GlobalMap == null)
            {
                return ret.ToList();
            }
            for (int i = 0; i < GlobalMap!.Size; i++)
            {
                foreach (var pair in GlobalMap[i])
                {
                    var smallerId = pair.Item1 <= pair.Item2 ? pair.Item1 : pair.Item2;
                    var biggerId = smallerId == pair.Item1 ? pair.Item2 : pair.Item1;
                    if (GlobalMap[smallerId, biggerId]!.Value.Type == EdgeType.Line)
                    {
                        set.Add((GlobalMap.GetVertex(smallerId), GlobalMap.GetVertex(biggerId)));
                    }
                }
            }
            foreach (var points in set)
            {
                Edge e = GlobalMap[points.Item1.Id, points.Item2.Id]!.Value;
                (Point, Point) controls = e.Controls!.Value;
                ret.Add((points.Item1, controls.Item1, controls.Item2, points.Item2));
            }
            return ret;
        }

        #endregion

        #region other methods

        private static (int[,], int[]) CreateSubMap(List<int> criticalPoints) //生成一个子图，其中子图的节点是所有需要经过的点 + 出发点。
        {
            int pointCount = criticalPoints.Count;
            int[,] subEdges = new int[pointCount, pointCount];
            int[] correspondence = criticalPoints.ToArray();
            for (int i = 0; i < pointCount; i++)
            {
                for (int j = 0; j < pointCount; j++)
                {
                    subEdges[i, j] = i == j ? int.MaxValue : GetClosestPathLength(criticalPoints[i], criticalPoints[j]);
                }
            }
            return (subEdges, correspondence);
        }

        private static List<int> GetClosestPath(int startId, int endId) //传参是出发建筑和终点建筑的中心点的id
        {
            //遍历的每一个点i都会有一个route[i],表示到达该点所进过的路线。
            int pointCount = GlobalMap!.Size; //点的数量
            List<int>[] route = new List<int>[pointCount];
            int[] distanceFromStart = new int[GlobalMap.Size]; //每个点到初始点的距离
            Array.Fill(distanceFromStart, int.MaxValue);
            distanceFromStart[startId] = 0;
            route[startId].Add(startId);
            int curId = startId;

            for (int i = 0; i < pointCount; i++) //循环len-1次
            {
                var nexts = GlobalMap[i]; //nexts为点[z]
                int cyc = 0; //遍历的次数，每次循环后++
                foreach ((int id, int dist) in nexts)
                {
                    if (distanceFromStart[i] > distanceFromStart[id] + dist) //如果出发点到点[z]的距离 大于 出发点到某点的距离+某点到点[z]的距离
                    {
                        distanceFromStart[i] = distanceFromStart[curId] + dist; //替换从出发点到点[z]的最短距离

                        route[cyc] = route[curId];
                        route[cyc].Add(cyc++); //记录此时的路线在list中,同时temp++
                    }
                }

                //遍历所有点，寻找下一个距离最近的点
                int minDistance = int.MaxValue;
                int tempId = -1;
                for (int j = 0; j < pointCount; j++)
                {
                    if (j != curId && distanceFromStart[j] >= distanceFromStart[curId] &&
                        distanceFromStart[j] < minDistance)
                    {
                        minDistance = distanceFromStart[j];
                        tempId = j;
                    }
                }
                curId = tempId;
            }
            for (int i = 0; i < route[endId].Count; i++)
            {
                Console.WriteLine(route[endId][i]);
            }
            return route[endId];
        }

        private static int GetClosestPathLength(int startId, int endId)
        {
            int pointCount = GlobalMap!.Size; //点的数量
            int[] distance = new int[GlobalMap.Size]; //每个点到初始点的距离
            Array.Fill(distance, int.MaxValue);
            distance[startId] = 0;
            int curId = startId;

            for (int i = 1; i < pointCount; i++)
            {
                var nexts = GlobalMap[i]; //nexts为点[z]
                foreach ((int id, int dist) in nexts)
                {
                    if (distance[i] > distance[id] + dist) //如果出发点到点[z]的距离 大于 出发点到某点的距离+某点到点[z]的距离
                    {
                        distance[i] = distance[curId] + dist; //替换从出发点到点[z]的最短距离
                    }
                }

                //遍历所有点，寻找下一个距离最近的点
                int minDistance = int.MaxValue;
                int tempId = -1;
                for (int j = 0; j < pointCount; j++)
                {
                    if (j != curId && distance[j] >= distance[curId] && distance[j] < minDistance)
                    {
                        minDistance = distance[j];
                        tempId = j;
                    }
                }
                curId = tempId;
            }
            return distance[endId];
        }

        public static JArray SaveAllBuildings() =>
            new(Array.ConvertAll(Buildings.ToArray(),
                                 building => (Id: building.Id, Name: building.Name, CenterId: building.Center.Id)));

        #endregion
    }

    public static class Navigate
    {
        public static void Show(List<int> points)
        {
            List<(Location.Vertex, Location.Vertex)> lineEndPointPairs = new();
            List<(Location.Vertex, Point, Point, Location.Vertex)> bezCurveControlPointTuples = new();
            if (points.Count <= 1)
            {
                throw new ArgumentException("too few points");
            }
            int prevId = points[0];
            for (int i = 1; i < points.Count - 1; i++)
            {
                Location.Edge? edge = Location.GlobalMap![prevId, i];
                if (!edge.HasValue)
                {
                    throw new ArgumentException($"there's no edge between vertex {prevId} and {i}");
                }
                if (edge.Value.Type == Location.EdgeType.Line)
                {
                    lineEndPointPairs.Add((Location.GlobalMap.GetVertex(prevId), Location.GlobalMap.GetVertex(i)));
                }
                else
                {
                    bezCurveControlPointTuples.Add((Location.GlobalMap.GetVertex(prevId),
                                                    edge.Value.Controls!.Value.Item1, edge.Value.Controls!.Value.Item2,
                                                    Location.GlobalMap.GetVertex(i)));
                }
            }
            UI.MapWindow mapWindow = new(lineEndPointPairs, bezCurveControlPointTuples);
            mapWindow.ShowDialog();
        }
    }
}