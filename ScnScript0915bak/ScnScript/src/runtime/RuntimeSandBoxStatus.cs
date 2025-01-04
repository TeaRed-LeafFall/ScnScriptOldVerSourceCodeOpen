using ScnScript.Runtime.Basic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ScnScript.Runtime;
/// <summary>
/// 运行时沙盒状态
/// </summary>
/// <remarks>
/// <para>如果函数标记【仅内部使用】，请尽量不要使用</para>
/// <para>这些内部逻辑通常不需要修改，非常不建议自行操作</para>
/// <para>除非你知道你在做什么，明白其后果</para>
/// <para>否则请勿修改</para>
/// </remarks>
public class RuntimeSandBoxStatus
{
    // 私有选择器变量
    private List<string> SelectorObject = new();
    private List<string> SelectorScene = new();
    private string SelectorNode = string.Empty;

    // 状态变量
    /// <summary>
    /// 是否运行结束
    /// </summary>
    public bool IsOver = false;
    /// <summary>
    /// 是否处于等待状态
    /// </summary>
    public bool IsWaiting = false;
    /// <summary>
    /// 是否正在运行
    /// </summary>
    public bool IsRunning = false;
    /// <summary>
    /// 是否返回
    /// </summary>
    public bool IsReturn = false;
    /// <summary>
    /// 【仅内部使用】初始化模式（请仅 init 节点使用，其他情况不需要修改）
    /// </summary>
    public bool InitMode = true;
    /// <summary>
    /// 【仅内部使用】是否在搜索节点和场景（仅在初始化时用得到，其他情况不需要修改）
    /// </summary>
    public bool IsSearchingNodeAndScene = false;

    // 获取
    /// <summary>
    /// 获取对象选择器状态
    /// </summary>
    /// <returns>对象选择器状态</returns>
    public string GetSelectorObject()
    {
        if(SelectorObject.Count is 0) return "$<NULL>";
        return string.Join(".", SelectorObject);
    }
    /// <summary>
    /// 获取场景选择器状态
    /// </summary>
    /// <returns>场景选择器状态</returns>
    public string GetSelectorScene()
    {
        var path = "Root";
        SelectorScene.ForEach(scene => path += "." + scene);
        return path;
    }
    /// <summary>
    /// 获取节点选择器状态
    /// </summary>
    /// <returns>节点选择器状态</returns>
    public string GetSelectorNode()
    {
        return SelectorNode;
    }
    /// <summary>
    /// 获取选择器状态
    /// </summary>
    /// <returns>选择器状态</returns>
    public string GetSelector(bool needNode=true)
    {
        if (string.IsNullOrEmpty(GetSelectorNode()) || !needNode)
        {
            if (IsSelectAnyObject())
            {
                return $"{GetSelectorScene()}.{GetSelectorObject()}";
            }
            return $"{GetSelectorScene()}";
        }
        if (IsSelectAnyObject())
        {
            return $"{GetSelectorScene()}.{GetSelectorObject()} [*{GetSelectorNode()}]";
        }
        return $"{GetSelectorScene()} [*{GetSelectorNode()}]";
    }
    public string GetSelectObjectName()
    {
        return SelectorObject[^1];
    }

    // 内部使用
    /// <summary>
    /// 【内部】获取对象选择器
    /// </summary>
    /// <returns>复制的对象列表</returns>
    internal List<string> GetSelectorObjects() => new List<string>(SelectorObject);
    /// <summary>
    /// 【内部】获取场景选择器
    /// </summary>
    /// <returns>复制的场景列表</returns>
    internal List<string> GetSelectorScenes() => new List<string>(SelectorScene);

    // 选择
    /// <summary>
    /// 选择对象
    /// </summary>
    /// <param name="data">函数调用数据</param>
    /// <param name="name">对象名称</param>
    /// <param name="frequency">次数</param>
    /// <param name="isTypeName">是否是类型</param>
    /// <returns>是否成功</returns>
    public bool SelectObject(ScriptFuncCallData data,string name, int frequency = 1)
    {
        if (string.IsNullOrEmpty(name) || !ScnScriptCommon.IsValidString(name)) return false;
        for (int i = 0; i < frequency; i++)
        {
            var obj = data.GetSelectorObject<ScriptObjectClass>();
            if (obj is not null)
            {
                if (obj.IsSubSelectionDisabled) return false;
                if (obj.CannotSelect) return false;
                obj.HandleObjectEvent(this, new ObjectEventArgs(obj.GetType(), data, ObjectEventType.ObjectOnSelected, name));
            }

            if (data.ScriptHost.IsObjectType(name)) throw new ScnRuntimeException(name+" 是类型,不能被选择!");

            SelectorObject.Add(name);
            Console.LogDebug("当前选择器: "+GetSelector());

        }
        return true;
    }

    /// <summary>
    /// 选择场景
    /// </summary>
    /// <param name="name">场景名称</param>
    /// <returns>是否成功</returns>
    public bool SelectScene(string name)
    {
        if (string.IsNullOrEmpty(name) || !ScnScriptCommon.IsValidString(name)) return false;
        SelectorScene.Add(name);
        return true;
    }
    /// <summary>
    /// 选择节点
    /// </summary>
    /// <param name="name">节点名称</param>
    /// <returns>是否成功</returns>
    public bool SelectNode(string name)
    {
        if (string.IsNullOrEmpty(name) || !ScnScriptCommon.IsValidString(name)) return false;
        SelectorNode = name;
        return true;
    }

    // 取消
    /// <summary>
    /// 取消选择对象
    /// </summary>
    /// <param name="objectName">对象名称</param>
    /// <param name="removeFrequency">次数</param>
    /// <returns>是否成功</returns>
    public bool DeselectObject(ScriptFuncCallData data,string? objectName=null,int removeFrequency=1)
    {
        // 如果SelectorObject为空或者移除频率小于1，则返回 false
        if (SelectorObject.Count is 0 || removeFrequency < 1) return false;

        // 记录需要移除的次数
        int frequencyToRemove = removeFrequency;

        // 从列表末尾开始遍历
        for (int i = SelectorObject.Count - 1; i >= 0 && frequencyToRemove > 0; i--)
        {
            string currentObjectName = SelectorObject[i];

            // 发送取消选择事件
            var obj = data.GetSelectorObject<ScriptObjectClass>();
            if (obj is not null)
            {
                obj.HandleObjectEvent(this, new ObjectEventArgs(obj.GetType(), data, ObjectEventType.ObjectOnDeselected, currentObjectName));
            }

            // 从列表中移除对象
            SelectorObject.RemoveAt(i);
            if (obj is not null)
            {
                if (obj.IsFirstSelected)
                {
                    obj.IsFirstSelected = false;
                    obj.HandleObjectEvent(this, new ObjectEventArgs(obj.GetType(), data, ObjectEventType.ObjectCreatedOver, currentObjectName));
                }
            }
            Console.LogDebug("当前选择器: " + GetSelector());
            // 减少需要移除的次数
            frequencyToRemove--;
        }

        // 如果移除次数等于 removeFrequency，则返回 true，否则返回 false
        return frequencyToRemove == 0;
    }
    /// <summary>
    /// 取消选择场景
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <returns>是否成功</returns>
    public bool DeselectScene(string sceneName)
    {
        if (SelectorScene.Count is 0) return false;
        if (!string.IsNullOrEmpty(sceneName))
        {
            SelectorScene.RemoveAt(SelectorScene.LastIndexOf(sceneName));
            return true;
        }
        return false;
    }
    /// <summary>
    /// 取消选择节点
    /// </summary>
    /// <returns>是否成功</returns>
    public bool DeselectNode()
    {
        if (string.IsNullOrEmpty(SelectorNode)) return false;
        SelectorNode = string.Empty;
        return true;
    }
    /// <summary>
    /// 取消选择所有
    /// </summary>
    /// <returns>是否成功</returns>
    public bool DeselectAll()
    {
        SelectorObject.Clear();
        SelectorScene.Clear();
        SelectorNode = string.Empty;
        return true;
    }

    // 检测
    /// <summary>
    /// 检测是否选择对象
    /// </summary>
    public bool IsSelectAnyObject() => SelectorObject.Count is not 0;
    /// <summary>
    /// 检测是否选择场景
    /// </summary>
    public bool IsSelectAnyScene() => SelectorScene.Count is not 0;
    /// <summary>
    /// 检测是否选择节点
    /// </summary>
    public bool IsSelectAnyNode() => !string.IsNullOrEmpty(SelectorNode);

    // 保存与读取
    /// <summary>
    /// 【仅内部使用】保存选择对象（注意传出是引用，通常需要深拷贝）
    /// </summary>
    internal List<string> SaveSelectorObject() => SelectorObject;
    /// <summary>
    /// 【仅内部使用】保存选择节点（注意传出是引用，通常需要深拷贝）
    /// </summary>
    internal List<string> SaveSelectorScene() => SelectorScene;
    /// <summary>
    /// 【仅内部使用】加载选择对象（覆盖）
    /// </summary>
    internal void LoadSelectorObject(List<string> data) => SelectorObject = data;
    /// <summary>
    /// 【仅内部使用】加载选择场景（覆盖）
    /// </summary>
    internal void LoadSelectorScene(List<string> data) => SelectorScene = data;
}
