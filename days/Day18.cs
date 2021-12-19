namespace aoc.Days;
public class Day18
{

    public List<string> Data(string path)
    {
        return Lib.LoadFile(path);
    }

    public long Part1(List<string> homework)
    {
        var f = "[[6,[5,[4,[3,2]]]],1]"; 


        var t = aoc.Days.Day18.Tree.From(null, f);
        t.Reduce(); 
        Console.WriteLine(t.ToString());
        return 99;
    }

    public long Part2(List<string> homework)
    {
        return -9;
    }

    public class Tree
    {
        public int index = 0; 
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
        public static Tree From(Tree? parent, string from)
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
            _t.Reduce(0);
            _t.Root.CheckEmpty(); 
             
            return _t;
        }
        public void CheckEmpty()
        {
            Console.WriteLine(this.ToString());
            Console.WriteLine("LEFT {0}", this.Left == null ? "-" : this.Left.IsEmpty() );
            Console.WriteLine("RIGH {0}", this.Right == null ? "-" : this.Right.IsEmpty() );

            if(this.Left != null)
            {
                this.Left.CheckEmpty(); 
                if (this.Left.IsEmpty())
                    this.Left = null; 
            }
             if(this.Right != null)
            {
                this.Right.CheckEmpty(); 
                if (this.Right.IsEmpty())
                    this.Right = null; 
            }

            if(this.Left is not null&& this.Right is null )
            {
                this.Right = new Tree(0,this,null,null);
            }
            if(this.Right is not  null && this.Left is null )
            {
                this.Left = new Tree(0,this,null,null);
            }


        }
        public bool IsEmpty(){
            return this.Left is null 
                && this.Right is null
                && !this.Value.HasValue;
        }

        public Tree Root
        {
            get{
                var r = this; 
                while(r.Parent is not null)
                {
                    r = r.Parent; 
                }
                return r; 
            }

        }


        public bool Explode(TreeSide side, int toset, int level)
        {
            if (side == TreeSide.Left && this.Left is not null && this.Left.Value.HasValue)
            {
                this.Left.Value = this.Left.Value + toset;
                return true;
            }
            if (side == TreeSide.Left 
                && this.Right is not null 
                && this.Left.Value.HasValue)
            {
                this.Left.Value = this.Left.Value + toset;
                return true;
            }


            if (side == TreeSide.Right && this.Right is not null && this.Right.Value.HasValue)
            {
                this.Right.Value = this.Right.Value + toset;
                return true;
            }
            if (this.Parent != null)
            {
                return this.Parent.Explode(side, toset,level+1);
            }
            return false;
        }

        public void Reduce()
        {
            var r = this.Root;
            r.Reduce(0); 
            r.CheckEmpty(); 

        }

        private void Reduce(int depth)
        {
            if (depth == 4
             && this.Left is not null
             && this.Right is not null
                && this.Left.Value.HasValue
                && this.Right.Value.HasValue)
            {
                int tosetl = this.Left.Value.GetValueOrDefault();
                int tosetr = this.Right.Value.GetValueOrDefault();

                this.Left.Value = null;
                this.Right.Value = null;


                if (!this.Explode(TreeSide.Left, tosetl,0))
                    this.Value = 0;

                if (! this.Explode(TreeSide.Right,tosetr,0))
                    this.Value = 0; 

                Console.WriteLine("EXPLODE MY CHILREN : Reduce called {0} {1}", depth, this.ToString());

            }

            if (this.Left != null)
            {
                this.Left.Reduce(depth + 1);

            }
            if (this.Right != null)
            {
                this.Right.Reduce(depth + 1);
            }
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