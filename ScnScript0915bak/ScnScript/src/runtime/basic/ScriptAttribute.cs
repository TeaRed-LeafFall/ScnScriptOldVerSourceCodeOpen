using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Runtime.Basic;

[AttributeUsage(AttributeTargets.Class)]
public class ScriptAttribute(ScriptServicesType type, string name) : Attribute
{
    public ScriptServicesType Type { get; set; } = type;
    public string Name { get; set; } = name;
}

public enum ScriptServicesType
{
    // 服务提供
    Provider,
    // 服务监听
    //Service,
    // 服务扩展
    //Extension
}

[AttributeUsage(AttributeTargets.Class)]
public class ScriptClassAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}
/// <summary>
/// 脚本函数特性标识
/// <list type="table">
/// <listheader>
/// <term>type</term>
/// <description>函数类型</description>
/// </listheader>
/// <item>
/// <term>head</term>
/// <description>函数名</description>
/// </item>
/// <item>
/// <term>akaNames</term>
/// <description>函数又称(可选)</description>
/// </item>
/// </list>
/// <example>
/// 使用示例：
/// <code>
/// [ScriptFunc(ActionType.Command, "test")]
/// public void Test(ScriptFuncCallData data)
/// {
///     // do something
/// }
/// </code>
/// </example>
/// </summary>
/// <remarks>
/// 注意：函数参数必须只有'ScriptFuncCallData data'
/// </remarks>
/// <exception cref="ScnRuntimeException"/>
[AttributeUsage(AttributeTargets.Method)]
public class ScriptFuncAttribute : Attribute
{
    public ScriptFuncAttribute(ActionType type,string head,params string[]? akaNames)
    {
        Type = type;

        if(Type is ActionType.Command)
        {
            if (string.IsNullOrEmpty(head))
            {
                throw new ScnRuntimeException("命令类型不支持标识为空！");
            }
        }
        if (!ScnScriptCommon.IsValidString(head))
        {
            throw new ScnRuntimeException("名称不支持包含非法字符！原值：" + head);
        }
        if (akaNames is not null)
        {
            foreach (var akaName in akaNames)
            {
                if (string.IsNullOrEmpty(akaName))
                {
                    throw new ScnRuntimeException("又称不应该存在空字符串!");
                }
                else
                {
                    if (!ScnScriptCommon.IsValidString(akaName))
                    {
                        throw new ScnRuntimeException("又称不应该包含非法字符!原值："+akaName);
                    }
                }
            }
        }
        Head = head;
        AkaNames = akaNames;
    }

    public ActionType Type { get; set; }
    public string Head { get; set; }
    public string[]? AkaNames { get; set; }
}