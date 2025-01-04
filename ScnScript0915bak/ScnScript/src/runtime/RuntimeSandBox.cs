using ScnScript.Runtime.Basic;
using System.Diagnostics;

namespace ScnScript.Runtime;
/// <summary>
/// 运行时沙盒
/// </summary>
/// <remarks>
/// <para>非常不建议手动操作内部属性，除非你完全理解运行逻辑</para>
/// <para>通常使用<seealso cref="ScnScriptHost"/>进行创建</para>
/// </remarks>
public class RuntimeSandBox : RuntimeModelBase
{
    public string wd_ResultKey => Status.GetSelectorScene()+ "!result";
    private const string node_Main = "main";
    private const string node_Init = "init";
    private const string ex_HostNotFound = "主机对象为空！";
    private const string ex_NodeOrSceneNotFound = "节点或场景不存在！";

    // 私有引用： 必要的基本执行数据
    private ScnScriptHost? host=null;
    private ParserResult parserResult;

    // 构造用
    /// <summary>
    /// 创建沙盒
    /// </summary>
    /// <param name="source">运行来源</param>
    public RuntimeSandBox(string source)
    {
        Source = !string.IsNullOrEmpty(source) ? source : "未知来源";
    }
    /// <summary>
    /// 创建沙盒
    /// </summary>
    /// <param name="host">主机对象</param>
    /// <param name="source">运行来源</param>
    public RuntimeSandBox(ScnScriptHost host, string source)
    {
        SetParent(host);
        Source = !string.IsNullOrEmpty(source) ? source : "未知来源";
    }
    /// <summary>
    /// 设置主机对象
    /// </summary>
    /// <param name="host">主机</param>
    public void SetParent(ScnScriptHost host)
    {
        this.host = host;
    }

    // 公共属性： 沙盒运行时数据
    /// <summary>
    /// 代码来源【不可修改】
    /// </summary>
    public string Source { get;private init; }
    /// <summary>
    /// 【关键】运行索引
    /// </summary>
    /// <remarks>请勿手动操作，这里公开只是为内部的脚本函数扩展方法提供方便</remarks>
    public int Index { get; set; } = 0;
    /// <summary>
    /// 沙盒工作的数据
    /// </summary>
    /// <remarks>
    /// <para>请不要直接手动操作这个数据!</para>
    /// <para>在 <seealso cref="Basic.ScriptFuncCallData"/> 内部具有 GetData,SetData方法</para>
    /// </remarks>
    public Dictionary<string, object> WorkerData { get; set; } = new();
    /// <summary>
    /// 沙盒运行时状态
    /// </summary>
    /// <remarks>如果是扩展函数需要获取，请使用 <seealso cref="Basic.ScriptFuncCallData.GetStatus()"/> 获取</remarks>
    public RuntimeSandBoxStatus Status { get; set; } = new();
    public Position GetPosition ()=> parserResult.Positions[Index];

    public DocumentType DocumentType { get; set; } =DocumentType.Normal;
    // 运行
    /// <summary>
    /// 运行 main 节点
    /// </summary>
    /// <exception cref="ScnRuntimeException">主机对象为空</exception>
    public override void Run()
    {
        if (host == null) throw new ScnRuntimeException(ex_HostNotFound);
        if (Status.IsRunning) return;
        var result=Call(node_Main);
        if(result is not null) Console.LogDebug("获取结果:" + result.ToString());
        Status.IsRunning = false;
        //if (Index != parserResult.Actions.Count) continue;
        Status.IsOver = true;
        Status.IsWaiting = true;
    }
    public object? GetStructData()
    {
        if(DocumentType is not DocumentType.Struct) throw new ScnRuntimeException("请求的文档类型不是结构体");
        Run();
        var key = Status.GetSelectorScene() + ".content";
        if (WorkerData.ContainsKey(key))
        {
            return WorkerData[key];
        }
        return null;
    }
    /// <summary>
    /// 初始化搜索场景与节点的开始与结束位置，运行 init 节点
    /// </summary>
    /// <exception cref="ScnRuntimeException">主机对象为空</exception>
    public void RunInit()
    {
        if (host == null) throw new ScnRuntimeException(ex_HostNotFound);
        Status.InitMode = true;
        SearchNodeAndScene();
        if (IsValidNodeOrScene(node_Init))
        {
            CallNode(node_Init);
        }
    }

    // 标识场景
    /// <summary>
    /// 搜索场景与节点的开始与结束位置
    /// </summary>
    /// <exception cref="ScnRuntimeException">主机对象为空</exception>
    private void SearchNodeAndScene()
    {
        if (host is null) throw new ScnRuntimeException(ex_HostNotFound);
        Status.IsSearchingNodeAndScene = true;
        Index = 0;
        while (Index < parserResult.Actions.Count)
        {
            ScnAction action = parserResult.Actions[Index];
            if(action.Type is ActionType.NodeSelector or ActionType.SceneSelector)
            {
                host.CallFunc(this, action);
            }
            Index++;
        }

        if (Status.IsSelectAnyNode())
        {
            var key = $"{Status.GetSelectorScene()}!*{Status.GetSelectorNode()}";
            // 这里减一是因为前面Index++之后的条件没有达成才可以运行到这里，所以Index一定等于Actions.Count，所以需要-1
            if (WorkerData.TryGetValue(key, out object? taggerObj))
            {
                var tagger = (Tagger)taggerObj;
                tagger.Point.End = Index - 1;
            }
            Status.DeselectNode();
        }
        Status.DeselectAll();
        Index = 0;
        Status.IsSearchingNodeAndScene = false;
    }

    // 场景和节点的常用操作
    /// <summary>
    /// 是不是有效的节点或场景
    /// </summary>
    /// <param name="nodeOrSceneName">节点或场景名称</param>
    /// <returns>是否存在</returns>
    public bool IsValidNodeOrScene(string nodeOrSceneName)
    {
        if (WorkerData.Count == 0)
        {
            return false;
        }

        var searchKey = $"{Status.GetSelectorScene()}!*{nodeOrSceneName}";
        return WorkerData.ContainsKey(searchKey);
    }
    /// <summary>
    /// "否"是节点，"是"是场景
    /// </summary>
    /// <param name="nodeOrSceneName">节点或场景名称</param>
    /// <returns>判断结果</returns>
    /// <exception cref="ScnRuntimeException">节点或场景不存在！</exception>
    public bool IsFalseNodeTrueScene(string nodeOrSceneName)
    {
        if (WorkerData.Count == 0)
        {
            return false;
        }
        var searchKey = $"{Status.GetSelectorScene()}!*{nodeOrSceneName}";
        if (WorkerData.ContainsKey(searchKey))
        {
            var tagger=(Tagger)WorkerData[searchKey];
            switch (tagger.Type)
            {
                case TaggerType.Node:
                    return false;
                case TaggerType.Scene:
                    return true;
                default:
                    throw new ScnRuntimeException(ex_NodeOrSceneNotFound+" "+nodeOrSceneName);
            }
        }
        throw new ScnRuntimeException(ex_NodeOrSceneNotFound + " " + nodeOrSceneName);
    }
    /// <summary>
    /// 设置为节点或场景的起始位置
    /// </summary>
    /// <param name="nodeOrSceneName">节点或场景名称</param>
    /// <returns>是否成功</returns>
    private bool SetStartIndex(string nodeOrSceneName)
    {
        if(WorkerData.Count == 0)
        {
            Index = 0;
            return false;
        }
        var searchKey = $"{Status.GetSelectorScene()}!*{nodeOrSceneName}";
        if (WorkerData.ContainsKey(searchKey))
        {
            var tagger = (Tagger)WorkerData[searchKey];
            Index = tagger.Point.Start;
            Console.LogDebug($"已经将起始位置设为 *{nodeOrSceneName}的位置 {Index}");
            return true;
        }
        Index = 0;
        return false;
    }
    /// <summary>
    /// 设置节点或场景的结束位置
    /// </summary>
    /// <param name="nodeOrSceneName">节点或场景名称</param>
    /// <returns>是否成功</returns>
    private int GetEndIndex(string nodeOrSceneName)
    {
        if (WorkerData.Count == 0)
        {
            return parserResult.Actions.Count - 1;
        }
        var searchKey = $"{Status.GetSelectorScene()}!*{nodeOrSceneName}";
        if (WorkerData.ContainsKey(searchKey))
        {
            var tagger = (Tagger)WorkerData[searchKey];
            return tagger.Point.End;
        }
        return parserResult.Actions.Count - 1;
    }

    // 呼叫
    /// <summary>
    /// 调用节点
    /// </summary>
    /// <param name="nodeName">节点名称</param>
    /// <returns>可能为空的返回结果</returns>
    /// <exception cref="ScnRuntimeException">节点不存在</exception>
    public object? CallNode(string nodeName)
    {
        if (!IsFalseNodeTrueScene(nodeName))
        {
            return Call(nodeName);
        }
        throw new ScnRuntimeException("节点不存在！");
    }
    /// <summary>
    /// 调用场景
    /// </summary>
    /// <param name="nodeName">场景名称</param>
    /// <returns>可能为空的返回结果</returns>
    /// <exception cref="ScnRuntimeException">场景不存在</exception>
    public object? CallScene(string sceneName)
    {
        if (IsFalseNodeTrueScene(sceneName))
        {
            return Call(sceneName);
        }
        throw new ScnRuntimeException("场景不存在！");
    }
    /// <summary>
    /// 调用节点或场景
    /// </summary>
    /// <param name="nodeOrSceneName">节点或场景名称</param>
    /// <returns>可能为空的返回结果</returns>
    /// <exception cref="ScnRuntimeException">场景不存在</exception>
    private object? Call(string nodeOrSceneName)
    {
        if (host == null) throw new ScnRuntimeException(ex_HostNotFound);
        Status.IsWaiting = false;
        Status.IsReturn = false;
        var originIndex = Index;
        Console.LogDebug("已保存原Index: "+originIndex);
        var originSelectorObject = Status.SaveSelectorObject().ToList();
        var originSelectorScene = Status.SaveSelectorScene().ToList();
        var originSelectorNode = Status.GetSelectorNode();


        if (SetStartIndex(nodeOrSceneName))
        {
            var endIndex = GetEndIndex(nodeOrSceneName);
            try
            {
                for (; Index <= endIndex; Index++)
                {
                    Status.IsRunning = true;

                    ScnAction action = parserResult.Actions[Index];

                    if (SkipSubScenesActions(nodeOrSceneName, action)) continue;

                    var successful = host.CallFunc(this, action);

                    if (!successful) Console.LogWarning("欸？遇到不认识的函数了，没人处理了呢。" + Environment.NewLine + action);
                    if (Status.IsReturn)
                    {
                        Status.IsReturn = false;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Position pos = parserResult.Positions[Index];
                var sourceCode = parserResult.SourceCode[Index];

#if DEBUG
                Console.LogWarning("你现在处于调试模式中，Call函数捕捉的异常被触发！");
                Console.LogDebug("注意：在此Break的作用是方便查看内部数据！");
                Console.LogDebug($"脚本运行时错误!{Environment.NewLine}【跟踪信息】{Environment.NewLine}位置：{pos}{Environment.NewLine}源代码：{sourceCode}{Environment.NewLine}来源：{Source}{Environment.NewLine}【错误消息】{Environment.NewLine}{e.Message}");
                Debugger.Break();
#endif

                throw new ScnRuntimeException($"脚本运行时错误!{Environment.NewLine}【跟踪信息】{Environment.NewLine}位置：{pos}{Environment.NewLine}源代码：{sourceCode}{Environment.NewLine}来源：{Source}{Environment.NewLine}【错误消息】{Environment.NewLine}{e.Message}");
            }

            Index = originIndex;
            Status.DeselectAll();
            Console.LogDebug("已还原原Index: " + originIndex);
            Status.LoadSelectorObject(originSelectorObject);
            Status.LoadSelectorScene(originSelectorScene);
            Status.SelectNode(originSelectorNode);
            Console.LogDebug(Status.GetSelector());
            Status.IsRunning = false;

            if (WorkerData.TryGetValue(wd_ResultKey, out var result))
            {
                WorkerData.Remove(wd_ResultKey);
                return result;
            }
            return null;
        }
        throw new ScnRuntimeException($"找不到节点或场景 {nodeOrSceneName}");
    }
    /// <summary>
    /// 跳过子场景和节点的调用
    /// </summary>
    /// <param name="nodeOrSceneName">当前节点或场景名称</param>
    /// <param name="action">Action</param>
    /// <returns>可以跳过</returns>
    private bool SkipSubScenesActions(string nodeOrSceneName,ScnAction action)
    {
        switch (action.Type)
        {
            case ActionType.NodeSelector:
                var nodeName = action.Head;
                if (nodeName != nodeOrSceneName)
                {
                    Index = GetEndIndex(nodeName);
                    return true; 
                }
                break;
            case ActionType.SceneSelector:
                var sceneName = action.Value;
                if (sceneName != nodeOrSceneName)
                {
                    Index = GetEndIndex(sceneName);
                    return true;
                }
                break;
        }
        return false;
    }

    // 加载与回调
    /// <summary>
    /// 加载脚本
    /// </summary>
    /// <param name="script">代码内容</param>
    public void Load(string script)
    {
        script = script.ReplaceLineEndings("\n");
        try
        {
            LexerResult lexerResult = ScnScriptCommon.Lexer(script);
            ParserResult parserResult = ScnScriptCommon.Parser(script, lexerResult);
            this.parserResult = parserResult;
            Status.IsOver = false;
            Index = 0;
            Status.IsWaiting = true;
        }
        catch (Exception e)
        {
            Console.LogFatal(e.Message);
            throw;
        }
    }
    /// <summary>
    /// 回调函数
    /// </summary>
    /// <param name="result">回调结果</param>
    public override void CallBack(object? result)
    {
        if (result is null) return;
        if (!WorkerData.TryAdd(wd_ResultKey, result))
        {
            WorkerData[wd_ResultKey] = result;
        }
        Console.LogDebug("回调完成！函数执行完成结果为：" + result);
    }

    // 其他
    public void Pause()
    {
        Status.IsWaiting = true;
    }
    public override void Stop()
    {
        Status.IsWaiting = true;
        Status.IsRunning = false;
    }
    public override void Clean()
    {
        host = null;
        Index = 0;
        Status.IsWaiting = true;
        Status.DeselectAll();
        WorkerData.Clear();
    }
    public override void Dispose()
    {
        Clean();
        GC.SuppressFinalize(this);
    }
    
}
public enum DocumentType
{
    Normal,
    Struct
}
