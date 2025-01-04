
using System.Diagnostics;

namespace ScnScript;

#region  此代码已经弃用，将在未来版本移除
#if NotSupported
public static class ScriptLexer
{
    private static readonly List<string> ObjcKeyWords =
    [
        "goto",
        "call"
    ];

    public enum LexerMode
    {
        Default,
        InGroup
    }
    public static Token LexerString(string command,LexerMode mode=LexerMode.Default)
    {
        if (string.IsNullOrEmpty(command))
        {
            throw new ArgumentNullException(nameof(command), $"{nameof(command)} cannot be null");
        }

        try
        {
            var stringHead = command
                .Substring(1)
                .TrimStart();
            var splitStrings = new List<string>();
            var keyValues = new List<string>();

            var keyName = string.Empty;
            if (!string.IsNullOrEmpty(stringHead))
            {
                splitStrings = stringHead.Split(" ")
                    .Select(str => str.TrimStart())
                    .Where(str => !string.IsNullOrEmpty(str))
                    .ToList();
            }
            if (splitStrings.Count > 0)
            {
                keyName = splitStrings[0];
            }
            if (splitStrings.Count > 0)
            {
                keyValues = splitStrings
                    .Skip(1)
                    .ToList();
            }

            var token = new Token();

            switch (command[0])
            {

                case ';':
                    token.TokenType = TokenType.ScnComments;
                    token.Value = stringHead;
                    break;

                case '@':
                    if (stringHead.StartsWith("clr", StringComparison.OrdinalIgnoreCase))
                    {
                        token.TokenType = TokenType.ObjClr;
                        if (keyValues.Count >= 1)
                        {
                            token.Key = keyValues[0];
                        }
                    }
                    else
                    {

                        foreach (var keyword in ObjcKeyWords)
                        {
                            if (stringHead.StartsWith(keyword, StringComparison.OrdinalIgnoreCase))
                            {
                                token.TokenType = TokenType.ObjCommand;
                                token.Key = keyword;
                                token.Tags = keyValues.ToList();
                                break;
                            }
                        }

                        if (token.TokenType != TokenType.ObjCommand)
                        {
                            token.TokenType = TokenType.ObjSelect;
                            token.Key = keyName;
                            token.Tags = keyValues.ToList();
                        }
                    }
                    break;

                case '#':
                    token.TokenType = TokenType.LocalCommand;
                    token.Key = keyName;
                    token.Tags = keyValues.ToList();
                    break;

                case '*':
                    token.TokenType = TokenType.NodeSelect;
                    token.Key = keyName;
                    //判断是否为场景节点
                    if (keyValues is ["|", _, ..] && (keyValues[1].Equals("Start") || keyValues[1].Equals("End")))
                    {
                        token.TokenType = TokenType.SceneSelect;
                        token.Tags.Add(keyValues[1]);
                    }
                    else
                    {
                        if (keyValues.ToList().Count > 1)
                        {
                            throw new Exception("出现与预期不同的参数，Node不应该具有除Start/End之外的标签");
                        }
                    }

                    break;

                case '[':
                    var hasCloseBracket = false;

                    token.TokenType = TokenType.GlobalCommand;
                    token.Key = keyName;
                    if (keyValues.Count != 0)
                    {
                        token.Value = keyValues[0];
                    }
                    //如果属于[a]类型
                    if (keyValues.Count == 0)
                    {
                        if (keyName.Contains("]"))
                        {
                            hasCloseBracket = true;
                            var closedStringIndex = keyName.IndexOf("]", StringComparison.Ordinal);
                            token.Key = keyName.Remove(closedStringIndex, 1);
                        }
                    }



                    //如果非[a]类型
                    for (var i = 0; i < keyValues.Count; i++)
                    {
                        if (keyValues[i].Contains("]"))
                        {
                            var closedStringIndex = keyValues[i].IndexOf("]", StringComparison.Ordinal);
                            var strRemoved = keyValues[i].Remove(closedStringIndex, 1);
                            hasCloseBracket = true;
                            if (string.IsNullOrEmpty(strRemoved))
                            {
                                keyValues.Remove(strRemoved);
                            }
                            else
                            {
                                keyValues[i] = strRemoved.Substring(0, closedStringIndex);
                                var linkString = string.Join(" ", keyValues.Skip(i + 1));
                                if (!string.IsNullOrEmpty(linkString))
                                {
                                    //将当前token设置为GroupCommand
                                    var thisTok = token;
                                    token.TokenType = TokenType.GroupCommand;
                                    token.Tokens.Add(thisTok);
                                    //右括号后面的内容还原字符串并重新加入识别
                                    var subToken = LexerString(linkString,LexerMode.InGroup);
                                    if (subToken.TokenType is TokenType.StringKey or TokenType.GlobalCommand)
                                    {
                                        token.Tokens.Add(subToken);
                                    }
                                    if (subToken.TokenType is TokenType.GroupCommand)
                                    {
                                        
                                    }
                                }
                                //请在keyValues删除后面的内容包含本keyValues[i]后面的内容
                                keyValues.RemoveRange(i + 1, keyValues.Count - i - 1);
                                break;
                            }

                        }
                    }
                    if (!hasCloseBracket)
                    {
                        throw new KeyNotFoundException("无法找到']'结尾符号");
                    }

                    token.Tags = keyValues.ToList();
                    break;
                // 请实现GroupCommand识别功能
                case '\\':
                    token.TokenType = TokenType.StringKey;
                    token.Value = stringHead;
                    break;
                case '.':
                    token.TokenType = TokenType.LocalConfig;
                    token.Key = keyName;
                    token.Tags = keyValues.ToList();
                    break;
                default:
                    token.TokenType = TokenType.StringKey;
                    //if (command.IndexOf('[') != 0)
                    //{
                    //    command.
                    //}
                    token.Value = command;
                    break;
            }
            Console.WriteLine("类型: "+token.TokenType.ToString()+" 键: "+token.Key + " 值: " +token.Value+" 标签: "+string.Join(",", token.Tags));
            return token;
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"ScnScript解析时出错! {Environment.NewLine}该标记内容:{Environment.NewLine}\t'{command}'{Environment.NewLine}------------------{Environment.NewLine}报错原因:{Environment.NewLine}{ex.Message}");
        }
        
    }


}
#endif
#endregion