// See https://aka.ms/new-console-template for more information

//Runs in 00:38:44.3712667 - Answer too high


using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

var machines = new List<Machine>();

var lines = File.ReadAllLines("input.txt");
foreach (var line in lines)
{
    List<int[]> switches = new List<int[]>();
    int[] joltages = [];
    
    var parts =  line.Split(' ');
    foreach (var part in parts)
    {
        var switchesMatch = Reg.Switches().Match(part);
        if (switchesMatch.Success)
        {
            var switchPart =  switchesMatch.Groups[1].Value.Split(',');
            switches.Add(switchPart.Select(int.Parse).ToArray());
        }
        
        var joltageMatch = Reg.Joltage().Match(part);
        if (joltageMatch.Success)
        {
            var joltagePart =  joltageMatch.Groups[1].Value.Split(',');
            joltages = joltagePart.Select(int.Parse).ToArray();
        } 
    }
    machines.Add(new Machine(joltages, Machine.OrderByRarity(switches)));
}

var stopWatch = Stopwatch.StartNew();
long accumulator = 0;
int index = 1;
foreach (var machine in machines)
{
    
    var solution = machine.Solve();                               
    if (solution != null)
    {
        accumulator += solution.Value;
        Console.WriteLine($"Solution for {index++} in {solution}");
    }
    else
    {
        Console.WriteLine($"Could not find a solution for {index++}");
    }
}
Console.WriteLine($"Accumulator: {accumulator}");
Console.WriteLine(stopWatch.Elapsed);

record Machine(int[] Joltage, int[][] Switches)
{
    public int? Solve()
    {
        //Count if this is the last switch that will affect any Joltage index
        int[][] remainingCount = new int[Switches.Length][];
        for (int i = 0; i < Switches.Length; i++)
        {
            List<int> indexes = new List<int>();
            int[] joltageCount = new int[Joltage.Length];
            for (int j = i; j < Switches.Length; j++)
            {
                var switches = Switches[j];
                foreach (var index in switches.Intersect(Switches[i]))
                {
                    joltageCount[index]++;
                }
            }

            for (int j = 0; j < joltageCount.Length; j++)
            {
                if (joltageCount[j] == 1)
                {
                    indexes.Add(j);
                }
            }

            if (indexes.Count > 0)
            {
                remainingCount[i] = indexes.ToArray();
            }
            else
            {
                remainingCount[i] = [];
            }
        }
        
        
        return SolveRecursive(Joltage, 0, remainingCount,0, null);
    }

    public static int[][] OrderByRarity(List<int[]> switches)
    {
        var counts = new Dictionary<int, int>();
        foreach (var @switch in switches)
        {
            foreach(var index in @switch)
            {
                CollectionsMarshal.GetValueRefOrAddDefault(counts, index, out _)++;
            }
        }

        var order = counts.OrderBy(x => x.Value);

        var finalList = new List<int[]>();
        foreach (var keyValuePair in order)
        {
            var joltageIndex = keyValuePair.Key;
            var allSwitchesContainingJIndex = switches.Where(x => x.Any(i => i == joltageIndex)).OrderByDescending(x => x.Length).ToArray();
            foreach (var i in allSwitchesContainingJIndex)
            {
                finalList.Add(i);
                switches.Remove(i);
            }
            
        }
        
        return finalList.ToArray();
    }

    int? SolveRecursive(int[] joltages, int switchIndex, int[][] lastChanceJoltages, int deep, int? toBeat)
    {
        //No point
        if (toBeat.HasValue && deep > toBeat.Value)
        {
            return null;
        }

        if (this.AllZeros(joltages))
        {
            return deep;
        }
        
        if (switchIndex >= Switches.Length)
        {
            return null;
        }

        int maxTimes = 200;
        for (var index = 0; index < Switches[switchIndex].Length; index++)
        {
            var switches = Switches[switchIndex][index];
            maxTimes = Math.Min(maxTimes, joltages[switches]);
        }

        if (toBeat != null)
        {
            maxTimes = Math.Min(maxTimes, toBeat.Value - deep);
        }
        

        int minSkipTimes = 0;
        //if it is, then time must start on it
        for (var index = 0; index < lastChanceJoltages[switchIndex].Length; index++)
        {
            var lastSwitchIndex = lastChanceJoltages[switchIndex][index];
            minSkipTimes = Math.Max(minSkipTimes, joltages[lastSwitchIndex]);
        }

        if (minSkipTimes > maxTimes)
        {
            return null;
        }
        
        for (int time = 0; time <= maxTimes; time++)
        {

            if (time > 0) //time == 0 we don't press this button
            {
                joltages = Subtract(joltages, Switches[switchIndex]);
            }

            //we skip until time is to consider, usually on the last switch to speed it
            if (time >= minSkipTimes)
            {
                var attempt = SolveRecursive(joltages, switchIndex + 1, lastChanceJoltages, deep + time, toBeat);
                if (attempt != null && (toBeat == null || attempt.Value < toBeat.Value))
                {
                    toBeat = attempt;
                }
            }
        }

        return toBeat;
    }
    
    static int[] Subtract(int[] joltages, int[] switches)
    {
        var newJoltages = new int[joltages.Length];
        int indexSwitch = 0;
        for (int i = 0; i < joltages.Length; i++)
        {
            newJoltages[i] = joltages[i];
            if (indexSwitch >= 0 && switches[indexSwitch] == i)
            {
                newJoltages[i]--;
                indexSwitch++;
                if (indexSwitch >= switches.Length)
                {
                    indexSwitch = -1;
                }
            }
        }

        return newJoltages;
    }
    
    bool AllZeros(int[] joltages)
    {
        for (int i = 0; i < joltages.Length; i++)
        {
            if (joltages[i] == 0)
            {
                continue;
            }
            return false;
        }
        return true;
    }
}

static partial class Reg
{

    [GeneratedRegex("\\{([0-9,]+)\\}")]
    public static partial Regex Joltage();

    [GeneratedRegex("\\(([0-9,]+)\\)")]
    public static partial Regex Switches();
}