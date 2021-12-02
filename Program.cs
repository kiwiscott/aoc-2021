using aoc; 

var (typeName, p1, p2) = Lib.ProcessArgs(args);

if (p1)
    Lib.RunMethod(typeName, "Part1");

if (p2)
    Lib.RunMethod(typeName, "Part2");
