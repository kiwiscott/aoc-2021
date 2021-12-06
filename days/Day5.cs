namespace aoc.Days;

class Day5
{
    public List<EndPoint> Data(string path)
    {
        var parse = delegate (string s)
        {
            var parts = s.Split(" -> ");
            var p1 = parts[0].Split(",");
            var p2 = parts[1].Split(",");

            return new EndPoint(
                    new Point(
                            int.Parse(p1.First()),
                            int.Parse(p1.Last())
                    ), new Point(
                            int.Parse(p2.First()),
                            int.Parse(p2.Last())
                            ));
        };

        var d = Lib.LoadList(path, parse);
        return d;
    }
    public int Part1(List<EndPoint> data)
    {
        return data.Select(ep => ep.AllPoints(includeDiagonal: false))
            .Aggregate(new List<Point>(), (l, points) => { l.AddRange(points); return l; })
            .GroupBy(p=>p)
            .Count(p => p.Count() > 1);
    }
    public int Part2(List<EndPoint> data)
    {
        return data.Select(ep => ep.AllPoints(includeDiagonal: true))
            .Aggregate(new List<Point>(), (l, points) => { l.AddRange(points); return l; })
            .GroupBy(p => p)
            .Count(p => p.Count() > 1);
    }

    public record struct Point(int x, int y);

    public record struct EndPoint(Point Start, Point End)
    {
        public IEnumerable<Point> AllPoints(bool includeDiagonal)
        {
            if (!includeDiagonal && !(Start.y == End.y || End.x == Start.x))
                yield break;


            //Number of Steps 
            var steps = Math.Max(Math.Abs(Start.x - End.x), Math.Abs(Start.y - End.y));

            for (int i = 0; i <= steps; i++)
            {
                var x = Start.x == End.x ? Start.x : (Start.x < End.x ? Start.x + i : Start.x - i);
                var y = Start.y == End.y ? Start.y : (Start.y < End.y ? Start.y + i : Start.y - i);
                yield return new Point(x, y);
            }
        }
    }
}