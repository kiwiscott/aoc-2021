using Xunit;
using aoc.Days;


public class Day5Test
{
    [Fact]
    public void PointLineA()
    {
        var ep = new Day5.EndPoint(new Day5.Point(0, 0), new Day5.Point(0, 10));
        var points = ep.AllPoints(false).ToArray();
        Assert.Equal(11, points.Count());

        Assert.Equal(0, points[0].x);
        Assert.Equal(0, points[0].y);

        Assert.Equal(0, points[10].x);
        Assert.Equal(10, points[10].y);
    }

    [Fact]
    public void PointLine()
    {
        var ep = new Day5.EndPoint(new Day5.Point(0, 0), new Day5.Point(3, 0));
        Assert.Equal(4, ep.AllPoints(false).Count());
    }

    [Fact]
    public void DiagonalPointLine()
    {
        var ep = new Day5.EndPoint(new Day5.Point(0, 0), new Day5.Point(5, 5));
        var points = ep.AllPoints(true).ToArray();
        Assert.Equal(6, points.Count());

        Assert.Equal(0, points[0].x);
        Assert.Equal(0, points[0].y);

        Assert.Equal(1, points[1].x);
        Assert.Equal(1, points[1].y);

        Assert.Equal(2, points[2].x);
        Assert.Equal(2, points[2].y);

        Assert.Equal(3, points[3].x);
        Assert.Equal(3, points[3].y);

        Assert.Equal(4, points[4].x);
        Assert.Equal(4, points[4].y);

        Assert.Equal(5, points[5].x);
        Assert.Equal(5, points[5].y);
    }

    [Fact]
    public void DiagonalPointRight()
    {
        var ep = new Day5.EndPoint(new Day5.Point(3, 3), new Day5.Point(1, 1));
        var points = ep.AllPoints(true).ToArray();
        Assert.Equal(3, points.Count());

        Assert.Equal(3, points[0].x);
        Assert.Equal(3, points[0].y);

        Assert.Equal(2, points[1].x);
        Assert.Equal(2, points[1].y);

        Assert.Equal(1, points[2].x);
        Assert.Equal(1, points[2].y);
    }

    [Fact]
    public void DiagonalPointRightFlippeDPointOrder()
    {
        var ep = new Day5.EndPoint(new Day5.Point(1, 1), new Day5.Point(3, 3));
        var points = ep.AllPoints(true).ToArray();
        Assert.Equal(3, points.Count());

        Assert.Equal(1, points[0].x);
        Assert.Equal(1, points[0].y);

        Assert.Equal(2, points[1].x);
        Assert.Equal(2, points[1].y);

        Assert.Equal(3, points[2].x);
        Assert.Equal(3, points[2].y);
    }



}




