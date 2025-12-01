using System.Text.RegularExpressions;

string input = File.ReadAllText("input.txt");

string[] lines = input.Split('\n');
int dial = 50;
int password = 0;
foreach (string line in lines)
{
    var numbers = GetNumbers().Match(line);
    if (!numbers.Success)
    {
        continue;
    }
    var rotation = int.Parse(numbers.Groups[2].Value);

    if (numbers.Groups[1].Value == "R")
    {
        dial += rotation;
        dial %= 100;
    }
    else
    {
        dial -= rotation;
        dial %= 100;
        if (dial < 0)
        {
            dial += 100; //dial is already negative
        }
    }

    if (dial == 0)
    {
        password++;
    }
}

Console.WriteLine(password);


partial class Program
{
    [GeneratedRegex("([LR])([0-9]+)")]
    private static partial Regex GetNumbers();
}