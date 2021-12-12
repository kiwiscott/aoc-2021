namespace aoc.Days;

class Day9
{
    public int[,] Data(string path)
    {
        return  Lib.LoadMatrix(path);
    }
    public int Part1(int[,] data)
    {
        int low_point_sum = 0;
        var y_len = data.GetLength(0);
        var x_len = data.GetLength(1);

        for (int y = 0; y < y_len; y++)
        {
            for (int x = 0; x < x_len; x++)
            {
                if (Neighbours(y, x, y_len, x_len).All(neighbour => data[y, x] < data[neighbour.Item1, neighbour.Item2]))
                {
                    low_point_sum += data[y, x] + 1;
                }
            }
        }
        return low_point_sum;
    }
    public int Part2(int[,] data)
    {
        var visited = new SortedSet<(int, int)>();
        var basinSizes = new List<int>();

        for (int y = 0; y < data.GetLength(0); y++)
        {
            for (int x = 0; x < data.GetLength(1); x++)
            {
                //if we haven't processed this value then process it unless its a 9
                if (data[y, x] == 9 || visited.Contains((y, x))) continue;

                var basin = BasinFrom(data, y, x);
                //Console.WriteLine("BASIN = {0}", basin.Count());
                basinSizes.Add(basin.Count());

                foreach (var v in basin)
                {
                    visited.Add(v);
                };

            }
        }

        return basinSizes.OrderByDescending(p => p).Take(3).Aggregate(1, (i, p) => i *= p);
    }
    public IEnumerable<(int, int)> Neighbours(int originY, int originX, int boundsY, int boundsX)
    {
        if (originY - 1 >= 0) yield return (originY - 1, originX);
        if (originY + 1 < boundsY) yield return (originY + 1, originX);

        if (originX - 1 >= 0) yield return (originY, originX - 1);
        if (originX + 1 < boundsX) yield return (originY, originX + 1);
    }



    SortedSet<(int, int)> BasinFrom(int[,] data, int originY, int originX)
    {
        var visited = new SortedSet<(int, int)>();
        var toProcess = new Queue<(int, int)>();

        toProcess.Enqueue((originY, originX));

        while (toProcess.Any())
        {
            var (y, x) = toProcess.Dequeue();
            visited.Add((y, x));
            foreach (var (ny, nx) in Neighbours(y, x, data.GetLength(0), data.GetLength(1)))
            {
                if (data[ny, nx] != 9 && !visited.Contains((ny, nx)))
                {
                    toProcess.Enqueue((ny, nx));
                }
            }
        }
        return visited;
    }
}