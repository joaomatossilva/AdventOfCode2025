// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var shapes = new List<List<string>>();
var regions = new List<Region>();

int lineIndex = 0;
while (lineIndex < lines.Length)
{
    var  line = lines[lineIndex];
    if (Reg.ShapeIndex().Match(line).Success)
    {
        var shape = new List<string>();
        do
        {
            lineIndex++;
            line =  lines[lineIndex];
            if (line.Length == 0)
            {
                break;
            }
            shape.Add(line);
        } while(true);
        shapes.Add(shape);
    }
    else
    {
        var regionMatch = Reg.Region().Match(line);
        if (!regionMatch.Success)
        {
            continue;
        }
        
        var w = int.Parse(regionMatch.Groups[1].Value);
        var h = int.Parse(regionMatch.Groups[2].Value);
        var presents = regionMatch.Groups[3].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        regions.Add(new Region(w, h, presents));
    }
    
    lineIndex++;
}

//Filter out all that are completely impossible by occupied area
var areas = shapes.Select(x =>
{
    int a = 0;
    foreach (var shapeLine in x)
    {
        a += shapeLine.Count(c => c == '#');
    }

    return a;
}).ToArray();

regions = regions.Where(region =>
{
    var totalArea = region.H * region.W;
    for (int i = 0; i < region.Presents.Length; i++)
    {
        totalArea -= region.Presents[i] * areas[i];
    }

    return totalArea >= 0;
}).ToList();

Console.WriteLine($"After area excluding {regions.Count} remaining");

//...

record Region(int W, int H, int[] Presents);

public static partial class Reg
{
    [GeneratedRegex("^\\d:")]
    public static partial Regex ShapeIndex();

    [GeneratedRegex("^(\\d+)x(\\d+):(.*)$")]
    public static partial Regex Region();
}