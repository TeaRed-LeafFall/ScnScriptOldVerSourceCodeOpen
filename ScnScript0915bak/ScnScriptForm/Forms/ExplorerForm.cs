using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ScnScriptForm.Forms;

public partial class ExplorerForm : Form
{
    public string SelectedPath { get; set; } = string.Empty;
    public delegate void ExplorerFileClickDelegate(string filePath);
    public event ExplorerFileClickDelegate? ExplorerFileClick;
    public ExplorerForm()
    {
        InitializeComponent();

    }

    private void ExplorerForm_Load(object sender, EventArgs e)
    {
        this.DoubleBuffered = true;//设置本窗体
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
        SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
    }

    /// <summary>
    /// 解决页面频繁刷新时界面闪烁问题
    /// </summary>
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x02000000;
            return cp;
        }
    }

    public void LoadDirectory(string path)
    {
        if (!Directory.Exists(path)) return;
        SelectedPath = path;
        RefreshDirectory();
    }
    public void RefreshDirectory()
    {
        treeView1.Nodes.Clear();
        var node = AddDirectoryNode(SelectedPath);
        treeView1.Nodes.Add(node);
        node.Expand();
    }
    // treeView update
    public TreeNode AddDirectoryNode(string path)
    {
        TreeNode node = new TreeNode(Path.GetFileName(path));
        foreach (string dir in Directory.GetDirectories(path))
        {
            var dir2 = Path.GetFileName(dir);
            if (dir2.StartsWith(".")) continue;
            if (dir2 is "bin" or "obj") continue;
            var subNode = AddDirectoryNode(dir);
            //subNode.ImageIndex = 0;
            //subNode.SelectedImageIndex = 0;
            node.Nodes.Add(subNode);
        }
        foreach (string file in Directory.GetFiles(path))
        {
            var file2 = Path.GetFileName(file);
            if (file2.StartsWith(".")) continue;
            if(Path.GetExtension(file2) is ".cs" or ".cpp" or ".h" or ".txt" or ".json" or ".exe" or ".csproj" or ".sln") continue;
            var subNode = new TreeNode(file2);
            //if (Path.GetExtension(file2) == ".scn")
            //{
            //    subNode.ImageIndex = 2;
            //    subNode.SelectedImageIndex = 2;
            //}
            //else
            //{
            //    subNode.ImageIndex = 1;
            //    subNode.SelectedImageIndex = 1;
            //}

            node.Nodes.Add(subNode);
        }
        return node;
    }


    private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
        var node = e.Node;
        if (node is null) return;
        // 确保是脚本文件
        if (Path.GetExtension(node.Text) is ".scn")
        {
            var path = Path.GetDirectoryName(SelectedPath) + "\\" + node.FullPath;
            ExplorerFileClick?.Invoke(path);
        }
    }
}
