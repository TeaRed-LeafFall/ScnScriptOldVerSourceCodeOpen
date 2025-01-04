using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.CodeAnalysis;

/// <summary>
/// 行为树叶子
/// </summary>
public class BehaviorTreeLeaf
{
    public List<BehaviorTreeLeaf>? Children { get; set; }
}
/// <summary>
/// 节点
/// </summary>
public class NodeLeaf : BehaviorTreeLeaf
{
    public string Name { get; set; } = string.Empty;
    public static NodeLeaf Copy(string name)
    {
        return new NodeLeaf { Name = name };
    }
    public override string ToString() => $"NodeLeaf: {Name}";
}
/// <summary>
/// 场景
/// </summary>
public class SceneLeaf : BehaviorTreeLeaf
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsComplete { get; set; } = false;
    internal bool IsEnd { get; set; } = false;
    public static SceneLeaf Copy(NodeLeaf node, string displayName)
    {
        return new SceneLeaf
        {
            Name = node.Name,
            DisplayName = displayName
        };
    }
    public static SceneLeaf Copy(NodeLeaf node)
    {
        return new SceneLeaf
        {
            Name = node.Name
        };
    }

    public override string ToString() => $"SceneLeaf: {Name} {DisplayName}";
}
/// <summary>
/// 字符串
/// </summary>
public class StringLeaf : BehaviorTreeLeaf
{
    public string Value { get; set; } = string.Empty;
    public override string ToString() => $"String: {Value}";
}
/// <summary>
/// 命令节点
/// </summary>
public class CommandLeaf : BehaviorTreeLeaf
{
    /// <summary>
    /// 命令类名称
    /// </summary>
    public string? ClassName;
    /// <summary>
    /// 命令方法名称
    /// </summary>
    public string MethodName = string.Empty;
    /// <summary>
    /// 命令配置
    /// </summary>
    public Dictionary<string, ExpressParametersValue>? Parameters { get; set; }
    /// <summary>
    /// 命令参数
    /// </summary>
    public List<string>? Args { get; set; }
    public bool HasCloseTag { get; set; }

    public override string ToString() => $"Command: {ClassName} {MethodName}";
}
/// <summary>
/// 对象控制器命令
/// </summary>
/// <remarks>
/// 用于对象创建与对象命令调用
/// </remarks>
public class ObjectControllerCommandLeaf : BehaviorTreeLeaf
{
    /// <summary>
    /// 对象控制器名称
    /// </summary>
    public string MethodName = string.Empty;
    /// <summary>
    /// 配置
    /// </summary>
    public Dictionary<string, ExpressParametersValue>? Parameters { get; set; }
    /// <summary>
    /// 值
    /// </summary>
    public string? Value;
    public List<string>? ScnKeywords { get; set; }
    public override string ToString() => $"ObjCommand: {MethodName}";
}
/// <summary>
/// 流程控制命令
/// </summary>
public class FlowControlCommandLeaf : BehaviorTreeLeaf
{
    /// <summary>
    /// 条件
    /// </summary>
    public ExpressParametersValueConditions? Conditions { get; set; }
    public FlowControlCommandType CommandType { get; set; } = FlowControlCommandType.Do;
    /// <summary>
    /// 路径子节点
    /// </summary>
    public Dictionary<bool, List<BehaviorTreeLeaf>>? RouteChildren { get; set; }
    /// <summary>
    /// 目标Id
    /// </summary>
    public int TargetId { get; set; }
}

public enum FlowControlCommandType
{
    If,
    While,
    For,
    Do,
}