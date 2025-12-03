// See https://aka.ms/new-console-template for more information

var lines = File.ReadAllText("input.txt").Split(Environment.NewLine);

long sum = 0;

foreach (var line in lines)
{
    var finalRating = FindMaxCombination(line);

    Console.WriteLine($"{line} => {finalRating}");
    
    sum += finalRating;
}

Console.WriteLine(sum);


static int FindMaxCombination(ReadOnlySpan<char> line)
{
    int max = 0;
    //until Length -1 because we need a 2 combination always
    for (int i = 0; i < line.Length - 1; i++)
    {
        var tenths =  (line[i] - '0') * 10;

        //skip if tenths value is already lower
        if (tenths < max / 10)
        {
            continue;
        }
        
        var found = DeepSearch(tenths, line.Slice(i + 1));
        if (found > max)
        {
            max = found;
        }
    }
    
    return max;
}

static int DeepSearch(int tenths, ReadOnlySpan<char> line)
{
    var max = tenths;
    foreach (var units in line)
    {
        var unit = units - '0';
        if(tenths + unit > max)
        {
            max = tenths + unit;
        }
    }

    return max;
}