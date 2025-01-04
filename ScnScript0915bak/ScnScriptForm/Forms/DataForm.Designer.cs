namespace ScnScriptForm.Forms
{
    partial class DataForm
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
            components = new System.ComponentModel.Container();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            treeView1 = new TreeView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            刷新ToolStripMenuItem = new ToolStripMenuItem();
            tabPage2 = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            comboBox1 = new ComboBox();
            propertyGrid1 = new PropertyGrid();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            tabPage2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(287, 436);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(treeView1);
            tabPage1.Location = new Point(4, 32);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(279, 400);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "数据库";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            treeView1.ContextMenuStrip = contextMenuStrip1;
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(3, 3);
            treeView1.Name = "treeView1";
            treeView1.PathSeparator = ".";
            treeView1.Size = new Size(273, 394);
            treeView1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 刷新ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(113, 32);
            // 
            // 刷新ToolStripMenuItem
            // 
            刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            刷新ToolStripMenuItem.Size = new Size(112, 28);
            刷新ToolStripMenuItem.Text = "刷新";
            刷新ToolStripMenuItem.Click += 刷新ToolStripMenuItem_Click;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel1);
            tabPage2.Location = new Point(4, 32);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(279, 400);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "对象查看";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(comboBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(propertyGrid1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(273, 394);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(267, 31);
            comboBox1.TabIndex = 1;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // propertyGrid1
            // 
            propertyGrid1.Dock = DockStyle.Fill;
            propertyGrid1.Location = new Point(3, 40);
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.Size = new Size(267, 351);
            propertyGrid1.TabIndex = 0;
            // 
            // DataForm
            // 
            AutoScaleDimensions = new SizeF(10F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(287, 436);
            Controls.Add(tabControl1);
            Name = "DataForm";
            Text = "DataForm";
            Load += DataForm_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ComboBox comboBox1;
        private PropertyGrid propertyGrid1;
        private TableLayoutPanel tableLayoutPanel1;
        private TreeView treeView1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 刷新ToolStripMenuItem;
    }
}