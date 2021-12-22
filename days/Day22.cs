namespace aoc.Days;
using System.Text.RegularExpressions;

public class Day22
{
    public List<Cubiod> Data(string path)
    {
        string pattern = "(?<l>[-0-9]+)..(?<r>[-0-9]+)";

        //on x=-20..26,y=-36..17,z=-47..7
        return Lib.LoadList(path, s =>
        {
            bool on = s.StartsWith("on");
            MatchCollection matches = Regex.Matches(s, pattern);

            var b = (Match m) =>
                       {
                           var l = m.Groups["l"].Value;
                           var r = m.Groups["r"].Value;
                           return new Bound(long.Parse(l), long.Parse(r));
                       };

            return new Cubiod(on, b(matches[0]), b(matches[1]), b(matches[2]));
        });
    }

    public long Part1(List<Cubiod> cuboids)
    {
        var x = new Bound(-50, 50);
        var y = new Bound(-50, 50);
        var z = new Bound(-50, 50);

        HashSet<Cube> on_cubes = new HashSet<Cube>();
        foreach (var cuboid in cuboids)
        {
            foreach (var c in cuboid.CubesWithin(x, y, z))
            {
                if (cuboid.on)
                {
                    on_cubes.Add(c);
                }
                else
                {
                    on_cubes.Remove(c);
                }
            }
        }

        return on_cubes.Count();
    }
    public long Part2(List<Cubiod> cuboids)
    {
        var empty = new Bound(0, 0);
        var start = new Cubiod(true, empty, empty, empty);

        List<Cubiod> cubes = new List<Cubiod>();
        cubes.Add(start);

        foreach (var c in cuboids)
        {
            //
            // The concept is simple. If we have an intersection we need to remove that part from the cube. That creates up to 6 slices of the 
            // original cube. So we can add, as a union, we spliy all other cubes where the cube exists. 
            //
            cubes = cubes.SelectMany(cube => cube.longersectSplit(c)).ToList();
            if (c.on)
            {
                cubes.Add(c);
            }
        }

        return cubes.Sum(p => p.Area());
    }

    public record Cube(long x, long y, long z);
    public record Bound(long min, long max)
    {
        public IEnumerable<long> Between(Bound other)
        {
            if (this.min > other.max || this.min < other.max)
                yield break;

            //Start is either -50 min 
            var start = other.min > this.min ? other.min : this.min;
            var end = other.max < this.max ? other.max : this.max; ;

            for (long i = start; i <= end; i++)
            {
                yield return i;
            }
        }
    }

    public record struct Cubiod(bool on, Bound x, Bound y, Bound z)
    {
        public static Cubiod From(long min_x, long max_x, long min_y, long max_y, long min_z, long max_z)
        {
            return new Cubiod(true, new Bound(min_x, max_x), new Bound(min_y, max_y), new Bound(min_z, max_z));
        }
        public IEnumerable<Cube> CubesWithin(Bound _x, Bound _y, Bound _z)
        {
            var _this_y = this.y;
            var _this_z = this.z;

            var r = from x in x.Between(_x)
                    from y in _this_y.Between(_y)
                    from z in _this_z.Between(_z)
                    select new Cube(x, y, z);

            foreach (var c in r)
            {
                yield return c;
            }
        }
        public IEnumerable<Cubiod> longersectSplit(Cubiod other)
        {
            if ((this.x.min <= other.x.max && this.x.max >= other.x.min)
            && (this.y.min <= other.y.max && this.y.max >= other.y.min)
            && (this.z.min <= other.z.max && this.z.max >= other.z.min))
            {
                // on x
                if (this.x.min < other.x.min)
                {
                    var m = this.x.min;
                    this.x = new Bound(other.x.min, this.x.max);
                    yield return Cubiod.From(m, other.x.min - 1, this.y.min, this.y.max, this.z.min, this.z.max);
                }
                if (this.x.max > other.x.max)
                {
                    var m = this.x.max;
                    this.x = new Bound(this.x.min, other.x.max);
                    yield return Cubiod.From(other.x.max + 1, m, this.y.min, this.y.max, this.z.min, this.z.max);

                }
                // on y
                if (this.y.min < other.y.min)
                {
                    var m = this.y.min;
                    yield return Cubiod.From(this.x.min, this.x.max, m, other.y.min - 1, this.z.min, this.z.max);
                    this.y = new Bound(other.y.min, this.y.max);
                }
                if (this.y.max > other.y.max)
                {
                    var m = this.y.max;
                    this.y = new Bound(this.y.min, other.y.max);
                    yield return Cubiod.From(this.x.min, this.x.max, other.y.max + 1, m, this.z.min, this.z.max);

                }
                // on z
                if (this.z.min < other.z.min)
                {
                    var m = this.z.min;
                    this.z = new Bound(other.z.min, this.z.max);
                    yield return Cubiod.From(this.x.min, this.x.max, this.y.min, this.y.max, m, other.z.min - 1);

                }
                if (this.z.max > other.z.max)
                {
                    var m = this.z.max;
                    this.z = new Bound(this.z.min, other.z.max);
                    yield return Cubiod.From(this.x.min, this.x.max, this.y.min, this.y.max, other.z.max + 1, m);

                }
            }
            else
            {
                yield return this;
            }
        }

        public long Area()
        {
            long result = 1;
            result *= (this.x.max - this.x.min) + 1;
            result *= (this.y.max - this.y.min) + 1;
            result *= (this.z.max - this.z.min) + 1;
            return Math.Abs(result);
        }

    }
}