using ScnScript.Data;
using ScnScript.Helpers;

namespace ScnScript.CodeAnalysis;

public static partial class ScriptCodeAnalysis
{
    // 完成时间： 2024年9月16日16点07分
    /// <summary>
    /// 词法分析器
    /// </summary>
    /// <param name="source">源代码</param>
    private static Dictionary<Position, ScriptToken> Lexer(string source)
    {
        var timer = new EasyTimer();
        timer.Start();

        var dic = new Dictionary<Position, ScriptToken>();

        Position position = Position.None;

        // 填充符号表
        var typeLookup = new Dictionary<char, TokenKind>();
        foreach (TokenKind tokenType in Enum.GetValues<TokenKind>())
        {
            if (tokenType is TokenKind.Unknown or TokenKind.Keyword) continue;
            typeLookup.Add((char)tokenType, tokenType);
        }

        var status = LexerStatus.None;
        //var token = new ScriptToken();

        Position startPosition = Position.None;
        var startIndex = -1;
        var escapeCount = 0;
        var commentCount = 0;
        var isMutiLineComment = false;
        var isSureLineComment = false;
        for (var i = 0; i < source.Length; i++)
        {
            // 获取char
            var c = source[i];
            // 移动列位置
            position.Column++;

            // 化繁为简： 使用查找表获取 Kind 灵活，代替写死的判断
            TokenKind symbol = typeLookup.GetValueOrDefault(c, TokenKind.Unknown);

            // 处理字符串逃脱
            if (status is LexerStatus.InString)
            {
                if (symbol is not TokenKind.Escape and not TokenKind.DoubleQuotationMarks)
                {
                    escapeCount = 0;
                }
            }

            // 处理字符
            switch (symbol)
            {
                // 标准
                case TokenKind.DoubleQuotationMarks:
                    HandleKeywordTokens(source, ref dic, ref startPosition, ref status, ref startIndex, i - 1);
                    HandleStringTokens(source, ref dic, ref position, ref status, ref startPosition, ref startIndex, ref escapeCount, i, symbol);
                    break;
                case TokenKind.SingleQuotationMarks:
                    HandleKeywordTokens(source, ref dic, ref startPosition, ref status, ref startIndex, i - 1);
                    HandleExpressionTokens(source, ref dic, ref position, ref status, ref startPosition, ref startIndex, i, symbol);
                    break;
                case TokenKind.Whitespace:
                    HandleKeywordTokens(source, ref dic, ref startPosition, ref status, ref startIndex, i - 1);
                    //HandleSingleTokens(ref dic, ref position, ref status, symbol);
                    break;
                case TokenKind.SemiColon:
                    HandleKeywordTokens(source, ref dic, ref startPosition, ref status, ref startIndex, i - 1);
                    //HandleSingleTokens(ref dic, ref position, ref status, symbol);
                    HandleCommentTokens(source, ref dic, ref position, ref status, ref startPosition, ref startIndex, ref commentCount, ref isMutiLineComment, ref isSureLineComment, i, symbol);
                    break;
                case TokenKind.Colon:
                case TokenKind.At:
                case TokenKind.Equal:
                case TokenKind.OpenParenthesis:
                case TokenKind.CloseParenthesis:
                case TokenKind.OpenBrace:
                case TokenKind.CloseBrace:
                case TokenKind.OpenBracket:
                case TokenKind.CloseBracket:
                case TokenKind.Add:
                case TokenKind.Minus:
                case TokenKind.Multiply:
                case TokenKind.Divide:
                case TokenKind.And:
                case TokenKind.Or:
                case TokenKind.Not:
                case TokenKind.Xor:
                case TokenKind.LessThan:
                case TokenKind.GreaterThan:
                case TokenKind.QuestionMark:
                case TokenKind.Cr:
                case TokenKind.Tab:
                    HandleKeywordTokens(source, ref dic, ref startPosition, ref status, ref startIndex, i - 1);
                    HandleSingleTokens(ref dic, ref position, ref status, symbol);
                    break;
                case TokenKind.Lf:
                    HandleCommentTokensTryEnd(source, ref dic, ref status, ref startPosition, ref startIndex, ref isMutiLineComment, ref commentCount, ref isSureLineComment, i - 1);
                    HandleKeywordTokens(source, ref dic, ref startPosition, ref status, ref startIndex, i - 1);
                    HandleSingleTokens(ref dic, ref position, ref status, symbol);
                    break;
                case TokenKind.Escape:
                    // 字符串逃脱处理
                    if (status is LexerStatus.InString)
                    {
                        if (escapeCount is 1)
                        {
                            escapeCount = 0;
                        }
                        else
                        {
                            escapeCount++;
                        }

                    }
                    break;
                // 通用
                default:
                    if (status is LexerStatus.None)
                    {
                        status = LexerStatus.InKeyword;
                        startIndex = i;
                        startPosition = new Position(position);
                    }
                    break;
            }

            // 处理换行
            if (symbol is not TokenKind.Lf) continue;
            position.Line++;
            position.Column = 0;
        }

        if (status is LexerStatus.InKeyword)
        {
            HandleKeywordTokens(source, ref dic, ref startPosition, ref status, ref startIndex, source.Length - 1);
        }
        if (status is LexerStatus.InComment)
        {
            HandleCommentTokensTryEnd(source, ref dic, ref status, ref startPosition, ref startIndex, ref isMutiLineComment, ref commentCount, ref isSureLineComment, source.Length - 1);
        }

        if (status is not LexerStatus.None)
        {
            throw status switch {
                LexerStatus.InString => new Exception($"字符串未结束，处于 {startPosition} 位置"),
                LexerStatus.InComment => new Exception($"注释未结束，处于 {startPosition} 位置。这可能是由于多行注释闭合的符号数量不对造成的，请检查您的脚本！"),
                _ => new Exception($"状态异常，未退出 {status} 状态")
            };

        }

        Console.WriteLine();


        timer.StopWithShowTime("Lexer 解释完成");

        return dic;


    }
    private static void HandleSingleTokens(ref Dictionary<Position, ScriptToken> dic, ref Position position, ref LexerStatus status, TokenKind symbol)
    {
        if (status is not LexerStatus.None) return;
        var token = new ScriptToken
        {
            Kind = symbol
        };
        dic.Add(new(position), token);
        //Console.WriteLine($"发现处于 {position} 的 {token}");
    }
    private static void HandleStringTokens(string source, ref Dictionary<Position, ScriptToken> dic, ref Position position, ref LexerStatus status, ref Position startPosition, ref int startIndex, ref int escapeCount, int i, TokenKind symbol)
    {
        switch (status)
        {
            case LexerStatus.None:
                status = LexerStatus.InString;
                startIndex = i;
                startPosition = new Position(position);
                break;

            // 判断是否是被转义字符
            case LexerStatus.InString when escapeCount is 1:
                escapeCount = 0;
                return;

            //Console.WriteLine($"发现处于 {startPosition} 的 {token}");
            case LexerStatus.InString:
            {
                var tokenString = source.Substring(startIndex + 1, i - startIndex - 1);
                status = LexerStatus.None;
                startIndex = -1;
                var token = new ScriptToken
                {
                    Kind = symbol,
                    Text = tokenString
                };
                dic.Add(startPosition, token);
                break;
            }
        }
    }
    private static void HandleExpressionTokens(string source, ref Dictionary<Position, ScriptToken> dic, ref Position position, ref LexerStatus status, ref Position startPosition, ref int startIndex, int i, TokenKind symbol)
    {
        switch (status)
        {
            case LexerStatus.None:
                status = LexerStatus.InExpression;
                startIndex = i;
                startPosition = new Position(position);
                break;

            case LexerStatus.InExpression:
            {
                status = LexerStatus.None;
                var tokenString = source.Substring(startIndex + 1, i - startIndex - 1);
                var token = new ScriptToken
                {
                    Kind = symbol,
                    Text = tokenString
                };
                dic.Add(startPosition, token);
                //Console.WriteLine($"发现处于 {startPosition} 的 {token}");
                break;
            }
        }
    }
    private static void HandleKeywordTokens(string source, ref Dictionary<Position, ScriptToken> dic, ref Position startPosition, ref LexerStatus status, ref int startIndex, int i)
    {
        if (status is not LexerStatus.InKeyword) return;
        var token = new ScriptToken
        {
            Kind = TokenKind.Keyword,
            Text = source.Substring(startIndex, i - startIndex + 1)
        };
        status = LexerStatus.None;
        startIndex = -1;
        dic.Add(new Position(startPosition), token);
        //Console.WriteLine($"发现处于 {startPosition} 的 {token}");
    }
    private static void HandleCommentTokens(string source, ref Dictionary<Position, ScriptToken> dic, ref Position position, ref LexerStatus status, ref Position startPosition, ref int startIndex, ref int commentCount, ref bool isMutiLineComment, ref bool isSureLineComment, int i, TokenKind symbol)
    {
        switch (status)
        {
            case LexerStatus.None:
                status = LexerStatus.InComment;
                startIndex = i;
                startPosition = new Position(position);
                break;

            case LexerStatus.InComment when isSureLineComment:
                return;

            case LexerStatus.InComment when !isMutiLineComment:
            {
                // ;;; 开始
                if (i == startIndex + 1 || i == startIndex + 2)
                {
                    commentCount++;
                    if (commentCount == 2)
                    {
                        isMutiLineComment = true;
                        commentCount = 0;
                    }
                }
                else
                {
                    // 如果开始是;;;的绝对不会运行到这里，运行到这里说明;之后存在杂项，并不连续
                    commentCount = 0;
                    isSureLineComment = true;
                }
                break;
            }

            case LexerStatus.InComment:
            {
                // ;;; 结束
                if (commentCount < 3)
                {
                    commentCount++;
                }

                if (commentCount == 3)
                {
                    commentCount = 0;
                    isSureLineComment = false;
                    isMutiLineComment = false;

                    var token = new ScriptToken
                    {
                        Kind = symbol,
                        Text = source.Substring(startIndex, i - startIndex + 1),
                    };
                    startIndex = -1;
                    status = LexerStatus.None;
                    dic.Add(new Position(startPosition), token);
                    //Console.WriteLine($"发现处于 {startPosition} 的 {token}");
                }
                break;
            }
        }
    }
    private static void HandleCommentTokensTryEnd(string source, ref Dictionary<Position, ScriptToken> dic, ref LexerStatus status, ref Position startPosition, ref int startIndex, ref bool isMutiLineComment, ref int commentCount, ref bool isSureLineComment, int i)
    {
        if (status is not LexerStatus.InComment) return;
        if (isMutiLineComment) return;
        commentCount = 0;
        isSureLineComment = false;
        isMutiLineComment = false;

        var token = new ScriptToken
        {
            Kind = TokenKind.SemiColon,
            Text = source.Substring(startIndex, i - startIndex + 1),
        };
        startIndex = -1;
        status = LexerStatus.None;
        dic.Add(new Position(startPosition), token);
        //Console.WriteLine($"发现处于 {startPosition} 的 {token}");
    }

}

public enum LexerStatus
{
    None,
    InString,
    InExpression,
    InKeyword,
    InComment,
}