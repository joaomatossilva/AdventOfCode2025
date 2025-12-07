// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices;

var lines = File.ReadAllLines("input.txt");

(int y, int x) start = (0, 0);
var grid = new List<bool[]>();
for (var index = 0; index < lines.Length; index++)
{
    var line = lines[index];
    var row = new bool[line.Length];
    for (int i = 0; i < line.Length; i++)
    {
        row[i] = line[i] == '^';
        if (line[i] == 'S')
        {
            start = (index, i);
        }
    }

    grid.Add(row);
}


Dictionary<int, long>[] beams = new Dictionary<int, long>[grid.Count];
//initial beam on Start.x
beams[0] = new Dictionary<int, long>();
beams[0].Add(start.x, 1);

for (int y = start.y + 1; y < grid.Count; y++)
{
    beams[y] = new Dictionary<int, long>();
    foreach (var beam in beams[y - 1])
    {
        //if we did not hit a splitter
        if (!grid[y][beam.Key])
        {
            CollectionsMarshal.GetValueRefOrAddDefault(beams[y], beam.Key, out _) += beam.Value;
        }
        else
        {
           
            var newBeamX = beam.Key + 1;
            if (newBeamX <= grid[0].Length)
            {
                CollectionsMarshal.GetValueRefOrAddDefault(beams[y], newBeamX, out _) += beam.Value;
            }
    
            newBeamX = beam.Key - 1;
            if (newBeamX >= 0)
            {
                CollectionsMarshal.GetValueRefOrAddDefault(beams[y], newBeamX, out _) += beam.Value;
            }
        }
    }
}

var count = beams[grid.Count - 1].Select(x => x.Value).Sum();
Console.WriteLine($"Timelines: {count}");