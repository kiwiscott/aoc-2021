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

        while (moved)
        {
            var right = MoveRight(seabed);
            var down = MoveDown(seabed);
            moved = right || down;
            count++;
        }
        return count+1;
    }
    public long Part2(char[,] seabed)
    {
        return -1;
    }

    bool MoveRight(char[,] data)
    {
        //We have to remember that we can't change the array as we process through it or else
        //th tests return the results from the changed values. That the 'challenge' with using 
        //ienumerable everywhere. Adding ToList in the second loop means the 'changes' loop is 
        //end till the end before processing. 
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