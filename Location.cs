using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace StudentScheduleManagementSystem.Map
{
    public static class Location
    {
        public static List<Building>? Buildings { get; private set; } = new();
        public static AdjacencyTable? GlobalMap { get; private set; }

        #region structs, classes and enums

        public struct Edge //边
        {
            public EdgeType Type { get; init; }
            public (Point, Point)? Controls { get; init; }
            public int Weight { get; init; }

            public Edge(EdgeType type, (Point, Point)? controls, int weight)
            {
                Type = type;
                Controls = controls;
                Weight = weight;
            }
        }

        public struct Vertex //点
        {
            public int Id { get; init; }
            public int X { get; init; }
            public int Y { get; init; }

            public Vertex(int id, int x, int y)
            {
                Id = id;
                X = x;
                Y = y;
            }
        }

        public struct Building //建筑
        {
            public int Id { get; set; } = -1;
            public Vertex center { get; set; } //4月8日新增，表示建筑中心的点
            public int DoorNumber { get; set; } = 0;//门的数量
            public string Name { get; set; } = String.Empty;
            public Vertex[] Doors { get; set; } = Array.Empty<Vertex>();//门所在的点，用来寻路

            public Building(int id, string name, Vertex[] doors)
            {
                Id = id;
                Name = name;
                Doors = doors;
                DoorNumber = doors.Length;
                if (DoorNumber > 4)
                {
                    throw new ArgumentOutOfRangeException(nameof(DoorNumber));
                }
                Doors = doors;
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

            public override int GetHashCode()
            {
                unchecked
                {
                    int result = 37;
                    result *= 397;
                    result += Id.GetHashCode();
                    result *= 397;
                    result += DoorNumber.GetHashCode();
                    result *= 397;
                    result += Name.GetHashCode();
                    result *= 397;
                    result += Doors.GetHashCode();
                    return result;
                }
            }

            public override bool Equals(object? obj)
            {
                if (obj == null)
                {
                    return false;
                }
                return Id == ((Building)obj).Id && Name == ((Building)obj).Name &&
                       DoorNumber == ((Building)obj).DoorNumber && Doors.SequenceEqual(((Building)obj).Doors);
            }
        }

        public unsafe class AdjacencyTable
        {
            private struct Node
            {
                public int adjVexId;
                public int adjVexDist;
                public EdgeType edgeType;
                public (Point, Point)? controls;
                public Node* next;
            }

            public int Size { get; init; }
            private readonly Node*[] _adjArray;
            private readonly (int, int)[] _locations;

            public AdjacencyTable(JArray vertexArray)
            {
                Size = vertexArray.Count;
                _adjArray = new Node*[Size];
                _locations = new (int, int)[Size];
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
                        Node* cur = _adjArray[id];
                        _locations[id] = (x, y);
                        foreach (JObject next in nexts)
                        {
                            Node node = new()
                            {
                                adjVexId = next["Id"]!.Value<int>(),
                                adjVexDist = next["Distance"]!.Value<int>(),
                                edgeType = next["EdgeType"]!.Value<string>() switch
                                {
                                    "Line" => EdgeType.Line, "QuadraticBezierCurve" => EdgeType.QuadraticBezierCurve,
                                    _ => throw new JsonFormatException("Wrong EdgeType")
                                },
                                controls = null,
                                next = null
                            };
                            if (node.adjVexId >= Size || node.adjVexId < 0)
                            {
                                throw new JsonFormatException("Wrong vertex id");
                            }
                            if (node.edgeType == EdgeType.QuadraticBezierCurve)
                            {
                                JArray controls = (JArray)next["Controls"]!;
                                if (controls.Count != 2)
                                {
                                    throw new JsonFormatException("Too many control points");
                                }
                                var control1 = JsonConvert.DeserializeObject<Point>(controls.ElementAt(1).ToString());
                                var control2 = JsonConvert.DeserializeObject<Point>(controls.ElementAt(2).ToString());
                                node.controls = (control1, control2);
                            }
                            if (_adjArray[id] == null) //头结点
                            {
                                _adjArray[id] = &node;
                                continue;
                            }
                            cur->next = &node; //非头结点
                            cur = cur->next;
                        }
                    }
                    foreach (var pointer in _adjArray)
                    {
                        if (pointer == null)
                        {
                            throw new JsonFormatException("Id is not continuous");
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new JsonFormatException("Wrong vertex id");
                }
            }

            #if DEBUG

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
                get
                {
                    List<(int, int)> list = new();
                    if (id >= Size || id < 0)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    Node* cur = _adjArray[id];
                    while (cur != null)
                    {
                        list.Add((cur->adjVexId, cur->adjVexDist));
                        cur = cur->next;
                    }
                    return list;
                }
            }

            public Edge? this[int fromId, int toId]
            {
                get
                {
                    if (fromId >= Size || toId >= Size || (fromId, toId) is not (>= 0, >= 0))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    Node* cur = _adjArray[fromId];
                    bool find = false;
                    while (cur != null)
                    {
                        if (cur->adjVexId == toId)
                        {
                            find = true;
                            break;
                        }
                        cur = cur->next;
                    }
                    return find ? new Edge(cur->edgeType, cur->controls, cur->adjVexDist) : null;
                }
            }

            public Vertex GetVertex(int id) => new(id, _locations[id].Item1, _locations[id].Item2);

            public JArray SaveInstance()
            {
                JArray root = new();
                for (int i = 0; i < Size; i++)
                {
                    JObject obj = new() { { "Id", i }, { "X", _locations[i].Item1 }, { "Y", _locations[i].Item2 } };
                    JArray nexts = new();
                    Node* cur = _adjArray[i];
                    while (cur != null)
                    {
                        JObject next = new()
                        {
                            { "Id", cur->adjVexId },
                            { "Distance", cur->adjVexDist },
                            { "EdgeType", cur->edgeType == EdgeType.Line ? "Line" : "QuadraticBezierCurve" }
                        };
                        JArray controls = new();
                        if (cur->edgeType == EdgeType.QuadraticBezierCurve)
                        {
                            JObject control1 = JObject.FromObject(cur->controls!.Value.Item1);
                            JObject control2 = JObject.FromObject(cur->controls!.Value.Item2);
                            controls.Add(control1);
                            controls.Add(control2);
                        }
                        next.Add(controls);
                        nexts.Add(next);
                        cur = cur->next;
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
            //4月8日晚新增：只需包含两点，且就算是回路也不用在尾部加出发点，因为getcircuit函数的传参List默认第一个是出发点
            List<int> path = GetCenters(new() { startBuilding, endBuilding });
            return GetClosestPath(path[0], path[1]);
        }

        public static List<int> GetClosestPath(int startId, int endId)//传参是出发建筑和终点建筑的中心点的id
        {
            //这里需要提一下：遍历的每一个点i都会有一个route[i],表示到达该点所进过的路线。
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


        public static List<int> GetClosestCircuit(List<Building> buildings)
        {
            List<int> points = GetCenters(buildings);
            if (points.Count<2 || points.Count > 20)//建筑太多或太少，报错
            {
                throw new TooManyTemporaryAffairsException();
            }
            (int[,] submap, int[] correspondence) = CreateSubMap(points);
            int row = (int)Math.Sqrt(submap.Length), column = 1 << (row - 1);
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
                    if (!hasVisited[i] && (e & bit) == 1)
                    {
                        if (min > (GlobalMap[correspondence[i], p]?.Weight ?? int.MaxValue) + dp[i, e ^ bit])
                        {
                            min = GlobalMap[correspondence[i], p]!.Value.Weight + dp[i, e ^ bit];
                            t = correspondence[i];
                        }
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
            res.Add(points[0]);//在建筑的关键点的最后添加一个出发点
            //res只是关键的点的路径，并不包含所有点，下面这个finalCircuit才包含所有点
            List<int> finalCircuit = new List<int>();
            for (int i = 1; i < res.Count; i++)
            {
                List<int> sectorOfCircuit = GetClosestPath(res[i - 1], res[i]);
                for (int j = 0; j < sectorOfCircuit.Count; j++)
                {
                    finalCircuit.Add(sectorOfCircuit[j]);
                }
            }
            return finalCircuit;
        }

        //重载，寻找两点最短路径的路径长
        public static int GetClosestPathLength(Building startBuilding, Building endBuilding)
        {
            //4月8日晚新增：只需包含两点，且就算是回路也不用在尾部加出发点，因为getcircuit函数的传参List默认第一个是出发点
            List<int> path = GetCenters(new() { startBuilding, endBuilding});

            return GetClosestPathLength(path[0], path[1]);
        }

        public static int GetClosestPathLength(int startId, int endId)
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

        #endregion

        #region API on searching

        public static List<Building> GetBuildingsByName(string name)
        {
            //UNDONE
            return new List<Building>() { new(0, "default building", Array.Empty<Vertex>()) };
        }

        #endregion

        #region other methods

        //从多个建筑找回路。将建筑转换为点.
        private static List<int> GetCenters(List<Building> buildings)
        {
            /*4月8日晚新增：该函数对两点路径和回路均适用。我之前理解有误。实际上，对于回路的传参buildings，
             *             并不需要在buildings的尾部再添加出发点。比如说从有0,1,2，三个点。buildings的序
             *             列就只需要包含0,1,2三个元素就行，出发点必须是第一个元素，其他途径点可以是乱序的，
            */
            int vertexCount = buildings.Count;//要经过的点的数量
            if(vertexCount <= 2 || vertexCount > 20)
            {
                throw new ArgumentException("too many or too few buildings");
            }
            List<int> points = new List<int>();//要返回的所有点
            for(int i = 0;i < vertexCount; i++)
            {
                points.Add(buildings[i].center.Id);//改为直接添加该建筑中心点vertex的Id，寻路改成按照中心点来找
            }
            return points;
        }

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

        public static void SetUp()
        {
            var information = FileManagement.FileManager.ReadFromMapFile(FileManagement.FileManager.MapFileDirectory);
            GlobalMap = new(information.Item1);
            foreach (JObject obj in information.Item2)
            {
                int id = obj["Id"]!.Value<int>();
                string name = obj["Name"]!.Value<string>()!;
                JArray doorsArray = (JArray)obj["Doors"]!;
                if (doorsArray.Count > 4)
                {
                    throw new JsonFormatException("too many doors in one building");
                }
                Building building = new(id,
                                        name,
                                        Array.ConvertAll(doorsArray.ToArray(),
                                                         token => GlobalMap.GetVertex(token["Id"]!.Value<int>())));
                Buildings!.Add(building);
            }
        }

        public static JArray SaveBuildings() => new (Buildings!.ToArray());

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
                    bezCurveControlPointTuples.Add((Location.GlobalMap.GetVertex(prevId), edge.Value.Controls!.Value.Item1, edge.Value.Controls!.Value.Item2, Location.GlobalMap.GetVertex(i)));
                }
            }
            UI.MapWindow mapWindow = new(lineEndPointPairs, bezCurveControlPointTuples);
            mapWindow.ShowDialog();
        }
    }
}