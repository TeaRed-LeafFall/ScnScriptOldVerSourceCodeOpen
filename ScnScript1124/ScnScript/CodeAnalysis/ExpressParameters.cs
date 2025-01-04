using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.CodeAnalysis;


// /// <summary>
// /// 表达参数
// /// </summary>
// public class ExpressParameters
// {
//     /// <summary>
//     /// 参数名
//     /// </summary>
//     public string Name { get; set; } = string.Empty;
//     /// <summary>
//     /// 参数值
//     /// </summary>
//     public ExpressParametersValue? Parameters { get; set; }
// }
/// <summary>
/// 表达式参数值
/// </summary>
public class ExpressParametersValue
{
    public T GetValue<T>() where T : ExpressParametersValue
    {
        return (T)this;
    }
}
/// <summary>
/// 字符串值
/// </summary>
public class ExpressParametersValueString(string value) : ExpressParametersValue
{
    public string Value { get; set; } = value;
    public override string ToString() => Value;
}
/// <summary>
/// 布尔值
/// </summary>
public class ExpressParametersValueBool(bool value) : ExpressParametersValue
{
    public bool Value { get; set; } = value;
    public override string ToString() => Value.ToString();
}
/// <summary>
/// 对象值
/// </summary>
public class ExpressParametersValueExpress(string value) : ExpressParametersValue
{
    public string Value { get; set; } = value;
    public override string ToString() => Value;
}
/// <summary>
/// 条件值
/// </summary>
public class ExpressParametersValueConditions : ExpressParametersValue
{
    public List<ExpressParametersValueConditionsItem> Items { get; set; } = new();
    public void AddItem(ExpressParametersValueConditionsItem item)
    {
        Items.Add(item);
    }
}
/// <summary>
/// 条件表达式结构
/// </summary>
public class ExpressParametersValueConditionsItem : ExpressParametersValue
{
    /// <summary>
    /// 左侧值
    /// </summary>
    public ExpressParametersValue? What { get; set; }
    /// <summary>
    /// 条件
    /// </summary>
    public ExpressParametersValueConditionsType Condition { get; set; }
    /// <summary>
    /// 右侧值
    /// </summary>
    public ExpressParametersValue? Value { get; set; }
}
/// <summary>
/// 用于表达式参数值条件类型
/// </summary>
public enum ExpressParametersValueConditionsType
{
    // 等于 ==
    Equal,
    // 不等于 !=
    NotEqual,
    // 大于 >
    GreaterThan,
    // 小于 <
    LessThan,
    // 大于等于 <=
    GreaterThanOrEqual,
    // 小于等于 >=
    LessThanOrEqual,
    // 与 &&
    And,
    // 或 ||
    Or,
}