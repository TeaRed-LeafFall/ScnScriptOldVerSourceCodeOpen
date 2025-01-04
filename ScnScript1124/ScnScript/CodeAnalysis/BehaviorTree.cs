using ScnScript.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace ScnScript.CodeAnalysis;

public class BehaviorTree
{
    [Browsable(true)]
    public List<BehaviorTreeLeaf> Tree { get; set; } = new();
    private ProblemProvider problemProvider { get; set; } = new();
    private string[] objCommandSystemKeywords = ["func", "method", "event", "call", "jump", "return", "switch", "clr"];
    private string[] registerObjCommandKeywords = [];
    private static string[] flowControlCommandKeywords = ["if", "else", "while", "for", "do"];
    /// <summary>
    /// 构建行为树
    /// </summary>
    /// <param name="tokens">词法分析器返回</param>
    /// <exception cref="Exception">语法错误</exception>
    public void Build(Dictionary<Position, ScriptToken> tokens)
    {
        Tree.Clear();
        problemProvider.Clear();
        var status = BuildStatusKind.None;
        var countKeyword = -1;
        Stack<BehaviorTreeLeaf> stack = new();
        BehaviorTreeLeaf? leaf = null;

        var inConfig = false;
        var configName = string.Empty;

        foreach (var token in tokens)
        {
            ////Debug.WriteLine($"{token.Key} {token.Value.Kind} {token.Value.Text}");
            problemProvider.ResetPosition(token.Key);
            switch (token.Value.Kind)
            {
                case TokenKind.DoubleQuotationMarks:
                    if (token.Value.Text is null)
                    {
                        problemProvider.Report(ProblemLevel.Warning, "双引号内不能为空");
                        continue;
                    }

                    //Debug.WriteLine($"string: \"{token.Value.Text}\"");


                    switch (status)
                    {
                        case BuildStatusKind.InNodeOrScene:
                            if (leaf is null) throw new Exception("leaf is null");
                            if (leaf is not SceneLeaf sceneLeaf) throw new Exception("leaf is not SceneLeaf");
                            if (string.IsNullOrEmpty(sceneLeaf.DisplayName))
                            {
                                sceneLeaf.DisplayName = token.Value.Text;
                            }
                            else
                            {
                                throw new Exception("不应存在重复的DisplayName");
                            }
                            break;
                        case BuildStatusKind.InObjectControllerCommand:
                            if (inConfig)
                            {
                                if (leaf is ObjectControllerCommandLeaf objCommandLeaf)
                                {
                                    if (configName is "name")
                                    {
                                        objCommandLeaf.MethodName = token.Value.Text;
                                        CompleteConfig();
                                        break;
                                    }
                                    objCommandLeaf.Parameters ??= [];
                                    objCommandLeaf.Parameters.Add(configName, new ExpressParametersValueString(token.Value.Text));
                                    CompleteConfig();
                                }
                            }
                            else
                            {
                                problemProvider.ReportError("字符串不能作为有效语句，如果是配置，请在前书写'配置项名称='语句");
                            }
                            break;
                        case BuildStatusKind.InCommand:
                            if (inConfig)
                            {
                                if (leaf is CommandLeaf commandLeaf)
                                {
                                    commandLeaf.Parameters ??= [];
                                    commandLeaf.Parameters.Add(configName, new ExpressParametersValueString(token.Value.Text));
                                    CompleteConfig();
                                }
                            }
                            else
                            {
                                problemProvider.Report(ProblemLevel.Warning, "不应该在命令中使用字符串作为参数，应该使用单引号表达，否则应该使用属性的方式传递");
                            }
                            break;
                        case BuildStatusKind.None:
                            if (stack.Count > 0)
                            {
                                BehaviorTreeLeaf node = stack.Peek() ?? throw new Exception("node is null");
                                node.Children ??= [];
                                node.Children.Add(new StringLeaf { Value = token.Value.Text });
                            }
                            else
                            {
                                Tree.Add(new StringLeaf { Value = token.Value.Text });
                            }
                            break;
                    }

                    break;
                case TokenKind.SingleQuotationMarks:
                    if (string.IsNullOrEmpty(token.Value.Text))
                    {
                        problemProvider.Report(ProblemLevel.Warning, "单引号内内容不能为空");
                        continue;
                    }
                    // 服务在命令中
                    if (status is BuildStatusKind.InObjectControllerCommand)
                    {
                        if (leaf is ObjectControllerCommandLeaf objCommandLeaf)
                        {
                            if (!inConfig)
                            {
                                if (string.IsNullOrEmpty(objCommandLeaf.Value))
                                {
                                    objCommandLeaf.Value = token.Value.Text;
                                }
                                else
                                {
                                    problemProvider.ReportError("对象选择器命令不能重复设值");
                                    continue;
                                }
                            }
                            else
                            {
                                objCommandLeaf.Parameters ??= [];
                                objCommandLeaf.Parameters.Add(configName, new ExpressParametersValueExpress(token.Value.Text));
                                CompleteConfig();
                            }

                        }
                        continue;
                    }
                    // 服务于命令中
                    if (status is BuildStatusKind.InCommand)
                    {
                        if (leaf is CommandLeaf commandLeaf)
                        {
                            if (!inConfig)
                            {
                                // 参数
                                commandLeaf.Args ??= [];
                                commandLeaf.Args.Add(token.Value.Text);
                            }
                            else
                            {
                                // 属性配置
                                commandLeaf.Parameters ??= [];
                                commandLeaf.Parameters.Add(configName, new ExpressParametersValueExpress(token.Value.Text));
                                CompleteConfig();
                            }

                        }
                        continue;
                    }


                    if (status is BuildStatusKind.None)
                    {
                        problemProvider.Report(ProblemLevel.Suggest, "不应该将表达式作为语句输入！");
                        continue;
                    }
                    break;
                case TokenKind.SemiColon:
                    ////Debug.WriteLine("注释已忽略！");
                    break;
                case TokenKind.Colon:
                    TryEndObjectControllerCommand();
                    if (status is BuildStatusKind.InCommand)
                    {
                        if (leaf is CommandLeaf commandLeaf)
                        {
                            if (countKeyword is 1)
                            {
                                // 即[aa:bb]
                                if (string.IsNullOrEmpty(commandLeaf.ClassName))
                                {
                                    commandLeaf.ClassName = commandLeaf.MethodName;
                                    commandLeaf.MethodName = string.Empty;
                                }
                            }
                        }
                    }
                    break;
                case TokenKind.Whitespace:
                    break;
                case TokenKind.At:
                    TryStartObjectControllerCommand();
                    break;
                case TokenKind.Equal:
                    // 对象命令配置处理
                    if (status is BuildStatusKind.InObjectControllerCommand)
                    {
                        if (!string.IsNullOrEmpty(configName))
                        {
                            inConfig = true;
                        }
                        else
                        {
                            problemProvider.ReportError("=不应该在这里");
                        }
                        continue;
                    }
                    // 命令的配置处理
                    if (status is BuildStatusKind.InCommand)
                    {
                        if (!string.IsNullOrEmpty(configName))
                        {
                            inConfig = true;
                        }
                        else
                        {
                            if (leaf is CommandLeaf commandLeaf)
                            {
                                // 属于[xxx=...]的情况，将值作为Value的值
                                if (!string.IsNullOrEmpty(commandLeaf.MethodName))
                                {
                                    //这里不需要重复判断configName是否为空，因为上面已经判断过了
                                    if (countKeyword is 1)
                                    {
                                        inConfig = true;
                                        configName = "Value";
                                    }
                                }
                            }
                            else
                            {
                                problemProvider.ReportError("异常情况！=不应该在这里");
                            }
                            problemProvider.ReportError("=不应该在这里");
                        }
                        continue;
                    }
                    break;
                case TokenKind.OpenParenthesis:
                    break;
                case TokenKind.CloseParenthesis:
                    break;
                case TokenKind.OpenBrace:
                    break;
                case TokenKind.CloseBrace:
                    break;
                case TokenKind.OpenBracket:
                    // 这里是命令的开始
                    if (status is not BuildStatusKind.None) continue;
                    status = BuildStatusKind.InCommand;
                    leaf = new CommandLeaf();
                    break;
                case TokenKind.CloseBracket:
                    // 这里是命令的结束
                    if (status is BuildStatusKind.InCommand)
                    {
                        status = BuildStatusKind.None;
                        //妈的离谱了，如果把if反转达成continue会让别的commandLeaf报错本地变量“commandLeaf”在声明之前无法使用
                        if (leaf is CommandLeaf commandLeaf)
                        {
                            // 处理className
                            // 即[a b]形式
                            if (string.IsNullOrEmpty(commandLeaf.ClassName))
                            {
                                if (!string.IsNullOrEmpty(configName))
                                {
                                    if (countKeyword is 2)
                                    {
                                        commandLeaf.ClassName = configName;
                                    }
                                }
                            }
                            // 加入到父节点
                            if (stack.Count > 0)
                            {
                                //获取父节点
                                var parent = stack.Peek();
                                parent.Children ??= [];
                                parent.Children.Add(leaf);
                            }
                            else
                            {
                                Tree.Add(leaf);
                            }
                            leaf = null;
                        }
                    }
                    break;
                case TokenKind.Add:
                    break;
                case TokenKind.Minus:
                    break;
                case TokenKind.Multiply:
                    // 这里是节点开始
                    if (status is BuildStatusKind.None)
                    {
                        status = BuildStatusKind.InNodeOrScene;
                        countKeyword = 0;
                    }
                    break;
                case TokenKind.Divide:
                    // 在命令名称开始前具有/可以看作closeTag
                    if (status is BuildStatusKind.InCommand)
                    {
                        if (leaf is CommandLeaf commandLeaf)
                        {
                            if (string.IsNullOrEmpty(commandLeaf.MethodName))
                            {
                                if (!commandLeaf.HasCloseTag)
                                {
                                    commandLeaf.HasCloseTag = true;
                                }
                                else
                                {
                                    problemProvider.Report(ProblemLevel.Warning, "重复的closeTag");
                                }
                            }
                            else
                            {
                                problemProvider.Report(ProblemLevel.Warning, "closeTag不应该出现在方法名称之后，正确语法应该类似于[/xxx]");
                            }
                        }
                    }
                    break;
                case TokenKind.And:
                    break;
                case TokenKind.Or:
                    if (status is BuildStatusKind.InNodeOrScene)
                    {
                        if (countKeyword != 1)
                        {
                            problemProvider.ReportError("The or symbol is not in the right place");
                            continue;
                        }
                        if (leaf is not null)
                        {
                            leaf = SceneLeaf.Copy((NodeLeaf)leaf);
                        }
                        continue;
                    }
                    if (status is BuildStatusKind.InCommand)
                    {
                        problemProvider.Report(ProblemLevel.Warning, "命令中使用|分隔仅在SCNVER3以前支持，目前没有任何作用，可以安全移除");
                    }
                    break;
                case TokenKind.Not:
                    break;
                case TokenKind.Xor:
                    if (status is BuildStatusKind.InObjectControllerCommand)
                    {
                        if (leaf is ObjectControllerCommandLeaf objCommandLeaf)
                        {
                            objCommandLeaf.ScnKeywords ??= [];
                            if (objCommandLeaf.ScnKeywords.Contains("close"))
                            {
                                problemProvider.ReportError("不应该重复使用关闭语法！");
                                continue;
                            }
                            else
                            {
                                objCommandLeaf.ScnKeywords.Add("close");
                            }
                        }
                    }
                    break;
                case TokenKind.LessThan:
                    break;
                case TokenKind.GreaterThan:
                    break;
                case TokenKind.QuestionMark:
                    break;
                case TokenKind.Escape:
                    break;
                case TokenKind.Lf:

                    if (status is BuildStatusKind.InNodeOrScene)
                    {
                        //Debug.WriteLine("Node or Scene LF");
                        status = BuildStatusKind.None;
                        countKeyword = -1;

                        BehaviorTreeLeaf? parent = null;
                        // 如果父节点是节点就推出
                        if (stack.Count > 0)
                        {
                            parent = stack.Peek();
                            if (parent is NodeLeaf parentNode)
                            {
                                stack.Pop();
                                //Debug.WriteLine("Pop Node: " + parentNode.Name);
                            }

                        }

                        if (leaf is NodeLeaf node)
                        {
                            if (stack.Count > 0)
                            {
                                parent = stack.Peek();
                                // 如果父节点是场景就加入
                                if (parent is SceneLeaf parentScene)
                                {
                                    if (parentScene.Name == node.Name)
                                    {
                                        problemProvider.ReportError($" {node.Name} 场景已存在");
                                        continue;
                                    }
                                    parentScene.Children ??= [];
                                    parentScene.Children.Add(node);
                                }
                                else
                                {
                                    problemProvider.ReportError($" {node.Name} 不应该出现在此");
                                    continue;
                                }
                            }
                            else
                            {
                                Tree.Add(leaf);
                            }
                            stack.Push(node);
                        }
                        else if (leaf is SceneLeaf scene)
                        {
                            if (stack.Count > 0)
                            {
                                parent = stack.Peek();
                                // 如果父节点是场景就加入
                                if (parent is SceneLeaf parentScene)
                                {
                                    if (parentScene.Name == scene.Name)
                                    {
                                        if (!scene.IsEnd)
                                        {
                                            problemProvider.ReportError($"{parentScene.Name} 场景未结束");
                                            continue;
                                        }
                                        parentScene.IsComplete = true;
                                        stack.Pop();
                                        leaf = null;
                                        break;
                                    }
                                    if (scene.IsEnd)
                                    {
                                        problemProvider.ReportError($"目前没有找到 {scene.Name} 场景，不能执行关闭场景");
                                        continue;
                                    }
                                    parentScene.Children ??= [];
                                    parentScene.Children.Add(scene);
                                }
                            }
                            else
                            {
                                Tree.Add(leaf);
                            }
                            stack.Push(scene);
                        }
                        leaf = null;
                    }

                    TryEndObjectControllerCommand();

                    if (inConfig || !string.IsNullOrEmpty(configName))
                    {

                        problemProvider.ReportError($"配置语句未结束, 目前值{configName}");
                    }

                    break;
                case TokenKind.Keyword:
                    if (token.Value.Text is null) break;
                    //Debug.WriteLine($"Keyword: {token.Value.Text}");
                    if (status is BuildStatusKind.InNodeOrScene)
                    {
                        countKeyword++;
                        //Debug.WriteLine($"countKeyword: {countKeyword}");

                        if (countKeyword == 1)
                        {
                            leaf = new NodeLeaf { Name = token.Value.Text };
                        }
                        else if (countKeyword == 2)
                        {
                            if (leaf is null) break;
                            if (token.Value.Text is not "Start" and not "End")
                            {
                                problemProvider.ReportError($"未能解释字符 SceneLeaf: {token.Value.Text}");
                                continue;
                            }
                            if (token.Value.Text is "End")
                            {
                                if (leaf is SceneLeaf scene)
                                {
                                    scene.IsEnd = true;
                                }
                            }
                        }
                        else
                        {
                            problemProvider.ReportError($"SceneLeaf 语法错误！");
                            continue;
                        }
                        continue;
                    }
                    if (status is BuildStatusKind.InObjectControllerCommand)
                    {
                        if (leaf is ObjectControllerCommandLeaf objCommandLeaf)
                        {
                            if (string.IsNullOrEmpty(objCommandLeaf.MethodName))
                            {
                                if (flowControlCommandKeywords.Contains(token.Value.Text))
                                {
                                    leaf = new FlowControlCommandLeaf();
                                }
                                else
                                {
                                    // 写这一部分时真的被搞崩了，逻辑弄不清楚
                                    objCommandLeaf.MethodName = token.Value.Text;
                                }
                            }
                            else
                            {
                                // 命令类型的objCommand除函数名称外的第二项作为函数值注入
                                if (objCommandSystemKeywords.Contains(objCommandLeaf.MethodName))
                                {
                                    if (string.IsNullOrEmpty(objCommandLeaf.Value))
                                    {
                                        objCommandLeaf.Value = token.Value.Text;
                                        continue;
                                    }
                                }

                                // objCommandLeaf.ScnKeywords ??= [];
                                // objCommandLeaf.ScnKeywords.Add(token.Value.Text);
                                if (!inConfig)
                                {
                                    if (string.IsNullOrEmpty(configName))
                                    {
                                        configName = token.Value.Text;
                                    }
                                    else
                                    {
                                        //Debug.WriteLine($"现在configName是{configName}，token是{token.Value.Text}");
                                        objCommandLeaf.ScnKeywords ??= [];
                                        objCommandLeaf.ScnKeywords.Add(token.Value.Text);
                                    }

                                }
                                else
                                {
                                    // 如果对象存在name属性,第一项100%为对象类型
                                    // 如果存在dtype属性，第一项100%为对象名称
                                    if (configName is "dtype")
                                    {
                                        objCommandLeaf.Parameters ??= [];
                                        objCommandLeaf.Parameters.Add("dtype", new ExpressParametersValueExpress(token.Value.Text));
                                        continue;
                                    }
                                    else
                                    {
                                        if (configName is "name")
                                        {
                                            objCommandLeaf.Parameters ??= [];
                                            if (objCommandLeaf.Parameters.ContainsKey("dtype"))
                                            {
                                                problemProvider.ReportError("不应该存在dtype定义与name共存的情况");
                                                continue;
                                            }
                                            objCommandLeaf.Parameters.Add("dtype", new ExpressParametersValueExpress(objCommandLeaf.MethodName));
                                            objCommandLeaf.MethodName = token.Value.Text;
                                            CompleteConfig();
                                            continue;
                                        }
                                    }

                                    // 布尔值处理
                                    if (token.Value.Text is "true")
                                    {
                                        objCommandLeaf.Parameters ??= [];
                                        objCommandLeaf.Parameters.Add(configName, new ExpressParametersValueBool(true));
                                        CompleteConfig();
                                        continue;
                                    }
                                    else if (token.Value.Text is "false")
                                    {
                                        objCommandLeaf.Parameters ??= [];
                                        objCommandLeaf.Parameters.Add(configName, new ExpressParametersValueBool(false));
                                        CompleteConfig();
                                        continue;
                                    }

                                }
                            }
                        }
                        continue;
                    }
                    if (status is BuildStatusKind.InCommand)
                    {
                        if (leaf is CommandLeaf commandLeaf)
                        {
                            if (string.IsNullOrEmpty(commandLeaf.MethodName))
                            {
                                commandLeaf.MethodName = token.Value.Text;
                                countKeyword = 1;
                            }
                            else
                            {
                                countKeyword++;
                                // 到达第三个关键字时可以确认是否是方法名称了
                                // 即[Class Method xxx]
                                if (string.IsNullOrEmpty(commandLeaf.ClassName))
                                {
                                    if (countKeyword is 3)
                                    {
                                        if (!inConfig)
                                        {
                                            commandLeaf.ClassName = commandLeaf.MethodName;
                                            commandLeaf.MethodName = configName;
                                            configName = string.Empty;
                                            //Debug.WriteLine($"{commandLeaf.ClassName} {commandLeaf.MethodName} {token.Value.Text}");
                                        }
                                    }
                                }
                                //可以作为int的，默认作为Value的值
                                if (int.TryParse(token.Value.Text, out int _))
                                {
                                    commandLeaf.Parameters ??= [];
                                    commandLeaf.Parameters.Add("Value", new ExpressParametersValueString(token.Value.Text));
                                    continue;
                                }
                                configName = token.Value.Text;
                            }
                        }
                        continue;
                    }
                    // 如果不属于任何语句，则作为普通文本处理
                    if (status is BuildStatusKind.None)
                    {
                        if (stack.Count > 0)
                        {
                            BehaviorTreeLeaf node = stack.Peek() ?? throw new Exception("node is null");
                            node.Children ??= [];
                            node.Children.Add(new StringLeaf { Value = token.Value.Text });
                        }
                        else
                        {
                            Tree.Add(new StringLeaf { Value = token.Value.Text });
                        }
                        continue;
                    }
                    break;
                // 忽略(这些类型是为了兼容CRLF和LF模式的，以及Tab不需要管理)
                case TokenKind.Cr:
                case TokenKind.Tab:
                    break;

                case TokenKind.Unknown:
                    break;

                default:
                    throw new Exception("存在错误，{token.Value.Kind} 不是有效的可处理类型");
            }
        }

        if (status is not BuildStatusKind.None)
        {
            problemProvider.ReportError($"存在语法错误！状态 ‘{status}’未闭合");
        }

        // 下面两个匿名函数是由于:与\n同样可以结束对象选择器所以就为了减少耦合而写的

        void TryStartObjectControllerCommand()
        {
            if (status is not BuildStatusKind.None) return;
            status = BuildStatusKind.InObjectControllerCommand;
            leaf = new ObjectControllerCommandLeaf();
        }

        void TryEndObjectControllerCommand()
        {
            if (status is not BuildStatusKind.InObjectControllerCommand) return;
            status = BuildStatusKind.None;
            countKeyword = -1;

            if (leaf is not ObjectControllerCommandLeaf objCommandLeaf) return;

            if (!string.IsNullOrEmpty(configName))
            {
                objCommandLeaf.ScnKeywords ??= [];
                objCommandLeaf.ScnKeywords.Add(configName);
                // tmd，这里忘记清空了
                CompleteConfig();
            }

            // 处理取消选择
            if (objCommandLeaf.MethodName is "clr")
            {
                //Debug.WriteLine("取消选择Clr函数处理！");

                if (stack.Count > 0)
                {
                    var parent = stack.Peek();
                    if (parent is ObjectControllerCommandLeaf parentCommandLeaf)
                    {
                        // 如果没有目标名称，直接推出
                        if (string.IsNullOrEmpty(objCommandLeaf.Value))
                        {
                            stack.Pop();
                        }
                        else
                        {
                            //Debug.WriteLine($"清除选择：{objCommandLeaf.Value}");
                            //Debug.WriteLine($"父节点：{parentCommandLeaf.MethodName}");
                            // 如果方法名与清除选择的名称相同，则清除选择
                            if (parentCommandLeaf.MethodName == objCommandLeaf.Value)
                            {
                                stack.Pop();
                                // woc，咱们bug终于修好了，2024年11月3日 15点50分好了
                                if (stack.Count > 0)
                                {
                                    var parentParent = stack.Peek();
                                    if (parentParent is ObjectControllerCommandLeaf parentParentCommandLeaf)
                                    {
                                        if (parentParentCommandLeaf.MethodName == objCommandLeaf.Value)
                                        {
                                            if (parentParentCommandLeaf.ScnKeywords is not null)
                                            {
                                                if (parentParentCommandLeaf.ScnKeywords.Contains("parent"))
                                                {
                                                    //Debug.WriteLine($"父节点：{parentParentCommandLeaf.MethodName} 触发了清除选择");

                                                    stack.Pop();
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                var isVaild = false;
                                if (parentCommandLeaf.Parameters is not null)
                                {
                                    if (parentCommandLeaf.Parameters.ContainsKey("dtype"))
                                    {
                                        if (parentCommandLeaf.Parameters["dtype"] is ExpressParametersValueExpress express && express.Value == objCommandLeaf.Value)
                                        {
                                            // 判断parent的parent是否也名称一样，如果具有parent关键字，则清除选择
                                            if (stack.Count > 1)
                                            {
                                                //Debug.WriteLine($"具有两个及以上的父节点：{parentCommandLeaf.MethodName}");

                                                if (stack.ElementAt(stack.Count - 2) is ObjectControllerCommandLeaf parentParentCommandLeaf)
                                                {
                                                    if (parentParentCommandLeaf.Parameters is not null)
                                                        if (parentParentCommandLeaf.Parameters.ContainsKey("dtype") && parentParentCommandLeaf.Parameters["dtype"] is ExpressParametersValueExpress pexpress && pexpress.Value == objCommandLeaf.Value)
                                                        {
                                                            if (parentParentCommandLeaf.ScnKeywords is not null && parentParentCommandLeaf.ScnKeywords.Contains("parent"))
                                                            {
                                                                //Debug.WriteLine($"清除同样的父选择：{parentParentCommandLeaf.Value}");
                                                                stack.Pop();
                                                            }
                                                        }
                                                }
                                            }
                                            stack.Pop();
                                            isVaild = true;
                                        }
                                    }

                                }
                                if (!isVaild)
                                {
                                    problemProvider.ReportError("没有找到要取消选择的对象");
                                }

                            }
                        }

                    }
                }
                return;
            }
            else
            {
                // 处理~close捷径
                if (objCommandLeaf.ScnKeywords is not null)
                {
                    if (objCommandLeaf.ScnKeywords.Contains("close"))
                    {
                        stack.Pop();
                    }

                }
            }
            // 处理栈顶的节点如果为 ObjectControllerCommandLeaf
            // 判断类型是否一致
            if (stack.Count > 0)
            {
                if (stack.Peek() is ObjectControllerCommandLeaf parentCommandLeaf)
                {
                    var isSameType = false;
                    //Debug.WriteLine("判断同一个类型");
                    if (parentCommandLeaf.Parameters is not null)
                    {
                        //Debug.WriteLine("参数判断");
                        // 判断是否类型
                        if (parentCommandLeaf.Parameters.ContainsKey("dtype"))
                        {
                            if (parentCommandLeaf.Parameters["dtype"] is ExpressParametersValueExpress express)
                            {
                                //Debug.WriteLine($"判断dtype类型,父节点{express.Value}");

                                if (objCommandLeaf.Parameters is not null)
                                {
                                    if (objCommandLeaf.Parameters.ContainsKey("dtype"))
                                    {
                                        var myExpress = objCommandLeaf.Parameters["dtype"] as ExpressParametersValueExpress;
                                        if (myExpress is not null)
                                        {
                                            //Debug.WriteLine($"判断dtype类型,子节点{myExpress.Value}");

                                            if (myExpress.Value == express.Value)
                                            {
                                                //Debug.WriteLine("通过type判断是同一个类型");
                                                isSameType = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Debug.WriteLine("没有找到dtype的表达式");
                            }
                        }
                        else
                        {
                            //Debug.WriteLine("dtype不存在！！！！");
                        }
                    }

                    if (!isSameType)
                    {
                        if (parentCommandLeaf.MethodName == objCommandLeaf.MethodName)
                        {
                            //Debug.WriteLine("通过名称判断是同一个类型");
                            isSameType = true;
                        }
                        else
                        {
                            //Debug.WriteLine($"未能匹配成功！原方法名称{parentCommandLeaf.MethodName}，对象值名称{objCommandLeaf.MethodName}");
                        }
                    }



                    if (isSameType)
                    {
                        //Debug.WriteLine("已匹配成功");

                        // 属于同一个类型并且没有parent关键字
                        if (parentCommandLeaf.ScnKeywords is null)
                        {
                            //Debug.WriteLine("已触发");
                            stack.Pop();
                        }
                        else
                        {
                            if (!parentCommandLeaf.ScnKeywords.Contains("parent"))
                            {
                                //Debug.WriteLine("已触发");
                                stack.Pop();
                            }
                        }
                    }
                    else
                    {
                        //Debug.WriteLine("未匹配成功");
                    }
                }

                // 添加节点
                if (stack.Count > 0)
                {
                    var parent = stack.Peek();
                    parent.Children ??= [];
                    parent.Children.Add(objCommandLeaf);
                }
                else
                {
                    Tree.Add(objCommandLeaf);
                }

                // 如果该节点不是命令系统关键字，则将其入栈（进入选择）
                if (!objCommandSystemKeywords.Contains(objCommandLeaf.MethodName))
                {
                    stack.Push(leaf);
                }
            }
        }
        // 完成配置
        void CompleteConfig()
        {
            inConfig = false;
            configName = string.Empty;
        }

#if !DEBUG
        if (problemProvider.HasError)
        {
            throw new FileLoadException("文件存在语法错误！");
        }
#endif

    }
    public ReadOnlyCollection<ProblemInfo> GetProblems() => problemProvider.GetProblems();
}
/// <summary>
/// 所处环境状态
/// </summary>
public enum BuildStatusKind
{
    None,
    InString,
    InCommand,
    InObjectControllerCommand,
    InNodeOrScene,
}
