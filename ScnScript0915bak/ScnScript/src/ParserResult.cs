namespace ScnScript;

public static partial class ScnScriptCommon;
/// <summary>
/// 解释器结果
/// </summary>
/// <param name="actions">命令行为</param>
/// <param name="positions">定位</param>
/// <param name="sourceCode">源代码</param>
public readonly struct ParserResult(Dictionary<int, ScnAction> actions, Dictionary<int, Position> positions, Dictionary<int, string> sourceCode)
{
    /// <summary>
    /// 运行行为指令
    /// </summary>
    public Dictionary<int,ScnAction> Actions { get; init; } = actions;
    /// <summary>
    /// 原位置
    /// </summary>
    public Dictionary<int,Position> Positions { get; init; } = positions;
    /// <summary>
    /// 原代码
    /// </summary>
    public Dictionary<int,string> SourceCode { get; init; } = sourceCode;

}
