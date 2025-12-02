// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

string input = File.ReadAllText("input.txt");

string[] ranges = input.Split(',');
long sum = 0;
foreach (var rangeString in ranges)     
{
    Console.WriteLine(rangeString);
    var match = GetRanges().Match(rangeString);
    if (!match.Success)
    {
        continue;
    }
    
    var startString =  match.Groups[1].Value;
    var endString = match.Groups[2].Value;
    
    var start = long.Parse(startString);
    var end = long.Parse(endString);

    var startLength = startString.Length / 2;
    var endLength = endString.Length / 2;

    if (startString.Length % 2 == 1)
    {
        startLength++;
    }
    if (endString.Length % 2 == 1)
    {
        endLength++;
    }
    
    for (int div = startLength; div <= endLength; div += 2)
    {
        var divider = (int)Math.Pow(10, div);
        for (long a = start; a <= end; a++)
        {
            if (a % divider == a / divider && a / divider * 10 >= divider)
            {
                Console.WriteLine($" ==> {a}");
                sum += a;
            }
        }
    }
    
    //Going for the Math way, did not prove so good on Part 2
    
}

Console.WriteLine(sum);


partial class Program
{
    [GeneratedRegex("([0-9]+)-([0-9]+)")]
    private static partial Regex GetRanges();
}