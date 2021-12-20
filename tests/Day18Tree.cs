using Xunit;
using aoc.Days;

public class Day18Tree
{
    [Fact]
    public void ComplexExample()
    {
        string f = "[[[[4,3],4],4],[7,[[8,4],9]]]";
        var f2 = "[1,1]";

        var t = aoc.Days.Day18.Tree.From(f);
        var t2 = aoc.Days.Day18.Tree.From(f2);

        var t_summed = t.Add(t2);
        Assert.Equal("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", t_summed.ToString());

    }


    [Theory]
    [InlineData("[[1,2],[[3,4],5]]", 143)]
    [InlineData("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384)]
    [InlineData("[[[[1,1],[2,2]],[3,3]],[4,4]]", 445)]
    [InlineData("[[[[3,0],[5,3]],[4,4]],[5,5]]", 791)]
    [InlineData("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137)]
    [InlineData("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488)]

    public void Magnitude(string from, int magnitude)
    {
        var t = aoc.Days.Day18.Tree.From(from);

        Assert.Equal(magnitude, t.Magnitude());


    }



    [Fact]
    public void FullExample()
    {

        var f = new List<string>{ "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]",
                                    "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]",
                                    "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]",
                                    "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]",
                                    "[7,[5,[[3,8],[1,4]]]]",
                                    "[[2,[2,2]],[8,[8,1]]]",
                                    "[2,9]",
                                    "[1,[[[9,3],9],[[9,0],[0,7]]]]",
                                    "[[[5,[7,4]],7],1]",
                                    "[[[[4,2],2],6],[8,7]]"};

        var expected = "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]";
        var t = aoc.Days.Day18.Tree.From(f.First());
        foreach (var s in f.Skip(1))
        {
            var tn = aoc.Days.Day18.Tree.From(s);

            t = t.Add(tn);
        }
        Assert.Equal(expected, t.ToString());
    }

    [Fact]
    public void SimpleIndex()
    {
        string f = "[1,2]";
        var t = aoc.Days.Day18.Tree.From(f);

        Assert.Equal(1, t.Left.Index);
        Assert.Equal(2, t.Index);
        Assert.Equal(3, t.Right.Index);
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



