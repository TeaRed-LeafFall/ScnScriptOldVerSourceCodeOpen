namespace ScnScript;

public static partial class ScnScriptCommon;
/// <summary>
/// 行为类型
/// </summary>
public enum ActionType
{
    // 字符串 "xxx" 或者 xxx
    String,
    // 全局命令 [xxx] 格式 [xxx xxx "xx" 'xxx' xx="x"]
    Command,
    // 对象/流程等命令 @xxx 格式
    Object,
    // 节点 *xxx
    NodeSelector,
    // 场景 *xxx | Start(End)
    SceneSelector
}

/// <summary>
/// 行为 (由Parser生成)
/// </summary>
/// <param name="type">类型</param>
/// <param name="value">值</param>
/// <param name="head">头文本(主键)</param>
/// <param name="args">参数</param>
/// <param name="configs">配置</param>
public readonly struct ScnAction(ActionType type, string value, string head="", List<string>? args=null, Dictionary<string, string>? configs=null)
{
    // 属性
    /// <summary>
    /// 行为类型
    /// </summary>
    public ActionType Type { get; } = type;
    /// <summary>
    /// 关键字
    /// </summary>
    public string Value { get; } = value;
    /// <summary>
    /// 头文本
    /// </summary>
    public string Head { get; } = head;
    /// <summary>
    /// 参数
    /// </summary>
    public  List<string>? Args { get; } = args; 
    /// <summary>
    /// 配置
    /// </summary>
    public Dictionary<string, string>? Configs { get; } = configs;

    // 转换字符串方法
    public override string ToString()
    {
        var argsText = string.Empty;
        if (Args != null)
        {
            argsText = string.Join(",", Args);
        }
        var configsText = string.Empty;
        if(Configs != null)
        {
            configsText = Configs.Aggregate(configsText, (current, item) => current + $"{Environment.NewLine} {item.Key} = {item.Value}");
        }
        var result = $"Action: [{Type}] [{Head}] [{Value}] ";
        if (!string.IsNullOrEmpty(argsText))
        {
            result += $" {Environment.NewLine}Args: {argsText}";
        }
        if (!string.IsNullOrEmpty(configsText))
        {
            result += $" {Environment.NewLine}Configs: {configsText}";
        }

        return result;
    }
}