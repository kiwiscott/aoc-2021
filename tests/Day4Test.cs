using Xunit;
using aoc.Days;

public class Day4Test
{
    List<string> TestData()
    {
        return (new string[] {"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1",
            "",
            "22 13 17 11  0",
            "8  2 23  4 24",
            "21  9 14 16  7",
            "6 10  3 18  5",
            "1 12 20 15 19",
            "",
            "3 15  0  2 22",
            "9 18 13 17  5",
            "19  8  7 25 23",
            "20 11 10 24  4",
            "14 21 16 12  6",
            "",
            "14 21 17 24  4",
            "10 16 15  9 19",
            "18  8 23 26 20",
            "22 11 13  6  5",
            "2  0 12  3  7",
            ""}).ToList();
    }
    [Fact]
    public void Test1()
    {
        Day4 d = new Day4(); 
        var data = TestData();

        var bingo = d.Process(data);
        Assert.Equal(3, bingo.Boards.Count());

        var result = d.Part1(bingo);
        Assert.Equal(4512, result);
    }
    [Fact]
    public void TestSecondColumn()
    {
        Day4 d = new Day4(); 
        var data = TestData();

        var bingo = d.Process(data);
        var w = bingo.PlayRound(21);
        Assert.Null(w);

        w = bingo.PlayRound(16);
        Assert.Null(w);

        w = bingo.PlayRound(8);
        Assert.Null(w);

        w = bingo.PlayRound(11);
        Assert.Null(w);

        w = bingo.PlayRound(0);
        Assert.NotNull(w);

    }


    [Fact]
    public void TestLastRow()
    {
        Day4 d = new Day4(); 
        var data = TestData();

        var bingo = d.Process(data);
        var w = bingo.PlayRound(2);
        Assert.Null(w);

        w = bingo.PlayRound(0);
        Assert.Null(w);

        w = bingo.PlayRound(12);
        Assert.Null(w);

        w = bingo.PlayRound(3);
        Assert.Null(w);

        w = bingo.PlayRound(7);
        Assert.NotNull(w);

    }


}




