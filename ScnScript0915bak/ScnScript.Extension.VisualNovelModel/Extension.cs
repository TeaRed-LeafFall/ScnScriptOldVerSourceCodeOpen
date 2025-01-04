using ScnScript.Runtime;
using ScnScript.Runtime.Basic;
using System.ComponentModel;
using Console = ScnScript.Runtime.Console;

namespace ScnScript.Extension.VisualNovelModel;

[Script(ScriptServicesType.Provider, "ScnScript.VisualNovelModel")]
[ScriptClass("VN")]
public class VisualNovelModelLib : ScriptProvider
{
    public override void OnLoad(ScnScriptHost host)
    {
        host.RegisterObjectType<Character>();
    }
}
[ScriptObject("Character",["character"])]
public class Character : ScriptObjectClass
{
    [Browsable(true), Category("角色属性"), Description("角色显示名称")]
    public string DisplayName { get; set; }
    public Character()
    {
        DisplayName = string.Empty;
        IsSubSelectionDisabled = true;
    }
    public override void HandleObjectEvent(object sender, ObjectEventArgs e)
    {
        switch (e.EventType)
        {
            case ObjectEventType.ObjectOnCreated:
                Console.LogInfo(e.ObjectName);
                Console.WriteLine($"Character {Name} created");
                break;
            case ObjectEventType.ObjectOnSelected:
                var configs=e.FuncCallData.ActionConfigs;
                var data = e.FuncCallData;
                if (configs is null) break;
                foreach (var config in configs)
                {
                    switch (config.Key)
                    {
                        case "name":
                            if (string.IsNullOrEmpty(DisplayName))
                            {
                                DisplayName = data.GetString(config.Value);
                            }
                            break;
                        case "getname":
                            DisplayName = data.GetString(config.Value);
                            break;
                    }
                }
                break;
            case ObjectEventType.ObjectOnDeselected:
                break;
            case ObjectEventType.ObjectContentChanged:
                var content = sender as string;
                if (content is null) break;
                Console.WriteLine($"Character {Name} content changed to {content}");
                break;
            case ObjectEventType.ObjectCreatedOver:
                break;
            case ObjectEventType.ObjectDestroyed:
                break;
        }
        base.HandleObjectEvent(sender, e);
    }
    public override bool ProgressAutoType(ScriptFuncCallData data)
    {
        var config = data.ActionConfigs;
        if(config is null) return false;
        if(config.ContainsKey("getname")) return true;
        return false;
    }
}