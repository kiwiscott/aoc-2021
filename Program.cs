
//If you just run the program it will find the current day and try and run that. 
//If you you want to run a day / part then you can also type that in 
//dotnet run Day1 Part1
using aoc;



var (typeName, p1, p2) = Lib.ProcessArgs(args);

if (p1)
    Lib.RunMethod(typeName, "Part1");

if (p2)
    Lib.RunMethod(typeName, "Part2");

