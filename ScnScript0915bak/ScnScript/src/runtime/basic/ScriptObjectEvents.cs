using ScnScript.Runtime.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Runtime.Basic;

public delegate void ObjectEventHandler(object sender, ObjectEventArgs args);
public class ObjectEventArgs(Type trusteeshipType, ScriptFuncCallData data, ObjectEventType eventType, string objectName) : EventArgs
{
    public Type TrusteeshipType { get; private init; } = trusteeshipType;
    public ScriptFuncCallData FuncCallData { get; private init; } = data;
    public string ObjectName { get; private init; } = objectName;
    public ObjectEventType EventType { get; private init; } = eventType;
}
public enum ObjectEventType
{
    // 在对象创建时触发
    ObjectOnCreated,
    // 在对象选中时触发
    ObjectOnSelected,
    // 在对象取消选中时触发(在取消函数运行中触发，选择器还没有取消选中时)
    ObjectOnDeselected,
    // 在子对象被添加时触发
    ObjectAddSubItem,
    // 在对象内容被改变时触发
    ObjectContentChanged,
    // 在对象创建结束时触发(第一次选中状态取消之后被触发)
    ObjectCreatedOver,
    // 在对象销毁时触发
    ObjectDestroyed,
}