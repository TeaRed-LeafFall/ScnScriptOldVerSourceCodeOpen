
using ScnScript.Extension.MenuModel;
using ScnScript.Runtime;
using ScnScriptForm.Forms;

namespace ScnScriptForm
{
    public partial class MainForm : Form
    {
        ExplorerForm explorerForm = new();
        ConsoleForm consoleForm = new();
        DataForm dataForm = new();
        Dictionary<string, CodeForm> codeForms = new();
        ScnScriptHost host = new();
        public bool isHighlightOut = false;
        string sandBoxId = string.Empty;
        public MainForm()
        {
            InitializeComponent();
            // 干掉跨线程调用检查
            // ui线程执行东西太烦人了，Invoke这里一个那里一个非常繁琐。
            // 我们尽量让必须在ui线程执行的代码在ui线程里执行，比如show
            CheckForIllegalCrossThreadCalls = false;
            explorerForm.MdiParent = this;
            explorerForm.ExplorerFileClick += ExplorerForm_ExplorerFileClick;
            explorerForm.Dock = DockStyle.Left;
            consoleForm.MdiParent = this;
            consoleForm.SetMainForm(this);
            consoleForm.Dock = DockStyle.Bottom;
            dataForm.MdiParent = this;
            dataForm.Dock = DockStyle.Right;
            host.RegisterFuncBook(new MenuModelModelLib());
            ScnScriptForm.Extension.FormModelLib.MainForm = this;
            host.RegisterFuncBook(new ScnScriptForm.Extension.FormModelLib());
            host.RegisterFuncBook(new ScnScript.Extension.VisualNovelModel.VisualNovelModelLib());
#if DEBUG
            explorerForm.LoadDirectory(@"D:\dev\ScnScript\ScnScriptConsole");
#endif
            explorerForm.Show();
            consoleForm.Show();
            dataForm.Show();
        }

        private void ExplorerForm_ExplorerFileClick(string filePath)
        {
            //MessageBox.Show(filePath);
            if (!codeForms.ContainsKey(filePath))
            {
                var codeForm = new CodeForm();
                codeForm.MdiParent = this;
                codeForm.Text = Path.GetFileName(filePath) + " - CodeForm";
                codeForm.FormClosed += CodeForm_FormClosed;
                codeForm.Show();
                codeForms.Add(filePath, codeForm);
                isHighlightOut = true;
                codeForm.LoadFile(filePath);
                isHighlightOut = false;
            }
            else
            {
                codeForms[filePath].Show();
            }
            toolStripLabel1.Text = Path.GetFileName(filePath);
            toolStripLabel1.Tag = filePath;
        }

        private void CodeForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if (sender is CodeForm codeForm)
            {
                codeForms.Remove(codeForm.filePath);
            }
        }

        private void 打开文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                explorerForm.Show();
                explorerForm.LoadDirectory(folderBrowserDialog1.SelectedPath);
            }

        }

        private void 资源管理器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (explorerForm.Visible)
            {
                explorerForm.Hide();
            }
            else
            {
                explorerForm.Show();
            }
        }

        private void 日志记录器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (consoleForm.Visible)
            {
                consoleForm.Hide();
            }
            else
            {
                consoleForm.Show();
            }
        }

        private void 数据查看器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataForm.Visible)
            {
                dataForm.Hide();
            }
            else
            {
                dataForm.Show();
            }
        }

        private void 开始调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
        private void Run()
        {

            var path = string.Empty;
            if (toolStripLabel1.Tag is string)
            {
                path = toolStripLabel1.Tag as string;
                if (!File.Exists(path))
                {
                    MessageBox.Show("文件不存在!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("请先打开一个文件!" + Environment.NewLine + "他会被设置为启动目标！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            开始调试ToolStripMenuItem.Enabled = false;
            toolStripButton2.Enabled = false;
            if (!string.IsNullOrEmpty(sandBoxId))
            {
                host.GetSandBox(sandBoxId).Dispose();
            }

            host.Clean();
            consoleForm.Clear();
            try
            {
                sandBoxId = host.CreateSandBox(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            


        Run:
            try
            {
                host.Run(sandBoxId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (host is { IsWaiting: true, IsOver: false })
            {
                goto Run;
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Run();

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "发生错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // 更新UI
            dataForm.LoadData(host.GetSandBox(sandBoxId).WorkerData);
            开始调试ToolStripMenuItem.Enabled = true;
            toolStripButton2.Enabled = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
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
    }
}
