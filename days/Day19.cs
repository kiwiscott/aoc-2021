namespace aoc.Days;
public class Day19
{

    public List<List<(int, int, int)>> Data(string path)
    {
        List<List<(int, int, int)>> scanners = new List<List<(int, int, int)>>();
        List<(int, int, int)> scanner = new List<(int, int, int)>();

        foreach (var s in Lib.LoadFile(path))
        {
            if (s.Contains("scanner"))
            {
                scanner = new List<(int, int, int)>();
            }
            else if (s.Contains(","))
            {
                var indexes = s.Split(',');
                var coord = (int.Parse(indexes[0]), int.Parse(indexes[1]), int.Parse(indexes[2]));
                scanner.Add(coord);
            }
            else if (String.IsNullOrEmpty(s))
            {
                scanners.Add(scanner);
            }
        }
        scanners.Add(scanner);
        return scanners;
    }

    public long Part1(List<List<(int, int, int)>> scanners)
    {
        List<List<float>> differences = new List<List<float>>();

        foreach (var scanner in scanners)
        {
            var query = scanner.SelectMany(x => scanner, (x, y) => new { x, y })
                .Where(p => p.x != p.y)
                .Select(c =>
                {
                    float deltaX = c.x.Item1 - c.y.Item1;
                    float deltaY = c.x.Item2 - c.y.Item2;
                    float deltaZ = c.x.Item3 - c.y.Item3;
                    float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
                    return distance;
                });
            differences.Add(query.Distinct().ToList());
        }
        for (int i = 0; i < differences.Count; i++)
        {
            for (int j = i; j < differences.Count; j++)
            {
                if (i == j) continue;

                if (differences[i].Intersect(differences[j]).Count() >= 12)
                {
                    Console.WriteLine("{0}>{1} [{2} MATCHES]", i, j, differences[i].Intersect(differences[j]).Count());

                }
            }

        }

        //at least 12 beacons
        //so : 12 MUST MATCH 

        //The scanners and beacons map a single contiguous 3d region. 
        // SO THEY ALL JOIN 

        // the scanners also don't know their rotation or facing direction.
        //wE SHOULD ROTATE In total, each scanner could be in any of 24 different orientations: 
        //facing positive or negative x, y, or z, and considering any of four directions "up" from that facing.

        //REGARDLESS OF WHICH DIRECTION THEY ARE FACING tHE GAPS ARE ALWAYS THE SAME 
        //
        //scanners are at most 1000 UNITES AWAY IN X,Y,Z FROM each other 

        return 99;
    }

    public long Part2(List<List<(int, int, int)>> scanners)
    {
        return 99;
    }
}