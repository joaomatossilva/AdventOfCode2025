// See https://aka.ms/new-console-template for more information
var lines = File.ReadAllLines("input.txt");

var grid = new List<bool[]>();
foreach (var line in lines)
{
    var row = new bool[line.Length];
    for (int i = 0; i < line.Length; i++)
    {
        row[i] = line[i] == '@';
    }
    grid.Add(row);
}

int count = 0;
for (var y = 0; y < grid.Count; y++)
{
    var row = grid[y];
    for (var x = 0; x < row.Length; x++)
    {
        var pos = row[x];
        if (!pos)
        {
            continue;
        }

        if (CheckAround(y, x) < 4)
        {
            count++;
            continue;
        }
    }
    Console.WriteLine();
}

Console.WriteLine(count);


int CheckAround(int y, int x) => AllAround(y, x).Select(((int y, int x) coords) => grid[coords.y][coords.x] ? 1 : 0).Sum();

IEnumerable<(int, int)> AllAround(int y, int x)
{
    if (y > 0)
    {
        if (x > 0)
        {
            yield return (y - 1, x - 1);
        }

        yield return (y - 1, x);

        if (x < grid[0].Length - 1)
        {
            yield return (y - 1, x + 1);
        }
    }

    if (x < grid[0].Length - 1)
    {
        yield return (y, x + 1);
    }

    if (x > 0)
    {
        yield return (y, x - 1);
    }

    if (y < grid.Count - 1)
    {
        if (x > 0)
        {
            yield return (y + 1, x - 1);
        }

        yield return (y + 1, x);
        
        if (x < grid[0].Length - 1)
        {
            yield return (y + 1, x + 1);
        }
    }
}