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
    public partial class DataForm : Form
    {
        public Dictionary<string, object>? Dic;
        public DataForm()
        {
            InitializeComponent();

        }
        public void LoadData(Dictionary<string, object> dic)
        {
            //propertyGrid1.SelectedObject = dic;
            Dic = dic;
            RefreshView();
        }

        private void DataForm_Load(object sender, EventArgs e)
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

        public void RefreshView()
        {
            treeView1.Nodes.Clear();
            if (Dic != null)
            {

                foreach (var item in Dic)
                {
                    SpawnNode(treeView1, item.Key);
                }
                if (treeView1.Nodes.Count > 0)
                    treeView1.ExpandAll();
            }
            //comboBox1.Items.Clear();
            if (Dic != null)
            {
                comboBox1.DataSource = Dic.Keys.ToList();
                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;
            }
        }
        public void SpawnNode(TreeView parent, string key)
        {
            if (key.Contains('.'))
            {
                var rKey = key.Split('.').First();
                var subkey = key.Substring(rKey.Length + 1);
                var isHave = parent.Nodes.ContainsKey(rKey);
                if (!isHave)
                {
                    SpawnNode(parent.Nodes.Add(rKey, rKey), subkey);
                }
                else
                {
                    SpawnNode(parent.Nodes[rKey]!, subkey);
                }
            }
            else
            {
                parent.Nodes.Add(key, key);
            }
        }
        public void SpawnNode(TreeNode parent, string key)
        {
            if (key.Contains('.'))
            {
                var rKey = key.Split('.').First();
                var subkey = key.Substring(rKey.Length + 1);
                var isHave = parent.Nodes.ContainsKey(rKey);
                if (!isHave)
                {

                    SpawnNode(parent.Nodes.Add(rKey, rKey), subkey);
                }
                else
                {
                    SpawnNode(parent.Nodes[rKey]!, subkey);
                }
            }
            else
            {
                parent.Nodes.Add(key, key);
            }

        }
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is null) return;
            if (Dic is null) return;
            var key = comboBox1.SelectedItem.ToString();
            if (key is null) return;
            if (Dic.ContainsKey(key))
            {
                propertyGrid1.SelectedObject = Dic[key];
            }
        }
    }
}
