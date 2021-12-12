using Xunit;
using aoc.Days;


public class Day9Test
{
    [Fact]
    public void PointLineA()
    {
        int[,] array4 = {
            {2,1,9,9,9,4,3,2,1,0},
            {3,9,8,7,8,9,4,9,2,1},
            {9,8,5,6,7,8,9,8,9,2},
            {8,7,6,7,8,9,6,7,8,9},
            {9,8,9,9,9,6,5,6,7,8}
        };
        Day9 d9 = new Day9();
        Assert.Equal(15, d9.Part1(array4));





    }
}



