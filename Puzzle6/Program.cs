// See https://aka.ms/new-console-template for more information

var lines = File.ReadAllText("input.txt").Split(Environment.NewLine);

long sum = 0;

foreach (var line in lines)
{
    var finalRating = FindMaxForPos(line, 12);
    
    Console.WriteLine($"{line} => {finalRating}");
    
    sum += finalRating;
}

Console.WriteLine(sum);


static long FindMaxForPos(ReadOnlySpan<char> line, int pos)
{
    long max = 0;
    int maxAt = 0;
    //Length - pos + 1 => 14 - 2 + => 13 (14 -1)
    for (int i = 0; i < line.Length - pos + 1; i++)
    {
        var val =  line[i] - '0';

        //skip if value is already lower
        if (val <= max)
        {
            continue;
        }

        max = val;
        maxAt = i;
    }

    max *= (long)Math.Pow(10, pos - 1);

    if (pos > 1)
    {
        max += FindMaxForPos(line.Slice(maxAt + 1), pos - 1);
    }
    
    return max;
}