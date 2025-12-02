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

    for (long a = start; a <= end; a++)
    {
        var number = a.ToString();
        if(IsMatch(number))
        {
            Console.WriteLine($" => {number}");
            sum += a;
        }
    }
}

// static bool IsMatch(ReadOnlySpan<char> number)
// {
//     // for each possible lenght of sequences (1 to half the lenght of number)
//     for (int i = 1; i <= number.Length / 2; i++)
//     {
//         //if number can't be divided in equal parts, continue
//         if (number.Length % i != 0)
//         {
//             continue;
//         }
//         
//         var needle = number.Slice(0, i);
//         //match until the end
//         int offset = i;
//         do
//         {
//             var test =  number.Slice(offset, i);
//             if (!test.SequenceEqual(needle))
//             {
//                 break;
//             }
//
//             offset += i;
//             
//             //if we matched all number, we got a positive
//             if (offset >= number.Length)
//             {
//                 return true;
//             }
//         } while(offset < number.Length);
//     }
//     return false;
// }

//alternative answer
static bool IsMatch(ReadOnlySpan<char> number) => GetPattern().IsMatch(number);

Console.WriteLine(sum);


partial class Program
{
    [GeneratedRegex("([0-9]+)-([0-9]+)")]
    private static partial Regex GetRanges();

    [GeneratedRegex("^(\\d+?)\\1+$")]
    private static partial Regex GetPattern();
}