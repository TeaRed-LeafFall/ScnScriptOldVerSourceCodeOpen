using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Console = ScnScript.Runtime.Console;
namespace ScnScript;
#if false
public static partial class ScnScriptCommon
{
    /// <summary>
    /// 执行词法分析(需要提供符号位置)
    /// </summary>
    /// <param name="context">原始文本</param>
    /// <param name="tokens">符号位置</param>
    /// <returns>词法分析结果</returns>
    /// <exception cref="ScnLexerException">解析异常</exception>
    [Obsolete]
    private static LexerResult LexerOld(string context, Dictionary<int, Token> tokens)
    {
        // 区域标识存储字典 (区域,标识类型)
        Dictionary<Area,TokenType> areaTokenTypes = new ();
        // 标识区域开始位置
        Dictionary<Area,Position> areaPositions = new ();
        
        // 关于定位: 当前位置
        var pos = new Position(LexerConfigs.DefaultPositionLineStart, LexerConfigs.DefaultPositionColumnStart);
        // 关于定位: 记录上一个标识符的结束位置
        var lastColumnEndInTokenKey = 0;
        // isLoop2是控制第二轮解析的开关
        // 第一轮: 标注 字符串 注释
        // 第二轮: 标注 场景 命令 对象
        var isLoop2 = false;
    LexerLoop:
        // 注意 我们不要按照字典的 key 遍历，我们需要知道这是字典的第几项，这样可以方便我们获取以后的字典项
        for (var index = 0; index < tokens.Count; index++)
        {
            var tokenMsg = tokens.ElementAt(index);

            #region 位置处理

            if (tokenMsg.Value.Type == TokenType.SymbolIgnoreLf)
            {
                pos.Line++;
                pos.Column = LexerConfigs.DefaultPositionResetColumnWhenNewLine;
                if (tokenMsg.Key > lastColumnEndInTokenKey)
                {
                    lastColumnEndInTokenKey = tokenMsg.Key;
                }
                continue;
            }

            pos.Column = tokenMsg.Key - lastColumnEndInTokenKey;
            #endregion

            #region 公共变量
            var isValidSymbol = LexerHelpers.CheckValidSymbol(areaTokenTypes, tokenMsg.Key);
            var endPos = -1;

            var isHaveEol = false;
            var isSemiEol = false;
            Area area;
            #endregion

            // 第二轮主判断
            if (isLoop2)
            {
                switch (tokenMsg.Value.Type)
                {
                    case TokenType.SymbolCommandStart:
                        break;

                    case TokenType.SymbolObject:
                        break;

                    case TokenType.SymbolNodeScene:
                        break;
                    // 不用解析的直接跳过
                    default:
                        continue;
                }
            }

            // 字符串的判断该标识符是否在字典的其他区域标识内和其他的有点不一样，所以单独提出来了。
            if (tokenMsg.Value.Type == TokenType.SymbolString)
            {
                foreach (var areaTokenType in areaTokenTypes)
                {
                    switch (areaTokenType.Value)
                    {
                        case TokenType.SymbolString when areaTokenType.Key.IsInArea(tokenMsg.Key):
                            isValidSymbol = false;
                            // 如果标识符在某个字符串区域中，则该标识符无效
                            // 这里是ScnScript的一个特性，字符串包裹的注释符号是没有用的
                            // 例如 ";" 还是字符串，但 ;"" 就是注释了
#if DEBUG
                            Console.LogWarning($"已忽略处于编号 {tokenMsg.Key} 的 {tokenMsg.Value.Type} 标识符，因为该标识符处于字符串区域中");
#endif
                            break;
                        case TokenType.SymbolComment when areaTokenType.Key.IsInArea(tokenMsg.Key):
                            isValidSymbol = false;
#if DEBUG
                            Console.LogWarning($"已忽略处于编号 {tokenMsg.Key} 的 {tokenMsg.Value.Type} 标识符，因为该标识符处于注释区域中");
#endif
                            break;
                        case TokenType.SymbolSingleQuotesExpression when areaTokenType.Key.IsInArea(tokenMsg.Key):
                            isValidSymbol = false;
#if DEBUG
                            Console.LogWarning($"已忽略处于编号 {tokenMsg.Key} 的 {tokenMsg.Value.Type} 标识符，因为该标识符处于表达式区域中");
#endif
                            break;
                    }
                }
                if (!isValidSymbol) continue;// 如果标识符无效则跳过这个的解析
            }

            // 忽略
            if (tokenMsg.Value.Type != TokenType.SymbolString && !isValidSymbol)
            {
#if DEBUG
                Console.LogWarning($"已忽略处于编号 {tokenMsg.Key} 的 {tokenMsg.Value.Type} 标识，因为其处于其他区域中");
#endif
                continue;// 如果标识符无效则跳过这个的解析
            }

#if DEBUG
            Console.LogInfo($"在位置为 {pos} [{tokenMsg.Key}] 处 发现 {tokenMsg.Value.Type} 标识");
#endif
            switch (tokenMsg.Value.Type)
            {
                // 第一轮
                case TokenType.SymbolString:
                    {

                        var isValidStringEndSymbol = true;
                        // 1.找到在字典中处于我们当前标识符之后的相同标识符
                        for (var indexForNextStringSymbol = index + 1; indexForNextStringSymbol < tokens.Count; indexForNextStringSymbol++)
                        {

                            var tokenMsgForNextStringSymbol = tokens.ElementAt(indexForNextStringSymbol);
                            if (tokenMsgForNextStringSymbol.Value.Type is TokenType.SymbolString)
                            {
                                // 2.确认标识符是否有效(且没有反斜杠转义)
                                // 如 "\\\\\"\" 我们需要反向检测前方 token 为反斜杠的数量，如果是奇数，说明当前标识符无效。
                                var escapeSymbolsCount = 0;
                                for (var indexForLastEscapeSymbol = indexForNextStringSymbol - 1;
                                     tokens.ElementAt(indexForLastEscapeSymbol).Value.Type == TokenType.SymbolEscape && indexForLastEscapeSymbol > index;
                                     indexForLastEscapeSymbol--)
                                {
                                    escapeSymbolsCount++;
                                }

                                if (escapeSymbolsCount % 2 is 0 or 0)
                                {
                                    endPos = tokenMsgForNextStringSymbol.Key;
                                    // index = indexForNextStringSymbol; 使用这种方法会直接让Index跳到字符串结尾处。
                                    // 我们打算使用区域标识，一个标识符的起始位置和结束位置，这样我们就可以在后续的语法分析中，直接使用这个区域标识。
                                    break;
                                }
                            }

                            if (tokenMsgForNextStringSymbol.Value.Type is not TokenType.SymbolIgnoreLf) continue;
                            isValidStringEndSymbol = false;
                            break;
                        }



                        if (endPos is -1 || !isValidStringEndSymbol)
                        {
                            throw new ScnLexerException($"在位置 {pos} 名为 {tokenMsg.Value.Type} 的 {ScnScriptExceptionMessage.ClosureNotFound}");
                        }

                        area = new Area(tokenMsg.Key, endPos);
                        areaTokenTypes.Add(area, TokenType.SymbolString);
                        areaPositions.Add(area, pos);
                        continue;
                    }

                case TokenType.SymbolComment:
                    {

                        // 1. 我们需要知道这是多行注释还是多行注释 判断联系起来的;数量
                        var isMultiLineComment = false;
                        var commentCount = 1;
                        if (tokens.Count > index + 2)
                        {
                            for (var indexForCommentEnd = index + 1; indexForCommentEnd < tokens.Count && indexForCommentEnd < index + 3; indexForCommentEnd++)
                            {
                                if (tokens.ElementAt(indexForCommentEnd).Value.Type == TokenType.SymbolComment) commentCount++;
                                if (commentCount >= 3) break;
                            }
                        }
                        if (commentCount >= 3) isMultiLineComment = true;

                        switch (isMultiLineComment)
                        {
                            // 单行注释处理
                            case false:
                                {
                                    for (var indexForCommentEnd = index + 1; indexForCommentEnd < tokens.Count; indexForCommentEnd++)
                                    {
                                        var nextEol = tokens.ElementAt(indexForCommentEnd);
                                        if (nextEol.Value.Type != TokenType.SymbolIgnoreLf) continue;
                                        isHaveEol = true;
                                        endPos = nextEol.Key - 1;
                                        break;
                                    }
                                    // 之后没有换行符说明这个注释符号后面全部内容都是注释
                                    if (!isHaveEol) endPos = context.Length - 1;
                                    break;
                                }

                            // 多行注释处理
                            case true:
                                {
                                    // 需要找到下一个 ;;; 结尾
                                    var isHaveCommentEnd = false;
                                    var commentEndCount = 0;
                                    for (var indexForCommentEnd = index + 3; indexForCommentEnd < tokens.Count; indexForCommentEnd++)
                                    {

                                        var nextComment = tokens.ElementAt(indexForCommentEnd);

                                        if (commentEndCount > 0 && nextComment.Value.Type is not TokenType.SymbolComment)
                                        {
                                            commentEndCount = 0;
                                        }

                                        if (nextComment.Value.Type is TokenType.SymbolComment)
                                        {
                                            commentEndCount++;
                                        }


                                        if (commentEndCount < 3) continue;
                                        isHaveCommentEnd = true;
                                        endPos = nextComment.Key;
                                        break;


                                    }
                                    if (!isHaveCommentEnd) throw new ScnLexerException($"在位置 {pos} 名为 {tokenMsg.Value.Type} 的 {ScnScriptExceptionMessage.ClosureNotFound}");
                                    break;
                                }
                        }

                        area = new Area(tokenMsg.Key, endPos);
                        areaTokenTypes.Add(area, TokenType.SymbolComment);
                        areaPositions.Add(area, pos);

                        continue;
                    }

                case TokenType.SymbolSingleQuotesExpression:
                    {
                        // 虽然这里和 string 判断很像，但是Expression的判断只管符号在哪，不管前面有没有\转义符
                        // 还有判断是否在其他区域内与字符串不一样，和其他的都一样
                        var isValidExpressionEndSymbol = true;
                        for (var indexForNextExpressionSymbol = index + 1; indexForNextExpressionSymbol < tokens.Count; indexForNextExpressionSymbol++)
                        {

                            var tokenMsgForNextExpressionSymbol = tokens.ElementAt(indexForNextExpressionSymbol);
                            if (tokenMsgForNextExpressionSymbol.Value.Type is TokenType.SymbolSingleQuotesExpression)
                            {
                                if (LexerHelpers.CheckValidSymbol(areaTokenTypes, tokenMsgForNextExpressionSymbol.Key))
                                {
                                    endPos = tokenMsgForNextExpressionSymbol.Key;
                                    isValidExpressionEndSymbol = true;
                                    break;
                                }
                            }

                            if (tokenMsgForNextExpressionSymbol.Value.Type is not TokenType.SymbolIgnoreLf) continue;
                            isValidExpressionEndSymbol = false;
                            break;
                        }

                        if (endPos is -1 || !isValidExpressionEndSymbol)
                        {
                            throw new ScnLexerException($"在位置 {pos} 名为 {tokenMsg.Value.Type} 的 {ScnScriptExceptionMessage.ClosureNotFound}");
                        }

                        area = new Area(tokenMsg.Key, endPos);
                        areaTokenTypes.Add(area, TokenType.SymbolSingleQuotesExpression);
                        areaPositions.Add(area, pos);
                        continue;
                    }
                // 仅第二轮
                case TokenType.SymbolNodeScene when isLoop2:
                    {
                        // 1.找到行末尾
                        for (var indexForEol = index + 1; indexForEol < tokens.Count; indexForEol++)
                        {
                            var eolToken = tokens.ElementAt(indexForEol);
                            if (eolToken.Value.Type is TokenType.SymbolIgnoreLf)
                            {
                                // 判断该换行符前面是有;，有应该采用,前面作为结尾
                                for (var indexForSemi = indexForEol - 1; indexForSemi > index && indexForSemi < indexForEol; indexForSemi--)
                                {
                                    var semiToken = tokens.ElementAt(indexForSemi);
                                    if (semiToken.Value.Type is not TokenType.SymbolComment) continue;
                                    isSemiEol = true;
                                    isHaveEol = true;
                                    endPos = semiToken.Key - 1;
                                    break;
                                }

                                if (isSemiEol) break;

                                // 判断换行符是否有效
                                isHaveEol = true;
                                if (areaTokenTypes.Any(areaTokenType => areaTokenType.Key.IsInArea(tokenMsg.Key)))
                                {
                                    Console.LogWarning($"已忽略处于编号 {eolToken.Key} 的 {eolToken.Value.Type} 标识符，因为该标识符处于其他区域中");
                                    throw new ScnLexerException($"在位置 {pos} 名为 {tokenMsg.Value.Type} 的 {ScnScriptExceptionMessage.ClosureNotFound} ! 在Node/Scene标识符后的第一个换行符必须为其结尾，注意字符串之中换行无效！字符串必须在同一行结尾");
                                }
                                if (!isHaveEol) continue;
                                endPos = eolToken.Key - 1;
                                break;
                            }
                        }
                        if (!isHaveEol)
                        {
                            endPos = context.Length - 1;
                        }

                        area = new Area(tokenMsg.Key, endPos);
                        areaTokenTypes.Add(area, TokenType.SymbolNodeScene);
                        areaPositions.Add(area, pos);

                        continue;
                    }

                case TokenType.SymbolCommandStart when isLoop2:
                    {
                        for (var indexForCommandEnd = index + 1; indexForCommandEnd < tokens.Count; indexForCommandEnd++)
                        {

                            var commandEndToken = tokens.ElementAt(indexForCommandEnd);
                            if (commandEndToken.Value.Type is TokenType.SymbolCommandEnd)
                            {
                                // 判断是否在其他区域
                                if (!LexerHelpers.CheckValidSymbol(areaTokenTypes, commandEndToken.Key))
                                {
#if DEBUG
                                    Console.LogWarning($"已忽略处于编号 {commandEndToken.Key} 的 {commandEndToken.Value.Type} 标识符，因为该标识符处于其他区域中");
#endif
                                    break;
                                }
                                endPos = commandEndToken.Key;
                                break;
                            }
                            if (commandEndToken.Value.Type is TokenType.SymbolIgnoreLf)
                            {
                                throw new ScnLexerException($"在位置 {pos} 名为 {tokenMsg.Value.Type} 的 {ScnScriptExceptionMessage.ClosureNotFound} ! 在命令开始符号[后的第一个换行符之前必须闭合]，注意不能在字符串之中换行！");
                            }
                        }

                        if (endPos is -1)
                        {
                            throw new ScnLexerException($"在位置 {pos} 名为 {tokenMsg.Value.Type} 的 {ScnScriptExceptionMessage.ClosureNotFound}");
                        }

                        area = new Area(tokenMsg.Key, endPos);
                        areaTokenTypes.Add(area, TokenType.SymbolCommandStart);
                        areaPositions.Add(area, pos);

                        continue;
                    }

                case TokenType.SymbolObject when isLoop2:
                    {
                        // 2024.07.14 修复了@object结尾问题，优化了代码结构，更加简单。
                        // 并且支持了 @xxx: 的格式
                        for (var indexForEndSymbol = index + 1; indexForEndSymbol < tokens.Count; indexForEndSymbol++)
                        {
                            var eolToken = tokens.ElementAt(indexForEndSymbol);
                            var isValid = eolToken.Value.Type is TokenType.SymbolIgnoreLf or TokenType.SymbolComment || (eolToken.Value.Type is TokenType.SymbolColon && LexerConfigs.DefaultSupportObjectColonEnd);
                            if (!isValid) continue;
                            if (LexerHelpers.CheckValidSymbol(areaTokenTypes, eolToken.Key))
                            {
                                isHaveEol = true;
                                endPos = eolToken.Key - 1;
                                if (eolToken.Value.Type is TokenType.SymbolColon)
                                {
                                    endPos = eolToken.Key;
                                }
                                break;
                            }
                            if (eolToken.Value.Type is TokenType.SymbolComment)
                            {
                                isHaveEol = true;
                                endPos = eolToken.Key - 1;
                                break;
                            }
                            Console.LogWarning("!!!!忽略的:" + eolToken.Value.Type);
                        }
                        if (!isHaveEol)
                        {
                            endPos = context.Length - 1;
                        }

                        area = new Area(tokenMsg.Key, endPos);
                        areaTokenTypes.Add(area, TokenType.SymbolObject);
                        areaPositions.Add(area, pos);

                        continue;
                    }

                default:
                    continue;
            }

        }

        if (!isLoop2)
        {
            isLoop2 = true;
            // 千万要记得给位置复位！！！！！
            pos = new Position(LexerConfigs.DefaultPositionLineStart, LexerConfigs.DefaultPositionColumnStart);
            lastColumnEndInTokenKey = 0;
#if DEBUG
            Console.LogWarning("第二轮解析开始!");
#endif
            goto LexerLoop;
        }

        var test = areaTokenTypes.OrderBy(x => x.Key.Start).ToDictionary(x => x.Key, x => x.Value);
        // Debug: 打印所有区域标识
#if DEBUG
        foreach (var areaTokenType in test)
        {
            // WARN: Length必须+1，原因是""之间长度为1，所以不加时会显示"。千万要记得加1!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Console.LogInfo($"报告 {areaTokenType.Key} 为 {areaTokenType.Value} 类型 内容 {context.Substring(areaTokenType.Key.Start, areaTokenType.Key.GetLength())}");
        }
#endif

        return new LexerResult(areaTokenTypes, areaPositions);
    }
    /// <summary>
    /// 执行词法分析(需要提供符号位置)
    /// </summary>
    /// <param name="context">原始文本</param>
    /// <param name="tokens">符号位置</param>
    /// <returns>词法分析结果</returns>
    /// <exception cref="ScnLexerException">解析异常</exception>
    private static LexerResult Lexer(string context, Dictionary<int, Token> tokens)
    {
        // 区域标识存储字典 (区域,标识类型)
        Dictionary<Area, TokenType> areaTokenTypes = new();
        // 标识区域开始位置
        Dictionary<Area, Position> areaPositions = new();

        // 关于定位: 当前位置
        var pos = new Position(LexerConfigs.DefaultPositionLineStart, LexerConfigs.DefaultPositionColumnStart);
        // 关于定位: 记录上一个标识符的结束位置
        var lastColumnEndInTokenKey = 0;

        // 状态需要
        var outPos = new Position(LexerConfigs.DefaultPositionLineStart, LexerConfigs.DefaultPositionColumnStart);
        var status = TokenType.SymbolUnknown;
        var startPos = -1;
        var endPos = -1;
        var escapeCount = 0;
        var escapeIndex = -1;
        var commentCount = 0;
        var commentIndex = -1;
        var commentMutiLine = false;
        var commentFinish = true;
        var noSymbolString = false;

        // 区域标识存储字典 (区域,标识类型)
        Dictionary<Area, TokenType> sub_areaTokenTypes = new();
        // 标识区域开始位置
        Dictionary<Area, Position> sub_areaPositions = new();

        var sub_status = TokenType.SymbolUnknown;
        var sub_startPos = -1;
        var sub_endPos = -1;
        var sub_outPos = new Position(LexerConfigs.DefaultPositionLineStart, LexerConfigs.DefaultPositionColumnStart);

        void AddAreaTokenTypes()
        {
            if (startPos == -1 || endPos == -1)
            {
                throw new ScnLexerException("错误的触发时机！");
            }
            if (startPos == endPos)
            {
                throw new ScnLexerException("错误的数据长度！");
            }
            var area = new Area(startPos, endPos);
            areaTokenTypes.Add(area, status);
            areaPositions.Add(area, outPos);
            startPos = -1;
            endPos = -1;
            status = TokenType.SymbolUnknown;
            commentCount = 0;
            escapeCount = 0;

            foreach (var item in sub_areaTokenTypes)
            {
                areaTokenTypes.Add(item.Key, item.Value);
            }
            sub_areaTokenTypes.Clear();
            foreach (var item in sub_areaPositions)
            {
                areaPositions.Add(item.Key, item.Value);
            }
            sub_areaPositions.Clear();
        }
        void AddSubAreaTokenTypes()
        {
            if (sub_startPos == -1 || sub_endPos == -1)
            {
                throw new ScnLexerException("错误的触发时机！");
            }
            if (sub_startPos == sub_endPos)
            {
                throw new ScnLexerException("错误的数据长度！");
            }
            var area = new Area(sub_startPos, sub_endPos);
            sub_areaTokenTypes.Add(area, sub_status);
            sub_areaPositions.Add(area, sub_outPos);
            sub_startPos = -1;
            sub_endPos = -1;
            sub_status = TokenType.SymbolUnknown;
        }
        void TryAddString(int _endPos)
        {
            if (noSymbolString && startPos != -1)
            {
                status=TokenType.SymbolString;
                endPos = _endPos-1;
                AddAreaTokenTypes();
            }
        }
        // 注意 我们不要按照字典的 key 遍历，我们需要知道这是字典的第几项，这样可以方便我们获取以后的字典项
        for (var index = 0; index < tokens.Count; index++)
        {
            var tokenMsg = tokens.ElementAt(index);

            #region 位置处理

            if (tokenMsg.Value.Type == TokenType.SymbolIgnoreLf)
            {
                pos.Line++;
                pos.Column = LexerConfigs.DefaultPositionResetColumnWhenNewLine;
                if (tokenMsg.Key > lastColumnEndInTokenKey)
                {
                    lastColumnEndInTokenKey = tokenMsg.Key;
                }
            }
            else
            {
                pos.Column = tokenMsg.Key - lastColumnEndInTokenKey;
            }

            #endregion

            // 这里是 2024.8.28 日重写逻辑。我们打算使用状态机处理
            // 这里是当天中午的Author，我们完成了这次改变。
            // 需要严格注意的是为什么需要在if处理完成后 continue 
            // 注意执行循序，if运行之后不会立刻返回会执行后面的代码，你如果不这样做。。。。
            // 直接疯狂报ScnLexerException错误的数据长度！
            // 这一次改变提高起码千倍效率
            // 以前的方法在 LexerOld 里，它采用了字典的遍历，查找上下文结尾，效率极低
            // 而现在我们采用了状态机，效率大大提高，省去很多不必要的遍历查询。

            if (noSymbolString)
            {
                Console.WriteLine(tokenMsg.Value.Value.ToString());
            }

            switch (tokenMsg.Value.Type)
            {
                case TokenType.SymbolString:
                    if(status is TokenType.SymbolUnknown)
                    {
                        status = TokenType.SymbolString;
                        startPos = tokenMsg.Key;
                        outPos = pos;
                        continue;
                    }
                    if(status is TokenType.SymbolString)
                    {
                        // 检查是否是有效的结尾（不被转义）
                        if(escapeCount is 0)
                        {
                            endPos = tokenMsg.Key;
                            AddAreaTokenTypes();
                        }
                        else
                        {
                            escapeCount = 0;
                            escapeIndex = -1;
                        }
                        continue;
                    }
                    // 处理命令以及场景标识内部的字符串
                    if(status is TokenType.SymbolCommandStart or TokenType.SymbolObject or TokenType.SymbolNodeScene)
                    {
                        if(sub_status is TokenType.SymbolUnknown)
                        {
                            sub_status = TokenType.SymbolString;
                            sub_startPos = tokenMsg.Key;
                            sub_outPos = pos;
                            continue;
                        }
                        if (sub_status is TokenType.SymbolString)
                        {
                            if (escapeCount is 0)
                            {
                                sub_endPos = tokenMsg.Key;
                                AddSubAreaTokenTypes();
                            }
                            continue;
                        }
                    }
                    continue;
                case TokenType.SymbolEscape:
                    if (status is TokenType.SymbolString || sub_status is TokenType.SymbolString)
                    {
                        // 如果转义字符数量为1，则说明是第一个转义字符，否则说明是第二个转义字符（即转义结束）
                        if (escapeCount >= 1) {
                            escapeCount = 0;
                            escapeIndex = -1;
                            continue;
                        }
                        escapeCount++;
                        escapeIndex = tokenMsg.Key;
                    }
                    continue;
                case TokenType.SymbolComment:
                    // 处理默认支持注释截断代码
                    if (LexerConfigs.DefaultSupportForRuncatingCodeWithComments)
                    {
                        if (status is TokenType.SymbolObject or TokenType.SymbolNodeScene)
                        {
                            if (sub_status is TokenType.SymbolUnknown)
                            {
                                endPos = tokenMsg.Key - 1;
                                AddAreaTokenTypes();
                            }
                        }
                    }

                    // 处理 ;;; xxx ;;; 或者 ; 开头的单行注释
                    if (status is TokenType.SymbolUnknown)
                    {
                        status = TokenType.SymbolComment;
                        startPos = tokenMsg.Key;
                        outPos = pos;
                        commentCount = 1;
                        commentIndex = tokenMsg.Key;
                        commentMutiLine = false;
                        continue;
                    }
                    if (status is TokenType.SymbolComment)
                    {
                        commentCount++;
                        
                        if (commentCount is 3)
                        {
                            if (!commentMutiLine)
                            {
                                // 多行注释开始
                                commentCount = 0;
                                commentMutiLine = true;
                                commentFinish = false;
                                commentIndex = -1;
                            }
                            else
                            {
                                // 多行注释结束(注意单行注释结束由换行符处理)
                                commentCount = 0;
                                endPos = tokenMsg.Key;
                                commentMutiLine = false;
                                commentFinish = true;
                                commentIndex = -1;
                                AddAreaTokenTypes();
                            }
                            continue;
                        }

                        // 是否是连续的注释;符号
                        if (commentIndex != (tokenMsg.Key - 1))
                        {
                            // 不是开始的连续符号才支持修改 commentIndex
                            if (commentMutiLine)
                            {
                                commentCount = 1;
                                commentIndex = tokenMsg.Key;
                            }
                        }
                        
                    }
                    continue;
                case TokenType.SymbolSingleQuotesExpression:
                    if (status is TokenType.SymbolUnknown)
                    {
                        status = TokenType.SymbolSingleQuotesExpression;
                        startPos = tokenMsg.Key;
                        outPos = pos;
                        continue;
                    }
                    if (status is TokenType.SymbolSingleQuotesExpression)
                    {
                        endPos = tokenMsg.Key;
                        AddAreaTokenTypes();
                        continue;
                    }
                    // 处理命令以及场景标识内部的字符串
                    if (status is TokenType.SymbolCommandStart or TokenType.SymbolObject or TokenType.SymbolNodeScene)
                    {
                        if (sub_status is TokenType.SymbolUnknown)
                        {
                            sub_status = TokenType.SymbolSingleQuotesExpression;
                            sub_startPos = tokenMsg.Key;
                            sub_outPos = pos;
                            continue;
                        }
                        if (sub_status is TokenType.SymbolSingleQuotesExpression)
                        {
                            sub_endPos = tokenMsg.Key;
                            AddSubAreaTokenTypes();
                            continue;
                        }
                    }
                    continue;
                case TokenType.SymbolNodeScene:
                    if (status is TokenType.SymbolUnknown)
                    {
                        status = TokenType.SymbolNodeScene;
                        startPos = tokenMsg.Key;
                        outPos = pos;
                    }
                    continue;
                case TokenType.SymbolCommandStart:
                    if (status is TokenType.SymbolUnknown)
                    {
                        status = TokenType.SymbolCommandStart;
                        startPos = tokenMsg.Key;
                        outPos = pos;
                    }
                    continue;
                case TokenType.SymbolCommandEnd:
                    if (status is TokenType.SymbolCommandStart)
                    {
                        endPos = tokenMsg.Key;
                        AddAreaTokenTypes();
                    }
                    continue;
                case TokenType.SymbolObject:
                    if (status is TokenType.SymbolUnknown)
                    {
                        status = TokenType.SymbolObject;
                        startPos = tokenMsg.Key;
                        outPos = pos;
                    }
                    continue;
                case TokenType.SymbolColon:
                    if (status is TokenType.SymbolObject && LexerConfigs.DefaultSupportObjectColonEnd)
                    {
                        endPos = tokenMsg.Key;
                        AddAreaTokenTypes();
                    }
                    continue;
                case TokenType.SymbolWhitespace:
                    break;
                case TokenType.SymbolIgnoreLf:
                    if (status is TokenType.SymbolString)
                    {
                        if (escapeCount is 1)
                        {
                            escapeCount = 0;
                            escapeIndex = -1;
                        }
                        continue;
                    }
                    if (status is TokenType.SymbolComment or TokenType.SymbolObject or TokenType.SymbolNodeScene)
                    {
                        if (commentMutiLine) continue;

                        endPos = tokenMsg.Key -1;
                        AddAreaTokenTypes();
                        continue;
                    }
                    if(status is TokenType.SymbolSingleQuotesExpression)
                    {
                        throw new ScnLexerException($"单引号表达式未结束，请检查处于 {outPos} 的单引号表达式是否在同行结束！");
                    }
                    if (sub_status is TokenType.SymbolSingleQuotesExpression)
                    {
                        throw new ScnLexerException($"单引号表达式未结束，请检查处于 {sub_outPos} 的单引号表达式是否在同行结束！");
                    }
                    if (sub_status is TokenType.SymbolString)
                    {
                        throw new ScnLexerException($"字符串未结束，请检查处于 {outPos} 的字符串是否在同行结束！属于参数的字符串不支持换行！");
                    }
                    continue;

                default:
                    if (status is TokenType.SymbolString)
                    {
                        if (escapeCount >= 1)
                        {
                            escapeCount = 0;
                            escapeIndex = -1;
                        }
                    }
                    if(status is TokenType.SymbolUnknown)
                    {
                        if (!noSymbolString)
                        {
                            noSymbolString = true;
                            startPos = tokenMsg.Key;
                            outPos = pos;
                        }
                        
                    }
                    continue;
            }
        }
        if (startPos is not -1 && endPos is -1)
        {
            if (!commentFinish) throw new ScnLexerException("注释没有闭合！");
            endPos = context.Length -1;
            AddAreaTokenTypes();
        }
        // Debug: 打印所有区域标识
#if DEBUG
        foreach (var areaTokenType in areaTokenTypes)
        {
            // WARN: Length必须+1，原因是""之间长度为1，所以不加时会显示"。千万要记得加1!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Console.LogInfo($"报告 {areaTokenType.Key} 为 {areaTokenType.Value} 类型 内容 {context.Substring(areaTokenType.Key.Start, areaTokenType.Key.GetLength())}");
        }
#endif
        return new LexerResult(areaTokenTypes, areaPositions);
    }
    /// <summary>
    /// 执行词法分析(帮你获取符号位置)
    /// </summary>
    /// <param name="context">原始文本</param>
    /// <returns>词法分析结果</returns>
    [Obsolete]
    public static LexerResult LexerOld(string context)
    {
        return LexerOld(context, LexerHelpers.GetSymbolPositions(context));
    }
    /// <summary>
    /// 执行词法分析(帮你获取符号位置)
    /// </summary>
    /// <param name="context">原始文本</param>
    /// <returns>词法分析结果</returns>
    public static LexerResult Lexer(string context)
    {
        return Lexer(context, LexerHelpers.GetSymbolPositions(context));
    }
    /// <summary>
    /// 执行词法分析(帮你获取符号位置)
    /// </summary>
    /// <param name="context">原始文本</param>
    /// <param name="skipIndex">开始分析的位置</param>
    /// <returns>词法分析结果</returns>
    public static LexerResult Lexer(string context,int skipIndex)
    {
        return Lexer(context, LexerHelpers.GetSymbolPositions(context,skipIndex));
    }
}
#endif