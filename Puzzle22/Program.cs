// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

var lines = File.ReadAllLines("input.txt");

var nodes = new Dictionary<string, List<string>>();

foreach (var line in lines)
{
    var parts = line.Split(' ');
    if (parts.Length > 1)
    {
        var nodeName = parts[0].TrimEnd(':');
        var outputs = parts.Skip(1).ToList();
        nodes.Add(nodeName, outputs);
    }
}

var stopWatch = Stopwatch.StartNew();
long pathsToFft = FindOut("svr", "fft", new Dictionary<string, int>());
long pathsToDac = FindOut("fft", "dac", new Dictionary<string, int>());
long pathsToOut = FindOut("dac", "out", new Dictionary<string, int>());

Console.WriteLine($"Found {pathsToFft * pathsToDac * pathsToOut} paths to Out");
Console.WriteLine($"{stopWatch.ElapsedMilliseconds} ms");

int FindOut(string node, string destination, Dictionary<string, int> memoized)
{
    if (node == destination)
    {
        return 1;
    }

    if (node == "out")
    {
        return 0;
    }

    if (memoized.TryGetValue(node, out var value))
    {
        return value;
    }
    
    int acc = 0;
    var nodeOutputs = nodes[node];
    foreach (var nodeOutput in nodeOutputs)
    {
        acc += FindOut(nodeOutput, destination, memoized);
    }
    
    memoized.Add(node, acc);
    
    return acc;
}