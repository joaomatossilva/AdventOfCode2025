// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var machines = new List<Machine>();

var lines = File.ReadAllLines("input.txt");
foreach (var line in lines)
{
    string lights = String.Empty;
    List<int[]> switches = new List<int[]>();
    
    var parts =  line.Split(' ');
    foreach (var part in parts)
    {
        var lightsMatch = Reg.Lights().Match(part);
        if (lightsMatch.Success)
        {
            lights = lightsMatch.Groups[1].Value;
        }
        
        var switchesMatch = Reg.Switches().Match(part);
        if (switchesMatch.Success)
        {
            var switchPart =  switchesMatch.Groups[1].Value.Split(',');
            switches.Add(switchPart.Select(int.Parse).ToArray());
        }
    }
    machines.Add(new Machine(lights, switches.ToArray()));
}

int accumulator = 0;
foreach (var machine in machines)
{
    var solution = machine.Solve();
    if (solution != null)
    {
        accumulator += solution.Value;
        Console.WriteLine($"Solution for {machine.Lights} in {solution}");
    }
    else
    {
        Console.WriteLine($"Could not find a solution for {machine.Lights}");
    }
}
Console.WriteLine($"Accumulator: {accumulator}");

record Machine(string Lights, int[][] Switches)
{
    public int? Solve()
    {
        var empty = new string('.', Lights.Length);
        var previousStates = new Stack<string>();
        previousStates.Push(empty);
        return SolveRecursive(empty, previousStates, null);
    }

    int? SolveRecursive(string lights, Stack<string> previousStates, int? toBeat)
    {
        //No point
        if (toBeat == 0)
        {
            return null;
        }
        
        int? best = toBeat;
        foreach (var switches in Switches)
        {
            var newStatus = Apply(lights, switches);

            //if we reach an already processed state, it's not a solution path
            if (previousStates.Contains(newStatus))
            {
                continue;
            }

            // //if we're back at square one it's not a solution path
            // if (newStatus.All(x => x == '.'))
            // {
            //     return null;
            // }
            
            if (newStatus == Lights)
            {
                return 1;
            }

            if (!best.HasValue || best.Value > 1)
            {
                previousStates.Push(newStatus);

                var innerSolution = SolveRecursive(newStatus, previousStates, best - 1);
                if (innerSolution != null && (best == null || innerSolution < best.Value))
                {
                    best = innerSolution.Value;
                }

                previousStates.Pop();
            }

            // //It's impossible to find a better solution so we're golden already
            // if (best == 1)
            // {
            //     break;
            // }
        }
        
        return best + 1;
    }
    
    static string Apply(string lights, int[] switches)
    {
        Span<char> newStatus = stackalloc char[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            if (!switches.Contains(i))
            {
                newStatus[i] = lights[i];
                continue;
            }

            if (lights[i] == '#')
            {
                newStatus[i] = '.';
            }
            else
            {
                newStatus[i] = '#';
            }
        }
        
        return new string(newStatus);
    }
}

static partial class Reg
{

    [GeneratedRegex("\\[([\\.#]+)\\]")]
    public static partial Regex Lights();

    [GeneratedRegex("\\(([0-9,]+)\\)")]
    public static partial Regex Switches();
}