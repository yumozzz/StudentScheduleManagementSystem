using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace StudentScheduleManagementSystem.Map
{
    public static class Location
    {
        private static List<Building> _buildings = new() { Constants.DefaultBuilding };

        public static List<Building> Buildings
        {
            get
            {
                var buildings = _buildings.GetRange(1, _buildings.Count - 1).ToArray();
                MergeSort.Sort(ref buildings, (building1, building2) => building1.Name.CompareTo(building2.Name));
                return buildings.ToList();
            }
            set => _buildings = new List<Building> { Constants.DefaultBuilding }.Concat(value).ToList();
        }
        public static AdjacencyTable? GlobalMap { get; set; }

        static Location()
        {
            try
            {
                (JArray, JArray) information;
                try
                {
                    information =
                        FileManagement.FileManager.ReadFromMapFile(FileManagement.FileManager.MapFileDirectory);
                }
                catch (Exception ex) when (ex is DirectoryNotFoundException or FileNotFoundException)
                {
                    return;
                }
                GlobalMap = new(information.Item1);
                List<Building> buildings = new();
                foreach (JObject obj in information.Item2)
                {
                    int id = obj["Id"]!.Value<int>();
                    string name = obj["Name"]!.Value<string>()!;
                    Building building = new(id, name, GlobalMap.GetVertex(obj["CenterId"]!.Value<int>()));
                    buildings.Add(building);
                }
                Buildings = buildings;
            }
            catch (Exception ex) when (ex is JsonFormatException or InvalidCastException)
            {
                MessageBox.Show("地图文件读取出错，已退出", "错误");
                Log.Error.Log("地图文件读取出错，已退出", ex);
                throw;
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
            public struct Node
            {
                public int pointId;
                public Edge edge;
            }

            public int Size { get; init; }
            public List<Node>[] _adjArray;
            public Point[] Location { get; private set; }

            public AdjacencyTable(List<Vertex> vertices, List<(Vertex, Vertex)> edges)
            {
                Size = vertices.Count;
                _adjArray = new List<Node>[Size];
                for (int i = 0; i < Size; i++)
                {
                    _adjArray[i] = new();
                }
                Location = vertices.ConvertAll(vertex => new Point(vertex.X, vertex.Y)).ToArray();

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
                Location = new Point[Size];

                try
                {
                    foreach (JObject vertex in vertexArray)
                    {
                        int id = vertex["Id"]!.Value<int>();
                        int x = vertex["X"]!.Value<int>();
                        int y = vertex["Y"]!.Value<int>();
                        JArray nexts = (JArray)vertex["Nexts"]!;
                        Location[id] = new(x, y);
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

            public List<(int, int)> this[int id] => _adjArray[id].Select(node => (id, node.pointId)).ToList();

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

            public Vertex GetVertex(int id) => new(id, Location[id].X, Location[id].Y);

            public JArray SaveInstance()
            {
                JArray root = new();
                for (int i = 0; i < Size; i++)
                {
                    JObject obj = new() { { "Id", i }, { "X", Location[i].X }, { "Y", Location[i].Y } };
                    JArray nexts = new();
                    foreach (var node in _adjArray[i])
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
                        next.Add("Controls", controls);
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


        public static List<int> Solve(int[,] adjacencyMatrix, int startCity)
        {
            int count = adjacencyMatrix.GetLength(0);

            List<int> cities = new();
            for (int i = 0; i < count; i++)
            {
                if (i != startCity)
                {
                    cities.Add(i);
                }
            }

            List<int> shortestPath = new();
            int shortestDistance = int.MaxValue;
            Permute(adjacencyMatrix, cities, startCity, 0, cities.Count - 1, ref shortestPath, ref shortestDistance);
            shortestPath.Insert(0, startCity);
            shortestPath.Add(startCity);

            return shortestPath;
        }

        public static void Permute(int[,] adjacencyMatrix,
                                   List<int> cities,
                                   int startCity,
                                   int left,
                                   int right,
                                   ref List<int> shortestPath,
                                   ref int shortestDistance)
        {
            if (left == right)
            {
                int currentDistance = CalculatePathDistance(adjacencyMatrix, cities, startCity);
                if (currentDistance < shortestDistance)
                {
                    shortestDistance = currentDistance;
                    shortestPath = new List<int>(cities);
                }
            }
            else
            {
                for (int i = left; i <= right; i++)
                {
                    Swap(cities, left, i);
                    Permute(adjacencyMatrix,
                            cities,
                            startCity,
                            left + 1,
                            right,
                            ref shortestPath,
                            ref shortestDistance);
                    Swap(cities, left, i);
                }
            }
        }

        public static int CalculatePathDistance(int[,] adjacencyMatrix, List<int> path, int startCity)
        {
            int distance = 0;
            int previousCity = startCity;

            foreach (int city in path)
            {
                distance += adjacencyMatrix[previousCity, city];
                previousCity = city;
            }

            distance += adjacencyMatrix[previousCity, startCity];

            return distance;
        }

        public static List<int> GetClosestCircuit(List<Building> buildings)
        {
            List<int> points = new(buildings.ConvertAll(building => building.Center.Id));

            if (points.Count <= 2 || points.Count >= 10) //建筑太多或太少，报错
            {
                throw new ArgumentException("too many or too few items in parameter \"buildings\"");
            }
            var(submap, correspondentId) = CreateSubMap(points);

            int startCity = 0;
            List<int> shortestPath = Solve(submap, startCity);
            List<int> transferred = new(shortestPath.ConvertAll(id => correspondentId[id]));
            List<int> result = new();
            for (int i = 1; i < transferred.Count; i++)
            {
                List<int> temp = GetClosestPath(transferred[i - 1], transferred[i]);
                for (int j = 0; j < temp.Count - 1; j++)
                {
                    result.Add(temp[j]);
                }
                if (i == transferred.Count - 1)
                {
                    result.Add(temp[^1]);
                }
            }
            return result;
        }

        //寻找两点最短路径的路径长
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
            return ret;
        }

        public static List<(Point, Point)> GetEdges()
        {
            HashSet<(Point, Point)> ret = new();
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
                        ret.Add((GlobalMap.GetVertex(smallerId).ToPoint(), GlobalMap.GetVertex(biggerId).ToPoint()));
                    }
                }
            }
            return ret.ToList();
        }

        public static List<Point> GetPoints() => GlobalMap?.Location.ToList() ?? new();

        public static List<Vertex> GetVertices()
        {
            int i = 0;
            return GlobalMap?.Location.ToList().ConvertAll(point => point.ToVertex(i++)) ?? new();
        }

        #if false
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

        #endif

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
            bool[] visited = new bool[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                route[i] = new();
                visited[i] = false;
            }
            int[] distanceFromStart = new int[GlobalMap.Size]; //每个点到初始点的距离
            Array.Fill(distanceFromStart, int.MaxValue);
            distanceFromStart[startId] = 0;
            route[startId].Add(startId);
            int curId = startId;

            for (int i = 0; i < pointCount; i++) //循环len-1次
            {
                for (int j = 0; j < GlobalMap._adjArray[curId].Count; j++)
                {
                    int id = GlobalMap._adjArray[curId][j].pointId;
                    int dist = GlobalMap._adjArray[curId][j].edge.Weight;
                    if (distanceFromStart[id] > distanceFromStart[curId] + dist) //如果出发点到点[z]的距离 大于 出发点到某点的距离+某点到点[z]的距离
                    {
                        distanceFromStart[id] = distanceFromStart[curId] + dist; //替换从出发点到点[z]的最短距离
                        for (int k = 0; k < route[curId].Count; k++)
                        {
                            route[id].Add(route[curId][k]);
                        }
                        route[id].Add(id); //记录此时的路线在list中,同时temp++
                    }
                }

                //遍历所有点，寻找下一个距离最近的点
                int minDistance = int.MaxValue;
                int tempId = startId;
                for (int j = 0; j < pointCount; j++)
                {
                    if (j != curId && distanceFromStart[j] >= distanceFromStart[curId] &&
                        distanceFromStart[j] < minDistance && visited[j] == false)
                    {
                        minDistance = distanceFromStart[j];
                        tempId = j;
                    }
                }
                visited[curId] = true;
                curId = tempId;
            }
            //  for(int i = 0; i < route[endId].Count; i++)
            //      Console.WriteLine(route[endId][i]);

            return route[endId];
        }

        private static int GetClosestPathLength(int startId, int endId)
        {
            int pointCount = GlobalMap!.Size; //点的数量
            int[] distance = new int[GlobalMap.Size]; //每个点到初始点的距离
            Array.Fill(distance, int.MaxValue);
            bool[] visited = new bool[pointCount];
            Array.Fill(visited, false);
            distance[startId] = 0;
            int curId = startId;

            for (int i = 1; i < pointCount; i++)
            {
                for (int j = 0; j < GlobalMap._adjArray[curId].Count; j++)
                {
                    int id = GlobalMap._adjArray[curId][j].pointId;
                    int dist = GlobalMap._adjArray[curId][j].edge.Weight;
                    if (distance[id] > distance[curId] + dist) //如果出发点到点[z]的距离 大于 出发点到某点的距离+某点到点[z]的距离
                    {
                        distance[id] = distance[curId] + dist; //替换从出发点到点[z]的最短距离
                    }
                }

                //遍历所有点，寻找下一个距离最近的点
                int minDistance = int.MaxValue;
                int tempId = startId;
                for (int j = 0; j < pointCount; j++)
                {
                    if (j != curId && distance[j] >= distance[curId] && distance[j] < minDistance &&
                        visited[j] == false)
                    {
                        minDistance = distance[j];
                        tempId = j;
                    }
                }
                visited[curId] = true;
                curId = tempId;
            }
            return distance[endId];
        }

        public static JArray SaveAllBuildings()
        {
            JArray ret = new();
            foreach (var building in Buildings)
            {
                JObject obj = new()
                {
                    { "Id", building.Id }, { "Name", building.Name }, { "CenterId", building.Center.Id }
                };
                ret.Add(obj);
            }
            return ret;
        }

        private static void Swap(List<int> list, int i, int j)
        {
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

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
            for (int i = 1; i < points.Count; i++)
            {
                Location.Edge? edge = Location.GlobalMap![prevId, points[i]];
                if (!edge.HasValue)
                {
                    throw new ArgumentException($"there's no edge between vertex {prevId} and {points[i]}");
                }
                if (edge.Value.Type == Location.EdgeType.Line)
                {
                    lineEndPointPairs.Add((Location.GlobalMap.GetVertex(prevId),
                                           Location.GlobalMap.GetVertex(points[i])));
                }
                else
                {
                    bezCurveControlPointTuples.Add((Location.GlobalMap.GetVertex(prevId),
                                                    edge.Value.Controls!.Value.Item1, edge.Value.Controls!.Value.Item2,
                                                    Location.GlobalMap.GetVertex(points[i])));
                }
                prevId = points[i];
            }
            UI.MapWindow mapWindow = new(lineEndPointPairs, bezCurveControlPointTuples);
            Thread thread = new(() => mapWindow.ShowDialog());
            thread.Start();
            thread.Join();
        }
    }
}