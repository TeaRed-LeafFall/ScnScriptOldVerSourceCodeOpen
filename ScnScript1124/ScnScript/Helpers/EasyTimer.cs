using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnScript.Helpers;
/// <summary>
/// 简易计时器
/// </summary>
public class EasyTimer
{
    private Stopwatch Stopwatch { get; set; } = new();

    /// <summary>
    /// 开始计时
    /// </summary>
    public void Start()
    {
        Stopwatch.Start();
    }
    /// <summary>
    /// 停止计时
    /// </summary>
    public void Stop()
    {
        Stopwatch.Stop();
        Stopwatch.Reset();
    }
    /// <summary>
    /// 停止计时并显示时间
    /// </summary>
    /// <param name="taskName">任务名称</param>
    public void StopWithShowTime(string taskName = "")
    {
        Stopwatch.Stop();
        Stopwatch.Reset();
        TimeSpan ts = Stopwatch.Elapsed;
        Console.WriteLine($"{taskName} 用时： {ts}");
    }
}
