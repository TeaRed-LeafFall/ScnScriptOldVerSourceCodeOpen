namespace ScnScript;

public static partial class ScnScriptCommon;
// Token
public struct Token
{
    public char Value { get; set; } = ' ';
    public TokenType Type { get; set; } = TokenType.SymbolWhitespace;

    public Token(TokenType type, char value)
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
    SymbolIgnoreLf = '\n',
    SymbolColon = ':',
    SymbolWhitespace = ' ',
    SymbolUnknown = '\0'
}