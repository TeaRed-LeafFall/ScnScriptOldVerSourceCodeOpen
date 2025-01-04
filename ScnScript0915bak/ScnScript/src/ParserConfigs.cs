namespace ScnScript;

public static partial class ScnScriptCommon;
public static class ParserConfigs
{

    /// <summary>
    /// 默认在解析器中移除字符串符号(指 "xx" 变成 xx )
    /// </summary>
    internal const bool DefaultStringSymbolRemoveFeature = true;
    /// <summary>
    /// 默认在解析器中移除空字符串
    /// </summary>
    internal const bool DefaultRemoveEmptyStringFeature = true;
    /// <summary>
    /// 默认在解析器中启用让对象+-特性支持
    /// </summary>
    internal const bool DefaultLetObjectPlusMinusFeature = true;
    /// <summary>
    /// 默认在解析器中启用让对象以-作为Clr特性支持
    /// </summary>
    internal const bool DefaultLetObjectStartWithMinusAsClrFeature = true;
    /// <summary>
    /// 默认在解析器中启用让命令以/作为结束标签的特性
    /// </summary>
    internal const bool DefaultLetCommandStartWithSlashAsCloseTagFeature = true;
    /// <summary>
    /// 默认在解析器中定义的非法字符
    /// </summary>
    internal const string IllegalCharacters = "!\"#$%&'()*+,-./:;<=>?@[]^`{|}~";
    /// <summary>
    /// 默认在解析器中启用让对象值作为参数的特性
    /// </summary>
    internal const bool DefaultLetObjectValueAsArgsFeature = true;
}
