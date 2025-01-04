using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Runtime.Basic;
[Script(ScriptServicesType.Provider, "ScnScriptLib")]
[ScriptClass("scn")]
public class ScriptScnLib : ScriptProvider
{
    /// <summary>
    /// 检查版本
    /// </summary>
    /// <exception cref="ScnRuntimeException">版本低于请求版本</exception>
    [ScriptFunc(ActionType.Command, "ver")]
    public void CheckVersion(ScriptFuncCallData data)
    {
        if (data.ActionConfigs is null) return;
        if (data.ActionConfigs!.Count is 1 && data.ActionConfigs!.TryGetValue("Value", out string? ver))
        {
            Version version = int.TryParse(ver, out var verNum) ? new Version(verNum, 0, 0, 0) : new Version(ver);

            Console.LogInfo($"请求 ScnScript 版本为 {version}");

            if(ScnScriptCommon.Version < version)
            {
                throw new ScnRuntimeException($"面对脚本请求的 {version} 版本, 当前 ScnScript {ScnScriptCommon.Version} 过旧, 请更新到更新的版本");
            }
            if(ScnScriptCommon.Version > version)
            {
                Console.LogWarning($"脚本请求的 {version} 版本低于当前 ScnScript 版本, 可能会遇到一些兼容性问题。");
            }
            if(ScnScriptCommon.Version == version)
            {
                Console.LogInfo($"脚本请求的 {version} 版本与当前 ScnScript {ScnScriptCommon.Version} 版本一致。");
            }
        }
        else
        {
            throw new ScnRuntimeException("Invalid version number");
        }
    }
    [ScriptFunc(ActionType.Command, "doctype")]
    public void Doctype(ScriptFuncCallData data)
    {
        if (data.ActionConfigs is null) return;
        if (data.ActionConfigs!.Count is 1 && data.ActionConfigs!.TryGetValue("Value", out string? value))
        {
            var doctype = data.GetString(value);
            if(Enum.TryParse(doctype, true, out DocumentType type))
            {
                data.SandBox.DocumentType = type;
            }
            else
            {
                throw new ScnRuntimeException($"你写的是什么类型？！ {doctype} 不是有效的 DocumentType 值");
            }
        }
    }
    [ScriptFunc(ActionType.Command,"shell")]
    public void Shell(ScriptFuncCallData data)
    {
        if (data.ActionConfigs is null) return;
        if (data.ActionConfigs!.Count is 1 && data.ActionConfigs!.TryGetValue("type", out string? value))
        {
            var type = data.GetString(value);
            if (data.ScriptHost.IsObjectType(type))
            {
                Console.LogInfo($"存在类型 {type}");
            }
            else
            {
                throw new ScnRuntimeException($"不存在脚本所要求的类型 {type} ,请尝试导入所需要的 ScriptProvider！");
            }
        }
    }
}
