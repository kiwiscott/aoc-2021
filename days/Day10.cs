namespace aoc.Days;

class Day10
{
    public List<string> Data(string path)
    {
        return Lib.LoadFile(path);
    }
    public int Part1(List<String> data)
    {
        /*
        If a chunk opens with (, it must close with ).
        If a chunk opens with [, it must close with ].
        If a chunk opens with {, it must close with }.
        If a chunk opens with <, it must close with >.
        */



        //Stop at the first incorrect closing character on each corrupted line.
        List<char> corrupted = new List<char>();

        foreach (var line in data)
        {
            var c = CorruptLine(line);
            if (c != ' ')
            {
                corrupted.Add(c);
            }
        }

        var numericLookup = new Dictionary<char, int> { { '>', 25137 }, { ')', 3 }, { ']', 57 }, { '}', 1197 } };

        return corrupted.Select(p => numericLookup[p]).Sum();
    }
    public long Part2(List<string> data)
    {
        var openers = new[] { '(', '[', '{', '<' };
        /*
            ): 1 point.
            ]: 2 points.
            }: 3 points.
            >: 4 points.
        */
        var numericLookup = new Dictionary<char, int> { { '(', 1 }, { '[', 2 }, { '{', 3 }, { '<', 4 } };

        var incomplete = data.RemoveAll(p => CorruptLine(p) != ' ');
        var totals = new List<long>();

        //we know the line is valid so we can just assume the pairs open and close 
        data.ForEach(line =>
        {
            var s = new Stack<char>();

            line.ToList().ForEach(c =>
            {
                if (openers.Contains(c))
                    s.Push(c);
                else
                    s.Pop();

            });

            var unclosed_opener = ' ';
            long current_score = 0L;

            while (s.TryPop(out unclosed_opener))
            {
                //Don't really need to complete the string here
                current_score = (current_score * 5) + numericLookup[unclosed_opener];
            }
            totals.Add(current_score);

        });
        return totals.OrderBy(p => p).ElementAt(totals.Count / 2);

    }

    char CorruptLine(string line)
    {
        var openers = new[] { '(', '[', '{', '<' };
        var pairs = new Dictionary<char, char> { { '>', '<' }, { ')', '(' }, { ']', '[' }, { '}', '{' } };

        var stack = new Stack<char>();
        var unclosed_opener = ' ';

        foreach (var c in line)
        {
            if (openers.Contains(c))
            {
                stack.Push(c);
            }
            else
            {
                if (stack.TryPop(out unclosed_opener) && pairs[c] != unclosed_opener)
                {
                    return c;
                }
            }
        }
        return ' ';
    }
}