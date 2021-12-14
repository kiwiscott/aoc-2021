namespace aoc.Days;

class Day14
{
    Dictionary<string, char> rules;
    Dictionary<(char, char, int), Dictionary<char, long>> cache;

    public Day14()
    {
        rules = new Dictionary<string, char>();
        cache = new Dictionary<(char, char, int), Dictionary<char, long>>();
    }

    public string Data(string path)
    {
        var lines = Lib.LoadFile(path);
        string polymer_template = lines.First();

        rules = new Dictionary<string, char>();

        foreach (var line in lines.Skip(2))
        {
            var parts = line.Split(" -> ");
            rules.Add(parts.First(), parts.Last().Last());
        }

        return polymer_template;
    }

    public long Part1(string polymer_template)
    {
        var pt = PolymerDictionary(polymer_template, 10);

        var min = pt.MinBy(p => p.Value).Value;
        var max = pt.MaxBy(p => p.Value).Value;

        //2112
        return max - min;
    }

    public long Part2(string polymer_template)
    {
        var pt = PolymerDictionary(polymer_template, 40);

        var min = pt.MinBy(p => p.Value).Value;
        var max = pt.MaxBy(p => p.Value).Value;

        return max - min;
    }

    private Dictionary<char, long> PolymerDictionary(string polymer_template, int run_process_times)
    {
        var result = polymer_template.Aggregate(new Dictionary<char, long>(), (d, c) => { d[c] = d.GetValueOrDefault(c) + 1; return d; });

        polymer_template
            .ToCharArray().SkipLast(1).Select((value, i) => (value, polymer_template[i + 1])).ToList()
            .ForEach(a => MergeInto(result, PairFactor(a.Item1, a.Item2, run_process_times)));

        return result;
    }

    private void MergeInto(Dictionary<char, long> into_dict, Dictionary<char, long> from_dict)
    {
        foreach (var (key, value) in from_dict)
            into_dict[key] = into_dict.GetValueOrDefault(key) + value;
    }

    private Dictionary<char, long> PairFactor(char left, char right, int process_multiplier)
    {
        var cache_key = (left, right, process_multiplier);


        if (cache.ContainsKey(cache_key))
        {
            return cache[cache_key];
        }

        Dictionary<char, long> result = new Dictionary<char, long>();

        if (process_multiplier == 0)
        {
            return result;
        }
        //if there's no matched rules 
        var middle = char.MaxValue;
        if (!this.rules.TryGetValue(left.ToString() + right.ToString(), out middle))
            return result;

        var new_process_multiplier = process_multiplier - 1;

        result[middle] = result.GetValueOrDefault(middle) + 1;

        //can't do a 
        foreach (var left_result in PairFactor(left, middle, new_process_multiplier))
            result[left_result.Key] = result.GetValueOrDefault(left_result.Key) + left_result.Value;

        foreach (var right_result in PairFactor(middle, right, new_process_multiplier))
            result[right_result.Key] = result.GetValueOrDefault(right_result.Key) + right_result.Value;


        cache.Add(cache_key, result);
        return result;
    }
}