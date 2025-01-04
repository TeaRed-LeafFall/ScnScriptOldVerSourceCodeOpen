using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Data;

public class Point2
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point2(double x, double y)
    {
        X = x;
        Y = y;
    }
    public Point2(Point2 point)
    {
        X = point.X;
        Y = point.Y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
