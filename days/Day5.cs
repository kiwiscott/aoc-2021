using System.Collections;
namespace aoc.Days;

public class Day5
{
    public record struct Point(int x, int y);
    public record struct EndPoint(Point start, Point end);

    public static List<EndPoint> Data(string path)
    {
        var parse = delegate (string? s)
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
    public static int Part1(List<EndPoint> data)
    {
        var allPoints = data.Select(ep => AllPoints(ep, false))
            .Aggregate(new List<Point>(), (l, points) => { l.AddRange(points); return l; })
            .GroupBy(p => (p.x, p.y))
            .Where(p => p.Count() > 1);



        return allPoints.Count();

    }
    public static int Part2(List<EndPoint> data)
    {

        var allPoints = data.Select(ep => AllPoints(ep, true))
           .Aggregate(new List<Point>(), (l, points) => { l.AddRange(points); return l; })
           .GroupBy(p => (p.x, p.y))
           .Where(p => p.Count() > 1);

        return allPoints.Count();
    }

    public static IEnumerable<Point> AllPoints(EndPoint ep, bool includeDiagonal)
    {
        //Horizontal or Vertical 
        if (ep.start.y == ep.end.y)
        {
            //Horizontal
            var sorted = (ep.start.x < ep.end.x) ? (ep.start.x, ep.end.x) : (ep.end.x, ep.start.x);
            for (int i = sorted.Item1; i <= sorted.Item2; i++)
            {
                yield return new Point(i, ep.start.y);
            }
        }
        else if (ep.start.x == ep.end.x)
        {
            //Vertical 
            var sorted = (ep.start.y < ep.end.y) ? (ep.start.y, ep.end.y) : (ep.end.y, ep.start.y);
            for (int i = sorted.Item1; i <= sorted.Item2; i++)
            {
                yield return new Point(ep.start.x, i);
            }
        }
        else if (includeDiagonal)
        {
            //Always start at the lowest x...... 
            var topPoint = (ep.start.x < ep.end.x) ? ep.start : ep.end;
            var bottomPoint = (ep.start.x > ep.end.x) ? ep.start : ep.end;
            var goingLeft = topPoint.y > bottomPoint.y; //number plus

            for (int i = topPoint.x; i <= bottomPoint.x; i++)
            {
                var currentY = goingLeft ? topPoint.y + (topPoint.x - i) : topPoint.y - (topPoint.x - i);
                yield return new Point(i, currentY);
            }
        }

    }
}