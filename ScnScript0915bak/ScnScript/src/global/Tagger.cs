using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript;
public enum TaggerType
{
    None,
    Scene,
    Node
}
public class Tagger
{
    public TaggerType Type;
    // 开始与结束位置
    public Point2 Point;
    public string DisplayName = string.Empty;
    public Tagger() { }
    public Tagger(TaggerType type, Point2 point)
    {
        Type = type;
        Point = point;
    }
}