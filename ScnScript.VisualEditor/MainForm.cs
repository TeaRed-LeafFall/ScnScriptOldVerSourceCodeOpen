namespace ScnScript.VisualEditor;

public partial class MainForm : Form
{
    bool isOpenProjectExplorerForm = false;
    ProjectExplorerForm projectExplorerForm = new();
    string appTitleBase = "Visual ScnScript Editor";

    public MainForm()
    {
        InitializeComponent();
    }

    private void 文件FToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        var form = new EditorForm();
        form.MdiParent = this;
        form.Show();
    }

    private void 层叠CToolStripMenuItem_Click(object sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.Cascade);
    }

    private void 垂直平铺VToolStripMenuItem_Click(object sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.TileVertical);
    }

    private void 水平平铺HToolStripMenuItem_Click(object sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.TileHorizontal);
    }

    private void 全部关闭LToolStripMenuItem_Click(object sender, EventArgs e)
    {
        foreach (Form childForm in MdiChildren)
        {
            childForm.Close();
        }
    }

    private void 项目资源管理器ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (isOpenProjectExplorerForm)
        {
            isOpenProjectExplorerForm = false;
            projectExplorerForm.Close();
        }
        else
        {
            isOpenProjectExplorerForm = true;
            projectExplorerForm = new ProjectExplorerForm();
            projectExplorerForm.MdiParent = this;
            projectExplorerForm.FormClosing += (sender, e) =>
            {
                isOpenProjectExplorerForm = false;
                项目资源管理器ToolStripMenuItem.Checked = isOpenProjectExplorerForm;
            };
            projectExplorerForm.Show();

        }
        项目资源管理器ToolStripMenuItem.Checked = isOpenProjectExplorerForm;
    }

    private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void UpdateTitle()
    {
        if (ActiveMdiChild != null)
        {
            //Text = appTitleBase + " - " + ActiveMdiChild.Text + " (活动中)";
        }
        else
        {
            Text = appTitleBase;
        }
    }

    private void MainForm_MdiChildActivate(object sender, EventArgs e)
    {
        UpdateTitle();
    }

    private void 代码CToolStripMenuItem_Click(object sender, EventArgs e)
    {
        文件FToolStripMenuItem1_Click(sender, e);
    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
        文件FToolStripMenuItem1_Click(sender, e);
    }
}
