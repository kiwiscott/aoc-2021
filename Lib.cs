using System.Reflection;

namespace aoc;
public static class Lib
{
    public static List<T> LoadList<T>(string path, Func<string, T> process)
    {
        var l = new List<T>();

        // Open the file to read from.
        using (StreamReader sr = File.OpenText(path))
        {
            string? s;
            while ((s = sr.ReadLine()) != null)
            {
                if (s is not null) l.Add(process(s));
            }
        }
        return l;
    }
    public static List<string> LoadFile(string path)
    {
        var l = new List<string>();

        // Open the file to read from.
        using (StreamReader sr = File.OpenText(path))
        {
            string? s;
            while ((s = sr.ReadLine()) != null)
            {
                l.Add(s);
            }
        }
        return l;
    }

    public static int[,] LoadMatrix(string path)
    {
        var data = Lib.LoadFile(path);

        int[,] numbers = new int[data.Count(), data[0].Count()];
        var current_row = 0;
        var current_column = 0;
        foreach (var line in data)
        {
            foreach (var c in line)
            {
                numbers[current_row, current_column] = int.Parse(c.ToString());
                current_column++;
            }
            current_row++;
            current_column = 0;
        }
        return numbers;
    }

}