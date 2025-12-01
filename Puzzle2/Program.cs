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
    var extraTurns = Math.Abs(rotation / 100);
    rotation %= 100;

    if (numbers.Groups[1].Value == "R")
    {
        dial += rotation;
    }
    else
    {
        dial -= rotation;
    }
    
    
    if (dial > 100)
    {
        extraTurns++;
    }
    dial  %= 100;
    
    //adjust negative    
    if (dial < 0)
    {
        if (dial > -1* rotation)
        {
            extraTurns ++;
        }
        
        dial += 100; //dial is already negative
    }

    password += extraTurns;
    
    if (dial == 0)
    {
        password++;
    }
    
    //Console.WriteLine($"{numbers.Groups[1].Value}{numbers.Groups[2].Value} => {dial} {extraTurns} {password}");
}

Console.WriteLine(password);


partial class Program
{
    [GeneratedRegex("([LR])([0-9]+)")]
    private static partial Regex GetNumbers();
}