using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Runtime.Basic;
[AttributeUsage(AttributeTargets.Class)]
public class ScriptObjectAttribute(string name, params string[]? akaNames) : Attribute
{
    public string Name { get; private init; } = name;
    public string[]? AkaNames { get; private init; } = akaNames;
}
