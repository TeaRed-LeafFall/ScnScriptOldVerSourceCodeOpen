namespace ScnScript;

public static partial class ScnScriptCommon;
/// <summary>
/// 词法分析器配置
/// </summary>
public static class LexerConfigs
{
    /// <summary>
    /// 默认的行起始位置
    /// </summary>
    internal const int DefaultPositionLineStart = 1;
    /// <summary>
    /// 默认的列起始位置
    /// </summary>
    internal const int DefaultPositionColumnStart = 1;
    /// <summary>
    /// 在换行时重置列位置
    /// </summary>
    internal const int DefaultPositionResetColumnWhenNewLine = 1;
    /// <summary>
    /// 默认支持对象冒号结束
    /// </summary>
    internal const bool DefaultSupportObjectColonEnd = true;
    /// <summary>
    /// 默认支持注释截断代码
    /// </summary>
    internal const bool DefaultSupportForRuncatingCodeWithComments = true;
    /// <summary>
    /// 如果输入的不是任何可识别命令，让其一律作为文本输入
    /// </summary>
    internal const bool DefaultLetIgnoreUnknownAsTextFeature = true;
}
