namespace ScnScript;
public static partial class ScnScriptCommon;
internal static class LexerHelpers
{

    // 判断是否在区域内
    /// <summary>
    /// 判断符号是否在区域内
    /// </summary>
    /// <param name="areaTokenTypes">区域标识存储字典 (区域,标识类型)</param>
    /// <param name="pos">位置</param>
    internal static bool CheckValidSymbol(Dictionary<Area, TokenType> areaTokenTypes, int pos)
    {
        return !areaTokenTypes.Any(areaTokenType => areaTokenType.Key.IsInArea(pos));
    }
    // 获取符号位置
    /// <summary>
    /// 获取符号位置
    /// </summary>
    /// <param name="context">代码文本</param>
    /// <param name="skipIndex">跳过索引(开始位置)</param>
    /// <returns>位置字典</returns>
    internal static Dictionary<int, Token> GetSymbolPositions(string context, int skipIndex=0)
    {
        var tokens = new Dictionary<int, Token>();

        // 创建一个查找表，假设TokenType的值和字符是一一对应的
        var tokenLookup = new Dictionary<char, TokenType>();
        // 将枚举值添加到查找表中
        foreach (TokenType tokenType in Enum.GetValues<TokenType>())
        {
            tokenLookup.Add((char)tokenType, tokenType);
        }
        // 遍历代码文本，查找符号
        for (var i = skipIndex; i < context.Length; i++)
        {
            var symbol = context[i];

            // 如果符号在查找表中，则将其添加到 tokens 字典中
            if (tokenLookup.TryGetValue(symbol, out TokenType tokenType))
            {
                tokens.Add(i, new Token(tokenType, symbol));
            }
        }
        return tokens;
    }
}