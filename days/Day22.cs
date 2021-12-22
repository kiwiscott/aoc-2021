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
                           return new Bound(int.Parse(l), int.Parse(r));
                       };

            return new Cubiod(on, b(matches[0]), b(matches[1]), b(matches[2]));
        });
    }

    public long Part2(List<Cubiod> cuboids)
    {

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
    public long Part2b(List<Cubiod> cuboids)
    {
        var start = new Cubiod(true, Bound.Empty(), Bound.Empty(), Bound.Empty());

        List<Cubiod> cubes = new List<Cubiod>();
        cubes.Add(start);

        foreach (var c in cuboids)
        {
            //If there is no intersection do nothing 
            cubes = cubes.SelectMany(cube => cube.Except(c)).ToList();
            if (c.on)
            {
                cubes.Add(c);
            }
        }
        HashSet<Cube> on_cubes = new HashSet<Cube>();
        foreach (var c in cuboids)
        {
            foreach (var cell in c.All())
            {
                on_cubes.Add(cell);
            }
        }


        //1227345351869476
        //81783712262379468
        return on_cubes.Count();

    }

    public record Cube(int x, int y, int z);
    public record Bound(int lower, int upper)
    {
        public IEnumerable<int> Between(Bound other)
        {
            if (this.lower > other.upper || this.upper < other.lower)
                yield break;

            //Start is either -50 min 
            var start = other.lower > this.lower ? other.lower : this.lower;
            var end = other.upper < this.upper ? other.upper : this.upper; ;

            for (int i = start; i <= end; i++)
            {
                yield return i;
            }
        }

        public IEnumerable<int> All()
        {
            for (int i = lower; i <= upper; i++)
            {
                yield return i;
            }
        }

        internal static Bound Empty()
        {
            return new Bound(0, 0);
        }

        internal long Diff()
        {
            return Math.Abs(upper - lower) + 1;
        }

        internal bool Intersect(Bound other)
        {
            if (this.lower > other.upper || this.upper < other.lower)
                return false;

            return true;
        }
        internal bool Within(Bound other)
        {
            return other.lower >= this.lower && other.upper <= this.upper;
        }
    }

    public record Cubiod(bool on, Bound x, Bound y, Bound z)
    {
        internal long Volume()
        {
            return x.Diff() * y.Diff() * z.Diff();
        }
        public IEnumerable<Cube> CubesWithin(Bound _x, Bound _y, Bound _z)
        {
            var r = from x in this.x.Between(_x)
                    from y in this.y.Between(_y)
                    from z in this.z.Between(_z)
                    select new Cube(x, y, z);

            foreach (var c in r)
            {
                yield return c;
            }
        }
        public IEnumerable<Cube> All()
        {
            var r = from x in this.x.All()
                    from y in this.y.All()
                    from z in this.z.All()
                    select new Cube(x, y, z);

            foreach (var c in r)
            {
                yield return c;
            }
        }
        public IEnumerable<Cubiod> Except(Cubiod except)
        {
            if (except.Within(this))
            {
                yield break;
            }
            if (!this.Intersect(except))
            {
                yield return this;
                yield break;
            }

            //X 
            if (this.x.lower < except.x.lower)
                yield return new Cubiod(true, new Bound(this.x.lower, except.x.lower - 1), y, z);

            if (this.x.upper > except.x.upper)
                yield return new Cubiod(true, new Bound(except.x.upper + 1, this.x.upper), y, z);
            //Y
            if (this.y.lower < except.y.lower)
                //Only include x that mathes the 
                yield return new Cubiod(true, except.x, new Bound(this.y.lower, except.y.lower - 1), z);

            if (this.y.upper > except.y.upper)
                yield return new Cubiod(true, except.x, new Bound(except.y.upper + 1, this.y.upper), z);
            //Y
            if (this.z.lower < except.z.lower)
                yield return new Cubiod(true, except.x, except.y, new Bound(this.z.lower, except.z.lower - 1));
            if (this.z.upper > except.z.upper)
                yield return new Cubiod(true, except.x, except.y, new Bound(except.z.upper + 1, this.z.upper));
        }
        public IEnumerable<Cubiod> Union(Cubiod union)
        {
            if (this.Within(union))
            {
                yield return this;
            }
            else if (union.Within(this))
            {
                yield return union;
            }
            else if (!Intersect(union))
            {
                yield return this;
                yield return union;
            }
            else
            {
                yield return this;
                foreach (var s in union.Except(this))
                    yield return s;
                //These intersect and we need to remove the intersections 
                //We need to return all of 1 and only the bit of 2 that isn't in 1 
                //so its 2 except 1 
            }
        }

        public bool Intersect(Cubiod compare)
        {
            //x
            return this.x.Intersect(compare.x) &&
             this.y.Intersect(compare.y) &&
             this.z.Intersect(compare.z);
        }
        public bool Within(Cubiod compare)
        {
            //x
            return this.x.Within(compare.x) &&
             this.y.Within(compare.y) &&
             this.z.Within(compare.z);
        }
    }
}


