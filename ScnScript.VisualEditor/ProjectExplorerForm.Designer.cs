namespace ScnScript.VisualEditor;

partial class ProjectExplorerForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        var treeNode1 = new TreeNode("节点1");
        var treeNode2 = new TreeNode("节点2");
        var treeNode3 = new TreeNode("节点0", new TreeNode[] { treeNode1, treeNode2 });
        var treeNode4 = new TreeNode("节点4");
        var treeNode5 = new TreeNode("节点5");
        var treeNode6 = new TreeNode("节点3", new TreeNode[] { treeNode4, treeNode5 });
        var treeNode7 = new TreeNode("节点8");
        var treeNode8 = new TreeNode("节点9");
        var treeNode9 = new TreeNode("节点7", new TreeNode[] { treeNode7, treeNode8 });
        var treeNode10 = new TreeNode("节点6", new TreeNode[] { treeNode9 });
        var treeNode11 = new TreeNode("节点11");
        var treeNode12 = new TreeNode("节点10", new TreeNode[] { treeNode11 });
        var resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectExplorerForm));
        treeView1 = new TreeView();
        SuspendLayout();
        // 
        // treeView1
        // 
        treeView1.BackColor = Color.FromArgb(31, 31, 31);
        treeView1.BorderStyle = BorderStyle.None;
        treeView1.Dock = DockStyle.Fill;
        treeView1.ForeColor = Color.White;
        treeView1.FullRowSelect = true;
        treeView1.HideSelection = false;
        treeView1.ImeMode = ImeMode.NoControl;
        treeView1.LabelEdit = true;
        treeView1.Location = new Point(0, 0);
        treeView1.Name = "treeView1";
        treeNode1.Name = "节点1";
        treeNode1.Text = "节点1";
        treeNode2.Name = "节点2";
        treeNode2.Text = "节点2";
        treeNode3.Name = "节点0";
        treeNode3.Text = "节点0";
        treeNode4.Name = "节点4";
        treeNode4.Text = "节点4";
        treeNode5.Name = "节点5";
        treeNode5.Text = "节点5";
        treeNode6.Name = "节点3";
        treeNode6.Text = "节点3";
        treeNode7.Name = "节点8";
        treeNode7.Text = "节点8";
        treeNode8.Name = "节点9";
        treeNode8.Text = "节点9";
        treeNode9.Name = "节点7";
        treeNode9.Text = "节点7";
        treeNode10.Name = "节点6";
        treeNode10.Text = "节点6";
        treeNode11.Name = "节点11";
        treeNode11.Text = "节点11";
        treeNode12.Name = "节点10";
        treeNode12.Text = "节点10";
        treeView1.Nodes.AddRange(new TreeNode[] { treeNode3, treeNode6, treeNode10, treeNode12 });
        treeView1.ShowLines = false;
        treeView1.Size = new Size(287, 359);
        treeView1.TabIndex = 0;
        // 
        // ProjectExplorerForm
        // 
        AutoScaleDimensions = new SizeF(7F, 19F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(287, 359);
        Controls.Add(treeView1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(303, 398);
        Name = "ProjectExplorerForm";
        ShowIcon = false;
        Text = "Project Explorer";
        ResumeLayout(false);
    }

    #endregion

    private TreeView treeView1;
}