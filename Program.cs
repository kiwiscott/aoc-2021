//var (typeName, p1, p2) = ProcessArgs(args);
var (typeName, p1, p2) = ("aoc.Days.Day22", false, true);


if (typeName == "ALL")
{
    for (int i = 1; i <= 25; i++)
    {
        RunMethod("aoc.Days.Day" + i, "Part1", true);
        RunMethod("aoc.Days.Day" + i, "Part2", true);
    }
}
else
{
    if (p1)
        RunMethod(typeName, "Part1");

    if (p2)
        RunMethod(typeName, "Part2");
}


static Tuple<string, bool, bool> ProcessArgs(string[] args)
{
    string typeName = "aoc.Days.Day" + DateTime.Now.Day;

    bool runPart1 = true;
    bool runPart2 = true;

    for (int i = 0; i < args.Length; i++)
    {
        if (args[i] == "ALL")
        {
            return Tuple.Create("ALL", runPart1, runPart2);
        }
        else if (args[i] == "-part" && args.GetValue(i + 1) is not null)
        {
            runPart1 = args[i + 1] == "1";
            runPart2 = args[i + 1] == "2";
        }
        else if (args[i] == "-day" && args.GetValue(i + 1) is not null)
        {
            typeName = "aoc.Days.Day" + args[i + 1];
        }
    }
    return Tuple.Create(typeName, runPart1, runPart2);
}


static bool RunMethod(string type, string method, bool silent = false)

{
    var t = Type.GetType(type);

    if (t is null)
    {
        if (!silent) { Console.WriteLine("Type {0} not found", type); }
        return false;
    }

    var constr = t.GetConstructor(System.Type.EmptyTypes);
    if (constr is null)
    {
        Console.WriteLine("Type {0} no empty constructor", type);
        return false;
    }
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
        return false;
    }

    if (data is null)
    {
        Console.WriteLine("{0}: Data Returned is Null", t.Name);
        return false;
    }


    var builderMethod = t.GetMethod(method);

    if (builderMethod is not null)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();


        var result = builderMethod.Invoke(o, new object[] { data });
        watch.Stop();
        Console.WriteLine("{0}.{1} : {2} in {3}ms", t.Name, method, result, watch.ElapsedMilliseconds);
        return true;
    }
    else
    {
        Console.WriteLine("{0}.{1} : Missing", t.Name, method);
        return false;
    }

}