using ScnScript.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.CodeAnalysis;


public static partial class ScriptCodeAnalysis
{
    public static BehaviorTree Parse(string context)
    {
        var tokens = Lexer(context);
        BehaviorTree bt = new BehaviorTree();
        bt.Build(tokens);
        return bt;
    }
}
