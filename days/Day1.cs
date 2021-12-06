
namespace aoc.Days;

class Day1
{
    public List<int> Data(string file)
    {
        var ints = Lib.LoadList<int>(file, (string s) => int.Parse(s));
        return ints;
    }

    public int Part1(List<int> data)
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
    public int Part2(List<int> data)
    {
        var increase = 0;
        for (int i = 3; i < data.Count(); i++)
        {
            var prev = data[i - 3] + data[i - 2] + data[i - 1];
            var curr = data[i - 2] + data[i - 1] + data[i];
            if (curr > prev) { increase += 1; };
        }
        return increase;
    }
}
