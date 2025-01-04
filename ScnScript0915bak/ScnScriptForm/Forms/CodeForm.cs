using ScnScriptForm.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScnScriptForm.Forms
{
    public partial class CodeForm : Form
    {
        public string filePath { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public RichTextBoxHighlight rtbHighlight { get; set; } = new RichTextBoxHighlight();
        public CodeForm()
        {
            InitializeComponent();
        }
        public void LoadFile(string path)
        {
            
            filePath = path;
            code = File.ReadAllText(path);
            richTextBox1.Text = code;
            rtbHighlight.source = richTextBox1;
            rtbHighlight.Highlight();
        }

        private void CodeForm_Load(object sender, EventArgs e)
        {
            richTextBox1.BackColor = ColorTranslator.FromHtml("#263238");
            richTextBox1.ForeColor = ColorTranslator.FromHtml("#89ddff");
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
