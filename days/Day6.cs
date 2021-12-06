namespace aoc.Days;

class Day6
{
    public long[] Data(string path)
    {
        var data = Lib.LoadFile(path);
        return data.First().Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(new long[9], (days, s) =>
            {
                var index = long.Parse(s);
                days[index] = days[index] += 1;
                return days;
            });
    }

    public long Part1(long[] days)
    {
        RunDay(days, 80);
        return days.Sum();
    }
    public long Part2(long[] days)
    {
        RunDay(days, 256);
        return days.Sum();
    }

    public void RunDay(long[] days, int number_of_days)
    {
        //C# doesn't have Queue or LinkedLists 
        //that can be changed so since this is easy I'll do it manualy.
        //Used Loops and Switches but this is the nicest way to do this. 
        while (number_of_days != 0)
        {
            number_of_days--;
            var tospawn = days[0];

            days[0] = days[1];
            days[1] = days[2];
            days[2] = days[3];
            days[3] = days[4];
            days[4] = days[5];
            days[5] = days[6];
            days[6] = days[7] + tospawn;
            days[7] = days[8];
            days[8] = tospawn;
        }
    }
}