namespace ScnScript;
/* 此文件揭露了ScnScript的运行代码的原理 于 2024.4.4 创建 */
public partial class Script
{
    
    private List<Token> tokens = new();
    private int index;
    private bool waitingMode;

    private Dictionary<string, ScnScriptData> asmData = new();

    private readonly List<string> selectPaths = new();
    private string selectPath = string.Empty;
    private string selectNode = string.Empty;
    private string selectScene = string.Empty;
    private string selectObject = string.Empty;

    private void InitializeBaseData()
    {
        index = 0;
        waitingMode = false;
        asmData.Clear();
    }

    private void InitializeSelector()
    {
        selectPaths.Clear();
        selectPath = string.Empty;
        selectNode = string.Empty;
        selectScene = string.Empty;
        selectObject = string.Empty;
    }

    public void Startup()
    {
        while (!waitingMode)
        {
            RunNext();
        }
    }

    public void Continue()
    {
        waitingMode = false;
    }
    public void RunNext()
    {
        if (index < tokens.Count)
        {   
            Run(tokens[index]);
            index++;
        }
        else
        {
            waitingMode = true;
            Console.WriteLine("运行完成，已暂停运行");
        }
    }
    public void Run(Token token)
    {
        try
        {
            switch (token.TokenType)
            {
                case TokenType.ObjSelect:
                    //父对象是否包含属性
                    if (asmData.ContainsKey(GetSelectPath()))
                    {
                        //如果父元素不允许包含子项
                        if (asmData[GetSelectPath()].Data.Unsub)
                        {
                            SetUnSelectPath();
                        }
                    }
                    if(token.Key is "if" or "for" or "while")
                    {
                        SetSelectPath(token.Key);
                        SetSelectPath(token.Key+index);
                    }
                    else {
                        SetSelectPath(token.Key);
                    }

                    //请插入跟踪点 将消息输出到输出窗口 目前选择器的值为{GetSelectPath()} 或者使用下面的输出
                    Console.WriteLine($"目前选择器的值为{GetSelectPath()}");

                    // 不是第一次选中该项，不加入列表
                    if (!asmData.ContainsKey(GetSelectPath()))
                    {
                        var scnObjectData = new ScnScriptData();
                        
                        if (token.Tags.Count != 0)
                        {
                            scnObjectData.Data.Unsub = false;
                            foreach (var tag in token.Tags)
                            {
                                if (tag.Contains('='))
                                {
                                    try
                                    {
                                        var config = tag.Split('=');
                                        var key = config[0];
                                        var value = config[1];

                                        if(string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                                        {
                                            throw new Exception("属性键值必须都要填写！");
                                        }

                                        if(key == "getname")
                                        {
                                            scnObjectData.Data.ObjectType = "character";
                                            scnObjectData.Data.Unsub = true;
                                        }

                                        scnObjectData.Configs.Add(key,value);
                                    }
                                    catch(Exception ex)
                                    {
                                        throw new Exception($"该命令的 \"key=value\" 配置格式不标准，导致出现错误:{Environment.NewLine}{ex.Message}");
                                    }
                                }
                                else
                                {
                                    if (token.Key is "if" or "for" or "while")
                                    {
                                        var rule=string.Join(' ', token.Tags);
                                        scnObjectData.Configs.Add("rule", rule);
                                        break;
                                    }
                                    else {
                                        throw new Exception($"该命令的配置格式不标准，对象后方不应该直接出现非配置属性值:{Environment.NewLine}\t \"{tag}\" ");
                                    }
                                }
                                
                            }
                        }

                        this.asmData.Add(GetSelectPath(), scnObjectData);
                    }

                    //请插入跟踪点 将消息输出到输出窗口 目前选择器的值为{GetSelectPath()} 或者使用下面的输出
                    Console.WriteLine($"目前选择器的值为{GetSelectPath()}");

                    break;
                case TokenType.ObjClr:
                    if (!string.IsNullOrEmpty(token.Key))
                    {
                        SetUnSelectPath(token.Key);
                    }
                    else
                    {
                        SetUnSelectPath();
                    }

                    //请插入跟踪点 将消息输出到输出窗口 目前选择器的值为{GetSelectPath()} 或者使用下面的输出
                    Console.WriteLine($"目前选择器的值为{GetSelectPath()}");

                    break;
                case TokenType.ObjCommand:

                    break;
                case TokenType.ScnComments:
                    break;

                case TokenType.NodeSelect:
                    break;

                case TokenType.SceneSelect:
                    break;

                case TokenType.LocalConfig:
                    break;

                case TokenType.ScnSetting:
                    break;

                case TokenType.GlobalCommand:
                    if (token.Key is "w" or "wait")
                    {
                        waitingMode = true;
                    }

                    if(token.Key is "e")
                    {
                        throw new Exception("程序主动报错");
                    }
                    break;

                case TokenType.StringKey:
                    if (string.IsNullOrEmpty(selectObject))
                    {
                        Console.WriteLine(token.Value);
                    }
                    break;

                case TokenType.Value:
                    break;

                case TokenType.Unk:
                    break;
                case TokenType.GroupCommand:
                    foreach (var tok in  token.Tokens)
                    {
                        RunCommand(tok);
                    }
                    break;
                default:
#if !DEBUG
                    throw new Exception("识别到未知命令");
#endif
#if DEBUG
                    break;
#endif
            }
            
        }
        catch (Exception ex)
        {
            throw new Exception($"ScnScript运行时出错!{Environment.NewLine}发生错误时行号:{Lexer.RealLineCount[index]} {Environment.NewLine}该标记行内容:{Environment.NewLine}\t'{Lexer.GetCodeString(Lexer.RealLineCount[index])}'{Environment.NewLine}------------------{Environment.NewLine}报错原因:{Environment.NewLine}{ex.Message}");
        }
    }

    private void SetSelectPath(string path)
    {
        if (string.IsNullOrEmpty(path) || path.Contains('.'))
        {
            return;
        }
        selectPath = path;
        selectPaths.Add(path);
    }

    private void SetUnSelectPath()
    {
        //移除一层选择器
        if (selectPaths.Count !=0)
        {
            selectPaths.RemoveAt(selectPaths.Count - 1);
        }
    }

    private void SetUnSelectPath(string path)
    {
        // 判断是否为空
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("The path cannot be null or empty.", nameof(path));
        }

        // 判断是否选择过这个path，也就是selectPaths是否包含path这个值
        var lastIndexOf = selectPaths.LastIndexOf(path);

        // 如果条件都满足，则移除直到第一个为path的项
        if (lastIndexOf >= 0)
        {
            selectPaths.RemoveRange(lastIndexOf, selectPaths.Count - lastIndexOf);
        }
    }
    
    public void SetIndex(int number)
    {
        this.index = number;
    }

    public List<Token> GetTokenList()
    {
        return tokens;
    }

    private string GetSelectPath()
    {
        if (selectPaths.Count == 0)
        {
            return "<未选中任何选择器>";
        }
        //根据selectPaths拼接里面所有的字符串项 输出xxx.xxx.xxx.xxx.xxx这样的结果
        var result = string.Join(".", selectPaths);

        return result;
    }
    public int GetIndex()
    {
        return index;
    }
}

public class ScnScriptData
{
    public DataType DataType=DataType.Unk;
    public Dictionary<string, string> Configs= new();
    public ScnObjectData Data = new();
    public class ScnObjectData
    {
        public string ObjectType = string.Empty;
        // (Object用) 是否允许出现子对象
        public bool Unsub;
    };
}

public enum DataType
{
    Object,
    Node,
    Scene,
    Value,
    String,
    Unk
}