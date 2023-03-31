using System.Numerics;

namespace StudentScheduleManagementSystem.Map
{
    public static class Location
    {
        private const int INF = 500;
        private const int MAX = 500;
        private const int MaxDoor = 4;
        private const int MaxAffair = 6;
        private static AdjacencyMatrix _globalMap, _subMap;

        #region structs

        private struct Edge //边
        {
            public int Id { get; init; }
            public int Weight { get; init; } //权重

            public Edge(int id, int weight)
            {
                Id = id;
                Weight = weight;
            }
        }

        public struct Point //点
        {
            public int Id { get; init; }
            public int X { get; init; }
            public int Y { get; init; }
            public string? BuildingName { get; init; }

            public Point(int id, int x, int y, string? buildingName)
            {
                Id = id;
                X = x;
                Y = y;
                BuildingName = buildingName;
            }
        }

        public struct Building//建筑
        {
            public int Id { get; set; }
            public int DoorNumber { get; set; }//门的数量
            public string Name { get; set; }
            public Point[] Doors { get; set; } = new Point[MaxDoor]; //门所在的点，用来寻路

            public Building(int id, string name, Point[] doors)
            {
                Id = id;
                Name = name;
                Doors = doors;
                DoorNumber = doors.Length;
                if (DoorNumber > MaxDoor)
                {
                    throw new ArgumentOutOfRangeException(nameof(DoorNumber));
                }
            }

        }

        private struct AdjacencyMatrix //邻接矩阵
        {
            public int Len { get; set; } //总点数
            public Point[] Points { get; set; } = new Point[MAX];
            public Edge[,] Edges { get; set; } = new Edge[MAX, MAX];
            public bool[,] HasVisited { get; set; } = new bool[MAX, MAX]; //每条边是否走过

            public AdjacencyMatrix(int len, Point[] p, Edge[,] e)
            {
                Len = len;
                for (int i = 0; i < len; i++)
                {
                    Points[i] = p[i];
                    for (int j = 0; j < len; j++)
                    {
                        Edges[i, j] = e[i, j];
                        HasVisited[i, j] = false;
                    }
                }
            }
        }

        #endregion

        #region API on pathfinding

        public static List<int> GetClosetPath(int startId, int endId) //startId=start.ID, endId=end.ID
        {
            List<int>[] route = new List<int>[100];
            int pointCount = _globalMap.Len; //点的数量
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
            int row = _subMap.Len, column = 1 << (row - 1);
            int[,] dp = new int[MaxAffair, 1 << MaxAffair];
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
            int pointCount = _globalMap.Len; //点的数量
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
            Point[] subPoints = new Point[pointCount];
            Edge[,] subEdges = new Edge[pointCount, pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                //endId[i] = new Edge[len];
                subPoints[i] = new(points[i], 0, 0, null);
                for (int j = 0; j < pointCount; j++)
                {
                    subEdges[i, j] = new(currentId++, i == j ? INF : GetClosetPathLength(points[i], points[j]));
                }
            }
            _subMap = new(pointCount, subPoints, subEdges);
        }

        #endregion
    }
}