// See https://aka.ms/new-console-template for more information

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

var startNode = "you";

var paths = FindOut(startNode);

Console.WriteLine($"Found {paths} paths to Out");

int FindOut(string node)
{
    if (node == "out")
    {
        return 1;
    }

    int acc = 0;
    var nodeOutputs = nodes[node];
    foreach (var nodeOutput in nodeOutputs)
    {
        acc += FindOut(nodeOutput);
    }

    return acc;
}