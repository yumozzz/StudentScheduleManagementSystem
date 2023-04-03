using Newtonsoft.Json.Linq;

namespace StudentScheduleManagementSystem.Map
{
    public static class Location
    {
        private static Dictionary<int, Vertex> _verteces = new();
        private const int MaxDoorCount = 4;
        private const int MaxAffairCount = 6;
        private static AdjacencyTable _globalMap, _subMap;

        #region structs, classes and enums

        public struct Edge //边
        {
            public EdgeType Type { get; init; }
            public ((int, int), (int, int))? Controls { get; init; }
            public int Weight { get; init; }

            public Edge(EdgeType type, ((int, int), (int, int))? controls, int weight)
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
            public int Id { get; set; }
            public int DoorNumber { get; set; } //门的数量
            public string Name { get; set; }
            public Vertex[] Doors { get; set; } = new Vertex[MaxDoorCount]; //门所在的点，用来寻路

            public Building(int id, string name, Vertex[] doors)
            {
                Id = id;
                Name = name;
                Doors = doors;
                DoorNumber = doors.Length;
                if (DoorNumber > MaxDoorCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(DoorNumber));
                }
            }
        }

        private unsafe class AdjacencyTable
        {
            private struct Node
            {
                public int adjVexId;
                public int adjVexDist;
                public EdgeType edgeType;
                public ((int, int), (int, int))? controls;
                public Node* next;
            }

            public int Size { get; init; }
            private Node*[] _array;

            public AdjacencyTable(JArray vertexArray)
            {
                Size = vertexArray.Count;
                _array = new Node*[Size];
                foreach (JObject vertex in vertexArray)
                {
                    int id = vertex["Id"]!.Value<int>();
                    int x = vertex["X"]!.Value<int>();
                    int y = vertex["Y"]!.Value<int>();
                    _verteces.Add(id, new(id, x, y));
                    JArray nexts = (JArray)vertex["Nexts"]!;
                    if (nexts.Count == 0)
                    {
                        continue;
                    }
                    Node* cur = _array[id];
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
                            var control1 = (x: controls.ElementAt(1)["X"]!.Value<int>(),
                                            y: controls.ElementAt(1)["Y"]!.Value<int>());
                            var control2 = (x: controls.ElementAt(2)["X"]!.Value<int>(),
                                            y: controls.ElementAt(2)["Y"]!.Value<int>());
                            node.controls = (control1, control2);
                        }
                        if (_array[id] == null) //头结点
                        {
                            _array[id] = &node;
                            continue;
                        }
                        cur->next = &node; //非头结点
                        cur = cur->next;
                    }
                }
                foreach (var pointer in _array)
                {
                    if (pointer == null)
                    {
                        throw new JsonFormatException("Id is not continuous");
                    }
                }
            }

            #if DEBUG

            public AdjacencyTable(int[,] adjMatrix)
            {
                Size = (int)Math.Sqrt(adjMatrix.Length);
            }

            #endif
            public List<(int, int)> this[int id]
            {
                get
                {
                    List<(int, int)> list = new();
                    if (id >= Size || id < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(id));
                    }
                    Node* cur = _array[id];
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
                        throw new ArgumentOutOfRangeException(nameof(fromId) + " or " + nameof(toId));
                    }
                    Node* cur = _array[fromId];
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
        }

        public enum EdgeType
        {
            Line,
            QuadraticBezierCurve,
        }

        #endregion

        #region API on pathfinding

        public static List<int> GetClosestPath(int startId, int endId)
        {
            //这里需要提一下：遍历的每一个点i都会有一个route[i],表示到达该点所进过的路线。
            int pointCount = _globalMap.Size; //点的数量
            List<int>[] route = new List<int>[pointCount];
            int[] distanceFromStart = new int[_globalMap.Size]; //每个点到初始点的距离
            Array.Fill(distanceFromStart, int.MaxValue);
            distanceFromStart[startId] = 0;
            route[startId].Add(startId);
            int curId = startId;

            for (int i = 0; i < pointCount; i++) //循环len-1次
            {
                var nexts = _globalMap[i]; //nexts为点[z]
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

        //TODO:verify
        public static List<int> GetClosestCircuit(List<int> points)
        {
            (int[,] submap, int[] correspondence) = CreateSubMap(points);
            int row = (int)Math.Sqrt(submap.Length), column = 1 << (row - 1);
            int[,] dp = new int[MaxAffairCount, 1 << MaxAffairCount];
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
            bool[] hasVisited = new bool[_globalMap.Size];
            int p = 0, e = column - 1, t = 0, cyc = 0;
            while (cyc++ < 10)
            {
                int min = int.MaxValue;
                for (int i = 0; i < row; i++)
                {
                    int bit = 1 << (i - 1);
                    if (!hasVisited[i] && (e & bit) == 1)
                    {
                        if (min > (_globalMap[correspondence[i], p]?.Weight ?? int.MaxValue) + dp[i, e ^ bit])
                        {
                            min = _globalMap[correspondence[i], p]!.Value.Weight + dp[i, e ^ bit];
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
            res.Add(points[0]);
            return res;
        }

        public static int GetClosestPathLength(int startId, int endId)
        {
            int pointCount = _globalMap.Size; //点的数量
            int[] distance = new int[_globalMap.Size]; //每个点到初始点的距离
            Array.Fill(distance, int.MaxValue);
            distance[startId] = 0;
            int curId = startId;

            for (int i = 1; i < pointCount; i++)
            {
                var nexts = _globalMap[i]; //nexts为点[z]
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
            return new List<Building>();
        }

        #endregion

        #region API on save and create instances to/from JSON

        #endregion

        #region private methods

        private static (int[,], int[]) CreateSubMap(List<int> criticalPoints) //生成一个子图，其中子图的节点是所有需要进过的点 + 出发点。
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

        #endregion
    }

    public static class Navigate
    {
        public static void Show(List<int> points)
        {
            /*UI.MapWindow mapWindow = new();
            mapWindow.ShowDialog();*/
        }
    }
}