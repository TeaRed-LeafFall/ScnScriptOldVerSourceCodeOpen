using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ScnScript.VisualEditor;
public partial class EditorForm : Form
{
    float standardFontSize;
    public EditorForm()
    {
        InitializeComponent();
    }


    private void EditorForm_Load(object sender, EventArgs e)
    {
        toolStripComboBox1.Text = toolStripComboBox1.Items[0].ToString();
        textBox1.MouseWheel += new MouseEventHandler(textBox1_MouseWheel);
        standardFontSize = textBox1.Font.Size;
    }

    private void textBox1_MouseWheel(object? sender, MouseEventArgs e)
    {
        if (Control.ModifierKeys == Keys.Control)
        {
            var delta = e.Delta;
            var direction = Math.Sign(delta);
            var newSize = textBox1.Font.Size + direction;
            var zoomRatio = newSize / standardFontSize;
            var zoomPercentage = (int)(zoomRatio * 100);

            if (zoomPercentage >= 400 && direction == 1)
            {
                newSize = standardFontSize * 4;
            }
            else if (zoomPercentage <= 69 && direction == -1)
            {
                newSize = standardFontSize * 0.69f;
            }
            if (zoomPercentage < 69 || zoomPercentage > 400)
            {
                return;
            }
            textBox1.Font = new Font(textBox1.Font.FontFamily, Convert.ToInt32(newSize));
            toolStripStatusLabel3.Text = $"{zoomPercentage}%";
        }
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        var s = textBox1.Text;
        var sp = s.Split(Environment.NewLine);
        var intsAllLine = sp.Length;
        int selectionStart = textBox1.SelectionStart;  // 获取选中文本的起始位置
        int lineNum = textBox1.GetLineFromCharIndex(selectionStart) + 1;  // 获取选中文本的行号
        int columnNum = selectionStart - textBox1.GetFirstCharIndexOfCurrentLine() + 1;  // 获取选中文本的列号
        if (columnNum < 0)
        {
            columnNum = textBox1.GetFirstCharIndexOfCurrentLine() - selectionStart + 1;  // 获取选中文本的列号
            toolStripStatusLabel1.Text = $"共 {intsAllLine} 行 (选中 {lineNum} 行 {columnNum} 长度)";
        }
        else
        {
            toolStripStatusLabel1.Text = $"共 {intsAllLine} 行 (选中 {lineNum} 行 {columnNum} 列)";
        }

        toolStripStatusLabel2.Text = $"共 {s.Length} 字";
    }

    private void textBox1_MouseDown(object sender, MouseEventArgs e)
    {
        textBox1_TextChanged(sender, e);
    }

    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
        textBox1_TextChanged(sender, e);
    }

    private void toolStripStatusLabel3_Click(object sender, EventArgs e)
    {
        textBox1.Font = new Font(textBox1.Font.FontFamily, Convert.ToInt32(standardFontSize));
        toolStripStatusLabel3.Text = $"100%";
    }
}
