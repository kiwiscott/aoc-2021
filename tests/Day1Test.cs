using Xunit;
using aoc.Days; 

public class Day1Test
{
    [Fact]
    public void Test1()
    {
        Day1 d = new Day1(); 
        var i = new List<int>() {1,2,1}; 
        var r = d.Part1(i);
        Assert.Equal(1, r);
    }
}