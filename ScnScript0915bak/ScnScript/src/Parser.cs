using System.Text;
using ScnScript.Runtime;
using Console = ScnScript.Runtime.Console;
namespace ScnScript;

public static partial class ScnScriptCommon
{
    /// <summary>
    /// 解析器
    /// </summary>
    /// <param name="context">请提供原代码</param>
    /// <param name="lexerResult">请提供词法解析结果</param>
    /// <returns>解析结果</returns>
    public static ParserResult Parser(string context, LexerResult lexerResult)
    {
        // 先载入Lexer的数据
        var areaTypes = lexerResult.AreaTokenTypes;
        var areaPositions = lexerResult.AreaPositions;
        
        // 需要输出的内容
        var actions = new Dictionary<int, ScnAction>();
        var positions = new Dictionary<int, Position>();
        var sourceCodes = new Dictionary<int, string>();
        
        // 分析代码
        
/*        // 其他内容作为文本输入 (2024.07.12)
        if (ParserConfigs.DefaultLetIgnoreUnknownAsTextFeature)
        {
            // 我们需要知道: 这些还没有加入Area的均为未知内容
            // 还有一点！ 文本输入默认不包括换行，需要[n]命令或者"\n"来换行
            #if DEBUG
            Console.LogWarning($"{nameof(ParserConfigs.DefaultLetIgnoreUnknownAsTextFeature)} 已经启用");
            # endif
            // 这里是额外的一张表，用来存储生成的字符串标识，之后会合并
            Dictionary<Area,TokenType> areaStrings = new ();
            // 临时存储区域的一张表
            var areas = areaTypes.Keys.ToList();
            var sb = new StringBuilder();
            var startIndex = -1;
            var position = new Position(LexerConfigs.DefaultPositionLineStart, LexerConfigs.DefaultPositionColumnStart);
            var startPosition = new Position(-1, -1);
            for (var i = 0; i < context.Length; i++)
            {

                switch (context[i])
                {
                    // 定位处理 一定要做，不要问我为什么。
                    case (char)TokenType.SymbolIgnoreLf:
                        position.Line++;
                        position.Column = LexerConfigs.DefaultPositionResetColumnWhenNewLine - 1;
                        break;

                    default:
                        position.Column++;
                        break;
                }
                
                if (!areas.Any(x => x.IsInArea(i)))
                {
                    switch (context[i])
                    {
                        case (char)TokenType.SymbolIgnoreLf:
                            EndString(i);
                            continue;
                        case (char)TokenType.SymbolIgnoreWhitespace:
                            EndString(i);
                            continue;
                        case '\t':
                            EndString(i);
                            continue;
                        default:
                            if (startIndex is -1)
                            {
                                startIndex = i;
                                startPosition = position;
                            }
                            sb.Append(context[i]);
                            continue;
                    }
                    
                }
                EndString(i);

                
            }
            // 如果字符串在末尾，可能就没有结束。所以移动出来，运行一下确保完成了
            EndString(context.Length);
            void EndString(int i)
            {
                if (sb.Length <= 1) return;
                var area = new Area(startIndex, i - 1);
                areaStrings.TryAdd(area, TokenType.SymbolString);
                areaPositions.TryAdd(area, startPosition);

                startIndex = -1;
                startPosition = new Position(-1, -1);
                sb.Clear();
            }
            #if DEBUG
            foreach (var areaString in areaStrings)
            {
                Console.LogInfo($"在 {areaString.Key} {areaPositions[areaString.Key]} 转换字符串: \"{context.Substring(areaString.Key.Start, areaString.Key.GetLength()+1)}\"");
            }
            #endif
            // 最后，合并 areaTypes 和 areaStrings
            areaTypes = areaTypes.Concat(areaStrings).ToDictionary();
        }
        */
        // 重新排序(这是必须的，不然在匹配时非常麻烦)
        //areaTypes=areaTypes.OrderBy(x => x.Key.Start).ToDictionary();
        //areaPositions = areaPositions.OrderBy(x => x.Key.Start).ToDictionary();

        var id = 0;
        for (var index = 0; index < areaTypes.Count; index++)
        {
            var areaType = areaTypes.ElementAt(index);

            // 获取该区域源代码
            var sourceCodeForArea = context.Substring(areaType.Key.Start, areaType.Key.GetLength());
            string noSymbolString;
            var isDone = false;
            List<string> keywords;

            // 准备
            ScnAction action=new();
            Position position=areaPositions[areaType.Key];
            var sourceCode=sourceCodeForArea;

            switch (areaType.Value)
            {
                case TokenType.SymbolString:

                    if(areaTypes.Any(area => area.Key.IsInAreaNoOnNode(areaType.Key.Start)))
                    {
                        break;
                    }

                    if (ParserConfigs.DefaultStringSymbolRemoveFeature)
                    {
                        if (sourceCodeForArea[0] is (char)TokenType.SymbolString)
                        {
                            noSymbolString = sourceCodeForArea.Remove(0, 1);
                            noSymbolString = noSymbolString.Remove(noSymbolString.Length - 1, 1);
                            if (ParserConfigs.DefaultRemoveEmptyStringFeature)
                            {
                                if (string.IsNullOrEmpty(noSymbolString) || string.IsNullOrWhiteSpace(noSymbolString))
                                {
                                    continue;
                                }
                            }
                            sourceCode = noSymbolString;
                        }
                    }
                    
                    action=new ScnAction(ActionType.String, sourceCode,string.Empty);
                    isDone = true;
                    break;

                case TokenType.SymbolCommandStart:
                    noSymbolString = sourceCodeForArea.Remove(0, 1);
                    noSymbolString = noSymbolString.Remove(noSymbolString.Length - 1, 1);
                    keywords = ParserHelpers.GetKeywords(noSymbolString, ParserHelpers.AddressTranslation(areaType.Key, lexerResult));
                    if (keywords.Count is 0)
                    {
                        throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} 没有找到命令名称！");
                    }
                    else
                    {
                        action = ParserHelpers.HandleParser(areaPositions, areaType, keywords, ActionType.Command);
                        isDone = true;
                    }
                    break;

                case TokenType.SymbolObject:
                    noSymbolString = sourceCodeForArea.Remove(0, 1);
                    // Lexer提供的部分代码可能存在:，如果在LexerConfig了启用了DefaultSupportObjectColonEnd，则去掉
                    if (LexerConfigs.DefaultSupportObjectColonEnd)
                    {
                        noSymbolString = noSymbolString.TrimEnd(':');
                    }
                    keywords = ParserHelpers.GetKeywords(noSymbolString,ParserHelpers.AddressTranslation(areaType.Key,lexerResult) );
                    if (keywords.Count is 0)
                    {
                        throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} 没有找到任何关键词！");
                    }
                    else
                    {
                        action = ParserHelpers.HandleParser(areaPositions, areaType,keywords,ActionType.Object);
                        isDone = true;
                    }

                    break;

                case TokenType.SymbolNodeScene:
                    noSymbolString = sourceCodeForArea.Remove(0, 1);
                    keywords = ParserHelpers.GetKeywordsOnlyWhitespace(noSymbolString,ParserHelpers.AddressTranslation(areaType.Key,lexerResult) );
                    if (keywords.Count is 0)
                    {
                        throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} 没有找到场景/节点名称！");
                    }
                    else
                    {
                        if (keywords.Count > 1)
                        {
                            var isValid = false;
                            if (keywords[1] is "|")
                            {
                                if (keywords.Count > 2 && keywords[2] is "Start" or "End")
                                {
                                    isValid = true;

                                    if (keywords.Count > 3)
                                    {
                                        Dictionary<string,string> properties = new()
                                        {
                                            { "DisplayName", keywords[3] }
                                        };
                                        action = new(ActionType.SceneSelector, keywords[0], keywords[2],default,properties);
                                    }
                                    else
                                    {
                                        action = new(ActionType.SceneSelector, keywords[0], keywords[2]);
                                    }
                                }
                            }


                            if (!isValid)
                            {
                                throw new ScnParserException($"在 {areaType.Key} 位置 {areaPositions[areaType.Key]} {ScnScriptExceptionMessage.GrammarError}");
                            }
                        }
                        else
                        {
                            action = new(ActionType.NodeSelector, string.Empty, keywords[0]);
                        }
                        
                        isDone = true;
                    }
                    break;
                
                default:
                    continue;
            }


            if (!isDone) continue;
            
                
            // 添加到结果
            actions.Add(id,action);
            positions.Add(id,position);
            sourceCodes.Add(id,sourceCode);
            id++;
        }
        
        // DEBUG: 打印结果
        #if DEBUG
        Console.LogInfo("Parser 生成结束！");
        foreach (var action in actions)
        {
            Console.LogInfo($"在编号 {action.Key} 位置 {positions[action.Key]} 根据源代码 {sourceCodes[action.Key]} => Type: {action.Value.Type} Value: {action.Value.Value} Head: {action.Value.Head}");
            if (action.Value.Args is { Count: > 0 })
            {
                Console.LogInfo($"参数: {string.Join(" , ", action.Value.Args)}");
            }
            if (action.Value.Configs is not { Count: > 0 }) continue;
            foreach(var config in action.Value.Configs)
            {
                Console.LogInfo($"配置: {config.Key} => {config.Value}");
            }
        }
        #endif
        // 返回结果
        return new ParserResult(actions, positions, sourceCodes);
    }

    public static bool IsValidString(string context)
    {
        // 判断字符串内是否包含以下特殊字符
        return !context.Any(ParserConfigs.IllegalCharacters.Contains);
    }
}