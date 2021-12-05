using Xunit;

public class Day1
{
    [Fact]
    public void Test1()
    {
        List<int> i = new List<int>();
        i.Add(1);
        i.Add(2);
        i.Add(1);

        var r = aoc.Days.Day1.Part1(i);
        Assert.Equal(1, r);
    }
}