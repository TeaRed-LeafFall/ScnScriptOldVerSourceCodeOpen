namespace ScnScript.Test;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        splitContainer1 = new SplitContainer();
        splitContainer2 = new SplitContainer();
        groupBox2 = new GroupBox();
        SCNTreeview = new TreeView();
        groupBox1 = new GroupBox();
        SCNOutM = new Label();
        splitContainer3 = new SplitContainer();
        CodeBox = new TextBox();
        contextMenuStrip1 = new ContextMenuStrip(components);
        载入文件ToolStripMenuItem = new ToolStripMenuItem();
        tabControl2 = new TabControl();
        tabPage4 = new TabPage();
        SOutBox = new TextBox();
        tabPage3 = new TabPage();
        flowLayoutPanel1 = new FlowLayoutPanel();
        SLexerButton = new Button();
        SRunButton = new Button();
        S = new Button();
        toolTip = new ToolTip(components);
        openFileDialog = new OpenFileDialog();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
        splitContainer2.Panel1.SuspendLayout();
        splitContainer2.Panel2.SuspendLayout();
        splitContainer2.SuspendLayout();
        groupBox2.SuspendLayout();
        groupBox1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
        splitContainer3.Panel1.SuspendLayout();
        splitContainer3.Panel2.SuspendLayout();
        splitContainer3.SuspendLayout();
        contextMenuStrip1.SuspendLayout();
        tabControl2.SuspendLayout();
        tabPage4.SuspendLayout();
        tabPage3.SuspendLayout();
        flowLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(splitContainer2);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(splitContainer3);
        splitContainer1.Size = new Size(834, 623);
        splitContainer1.SplitterDistance = 216;
        splitContainer1.TabIndex = 0;
        // 
        // splitContainer2
        // 
        splitContainer2.Dock = DockStyle.Fill;
        splitContainer2.Location = new Point(0, 0);
        splitContainer2.Name = "splitContainer2";
        splitContainer2.Orientation = Orientation.Horizontal;
        // 
        // splitContainer2.Panel1
        // 
        splitContainer2.Panel1.Controls.Add(groupBox2);
        // 
        // splitContainer2.Panel2
        // 
        splitContainer2.Panel2.Controls.Add(groupBox1);
        splitContainer2.Size = new Size(216, 623);
        splitContainer2.SplitterDistance = 382;
        splitContainer2.TabIndex = 0;
        // 
        // groupBox2
        // 
        groupBox2.Controls.Add(SCNTreeview);
        groupBox2.Dock = DockStyle.Fill;
        groupBox2.ForeColor = SystemColors.ButtonHighlight;
        groupBox2.Location = new Point(0, 0);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(216, 382);
        groupBox2.TabIndex = 0;
        groupBox2.TabStop = false;
        groupBox2.Text = "节点与对象管理器";
        // 
        // SCNTreeview
        // 
        SCNTreeview.BackColor = Color.FromArgb(31, 31, 31);
        SCNTreeview.BorderStyle = BorderStyle.None;
        SCNTreeview.Dock = DockStyle.Fill;
        SCNTreeview.ForeColor = SystemColors.Window;
        SCNTreeview.FullRowSelect = true;
        SCNTreeview.HideSelection = false;
        SCNTreeview.HotTracking = true;
        SCNTreeview.LineColor = Color.DimGray;
        SCNTreeview.Location = new Point(3, 22);
        SCNTreeview.Name = "SCNTreeview";
        SCNTreeview.ShowNodeToolTips = true;
        SCNTreeview.Size = new Size(210, 357);
        SCNTreeview.TabIndex = 0;
        toolTip.SetToolTip(SCNTreeview, "节点与对象管理器");
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(SCNOutM);
        groupBox1.Dock = DockStyle.Fill;
        groupBox1.Font = new Font("Resource Han Rounded CN", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
        groupBox1.ForeColor = SystemColors.ButtonHighlight;
        groupBox1.Location = new Point(0, 0);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(216, 237);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "预览";
        toolTip.SetToolTip(groupBox1, "预览输出");
        // 
        // SCNOutM
        // 
        SCNOutM.Dock = DockStyle.Fill;
        SCNOutM.Location = new Point(3, 22);
        SCNOutM.Name = "SCNOutM";
        SCNOutM.Size = new Size(210, 212);
        SCNOutM.TabIndex = 0;
        SCNOutM.Text = "尝试在此输出内容";
        SCNOutM.TextAlign = ContentAlignment.MiddleCenter;
        toolTip.SetToolTip(SCNOutM, "预览输出文本");
        // 
        // splitContainer3
        // 
        splitContainer3.Dock = DockStyle.Fill;
        splitContainer3.Location = new Point(0, 0);
        splitContainer3.Name = "splitContainer3";
        splitContainer3.Orientation = Orientation.Horizontal;
        // 
        // splitContainer3.Panel1
        // 
        splitContainer3.Panel1.Controls.Add(CodeBox);
        // 
        // splitContainer3.Panel2
        // 
        splitContainer3.Panel2.Controls.Add(tabControl2);
        splitContainer3.Size = new Size(614, 623);
        splitContainer3.SplitterDistance = 428;
        splitContainer3.TabIndex = 0;
        // 
        // CodeBox
        // 
        CodeBox.AcceptsReturn = true;
        CodeBox.AcceptsTab = true;
        CodeBox.BackColor = Color.FromArgb(31, 31, 31);
        CodeBox.BorderStyle = BorderStyle.None;
        CodeBox.ContextMenuStrip = contextMenuStrip1;
        CodeBox.Dock = DockStyle.Fill;
        CodeBox.ForeColor = SystemColors.Window;
        CodeBox.Location = new Point(0, 0);
        CodeBox.Multiline = true;
        CodeBox.Name = "CodeBox";
        CodeBox.PlaceholderText = "请输入ScnScript代码或右键以加载文件";
        CodeBox.ScrollBars = ScrollBars.Vertical;
        CodeBox.Size = new Size(614, 428);
        CodeBox.TabIndex = 1;
        toolTip.SetToolTip(CodeBox, "ScnScript代码普通输入框");
        // 
        // contextMenuStrip1
        // 
        contextMenuStrip1.BackColor = Color.FromArgb(61, 61, 61);
        contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 载入文件ToolStripMenuItem });
        contextMenuStrip1.Name = "contextMenuStrip1";
        contextMenuStrip1.RenderMode = ToolStripRenderMode.System;
        contextMenuStrip1.Size = new Size(131, 28);
        // 
        // 载入文件ToolStripMenuItem
        // 
        载入文件ToolStripMenuItem.ForeColor = SystemColors.Window;
        载入文件ToolStripMenuItem.Name = "载入文件ToolStripMenuItem";
        载入文件ToolStripMenuItem.Size = new Size(130, 24);
        载入文件ToolStripMenuItem.Text = "载入文件";
        载入文件ToolStripMenuItem.Click += 载入文件ToolStripMenuItem_Click;
        // 
        // tabControl2
        // 
        tabControl2.Controls.Add(tabPage4);
        tabControl2.Controls.Add(tabPage3);
        tabControl2.Dock = DockStyle.Fill;
        tabControl2.Location = new Point(0, 0);
        tabControl2.Name = "tabControl2";
        tabControl2.SelectedIndex = 0;
        tabControl2.ShowToolTips = true;
        tabControl2.Size = new Size(614, 191);
        tabControl2.SizeMode = TabSizeMode.Fixed;
        tabControl2.TabIndex = 0;
        // 
        // tabPage4
        // 
        tabPage4.BackColor = Color.FromArgb(31, 31, 31);
        tabPage4.Controls.Add(SOutBox);
        tabPage4.Location = new Point(4, 28);
        tabPage4.Name = "tabPage4";
        tabPage4.Padding = new Padding(3);
        tabPage4.Size = new Size(606, 159);
        tabPage4.TabIndex = 1;
        tabPage4.Text = "日志";
        tabPage4.ToolTipText = "日志选项卡";
        // 
        // SOutBox
        // 
        SOutBox.BackColor = Color.FromArgb(31, 31, 31);
        SOutBox.BorderStyle = BorderStyle.None;
        SOutBox.Dock = DockStyle.Fill;
        SOutBox.ForeColor = SystemColors.Window;
        SOutBox.Location = new Point(3, 3);
        SOutBox.Multiline = true;
        SOutBox.Name = "SOutBox";
        SOutBox.PlaceholderText = "也许在操作点击“开始运行”按钮会显示一些内容";
        SOutBox.ReadOnly = true;
        SOutBox.ScrollBars = ScrollBars.Vertical;
        SOutBox.Size = new Size(600, 153);
        SOutBox.TabIndex = 0;
        // 
        // tabPage3
        // 
        tabPage3.BackColor = Color.FromArgb(31, 31, 31);
        tabPage3.Controls.Add(flowLayoutPanel1);
        tabPage3.Location = new Point(4, 28);
        tabPage3.Name = "tabPage3";
        tabPage3.Padding = new Padding(3);
        tabPage3.Size = new Size(606, 159);
        tabPage3.TabIndex = 0;
        tabPage3.Text = "操作";
        tabPage3.ToolTipText = "操作选项卡";
        // 
        // flowLayoutPanel1
        // 
        flowLayoutPanel1.Controls.Add(SLexerButton);
        flowLayoutPanel1.Controls.Add(SRunButton);
        flowLayoutPanel1.Controls.Add(S);
        flowLayoutPanel1.Dock = DockStyle.Fill;
        flowLayoutPanel1.Location = new Point(3, 3);
        flowLayoutPanel1.Name = "flowLayoutPanel1";
        flowLayoutPanel1.Size = new Size(600, 153);
        flowLayoutPanel1.TabIndex = 0;
        // 
        // SLexerButton
        // 
        SLexerButton.Cursor = Cursors.Hand;
        SLexerButton.FlatStyle = FlatStyle.Flat;
        SLexerButton.ForeColor = SystemColors.ButtonFace;
        SLexerButton.Location = new Point(3, 3);
        SLexerButton.Name = "SLexerButton";
        SLexerButton.Size = new Size(81, 32);
        SLexerButton.TabIndex = 0;
        SLexerButton.Text = "开始解析";
        SLexerButton.UseVisualStyleBackColor = true;
        SLexerButton.Click += SLexerButton_Click;
        // 
        // SRunButton
        // 
        SRunButton.Cursor = Cursors.Hand;
        SRunButton.FlatStyle = FlatStyle.Flat;
        SRunButton.ForeColor = SystemColors.Window;
        SRunButton.Location = new Point(90, 3);
        SRunButton.Name = "SRunButton";
        SRunButton.Size = new Size(83, 32);
        SRunButton.TabIndex = 1;
        SRunButton.Text = "开始运行";
        SRunButton.UseVisualStyleBackColor = true;
        SRunButton.Click += SRunButton_Click;
        // 
        // S
        // 
        S.Cursor = Cursors.Hand;
        S.FlatStyle = FlatStyle.Flat;
        S.ForeColor = SystemColors.Window;
        S.Location = new Point(179, 3);
        S.Name = "S";
        S.Size = new Size(66, 32);
        S.TabIndex = 2;
        S.Text = "继续";
        S.UseVisualStyleBackColor = true;
        // 
        // toolTip
        // 
        toolTip.BackColor = Color.FromArgb(31, 31, 31);
        toolTip.ForeColor = SystemColors.ButtonHighlight;
        toolTip.IsBalloon = true;
        // 
        // openFileDialog
        // 
        openFileDialog.DefaultExt = "scn";
        openFileDialog.FileName = "openFileDialog1";
        openFileDialog.Filter = "ScnScript文件|*.scn|所有文件|*.*";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 19F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(46, 46, 46);
        ClientSize = new Size(834, 623);
        Controls.Add(splitContainer1);
        ForeColor = SystemColors.ButtonFace;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ScnScript.Test (测试)";
        Load += Form1_Load;
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        splitContainer2.Panel1.ResumeLayout(false);
        splitContainer2.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
        splitContainer2.ResumeLayout(false);
        groupBox2.ResumeLayout(false);
        groupBox1.ResumeLayout(false);
        splitContainer3.Panel1.ResumeLayout(false);
        splitContainer3.Panel1.PerformLayout();
        splitContainer3.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
        splitContainer3.ResumeLayout(false);
        contextMenuStrip1.ResumeLayout(false);
        tabControl2.ResumeLayout(false);
        tabPage4.ResumeLayout(false);
        tabPage4.PerformLayout();
        tabPage3.ResumeLayout(false);
        flowLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;
    private GroupBox groupBox1;
    private SplitContainer splitContainer3;
    private ToolTip toolTip;
    private TextBox CodeBox;
    private TabControl tabControl2;
    private TabPage tabPage4;
    private TabPage tabPage3;
    private TextBox SOutBox;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button SLexerButton;
    private Button SRunButton;
    private Button S;
    private GroupBox groupBox2;
    private TreeView SCNTreeview;
    private Label SCNOutM;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem 载入文件ToolStripMenuItem;
    private OpenFileDialog openFileDialog;
}
