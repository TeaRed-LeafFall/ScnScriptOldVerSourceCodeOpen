
using System.Collections.ObjectModel;
using ScnScript.Data;

namespace ScnScript.CodeAnalysis;

/// <summary>
/// 提供分析器诊断信息：也就是代码在编译过程中发现的问题
/// </summary>
public class ProblemProvider
{
    private List<ProblemInfo> problems = [];
    private Position currentPosition = new(-1, -1);

    private void CheckError(ProblemLevel level)
    {
        if (level is ProblemLevel.Error)
        {
            HasError = true;
        }
    }
    private void CheckError(ProblemInfo problem)
    {
        if (problem.Level is ProblemLevel.Error)
        {
            HasError = true;
        }
    }
    public void Report(ProblemInfo problem)
    {
        CheckError(problem);
        problems.Add(problem);
    }
    public void Report(ProblemLevel level, string message)
    {
        CheckError(level);
        problems.Add(new ProblemInfo
        {
            Level = level,
            Message = message,
            Position = currentPosition,
        });
    }
    public void ReportError(string message)
    {
        CheckError(ProblemLevel.Error);
        problems.Add(new ProblemInfo
        {
            Level = ProblemLevel.Error,
            Message = message,
            Position = currentPosition,
        });
    }
    public void ResetPosition(Position position)
    {
        currentPosition = position;
    }
    public void Clear()
    {
        problems.Clear();
    }
    public ReadOnlyCollection<ProblemInfo> GetProblems()
    {
        return problems.AsReadOnly();
    }
    public bool HasError { get; private set; }
}

public class ProblemInfo
{
    public ProblemLevel Level { get; init; }
    public string Message { get; init; } = string.Empty;
    //public int Id { get; set; }
    public Position Position { get; init; } = new(-1, -1);
    public override string ToString()
    {
        return $"[{Level}] {Message} at {Position}";
    }
}
public enum ProblemLevel
{
    Error,
    Warning,
    Info,
    Suggest
}