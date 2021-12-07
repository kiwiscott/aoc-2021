namespace aoc.Days;

class Day7
{
    public SortedDictionary<int, int> Data(string path)
    {
        var data = Lib.LoadFile(path);
        return data.First().Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(new SortedDictionary<int, int>(), (dict, s) =>
             {
                 var i = int.Parse(s);
                 if (dict.ContainsKey(i))
                 {
                     dict[i] = dict[i] + 1;
                 }
                 else
                 {
                     dict[i] = 1;
                 }
                 return dict;
             });
    }

    public int Part1(SortedDictionary<int, int> crabs)
    {
        return Enumerable.Range(0, crabs.Keys.Max()).Min(value => crabs.Select(kv => Math.Abs(kv.Key - value) * kv.Value).Sum());
    }
    public int Part2(SortedDictionary<int, int> crabs)
    {
        var i = Enumerable.Range(0,  crabs.Keys.Max()).Min(value => crabs.Select(kv =>
        {
            var gap = Convert.ToDouble(Math.Abs(kv.Key - value));
            var cost_for_one = gap * ((gap + 1.0) / 2.0);
            return cost_for_one * kv.Value;
        }).Sum());

        return Convert.ToInt32(i);
    }
}