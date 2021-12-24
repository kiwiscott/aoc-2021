namespace aoc.Days;
using System.Text.RegularExpressions;

public class Day23
{
    public Game Data(string path)
    {
        var chars = new[] { 'A', 'B', 'C', 'D' };
        var empty_game = Enumerable.Repeat(Game.EMPTY, 19).ToArray();

        var f = Lib.LoadFile(path);
        //string are found out of order so reorder them 
        var order = new List<int>() { 11, 13, 15, 17, 12, 14, 16, 18 };
        foreach (var s in f)
        {
            foreach (var c in s.Where(x => chars.Contains(x)))
            {
                empty_game[order[0]] = c;
                order.RemoveAt(0);
            }
        }

        //Start with a blank game 
        return new Game(new string(empty_game), 0, String.Empty);
    }

    public long Part1(Game game)
    {
        return 12;
        long min_cost = long.MaxValue;

        Dictionary<string, long> seen = new Dictionary<string, long>();
        var to_process = new PriorityQueue<Game, long>();

        to_process.Enqueue(game, 0);

        while (to_process.TryDequeue(out var g, out var i))
        {
            var seen_value = seen.GetValueOrDefault(g.State, long.MaxValue);
            if (g.Cost > min_cost || g.Cost > seen_value)
            {
                continue;
            }

            else
            {
                seen.TryAdd(g.State, 0);
                seen[g.State] = g.Cost;

                foreach (var ng in g.PossibleMoves())
                {
                    if (ng.Cost < min_cost && ng.Organized())
                    {
                        Console.WriteLine(ng.Cost.ToString() + " " + ng.History);
                        min_cost = ng.Cost;
                    }
                    else if (ng.Cost < min_cost)
                    {
                        seen_value = seen.GetValueOrDefault(ng.State, long.MaxValue);
                        if (ng.Cost < seen_value)
                        {
                            seen.TryAdd(ng.State, 0);
                            seen[ng.State] = ng.Cost;

                            to_process.Enqueue(ng, ng.Cost);
                        }
                    }
                }
            }
        }

        return min_cost;

    }
    public long Part2(Game game)
    {

        var state = "...........CDDBBCBDDBAAAACC";




        Game4 g4 = new Game4(state, 0, String.Empty);

        Print(g4);

        long min_cost = long.MaxValue;

        Dictionary<string, long> seen = new Dictionary<string, long>();
        var to_process = new PriorityQueue<Game4, long>();

        to_process.Enqueue(g4, 0);

        int count = 0;

        while (to_process.TryDequeue(out var g, out var i))
        {
            count++;
            var seen_value = seen.GetValueOrDefault(g.State, long.MaxValue);
            if (g.Cost > min_cost || g.Cost > seen_value)
            {
                continue;
            }

            else
            {
                seen.TryAdd(g.State, 0);
                seen[g.State] = g.Cost;

                foreach (var ng in g.PossibleMoves())
                {

                    if (ng.Cost < min_cost && ng.Organized())
                    {
                        Console.WriteLine(ng.Cost);

                        min_cost = ng.Cost;
                    }
                    else if (ng.Cost < min_cost)
                    {
                        seen_value = seen.GetValueOrDefault(ng.State, long.MaxValue);
                        if (ng.Cost < seen_value)
                        {
                            //if (ng.History.Contains("A.19->1")
                            //&& ng.State[5] == 'B'
                            //&& ng.State[11] == 'B' && ng.State[22] == 'C' && ng.State[21] == 'C' && ng.State[20] == 'C')
                            //if (ng.State[3] == 'B' && ng.State[11] == 'B')
                            //{
                            //    Print(ng);
                            //    Console.WriteLine(ng.History);
                            //}


                            seen.TryAdd(ng.State, 0);
                            seen[ng.State] = ng.Cost;

                            to_process.Enqueue(ng, ng.Cost);
                        }
                    }
                }
            }
        }
        return min_cost;
    }

    public void Print(Game4 g)
    {
        Console.WriteLine("#############");
        Console.WriteLine("#{0}#", g.State.Substring(0, 11));
        Console.WriteLine("###{0}#{1}#{2}#{3}###", g.State[11], g.State[15], g.State[19], g.State[23]);
        Console.WriteLine("  #{0}#{1}#{2}#{3}#", g.State[12], g.State[16], g.State[20], g.State[24]);
        Console.WriteLine("  #{0}#{1}#{2}#{3}#", g.State[13], g.State[17], g.State[21], g.State[25]);
        Console.WriteLine("  #{0}#{1}#{2}#{3}#", g.State[14], g.State[18], g.State[22], g.State[26]);
        Console.WriteLine("  #########");
        Console.WriteLine("");


    }

    public class Game4
    {
        string _state;
        private long _moves;
        public const string ORGANISED = "...........AAAABBBBCCCCDDDD";
        public string _history;

        public const char EMPTY = '.';

        Dictionary<char, int> cost_to_move;
        public Game4(string state, long moves, string history)
        {
            this._state = state;
            this._moves = moves;
            this.cost_to_move = new Dictionary<char, int>() { { 'A', 1 }, { 'B', 10 }, { 'C', 100 }, { 'D', 1000 } };
            this._history = history;
        }

        public bool Organized()
        {
            return this._state == ORGANISED;
        }
        public long Cost
        {
            get
            {
                return _moves;
            }
        }
        public string State
        {
            get
            {
                return _state;
            }
        }
        public string History
        {
            get
            {
                return _history;
            }
        }

        internal bool RoomSolved(char c)
        {
            switch (c)
            {
                case 'A':
                    return _state[11] == 'A' && _state[12] == 'A' && _state[13] == 'A' && _state[14] == 'A';
                case 'B':
                    return _state[15] == 'B' && _state[16] == 'B' && _state[17] == 'B' && _state[18] == 'B';
                case 'C':
                    return _state[19] == 'C' && _state[20] == 'C' && _state[21] == 'C' && _state[22] == 'C';
                case 'D':
                    return _state[23] == 'D' && _state[24] == 'D' && _state[25] == 'D' && _state[26] == 'D';
                default:
                    return false;
            }
        }

        public bool under_needs_to_change(int from)
        {
            var change = false;

            switch (from)
            {
                case 11:
                    change = !_state[12..14].All(p => p == ORGANISED[from]); break;
                case 12:
                    change = !_state[13..14].All(p => p == ORGANISED[from]); break;
                case 13:
                    change = _state[from] != ORGANISED[from]; break;
                case 14:
                    change = false; break;
                //B
                case 15:
                    change = !_state[16..18].All(p => p == ORGANISED[from]); break;
                case 16:
                    change = !_state[17..18].All(p => p == ORGANISED[from]); break;
                case 17:
                    change = _state[18] != ORGANISED[from]; break;
                case 18:
                    change = false; break;
                //C 
                case 19:
                    change = !_state[20..22].All(p => p == ORGANISED[from]); break;
                case 20:
                    change = !_state[21..22].All(p => p == ORGANISED[from]); break;
                case 21:
                    change = _state[22] != ORGANISED[from]; break;
                case 22:
                    change = false; break;
                //D
                case 23:
                    change = !_state[24..26].All(p => p == ORGANISED[from]); break;
                case 24:
                    change = !_state[25..26].All(p => p == ORGANISED[from]); break;
                case 25:
                    change = _state[25] != ORGANISED[from]; break;
                case 26:
                    change = false; break;
                default:
                    change = false; break;
            }

            //if (from == 17)
            // Console.WriteLine("under_needs_to_change:{0} == {1}", from, change);
            return change;


        }

        public IEnumerable<Game4> PossibleMoves()
        {
            //move from lower in room to top provided we're not in target position and top is empty.
            var movement_in_room = new[] {
                (12,11), (13,12), (14,13), //A 
                (16,15), (17,16), (18,17), //A 
                (20,19), (21,20), (22,21), //A 
                (24,23), (25,24), (26,25), //A 
            };

            foreach (var (from, to) in movement_in_room)
            {
                if (_state[to] != EMPTY || _state[from] == EMPTY)
                    continue;

                // We need to make sure nothing under this 
                if (_state[from] == ORGANISED[from] && !under_needs_to_change(from))
                    continue; // We are were we are supposed to be 

                yield return CreateAndMove(from, to, 1);
            }

            //move from top to lower in room if lower is target position and empty
            //Same list as above but reversed the from,to 
            foreach (var (to, from) in movement_in_room)
            {
                //If its already occupied by the right char leave it
                if (_state[to] == ORGANISED[to] || _state[from] == EMPTY)
                    continue;

                //If this char should be there and its open 
                if (_state[from] == ORGANISED[to] && _state[to] == EMPTY)
                    yield return CreateAndMove(from, to, 1);
            }

            //move from top space in room to any stoppable space in hallway provided the room isn't solved 
            //and there is nothing blocking the move
            var top_of_room = new[] { (11, 2), (15, 4), (19, 6), (23, 8) };
            var hall_stops = new[] { 0, 1, 3, 5, 7, 9, 10 };

            foreach (var (top, entry) in top_of_room)
            {
                if (_state[top] == EMPTY) continue;

                foreach (var stop in hall_stops)
                {
                    //if this item is in the right place and the room is complete leave it 
                    if (_state[top] == ORGANISED[top] && RoomSolved(_state[top]))
                        continue;

                    if (_state[stop] == EMPTY && NothingBetween(entry, stop))
                    {
                        //################ WE NEED TO MAKE SURE NOTHING IS IN THE WAY HERE 
                        var cost = Diff(entry, stop) + 1; //+1 cause we move into hall 
                        yield return CreateAndMove(top, stop, cost);
                    }
                }
            }

            //move from hallway to top space in target room if it's empty and the lower room 
            //is either empty or has correct amphipod and there are no intervening blockers. 
            foreach (var stop in hall_stops)
            {
                if (_state[stop] == EMPTY) continue;

                foreach (var (top, entry) in top_of_room)
                {
                    //Exclude the current char 
                    var exclude_stop_char = stop < entry ? stop + 1 : stop - 1;


                    if (_state[top] == EMPTY && _state[stop] == ORGANISED[top] && NothingBetween(exclude_stop_char, entry))
                    {
                        //The top item in the room we are moving to must be the same number. 
                        if ((_state[top + 1] == EMPTY || _state[top + 1] == ORGANISED[top])
                            && (_state[top + 2] == EMPTY || _state[top + 2] == ORGANISED[top])
                            && (_state[top + 3] == EMPTY || _state[top + 3] == ORGANISED[top]))
                        {
                            //################ WE NEED TO MAKE SURE NOTHING IS IN THE WAY HERE 
                            var cost = Diff(entry, stop) + 1;
                            yield return CreateAndMove(stop, top, cost);
                        }
                    }
                }
            }
        }

        private bool NothingBetween(int from, int to)
        {
            var start = from < to ? from : to;
            var end = from > to ? from : to;
            return _state[start..end].All(a => a == EMPTY);
        }


        private Game4 CreateAndMove(int from, int to, int cost)
        {
            var ch = _state.ToCharArray();
            ch[from] = EMPTY;
            ch[to] = _state[from];
            var newgame = new string(ch);

            var new_cost = cost * cost_to_move[_state[from]];
            var new_history = this._history + String.Format("\t{0}.{1}->{2}({3})\n", _state[from], from, to, new_cost);

            return new Game4(newgame, _moves + new_cost, new_history);

        }
        int Diff(int number1, int number2)
        {
            int result = number1 > number2 ? number1 - number2 : number2 - number1;
            return result;
        }
    }



    public class Game
    {
        string _state;
        private long _moves;
        public const string ORGANISED = "...........AABBCCDD";
        public string _history;

        public const char EMPTY = '.';

        Dictionary<char, int> cost_to_move;
        public Game(string state, long moves, string history)
        {
            this._state = state;
            this._moves = moves;
            this.cost_to_move = new Dictionary<char, int>() { { 'A', 1 }, { 'B', 10 }, { 'C', 100 }, { 'D', 1000 } };
            this._history = history;
        }

        public bool Organized()
        {
            return this._state == ORGANISED;
        }
        public long Cost
        {
            get
            {
                return _moves;
            }
        }
        public string State
        {
            get
            {
                return _state;
            }
        }
        public string History
        {
            get
            {
                return _history;
            }
        }

        internal bool RoomSolved(char c)
        {
            switch (c)
            {
                case 'A':
                    return _state[11] == 'A' && _state[12] == 'A';
                case 'B':
                    return _state[13] == 'B' && _state[14] == 'B';
                case 'C':
                    return _state[15] == 'C' && _state[16] == 'C';
                case 'D':
                    return _state[17] == 'D' && _state[18] == 'D';
                default:
                    return false;
            }
        }

        public IEnumerable<Game> PossibleMoves()
        {
            //move from lower in room to top provided we're not in target position and top is empty.
            var movement_in_room = new[] { (18, 17), (16, 15), (14, 13), (12, 11) };
            foreach (var (from, to) in movement_in_room)
            {
                if (_state[from] == ORGANISED[from] || _state[from] == EMPTY)
                    continue; // We are were we are supposed to be 

                if (_state[to] == EMPTY)
                    yield return CreateAndMove(from, to, 1);
            }
            //move from top to lower in room if lower is target position and empty
            //Same list as above but reversed the from,to 
            foreach (var (to, from) in movement_in_room)
            {
                //If its already occupied by the right char leave it
                if (_state[to] == ORGANISED[to] || _state[from] == EMPTY)
                    continue;

                //If this char should be there and its open 
                if (_state[from] == ORGANISED[to] && _state[to] == EMPTY)
                    yield return CreateAndMove(from, to, 1);
            }

            //move from top space in room to any stoppable space in hallway provided the room isn't solved 
            //and there is nothing blocking the move
            var top_of_room = new[] { (11, 2), (13, 4), (15, 6), (17, 8) };
            var hall_stops = new[] { 0, 1, 3, 5, 7, 9, 10 };

            foreach (var (top, entry) in top_of_room)
            {
                if (_state[top] == EMPTY) continue;

                foreach (var stop in hall_stops)
                {
                    //if this item is in the right place and the room is complete leave it 
                    if (_state[top] == ORGANISED[top] && RoomSolved(_state[top]))
                        continue;

                    if (_state[stop] == EMPTY && NothingBetween(entry, stop))
                    {
                        //################ WE NEED TO MAKE SURE NOTHING IS IN THE WAY HERE 
                        var cost = Diff(entry, stop) + 1; //+1 cause we move into hall 
                        yield return CreateAndMove(top, stop, cost);
                    }
                }
            }

            //move from hallway to top space in target room if it's empty and the lower room 
            //is either empty or has correct amphipod and there are no intervening blockers. 
            foreach (var stop in hall_stops)
            {
                if (_state[stop] == EMPTY) continue;

                foreach (var (top, entry) in top_of_room)
                {
                    //Exclude the current char 
                    var exclude_stop_char = stop < entry ? stop + 1 : stop - 1;


                    if (_state[top] == EMPTY && _state[stop] == ORGANISED[top] && NothingBetween(exclude_stop_char, entry))
                    {
                        if (_state[top + 1] == EMPTY || _state[top + 1] == ORGANISED[top + 1])
                        {

                            //################ WE NEED TO MAKE SURE NOTHING IS IN THE WAY HERE 
                            var cost = Diff(entry, stop) + 1;
                            yield return CreateAndMove(stop, top, cost);
                        }
                    }
                }
            }
        }

        private bool NothingBetween(int from, int to)
        {
            var start = from < to ? from : to;
            var end = from > to ? from : to;
            return _state[start..end].All(a => a == EMPTY);
        }


        private Game CreateAndMove(int from, int to, int cost)
        {
            var ch = _state.ToCharArray();
            ch[from] = EMPTY;
            ch[to] = _state[from];
            var newgame = new string(ch);

            var new_cost = cost * cost_to_move[_state[from]];
            var new_history = this._history + String.Format("\t{0}.{1}->{2}({3})\n", _state[from], from, to, new_cost);

            return new Game(newgame, _moves + new_cost, new_history);

        }
        int Diff(int number1, int number2)
        {
            int result = number1 > number2 ? number1 - number2 : number2 - number1;
            return result;
        }
    }
}