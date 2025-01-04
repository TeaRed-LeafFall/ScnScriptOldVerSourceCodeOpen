// See https://aka.ms/new-console-template for more information



using ScnScript.CodeAnalysis;

Console.WriteLine("ScnScript v4 Preview Version");
// {ScnScriptCommon.VersionText}
Console.WriteLine($"ScnScript Version Code v4.0perview-alpha-rebuild4-early");
Console.WriteLine("This is Console demo for Test!");

var path = @"D:\dev\ScnScript\Sample\MainWindowMenu.scn";
var context = File.ReadAllText(path);
Console.WriteLine("Hello, World!");
Console.WriteLine(new string('-', 30));
Console.WriteLine("来自 " + path + " 的文件内容为：");
Console.WriteLine(context);
Console.WriteLine(new string('-', 30));
Console.WriteLine("开始解析...");
BehaviorTree tree= ScriptCodeAnalysis.Parse(context);

Console.ReadLine();