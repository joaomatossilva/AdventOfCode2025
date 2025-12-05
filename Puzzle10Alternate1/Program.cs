// Alternate Answer with coordinate compression

using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var ranges = new List<Range>();
HashSet<long> coordinates = new HashSet<long>();

foreach (var line in lines)
{
    var rangeMatch = Reg.GetRanges().Match(line);
    if (rangeMatch.Success)
    {
        var start = long.Parse(rangeMatch.Groups[1].Value);
        var end = long.Parse(rangeMatch.Groups[2].Value);
        var range =  new Range(start, end);
        coordinates.Add(start);
        coordinates.Add(end + 1);
        ranges.Add(range);
    }
}

long count = 0;
var sorted = coordinates.OrderBy(c => c).ToList();
for (int i = 0; i < sorted.Count - 1; i++)
{
    var start = sorted[i];
    var end = sorted[i + 1];
    
    foreach (var range in ranges)
    {
        if (range.IsInside(start))
        {
            count += end - start;
            break;
        }
    }
}



Console.WriteLine($"Count: {count}");

partial class Reg
{
    [GeneratedRegex("^(\\d+)-(\\d+)$")]
    public static partial Regex GetRanges();
}

record Range(long Start, long End)
{
    public bool IsInside(long x) => x >= Start && x <= End;
}