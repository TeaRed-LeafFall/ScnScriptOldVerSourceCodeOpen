using ScnScript.Runtime.Basic;
using ScnScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScnScript;
using ScnScript.Extension.MenuModel;
using System.ComponentModel;

namespace ScnScriptForm.Extension;

[Script(ScriptServicesType.Provider, "ScnScriptForm")]
[ScriptClass("formLib")]
public class FormModelLib : ScriptProvider
{
    public override void OnLoad(ScnScriptHost host)
    {
        host.RegisterObjectType<Form>();
    }

    public static System.Windows.Forms.Form? MainForm { get; set; }

    [ScriptFunc(ActionType.Object, "ContentPlacer")]
    public static void ContentPlacer(ScriptFuncCallData data)
    {
        var configs = data.ActionConfigs;
        var pKey= data.Status.GetSelector(false);
        if (data.HasData(pKey))
        {
            var parent = data.GetData(pKey) as Form;
            if (parent is null) return;
            var convertType = string.Empty;
            object? sourceData = null;
            if(configs is not null)
            {
                foreach (var config in configs)
                {
                    switch (config.Key)
                    {
                        case "convert":
                            var convert = config.Value.ToString();
                            convertType = data.GetString(convert);
                            break;
                        case "from":
                            var from = config.Value.ToString();
                            var vKey = data.GetString(from);
                            if(data.TryGetDataByKey(vKey,out var value))
                            {
                                if(value is not null)
                                {
                                    sourceData = value;
                                }
                            }
                            break;
                    }
                }
            }

            if (sourceData is not null)
            {
                if (!string.IsNullOrEmpty(convertType))
                {
                    switch (convertType)
                    {
                        case "MenuStrip":
                            var menuStrip = new System.Windows.Forms.MenuStrip();
                            var menuData = sourceData as Menu;
                            if(parent.form is not null && menuData is not null)
                            {
                                menuStrip = ConvertMenuStrip(menuData);
                                menuStrip.Dock = DockStyle.Top;
                                parent.form.Controls.Add(menuStrip);
                                parent.form.MainMenuStrip = menuStrip;
                            }
                            
                            break;
                    }
                }
            }
        }
    }
    public static MenuStrip ConvertMenuStrip(Menu menu)
    {
        var menuStrip = new MenuStrip();
        var menuItems = new List<ToolStripItem>();
        foreach (var obj in menu.SubItems)
        {
            var item = obj as MenuItem;
            if (item is null) continue;
            menuItems.Add(CreateMenu(item));
        }
        menuStrip.Items.AddRange(menuItems.ToArray());
        return menuStrip;
    }
    public static ToolStripItem CreateMenu(MenuItem menuItem)
    {
        switch (menuItem.Type)
        {
            case MenuItemType.Normal:
                var menuItem1 = new ToolStripMenuItem(menuItem.Text);
                CreateSubMenu(menuItem, menuItem1);
                return menuItem1;
            case MenuItemType.Separator:
                var menuItem2 = new ToolStripSeparator();
                return menuItem2;
            case MenuItemType.Check:
                var menuItem3 = new ToolStripMenuItem(menuItem.Text);
                CreateSubMenu(menuItem, menuItem3);
                menuItem3.Checked = true;
                menuItem3.CheckOnClick = true;
                return menuItem3;
            case MenuItemType.Choice:
                var menuItem4 = new ToolStripMenuItem(menuItem.Text);
                CreateSubMenu(menuItem, menuItem4);
                menuItem4.CheckState = CheckState.Indeterminate;
                menuItem4.CheckOnClick = true;
                return menuItem4;
        }
        throw new Exception("Unknown menu item type");
    }

    private static void CreateSubMenu(MenuItem menuItem, ToolStripMenuItem menuItem1)
    {
        menuItem.SubItems.ForEach(x =>
        {
            var subItem = x as MenuItem;
            if (subItem is null) return;
            menuItem1.DropDownItems.Add(CreateMenu(subItem));
        });
    }
    [ScriptFunc(ActionType.Command, "showForm")]
    public static void ShowForm(ScriptFuncCallData data)
    {
        var configs = data.ActionConfigs;
        if (configs != null)
        {
            if(configs.TryGetValue("Value",out var findKey))
            {
                var key = data.GetString(findKey);
                if (!string.IsNullOrEmpty(key))
                {
                    if(data.TryGetDataByKey(key,out var value))
                    {
                        var form = value as Form;
                        if (form is not null && form.form is not null)
                        {
                            //form.form.MdiParent = MainForm;
                            if (MainForm is null) return;
                            MainForm.Invoke((MethodInvoker)delegate ()
                            {
                                form.form.Show();
                            });
                            
                        }
                    }
                }
            }
        }
    }

}

[ScriptObject("Form")]
public class Form : ScriptObjectClass
{
    [Browsable(true), Category("窗口数据"), Description("Form数据")]
    public System.Windows.Forms.Form? form { get; set; }
    public override void HandleObjectEvent(object sender, ObjectEventArgs e)
    {
        switch (e.EventType)
        {
            case ObjectEventType.ObjectOnCreated:
                form = new System.Windows.Forms.Form();
                form.Text = Name;
                var configs = e.FuncCallData.ActionConfigs;
                if(configs != null)
                {
                    foreach (var config in configs)
                    {
                        switch (config.Key)
                        {
                            case "title":
                                form.Text = config.Value.ToString();
                                break;
                        }
                    }
                }
                
                break;
        }

        base.HandleObjectEvent(sender, e);
    }
}