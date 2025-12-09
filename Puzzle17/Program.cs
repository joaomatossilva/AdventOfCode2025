// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var points = new List<Point>();

foreach (var line in lines)
{
    var match = Reg.GetNumbers().Match(line);
    if (!match.Success)
    {
        continue;
    }
    
    var point = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
    points.Add(point);
}

long largestArea = 0;

//appending start point on end to close polygon
points.Add(points[0]);
for (int i = 0; i < points.Count; i++)
{
    for (int j = i + 1; j < points.Count; j++)
    {
        var pointA = points[i];
        var pointB = points[j];
        var area = pointA.CalculateAreaWith(pointB);
        if (area > largestArea)
        {
            Console.WriteLine($"{pointA} -  {pointB} => {area}");
            largestArea = area;
        }
    }
}

Console.WriteLine($"Largest Rectangle: {largestArea}");

record Point(int X, int Y)
{
    public long CalculateAreaWith(Point other)
    {
        long dX = Math.Abs(X - other.X) + 1;
        long dY = Math.Abs(Y - other.Y) + 1;
        return dX * dY;
    }
};

record Rectangle(Point A, Point B, long Area);

partial class Reg
{
    [GeneratedRegex("^(\\d+),(\\d+)$")]
    public static partial Regex GetNumbers();
}