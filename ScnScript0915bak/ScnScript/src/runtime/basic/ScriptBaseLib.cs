
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace ScnScript.Runtime.Basic;

[Script(ScriptServicesType.Provider, "ScnScriptBase")]
[ScriptClass("this")]
public class ScriptBaseLib : ScriptProvider
{

    public override void OnLoad(ScnScriptHost host)
    {
        host.RegisterObjectType<ScriptObject>();
    }
    // 测试
    /// <summary>
    /// 测试：显示欢迎信息和文件来源
    /// </summary>
    [ScriptFunc(ActionType.Command, "test")]
    public static void Test(ScriptFuncCallData data)
    {
        Console.WriteLine("欢迎使用 ScnScript!");
        Console.WriteLine(data.ScriptSource);
    }
    
    // 常用字符串输出相关命令
    /// <summary>
    /// 字符串监听
    /// </summary>
    /// <remarks>之后应该传递到父对象上</remarks>
    [ScriptFunc(ActionType.String, "")]
    public static void StringMonitor(ScriptFuncCallData data)
    {
        var obj = data.GetSelectorObject<ScriptObjectClass>();
        var content = data.ActionValue;
        if (obj is not null)
        {
            obj.HandleObjectEvent(content, new ObjectEventArgs(obj.GetType(), data, ObjectEventType.ObjectContentChanged, obj.Name));
        }
        else
        {
            Console.Write(content);
        }
        
        
    }
    /// <summary>
    /// 等待
    /// </summary>
    [ScriptFunc(ActionType.Command, "w",["wait"])]
    public static void Wait(ScriptFuncCallData data)
    {
        var sb = data.SandBox;
        var time = data.ActionValue;
        if (!string.IsNullOrEmpty(time))
        {
            var timeInt = float.Parse(time) *1000;
            Console.LogInfo("等待" + time + "s");
            Task.Delay((int)timeInt).Wait();
            Console.LogInfo("等待结束");
            return;
        }

        Console.Read();
    }
    /// <summary>
    /// 读取一行
    /// </summary>
    /// <returns>读取结果</returns>
    [ScriptFunc(ActionType.Command, "rl",["readline","ReadLine"])]
    public static string? ReadLine(ScriptFuncCallData data)
    {
        var result = Console.ReadLine();
        if(string.IsNullOrEmpty(result)) return null;
        return result;
    }
    /// <summary>
    /// 输入换行
    /// </summary>
    [ScriptFunc(ActionType.Command, "n")]
    public static void NewLine(ScriptFuncCallData data)
    {
        var obj = data.GetSelectorObject<ScriptObjectClass>();
        if (obj is not null)
        {
            obj.HandleObjectEvent(Environment.NewLine, new ObjectEventArgs(obj.GetType(), data, ObjectEventType.ObjectContentChanged, obj.Name));
        }
        else
        {
            Console.Write(Environment.NewLine);
        }
        
    }
    /// <summary>
    /// 继续：之后会有用的，自动等待功能还没有做
    /// </summary>
    [ScriptFunc(ActionType.Command, "c")]
    public static void Continue(ScriptFuncCallData data)
    {
        NewLine(data);
    }

    // 对象选择器相关命令
    /// <summary>
    /// 创建对象
    /// </summary>
    /// <exception cref="ScnRuntimeException">创建失败</exception>
    [ScriptFunc(ActionType.Object,"")]
    public static void ObjectCreator(ScriptFuncCallData data)
    {
        RuntimeSandBoxStatus status = data.Status;
        var objName = data.ActionHead;
        ScnScriptHost host = data.ScriptHost;
        if (!string.IsNullOrEmpty(objName))
        {
            var name = objName;
            // 1. 判断当前选中对象是否具有Unsub属性
            var parent = data.GetSelectorObject<ScriptObjectClass>();
            if (parent is not null)
            {
                if (parent.IsSubSelectionDisabled)
                {
                    status.DeselectObject(data);
                }
            }

            // 2. 决定类型

            // 2.1 尝试获取类型配置
            var haveTypeConfig = data.GetActionConfigValue("gettype", out string type);

            if (haveTypeConfig && !host.IsObjectType(type))
            {
                throw new ScnRuntimeException("操作失败！类型不存在！");
            }

            // 2.2 没有类型配置，则默认为对象名称
            if (!haveTypeConfig)
            {
                type = objName;
                if (!host.IsObjectType(type))
                {
                    //自动处理类型
                    var typeName = host.TryGetObjectTypeByProgressAutoType(data);
                    if (string.IsNullOrEmpty(typeName))
                    {
                        //判断是否有该名称的对象
                        if(data.TryGetDataByKey(objName,out object? value))
                        {
                            if(value is not null)
                            {
                                var xobj = value as ScriptObjectClass;
                                if (xobj is not null)
                                {
                                    // 等等等等，没有完成
                                    status.SelectObject(data, objName, 1);
                                    data.SetData(status.GetSelector(false), xobj);
                                    xobj.HandleObjectEvent(data.SandBox, new ObjectEventArgs(xobj.GetType(), data, ObjectEventType.ObjectOnSelected, objName));
                                }
                            }
                        }
                        else
                        {
                            type = "object"; // 如果没有该类型，则默认为 object
                        }
                        
                    }
                    else
                    {
                        type = typeName;
                        haveTypeConfig = true;
                    }
                    
                }
            }

            // 3. 设置对象名称
            // 3.1 尝试获取名称配置
            if (data.GetActionConfigValue("name", out var configName))
            {
                name = data.GetString(configName);
                objName = data.GetString(configName);

                if (parent is not null)
                {
                    if (parent.Name == objName)
                    {
                        throw new ScnRuntimeException("操作失败！对象名称不能与父对象名称相同！");
                    }
                }
            }
            else
            {
                if (!haveTypeConfig)
                {
                    // 3.2 如果没有名称配置，则尝试获取对象名称
                    // 同类型名称计数器
                    var sameKeyName = string.Empty;
                    if (status.IsSelectAnyObject())
                    {
                        sameKeyName = $"{status.GetSelector(false)}?{type}$countSameName";
                    }
                    else
                    {
                        sameKeyName = $"{status.GetSelectorScene()}?{type}$countSameName";
                    }
                    var count = data.GetData(sameKeyName) is int ? (int)data.GetData(sameKeyName) + 1 : 1;
                    objName = objName + count;
                    name = objName;
                    data.SetData(sameKeyName, count);
                }
                
            }

            // 4. 创建对象
            ScriptObjectClass? obj = host.CreateObject(type);
            // 5. 设置对象名称属性
            obj.Name = name;
            // 6. 发送事件
            obj.HandleObjectEvent(data.SandBox, new ObjectEventArgs(obj.GetType(), data, ObjectEventType.ObjectOnCreated, name));

            // 7. 选择对象
            var isComplete = status.SelectObject(data, objName, 1);
            if (obj is not null)
            {
                data.SetData(status.GetSelector(false), obj);
                obj.HandleObjectEvent(data.SandBox, new ObjectEventArgs(obj.GetType(), data, ObjectEventType.ObjectOnSelected, objName));
            }

            if (!isComplete) throw new ScnRuntimeException("操作失败！所在位置不能选择或者存在非法字符！");

        }
        else
        {
            throw new ScnRuntimeException("操作失败！对象名称不能为空！");
        }
    }
    /// <summary>
    /// 调用节点或场景
    /// </summary>
    /// <exception cref="ScnRuntimeException">调用失败</exception>
    [ScriptFunc(ActionType.Object, "call")]
    public static object? CallCommand(ScriptFuncCallData data)
    {
        object? result;
        var args = data.ActionArgs;
        if (args is null) throw new ScnRuntimeException("没有可供返回的数据");
        if (args.Count < 0) throw new ScnRuntimeException("没有可供返回的数据");
        var name=args[0];
        if (name.StartsWith('*'))
        {
            name = name.Substring(1);
            if (data.IsFalseNodeTrueScene(name))
            {
                result=data.CallScene(name);
            }
            else
            {
                result=data.CallNode(name);
            }

            return result;

        }
        else
        {
            throw new ScnRuntimeException("调用失败！需要以*开头的名称，请检查您的语法！");
        }

    }
    /// <summary>
    /// 取消选择
    /// </summary>
    /// <exception cref="ScnRuntimeException">操作失败！对象不存在或者存在非法字符！</exception>
    [ScriptFunc(ActionType.Object, "clr")]
    public static void Clr(ScriptFuncCallData data)
    {
        // 清除选择器之前判断对象是否创建完成，创建完成即发送创建完成事件。
        RuntimeSandBoxStatus status=data.Status;
        var isComplete = false;
        var host = data.ScriptHost;
        var args = data.ActionArgs;
        var clrObjName = string.Empty;
        if (args is not null && args.Count > 0)
        {
            clrObjName = args[0];
        }
        
        if (!string.IsNullOrEmpty(clrObjName))
        {
            isComplete = status.DeselectObject(data,clrObjName,1);
        }
        else
        {
            isComplete = status.DeselectObject(data,null,1);
            //obj?.HandleObjectEvent(data.SandBox, new ObjectEventArgs(obj.GetType(), data, ObjectEventType.ObjectOnDeselected, objName)); 无需该行，在status.DeselectObject内部已经处理

        }

        if (!isComplete) throw new ScnRuntimeException("操作失败！对象不存在或者存在非法字符！请注意不能使用类型名称，类型创建如果没有自行定义名称会被计数器影响！ "+clrObjName);
    }
    /// <summary>
    /// 选择或取消选择对象
    /// </summary>
    /// <exception cref="ScnRuntimeException">操作失败</exception>
    [ScriptFunc(ActionType.Object, "Selector")]
    public static void Selector(ScriptFuncCallData data)
    {
        RuntimeSandBoxStatus status = data.Status;
        var objName = data.ActionValue;
        if (!string.IsNullOrEmpty(objName))
        {
            var isComplete = false;
            var frequency = 1;
            int.TryParse(data.GetActionConfigValue("Frequency"), out frequency);

            switch (data.GetActionConfigValue("Mode"))
            {
                case "Plus":
                    isComplete = status.SelectObject(data,objName, frequency);
                    break;
                case "Minus":
                    isComplete = status.DeselectObject(data,objName, frequency);
                    break;
                default:
                    throw new ScnRuntimeException("操作失败！未知的操作模式！");
            }
            if(!isComplete) throw new ScnRuntimeException("操作失败！对象不存在或者存在非法字符！请检查所处位置是否允许进行此操作！"+objName);

            
        }
    }

    // 常用操作流程以及设置的命令
    /// <summary>
    /// 阻止子对象创建
    /// </summary>
    [ScriptFunc(ActionType.Command,"unsub")]
    public static void Unsub(ScriptFuncCallData data)
    {
        var status = data.Status;
        var keyName = status.GetSelector(false);
        var obj = data.GetSelectorObject<ScriptObjectClass>();
        if (obj is null) throw new ScnRuntimeException($"操作失败！当前对象 '{status.GetSelectObjectName()}' 不是 ScriptObjectClass 实例");
        obj.IsSubSelectionDisabled = true;

    }
    /// <summary>
    /// 允许子对象创建
    /// </summary>
    [ScriptFunc(ActionType.Command, "sub")]
    public static void sub(ScriptFuncCallData data)
    {
        var status = data.Status;
        var obj = data.GetSelectorObject<ScriptObjectClass>();
        if (obj is null) throw new ScnRuntimeException($"操作失败！当前对象 '{status.GetSelectObjectName()}' 不是 ScriptObjectClass 实例");
        obj.IsSubSelectionDisabled = false;
    }
    /// <summary>
    /// 返回
    /// </summary>
    [ScriptFunc(ActionType.Command, "return")]
    public static void Return(ScriptFuncCallData data)
    {
        var config = data.ActionConfigs;
        if(config is null) throw new ScnRuntimeException("返回命令必须包含返回值！");
        var result = config["Value"];
        if (string.IsNullOrEmpty(result))
        {
            if (data.HasData(data.SandBox.wd_ResultKey))
            {
                data.SandBox.WorkerData.Remove(data.SandBox.wd_ResultKey);
            }
        }
        else
        {
            // 如果不是字符串，则尝试从数据中获取
            if (!result.Contains('\"'))
            {
                if (data.TryGetDataByKey(result, out var resultObj))
                {
                    data.SetData(data.SandBox.wd_ResultKey, resultObj);
                }
            }
            else
            {
                data.SetData(data.SandBox.wd_ResultKey, result);
            }
            
        }
        data.Status.IsReturn = true;
    }
    /// <summary>
    /// 输出
    /// </summary>
    [ScriptFunc(ActionType.Command, "out")]
    public static void Out(ScriptFuncCallData data)
    {
        var searchKey = data.ActionValue;
        var obj = data.GetSelectorObject<ScriptObjectClass>();
        object? result=null;
        if (!string.IsNullOrEmpty(searchKey))
        {
            
            if(data.TryGetDataByKey(searchKey,out result))
            {
                if(result is not null)
                {
                    Console.Write(result.ToString()!);
                }
                else
                {
                    Console.Write("null");
                }
            }
            else
            {
                throw new ScnRuntimeException("输出命令中指定的对象不存在！");
            }

            if (obj is not null && result is not null)
            {
                obj.HandleObjectEvent(result, new ObjectEventArgs(obj.GetType(), data, ObjectEventType.ObjectContentChanged, obj.Name));
            }

        }
    }
    [ScriptFunc(ActionType.Command,"getstruct")]
    public static object? GetStruct(ScriptFuncCallData data)
    {
        var configs = data.ActionConfigs;
        ScnScriptHost host = data.ScriptHost;
        if (configs is null) return null;
        if (configs.TryGetValue("file",out var a))
        {
            if (!string.IsNullOrEmpty(a))
            {
                var folder = Path.GetDirectoryName(data.ScriptSource);
                if(!Directory.Exists(folder)) return null;
                var path = Path.Combine(folder, data.GetString(a)+".scn");
                if (File.Exists(path))
                {
                    if (host.HasSandBox(path))
                    {
                        throw new ScnRuntimeException("获取结构体失败，文件已存在！同一结构体文件应该只能临时使用，数据输出即销毁，不能存在多次重复访问。应该使用数据存储");
                    }
                    else
                    {
                        var sbId = host.CreateSandBox(path);
                        var sandbox = host.GetSandBox(sbId);
                        var structData = sandbox.GetStructData();
                        sandbox.Dispose();
                        host.RemoveSandBox(sbId);
                        if (data.SaveAsData(structData)) return null;
                        return structData;
                    }
                }
                else
                {
                    throw new ScnRuntimeException($"获取结构体失败，文件不存在！ 请检查该文件是否存在或程序具有访问权限! {path}");
                }
            }
        }
        return null;
    }

    // 场景和节点的选择器
    /// <summary>
    /// 选择场景
    /// </summary>
    [ScriptFunc(ActionType.SceneSelector,"Start")]
    public static void SceneStart(ScriptFuncCallData data)
    {
        var status = data.Status;
        var sceneName = data.ActionValue;

        if (status.IsSelectAnyNode())
        {
            if (status.IsSearchingNodeAndScene)
            {
                var taggerOld = (Tagger)data.GetData($"{status.GetSelectorScene()}!*{status.GetSelectorNode()}");
                taggerOld.Point.End = data.SandBox.Index - 1;
            }
            status.DeselectNode();
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            if (status.IsSearchingNodeAndScene)
            {
                var tagger = new Tagger(TaggerType.Scene, new Point2(data.SandBox.Index));
                if(data.ActionConfigs is not null)
                {
                    var configs = data.ActionConfigs;
                    if(configs is not null)
                    if(configs.TryGetValue("DisplayName", out var dName))
                    {
                        tagger.DisplayName=dName;
                    }
                }
                data.SetData($"{status.GetSelectorScene()}!*{sceneName}", tagger);
            }
            status.SelectScene(sceneName);
        }
    }
    /// <summary>
    /// 结束场景
    /// </summary>
    [ScriptFunc(ActionType.SceneSelector, "End")]
    public static void SceneEnd(ScriptFuncCallData data)
    {
        var status = data.Status;
        var sceneName = data.ActionValue;
        if (!string.IsNullOrEmpty(sceneName))
        {
            if (status.IsSearchingNodeAndScene)
            {
                if (status.IsSelectAnyNode())
                {
                    var taggerOld = (Tagger)data.GetData($"{status.GetSelectorScene()}!*{status.GetSelectorNode()}");
                    taggerOld.Point.End = data.SandBox.Index - 1;
                    status.DeselectNode();
                }
                status.DeselectScene(sceneName);
                var tagger = (Tagger)data.GetData($"{status.GetSelectorScene()}!*{sceneName}");
                tagger.Point.End = data.SandBox.Index;
            }
            else
            {
                status.DeselectScene(sceneName);
            }
            
        }
    }
    /// <summary>
    /// 选择节点
    /// </summary>
    [ScriptFunc(ActionType.NodeSelector,"")]
    public static void NodeHandle(ScriptFuncCallData data)
    {
        var status=data.Status;
        var nodeName = data.ActionHead;
        if (!string.IsNullOrEmpty(nodeName))
        {
            // 是否是搜索模式
            if (status.IsSearchingNodeAndScene)
            {
                if (status.IsSelectAnyNode())
                {
                    var oldTagger = (Tagger)data.GetData($"{status.GetSelectorScene()}!*{status.GetSelectorNode()}");
                    oldTagger.Point.End = data.SandBox.Index - 1;
                    status.DeselectNode();
                }
                
                status.SelectNode(nodeName);
                var TaggerKey = $"{status.GetSelectorScene()}!*{nodeName}";
                data.SetData(TaggerKey, new Tagger (TaggerType.Node,new Point2(data.SandBox.Index)));
            }
            else
            {
                if (status.InitMode)
                {
                    if (status.GetSelectorNode() is "init" && nodeName is not "init")
                    {
                        status.DeselectNode();
                        status.InitMode = false;
                        //return;
                    }
                }
                status.DeselectNode();
                status.SelectNode(nodeName);
            }
            
        }
        Console.LogDebug(status.GetSelector());
    }

    [ScriptFunc(ActionType.Command, "debug")]
    public static void DebugHandle(ScriptFuncCallData data)
    {
       //debug break
       Debugger.Break();
    }
    //其他
    public override void OnCall()
    {
        //Console.LogDebug("ScriptLib 被调用了!");
    }
}