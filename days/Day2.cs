
namespace aoc.Days;

public class Day2
{
    public static object Data(string file)
    {
        Func<string?, Command> convert = delegate (string? s)
        {
            var parts = s.Split(" ");
            var x = Direction.Forward;

            switch (parts[0])
            {

                case "forward":
                    x = Direction.Forward;
                    break;
                case "down":
                    x = Direction.Down;
                    break;
                case "up":
                    x = Direction.Up;
                    break;
                default:
                    Console.WriteLine(s);
                    throw new InvalidCastException("Data in row not correct");
            }
            return new Command(x, int.Parse(parts[1]));
        };


        var d = Lib.Load<Command>(file, convert);
        return d;
    }

    public record struct Command(Direction direction, int steps);
    public enum Direction { Forward, Down, Up }

    public static int Part1(List<Command> data)
    {
        var g = data.GroupBy(
            item => item.direction, 
            item => item.steps, 
            (direction,steps) => new {Key = direction, Sum = steps.Sum()} 
        ).ToDictionary(k => k.Key);

        return (g[Direction.Down].Sum -  g[Direction.Up].Sum) *  g[Direction.Forward].Sum;
    }
    public static int Part2(List<Command> data)
    {
        /*
        down X increases your aim by X units.
        up X decreases your aim by X units.
        forward X does two things:
        It increases your horizontal position by X units.
        It increases your depth by your aim multiplied by X.
        */
        var depth = 0;
        var horizontal_position = 0;
        var aim = 0;

        foreach (var item in data)
        {
            switch (item.direction)
            {
                case Direction.Down:
                    aim = aim + item.steps;
                    break;
                case Direction.Up:
                    aim = aim - item.steps;
                    break;
                case Direction.Forward:
                    horizontal_position = horizontal_position + item.steps;
                    depth = depth + (aim * item.steps);
                    break;
            }
        }

        return depth * horizontal_position;
    }
}
