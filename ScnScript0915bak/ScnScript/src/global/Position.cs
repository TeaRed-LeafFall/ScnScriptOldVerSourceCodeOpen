namespace ScnScript;

public static partial class ScnScriptCommon;
/// <summary>
/// 标志位置
/// </summary>
/// <param name="line">行</param>
/// <param name="column">列</param>
public struct Position(int line, int column)
{
    public int Line { get; set; } = line;
    public int Column { get; set; } = column;

    public override string ToString()
    {
        return $"({Line}, {Column})";
    }
}