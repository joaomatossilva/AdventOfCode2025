// See https://aka.ms/new-console-template for more information

var lines = File.ReadAllLines("input.txt")
    .Select(x => x.ToCharArray())
    .ToArray();

var numbers = new List<long[]>();
char[] ops = null;

int opIndex = 0;
int nextOpIndex = 0;
do
{
    for (nextOpIndex = opIndex + 1; nextOpIndex < lines[^1].Length; nextOpIndex++)
    {
        if (lines[^1][nextOpIndex] != ' ')
        {
            break;
        }
    }

    var numbersArray = ReadNumbers(opIndex, nextOpIndex == lines[^1].Length ? nextOpIndex : nextOpIndex - 1);
    numbers.Add(numbersArray);
    
    opIndex = nextOpIndex;
} while(opIndex < lines[^1].Length);

ops = lines[^1].Where(x => x == '+' || x == '*').ToArray();

if (ops == null)
{
    throw new Exception("No ops found");
}
var opsLenght = ops.Length;
if (numbers.Count != opsLenght)
{
    throw new Exception("Invalid number length");
}

long totalCount = 0;

for (int i = 0; i < opsLenght; i++)
{
    long opsTotal = ops[i] == '+' ? 0 : 1;
    for (int n = 0; n < numbers[i].Length; n++)
    {
        if (ops[i] == '+')
        {
            opsTotal += numbers[i][n];
        }
        else if (ops[i] == '*')
        {
            opsTotal *= numbers[i][n];
        }
    }
    totalCount += opsTotal;
}

Console.WriteLine(totalCount);

long[] ReadNumbers(int startIndex, int endIndex)
{
    var colNumbers = new List<long>();
    for (int i = endIndex - 1; i >= startIndex; --i)
    {
        long number = 0;
        int order = 0;
        for (int n = lines.Length - 2; n >= 0; n--)
        {
            var c =  lines[n][i];
            if (c != ' ')
            {
                number += (c - '0') * (long)Math.Pow(10, order++);
            }
        }
        colNumbers.Add(number);
    }
    
    return colNumbers.ToArray();
}