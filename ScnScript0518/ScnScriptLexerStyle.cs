using System;
using System.Collections.Generic;

public class Token
{
    public string Type { get; set; }
    public string Value { get; set; }

    public override string ToString()
    {
        return $"Token({Type}, {Value})";
    }
}
//表达位置的数据结构
public class Position
{
    public int Line { get; set; }
    public int Column { get; set; }

    public override string ToString()
    {
        return $"({Line}, {Column})";
    }
}

public class TokenGenerator
{
    private readonly string _tokenType;

    public TokenGenerator(string tokenType)
    {
        _tokenType = tokenType;
    }

    public Token GenerateToken(string input)
    {
        return new Token
        {
            Type = _tokenType,
            Value = input
        };
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var tokenGenerator = new TokenGenerator("example");
        var token = tokenGenerator.GenerateToken("Hello, World!");
        Console.WriteLine(token);
    }
}