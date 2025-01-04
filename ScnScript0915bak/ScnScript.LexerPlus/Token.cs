namespace ScnScript;

public static partial class ScnScriptCommon;
//// Token
public struct Token
{
    public string Value { get; set; } = string.Empty;
    public TokenType Type { get; set; } = TokenType.SymbolWhitespace;

    public Token(TokenType type, string value)
    {
        this.Type = type;
        this.Value = value;
    }
}

public enum TokenType
{
    SymbolDoubleQuotationMarks = '"',
    SymbolSingleQuotationMarks = '\'',
    SymbolSemiColon = ';',
    SymbolColon = ':',
    SymbolWhitespace = ' ',
    SymbolAt = '@',
    SymbolDot = '.',
    SymbolEqual = '=',
    SymbolComma = ',',
    SymbolOpenParenthesis = '(',
    SymbolCloseParenthesis = ')',
    SymbolOpenBrace = '{',
    SymbolCloseBrace = '}',
    SymbolOpenBracket = '[',
    SymbolCloseBracket = ']',
    SymbolAdd = '+',
    SymbolSubtract = '-',
    SymbolMultiply = '*',
    SymbolDivide = '/',
    SymbolModulo = '%',
    SymbolPower = '^',
    SymbolAnd = '&',
    SymbolOr = '|',
    SymbolNot = '!',
    SymbolXor = '~',
    SymbolLessThan = '<',
    SymbolGreaterThan = '>',
    SymbolEscape = '\\',
    SymbolLf = '\n',
    SymbolCr = '\r',
    SymbolTab = '\t',
    SymbolUnknown = '\0'
}