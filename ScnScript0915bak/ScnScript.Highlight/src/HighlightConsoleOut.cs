using ScnScript.Highlight;
using ScnScript;
using System.Drawing;
using Console = Colorful.Console;
namespace ScnScript.Highlight;

public static class HighlightConsoleOut
{
    /* string #b9e88d 字符串
expression #d69d73 单引号表达式
scene #c792ea 场景与属性
comment #546e7a 注释
command #ffbf51 命令
objectCommand #f07178 对象命令
symbol #89ddff 符号与普通文本
number #d3856c 数字 
inSelectLineNumber #6c8692 选中行号
background #263238 背景*/
    // 色彩转换
    public static Color GetColor(HighlightType? type)
    {
        return type switch
        {
            HighlightType.ObjectCommand => ColorTranslator.FromHtml("#f07178"),
            HighlightType.String => ColorTranslator.FromHtml("#b9e88d"),
            HighlightType.Comment => ColorTranslator.FromHtml("#546e7a"),
            HighlightType.Command => ColorTranslator.FromHtml("#ffbf51"),
            HighlightType.Number => ColorTranslator.FromHtml("#d3856c"),
            HighlightType.Expression => ColorTranslator.FromHtml("#d69d73"),
            HighlightType.Key => ColorTranslator.FromHtml("#c792ea"),
            HighlightType.Scene => ColorTranslator.FromHtml("#c792ea"),
            HighlightType.Symbol => ColorTranslator.FromHtml("#89ddff"),
            _ => Color.White
        };
    }
    /// <summary>
    /// 输出高亮
    /// </summary>
    /// <param name="line">行号</param>
    /// <param name="text">行文本</param>
    /// <param name="highlight">高亮数据</param>
    /// <param name="maxLineNumber">最大行号(用于靠右对齐，-1表示没有)</param>
    /// <param name="useZeroFillBlank">是否使用0填充空白(需要最大行号)</param>
    public static void HighlightOut(int line, string text, Dictionary<Area, HighlightType>? highlight,int maxLineNumber=-1,bool useZeroFillBlank=false)
    {

        var currentColor = Color.White;
        Console.ForegroundColor = Color.White;
        var lineNumberColor = ColorTranslator.FromHtml("#6c8692");
        if(maxLineNumber <= 0)
        {
            Console.Write($"{line} |",lineNumberColor);
        }
        else
        {
            if (useZeroFillBlank)
            {
                Console.Write($"{line.ToString().PadLeft(maxLineNumber.ToString().Length, '0')} |", lineNumberColor);
            }
            else
            {
                Console.Write($"{line.ToString().PadLeft(maxLineNumber.ToString().Length)} |", lineNumberColor);
            }
        }

        if (highlight == null)
        {
            Console.WriteLine(text, ColorTranslator.FromHtml("#89ddff"));
            return;
        }

        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];
            var tColor = ColorTranslator.FromHtml("#89ddff");
            if (highlight.Any(h => h.Key.IsInArea(i)))
            {
                tColor = GetColor(highlight[highlight.Last(h => h.Key.IsInArea(i)).Key]);
            }

            Console.Write(c,tColor);
            Console.ForegroundColor = currentColor;
        }
        Console.WriteLine();
    }
}