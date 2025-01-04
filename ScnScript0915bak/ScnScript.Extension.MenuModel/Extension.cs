using ScnScript.Runtime;
using ScnScript.Runtime.Basic;
using System.ComponentModel;
using System.Text;
using Console = ScnScript.Runtime.Console;

namespace ScnScript.Extension.MenuModel;

[Script(ScriptServicesType.Provider, "ScnScript.MenuModel")]
[ScriptClass("Menu")]
public class MenuModelModelLib : ScriptProvider
{
    public override void OnLoad(ScnScriptHost host)
    {
        host.RegisterObjectType<Menu>();
        host.RegisterObjectType<MenuItem>();
    }
}

[ScriptObject("Menu")]
public class Menu : ScriptObjectClass
{
    public Menu()
    {
        DoRemoveSubDataInDeselected = true;
    }
    public override string ToStringThisMethod()
    {
        return $"菜单 {Name}";
    }
}

public enum MenuItemType
{
    Normal,
    Separator,
    Check,
    Choice
}

[ScriptObject("MenuItem")]
public class MenuItem : ScriptObjectClass
{
    [Browsable(true), Category("菜单属性"), Description("菜单显示名称")]
    public string Text { get; set; } = "菜单项";
    [Browsable(true), Category("菜单属性"), Description("菜单类型")]
    public MenuItemType Type { get; set; } = MenuItemType.Normal;
    public MenuItem()
    {
        IsSubSelectionDisabled = true;
        DoRemoveSubDataInDeselected = true;
    }
    public override string ToStringThisMethod()
    {
        return $"菜单项 {Text} [类型 {Type}]";
    }
    public override void HandleObjectEvent(object sender, ObjectEventArgs e)
    {
        switch (e.EventType)
        {
            case ObjectEventType.ObjectOnCreated:
                Console.LogInfo(e.ObjectName);
                var args = e.FuncCallData.ActionArgs;
                if (args is not null)
                if (args.Count > 0)
                {
                    foreach (var arg in args)
                    {
                        switch (arg)
                        {
                            case "parent":
                                IsSubSelectionDisabled = false;
                                break;
                        }
                    }
                }
                var configs = e.FuncCallData.ActionConfigs;

                if (configs is not null)
                if (configs.Count > 0)
                {
                    foreach (var config in configs)
                    {
                        switch (config.Key)
                        {
                            case "text":
                                    // 去除左右双引号
                                    Text = e.FuncCallData.GetString(config.Value);
                                break;
                            case "type":
                                if (Enum.TryParse(e.FuncCallData.GetString(config.Value), true, out MenuItemType type))
                                {
                                    Type = type;
                                }
                                else
                                {
                                    Console.LogError($"Invalid MenuItemType: {config.Value}");
                                }
                                break;
                        }

                    }
                }
                Console.WriteLine($"MenuItem {Text} created");
                return;
            case ObjectEventType.ObjectOnSelected:
                if (IsSubSelectionDisabled)
                {
                    e.FuncCallData.Status.DeselectObject(e.FuncCallData, e.ObjectName);
                }
                break;
        }
        base.HandleObjectEvent(sender, e);
    }
}
