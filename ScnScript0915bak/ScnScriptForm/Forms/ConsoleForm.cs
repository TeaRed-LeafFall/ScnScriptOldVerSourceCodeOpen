using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScnScriptForm.Forms
{
    public partial class ConsoleForm : Form
    {
        MainForm? mainForm;
        public ConsoleForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //if (this.IsHandleCreated)
            //{

            //}
            //else
            //{
            //    this.HandleCreated += (sender, e) => AddEventHandlers();
            //}
            AddEventHandlers();

        }
        private void AddEventHandlers()
        {
            ScnScript.Runtime.Console.LogOutput += (msgBoxResult, type) =>
            {
                //Debug.WriteLine(type + ": " + msgBoxResult);
                if (mainForm is not null)
                {
                    if (mainForm.isHighlightOut) return;
                }
                if (mainForm is not null && mainForm.isHighlightOut) return;
                textBox1.AppendText(type + ": " + msgBoxResult + Environment.NewLine);
                textBox1.Select(textBox1.TextLength, 0);
                textBox1.ScrollToCaret();
            };
            ScnScript.Runtime.Console.ReadInput += mode =>
            {
                return "clear";
            };
            ScnScript.Runtime.Console.WriteOutput += (string content) =>
            {
                textBox2.AppendText(content);
                textBox2.Select(textBox2.TextLength, 0);
                textBox2.ScrollToCaret();
            };
        }

        public void SetMainForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }
        public void Clear()
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void ConsoleForm_Load(object sender, EventArgs e)
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
