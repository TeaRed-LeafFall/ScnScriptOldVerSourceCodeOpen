
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Runtime.Basic;

public class ScriptObjectClass
{
    public virtual void HandleObjectEvent(object sender, ObjectEventArgs e)
    {
        Console.LogDebug($"{e.ObjectName} {e.EventType}");
        switch (e.EventType)
        {
            case ObjectEventType.ObjectCreatedOver:
                var status = e.FuncCallData.Status;
                var aSelector = e.FuncCallData.Status.GetSelectorScene() + ".content";


                var obj = e.FuncCallData.GetSelectorObject<ScriptObjectClass>();
                if (obj is not null)
                {
                    if (!obj.IsSubSelectionDisabled)
                    {
                        obj.HandleObjectEvent(this, new(e.TrusteeshipType, e.FuncCallData, ObjectEventType.ObjectAddSubItem, this.Name));
                    }
                }

                // 防止绝后(把自己删了没有父对象接收时没了)
                if (!e.FuncCallData.Status.IsSelectAnyObject())
                {
                    if (e.FuncCallData.HasData(aSelector))
                    {
                        if (e.FuncCallData.SandBox.DocumentType is DocumentType.Normal)
                        {
                            var list = e.FuncCallData.GetData(aSelector) as List<ScriptObjectClass>;
                            if (list is not null) list.Add(this);
                        }
                        else
                        {
                            throw new ScnRuntimeException("类型为结构的脚本不应该设置多个对象！");
                        }
                    }
                    else
                    {
                        if (e.FuncCallData.SandBox.DocumentType is DocumentType.Normal)
                        {
                            e.FuncCallData.SetData(aSelector, new List<ScriptObjectClass>());
                        }
                        else
                        {
                            e.FuncCallData.SetData(aSelector, this);
                        }

                    }
                }
                break;
            case ObjectEventType.ObjectAddSubItem:
                if (IsSubSelectionDisabled) break;
                var subitem = sender as ScriptObjectClass;
                if (subitem is not null)
                {
                    SubItems.Add(subitem);
                }
                break;
            case ObjectEventType.ObjectOnDeselected:
                if (DoRemoveSubDataInDeselected)
                {
                    var dic=e.FuncCallData.GetAllData();
                    var tSelector = e.FuncCallData.Status.GetSelector(false);
                    foreach (var item in dic)
                    {
                        if (!string.IsNullOrEmpty(item.Key))
                        {
                            if (item.Key.StartsWith(tSelector))
                            {
                                e.FuncCallData.RemoveData(item.Key);
                            }
                        }
                    }
                }
                break;
        }
    }
    /// <summary>
    /// 表示是否禁止创建子对象。
    /// 如果为 true，则子对象不能被选择。
    /// </summary>
    [Browsable(true), Category("脚本对象"),Description("表示是否禁止创建子对象。\r\n如果为 true，则子对象不能被选择。"), ReadOnly(true)]
    public bool IsSubSelectionDisabled { get; set; }
    /// <summary>
    /// 表示是否禁止对象被选择。
    /// </summary>
    [Browsable(true), Category("脚本对象"), Description("表示是否禁止对象被选择。"), ReadOnly(true)]
    public bool CannotSelect { get; set; }
    /// <summary>
    /// 对象名称，并非在空间中真实名称
    /// </summary>
    [Browsable(true), Category("脚本对象"), Description("对象名称，并非在空间中真实名称"), ReadOnly(true)]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 【仅内部】是否是第一次选中
    /// </summary>
    [Browsable(false)]
    public bool IsFirstSelected { get; set; } = true;
    /// <summary>
    /// 是否在取消选中时删除子对象数据(指在WorkData中的临时数据)
    /// </summary>
    [Browsable(true), Category("脚本对象"), Description("是否在取消选中时删除子对象数据(指在WorkData中的临时数据)"), ReadOnly(true)]
    public bool DoRemoveSubDataInDeselected { get; set; } = false;
    /// <summary>
    /// 子对象
    /// </summary>
    [Browsable(true), Category("脚本对象"), Description("子对象"),ReadOnly(true)]
    public List<ScriptObjectClass> SubItems { get; set; } = new();
    /// <summary>
    /// 【不要更改】到字符串方法
    /// </summary>
    /// <remarks>
    /// <para>通常会被out操作使用，会自动输出SubItems</para>
    /// <para>如果需要更改当前项的输出方法，请重写ToStringThisMethod方法</para>
    /// <para>如果需要更改子对象的输出逻辑，那么需要重写该方法</para>
    /// </remarks>
    /// <returns></returns>
    public override string ToString()
    {
        return ToStringImply(0);
    }
    public virtual string ToStringThisMethod()
    {
        return $"{this.GetType().Name} {Name}";
    }
    private string ToStringImply(int indentLevel)
    {
        StringBuilder sb = new StringBuilder();
        // 根据缩进级别添加制表符
        string indent = new string('\t', indentLevel);
        sb.Append(indent + ToStringThisMethod());
        if (SubItems.Count > 0)
        {
            sb.AppendLine(" 具有以下子项目:");
            foreach (var item in SubItems)
            {
                // 递归调用ToStringImply，增加缩进级别
                sb.AppendLine(item.ToStringImply(indentLevel + 1));
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 处理自动类型判断
    /// </summary>
    /// <returns>是否属于该类型</returns>
    public virtual bool ProgressAutoType(ScriptFuncCallData data)
    {
        return false;
    }
}
[ScriptObject("object")]
public class ScriptObject : ScriptObjectClass
{
    public override void HandleObjectEvent(object sender, ObjectEventArgs e)
    {
        base.HandleObjectEvent(sender, e);
    }
}

[ScriptObject("class")]
public class ScriptClass : ScriptObjectClass
{

}
