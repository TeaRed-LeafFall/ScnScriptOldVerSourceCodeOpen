
namespace ScnScript;
#region 语法讲解
/*
    对象选择器：以@开头，用于标识对象或函数。例如：@MyClass。

    节点/章节：使用*开头，表示一个命名空间或章节。例如：*Test。

    注释：以;开头，用于注释说明和文档。

    特殊关键字：如@new、@func、@clr等，表示特定的功能或操作。

    全局命令：通过[]闭合，对应操作[(类) 命令 (| 参数...)]

    清除对象选择器：使用@clr清除之前的对象选择器。后面可以指定取消的对象

    结束与开始章节：使用| End和| Start表示章节的结尾和开头。
*/
#endregion

#region  此代码已经弃用，将在未来版本移除
#if NotSupported
public class ScnLexer
{
    private List<string> codeLines=new();
    private readonly List<Token> tokens = new();
    public Dictionary<int, int> RealLineCount = new();
    private int realIndex;
    public void InputString(string s)
    {
        realIndex = 0;
        tokens.Clear();
        codeLines.Clear();
        // 使用 Environment.NewLine 分割字符串，并移除空字符串
        var strings = s.Split("\n").ToList();

        // 初始化一个具有合适容量的 List
        var clearlyString = new List<string>();

        // 遍历分割后的字符串数组
        foreach (var str in strings)
        {
            // 去除每个字符串开头的空格和制表符
            var trimmedString = str.TrimStart();
            // 如果不为空，则添加到 clearlyString 中
            clearlyString.Add(trimmedString);
        }

        // 将处理后的字符串列表保存到 CodeLines 中
        codeLines = clearlyString;
    }

    public void LexerStrings()
    {
        realIndex = 0;
        while (realIndex < codeLines.Count)
        {
            NextToken();
        }
    }

    private void NextToken()
    {
        if (realIndex >= codeLines.Count)
        {
            return;
        }
        if (!string.IsNullOrEmpty(codeLines[realIndex]))
        {
            var tok = LexerString(codeLines[realIndex]);
            if(tok.TokenType is not TokenType.ScnComments) {
                tokens.Add(tok);
                RealLineCount.Add(tokens.Count - 1,realIndex+1);
                Console.WriteLine("Token序号:"+ (tokens.Count - 1).ToString()+" 真实行号:" +(realIndex+1).ToString());
            }
        }
        realIndex++;
        
    }
    public Token LexerString(string command)
    {
        if (string.IsNullOrEmpty(command))
        {
            throw new ArgumentNullException(nameof(command), $"{nameof(command)} cannot be null");
        }

        try
        {
           return ScriptLexer.LexerString(command);
        }
        catch (Exception ex)
        {
            throw new Exception($"ScnScript运行时出错!{Environment.NewLine}跟踪行号:{realIndex}{Environment.NewLine}下面是报错内容: {Environment.NewLine}{ex.Message}"); 
        }
        
    }

    //public void SetIndex(int number)
    //{
    //    this.index = number;
    //}

    //public int GetIndex()
    //{
    //    return index;
    //}
    public Token GetTokenByIndex(int number)
    {
        return tokens[number];
    }
    public List<Token> GetTokenList()
    {
        return tokens;
    }
    public string GetCodeString(int number)
    {
        return codeLines[number-1];
    }
}
#endif
#endregion
