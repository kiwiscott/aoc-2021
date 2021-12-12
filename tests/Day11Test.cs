using Xunit;
using aoc.Days;


public class Day11NeighboursTest
{
    [Fact]
    public void Neighbours()
    {

        Day11 d = new Day11();
        Assert.Equal(3, d.Neighbours(0, 0, 5, 5).Count()); 
    }
}



