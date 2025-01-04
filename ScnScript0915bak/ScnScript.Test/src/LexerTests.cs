using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScnScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTests;

//[TestClass]
//public class LexerHelperTests
//{
//    [TestMethod]
//    public void GetSymbolPositions()
//    {
//        var intTokens= ScnScript.LexerHelpers.GetSymbolPositions(";[a@b]\":",1);
//        // 这里跳过了一个所以是五
//        if (intTokens.Keys.Count != 5) Assert.Fail("错误数量的符号输出");
//        if(intTokens.Values.Any(x => x.Type == TokenType.SymbolComment))
//        {
//            Assert.Fail("跳过功能存在问题");
//        }

//    }
//}
[TestClass]
public class LexerTests
{
    [TestMethod]
    public void LexerTest()
    {
        var result=ScnScriptCommon.Lexer("[scn ver 4]");
        if (!result.AreaTokenTypes.Values.Contains(TokenType.SymbolCommandStart)) Assert.Fail("命令解析存在问题");
    }
    [TestMethod]
    public void LexerWithSkipTest()
    {
        var result = ScnScriptCommon.Lexer(";;;[scn ver 4]",3);
        if (!result.AreaTokenTypes.Values.Contains(TokenType.SymbolCommandStart)) Assert.Fail("命令解析存在问题");
    }
}