// See https://aka.ms/new-console-template for more information



using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using Microsoft.VisualBasic;
using ScnScript;
//using ScnScript.Extension.MenuModel;
//using ScnScript.Highlight;
using ScnScript.LexerPlus;
//using ScnScript.Runtime;
//using ScnScript.Runtime.Basic;
using ScnScriptConsole;
using Console = System.Console;
//using Console = Colorful.Console;
// v3 Test
//ScnRunner runner=new();
//Console.WriteLine("Hello, World!");
////runner.InputString(File.ReadAllText("D:\\dev\\ScnScript\\ScnScriptConsole\\__ScnScript.scn"));

// v4 Alpha version
//var context = File.ReadAllText(@"D:\dev\ScnScript\ScnScriptConsole\MainWindowMenu.scn").ReplaceLineEndings("\n");
//var lexerResult = ScnScriptCommon.Lexer(context);
//Console.WriteLine("");
//Console.WriteLine("Lexer OK!");
////Console.WriteLine("按任意键继续");
////Console.ReadLine();
//var parserResult = ScnScriptCommon.Parser(context, lexerResult);
//Console.WriteLine("");
//Console.WriteLine("Parser OK!");

// v4 Preview version
Console.Clear();

Console.WriteLine("ScnScript v4 Preview Version");
// {ScnScriptCommon.VersionText}
Console.WriteLine($"ScnScript Version Code v4.0perview-alpha-rebuild4-early");
Console.WriteLine("This is Console demo for Test!");
Console.WriteLine();
// 提前完成公共类实现
//ScnScript.Runtime.Console.LogOutput += (msgBoxResult, type) =>
//{
//    Debug.WriteLine(type + ": " + msgBoxResult);
//};
//ScnScript.Runtime.Console.ReadInput += mode =>
//{
//    if (mode is ScnScript.Runtime.Console.ReadMode.Key)
//    {
//        return (string?)Console.ReadKey().Key.ToString();
//    }
//    return Console.ReadLine();
//};
//ScnScript.Runtime.Console.WriteOutput += Console.Write;

//测试高亮功能
var path = @"D:\dev\ScnScript\ScnScriptConsole\__ScnScript.scn";
var context = File.ReadAllText(path);

PlusClass.Lexer(context);


return;

//var helper = new HighlightHelper();
//Console.WriteLine("高亮效果————————————");
//var timer = new EasyTimer();
//timer.Start();
//helper.HighlightByCode(context);
//var hData = helper.GetHighlightData();
//var MaxLineNum = helper.GetMaxLineNum();
//for (int lineNum = 1; lineNum <= MaxLineNum; lineNum++)
//{
//    HighlightConsoleOut.HighlightOut(lineNum, helper.GetLine(lineNum), hData.GetLineHighlightAreaTypes(lineNum), MaxLineNum, true);
//}
//Console.WriteLine($"注：{Environment.NewLine}高亮效果使用ScnScript.Highlight提供解析/Colorful.Console库提供色彩显示。{Environment.NewLine}背景色彩由于bug而直接不用了，前景似乎由于控制台限制而出现少量换色的情况。");
//timer.ShowTime();
//Console.WriteLine();
//Console.WriteLine("脚本解析————————————");
//timer.Start();
//var host = new ScnScriptHost();
//host.RegisterFuncBook(new MenuModelModelLib());
//RuntimeSandBox sb = host.GetSandBox(host.CreateSandBox(path));
//timer.ShowTime();

//Console.WriteLine();
//Console.WriteLine("脚本运行————————————");
//Run:
//try
//{
//    host.Run();
//}
//catch (Exception e)
//{
//    Console.WriteLine(e.Message);
//}

//if (host is { IsWaiting: true, IsOver: false })
//{
//    goto Run;
//}
//Console.WriteLine();
//Console.WriteLine("——————脚本运行已结束按任意键退出");
//Console.ReadLine();
//return;
