using System.Text.RegularExpressions;

namespace aoc.Days;
class Day17
{
    public (int, int, int, int) Data(string path)
    {
        //return (20, 30, -10, -5);

        var row = Lib.LoadFile(path).First();
        string pattern = @"([-0-9]+)";
        MatchCollection matches = Regex.Matches(row, pattern);
        return (
                int.Parse(matches[0].Value),
                int.Parse(matches[1].Value),
                int.Parse(matches[2].Value),
                int.Parse(matches[3].Value));

    }

    public long Part1((int, int, int, int) target_area)
    {

        var max_x = Math.Max(target_area.Item1, target_area.Item2);
        var min_y = Math.Min(target_area.Item3, target_area.Item4);

        var max = int.MinValue;

        for (int x = 0; x < max_x; x++)
        {
            for (int y = min_y; y < max_x; y++)
            {
                int? max_for_pair = CheckPair(x, y, target_area);
                if (max_for_pair.HasValue && max < max_for_pair)
                {
                    max = max_for_pair.GetValueOrDefault();
                }
            }
        }

        return max;
    }
    public long Part2((int, int, int, int) target_area)
    {
        var max_x = Math.Max(target_area.Item1, target_area.Item2);
        var min_y = Math.Min(target_area.Item3, target_area.Item4);

        var matches_count = 0;

        for (int x = 0; x <= max_x; x++)
        {
            for (int y = min_y; y <= max_x; y++)
            {
                int? max_for_pair = CheckPair(x, y, target_area);

                if (max_for_pair.HasValue)
                    matches_count += 1;
            }
        }

        return matches_count;
    }

    private int? CheckPair(int x, int y, (int, int, int, int) target_area)
    {
        var velocity = (x, y);
        var pos = (0, 0);
        int max = int.MinValue;

        while (pos.Item1 <= target_area.Item2 && pos.Item2 >= target_area.Item3)
        {
            pos.Item1 += velocity.Item1;
            pos.Item2 += velocity.Item2;

            //Console.WriteLine("velocity = [{0},{1}] pos = [{2},{3}] max={4}", velocity.Item1, velocity.Item2, pos.Item1, pos.Item2, max);

            if (max < pos.Item2) max = pos.Item2;

            if (pos.Item1.Between(target_area.Item1, target_area.Item2)
                && pos.Item2.Between(target_area.Item3, target_area.Item4))
            {
                //Console.WriteLine("velocity = [{0},{1}] pos = [{2},{3}] max={4}", x, y, pos.Item1, pos.Item2, max);
                return max;
            }


            //Adjust Velocity
            if (velocity.Item1 >= 1)
                velocity.Item1 += -1;
            else if (velocity.Item1 <= -1)
                velocity.Item1 += 1;

            velocity.Item2 += -1;
        }

        return null;
    }
}
public static class IntExtension
{
    public static bool Between(this int n, int begin, int end)
    {
        return (n >= begin && n <= end) || (n >= end && n <= begin);
    }

}