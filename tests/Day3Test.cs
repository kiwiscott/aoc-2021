using Xunit;
using aoc.Days;

public class Day3Test
{
    [Fact]
    public void Test1()
    {
        Day3 d = new Day3(); 
        var data = new string[] { "00100", "11110", "10110", "10111", "10101", "01111", "00111", "11100", "10000", "11001", "00010", "01010" };
        var ba = data.Select(s => d.Convert(s)).ToList();
        Assert.Equal(198, d.Part1(ba));
    }
    [Fact]
    public void TestPart2()
    {
        Day3 d = new Day3(); 
        var data = new string[] { "00100", "11110", "10110", "10111", "10101", "01111", "00111", "11100", "10000", "11001", "00010", "01010" };
        var ba = data.Select(s => d.Convert(s)).ToList();
        Assert.Equal(230, d.Part2(ba));
    }

}




