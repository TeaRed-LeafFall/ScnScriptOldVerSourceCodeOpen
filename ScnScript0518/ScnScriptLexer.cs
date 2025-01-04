namespace ScnScript;
/* 此文件揭露了ScnScript的词法解析器代码 于 2024.4.4 创建 */
/* 注意 ： 这并不是第一版代码，第一版代码基于字符串便携指令解析，会有很多限制，所以使用逐个字符判断的方法 */
public partial class Script
{
    private List<string> sourceCodeLine=new();
    // 记录每个Token的行和列,索引为Token的ID
    public Dictionary<int, Position> RealPosition = new();
    //private int LexerIndex;
    private int LexerLine;
    private int LexerColumn;
    public static List<string> ClearlyString(string sourceCode){
        // 使用 Environment.NewLine 分割字符串
        var strings = sourceCode.Split("\n").ToList();

        // 初始化一个具有合适容量的 List
        var result = new List<string>();

        // 遍历分割后的字符串数组
        foreach (var str in strings)
        {
            // 去除每个字符串开头的空格和制表符
            var trimmedString = str.TrimStart();
            // 如果不为空，则添加到 clearlyString 中
            result.Add(trimmedString);
        }
        return result;
    }
    public void LexerStrings(List<string> strings){
        // 清空
        RealPosition.Clear();
        sourceCodeLine.Clear();
        sourceCodeLine=strings;
        tokens.Clear();
        LexerLine=0;
        LexerColumn=0;

        // 按照行进行遍历 
        for(;LexerLine<sourceCodeLine.Count;LexerLine++){
            var str=sourceCodeLine[LexerLine];
            // 空的跳过
            if(string.IsNullOrEmpty(str)) continue;
            // 获取Token
            try{
                var LineTokens=GetToken(str);
                foreach(var token in LineTokens){
                    // 跳过注释
                    if(token.TokenType is not TokenType.ScnComments){
                        // 添加
                        tokens.Add(token);
                        // 记录位置
                        RealPosition.Add(tokens.Count,new Position(LexerLine,LexerColumn));
                    }
                    
                }
                
            }catch(Exception e){
                throw new Exception($"ScnScript解析时出错!{Environment.NewLine}跟踪行号:{i+1}{Environment.NewLine}下面是报错内容: {Environment.NewLine}{e.Message}");
            }
            
            
        }

    }
    public List<Token> GetToken(string command){
        
        LexerColumn=0;
        if(command.StartsWith("//")){
            
            return new List<Token>{
                new Token{
                    TokenType=TokenType.ScnComments,
                    Value=command.Substring(2),
                }
            };
        }
    }
}

public struct Position
{
    public int Line;
    public int Column;
    public Position(int line, int column){
        Line=line;
        Column=column;
    }
}