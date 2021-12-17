using System.Text.RegularExpressions;

namespace aoc.Days;
class Day17
{
    public (int, int, int, int) Data(string path)
    {
        var row = Lib.LoadFile(path).First(); 
        string pattern = @"([-0-9]+)"; 
        MatchCollection matches = Regex.Matches (row, pattern);
        return (
                int.Parse(matches[0].Value),
                int.Parse(matches[1].Value),
                int.Parse(matches[2].Value), 
                int.Parse(matches[3].Value)); 

    }

    public long Part1((int, int, int, int) target_area)
    {
        /*
        The probe's x position increases by its x velocity.
        The probe's y position increases by its y velocity.
        Due to drag, the probe's x velocity changes by 1 toward the value 0; that is, it decreases by 1 
        if it is greater than 0, increases by 1 if it is less than 0, or does not change if it is already 0.
        Due to gravity, the probe's y velocity decreases by 1.
        */
        var max_x = Math.Max(target_area.Item1, target_area.Item2);
        var min_y =Math.Abs(Math.Min(target_area.Item3, target_area.Item4)); 

        var max = int.MinValue;
        var pair_max = (0,0);  

        for (int x = 1; x < max_x; x++)
        {
            for (int y= 1; y < max_x; y++)
            {
                int? max_for_pair = CheckPair(x,y,target_area); 
                if(max_for_pair.HasValue  && max < max_for_pair)
                {
                    max = max_for_pair.GetValueOrDefault(); 
                    pair_max = (x,y); 
                }
            }
        }

        Console.WriteLine(max);
        Console.WriteLine(pair_max);


        return 99;

    }

    private int? CheckPair(int x, int y, (int, int, int, int) target_area)
    {
        var velocity = (x, y);
        var pos = (0, 0);
        int max = int.MinValue;  

        while (pos.Item1 < target_area.Item2 && pos.Item2 > target_area.Item4)
        {
            pos.Item1 += velocity.Item1;
            pos.Item2 += velocity.Item2;

            if(max < pos.Item2) max = pos.Item2; 

            if(pos.Item1 >= target_area.Item1 && pos.Item1 <= target_area.Item2 
            && pos.Item2 >= target_area.Item3 && pos.Item2 <= target_area.Item4 )
            {
                Console.WriteLine("velocity = [{0},{1}] pos = [{2},{3}] max={4}", x,y, pos.Item1, pos.Item2, max); 
                return max; 
            }


            //Adjust Velocity
            if (velocity.Item1 > 1)
                velocity.Item1 += -1;
            else if (velocity.Item1 < 1)
                velocity.Item1 += 1;

            velocity.Item2--;
        }
        //Console.WriteLine("noMatch = [{0},{1}]", x,y); 

        return null; 
    }

    public long Part2((int, int, int, int) target_area)
    {
        return -9;
    }
}