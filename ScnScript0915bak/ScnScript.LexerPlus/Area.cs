namespace ScnScript;

public static partial class ScnScriptCommon;
/// <summary>
/// 区域
/// </summary>
/// <remarks>区域是一个具有两个数字的结构，表示区域的起始位置和结束位置。</remarks>
/// <param name="start">开始点</param>
/// <param name="end">结束点</param>
public readonly struct Area(int start, int end)
{
    /// <summary>
    /// 开始位置
    /// </summary>
    public int Start { get; } = start;
    /// <summary>
    /// 结束位置
    /// </summary>
    public int End { get; init; } = end;

    /// <summary>
    /// 返回区域的长度
    /// </summary>
    /// <returns>长度</returns>
    public int GetLength() => End < Start ? throw new ArgumentOutOfRangeException(nameof(End), "结束点不能小于开始点") : End - Start + 1;
    /// <summary>
    /// 判断给定的索引是否在区域内
    /// </summary>
    /// <param name="index">一个点</param>
    /// <returns>是否在范围内</returns>
    public bool IsInArea(int index) => index >= Start && index <= End;
    /// <summary>
    /// 判断给定的索引是否在区域内，但不包括节点
    /// </summary>
    /// <param name="index">一个点</param>
    /// <returns>是否在范围内</returns>
    public bool IsInAreaNoOnNode(int index) => index > Start && index < End;
    /// <summary>
    /// 判断给定的区域是否在区域内
    /// </summary>
    /// <param name="area">区域</param>
    /// <returns>是否在范围内</returns>
    public bool IsAreaInArea(Area area) => area.Start > Start && area.End < End;
    
    public override string ToString() => $"({Start} ~ {End})";
}