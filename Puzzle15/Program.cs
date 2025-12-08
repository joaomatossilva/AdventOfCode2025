// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var smallest = new PriorityQueue<Pair, double>();
var nodes = new Dictionary<JuntionBox, List<JuntionBox>>();

for (int i = 0; i < lines.Length; i++)
{
    var line = lines[i];
    var match = Reg.GetCoords().Match(line);
    if(!match.Success)
        continue;
    
    nodes.Add(new JuntionBox(int.Parse(match.Groups[1].Value),  int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value)), new List<JuntionBox>());
}

var keys = nodes.Keys.ToArray();
for (int i = 0; i < keys.Length; i++)
{
    for (int j = i + 1; j < keys.Length; j++)
    {
        var distance = keys[i].DistanceTo(keys[j]);
        smallest.Enqueue(new Pair(keys[i], keys[j], distance), Int32.MaxValue - distance);
        if(smallest.Count > 1000)
        {
            smallest.Dequeue();
        }
    }
}

var smallPairs = smallest.UnorderedItems.OrderBy(x => x.Element.Distance).Select(x => x.Element).Take(1000);

foreach (var pair in smallPairs)
{
    nodes[pair.A].Add(pair.B);
    nodes[pair.B].Add(pair.A);
}

HashSet<JuntionBox> seen = new();
var circuits = new List<HashSet<JuntionBox>>();

foreach (var node in nodes)
{
    if (!seen.Add(node.Key))
    {
        continue;
    }
    HashSet<JuntionBox> connections = new HashSet<JuntionBox>();
    AddNode(node.Key, connections);
    circuits.Add(connections);
}

void AddNode(JuntionBox box, HashSet<JuntionBox> connections)
{
    connections.Add(box);
    foreach (var connected in nodes[box])
    {
        if (!seen.Add(connected))
        {
            continue;
        }
        AddNode(connected, connections);
    }
}

var smallestConnected = circuits.Select(x => x.Count).OrderByDescending(x => x).Take(3);


Console.WriteLine($"Smallest product: {smallestConnected.Aggregate((x, x1) => x * x1)}");

record JuntionBox(int X, int Y, int Z)
{
    public double DistanceTo(JuntionBox box) => Math.Pow(box.X - X, 2) + Math.Pow(box.Y - Y, 2) + Math.Pow(box.Z - Z, 2);
}

record Pair(JuntionBox A, JuntionBox B, double Distance);

partial class Reg
{
    [GeneratedRegex("^(\\d+),(\\d+),(\\d+)$")]
    public static partial Regex GetCoords();
}