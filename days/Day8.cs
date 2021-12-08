namespace aoc.Days;

class Day8
{
    public record SignalPattern(List<string> input, List<string> output);

    public List<SignalPattern> Data(string path)
    {
        return Lib.LoadList(path, (s) =>
        {
            var input = s.Split("|", StringSplitOptions.RemoveEmptyEntries).First().Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.ToCharArray().OrderBy(p => p).Aggregate("", (s, c) => { s += c; return s; }));
            var output = s.Split("|", StringSplitOptions.RemoveEmptyEntries).Last().Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.ToCharArray().OrderBy(p => p).Aggregate("", (s, c) => { s += c; return s; }));
            return new SignalPattern(input.ToList(), output.ToList());
        });
    }


    public int Part1(List<SignalPattern> patterns)
    {
        var uniqueNumbers = new[] { 2, 3, 4, 7 };
        return patterns.Select(p => p.output.Count(s => uniqueNumbers.Any(a => a == s.Length))).Sum();
    }
    public int Part2(List<SignalPattern> patterns)
    {
        var result = patterns.Aggregate(0, (i, pattern) =>
        {
            var lookup = ProcessSignals(pattern);
            //Collect the matched digits to a string and then change to an int 
            var collected = pattern.output.Aggregate("", (collector, s) => { collector += lookup[s]; return collector; });
            i += int.Parse(collected);
            return i;
        });

        return result;
    }
    private Dictionary<string, string> ProcessSignals(SignalPattern pattern)
    {
        /*
        Given the list that we have for a basic rules we can figure out the values in the inputs by processing the number of 
        letters in order and the known patterns of the first 4 constant numbers. e.g we know 9 is the only digit where all the letters of 4 are present. 

        1 = item with 2
        4 = item with 4
        7 = item with 3
        8 = item with 7

        //
        #9 - 6 and all digits of 4 
        #0 - 6 all all digits of 1 and not 9
        #6 - 6 and NOT 9 or 6 
        //
        3 - 5 and all digits of 7 
        5 - 5 and all the letters in 5 are in 9 
        2 - 5 and NOT 3 or 5 
        */


        //acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf
        var s1 = pattern.input.First(p => p.Length == 2);
        var s4 = pattern.input.First(p => p.Length == 4);
        var s7 = pattern.input.First(p => p.Length == 3);
        var s8 = pattern.input.First(p => p.Length == 7);
        //Six Digits 
        var s9 = pattern.input.First(p => p.Length == 6 && s4.All(c => p.Contains(c)));
        var s0 = pattern.input.First(p => p.Length == 6 && p != s9 && s1.All(c => p.Contains(c)));
        var s6 = pattern.input.First(p => p.Length == 6 && p != s9 && p != s0);
        //5 Digits 
        var s3 = pattern.input.First(p => p.Length == 5 && s7.All(c => p.Contains(c)));
        var s5 = pattern.input.First(p => p.Length == 5 && p != s3 && 5 == s9.Count(c => p.Contains(c)));
        var s2 = pattern.input.First(p => p.Length == 5 && p != s3 && p != s5);

        var d = new Dictionary<string, string>();
        d.Add(s0, "0");
        d.Add(s1, "1");
        d.Add(s2, "2");
        d.Add(s3, "3");
        d.Add(s4, "4");
        d.Add(s5, "5");
        d.Add(s6, "6");
        d.Add(s7, "7");
        d.Add(s8, "8");
        d.Add(s9, "9");
        return d;
    }
}