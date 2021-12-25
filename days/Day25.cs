namespace aoc.Days;
using System.Text.RegularExpressions;

public class Day25
{
    public char[,] Data(string path)
    {
        var data = Lib.LoadFile(path);

        char[,] numbers = new char[data.Count(), data[0].Count()];
        var current_row = 0;
        var current_column = 0;
        foreach (var line in data)
        {
            foreach (var c in line.ToCharArray())
            {
                numbers[current_row, current_column] = c;
                current_column++;
            }
            current_row++;
            current_column = 0;
        }
        return numbers;
    }

    const char DOWN = 'v';
    const char RIGHT = '>';
    const char EMPTY = '.';

    public long Part1(char[,] seabed)
    {
        int count = 0;
        bool moved = true;

        //Print(seabed);

        while (moved)
        {

            var right = MoveRight(seabed);
            var down = MoveDown(seabed);
            moved = right || down;
            count++;
            

        }
        //Print(seabed);


        return count+1;
    }
    public long Part2(char[,] seabed)
    {
        return 14;
    }

    void Print(char[,] data)
    {
        Console.Write(System.Environment.NewLine);
        Console.WriteLine("----STEP---- y:{0}, x:{1}", data.GetLength(0), data.GetLength(1));

        for (int y = 0; y < data.GetLength(0); y++)
        {
            for (int x = 0; x < data.GetLength(1); x++)
            {
                Console.Write(data[y, x]);
            }
            Console.Write(System.Environment.NewLine);

        }
    }
    bool MoveRight(char[,] data)
    {
        var newx = (int x) => { return ((x + 1 < data.GetLength(1)) ? x + 1 : 0); };
        var changes = from y in Enumerable.Range(0, data.GetLength(0))
                      from x in Enumerable.Range(0, data.GetLength(1))
                      where data[y, x] == RIGHT && data[y, newx(x)] == EMPTY
                      select new { y, x, nx = newx(x) };

        foreach (var c in changes.ToList())
        {
            data[c.y, c.x] = EMPTY;
            data[c.y, c.nx] = RIGHT;
        }
        return changes.Any();

    }
    bool MoveDown(char[,] data)
    {


        var newy = (int y) => { return ((y + 1 < data.GetLength(0)) ? y + 1 : 0); };

        var changes = from y in Enumerable.Range(0, data.GetLength(0))
                      from x in Enumerable.Range(0, data.GetLength(1))
                      where data[y, x] == DOWN && data[newy(y), x] == EMPTY
                      select new { y, x, ny = newy(y)};

        foreach (var c in changes.ToList())
        {
            data[c.y, c.x] = EMPTY;
            data[c.ny, c.x] = DOWN;
        }
        return changes.Any();
    }
}