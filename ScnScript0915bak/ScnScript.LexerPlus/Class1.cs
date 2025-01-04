using System.Diagnostics;

namespace ScnScript.LexerPlus;

public static class PlusClass
{
    public struct LexerOut(Dictionary<Area, Token> keywords, Dictionary<Area, Position> areaPositions)
    {
        public Dictionary<Area, Token> Keywords = keywords;
        public Dictionary<Area, Position> AreaPositions { get; init; } = areaPositions;
    }
    public static LexerOut Lexer(string input)
    {
        var keywords = new Dictionary<Area, Token>();
        var positions = new Dictionary<Area, Position>();
        var keyStart = -1;
        var keyEnd = -1;
        var pos = new Position(1, 1);

        var typeLookup = new Dictionary<char, TokenType>();
        foreach (TokenType tokenType in Enum.GetValues<TokenType>())
        {
            typeLookup.Add((char)tokenType, tokenType);
        }

        void PushItem(TokenType symbol)
        {
            if (keyStart > -1)
            {
                var key = new Area(keyStart, keyEnd);
                keywords.Add(key, new Token(symbol, input.Substring(key.Start, key.GetLength())));
                positions.Add(key, pos);
                Console.WriteLine($"{key} {pos} {symbol} {input.Substring(key.Start, key.GetLength())}");
            }
            keyStart = -1;
            keyEnd = -1;
        }
        var lastsymbol = TokenType.SymbolUnknown;
        for (int i = 0; i <= input.Length; i++)
        {
            if (i > input.Length -1) break;
            char item = input[i];
            pos.Column++;
            var symbol = TokenType.SymbolUnknown;
            var vaildSymbol=typeLookup.TryGetValue(item, out symbol);
            switch (symbol)
            {
                case TokenType.SymbolLf:
                    pos.Line++;
                    pos.Column = 1;
                    keyEnd = i;
                    PushItem(lastsymbol);
                    break;
                case TokenType.SymbolWhitespace:
                    if(keyStart != -1)
                    {
                        keyEnd = i;
                        PushItem(lastsymbol);
                    }
                    
                    break;
                default:
                    
                    if (keyStart == -1)
                    {
                        lastsymbol = symbol;
                    }
                    if (vaildSymbol)
                    {
                        if(keyStart != -1)
                        {
                            if(keyStart != i)
                            {
                                keyEnd = i - 1;
                                PushItem(lastsymbol);
                            }
                            
                            keyEnd = i;
                            PushItem(lastsymbol);
                            lastsymbol = TokenType.SymbolUnknown;
                        }
                        else
                        {
                            keyStart = i;
                        }
                        
                    }
                    break;
            }
        }
        keyEnd = input.Length -1;
        PushItem(lastsymbol);
        return new(keywords,positions);
    }
}
