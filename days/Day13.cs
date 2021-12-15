namespace aoc.Days;

class Day13
{
    public (List<(int, int)>, List<(char, int)>) Data(string path)
    {
        var lines = Lib.LoadFile(path);

        var dots = lines.Where(p => !String.IsNullOrWhiteSpace(p) && !p.StartsWith("fold along"))
            .Select(p =>
            {
                var parts = p.Split(',');
                return (int.Parse(parts.First()), int.Parse(parts.Last()));
            }).ToList();


        var folds = lines.Where(p => p.StartsWith("fold along"))
            .Select(p =>
            {
                var parts = p.Split('=');
                return (parts.First().Last(), int.Parse(parts.Last()));
            }).ToList();

        return (dots, folds);
    }

    public int Part1((List<(int, int)>, List<(char, int)>) dotsandfolds)
    {
        //Only Process the first item 
        var dots = Fold(dotsandfolds.Item1, dotsandfolds.Item2.Take(1));

        return dots.Count;
    }

    public long Part2((List<(int, int)>, List<(char, int)>) dotsandfolds)
    {
        var dots = Fold(dotsandfolds.Item1, dotsandfolds.Item2);
        var max_x = dots.Max(p => p.Item1);
        var max_y = dots.Max(p => p.Item2);

        Console.WriteLine("----------------------------------------");

        for (int y = 0; y <= max_y; y++)
        {
            for (int x = 0; x <= max_x; x++)
            {
                if (dots.Contains((x, y)))
                {
                    Console.Write("X");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.Write("\n");
        }
        Console.WriteLine("-----------------------------------------");


        return dots.Count;
    }

    private List<(int, int)> Fold(List<(int, int)> dots, IEnumerable<(char, int)> folds)
    {
        foreach (var fold in folds)
        {

            dots = dots.Select(d =>
           {
               var fold_line = fold.Item2;

               var (x, y) = d;
               var (_x, _y) = d;

               switch (fold.Item1) //x or y
               {
                   case 'x':
                       x = (x > fold_line) ? fold_line - (x - fold_line) : x;
                       break;
                   case 'y':
                       y = (y > fold_line) ? fold_line - (y - fold_line) : y;

                       break;
                   default:
                       throw new NotImplementedException("Shouldnt be here");
               }
               //Console.WriteLine("({0},{1}) => ({2},{3})", _x, _y, x, y);
               return (x, y);
           }).Distinct().ToList();
        }
        return dots;
    }

}