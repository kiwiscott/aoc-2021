using System.Collections;
namespace aoc.Days;

class Day4
{
    public Bingo Data(string path)
    {
        var data = Lib.LoadFile(path);
        return Process(data);
    }
    public Bingo Process(List<string> lines)
    {
        Bingo b = new Bingo();
        b.NumbersToPlay = lines.First().Split(",").Select(p => int.Parse(p)).ToList();

        // we skip the first two rows and then skip the 6th as well
        List<int[]> xxxx = new List<int[]>();

        foreach (var row in lines.Skip(1))
        {
            if (String.IsNullOrEmpty(row) && xxxx.Count() == 5)
            {
                //Silly
                int[,] numbers = new int[5, 5];

                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        numbers[j, k] = xxxx[j][k];
                    }
                }

                Board board = new Board(numbers);
                b.Boards.Add(board);
                xxxx.Clear();
            }
            else if (!String.IsNullOrEmpty(row))
            {
                xxxx.Add(row.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray());
            }
        }

        if (xxxx.Count() == 5)
        {
            //Silly
            int[,] numbers = new int[5, 5];

            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    numbers[j, k] = xxxx[j][k];
                }
            }

            Board board = new Board(numbers);
            b.Boards.Add(board);
            xxxx.Clear();
        }

        return b;
    }
    public int Part1(Bingo data)
    {
        var b = data.PlayTillWinner();

        if (b != null)
        {
            return b.Unmarked().Sum() * data.LastNumber;
        }

        return 0;
    }
    public int Part2(Bingo data)
    {
        var b = data.PlayTillWinner();
        while (data.ActiveBoards().Count() > 0)
        {
            b = data.PlayTillWinner();
        }

        if (b != null)
        {
            return b.Unmarked().Sum() * data.LastNumber;
        }

        return 0;
    }

    public class Bingo
    {
        public List<int> NumbersToPlay = new List<int>();
        public List<Board> Boards = new List<Board>();
        public int LastNumber = -1;

        public Board? PlayRound(int number)
        {
            LastNumber = number;
            foreach (var b in this.Boards)
            {
                if (b.Mark(number) && b.Winner())
                {
                    return b;
                }
            }
            return null;
        }
        public Board? PlayTillWinner()
        {
            foreach (var n in NumbersToPlay)
            {
                var b = PlayRound(n);
                if (b != null)
                {
                    return b;
                }

            }
            return null;
        }

        public IEnumerable<Board> ActiveBoards()
        {
            return this.Boards.Where(p => !p.Winner());
        }
    }
    public class Board
    {
        public Board(int[,] numbers)
        {
            Numbers = numbers;
        }

        public int[,] Numbers { get; set; }

        public bool Winner()
        {
            //Check Rows and Columns
            for (int i = 0; i < 5; i++)
            {
                //Row
                if (Enumerable.Range(0, 5).All(x => this.Numbers[i, x] == -1))
                    return true;

                //Column
                if (Enumerable.Range(0, 5).All(x => this.Numbers[x, i] == -1))
                    return true;
            }

            return false;
        }

        public IEnumerable<int> Unmarked()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (this.Numbers[i, j] > 0)
                    {
                        yield return this.Numbers[i, j];
                    }
                }
            }
        }

        public bool Mark(int number)
        {
            bool marked = false;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (this.Numbers[i, j] == number)
                    {
                        marked = true;
                        this.Numbers[i, j] = -1;
                    }
                }
            }
            return marked;
        }
    }
}
