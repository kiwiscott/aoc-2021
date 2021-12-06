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


}