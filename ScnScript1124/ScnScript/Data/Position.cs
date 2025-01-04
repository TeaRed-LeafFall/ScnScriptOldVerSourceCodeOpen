using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Data;
// 这是一个标识代码位置的类
public class Position(int line, int column)
{
    private const int PositionStartLine = 1;

    public int Line { get; set; } = line;
    public int Column { get; set; } = column;

    public override string ToString()
    {
        return $"({Line}, {Column})";
    }
    public Position(Position pos) : this(pos.Line, pos.Column)
    {
    }
    public static Position None => new(PositionStartLine, 0);
}
