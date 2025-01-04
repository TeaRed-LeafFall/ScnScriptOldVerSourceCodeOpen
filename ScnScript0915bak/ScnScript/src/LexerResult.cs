namespace ScnScript;

public static partial class ScnScriptCommon;

public readonly struct LexerResult(Dictionary<Area, TokenType> areaTokenTypes, Dictionary<Area, Position> areaPositions)
{
    public Dictionary<Area, TokenType> AreaTokenTypes { get; init; }= areaTokenTypes;
    public Dictionary<Area, Position> AreaPositions { get; init; } = areaPositions;
}