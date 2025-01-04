
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
            // �ɵ����̵߳��ü��
            // ui�߳�ִ�ж���̫�����ˣ�Invoke����һ������һ���ǳ�������
            // ���Ǿ����ñ�����ui�߳�ִ�еĴ�����ui�߳���ִ�У�����show
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

        private void ���ļ���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                explorerForm.Show();
                explorerForm.LoadDirectory(folderBrowserDialog1.SelectedPath);
            }

        }

        private void ��Դ������ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void ��־��¼��ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void ���ݲ鿴��ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void ��ʼ����ToolStripMenuItem_Click(object sender, EventArgs e)
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
                    MessageBox.Show("�ļ�������!", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("���ȴ�һ���ļ�!" + Environment.NewLine + "���ᱻ����Ϊ����Ŀ�꣡", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ��ʼ����ToolStripMenuItem.Enabled = false;
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
                MessageBox.Show(ex.Message, "��������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            


        Run:
            try
            {
                host.Run(sandBoxId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��������", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(e.Error.Message, "��������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // ����UI
            dataForm.LoadData(host.GetSandBox(sandBoxId).WorkerData);
            ��ʼ����ToolStripMenuItem.Enabled = true;
            toolStripButton2.Enabled = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;//���ñ�����
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // ��ֹ��������.
            SetStyle(ControlStyles.DoubleBuffer, true); // ˫����
        }
        /// <summary>
        /// ���ҳ��Ƶ��ˢ��ʱ������˸����
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
