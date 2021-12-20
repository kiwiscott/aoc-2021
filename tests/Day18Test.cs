using Xunit;
using aoc.Days;

public class Day18
{


    [Theory]
    [InlineData("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
    [InlineData("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
    [InlineData("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
    [InlineData("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
    public void Reduced(string tree, string expected)
    {
        var t = aoc.Days.Day18.Tree.From(tree);
        t.Reduce();
        Assert.Equal(expected, t.ToString());
    }

    [Fact]
    public void DoubleReduce()
    {
        var f = "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]";
        //var ex = "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]";
        var ex2 = "[[3,[2,[8,0]]],[9,[5,[7,0]]]]";

        var t = aoc.Days.Day18.Tree.From(f);
        t.Reduce();
        Assert.Equal(ex2, t.ToString());

    }
    [Fact]
    public void BasicAddition()
    {
        var f = "[1,2]";
        var f2 = "[[3,4],5]";
        var t = aoc.Days.Day18.Tree.From(f);
        var t2 = aoc.Days.Day18.Tree.From(f2);

        var t_summed = t.Add(t2);

        var expected = String.Format("[{0},{1}]", f, f2);
        Assert.Equal(expected, t_summed.ToString());
    }



    [Theory]
    [InlineData("[1,2]")]
    [InlineData("[[1,2],3]")]
    [InlineData("[9,[8,7]]")]
    [InlineData("[[1,9],[8,5]]")]
    [InlineData("[[[[1,2],[3,4]],[[5,6],[7,8]]],9]")]
    [InlineData("[[[9,[3,8]],[[0,9],6]],[[[3,7],[4,9]],3]]")]
    [InlineData("[[[[1,3],[5,3]],[[1,3],[8,7]]],[[[4,9],[6,9]],[[8,2],[7,3]]]]")]
    public void Validate(string tree)
    {
        var t = aoc.Days.Day18.Tree.From(tree);
        Assert.Equal(tree, t.ToString());

    }


    [Fact]
    public void SimplePair()
    {
        string f = "[1,2]";
        var t = aoc.Days.Day18.Tree.From(f);
        Assert.NotNull(t);
        Assert.Null(t.Value);
        Assert.NotNull(t.Left);
        Assert.Equal(1, t.Left.Value);

        Assert.NotNull(t.Right);
        Assert.Equal(2, t.Right.Value);
        Assert.Equal(f, t.ToString());
    }

    [Fact]
    public void NestedLeft()
    {
        string f = "[[1,2],3]";
        var t = aoc.Days.Day18.Tree.From(f);
        Assert.NotNull(t);
        Assert.Null(t.Value);
        Assert.NotNull(t.Left);
        Assert.NotNull(t.Right);
        Assert.Equal(3, t.Right.Value);
        Assert.Equal(f, t.ToString());
    }
}



