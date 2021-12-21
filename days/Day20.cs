namespace aoc.Days;

public class Day20
{
    public (string algo, char[,] image) Data(string path)
    {
        var data = Lib.LoadFile(path);
        var algo = data.First();

        int padding = 100;


        char[,] image = OutputImage(data.Skip(2).Count() + padding + padding, data[3].Count() + padding + padding);
        var current_row = padding;
        var current_column = padding;
        foreach (var line in data.Skip(2))
        {
            foreach (var c in line)
            {
                image[current_row, current_column] = c;
                current_column++;
            }
            current_row++;
            current_column = padding;
        }
        return (algo, image);
    }


    public long Part1((string algo, char[,] image) data)
    {
        int count = RunIt(data.algo, data.image, 2);
        return count;
    }
    public long Part2((string algo, char[,] image) data)
    {
        int count = RunIt(data.algo, data.image, 50);
        return count;
    }

    private int RunIt(string algo, char[,] input, int runtimes)
    {

        for (int i = 0; i < runtimes; i++)
        {
            //Empty Output
            var output = OutputImage(input.GetLength(0), input.GetLength(1));

            for (int y = 0; y < output.GetLength(0); y++)
            {
                for (int x = 0; x < output.GetLength(1); x++)
                {
                    var n_string = Neighbours(y, x, input);

                    var index = Convert.ToInt32(n_string.Replace('#', '1').Replace('.', '0'), 2);
                    output[y, x] = algo[index];
                }
            }
            input = output;
            //Print(input);
        }

        //        Print(input);
        //Something is wrong with the out row so just trim it and be done 
        var count = 0;
        int remove_padding = 50;
        for (int y = remove_padding; y < input.GetLength(0) - remove_padding; y++)
        {
            for (int x = remove_padding; x < input.GetLength(1) - remove_padding; x++)
            {
                if (input[y, x] == '#') count++;
            }
        }

        return count;
    }
    public char[,] OutputImage(int len_y, int len_x)
    {
        var output = new char[len_y, len_x];

        for (int y = 0; y < output.GetLength(0); y++)
        {
            for (int x = 0; x < output.GetLength(1); x++)
            {
                output[y, x] = '.';
            }
        }
        return output;
    }

    public string Neighbours(int originY, int originX, char[,] image)
    {
        var offsets = new[] { (-1, -1), (-1, 0), (-1, +1), (0, -1), (0, 0), (0, +1), (+1, -1), (+1, 0), (+1, +1) };
        var y_len = image.GetLength(0);
        var x_len = image.GetLength(1);


        string s = String.Empty;

        foreach (var (offY, offX) in offsets)
        {
            var y = originY + offY;
            var x = originX + offX;

            //We can only se part of the image so assume its dark 
            if (y > 0 && y < y_len && x > 0 && x < x_len)
            {
                s += image[y, x];
            }
            else
            {
                s += '.';
            }
        }
        return s;
    }


    void Print(char[,] data)
    {
        Console.Write(System.Environment.NewLine);

        for (int y = 0; y < data.GetLength(0); y++)
        {
            for (int x = 0; x < data.GetLength(1); x++)
            {
                Console.Write(data[y, x]);
            }
            Console.Write(System.Environment.NewLine);
        }
        Console.Write(System.Environment.NewLine);

    }

}