using Newtonsoft.Json.Linq;
using System.Linq;

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
            public ((int, int), (int, int))? Controls{ get; init; }
            public int Weight { get; init; }

            public Edge(EdgeType type,((int, int), (int, int))? controls,int weight)
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

            public int Size{ get; init; }
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
                        if (node.adjVexId >= Size || node.adjVexId <0)
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
                foreach(var pointer in _array)
                {
                    if (pointer == null)
                    {
                        throw new JsonFormatException("Id is not continuous");
                    }
                }
            }

            public List<(int,int)> this[int id]
            {
                get
                {
                    List<(int,int)> list = new();
                    if (id >= Size||id<0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(id));
                    }
                    Node* cur = _array[id];
                    while (cur->next != null)
                    {
                        list.Add((cur->adjVexId,cur->adjVexDist));
                        cur = cur->next;
                    }
                    return list;
                }
            }

            public Edge? this[int fromId, int toId]
            {
                get
                {
                    if (fromId >= Size || toId >= Size || (fromId, toId) is not ( >= 0, >= 0))
                    {
                        throw new ArgumentOutOfRangeException(nameof(fromId) + " or " + nameof(toId));
                    }
                    Node* cur = _array[fromId];
                    bool find = false;
                    while (cur->next != null)
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

        public static List<int> GetClosetPath(int startId, int endId) //startId=start.ID, endId=end.ID
        {
            List<int>[] route = new List<int>[100];
            int pointCount = _globalMap.Size; //点的数量
            int[] distance = new int[MAX]; //每个点到初始点的距离
            Array.Fill(distance, INF);
            distance[startId] = 0;
            route[startId].Add(startId); //route是第38行定义的向量
            int t = startId;

            for (int z = 1; z < pointCount; z++) //循环len-1次
            {
                //找点的相邻边,该边标记为1，更新边对应的下个点的dist
                for (int i = 0; i < pointCount; i++)
                {
                    if (i != t && _globalMap.Edges[t, i].Weight != INF &&
                        distance[i] > distance[t] + _globalMap.Edges[t, i].Weight)
                    {
                        distance[i] = distance[t] + _globalMap.Edges[t, i].Weight;
                        route[i] = route[t];
                        route[i].Add(i);
                    }
                }

                //遍历所有点，寻找下一个距离最近的点
                int mmin = MAX;
                int x = -1;
                for (int i = 0; i < pointCount; i++)
                {
                    if (i != t && distance[i] >= distance[t] && distance[i] < mmin)
                    {
                        mmin = distance[i];
                        x = i;
                    }
                }
                t = x;
            }
            for (int i = 0; i < route[endId].Count; i++)
            {
                Console.WriteLine(route[endId][i]);
            }
            return route[endId];
        }

        public static List<int> GetClosetPath(List<int> points) // 0 , {1,2,3,4}
        {
            CreateSubMap(points);
            int row = _subMap.Size, column = 1 << (row - 1);
            int[,] dp = new int[MaxAffairCount, 1 << MaxAffairCount];
            List<int> res = new() { points[0] };

            for (int i = 0; i < row; i++)
            {
                dp[i, 0] = _subMap.Edges[i, 0].Weight;
            }

            for (int j = 1; j < column; j++) //遍历dp，寻路
            {
                for (int i = 0; i < row; i++)
                {
                    dp[i, j] = INF;
                    int bit = j >> (i - 1);
                    if ((bit & 1) == 1)
                    {
                        continue;
                    }
                    for (int k = 1; k < row; k++)
                    {
                        int bit2 = j >> (k - 1);
                        int bit3 = j ^ (1 << (k - 1));
                        if ((bit2 & 1) == 1 && dp[i, j] > _subMap.Edges[i, k].Weight + dp[k, bit3])
                        {
                            dp[i, j] = _subMap.Edges[i, k].Weight + dp[k, bit3];
                        }
                    }
                }
            }
            bool[] hasVisited = new bool[MAX];
            int pt = 0, e = column - 1, temp = 0, u = 0;
            while (u++ < 10) //防止死循环...
            {
                int min = INF;
                for (int i = 0; i < row; i++)
                {
                    int bit = 1 << (i - 1);
                    if (!hasVisited[i] && (e & bit) == 1)
                    {
                        if (min > _globalMap.Edges[_subMap.Points[i].Id, pt].Weight + dp[i, e ^ bit])
                        {
                            min = _globalMap.Edges[_subMap.Points[i].Id, pt].Weight + dp[i, e ^ bit];
                            temp = _subMap.Points[i].Id;
                        }
                    }
                }
                if (pt == temp)
                {
                    break;
                }
                pt = temp;
                res.Add(pt);
                hasVisited[pt] = true;
                e ^= (1 << (pt - 1));
            }
            res.Add(points[0]);
            return res;
        }

        public static int GetClosetPathLength(int startId, int endId) //startId=start.ID, endId=end.ID
        {
            int pointCount = _globalMap.Size; //点的数量
            int[] distance = new int[MAX]; //每个点到初始点的距离
            Array.Fill(distance, INF);
            distance[startId] = 0;
            int t = startId;

            for (int z = 1; z < pointCount; z++)
            {
                //找点的相邻边,该边标记为1，更新边对应的下个点的dist
                for (int i = 0; i < pointCount; i++)
                {
                    if (i != t && _globalMap.Edges[t, i].Weight != INF &&
                        distance[i] > distance[t] + _globalMap.Edges[t, i].Weight)
                    {
                        distance[i] = distance[t] + _globalMap.Edges[t, i].Weight;
                    }
                }

                //遍历所有点，寻找下一个距离最近的点
                int mmin = MAX;
                int x = -1;
                for (int i = 0; i < pointCount; i++)
                {
                    if (i != t && distance[i] >= distance[t] && distance[i] < mmin)
                    {
                        mmin = distance[i];
                        x = i;
                    }
                }
                t = x;
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

        private static void CreateSubMap(List<int> points)
        {
            int pointCount = points.Count, currentId = 0;
            Vertex[] subPoints = new Vertex[pointCount];
            Edge[,] subEdges = new Edge[pointCount, pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                //endId[i] = new Edge[len];
                subPoints[i] = new(points[i], 0, 0);
                for (int j = 0; j < pointCount; j++)
                {
                    subEdges[i, j] = new(currentId++, i == j ? INF : GetClosetPathLength(points[i], points[j]));
                }
            }
            _subMap = new(pointCount, subPoints, subEdges);
        }

        #endregion
    }

    public static class Navigate
    {
        public static void Show(List<int> points)
        {
            UI.MapWindow mapWindow = new();
            mapWindow.ShowDialog();
        }
    }
}