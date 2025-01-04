using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Runtime.Basic;

public abstract class ScriptProvider
{
    public virtual void OnLoad(ScnScriptHost host){}

    public virtual void OnCall(){}
}
