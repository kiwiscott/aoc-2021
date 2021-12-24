namespace aoc.Days;
using System.Text.RegularExpressions;

public class Day24
{
    public record Rule(int idx_end, int idx_st, int diff);
    public List<Rule> Data(string path)
    {
        /*
        Turns out this was more of a lofic puzzle. You need to figire out from the instructions what the logic was. Its boiled  down 
        to a simple set of rules. The index of the input number must match another  index with an offset value 
            e.g. index[3] = index[2] - 8  

            (3,2, -8),
            (5,4,  +8),
            (8,7,  +6),
            (10,9, +5),
            (11,6,  -3),
            (12,1, -1),
            (13,0,  -5)};
        
        We just built the offset list from the fileand generated all valid numbers. 

        */
        var f = Lib.LoadFile(path);

        Stack<(int idx, int val)> stack = new Stack<(int idx, int val)>();
        var rules = new List<Rule>();
        var i = 0;


        foreach (var s in f.Chunk(18))
        {
            var p = s[4].Split(" ").Last() == "1" ? "PUSH" : "POP";

            if (p == "PUSH")
            {
                var v = int.Parse(s[15].Split(" ").Last());
                stack.Push((i, v));
            }
            else
            {
                var tv = int.Parse(s[5].Split(" ").Last());
                var dq = stack.Pop();
                rules.Add(new Rule(i, dq.idx, dq.val + tv));
            }
            i++;
        }

        return rules;
    }

    public long Part1(List<Rule> rules)
    {

        return digits14(rules).Max();

    }
    public long Part2(List<Rule> rules)
    {
        return digits14(rules).Min();
    }

    private List<long> digits14(List<Rule> rules)
    {
        List<string> values = new List<string>();
        values.Add("00000000000000");

        var int_range = Enumerable.Range(1, 9);
        foreach (var rule in rules)
        {

            var valid_pairs = from p1 in int_range
                              from p2 in int_range
                              where (p1 - p2 == rule.diff)
                              select (p1.ToString()[0], p2.ToString()[0]);

            values = values.SelectMany(e =>
            {
                return valid_pairs.Select(p =>
                 {
                     var new_e = e.ToCharArray();
                     new_e[rule.idx_end] = p.Item1;
                     new_e[rule.idx_st] = p.Item2;
                     return new string(new_e) as string;
                 });


            }).ToList<string>();
        }

        return values.Select(p => long.Parse(p)).ToList();

    }

}