using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScnScript;
namespace ParserTests;
[TestClass]
public class ParserTests
{
    [TestMethod]
    public void ParserTest()
    {
        var content = "[scn ver 4]";
        var lexerResult=ScnScriptCommon.Lexer(content);
        var parserResult=ScnScriptCommon.Parser(content,lexerResult);
        if(!parserResult.Actions.Values.Any(x => x.Type == ActionType.Command))
        {
            Assert.Fail("未能解释命令");
        }
        if (!parserResult.Actions.Values.Any(x => x.Head == "scn"))
        {
            Assert.Fail("未能解释命令头");
        }
        if (!parserResult.Actions.Values.Any(x => x.Configs!.ContainsKey("Value")))
        {
            Assert.Fail("未能解释属性");
        }
    }
    [TestMethod]
    public void ParserValidNameTest()
    {
        var content = "@%^&*@";
        var lexerResult = ScnScriptCommon.Lexer(content);
        try
        {
            var parserResult = ScnScriptCommon.Parser(content, lexerResult);
        }
        catch
        {
            return;
        }
        
        Assert.Fail("未能在非法字符报错");
    }

}
[TestClass]
public class ParserFeatureTests
{
    [TestMethod]
    public void LetIgnoreUnknownAsTextFeatureTest()
    {

        var content = " hello ";
        var lexerResult = ScnScriptCommon.Lexer(content);
        var parserResult = ScnScriptCommon.Parser(content, lexerResult);
        if (parserResult.Actions.Count == 0)
        {
            Assert.Fail("返回为空，未能解释字符串");
        }
        if (!parserResult.Actions.Values.Any(x => x.Value == "hello"))
        {
            Assert.Fail("未能解释字符串");
        }
    }
    [TestMethod]
    public void LetCommandStartWithSlashAsCloseTagFeatureTest()
    {
        //DefaultLetCommandStartWithSlashAsCloseTagFeature
        var content = "[/myTest]";
        var lexerResult = ScnScriptCommon.Lexer(content);
        var parserResult = ScnScriptCommon.Parser(content, lexerResult);
        if (parserResult.Actions.Count == 0)
        {
            Assert.Fail("返回为空，未能解释命令");
        }
        if (!parserResult.Actions.Values.Any(x => x.Value == "close"))
        {
            Assert.Fail("特性失效");
        }
    }
    [TestMethod]
    public void LetObjectStartWithMinusAsClrFeatureTest()
    {
        var content = "@-a";
        var lexerResult = ScnScriptCommon.Lexer(content);
        var parserResult = ScnScriptCommon.Parser(content, lexerResult);
        if (parserResult.Actions.Count == 0)
        {
            Assert.Fail("返回为空，未能解释命令");
        }
        if (!parserResult.Actions.Values.Any(x => x.Value is "a" && x.Head is "clr"))
        {
            Assert.Fail("特性失效");
        }
    }
    [TestMethod]
    public void LetObjectPlusMinusFeatureTest()
    {
        var content = "@a-2";
        var lexerResult = ScnScriptCommon.Lexer(content);
        var parserResult = ScnScriptCommon.Parser(content, lexerResult);
        if (parserResult.Actions.Count == 0)
        {
            Assert.Fail("返回为空，未能解释命令");
        }
        if (!parserResult.Actions.Values.Any(x => x.Value is "a" && x.Head is "Selector" && x.Configs!["Mode"] == "Minus" && x.Configs["Frequency"] == "2"))
        {
            Assert.Fail("特性失效");
        }
    }
}
