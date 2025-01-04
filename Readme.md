# ScnScript 项目说明
这存储库如同名称一样 ScnScriptOldVerSourceCodeOpen ，是ScnScript的旧版本源代码开放存储库。

ScnScript是一个简单的脚本语言，基于C#开发。

没有什么好说的，如果你感兴趣可以查看代码拿出你需要的部分。

⚠️ 注意: 这里的项目基本功能上但是很不完善的，有兴趣可以自己随便玩一下。

随便一说，ScnScript并不是停止开发了，只是因为内部逻辑有很大的改动，不同的地方过于巨大，可以说是新年新面貌。

目前作者正在筹划ScnScript v5和其语法。ScnScript 最后不一定是开源项目，请注意。

主要是开源许可对于版权保护并不大，不过目前存储库里的内容对作者来说并没有什么价值，所以开源了。

这里的代码都以 MIT License 开源，你可以随意使用，但是请记得保留许可。

-- 该存储库项目上传与定档时间 2025.1.4 --

## 目录介绍

2023到2024年初的scn ver 3

- ScnScript.Test 
- ScnScript.VisualEditor

2024年的scn ver 4各个版本

后面的数字是月+日,例如ScnScript0518是2024年5月18日

- ScnScript0518
- ScnScript0915
- ScnScript1124

## 项目历史
具体以前的历史在这里简单阐述一下：

正式开始开发时从v3版本开始(2023年期间，其实是大概11月末)，v3是以行作为解析单位的，所以有很多局限性。为未来重写埋下伏笔

在2024年从零开始写词法分析器和语法分析器

设计了v4版简单的语法(去除了局部命令与点命令之后),后来加入了更好的语法糖(`@-b`,`@b+2`,`[/bbb]`)等。

在v4开发中产生了运行器，运行单元的设想，于是Host，SandBox概念就出来了

在2024八月末(8.29)完成了对象创建以及对应事件处理(包括自定义对象类型，结构体文档类型，返回，查询当前环境内的局部变量)，在前一天(8.28)完成Lexer解析速度的巨大提升(重写为状态机方法)还有字符串文本跨行功能(请注意属性不能这样写)。

在2024.9.7决定重写底层，目前版本号从v4.0perview变成v4.0perview-alpha-rebuild4-early。

让我来解释一下发生什么了。词法分析器逻辑重新搞(从识别区域变成分词)。

语法分析器的将未识别的文字转化为字符串命令属性操作有性能问题，于是需要把这个功能该到Lexer上提升效率。

所以需要对命令解析大改。其次，对象选择器和创建函数等内部函数方法与场景和节点位置标记再加上工作数据与全局数据的定位与逻辑不太清晰。

host在调用函数上面的逻辑也并没有完全写好，ScnAction结构的Head与Value关系“暧昧不清”，类名与函数名的识别太难识别了，所以需要变动。

然后我们的Call函数虽然可以调用场景什么的了，但是if等之类的流程函数以及系统关键字close，new，parent什么的目前由方法自行处理，

我们需要在运行前了解对象结构，函数信息，可用属性以及语法高亮，所以TokenType上面的肯定要改。

最后还需要代码生成，可视化监视，逻辑肯定得改。以及还有更方便的扩展想法。

到 2024.11.24 时行为树的编写方法实在是太过于麻烦了，所以最后放弃了。

于是在 2025.1.4 日正式决定 重新开始以行为解析单位，方便维护。

反正兜兜转转还是回到开始了，所以打算把旧版ScnScript的全部代码以MIT许可证公开得了。