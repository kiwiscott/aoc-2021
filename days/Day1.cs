
namespace aoc.Days;

public class Day1
{
    public static List<int> Data(string file)
    {
        Func<string?, int> convert = delegate (string? s)
        {

            return int.Parse(s ?? "0");
        };

        var ints = Lib.Load<int>(file, convert);
        return ints;
    }

    public static int Part1(List<int> data)
    {
        var ints = data;

        //1715 is right
        var increase = 0;
        int? last = null;

        foreach (var i in ints)
        {
            if (last is not null && i > last) { increase += 1; };
            last = i;
        }
        return increase;

    }
    public static int Part2(List<int> data)
    {
        var ints = data;


        //1715 is right
        var increase = 0;
        for (int i = 3; i < ints.Count(); i++)
        {
            var prev = ints[i - 3] + ints[i - 2] + ints[i - 1];
            var curr = ints[i - 2] + ints[i - 1] + ints[i];
            if (curr > prev) { increase += 1; };
        }
        return increase;
    }
}
