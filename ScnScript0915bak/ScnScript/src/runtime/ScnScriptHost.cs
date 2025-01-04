
using System;
using System.Reflection;
using ScnScript.Runtime.Basic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScnScript.Runtime;
/// <summary>
/// 脚本宿主
/// </summary>
public class ScnScriptHost : RuntimeModelBase
{
    // 私有变量： 沙盒列表，主沙盒引用，外部代码索引
    private Dictionary<string,RuntimeSandBox> sandBoxes =new();
    private RuntimeSandBox? mainSandBox;
    private int outsideCodeIndex = 0;

    // 构造方法
    public ScnScriptHost()
    {
        RegisterFuncBook(new ScriptBaseLib());
        RegisterFuncBook(new ScriptScnLib());
    }

    // 公共属性： 全局数据，是否在等待，是否结束
    /// <summary>
    /// 全局数据
    /// </summary>
    public Dictionary<string, object> GlobalData { get; set; } = new();
    /// <summary>
    /// 是否在等待
    /// </summary>
    public bool IsWaiting => mainSandBox?.Status.IsWaiting ?? false;
    /// <summary>
    /// 是否结束
    /// </summary>
    public bool IsOver => mainSandBox?.Status.IsOver ?? false;

    // 私有属性： 扩展提供者（类），函数书
    /// <summary>
    /// 提供者注册列表
    /// </summary>
    private Dictionary<string, ScriptProvider> ProviderBook { get; set; } = new();
    /// <summary>
    /// 函数注册列表
    /// </summary>
    private Dictionary<RuntimeFuncInfo, MethodInfo> FuncBook { get; set; } = new();

    // 创建沙盒的方法
    /// <summary>
    /// 创建沙盒
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns>沙盒Id</returns>
    public string CreateSandBox(string path) => CreateSandBox(File.ReadAllText(path), path);
    /// <summary>
    /// 创建沙盒
    /// </summary>
    /// <remarks>请注意外侧来源不会检测是否重复</remarks>
    /// <param name="sourceCode">源代码</param>
    /// <returns>沙盒Id</returns>
    public string CreateSandBoxByCode(string sourceCode)
    {
        var key = CreateSandBox(sourceCode, $"<- 外部代码 编号{outsideCodeIndex} ->");
        outsideCodeIndex++;
        return key;
    }
    /// <summary>
    /// 创建沙盒
    /// </summary>
    /// <param name="sourceCode">源代码</param>
    /// <param name="source">来源</param>
    /// <returns>沙盒Id</returns>
    /// <exception cref="ScnRuntimeException">创建失败</exception>
    public string CreateSandBox(string sourceCode, string source)
    {
        try
        {
            var sb = new RuntimeSandBox(this, source);
            if (sandBoxes.ContainsKey(source)) throw new ScnRuntimeException("创建失败，已存在相同来源的沙盒！");
            sandBoxes.Add(source, sb);
            if (mainSandBox == null)
            {
                mainSandBox = sb;
            }
            sb.Load(sourceCode);
            sb.RunInit();
        }
        catch (Exception e)
        {
            throw new ScnRuntimeException($"初始化沙盒失败！{Environment.NewLine}来源: {source} {Environment.NewLine}原因如下:{Environment.NewLine}{e.Message}");
        }
        return source;
    }

    // 是否存在沙盒
    /// <summary>
    /// 是否存在沙盒
    /// </summary>
    public bool HasSandBox(string source) => sandBoxes.ContainsKey(source);

    // 移除和获取沙盒
    /// <summary>
    /// 移除沙盒
    /// </summary>
    /// <param name="source">沙盒Id（来源）</param>
    /// <exception cref="ScnRuntimeException">不存在</exception>
    public void RemoveSandBox(string source)
    {
        if (sandBoxes.TryGetValue(source, out RuntimeSandBox? sb))
        {
            sb.Dispose();
        }
        else
        {
            throw new ScnRuntimeException("移除失败，未找到指定沙盒！");
        }
    }
    /// <summary>
    /// 获取沙盒
    /// </summary>
    /// <param name="source">沙盒Id（来源）</param>
    /// <returns>沙盒</returns>
    /// <exception cref="ScnRuntimeException">不存在</exception>
    public RuntimeSandBox GetSandBox(string source)
    {
        if (sandBoxes.TryGetValue(source, out RuntimeSandBox? value))
        {
            return value;
        }
        else
        {
            throw new ScnRuntimeException("获取失败，未找到指定沙盒！");
        }
    }
    // 执行的核心逻辑： 注册函数，调用方法
    /// <summary>
    /// 注册函数
    /// </summary>
    /// <param name="funcProvider">函数提供类</param>
    /// <exception cref="ScnRuntimeException">注册失败</exception>
    public void RegisterFuncBook(ScriptProvider funcProvider)
    {
        // 注册提供者
        Type info = funcProvider.GetType();
        var attributes=info.GetCustomAttributes(true);
        var isValid = false;
        foreach (var attribute in attributes)
        {
            if (attribute is not ScriptAttribute script) continue;

            var head = script.Name +"!"+ script.Type;
            if (!ProviderBook.TryAdd(head, funcProvider))
            {
                throw new ScnRuntimeException($"函数注册失败！函数名 '{head}' 已存在！");
            }
            isValid = true;
        }
        if (!isValid) throw new ScnRuntimeException($"函数注册失败！函数 '{info.Name}' 没有标记为脚本函数！");

        funcProvider.OnLoad(this);
        //注册函数书
        var functions = info.GetMethods();
        ScriptClassAttribute? classAttribute = null;

        // 获取类的特性
        var className = string.Empty;
        foreach (var attribute in attributes)
        {
            switch (attribute)
            {
                case ScriptClassAttribute scriptClassAttribute:
                    classAttribute = scriptClassAttribute;
                    className = scriptClassAttribute.Name;
                    break;
            }
        }

        foreach (MethodInfo oneFunction in functions)
        {
            var objects = oneFunction.GetCustomAttributes(true);
            foreach (var obj in objects)
            {
                if (obj is not ScriptFuncAttribute funcAttribute) continue;
                FuncBook.Add(new RuntimeFuncInfo
                {
                    Provider = funcProvider,
                    ClassName = className,
                    Head = funcAttribute.Head,
                    Type = funcAttribute.Type,
                    AkaNames = funcAttribute.AkaNames,
                }, oneFunction);
            }
        }

    }
    /// <summary>
    /// 调用函数
    /// </summary>
    /// <param name="sandBox">沙盒</param>
    /// <param name="action">Action</param>
    /// <returns>是否成功</returns>
    /// <exception cref="ScnRuntimeException">运行出现错误</exception>
    public bool CallFunc(RuntimeSandBox sandBox,ScnAction action)
    {
        if (ProviderBook.Count is 0) throw new ScnRuntimeException("函数调用失败！提供者表为空！");
        // 监听思路： 记录监听函数，如果后面有匹配的实际函数，则运行实际函数，否则运行监听函数
        MethodInfo? listenerInfo = null;
        ScriptProvider? listenerProvider = null;
        // 可共用变量
        var isValid = false;
        var head = string.Empty;
        foreach (var func in FuncBook)
        {
            head = $"[{sandBox.Source}?{sandBox.Index}@{sandBox.GetPosition()}]{func.Key.ClassName}!{func.Value.Name}!{func.Key.Type}";

            // 查找可用的函数
            if (func.Key.Type != action.Type) continue;
            
            // 头名称相同
            if (func.Key.Head == action.Head)
            {
                isValid = true;
            }
            else
            {
                // 别名相同
                if (action.Type is not ActionType.String && func.Key.AkaNames is not null && func.Key.AkaNames.Contains(action.Head))
                {
                    isValid = true;
                }
                else
                {
                    //不是命令类型且名称为空，即提供监听运行
                    if (action.Type is not ActionType.Command)
                    {
                        if (string.IsNullOrEmpty(func.Key.Head))
                        {
                            // 监听函数
                            if (listenerInfo is null)
                            {
                                listenerInfo = func.Value;
                                listenerProvider = func.Key.Provider;
                            }
                            else
                            {
                                throw new ScnRuntimeException("不应该存在多个监听同一类型Action的函数!");
                            }

                            continue;
                        }
                    }
                }
            }

            // 判断是否 head 为类名
            if (action.Type is not ActionType.String && func.Key.ClassName == action.Head)
            {
                if (func.Key.Head == action.Value)
                {
                    isValid = true;
                }
            }

            if (!isValid) continue;

            CallCommand(sandBox, action, func.Key.Provider, head, func.Value);
            return true;
        }

        if (!isValid)
        {
            // 若果有监听函数，则调用监听函数（证明没有对应实际函数）
            if (listenerInfo is not null && listenerProvider is not null)
            {
                CallCommand(sandBox, action, listenerProvider, head, listenerInfo);
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// 调用方法
    /// </summary>
    /// <param name="sandBox">沙盒</param>
    /// <param name="action">Action</param>
    /// <param name="provider">提供者</param>
    /// <param name="head">调用头</param>
    /// <param name="mInfo">方法信息</param>
    /// <exception cref="ScnRuntimeException">运行异常</exception>
    private void CallCommand(RuntimeSandBox sandBox, ScnAction action, ScriptProvider provider, string head, MethodInfo mInfo)
    {
        var parameters = mInfo.GetParameters();
        if (parameters.Length >= 1 && parameters[0].ParameterType != typeof(ScriptFuncCallData))
        {
            throw new ScnRuntimeException("调用时发现名为 '" + mInfo.Name + "' 的函数参数不正确!" + Environment.NewLine + "函数参数必须只能接收 'ScriptFuncCallData data' 一项参数");
        }
        
        provider.OnCall();

        object? result;
        try
        {
            // 调用方法无参数
            if (parameters.Length is 0)
            {
                result = mInfo.Invoke(provider, null);
            }
            else
            {
                // 创建参数数组
                object?[] parameter =
               [
                   new ScriptFuncCallData(selfAction: action, scriptHost: this, sandBox: sandBox, callHead: head)
               ];

                // 调用方法带参数
                result = mInfo.Invoke(provider, parameter);
            }
            sandBox.CallBack(result);
        }
        catch(Exception ex)
        {
            if(ex.InnerException is not null) throw ex.InnerException;
            throw new ScnRuntimeException("调用函数 '" + mInfo.Name + "' 时发生出乎意料的非内部错误：" + ex.Message);
        }
        
    }

    /// <summary>
    /// 对象类型注册列表
    /// </summary>
    private Dictionary<ScriptObjectAttribute, Type> ObjectTypeBook { get; set; } = new();
    /// <summary>
    /// 注册对象类型
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public void RegisterObjectType<T>() where T : ScriptObjectClass
    {
        var type = typeof(T);
        var attr = type.GetCustomAttribute<ScriptObjectAttribute>();
        if (attr is not null)
        {
            if (ObjectTypeBook.TryAdd(attr, type))
            {
                Console.LogInfo("已注册对象类型 '" + attr.Name + "'");
            }
            else
            {
                Console.LogWarning("对象类型 '" + attr.Name + "' 已存在，无法重复注册！");
            }
        }
        else
        {
            throw new ScnRuntimeException("对象类型 '" + type.Name + "' 未设置 ScriptObjectAttribute 属性，无法注册！");
        }
    }
    /// <summary>
    /// 获取实例化的对象
    /// </summary>
    /// <param name="name">类型名称</param>
    /// <returns>实例化对象</returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ScnRuntimeException"></exception>
    public ScriptObjectClass CreateObject(string name)
    {
        foreach (var item in ObjectTypeBook)
        {
            var isValid = false;
            if (item.Key.Name == name)
            {
                isValid = true;
            }
            if (item.Key.AkaNames is not null)
            {
                if (item.Key.AkaNames.Contains(name))
                {
                    isValid = true;
                }
            }
            if (isValid)
            {
                var obj = Activator.CreateInstance(item.Value);
                if (obj is null) throw new InvalidOperationException($"无法实例化对象 '{name}'");
                if (obj is not ScriptObjectClass) throw new InvalidOperationException($"对象 '{name}' 不是 ScriptObjectClass 类型");
                return (ScriptObjectClass)obj;
            }
        }
        throw new ScnRuntimeException("未找到对象类型 '" + name + "'");
    }
    /// <summary>
    /// 判断对象类型是否存在
    /// </summary>
    /// <param name="name">类型名称</param>
    public bool IsObjectType(string name)
    {
        foreach (var item in ObjectTypeBook)
        {
            if (item.Key.Name == name)
            {
                return true;
            }
            if (item.Key.AkaNames is not null)
            {
                if (item.Key.AkaNames.Contains(name))
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 获取对象类型(使用对象类型的ProgressAutoType推断)
    /// </summary>
    /// <param name="data">调用数据</param>
    /// <returns>对象名称(可能为空)</returns>
    public string? TryGetObjectTypeByProgressAutoType(ScriptFuncCallData data)
    {
        foreach (var item in ObjectTypeBook)
        {
            var obj = Activator.CreateInstance(item.Value) as ScriptObjectClass;
            if (obj is not null)
            {
                if (obj.ProgressAutoType(data))
                {
                    return item.Key.Name;
                }
            }
        }
        return null;
    }
    // 运行沙盒
    /// <summary>
    /// 运行主沙盒
    /// </summary>
    /// <exception cref="ScnRuntimeException">运行失败</exception>
    public override void Run()
    {
        if(mainSandBox != null)
        {
            mainSandBox.Run();
        }
        else
        {
            throw new ScnRuntimeException("需要创建一个主沙盒(即第一个沙盒)！");
        }
    }
    /// <summary>
    /// 运行指定沙盒
    /// </summary>
    /// <param name="source">沙盒Id</param>
    /// <exception cref="ScnRuntimeException">运行失败</exception>
    public void Run(string source)
    {
        if (sandBoxes.TryGetValue(source, out RuntimeSandBox? sb))
        {
            sb.Run();
        }
        else
        {
            throw new ScnRuntimeException("运行失败，未找到指定沙盒！");
        }
    }

    // 终止运行
    public override void Stop()
    {
        foreach (RuntimeSandBox sandBox in sandBoxes.Values)
        {
            sandBox.Stop();
        }
    }
    public void Stop(string source)
    {
        if (sandBoxes.TryGetValue(source, out RuntimeSandBox? sb))
        {
            sb.Stop();
        }
        else
        {
            throw new ScnRuntimeException("停止失败，未找到指定沙盒！");
        }
    }

    // 其他
    public override void Clean()
    {
        foreach (RuntimeSandBox sandbox in sandBoxes.Values)
        {
            sandbox.Clean();
        }
        sandBoxes.Clear();
        GlobalData.Clear();
    }
    public override void Dispose()
    {

        Clean();
        foreach (RuntimeSandBox sandbox in sandBoxes.Values)
        {
            sandbox.Dispose();
        }
        FuncBook.Clear();
        ProviderBook.Clear();
        GC.SuppressFinalize(this);
    }
}
