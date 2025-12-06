// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var numbers = new List<long[]>();
char[] ops = null;

foreach (var line in lines)
{
    var numbersMatch = Reg.GetNumbers().Match(line);
    if (numbersMatch.Success)
    {
        var numbersArray = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        numbers.Add(numbersArray);
        continue;
    }
    
    var opsMatch = Reg.GetOps().Match(line);
    if (opsMatch.Success)
    {
        ops = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToArray();
    }
}

if (ops == null)
{
    throw new Exception("No ops found");
}
var opsLenght = ops.Length;
foreach (var numberLine in numbers)
{
    if (numberLine.Length != opsLenght)
    {
        throw new Exception("Invalid number length");
    }
}

long totalCount = 0;

for (int i = 0; i < opsLenght; i++)
{
    long opsTotal = ops[i] == '+' ? 0 : 1;
    for (int n = 0; n < numbers.Count; n++)
    {
        if (ops[i] == '+')
        {
            opsTotal += numbers[n][i];
        }
        else if (ops[i] == '*')
        {
            opsTotal *= numbers[n][i];
        }
    }
    totalCount += opsTotal;
}

Console.WriteLine(totalCount);


partial class Reg
{
    [GeneratedRegex("\\d+")]
    public static partial Regex GetNumbers();

    [GeneratedRegex("[\\*\\+]")]
    public static partial Regex GetOps();
}