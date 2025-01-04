using ScnScript.Runtime.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Runtime;
/// <summary>
/// 函数信息
/// </summary>
public class RuntimeFuncInfo
{
    /// <summary>
    /// 函数提供者
    /// </summary>
    public ScriptProvider Provider { get; init; }
    /// <summary>
    /// 函数针对Action类型
    /// </summary>
    public ActionType Type { get; init; }
    /// <summary>
    /// 函数类名
    /// </summary>
    public string ClassName { get; init; }
    /// <summary>
    /// 函数名称
    /// </summary>
    public string Head { get; init; }
    /// <summary>
    /// 函数别称
    /// </summary>
    public string[]? AkaNames { get; init; }
}
