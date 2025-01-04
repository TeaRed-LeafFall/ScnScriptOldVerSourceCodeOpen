using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScnScript.Runtime.Basic;
/// <summary>
/// 函数调用数据
/// </summary>
/// <remarks>
/// <para>通常扩展函数使用此数据类获取各种各样的数据</para>
/// <para>具有丰富的封装函数，只需要调用里面的函数来完成各种操作</para>
/// </remarks>
/// <param name="selfAction">自身Action</param>
/// <param name="scriptHost">运行主机</param>
/// <param name="sandBox">沙盒</param>
/// <param name="callHead">调用头（通常用不到）</param>
public class ScriptFuncCallData(ScnAction selfAction, ScnScriptHost scriptHost, RuntimeSandBox sandBox, string callHead)
{
    // 主要数据
    public ScnAction SelfAction { get; init; } = selfAction;
    public ScnScriptHost ScriptHost { get; init; } = scriptHost;
    public RuntimeSandBox SandBox { get; init; } = sandBox;
    public string CallHead { get; init; } = callHead;
    public RuntimeSandBoxStatus Status { get; init; } = sandBox.Status;
    public string ScriptSource => SandBox.Source;
    public string ActionValue => SelfAction.Value;
    public string ActionHead => SelfAction.Head;
    public List<string>? ActionArgs => SelfAction.Args;
    public Dictionary<string, string>? ActionConfigs => SelfAction.Configs;

    // 数据的封装方法
    public bool HasData(string key,bool isGlobal = false)
    {
        if (!isGlobal)
        {
            return SandBox.WorkerData.ContainsKey(key);
        }
        else
        {
            return ScriptHost.GlobalData.ContainsKey(key);
        }
    }
    public object GetData(string key,bool isGlobal = false)
    {
        if (!isGlobal)
        {
            if (SandBox.WorkerData.TryGetValue(key, out var wData))
            {
                return wData;
            }
        }
        else
        {
            if (ScriptHost.GlobalData.TryGetValue(key, out var gData))
            {
                return gData;
            }
        }
 
        return new ScnRuntimeException(ScnScriptExceptionMessage.UnknownVariable + ": " + key);
    }
    public void SetData(string key, object value, bool isGlobal = false)
    {
        if (!isGlobal)
        {
            if (!SandBox.WorkerData.TryAdd(key, value))
            {
                SandBox.WorkerData[key] = value;
            }
        }
        else
        {
            if (!ScriptHost.GlobalData.TryAdd(key, value))
            {
                ScriptHost.GlobalData[key] = value;
            }
        }

    }
    public bool TryGetData(string key, [MaybeNullWhen(false)]  out object value, bool isGlobal = false)
    {
        if (!isGlobal)
        {
            return SandBox.WorkerData.TryGetValue(key, out value);
        }
        else
        {
            return ScriptHost.GlobalData.TryGetValue(key, out value);
        }
    }
    public T? GetData<T>(string key, bool isGlobal = false)
    {
        if (!isGlobal)
        {
            if (SandBox.WorkerData.TryGetValue(key, out var wData))
            {
                return (T)wData;
            }
        }
        else
        {
            if (ScriptHost.GlobalData.TryGetValue(key, out var gData))
            {
                return (T)gData;
            }
        }
        return default;
    }
    public void RemoveData(string key, bool isGlobal = false)
    {
        if (!isGlobal)
        {
            SandBox.WorkerData.Remove(key);
        }
        else
        {
            ScriptHost.GlobalData.Remove(key);
        }
    }
    public Dictionary<string, object> GetAllData(bool isGlobal = false)
    {
        if (!isGlobal)
        {
            return SandBox.WorkerData;
        }
        else
        {
            return ScriptHost.GlobalData;
        }
    }
    /// <summary>
    /// 获取当前局变量
    /// </summary>
    /// <remarks>
    /// Key的输入以!开头将不在前面加入.来索引，比如!xx => xx ，xx => .xx
    /// </remarks>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <returns>是否成功</returns>
    public bool TryGetDataByKey(string key, [MaybeNullWhen(false)] out object value)
    {
        var dic = ScriptHost.GlobalData;
        var addDot = !key.StartsWith("!");
        if (!addDot) key = key.Remove(0, 1);
        // 获取一份备份的选择器
        var sceneSelectors = SandBox.Status.GetSelectorScenes();
        var objectSelectors = SandBox.Status.GetSelectorObjects();
        // 尝试获取场景内当前对象父级属性
        var scenePath = "Root";
        searchKeyStart:
        sceneSelectors.ForEach(scene => scenePath += "." + scene);
        var searchKey = scenePath;
        if(objectSelectors.Count > 0)
        {
            searchKey += "." + string.Join(".", objectSelectors);
        }
        searchKey += addDot ? "." : "";
        searchKey += key;
        Console.LogDebug($"正在尝试寻找Key值为 {searchKey} 的 Workerdata 项");
        if (HasData(searchKey))
        {
            value = GetData(searchKey);
            return true;
        }
        // 如果没有找到，查看是否具有对象选择，否则尝试移除场景选择
        if (objectSelectors.Count > 0)
        {
            objectSelectors.RemoveAt(objectSelectors.Count - 1);
        }
        else
        {
            if(sceneSelectors.Count > 0)
            {
                sceneSelectors.RemoveAt(sceneSelectors.Count - 1);
            }
            else
            {
                // 这里已经没有对象和场景选择，也没有找到对应数据，直接返回false
                value = null;
                return false;
            }
        }
        goto searchKeyStart;
    }
    /// <summary>
    /// 设置变量
    /// </summary>
    /// <remarks>
    /// Key的输入以!开头将不在前面加入.来索引，
    /// 比如!xx => xx ，xx => .xx
    /// </remarks>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="isGlobal">是否是全局数据</param>
    /// <returns>是否成功</returns>
    public bool SetDataByKey(string key, object value,bool isGlobal = false)
    {
        var dic = isGlobal ? ScriptHost.GlobalData : SandBox.WorkerData;
        var addDot = !key.StartsWith("!");
        if (!addDot) key = key.Remove(0, 1);
        var searchKey=string.Empty;
        searchKey += isGlobal ? "" : SandBox.Status.GetSelector(false);
        searchKey += addDot ? "." : "";
        searchKey += key;
        Console.LogDebug($"正在尝试设置Key值为 {searchKey} 的项");
        dic[searchKey] = value;
        return true;
    }

    // 通用处理的封装方法
    /// <summary>
    /// 通过Action属性尝试保存数据
    /// </summary>
    /// <remarks>
    /// 在命令中可以通过saveas，global来指定保存的键名和是否是全局数据
    /// </remarks>
    /// <param name="data">保存的数据</param>
    /// <returns>是否成功</returns>
    public bool SaveAsData(object? data)
    {
        var configs = SelfAction.Configs;
        if(configs is null) return false;
        if (configs.TryGetValue("saveas", out var saveas))
        {
            var valueName = GetString(saveas);
            if (!string.IsNullOrEmpty(valueName))
            {
                var isGlobal = false;
                if (configs.TryGetValue("global", out var global))
                {
                    if(!bool.TryParse(global, out isGlobal))
                    {
                        throw new ScnRuntimeException("global属性不是bool类型!");
                    }
                }
                if (data is not null) SetDataByKey(valueName, data, isGlobal);
            }
        }
        return true;
    }

    // 常用获取信息的封装方法
    public string GetActionConfigValue(string key)
    {
        if (SelfAction.Configs != null && SelfAction.Configs.TryGetValue(key, out var value))
        {
            return value;
        }
        throw new ScnRuntimeException(ScnScriptExceptionMessage.UnknownProperty + ": " + key);
    }
    public bool GetActionConfigValue(string key,out string value)
    {
        //string? temp = string.Empty;
        if (SelfAction.Configs != null && SelfAction.Configs.TryGetValue(key, out var temp))
        {
            value = temp;
            return true;
        }
        value = string.Empty;
        return false;
    }
    public string GetString(string content,bool trimStringSymbol=true)
    {
        var result = content;
        if (trimStringSymbol)
        {
            if(result.StartsWith('\"') && result.EndsWith('\"'))
            {
                result = result.Substring(1, result.Length - 2);
            }
        }
        return System.Text.RegularExpressions.Regex.Unescape(result);
    }
    public T? GetSelectorObject<T>() where T : ScriptObjectClass
    {
        var key = Status.GetSelector(false);
        return HasData(key) ? (T)GetData(key) : null;
    }
    //映射沙盒调用方法
    public object? CallNode(string nodeName) => SandBox.CallNode(nodeName);
    public object? CallScene(string sceneName) => SandBox.CallScene(sceneName);
    public bool IsValidNodeOrScene(string nodeOrSceneName) => SandBox.IsValidNodeOrScene(nodeOrSceneName);
    public bool IsFalseNodeTrueScene(string nodeOrSceneName) => SandBox.IsFalseNodeTrueScene(nodeOrSceneName);
    
}
