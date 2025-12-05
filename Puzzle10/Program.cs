// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var ranges = new List<Range>();

foreach (var line in lines)
{
    var rangeMatch = Reg.GetRanges().Match(line);
    if (rangeMatch.Success)
    {
        var start = long.Parse(rangeMatch.Groups[1].Value);
        var end = long.Parse(rangeMatch.Groups[2].Value);
        var range =  new Range(start, end);

        var listToRemove = new List<Range>();
        foreach (var range1 in ranges)
        {
            range = range.Adjust(range1,  out bool removeOther);
            if (removeOther)
            {
                listToRemove.Add(range1);
            }
        }

        ranges = ranges.Except(listToRemove).ToList();
        
        if(range.Length > 0)
            ranges.Add(range);
    }
}

long count = 0;
foreach (var range in ranges)
{
    count += range.Length;
}

Console.WriteLine($"Count: {count}");

partial class Reg
{
    [GeneratedRegex("^(\\d+)-(\\d+)$")]
    public static partial Regex GetRanges();
}

record Range(long Start, long End)
{
    public long Length => End - Start + 1;

    public Range Adjust(Range other, out bool removeOther)
    {
        removeOther = false;
        long start = Start;
        long end = End;
        
        if (Start <= other.End && Start >= other.Start)
        {
            start = other.End + 1;
        }

        if (End >= other.Start && End <= other.End)
        {
            end = other.Start - 1;
        }
        
        if (End >= other.End && Start <= other.Start)
        {
            removeOther =  true;
            return this;
        }
        
        if (End <= other.End && Start >= other.Start)
        {
            start = 1;
            end = 0;
        }
        
        return new Range(Start: start, End: end);
    }
}