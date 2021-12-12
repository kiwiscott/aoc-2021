namespace aoc.Days;

class Day11
{
    public int[,] Data(string path)
    {
        return Lib.LoadMatrix(path);
    }
    public int Part1(int[,] data)
    {
        return DoStep(data, 2);
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

    int DoStep(int[,] data, int steps)
    {
        steps = steps - 1;
        var y_len = data.GetLength(0);
        var x_len = data.GetLength(1);
        var flashes = 0;
        var flash_again = false;

        Console.WriteLine("[{0},{1}] n= {2}", y_len, x_len, data.Length);



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

                        foreach (var (neighX, neighY) in Neighbours(y, x, y_len, x_len))
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


        Print(data);


        if (steps > 0)
        {
            return flashes + DoStep(data, steps);
        }

        return flashes;

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
    public long Part2(int[,] data)
    {
        return 0;
    }
}