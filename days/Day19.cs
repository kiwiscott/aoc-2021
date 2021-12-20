namespace aoc.Days;

public class Day19
{
#pragma warning disable 8602 
    public Scanner[] Data(string path)
    {
        int scanner_id = 0;
        List<Scanner> scanners = new List<Scanner>();
        var scanner = new Scanner();

        foreach (var s in Lib.LoadFile(path))
        {
            if (s.Contains("scanner"))
            {
                if (scanner_id == 0)
                    scanner = new Scanner(new Coord(0, 0, 0), scanner_id);
                else
                    scanner = new Scanner(null, scanner_id);

                scanner_id++;
            }

            else if (s.Contains(","))
            {
                var indexes = s.Split(',');
                var coord = new Coord(int.Parse(indexes[0]), int.Parse(indexes[1]), int.Parse(indexes[2]));
                scanner.beacons.Add(coord);
            }
            else if (String.IsNullOrEmpty(s))
            {
                scanners.Add(scanner);
            }
        }
        scanners.Add(scanner);
        return scanners.ToArray();
    }



    public long Part1(Scanner[] scanners)
    {
        scanners = RunScanners(scanners);
        var c = scanners.SelectMany(p => p.Beacons()).DistinctBy(p => (p.x, p.y, p.z)).Count();
        return c;
    }

    public long Part2(Scanner[] scanners)
    {
        int max = 0;
        scanners = RunScanners(scanners);


        for (int i = 0; i < scanners.Length - 1; i++)
        {
            var off1 = scanners[i].offset;
            var off2 = scanners[i + 1].offset;
            var manhattan = Math.Abs(off1.x - off2.x) + Math.Abs(off1.y - off2.y) + Math.Abs(off1.z - off2.z);
            max = manhattan > max ? manhattan : max;
        }

        return max;
    }

    public Scanner[] RunScanners(Scanner[] scanners)
    {
        while (scanners.Any(p => p.offset is null))
        {
            for (int i = 0; i < scanners.Length; i++)
            {
                for (int j = 0; j < scanners.Length; j++)
                {
                    if (i == j) continue;

                    var scanner1 = scanners[i];
                    var scanner2 = scanners[j];

                    if (scanner1.offset is null || scanner2.offset is not null) continue;

                    int pointMatches = scanner2.EucMatches(scanner1);
                    if (pointMatches >= 66)
                    {
                        //Console.WriteLine("Match {0}->{1}-> Points:{2}", scanner1.id, scanner2.id, pointMatches);
                        //we know that these match  
                        scanner2.MoveToOffset(scanner1);
                        scanners[j] = scanner2;
                    }
                }
            }
        }
        return scanners;

    }


    public record Coord(int x, int y, int z);
    public record Distance(float distance, Coord from, Coord to);

    public record struct Scanner
    {
        public Coord? offset { get; private set; }
        public int id;
        public List<Coord> beacons;
        int rotation;

        public Scanner(Coord? offset, int id)
        {
            this.offset = offset;
            this.id = id;
            this.beacons = new List<Coord>();
            this.rotation = 0;
        }


        public void Rotate()
        {
            this.rotation += 1;
        }
        public IEnumerable<Coord> Beacons()
        {
            Coord rotate(int rotation, Coord coord)
            {
                var (x, y, z) = coord;

                // rotate coordinate system so that x-axis points in the possible 6 directions
                switch (rotation % 6)
                {
                    case 0: (x, y, z) = (x, y, z); break;
                    case 1: (x, y, z) = (-x, y, -z); break;
                    case 2: (x, y, z) = (y, -x, z); break;
                    case 3: (x, y, z) = (-y, x, z); break;
                    case 4: (x, y, z) = (z, y, -x); break;
                    case 5: (x, y, z) = (-z, y, x); break;
                }

                // rotate around x-axis:
                switch ((rotation / 6) % 4)
                {
                    case 0: (x, y, z) = (x, y, z); break;
                    case 1: (x, y, z) = (x, -z, y); break;
                    case 2: (x, y, z) = (x, -y, -z); break;
                    case 3: (x, y, z) = (x, z, -y); break;
                }

                return new Coord(x, y, z);
            }

            int rot = this.rotation;

            var _offset = this.offset is null ? new Coord(0, 0, 0) : this.offset;

            foreach (var b in this.beacons)
            {
                var _b = rotate(rot, b);
                yield return new Coord(_b.x + _offset.x, _b.y + _offset.y, _b.z + _offset.z);
            }

        }

        public IEnumerable<Distance> BeaconsEculiadianMap()
        {
            var _beacons = this.Beacons();

            var query = _beacons.SelectMany(x => _beacons, (c1, c2) => new { c1, c2 })
               .Where(p => p.c1 != p.c2)
               .Select(pair =>
               {
                   float deltaX = Diff(pair.c1.x, pair.c2.x);
                   float deltaY = Diff(pair.c1.y, pair.c2.y);
                   float deltaZ = Diff(pair.c1.z, pair.c2.z);
                   float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
                   return new Distance(distance, pair.c1, pair.c2);
               });
            return query.DistinctBy(c => c.distance);
        }

        internal int EucMatches(Scanner scanner2)
        {
            var _thiseuc = this.BeaconsEculiadianMap();
            var _that = scanner2.BeaconsEculiadianMap();

            var res = _thiseuc.IntersectBy(

               _that.Select(p => p.distance), (x => x.distance));

            return res.Count();

        }
        static int Diff(int number1, int number2)
        {
            int result = number1 > number2 ? number1 - number2 : number2 - number1;
            return result;
        }

        internal void MoveToOffset(Scanner that)
        {
            var coord = FindFirstEucMatch(that);
            if (coord == null) return;
            this.offset = coord;

            //this.PrintBeacons();
            //that.PrintBeacons();

            //Check for 12 matches 
            var thatbeacons = that.Beacons().ToList();
            var c = this.Beacons().Where(b => thatbeacons.Any(tb => b.x == tb.x && b.y == tb.y && b.z == tb.z)).Count();
        }

        internal Coord? FindFirstEucMatch(Scanner that)
        {
            int ProcessDiff(int thisi, int thati)
            {
                var x = thisi < thati
                           ? Math.Abs(Diff(thisi, thati))
                           : -Math.Abs(Diff(thisi, thati));

                //Console.WriteLine("{0} diff {1} equals{2}", thisi, thati, x);
                return x;
            }
            for (int i = 0; i < 24; i++)
            {
                //Console.WriteLine(i);

                //I need to find the matches in both lists 
                var _thismatches = this.BeaconsEculiadianMap().IntersectBy(that.BeaconsEculiadianMap().Select(p => p.distance), (x => x.distance)).ToList();
                var _thatmatches = that.BeaconsEculiadianMap().IntersectBy(this.BeaconsEculiadianMap().Select(p => p.distance), (x => x.distance)).ToList();

                var matches_for_these_values = 0;
                foreach (var match in _thismatches)
                {
                    var thatmatch = _thatmatches.First(p => p.distance == match.distance);
                    //If all the matches are that same we are good 

                    //Lets check for 10 matches 
                    var x_match = (thatmatch.from.x - match.from.x) == (thatmatch.to.x - match.to.x);
                    var y_match = (thatmatch.from.y - match.from.y) == (thatmatch.to.y - match.to.y);
                    var z_match = (thatmatch.from.z - match.from.z) == (thatmatch.to.z - match.to.z);

                    if (x_match && y_match && z_match)
                    {
                        if (matches_for_these_values < 12)
                        {
                            matches_for_these_values += 1;
                        }
                        else
                        {
                            return new Coord(
                                    ProcessDiff(match.from.x, thatmatch.from.x),
                                    ProcessDiff(match.from.y, thatmatch.from.y),
                                    ProcessDiff(match.from.z, thatmatch.from.z));
                        }
                    }
                }
                this.Rotate();
            }
            return null;
        }
        internal void PrintBeacons()
        {
            Console.WriteLine("----- scanner {0} ------", this.id);

            foreach (var b in this.Beacons())
            {
                Console.WriteLine("[{0},{1},{2}]",
                       b.x, b.y, b.z);
            }
            Console.WriteLine("");

        }
    }
}
