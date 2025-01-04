using ScnScript.Highlight;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ScnScriptForm.Helpers;

public class RichTextBoxHighlight
{
    HighlightHelper helper = new HighlightHelper();
    public RichTextBox? source { get; set; }
    public void Highlight()
    {
        if (source is null) return;
        source.UseWaitCursor = true;
        helper.HighlightByCode(source.Text);
        var hData = helper.GetHighlightData();
        var MaxLineNum = helper.GetMaxLineNum();
        for (int i = 1; i <= MaxLineNum; i++)
        {
            source.DeselectAll();
            var sourceColor = source.SelectionColor;
            if (hData.HasThisLine(i))
            {
                var data = hData.GetLineHighlightAreaTypes(i);
                if (data is null) continue;

                // 反向排序
                var newData = data.OrderBy(x => x.Key.Start).ToDictionary();
                if (newData is null) continue;
                foreach (var item in newData)
                {
                    var point = helper.GetLineToPoint(i);
                    var start = point + item.Key.Start + i - 1;

                    source.Select(start, item.Key.GetLength());
                    source.SelectionColor = HighlightConsoleOut.GetColor(item.Value);
                }
            }
            
            source.DeselectAll();
            source.SelectionColor = sourceColor;
        }
        source.Select(0, 0);
        source.Refresh();
        source.ReadOnly = true;
        source.UseWaitCursor = false;
    }
}
