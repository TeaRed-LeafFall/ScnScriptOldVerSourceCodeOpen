namespace ScnScript;

public enum TokenType
{
    //节点
    NodeSelect,
    //场景
    SceneSelect,
    //选中对象
    ObjSelect,
    //对象命令
    ObjCommand,
    //取消选中对象
    ObjClr,
    //本地配置
    LocalConfig,
    //解析配置
    ScnSetting,
    //注释
    ScnComments,
    //全局命令
    GlobalCommand,
    //字符串值
    StringKey,
    //变量与数据
    Value,
    //未知
    Unk
}


public class Token
{
    public TokenType TokenType = TokenType.Unk;
    //命令主键值
    public string Key = string.Empty;
    //命令值
    public string Value = string.Empty;
    //命令配置
    public Dictionary<string, string> Config = new();
    //命令标签
    public List<string> Tags = new();
    
}