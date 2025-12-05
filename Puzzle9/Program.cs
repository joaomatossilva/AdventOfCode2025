// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var ranges = new List<Range>();
var ids = new List<long>();

foreach (var line in lines)
{
    var rangeMatch = Reg.GetRanges().Match(line);
    if (rangeMatch.Success)
    {
        var start = long.Parse(rangeMatch.Groups[1].Value);
        var end = long.Parse(rangeMatch.Groups[2].Value);
        ranges.Add(new Range(start, end));
        continue;
    }
    
    var idsMatch = Reg.GetIds().Match(line);
    if (idsMatch.Success)
    {
        ids.Add(long.Parse(idsMatch.Groups[1].Value));
    }
}

var count = 0;
foreach (var id in ids)
{
    foreach (var range in ranges)
    {
        if (range.IsInside(id))
        {
            count++;
            break;
        }
    }
}

Console.WriteLine($"Count: {count}");

partial class Reg
{
    [GeneratedRegex("^(\\d+)-(\\d+)$")]
    public static partial Regex GetRanges();

    [GeneratedRegex("^(\\d+)$")]
    public static partial Regex GetIds();
}

record Range(long Start, long End)
{
    public bool IsInside(long x) => x >= Start && x <= End;
}