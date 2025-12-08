// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var pairs = new List<Pair>();
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
        pairs.Add(new Pair(keys[i], keys[j], distance));
    }
}

var smallPairs = pairs.OrderBy(x => x.Distance);

var circuits = new List<HashSet<JuntionBox>>();

foreach (var smallPair in smallPairs)
{
    var circuitOfA = GetCircuitOf(smallPair.A);
    var circuitOfB = GetCircuitOf(smallPair.B);

    if (circuitOfA == null && circuitOfB == null)
    {
        circuits.Add([smallPair.A, smallPair.B]);
    }
    else if(circuitOfA != null && circuitOfB == null)
    {
        circuitOfA.Add(smallPair.B);
    }
    else if(circuitOfB != null && circuitOfA == null)
    {
        circuitOfB.Add(smallPair.A);
    }
    else if (circuitOfA == circuitOfB) // same circuit, don't do anything
    {
        continue;
    }
    else
    {
        //move all of B into A
        foreach (var juntionBox in circuitOfB)
        {
            circuitOfA.Add(juntionBox);
        }

        circuits.Remove(circuitOfB);
    }

    if (circuits.Count == 1 && circuits[0].Count == nodes.Count)
    {
        Console.WriteLine($"Found Node at {smallPair.A.X} * {smallPair.B.X} = {smallPair.A.X *  smallPair.B.X}");
        return;
    }
}

Console.WriteLine("Not Found");

HashSet<JuntionBox>? GetCircuitOf(JuntionBox box) => circuits.FirstOrDefault(circuit => circuit.Contains(box));

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