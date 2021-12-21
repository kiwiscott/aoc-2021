namespace aoc.Days;

public class Day21
{
    public (int player1, int player2) Data(string path)
    {
        var f = Lib.LoadFile(path);
        return
        (
            int.Parse(f.First().Split(':', StringSplitOptions.TrimEntries).Last()),
            int.Parse(f.Last().Split(':', StringSplitOptions.TrimEntries).Last())
        );
    }



    public long Part1((int player1, int player2) data)
    {
        var res = Play(data.player1, 0, data.player2, 0, 1000);
        return res.rolls * (res.player1Score > res.player2score ? res.player2score : res.player1Score);
    }

    public long Part2((int player1, int player2) data)
    {
        var res = Play2(data.player1, 0, data.player2, 0, true, 21);
        return (res.p1 > res.p2 ? res.p1 : res.p2);
    }
    enum Game { Player1, Player2 }


    static Dictionary<string, (long p1, long p2)> cache = new Dictionary<string, (long p1, long p2)>();

    Dictionary<int, int> DiceValueAndFrequencies()
    {
        var d = new Dictionary<int, int>();
        var possibilities = from x in Enumerable.Range(1, 3)
                            from y in Enumerable.Range(1, 3)
                            from z in Enumerable.Range(1, 3)
                            select x + y + z;
        foreach (var p in possibilities)
        {
            if (!d.ContainsKey(p)) { d[p] = 0; }
            d[p]++;
        }

        return d;
    }

    private (long p1, long p2) Play2(int p1pos, int p1score, int p2pos, int p2score, bool player1, int play_until)
    {
        var key = String.Format("{0}_{1}_{2}_{3}_{4}", p1pos, p1score, p2pos, p2score, player1 ? "1" : "2");
        if (cache.ContainsKey(key)) return cache[key];


        if (p1score >= play_until) return new(1, 0);
        if (p2score >= play_until) return new(0, 1);

        (long p1, long p2) wins = new(0, 0);

        foreach (var dv in DiceValueAndFrequencies())
        {
            var p1p = player1 ? (p1pos + dv.Key) % 10 : p1pos;
            var p1s = player1 ? p1score + (p1p == 0 ? 10 : p1p) : p1score;

            var p2p = !player1 ? (p2pos + dv.Key) % 10 : p2pos;
            var p2s = !player1 ? p2score + (p2p == 0 ? 10 : p2p) : p2score;

            var r = Play2(p1p, p1s, p2p, p2s, player1 ? false : true, play_until);
            wins.p1 += r.p1 * dv.Value;
            wins.p2 += r.p2 * dv.Value;

        }
        cache.Add(key, wins);
        return wins;
    }

    private (int rolls, int player1Score, int player2score) Play(int p1pos, int p1score, int p2pos, int p2Score, int playUntil)
    {
        var dice = Enumerable.Range(1, int.MaxValue).Chunk(3).GetEnumerator();

        (int score, int pos, int turns) player1 = new(p1score, p1pos, 0);
        (int score, int pos, int turns) player2 = new(p2Score, p2pos, 0);
        int rolls = 0;
        var whose_turn = Game.Player1;

        while (player1.score < playUntil && player2.score < playUntil)
        {
            dice.MoveNext();
            var di = dice.Current.ToArray();
            var tomove = (di.Sum() % 10);

            rolls += 3;

            if (whose_turn == Game.Player1)
            {
                var p = player1.pos;
                var s = player1.score;

                player1.pos = (player1.pos + tomove) % 10;
                player1.score += player1.pos == 0 ? 10 : player1.pos;
                player1.turns++;
                whose_turn = Game.Player2;
            }
            else
            {
                var p = player2.pos;
                var s = player2.score;
                player2.pos = (player2.pos + tomove) % 10;
                player2.score += player2.pos == 0 ? 10 : player2.pos;
                player2.turns++;
                whose_turn = Game.Player1;
            }
        }

        return (rolls, player1.score, player2.score);
    }


}


