using System.Text;

namespace ScnScript;

public static partial class ScnScriptCommon;
internal static class ParserHelpers
{
    /// <summary>
    /// 地址转换
    /// </summary>
    /// <remarks>将解析器结果区域重叠在父区域的区域转换为相对位置</remarks>
    /// <param name="parentArea">父区域</param>
    /// <param name="lexerResult">解析器结果</param>
    /// <returns>所有相对区域的列表</returns>
    internal static List<Area> AddressTranslation(Area parentArea,LexerResult lexerResult)
    {
        return (from areaTokenType in lexerResult.AreaTokenTypes
                where parentArea.IsInArea(areaTokenType.Key.Start)
                where parentArea.Start != areaTokenType.Key.Start || parentArea.End != areaTokenType.Key.End
                select new Area(areaTokenType.Key.Start - parentArea.Start, areaTokenType.Key.End - parentArea.Start)).ToList();
    }
    /// <summary>
    /// 获取关键词
    /// </summary>
    /// <param name="context">该部分代码(标识符号记得去掉)</param>
    /// <param name="needIgnoreAreas">在代码区域的忽略位置</param>
    /// <returns>关键词列表</returns>
    internal static List<string> GetKeywords(string context, List<Area> needIgnoreAreas)
    {
        var keywords = new List<string>();
        var sb = new StringBuilder();
        var number = 0;
        for (var i = 0; i <= context.Length; i++)
        {
            if(i == context.Length)
            {
                EndString();
                break;
            }
            switch (context[i])
            {
                case (char)TokenType.SymbolWhitespace:
                    // 这里是 xxx="xxx xxx"的处理（原本以为很复杂，后来思路开窍了）
                    if (needIgnoreAreas.Any(area => area.IsInArea(i + 1)))
                    {
                        sb.Append(context[i]);
                        break;
                    }
                    EndString();
                    break;
                case '=':
                    // 这里如果是第0个字符串之后的
                    if(number == 0)
                    {
                        EndString();
                        break;
                    }
                    // 这里记得加，不然普通的=会直接忽略
                    sb.Append(context[i]);
                    break;
                default:
                    var index = context.IndexOf("->", i, StringComparison.Ordinal);
                    if (index > 0)
                    {
                        // 判断是否存在于 needIgnoreAreas 中
                        if (!needIgnoreAreas.Any(area => area.IsInArea(i)))
                        {
                            sb.Append(context.AsSpan(i, index - i));
                            EndString();
                            // 这里是 index+1， 因为 break 之后在 for 循环中会 i++ 所以不需要写2
                            i = index + 1;
                            break;
                        }
                    }
                    sb.Append(context[i]);
                    break;
            }
        }
        return keywords;

        void EndString()
        {
            if (sb.Length > 0)
            {
                keywords.Add(sb.ToString());
            }
            sb.Clear();
            number++;
        }
    }
    /// <summary>
    /// 获取使用空格分割的关键词
    /// </summary>
    /// <param name="context">该部分代码(标识符号记得去掉)</param>
    /// <param name="needIgnoreAreas">在代码区域的忽略位置</param>
    /// <returns>关键词列表</returns>
    internal static List<string> GetKeywordsOnlyWhitespace(string context,List<Area> needIgnoreAreas)
    {
        var keywords = new List<string>();
        var sb = new StringBuilder();
        for (var i = 0; i <= context.Length; i++)
        {
            if (i == context.Length)
            {
                EndString();
                break;
            }
            switch (context[i])
            {
                case (char)TokenType.SymbolWhitespace:
                    if (needIgnoreAreas.Any(area => area.IsInArea(i + 1)))
                    {
                        sb.Append(context[i]);
                        break;
                    }
                    EndString();
                    break;
                default:
                    sb.Append(context[i]);
                    break;
            }
        }
        return keywords;

        void EndString()
        {
            if (sb.Length > 0)
            {
                keywords.Add(sb.ToString());
            }
            sb.Clear();
        }
    }
    /// <summary>
    /// 处理解析器可以在不同环境一起用的代码
    /// </summary>
    /// <param name="areaPositions">区域位置</param>
    /// <param name="areaType">区域类型</param>
    /// <param name="keywords">关键词(请使用GetKeywords获取)</param>
    /// <param name="actionType">输出的Action类型</param>
    /// <returns>ScnAction</returns>
    /// <exception cref="ScnParserException">解析异常</exception>
    internal static ScnAction HandleParser(Dictionary<Area, Position> areaPositions, KeyValuePair<Area, TokenType> areaType, List<string> keywords,ActionType actionType)
    {
        var head = string.Empty;
        var value = string.Empty;
        var args = new List<string>();
        var properties = new Dictionary<string, string>();
        foreach (var keyword in keywords)
        {
            switch (keyword[0])
            {
                case (char)TokenType.SymbolString:
                    if (!properties.TryAdd("Value", keyword))
                    {
                        throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} {ScnScriptExceptionMessage.GrammarError} 重复定义了 Value 属性！");
                    }
                    break;
                case (char)TokenType.SymbolSingleQuotesExpression:
                    // 去头去尾
                    var noSymbolExpression = keyword.Remove(0, 1);
                    noSymbolExpression = noSymbolExpression.Remove(noSymbolExpression.Length - 1, 1);
                    args.Add(noSymbolExpression);
                    break;
                default:
                    // 处理属性
                    if (keyword.Contains('='))
                    {
                        // 分开为两部分
                        var keyValue = keyword.Split('=', 2);
                        if (!ScnScriptCommon.IsValidString(keyValue[0])) throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} {ScnScriptExceptionMessage.IllegalCharacter} 属性名 {keyValue[0]} 不合法！");
                        if (!properties.ContainsKey(keyValue[0]))
                        {
                            properties.Add(keyValue[0], keyValue[1]);
                        }
                        else
                        {
                            throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} {ScnScriptExceptionMessage.GrammarError} 重复定义了 {keyValue[0]} 属性！");
                        }

                    }
                    else
                    {
                        // 是否头被赋值: 让第一个关键字作为 head
                        if (head == string.Empty)
                        {
                            head = keyword;

                            // 关闭标签语法支持
                            if (actionType is ActionType.Command && ParserConfigs.DefaultLetCommandStartWithSlashAsCloseTagFeature)
                            {
                                if (head.StartsWith('/'))
                                {
                                    head = head.Remove(0, 1);
                                    value = "close";
                                    break;
                                }
                            }
                            
                            if (actionType is ActionType.Object)
                            {
                                // -xxx 语法支持
                                if (ParserConfigs.DefaultLetObjectStartWithMinusAsClrFeature && head.StartsWith('-'))
                                {
                                    head = head.Remove(0, 1);
                                    if (!ScnScriptCommon.IsValidString(head) || string.IsNullOrEmpty(head)) throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} {ScnScriptExceptionMessage.IllegalCharacter} 值名 {head} 不合法！");
                                    value = head;
                                    head = "clr";
                                    //break;
                                }
                                // Selector高级语法支持
                                if (ParserConfigs.DefaultLetObjectPlusMinusFeature)
                                {
                                    if (head.Contains('+') || head.Contains('-'))
                                    {
                                        var isPlus = head.IndexOf('+');
                                        var theOperator = isPlus > 0 ? '+' : '-';
                                        var spilt = head.Split(theOperator, 2);
                                        if (spilt.Length <= 1) throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} {ScnScriptExceptionMessage.GrammarError}  {head} 不是规范的 LetObjectPlusMinusFeature 语法");
                                        int number = int.Parse(spilt[1]);
                                        var mode = isPlus > 0 ? "Plus" : "Minus";

                                        // 这里是一个转换例子 @Selector xxx Mode=Plus Frequency=2
                                        head = "Selector";
                                        if (!ScnScriptCommon.IsValidString(spilt[0]) || string.IsNullOrEmpty(spilt[0])) throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} {ScnScriptExceptionMessage.IllegalCharacter} 值名 {spilt[0]} 不合法！");
                                        value = spilt[0];
                                        properties.Add("Mode", mode);
                                        properties.Add("Frequency", number.ToString());
                                        //break;
                                    }
                                    
                                }

                            }

                            

                            if (!ScnScriptCommon.IsValidString(head) || string.IsNullOrEmpty(head)) throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} {ScnScriptExceptionMessage.IllegalCharacter} 值名 {head} 不合法！");
                        }
                        else
                        {
                            // 是否Value被赋值
                            if (value == string.Empty)
                            {
                                if(actionType is ActionType.Object)
                                {
                                    if (ParserConfigs.DefaultLetObjectValueAsArgsFeature)
                                    {
                                        args.Add(keyword);
                                    }
                                }
                                else
                                {
                                    value = keyword;
                                }
                            }
                            else
                            {
                                // 尝试让第三项作为属性中Value的值
                                if (!properties.TryAdd("Value", keyword))
                                {
                                    throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} {ScnScriptExceptionMessage.GrammarError} 重复定义了 Value 属性！");
                                }

                            }
                        }
                    }
                break;
            }
        }

        // 已弃用，字符串已经在前面处理了！
        if (head == string.Empty && keywords.Count > 0)
        {
            head = keywords[0];
        }

        return new ScnAction(actionType, value, head, args, properties);
    }
}
