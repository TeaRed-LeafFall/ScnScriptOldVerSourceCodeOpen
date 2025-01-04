using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Data;

public class Area
{
    public int Start { get; set; }
    public int End { get; set; }
    public int Length => End - Start;
    public override string ToString()
    {
        return $"[{Start},{End}] {Length}";
    }
    public Area(Area area)
    {
        Start = area.Start;
        End = area.End;
    }
}
