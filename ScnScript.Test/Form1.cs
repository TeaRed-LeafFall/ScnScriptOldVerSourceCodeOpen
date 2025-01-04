using System.Diagnostics;

namespace ScnScript.Test;


public partial class Form1 : Form
{
    private readonly ScnRunner runner = new();

    public Form1()
    {
        InitializeComponent();
    }

    private void 载入文件ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DialogResult dr = openFileDialog.ShowDialog(this);
        if (dr == DialogResult.OK)
        {
            CodeBox.Text = File.ReadAllText(openFileDialog.FileName);
        }
    }

    private void SLexerButton_Click(object sender, EventArgs e)
    {
        SLexerButton.Enabled = false;
        if (string.IsNullOrEmpty(CodeBox.Text))
        {
            SLexerButton.Enabled = true;
            return;
        }

        try
        {
            runner.InputString(CodeBox.Text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ScnScript 发生错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        List<Token> tokens = runner.GetTokenList();
        SOutBox.Text = String.Empty;
        SCNTreeview.Nodes.Clear();
        SCNTreeview.Nodes.Add("Root");

        for (var i = 0; i < tokens.Count; i++)
        {
            var str = $"({i}) {tokens[i].TokenType}";

            if (!string.IsNullOrEmpty(tokens[i].Key))
            {
                str += $"\tKey: '{tokens[i].Key}'";
            }

            if (!string.IsNullOrEmpty(tokens[i].Value))
            {
                str += $"\tValue: '{tokens[i].Value}'";
            }

            if (tokens[i].Tags.Count > 0)
            {
                str += $"\tTags: '{string.Join(",", tokens[i].Tags)}'";
            }
            SOutBox.Text += str + Environment.NewLine;

            if (tokens[i].TokenType != TokenType.ObjClr && tokens[i].TokenType != TokenType.ScnComments && tokens[i].TokenType != TokenType.LocalCommand && tokens[i].TokenType != TokenType.StringKey && tokens[i].TokenType != TokenType.GlobalCommand)
            {
                SCNTreeview.Nodes[0].Nodes.
                    Add(new TreeNode
                    {
                        Text = $@"{tokens[i]?.Key} ({tokens[i].TokenType})",
                        ToolTipText = str,
                    });
            }
        }
        SCNTreeview.Nodes[0].Expand();
        SLexerButton.Enabled = true;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        CodeBox.Text = File.ReadAllText(@"D:\dev\ScnScript\ScnScript.Test\TestSample\MainWindowMenu.scn");
    }

    private void SRunButton_Click(object sender, EventArgs e)
    {
        SRunButton.Enabled = false;
        try
        {
            runner.RunAllCommand();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ScnScript 发生错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        SRunButton.Enabled = true;
    }
}
