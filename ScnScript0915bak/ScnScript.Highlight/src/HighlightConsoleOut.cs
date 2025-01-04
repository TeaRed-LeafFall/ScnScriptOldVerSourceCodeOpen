using ScnScript.Highlight;
using ScnScript;
using System.Drawing;
using Console = Colorful.Console;
namespace ScnScript.Highlight;

public static class HighlightConsoleOut
{
    /* string #b9e88d �ַ���
expression #d69d73 �����ű��ʽ
scene #c792ea ����������
comment #546e7a ע��
command #ffbf51 ����
objectCommand #f07178 ��������
symbol #89ddff ��������ͨ�ı�
number #d3856c ���� 
inSelectLineNumber #6c8692 ѡ���к�
background #263238 ����*/
    // ɫ��ת��
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
    /// �������
    /// </summary>
    /// <param name="line">�к�</param>
    /// <param name="text">���ı�</param>
    /// <param name="highlight">��������</param>
    /// <param name="maxLineNumber">����к�(���ڿ��Ҷ��룬-1��ʾû��)</param>
    /// <param name="useZeroFillBlank">�Ƿ�ʹ��0���հ�(��Ҫ����к�)</param>
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