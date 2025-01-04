
using ScnScript;
using ScnScript.Highlight;
using System.Diagnostics;

namespace ScnScript.Highlight;

public enum HighlightType
{
    String,
    Expression,
    Scene,
    Comment,
    Command,
    ObjectCommand,
    Symbol,
    Number,
    Key,
    Ignore
}

/// <summary>
/// 高亮数据辅助类
/// </summary>
public class HighlightDataHelper
{
    /// <summary>
    /// 高亮数据
    /// </summary>
    private Dictionary<int, Dictionary<Area, HighlightType>> hData = new();
    /// <summary>
    /// 清空数据
    /// </summary>
    public void Clear()
    {
        hData.Clear();
    }
    /// <summary>
    /// 获取行高亮区域类型
    /// </summary>
    /// <param name="line">行</param>
    /// <returns>数据</returns>
    public Dictionary<Area, HighlightType>? GetLineHighlightAreaTypes(int line)
    {
        return hData.GetValueOrDefault(line);
    }
    public bool HasThisLine(int line)
    {
        return hData.ContainsKey(line);
    }
    /// <summary>
    /// 添加该行某个区域的高亮类型
    /// </summary>
    /// <param name="line">行</param>
    /// <param name="area">区域</param>
    /// <param name="type">类型</param>
    public void AddHighlightType(int line, Area area, HighlightType type)
    {
        if (hData.TryGetValue(line, out var value))
        {
            value[area] = type;
        }
        else
        {
            hData[line] = new Dictionary<Area, HighlightType>
            {
                { area, type }
            };
        }
    }
}
/// <summary>
/// 高亮辅助类
/// </summary>
public class HighlightHelper
{
    // 私有数据： 高亮数据，行文本，是否是多行注释，跳转索引
    private HighlightDataHelper hData = new();
    private Dictionary<int, string> lines = new();
    private Dictionary<int, int> linesToPoints = new();
    private int nowPoint = 0;
    private bool mutiLineCommentStarted = false;
    private int jumpToIndex = 0;
    private int MaxLineNum = 0;

    /// <summary>
    /// 高亮该行某个区域里的节点，符号与属性键
    /// </summary>
    /// <param name="start">开始位置</param>
    /// <param name="end">结束位置</param>
    /// <param name="line">行内容</param>
    /// <param name="lineNum">行号</param>
    private void HighlightNodeSymbolAndKey(int start,int end,string line,int lineNum,Dictionary<Area,TokenType> dic)
    {
        var firstSpacePassed = false;
        var lastSpaceIndex = -1;
        for (var i = start; i < end; i++)
        {
            if (dic.Any(x => x.Key.IsInArea(i) && x.Value is not TokenType.SymbolCommandStart and not TokenType.SymbolObject)) continue;
            // 是否是第一个空格
            if (line[i] is ' ')
            {
                firstSpacePassed = true;
                lastSpaceIndex = i;
            }
            if (line[i] is '=')
            {
                if (firstSpacePassed)
                {
                    hData.AddHighlightType(lineNum, new Area(lastSpaceIndex, i - 1), HighlightType.Key);
                }
            }
            // 是否是场景节点标识
            if (line[i] is '*')
            {
                var endIndex = line.IndexOf(' ', i, 1);
                if (endIndex is -1)
                {
                    endIndex = end;
                }
                hData.AddHighlightType(lineNum, new Area(i, endIndex), HighlightType.Scene);
            }
            else
            {
                //是否是其他符号
                if (!ScnScriptCommon.IsValidString(line[i].ToString()))
                {
                    hData.AddHighlightType(lineNum, new Area(i, i), HighlightType.Symbol);
                }
            }
        }
    }
    
    /// <summary>
    /// 高亮行
    /// </summary>
    /// <param name="line">行文本</param>
    /// <param name="lineNum">行号</param>
    private void Highlight(string line, int lineNum)
    {
        jumpToIndex = 0;
        lines.Add(lineNum, line);
        linesToPoints.Add(lineNum, nowPoint);

        nowPoint += line.Length;
        // 处于多行注释中
        if (mutiLineCommentStarted)
        {
            if (line.Contains(";;;"))
            {
                var i = line.IndexOf(";;;", StringComparison.Ordinal);
                hData.AddHighlightType(lineNum, new Area(0, i+3), HighlightType.Comment);
                mutiLineCommentStarted = false;
                jumpToIndex = i + 3;
            }
            else
            {
                hData.AddHighlightType(lineNum, new Area(0, line.Length), HighlightType.Comment);
                goto End;
            }
        }

        try
        {
            // 这里必须对Lexer进行升级了，因为必须跳过;;;，要不然它会认为这是一个注释开始，然后找不到结束。
            // 现在Lexer可以接受跳过索引了
            LexerResult result = ScnScriptCommon.Lexer(line,jumpToIndex);
            foreach (var item in result.AreaTokenTypes)
            {
                var type = HighlightType.String;
                var isValid = true;
                switch (item.Value)
                {
                    case TokenType.SymbolString:
                        type = HighlightType.String;
                        break;
                    case TokenType.SymbolComment:
                        type = HighlightType.Comment;
                        break;
                    case TokenType.SymbolCommandStart:
                        type = HighlightType.Command;
                        break;
                    case TokenType.SymbolObject:
                        type = HighlightType.ObjectCommand;
                        break;
                    case TokenType.SymbolNodeScene:
                        type = HighlightType.Scene;
                        break;
                    case TokenType.SymbolSingleQuotesExpression:
                        type = HighlightType.Expression;
                        break;
                    case TokenType.SymbolEscape:
                        type = HighlightType.Symbol;
                        break;
                    case TokenType.SymbolIgnoreLf:
                        type = HighlightType.Ignore;
                        break;
                    case TokenType.SymbolWhitespace:
                        type = HighlightType.Ignore;
                        break;
                    default:
                        isValid = false;
                        break;
                }
                switch (type)
                {
                    case HighlightType.ObjectCommand:
                        HighlightNodeSymbolAndKey(item.Key.Start, item.Key.End, line, lineNum, result.AreaTokenTypes);
                        break;

                    case HighlightType.Command:
                        HighlightNodeSymbolAndKey(item.Key.Start+1, item.Key.End, line, lineNum, result.AreaTokenTypes);
                        break;
                }
                if (isValid) hData.AddHighlightType(lineNum, item.Key, type);
            }
        }
        catch
        {
            // 如果报错，有可能是注释没有闭合。因为我们使用单行处理，所以可能检查不到注释闭合
            // 所以我们使用 mutiLineCommentStarted 布尔值来检查是否处于注释中
            if (line.Contains(";;;"))
            {
                mutiLineCommentStarted = true;
                hData.AddHighlightType(lineNum, new Area(0, line.Length), HighlightType.Comment);
            }
            //else
            //{
            //    throw;
            //}
        }

        
    End:
        return;
        //HighlightConsoleOut.HighlightOut(lineNum, line, hData.GetLineHighlightAreaTypes(lineNum),MaxLineNum,true);
    }
    /// <summary>
    /// 高亮
    /// </summary>
    /// <param name="lines">行列表</param>
    private void Highlight(List<string> lines)
    {
        hData.Clear();
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            MaxLineNum = lines.Count;
            Highlight(line, i+1);
        }
    }
    /// <summary>
    /// 由代码获取高亮
    /// </summary>
    /// <param name="code">源代码</param>
    public void HighlightByCode(string code)
    {
        var a=code.ReplaceLineEndings();
        Highlight(a.Split(Environment.NewLine).ToList());

    }

    public HighlightDataHelper GetHighlightData()
    {
        return hData;
    }
    public int GetMaxLineNum()
    {
        return MaxLineNum;
    }
    public string GetLine(int lineNum)
    {
        return lines[lineNum];
    }
    public int GetLineToPoint(int lineNum)
    {
        return linesToPoints[lineNum];
    }
}
