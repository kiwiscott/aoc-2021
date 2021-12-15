namespace aoc.Days;

using Point = Tuple<int, int>;

//CS8600

class Day15
{
    public int[,] Data(string path)
    {
        return Lib.LoadMatrix(path);
    }
    public int Part1(int[,] data)
    {
        return ShortestPath(data,
            new Point(0, 0),
            new Point(data.GetUpperBound(0), data.GetUpperBound(1))
        );

    }
    public int Part2(int[,] data)
    {
        //big a bloody big grid 
        int[,] bigGrid = new int[(1 + data.GetUpperBound(0)) * 5, (1 + data.GetUpperBound(1)) * 5];

        for (int y = 0; y <= bigGrid.GetUpperBound(0); y++)
        {
            for (int x = 0; x <= bigGrid.GetUpperBound(1); x++)
            {
                if (x <= data.GetUpperBound(1) && y <= data.GetUpperBound(0))
                {
                    bigGrid[y, x] = data[y, x];
                }
                else if (y <= data.GetUpperBound(0))
                {
                    //Look Left 
                    var v = bigGrid[y, x - data.GetUpperBound(1) - 1] + 1;
                    bigGrid[y, x] = v > 9 ? 1 : v;
                }
                else
                {
                    //Look Up 
                    var v = bigGrid[y - data.GetUpperBound(0) - 1, x] + 1;
                    bigGrid[y, x] = v > 9 ? 1 : v;
                }

            }
        }
        return ShortestPath(bigGrid,
                    new Point(0, 0),
                    new Point(bigGrid.GetUpperBound(0), bigGrid.GetUpperBound(1))
                );

    }
    void Print(int[,] data)
    {
        Console.Write(System.Environment.NewLine);
        Console.WriteLine("----STEP----");

        for (int y = 0; y < data.GetLength(0); y++)
        {
            for (int x = 0; x < data.GetLength(1); x++)
            {
                Console.Write(data[y, x]);
            }
            Console.Write(System.Environment.NewLine);
        }
    }

    public IEnumerable<Point> Neighbours(Point origin, Point bounds)
    {
        var (y, x) = origin;

        if (x - 1 >= 0) yield return new Point(y, x - 1);
        if (y - 1 >= 0) yield return new Point(y - 1, x);

        if (x + 1 <= bounds.Item2) yield return new Point(y, x + 1);
        if (y + 1 <= bounds.Item1) yield return new Point(y + 1, x);
    }

    int ShortestPath(int[,] grid, Point start, Point goal)
    {
        var grid_bounds = new Point(grid.GetUpperBound(0), grid.GetUpperBound(1));

        var toProcess = new PriorityQueue<(Point, int), int>();
        toProcess.Enqueue((start, 0), 1);

        int[,] costs = new int[grid_bounds.Item1 + 1, grid_bounds.Item2 + 1];
        for (int y = 0; y <= grid_bounds.Item1; y++)
        {
            for (int x = 0; x <= grid_bounds.Item2; x++)
            {
                costs.SetValue(int.MaxValue, y, x);
            }
        }

        costs.SetValue(0, 0, 0);

        int min_matching_value = int.MaxValue;

        while (toProcess.TryDequeue(out var next_item, out var i))
        {
            var (current, cost) = next_item;

            if (current.Item1 == goal.Item1 && current.Item2 == goal.Item2)
            {
                min_matching_value = cost;
            }

            if (cost > min_matching_value) continue;

            foreach (var next in Neighbours(current, grid_bounds))
            {
                var new_cost = cost + grid[next.Item1, next.Item2];

                if (new_cost < costs[next.Item1, next.Item2] && new_cost < min_matching_value)
                {
                    costs[next.Item1, next.Item2] = new_cost;

                    //Use distance to process the nearest to the goal first 
                    //This was a massive speed up 
                    var priority = -1 * (goal.Item1 - next.Item1 + goal.Item2 - next.Item2);

                    toProcess.Enqueue((next, new_cost), priority);
                }
            }
        }

        return min_matching_value;
    }
}