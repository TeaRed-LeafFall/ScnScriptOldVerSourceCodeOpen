namespace ScnScript;

public static partial class ScnScriptCommon;
public static class ScnScriptExceptionMessage
{
    // 语法错误
    public const string GrammarError = "存在语法错误";
    public const string ClosureNotFound = "闭合未找到";
    public const string IllegalCharacter = "存在非法字符";
    // 运行时错误
    public const string NotSupportCommand = "不受支持的命令，命令已弃用或者不支持您的运行平台";
    public const string UnknownCommand = "未知命令";
    public const string UnknownKeyword = "未知对象关键字";
    public const string UnknownVariable = "未知变量";
    public const string UnknownFunction = "未知函数";
    public const string UnknownClass = "未知类";
    public const string UnknownType = "未知类型";
    public const string UnknownParameters = "未知参数";
    public const string UnknownProperty = "未知属性";

}

// 词法分析异常
public class ScnLexerException(string message) : Exception(message);

// 解析器分析异常
public class ScnParserException(string message) : Exception(message);

// 运行时异常
public class ScnRuntimeException(string message) : Exception(message);