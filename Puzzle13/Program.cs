// See https://aka.ms/new-console-template for more information

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

var beams = new HashSet<int>();
//initial beam on Start.x
beams.Add(start.x);
int split = 0;

for (int y = start.y + 1; y < grid.Count; y++)
{
    var newBeams = new HashSet<int>();
    foreach (var beam in beams)
    {
        //if we did not hit a splitter
        if (!grid[y][beam])
        {
            newBeams.Add(beam);
        }
        else
        {
            split++;
            
            foreach (var newBeam in SplitBeams(beam, newBeams))
            {
                newBeams.Add(newBeam);
            }
        }
    }
    beams = newBeams;
}

Console.WriteLine($"Splits: {split}");

IEnumerable<int> SplitBeams(int beam, HashSet<int> currentBeams)
{
    var newBeam = beam + 1;
    if (newBeam <= grid[0].Length && !currentBeams.Contains(newBeam))
    {
        yield return newBeam;
    }
    
    newBeam = beam - 1;
    if (newBeam > 0 && !currentBeams.Contains(newBeam))
    {
        yield return newBeam;
    }
}