using ScnScript.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.CodeAnalysis;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly struct ScriptToken
{
    public string? Text { get; init; }
    public TokenKind Kind { get; init; }
    public override string ToString()
    {
        if(Text is null)
        {
            return Kind is TokenKind.Lf ? "LF \\n" : $"{Kind} {(char)Kind}";
        }
        return $"{Kind} {Text}";
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}

