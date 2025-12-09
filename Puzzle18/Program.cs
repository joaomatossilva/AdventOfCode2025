// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var inputLines = File.ReadAllLines("input.txt");
var points = new List<Point>();

foreach (var line in inputLines)
{
    var match = Reg.GetNumbers().Match(line);
    if (!match.Success)
    {
        continue;
    }
    
    var point = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
    points.Add(point);
}

//build polygon based on H and V
var polygonH = new Dictionary<int, List<Line>>();
var polygonV = new Dictionary<int, List<Line>>();

//appending start point on end to close polygon
points.Add(points[0]);
for (int i = 0; i < points.Count - 1; i++)
{
    var line = new Line(points[i], points[i + 1]);
    if (line.A.X == line.B.X) // Same X => vertical
    {
        if (polygonV.TryGetValue(line.A.X, out var list))
        {
            list.Add(line);
            continue;
        }
        polygonV.Add(line.A.X, [line]);
    }
    if (line.A.Y == line.B.Y) // Same Y => horizontal
    {
        if (polygonH.TryGetValue(line.A.Y, out var list))
        {
            list.Add(line);
            continue;
        }
        polygonH.Add(line.A.Y, [line]);
    }
}

// Build H and V rasters
var rasterV =  new Dictionary<int, List<Line>>();
var rasterH = new Dictionary<int, List<Line>>();

var startX = polygonV.Keys.Min();
var endX = polygonV.Keys.Max();
var startY = polygonH.Keys.Min();
var endY = polygonH.Keys.Max();

// vertical rasters
for (int x = startX; x <= endX; x++)
{
    var vRasters = new List<Line>();

    if (!polygonV.TryGetValue(x, out var vCols))
    {
        vCols = [];
    }

    var yCrossingLinesOnX = polygonH.Where(phKey => phKey.Value.Any(pH => pH.CrossesOnX(x))).Select(phKey => phKey.Key).OrderBy(y => y).ToList();

    //if we have even numbers matching vertical lines, we need to adjust beginning and ends
    foreach (var vCol in vCols)
    {
        var minY = Math.Min(vCol.A.Y, vCol.B.Y);
        var lessers = yCrossingLinesOnX.Count(y => y < minY);
        if (lessers % 2 != 0)
        {
            yCrossingLinesOnX.Remove(minY);
        }
        
        var maxY = Math.Max(vCol.A.Y, vCol.B.Y);
        var maxers = yCrossingLinesOnX.Count(y => y > maxY);
        if (maxers % 2 != 0)
        {
            yCrossingLinesOnX.Remove(maxY);
        }
    }
    
    for(int i = 0; (i + 1) < yCrossingLinesOnX.Count; i += 2)
    {
        var a = new Point(x, yCrossingLinesOnX[i]);
        var b = new Point(x, yCrossingLinesOnX[i + 1]);
        var col = new Line(a, b);
        vRasters.Add(col);
    }
    
    if(vRasters.Count > 0)
        rasterV.Add(x, vRasters);
}

// horizontal rasters
for (int y = startY; y <= endY; y++)
{
    var yRasters = new List<Line>();

    if (!polygonH.TryGetValue(y, out var hLines))
    {
        hLines = [];
    }

    var xCrossingColsOnY = polygonV.Where(pvKey => pvKey.Value.Any(pV => pV.CrossesOnY(y))).Select(pvKey => pvKey.Key).ToList();
    
    //if we have even numbers matching horizontal lines, we need to adjust beginning and ends
    foreach (var hLine in hLines)
    {
        var minX = Math.Min(hLine.A.X, hLine.B.X);
        var lessers = xCrossingColsOnY.Count(x => x < minX);
        if (lessers % 2 != 0)
        {
            xCrossingColsOnY.Remove(minX);
        }
        
        var maxX = Math.Max(hLine.A.X, hLine.B.X);
        var maxers = xCrossingColsOnY.Count(x => x > maxX);
        if (maxers % 2 != 0)
        {
            xCrossingColsOnY.Remove(maxX);
        }
    }
    
    for(int i = 0; (i + 1) < xCrossingColsOnY.Count; i += 2)
    {
        var a = new Point(xCrossingColsOnY[i], y);
        var b = new Point(xCrossingColsOnY[i + 1], y);
        var line = new Line(a, b);
        yRasters.Add(line);
    }
    
    if(yRasters.Count > 0)
        rasterH.Add(y, yRasters);
}

long largestArea = 0;

for (int i = 0; i < points.Count; i++)
{
    for (int j = i + 1; j < points.Count; j++)
    {
        var pointA = points[i];
        var pointB = points[j];
        var area = pointA.CalculateAreaWith(pointB);
        
        if (area > largestArea)
        {
            var hSides = pointA.GetHorizontalSides(pointB);
            var hSidesInsideAll = hSides.All(hSide =>
            {
                var line = rasterH[hSide.A.Y];
                return line.Any(l => l.IsContainedHorizontally(hSide));
            });
            
            if (!hSidesInsideAll)
            {
                continue;
            }
            
            var vSides = pointA.GetVerticalSides(pointB);
            var vSidesInsideAll = vSides.All(vSide =>
            {
                var col = rasterV[vSide.A.X];
                return col.Any(l => l.IsContainedVertically(vSide));
            });
            
            if (!vSidesInsideAll)
            {
                continue;
            }

            Console.WriteLine($"Found {pointA} -  {pointB} => {area}");
            largestArea = area;
        }
    }
}

Console.WriteLine($"Largest Rectangle: {largestArea}");

public record Line(Point A, Point B)
{
    public bool IsContainedHorizontally(Line l)
    {
        return Math.Min(l.A.X, l.B.X) >= Math.Min(A.X, B.X) && Math.Max(l.A.X, l.B.X) <= Math.Max(A.X, B.X);
    }
    
    public bool IsContainedVertically(Line l)
    {
        return Math.Min(l.A.Y, l.B.Y) >= Math.Min(A.Y, B.Y) && Math.Max(l.A.Y, l.B.Y) <= Math.Max(A.Y, B.Y);
    }

    public bool CrossesOnX(int x)
    {
        return x >= Math.Min(A.X, B.X) && x <= Math.Max(A.X, B.X);
    }

    public bool CrossesOnY(int y)
    {
        return y >= Math.Min(A.Y, B.Y) && y <= Math.Max(A.Y, B.Y);
    }
}

public record Point(int X, int Y)
{
    public long CalculateAreaWith(Point other)
    {
        long dX = Math.Abs(X - other.X) + 1;
        long dY = Math.Abs(Y - other.Y) + 1;
        return dX * dY;
    }

    public IEnumerable<Line> GetHorizontalSides(Point other)
    {
        yield return new Line(this, other with { Y = Y });
        if (other.Y != Y)
        {
            yield return new Line(this with { Y = other.Y }, other);
        }
    }

    public IEnumerable<Line> GetVerticalSides(Point other)
    {
        yield return new Line(this, other with { X = X });
        if (other.X != X)
        {
            yield return new Line(this with { X = other.X }, other);
        }
    }
};

partial class Reg
{
    [GeneratedRegex("^(\\d+),(\\d+)$")]
    public static partial Regex GetNumbers();
}