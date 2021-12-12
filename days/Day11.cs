namespace aoc.Days;

class Day11
{
    public int[,] Data(string path)
    {
        return Lib.LoadMatrix(path);
    }
    public int Part1(int[,] data)
    {
        var count = 100;
        var flashes = 0;
        while (count > 0)
        {
            count--;
            var r = DoStep(data);
            flashes += r.Item1;
            data = r.Item2;
        }

        return flashes;
    }
    public long Part2(int[,] data)
    {
        var steps = 0;
        while (!AllZeros(data))
        {
            steps += 1;
            var r = DoStep(data);
            data = r.Item2;

        }
        return steps;
    }
    bool AllZeros(int[,] data)
    {
        for (int y = 0; y < data.GetLength(0); y++)
        {
            for (int x = 0; x < data.GetLength(1); x++)
            {
                if (data[y, x] != 0) return false;
            }
        }
        return true;
    }

    void Print(int[,] data)
    {
        Console.Write(System.Environment.NewLine);
        Console.WriteLine("----STEP----");

        for (int y = 0; y < data.GetLength(0); y++)
        {
            for (int x = 0; x < data.GetLength(1); x++)
            {
                Console.Write("{0:0}, ", data[y, x]);
            }
            Console.Write(System.Environment.NewLine);

        }
    }

    (int, int[,]) DoStep(int[,] data)
    {
        var y_len = data.GetLength(0);
        var x_len = data.GetLength(1);
        var flashes = 0;
        var flash_again = false;

        //Increment
        for (int y = 0; y < y_len; y++)
        {
            for (int x = 0; x < x_len; x++)
            {
                data[y, x] += 1;
                if (data[y, x] > 9) { flash_again = true; }
            }
        }
        while (flash_again)
        {
            flash_again = false;

            for (int y = 0; y < y_len; y++)
            {
                for (int x = 0; x < x_len; x++)
                {

                    if (data[y, x] > 9)
                    {
                        data[y, x] = 0;
                        flashes += 1;


                        foreach (var (neighY, neighX) in Neighbours(y, x, y_len, x_len))
                        {
                            switch (data[neighY, neighX])
                            {
                                case 0:
                                    break;
                                case 9:
                                    flash_again = true;
                                    data[neighY, neighX] += 1;
                                    break;
                                default:
                                    data[neighY, neighX] += 1;
                                    break;
                            }
                        }

                    }
                }
            }
        }
        return (flashes, data);
    }

    public IEnumerable<(int, int)> Neighbours(int originY, int originX, int boundsY, int boundsX)
    {
        var offsets = new[] {
            (-1, -1), (-1, 0), (-1, +1),
            (0, -1),(0, +1),
            (+1, -1), (+1, 0), (+1, +1)};

        foreach (var (offY, offX) in offsets)
        {
            var y = originY + offY;
            var x = originX + offX;

            if (y >= 0 && y < boundsY && x >= 0 && x < boundsX)
                yield return (y, x);

        }
    }

}