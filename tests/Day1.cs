using Xunit;

public class UnitTest21
{
    [Fact]
    public void Test1()
    {
        List<int> i = new List<int>();
        var r = aoc.Days.Day1.Part1(i);
        Assert.Equal(0, r);

    }
}