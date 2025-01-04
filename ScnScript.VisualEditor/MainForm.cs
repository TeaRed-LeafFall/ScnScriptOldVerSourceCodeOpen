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

    private void �ļ�FToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        var form = new EditorForm();
        form.MdiParent = this;
        form.Show();
    }

    private void ���CToolStripMenuItem_Click(object sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.Cascade);
    }

    private void ��ֱƽ��VToolStripMenuItem_Click(object sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.TileVertical);
    }

    private void ˮƽƽ��HToolStripMenuItem_Click(object sender, EventArgs e)
    {
        LayoutMdi(MdiLayout.TileHorizontal);
    }

    private void ȫ���ر�LToolStripMenuItem_Click(object sender, EventArgs e)
    {
        foreach (Form childForm in MdiChildren)
        {
            childForm.Close();
        }
    }

    private void ��Ŀ��Դ������ToolStripMenuItem_Click(object sender, EventArgs e)
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
                ��Ŀ��Դ������ToolStripMenuItem.Checked = isOpenProjectExplorerForm;
            };
            projectExplorerForm.Show();

        }
        ��Ŀ��Դ������ToolStripMenuItem.Checked = isOpenProjectExplorerForm;
    }

    private void �˳�XToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void UpdateTitle()
    {
        if (ActiveMdiChild != null)
        {
            //Text = appTitleBase + " - " + ActiveMdiChild.Text + " (���)";
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

    private void ����CToolStripMenuItem_Click(object sender, EventArgs e)
    {
        �ļ�FToolStripMenuItem1_Click(sender, e);
    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
        �ļ�FToolStripMenuItem1_Click(sender, e);
    }
}
