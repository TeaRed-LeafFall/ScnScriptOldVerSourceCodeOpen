using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScriptConsole;

public class EasyTimer : Stopwatch
{
    public void ShowTime()
    {
        this.Stop();
        this.Reset();
        TimeSpan ts = this.Elapsed;
        string elapsedTime = $"{ts.Hours:00}h{ts.Minutes:00}m{ts.Seconds:00}.{ts.Milliseconds:000}s";
        Console.WriteLine("加载用时：" + elapsedTime);
    }
}
