namespace aoc.Days;

class Day12
{
    public List<(string, string)> Data(string path)
    {
        return Lib.LoadList(path, s =>
        {
            var parts = s.Split('-');
            return (parts.First(), parts.Last());
        });
    }

    public int Part1(List<(string, string)> data)
    {
        Func<string, string, bool> predicate = (path, new_end) =>
        {
            // we cannot visit start twice 
            if (new_end == "start") { return false; }
            //We can revist upper
            if (new_end.Any(char.IsUpper)) { return true; }

            return !path.Contains(new_end);
        };

        return FindPaths(data, predicate);
    }

    public long Part2(List<(string, string)> data)
    {
        Func<string, string, bool> predicate = (path, new_end) =>
        {
            // we cannot visit start twice 
            if (new_end == "start") { return false; }
            if (new_end == "end") { return true; }
            //We can revist upper
            if (new_end.Any(char.IsUpper)) { return true; }


            //if anything is already two and this exists we can't go that way 
            if (path.Contains(new_end)
                && path.Split(',').Where(p => p.All(char.IsLower)).GroupBy(x => x).Any(g => g.Key.Any(char.IsLower) && g.Count() == 2))
                return false;
            return true;
        };

        return FindPaths(data, predicate);
    }


    public int FindPaths(List<(string, string)> data, Func<string, string, bool> predicate)
    {
        HashSet<string> results = new HashSet<string>();
        HashSet<string> visted = new HashSet<string>();
        var q = new Queue<string>(new[] { "start" });

        while (q.Any())
        {
            var path = q.Dequeue();
            visted.Add(path);

            var last = path.Split(',').Last();

            var newPaths = data
                .Where(d => d.Item1 == last || d.Item2 == last)
                .Select(d => d.Item1 == last ? d.Item2 : d.Item1)
                .Where(new_end => predicate(path, new_end))
                .Select(d => String.Concat(path, ",", d));

            foreach (var p in newPaths)
            {
                if (p.EndsWith("end"))
                {
                    if (!results.Contains(p))
                        results.Add(p);
                }
                else if (!visted.Contains(p))
                {
                    q.Enqueue(p);
                }
            }
        }

        return results.Count();
    }

}