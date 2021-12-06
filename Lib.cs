using System;
using System.IO;
using System.Reflection;

namespace aoc;
public static class Lib
{
    public static List<T> LoadList<T>(string path, Func<string?, T> process)
    {
        var l = new List<T>();

        // Open the file to read from.
        using (StreamReader sr = File.OpenText(path))
        {
            string? s;
            while ((s = sr.ReadLine()) != null)
            {
                l.Add(process(s));
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


    internal static Tuple<string, bool, bool> ProcessArgs(string[] args)
    {
        string typeName = "aoc.Days.Day" + DateTime.Now.Day;

        bool runPart1 = true;
        bool runPart2 = true;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-part" && args.GetValue(i + 1) is not null)
            {
                runPart1 = args[i + 1] == "1";
                runPart2 = args[i + 1] == "2";
            }
            else if (args[i] == "-day" && args.GetValue(i + 1) is not null)
            {
                typeName = "aoc.Days.Day" + args[i+1];
            }
        }
        return Tuple.Create(typeName, runPart1, runPart2);
    }

    internal static void RunMethod(string type, string method)

    {
        var t = Type.GetType(type);

        if (t is null)
        {
            Console.WriteLine("Type {0} not found", type);
            return;
        }

        var constr = t.GetConstructor(System.Type.EmptyTypes); 
        var o = constr.Invoke(null); 

        object? data = null;

        var dataMethod = t.GetMethod("Data");
        if (dataMethod is not null)
        {
            string filename = String.Format(@"data/{0}.txt", t.Name.ToLower());
            data = dataMethod.Invoke(o, new object[] { filename });
        }
        else
        {

            Console.WriteLine("{0}: Missing Data Function", t.Name);
            return;
        }


        var builderMethod = t.GetMethod(method);

        if (builderMethod is not null)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();


            var result = builderMethod.Invoke(o, new object[] { data });
            watch.Stop();
            Console.WriteLine("{0}.{1} : {2} in {3}ms", t.Name, method, result, watch.ElapsedMilliseconds);
        }
        else
        {
            Console.WriteLine("{0}.{1} : Missing", t.Name, method);
        }

    }
}