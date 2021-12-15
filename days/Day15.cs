namespace aoc.Days;

class Day15
{
    public readonly record struct Point(int y, int x);

    public int[,] Data(string path)
    {
        return Lib.LoadMatrix(path);
    }
    public int Part1(int[,] data)
    {
        return ShortestPath(data,
            (0, 0),
            (data.GetUpperBound(0), data.GetUpperBound(1)) 
        );

    }
    public int Part2(int[,] data)
    {
        return 0;

    }
    public IEnumerable<(int, int)> Neighbours(int originY, int originX, int boundsY, int boundsX)
    {
        if (originY - 1 >= 0) yield return (originY - 1, originX);
        if (originY + 1 < boundsY) yield return (originY + 1, originX);

        if (originX - 1 >= 0) yield return (originY, originX - 1);
        if (originX + 1 < boundsX) yield return (originY, originX + 1);
    }

    /*
    def dijkstra_search(graph: WeightedGraph, start: Location, goal: Location):
    frontier = PriorityQueue()
    frontier.put(start, 0)
    came_from: Dict[Location, Optional[Location]] = {}
    cost_so_far: Dict[Location, float] = {}
    came_from[start] = None
    cost_so_far[start] = 0
    
    while not frontier.empty():
        current: Location = frontier.get()
        
        if current == goal:
            break
        
        for next in graph.neighbors(current):
            new_cost = cost_so_far[current] + graph.cost(current, next)
            if next not in cost_so_far or new_cost < cost_so_far[next]:
                cost_so_far[next] = new_cost
                priority = new_cost
                frontier.put(next, priority)
                came_from[next] = current
    
    return came_from, cost_so_far

    */

    int ShortestPath(int[,] grid, (int, int) start, (int, int) goal)
    {
        Dictionary<(int, int), int> costs = new Dictionary<(int, int), int>();
        Dictionary<(int, int), (int, int)> came_from = new Dictionary<(int, int), (int, int)>();
        Stack<((int, int), int)> toProcess = new Stack<((int, int), int)>();
        int min_cost = int.MaxValue;

        toProcess.Push((start, 0));
        costs[start] = 0;

        while (toProcess.Any())
        {
            //Console.WriteLine("CCCCCCCCCCCCCCCCCCCCC");
            var (current, cost) = toProcess.Pop();

            if(cost > min_cost) continue; 

            if (current == goal)
            {
                min_cost = cost; 
            }

            foreach (var new_point in Neighbours(current.Item1, current.Item2, grid.GetLength(0), grid.GetLength(1)))
            {
                var new_cost = cost + grid[current.Item1, current.Item2];
                if (!costs.ContainsKey(new_point) || new_cost < costs[new_point])
                {
                    //Console.WriteLine("{0},{1}-{2}", new_point.Item1, new_point.Item2, cost);

                    costs[new_point] = new_cost;
                    toProcess.Push((new_point, new_cost));
                    came_from[new_point] = current;
                }
            }
        }
        return costs[goal];
    }
}