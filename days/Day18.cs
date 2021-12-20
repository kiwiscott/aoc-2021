namespace aoc.Days;
public class Day18
{

    public List<Tree> Data(string path)
    {
        return Lib.LoadList(path, s => Tree.From(s));
    }

    public long Part1(List<Tree> homework)
    {

        var t = homework.First();
        foreach (var to_add in homework.Skip(1))
        {
            t = t.Add(to_add);
        }

        return t.Magnitude();
    }

    public long Part2(List<Tree> homework)
    {
        var query = from x in homework
                    from y in homework
                    where x.ToString() != y.ToString()
                    select (x.ToString(), y.ToString());

        int max = query.Select(p =>
        {
            var t = Tree.From(p.Item1);
            var t2 = Tree.From(p.Item2);
            return t2.Add(t).Magnitude();

        }).Max();

        return max;
    }


    public class Tree
    {
        public int Index = 0;
        public Tree? Left { get; private set; }
        Tree? Parent { get; set; }

        public Tree? Right { get; private set; }
        public int? Value { get; private set; }

        private Tree(int? value, Tree? parent, Tree? left, Tree? right)
        {
            this.Parent = parent;
            this.Left = left;
            this.Right = right;
            this.Value = value;

            if (this.Right is not null)
                this.Right.Parent = this;
            if (this.Left is not null)
                this.Left.Parent = this;
        }
        public static Tree From(string from)
        {
            var root = From(null, from);
            root.Reindex();
            return root;
        }

        private void Reindex()
        {
            int index = 0;
            foreach (var n in this.Root.IndexTree())
            {
                index++;
                n.Index = index;
            }

        }

        private IEnumerable<Tree> IndexTree()
        {
            if (this.Left != null)
            {
                foreach (var n in this.Left.IndexTree())
                {
                    yield return n;
                }

            }

            yield return this;

            if (this.Right != null)
            {
                foreach (var n in this.Right.IndexTree())
                {
                    yield return n;
                }
            }


        }

        private static Tree From(Tree? parent, string from)
        {
            int value = 0;
            if (int.TryParse(from, out value))
            {
                return new Tree(value, parent, null, null);
            }

            from = from.Remove(0, 1);
            from = from.Remove(from.Length - 1);

            //find index to split at
            int depth = 0;
            int index = 0;
            for (int i = 0; i < from.Length; i++)
            {
                if (from[i] == ',' && depth == 0)
                {
                    index = i;
                    break;
                }
                else if (from[i] == '[')
                {
                    depth += 1;
                }
                else if (from[i] == ']')
                {
                    depth += -1;
                }
            }

            var ls = from.Substring(0, index);
            var rs = from.Substring(index + 1);
            var l = Tree.From(parent, ls);
            var r = Tree.From(parent, rs);

            return new Tree(null, parent, l, r);
        }
        public override string ToString()
        {
            if (this.Value.HasValue)
                return this.Value.ToString();
            return String.Format("[{0},{1}]", this.Left, this.Right);
        }

        internal Tree Add(Tree t2)
        {
            var _t = new Tree(null, null, this, t2);
            _t.Reindex();
            _t.Reduce();
            return _t;
        }
        public void CheckEmpty()
        {
            if (this.Left != null)
            {
                this.Left.CheckEmpty();
                if (this.Left.IsEmpty())
                    this.Left = null;
            }
            if (this.Right != null)
            {
                this.Right.CheckEmpty();
                if (this.Right.IsEmpty())
                    this.Right = null;
            }

            if (this.Left is not null && this.Right is null)
            {
                this.Right = new Tree(0, this, null, null);
            }
            if (this.Right is not null && this.Left is null)
            {
                this.Left = new Tree(0, this, null, null);
            }


        }
        public bool IsEmpty()
        {
            return this.Left is null
                && this.Right is null
                && !this.Value.HasValue;
        }

        public Tree Root
        {
            get
            {
                var r = this;
                while (r.Parent is not null)
                {
                    r = r.Parent;
                }
                return r;
            }

        }


        public bool Explode(TreeSide side, int toset, int sourceIndex)
        {
            Func<int, bool> test = (side == TreeSide.Left)
                        ? (index) => { return index < sourceIndex; }
            : (index) => { return index > sourceIndex; };

            //Check Left or Right if the indexs are okay 
            if (side == TreeSide.Left)
            {
                var lefts = this.Root.IndexTree().Where(n => n.Index < sourceIndex && n.Value.HasValue).OrderBy(n => n.Index);
                if (lefts.Any())
                {
                    lefts.Last().Value += toset;
                    return true;
                }
            }
            else if (side == TreeSide.Right)
            {
                var rights = this.Root.IndexTree().Where(n => n.Index > sourceIndex && n.Value.HasValue).OrderBy(n => n.Index);
                if (rights.Any())
                {
                    rights.First().Value += toset;
                    return true;
                }
            }
            return false;
        }

        public void Reduce()
        {
            var r = this.Root;
            var run = true;
            while (run)
            {
                run = r.Reduce(0);
                if (!run)
                {
                    run = r.Split();
                }
                r.CheckEmpty();
                r.Reindex();
            }
        }
        private bool Split()
        {
            var split = this.Root.IndexTree().Where(n => n.Value.HasValue && n.Value >= 10).OrderBy(p => p.Index);
            if (split.Any())
            {

                var to_split = split.First();

                int val_to_split = to_split.Value.GetValueOrDefault();
                to_split.Value = null;
                int vl = (int)Math.Floor(val_to_split / 2.0);
                int vr = (int)Math.Ceiling(val_to_split / 2.0);

                to_split.Left = new Tree(vl, to_split, null, null);
                to_split.Right = new Tree(vr, to_split, null, null);
                return true;
            }
            return false;
        }

        private bool Reduce(int depth)
        {
            if (depth == 4 && this.Left is not null && this.Right is not null && this.Left.Value.HasValue && this.Right.Value.HasValue)
            {
                //Console.WriteLine("EXPLODE MY CHILREN : Reduce called {0} {1}", depth, this.ToString());


                if (!this.Explode(TreeSide.Left, this.Left.Value.Value, this.Left.Index))
                    this.Value = 0;

                if (!this.Explode(TreeSide.Right, this.Right.Value.Value, this.Right.Index))
                    this.Value = 0;

                this.Left.Value = null;
                this.Right.Value = null;
                return true;

            }

            if (this.Left != null)
            {
                var t = this.Left.Reduce(depth + 1);
                if (t)
                    return true;

            }
            if (this.Right != null)
            {
                var t = this.Right.Reduce(depth + 1);
                if (t)
                    return true;
            }
            return false;
        }

        public int Magnitude()
        {
            /*
        To check whether it's the right answer, the snailfish teacher only checks the magnitude of the final sum.
        The magnitude of a pair is 3 times the magnitude of its left element plus 2 times the magnitude of its right element. 
        The magnitude of a regular number is just that number.
        For example, the magnitude of [9,1] is 3*9 + 2*1 = 29; the magnitude of [1,9] is 3*1 + 2*9 = 21. 
        Magnitude calculations are recursive: the magnitude of [[9,1],[1,9]] is 3*29 + 2*21 = 129.
        */
            if (this.Value.HasValue)
                return this.Value.GetValueOrDefault();

            return (this.Left.Magnitude() * 3) + (this.Right.Magnitude() * 2);
        }
    }
}
public enum TreeSide
{
    Left, Right
}
/*
To reduce a snailfish number, you must repeatedly do the first action in this list that applies to the snailfish number:

If any pair is nested inside four pairs, the leftmost such pair explodes.
If any regular number is 10 or greater, the leftmost such regular number splits.*/