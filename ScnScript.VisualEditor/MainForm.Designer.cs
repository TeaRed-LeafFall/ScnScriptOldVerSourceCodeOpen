namespace ScnScript.VisualEditor;

partial class MainForm
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
        var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        toolStrip1 = new ToolStrip();
        toolStripButton1 = new ToolStripButton();
        toolStripSeparator2 = new ToolStripSeparator();
        toolStripComboBox1 = new ToolStripComboBox();
        toolStripSeparator3 = new ToolStripSeparator();
        toolStripButton2 = new ToolStripButton();
        toolStripSeparator1 = new ToolStripSeparator();
        toolStripButton3 = new ToolStripButton();
        toolStripButton4 = new ToolStripButton();
        toolStripSeparator4 = new ToolStripSeparator();
        toolStripButton5 = new ToolStripButton();
        toolStripButton6 = new ToolStripButton();
        toolStripButton7 = new ToolStripButton();
        toolStripSeparator5 = new ToolStripSeparator();
        toolStripButton9 = new ToolStripButton();
        toolStripSeparator6 = new ToolStripSeparator();
        menuStrip1 = new MenuStrip();
        文件FToolStripMenuItem = new ToolStripMenuItem();
        新建ToolStripMenuItem = new ToolStripMenuItem();
        项目PToolStripMenuItem = new ToolStripMenuItem();
        文件FToolStripMenuItem1 = new ToolStripMenuItem();
        打开ToolStripMenuItem = new ToolStripMenuItem();
        项目PToolStripMenuItem1 = new ToolStripMenuItem();
        文件夹DToolStripMenuItem = new ToolStripMenuItem();
        文件FToolStripMenuItem2 = new ToolStripMenuItem();
        toolStripMenuItem7 = new ToolStripSeparator();
        保存SToolStripMenuItem = new ToolStripMenuItem();
        全部保存AToolStripMenuItem = new ToolStripMenuItem();
        打开项目目录ToolStripMenuItem = new ToolStripMenuItem();
        默认方式打开此文件ToolStripMenuItem = new ToolStripMenuItem();
        重新关联格式ToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem1 = new ToolStripSeparator();
        退出XToolStripMenuItem = new ToolStripMenuItem();
        视图VToolStripMenuItem = new ToolStripMenuItem();
        代码CToolStripMenuItem = new ToolStripMenuItem();
        项目资源管理器ToolStripMenuItem = new ToolStripMenuItem();
        项目PToolStripMenuItem2 = new ToolStripMenuItem();
        添加项ToolStripMenuItem = new ToolStripMenuItem();
        添加现有项ToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem2 = new ToolStripSeparator();
        新建文件夹ToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem3 = new ToolStripSeparator();
        生成ToolStripMenuItem = new ToolStripMenuItem();
        发布ToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem4 = new ToolStripSeparator();
        测试运行ToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem6 = new ToolStripSeparator();
        项目属性ToolStripMenuItem = new ToolStripMenuItem();
        调试ToolStripMenuItem = new ToolStripMenuItem();
        开始调试ToolStripMenuItem = new ToolStripMenuItem();
        开始执行不调试ToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem8 = new ToolStripSeparator();
        调试属性ToolStripMenuItem = new ToolStripMenuItem();
        窗口WToolStripMenuItem = new ToolStripMenuItem();
        层叠放置CToolStripMenuItem = new ToolStripMenuItem();
        垂直平铺VToolStripMenuItem = new ToolStripMenuItem();
        水平平铺HToolStripMenuItem = new ToolStripMenuItem();
        全部关闭LToolStripMenuItem = new ToolStripMenuItem();
        帮助ToolStripMenuItem = new ToolStripMenuItem();
        关于ToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem5 = new ToolStripSeparator();
        首选项ToolStripMenuItem = new ToolStripMenuItem();
        openFileDialog1 = new OpenFileDialog();
        saveFileDialog1 = new SaveFileDialog();
        folderBrowserDialog1 = new FolderBrowserDialog();
        toolStrip1.SuspendLayout();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // toolStrip1
        // 
        toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripSeparator2, toolStripComboBox1, toolStripSeparator3, toolStripButton2, toolStripSeparator1, toolStripButton3, toolStripButton4, toolStripSeparator4, toolStripButton5, toolStripButton6, toolStripButton7, toolStripSeparator5, toolStripButton9, toolStripSeparator6 });
        toolStrip1.Location = new Point(0, 27);
        toolStrip1.Name = "toolStrip1";
        toolStrip1.Size = new Size(700, 27);
        toolStrip1.TabIndex = 1;
        toolStrip1.Text = "toolStrip1";
        // 
        // toolStripButton1
        // 
        toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
        toolStripButton1.Image = Properties.Resources.file;
        toolStripButton1.ImageTransparentColor = Color.Magenta;
        toolStripButton1.Name = "toolStripButton1";
        toolStripButton1.Size = new Size(23, 24);
        toolStripButton1.Text = "创建文件";
        toolStripButton1.ToolTipText = "创建文件";
        toolStripButton1.Click += toolStripButton1_Click;
        // 
        // toolStripSeparator2
        // 
        toolStripSeparator2.Name = "toolStripSeparator2";
        toolStripSeparator2.Size = new Size(6, 27);
        // 
        // toolStripComboBox1
        // 
        toolStripComboBox1.Items.AddRange(new object[] { "v0.1" });
        toolStripComboBox1.Name = "toolStripComboBox1";
        toolStripComboBox1.Size = new Size(121, 27);
        toolStripComboBox1.Text = "v0.1";
        toolStripComboBox1.ToolTipText = "语言版本号";
        // 
        // toolStripSeparator3
        // 
        toolStripSeparator3.Name = "toolStripSeparator3";
        toolStripSeparator3.Size = new Size(6, 27);
        // 
        // toolStripButton2
        // 
        toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
        toolStripButton2.Image = Properties.Resources.exec;
        toolStripButton2.ImageTransparentColor = Color.Magenta;
        toolStripButton2.Name = "toolStripButton2";
        toolStripButton2.Size = new Size(23, 24);
        toolStripButton2.Text = "开始运行/继续执行";
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(6, 27);
        // 
        // toolStripButton3
        // 
        toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
        toolStripButton3.Image = Properties.Resources._break;
        toolStripButton3.ImageTransparentColor = Color.Magenta;
        toolStripButton3.Name = "toolStripButton3";
        toolStripButton3.Size = new Size(23, 24);
        toolStripButton3.Text = "暂停";
        // 
        // toolStripButton4
        // 
        toolStripButton4.DisplayStyle = ToolStripItemDisplayStyle.Image;
        toolStripButton4.Image = Properties.Resources.kill;
        toolStripButton4.ImageTransparentColor = Color.Magenta;
        toolStripButton4.Name = "toolStripButton4";
        toolStripButton4.Size = new Size(23, 24);
        toolStripButton4.Text = "终止";
        // 
        // toolStripSeparator4
        // 
        toolStripSeparator4.Name = "toolStripSeparator4";
        toolStripSeparator4.Size = new Size(6, 27);
        // 
        // toolStripButton5
        // 
        toolStripButton5.DisplayStyle = ToolStripItemDisplayStyle.Image;
        toolStripButton5.Image = Properties.Resources._return;
        toolStripButton5.ImageTransparentColor = Color.Magenta;
        toolStripButton5.Name = "toolStripButton5";
        toolStripButton5.Size = new Size(23, 24);
        toolStripButton5.Text = "下一条语句";
        // 
        // toolStripButton6
        // 
        toolStripButton6.DisplayStyle = ToolStripItemDisplayStyle.Image;
        toolStripButton6.Image = Properties.Resources.step;
        toolStripButton6.ImageTransparentColor = Color.Magenta;
        toolStripButton6.Name = "toolStripButton6";
        toolStripButton6.Size = new Size(23, 24);
        toolStripButton6.Text = "逐语句";
        // 
        // toolStripButton7
        // 
        toolStripButton7.DisplayStyle = ToolStripItemDisplayStyle.Image;
        toolStripButton7.Image = Properties.Resources.trace;
        toolStripButton7.ImageTransparentColor = Color.Magenta;
        toolStripButton7.Name = "toolStripButton7";
        toolStripButton7.Size = new Size(23, 24);
        toolStripButton7.Text = "跳转到此";
        // 
        // toolStripSeparator5
        // 
        toolStripSeparator5.Name = "toolStripSeparator5";
        toolStripSeparator5.Size = new Size(6, 27);
        // 
        // toolStripButton9
        // 
        toolStripButton9.DisplayStyle = ToolStripItemDisplayStyle.Image;
        toolStripButton9.Image = Properties.Resources._object;
        toolStripButton9.ImageTransparentColor = Color.Magenta;
        toolStripButton9.Name = "toolStripButton9";
        toolStripButton9.Size = new Size(23, 24);
        toolStripButton9.Text = "对象查看器";
        // 
        // toolStripSeparator6
        // 
        toolStripSeparator6.Name = "toolStripSeparator6";
        toolStripSeparator6.Size = new Size(6, 27);
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { 文件FToolStripMenuItem, 视图VToolStripMenuItem, 项目PToolStripMenuItem2, 调试ToolStripMenuItem, 窗口WToolStripMenuItem, 帮助ToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.MdiWindowListItem = 窗口WToolStripMenuItem;
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(700, 27);
        menuStrip1.TabIndex = 3;
        menuStrip1.Text = "menuStrip1";
        // 
        // 文件FToolStripMenuItem
        // 
        文件FToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 新建ToolStripMenuItem, 打开ToolStripMenuItem, toolStripMenuItem7, 保存SToolStripMenuItem, 全部保存AToolStripMenuItem, 打开项目目录ToolStripMenuItem, 默认方式打开此文件ToolStripMenuItem, 重新关联格式ToolStripMenuItem, toolStripMenuItem1, 退出XToolStripMenuItem });
        文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
        文件FToolStripMenuItem.Size = new Size(62, 23);
        文件FToolStripMenuItem.Text = "文件(&F)";
        // 
        // 新建ToolStripMenuItem
        // 
        新建ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 项目PToolStripMenuItem, 文件FToolStripMenuItem1 });
        新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
        新建ToolStripMenuItem.Size = new Size(226, 24);
        新建ToolStripMenuItem.Text = "新建(&N)";
        // 
        // 项目PToolStripMenuItem
        // 
        项目PToolStripMenuItem.Name = "项目PToolStripMenuItem";
        项目PToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.N;
        项目PToolStripMenuItem.Size = new Size(201, 24);
        项目PToolStripMenuItem.Text = "项目(&P)";
        // 
        // 文件FToolStripMenuItem1
        // 
        文件FToolStripMenuItem1.Name = "文件FToolStripMenuItem1";
        文件FToolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.N;
        文件FToolStripMenuItem1.Size = new Size(201, 24);
        文件FToolStripMenuItem1.Text = "文件(&F)";
        文件FToolStripMenuItem1.Click += 文件FToolStripMenuItem1_Click;
        // 
        // 打开ToolStripMenuItem
        // 
        打开ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 项目PToolStripMenuItem1, 文件夹DToolStripMenuItem, 文件FToolStripMenuItem2 });
        打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
        打开ToolStripMenuItem.Size = new Size(226, 24);
        打开ToolStripMenuItem.Text = "打开(&O)";
        // 
        // 项目PToolStripMenuItem1
        // 
        项目PToolStripMenuItem1.Name = "项目PToolStripMenuItem1";
        项目PToolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.Shift | Keys.O;
        项目PToolStripMenuItem1.Size = new Size(238, 24);
        项目PToolStripMenuItem1.Text = "项目(&P)";
        // 
        // 文件夹DToolStripMenuItem
        // 
        文件夹DToolStripMenuItem.Name = "文件夹DToolStripMenuItem";
        文件夹DToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Alt | Keys.Shift | Keys.O;
        文件夹DToolStripMenuItem.Size = new Size(238, 24);
        文件夹DToolStripMenuItem.Text = "文件夹(&D)";
        // 
        // 文件FToolStripMenuItem2
        // 
        文件FToolStripMenuItem2.Name = "文件FToolStripMenuItem2";
        文件FToolStripMenuItem2.ShortcutKeys = Keys.Control | Keys.O;
        文件FToolStripMenuItem2.Size = new Size(238, 24);
        文件FToolStripMenuItem2.Text = "文件(&F)";
        // 
        // toolStripMenuItem7
        // 
        toolStripMenuItem7.Name = "toolStripMenuItem7";
        toolStripMenuItem7.Size = new Size(223, 6);
        // 
        // 保存SToolStripMenuItem
        // 
        保存SToolStripMenuItem.Name = "保存SToolStripMenuItem";
        保存SToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
        保存SToolStripMenuItem.Size = new Size(226, 24);
        保存SToolStripMenuItem.Text = "保存(&S)";
        // 
        // 全部保存AToolStripMenuItem
        // 
        全部保存AToolStripMenuItem.Name = "全部保存AToolStripMenuItem";
        全部保存AToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
        全部保存AToolStripMenuItem.Size = new Size(226, 24);
        全部保存AToolStripMenuItem.Text = "全部保存(&A)";
        // 
        // 打开项目目录ToolStripMenuItem
        // 
        打开项目目录ToolStripMenuItem.Enabled = false;
        打开项目目录ToolStripMenuItem.Name = "打开项目目录ToolStripMenuItem";
        打开项目目录ToolStripMenuItem.Size = new Size(226, 24);
        打开项目目录ToolStripMenuItem.Text = "打开项目目录";
        // 
        // 默认方式打开此文件ToolStripMenuItem
        // 
        默认方式打开此文件ToolStripMenuItem.Enabled = false;
        默认方式打开此文件ToolStripMenuItem.Name = "默认方式打开此文件ToolStripMenuItem";
        默认方式打开此文件ToolStripMenuItem.Size = new Size(226, 24);
        默认方式打开此文件ToolStripMenuItem.Text = "默认方式打开此文件";
        // 
        // 重新关联格式ToolStripMenuItem
        // 
        重新关联格式ToolStripMenuItem.Name = "重新关联格式ToolStripMenuItem";
        重新关联格式ToolStripMenuItem.Size = new Size(226, 24);
        重新关联格式ToolStripMenuItem.Text = "重新关联格式";
        // 
        // toolStripMenuItem1
        // 
        toolStripMenuItem1.Name = "toolStripMenuItem1";
        toolStripMenuItem1.Size = new Size(223, 6);
        // 
        // 退出XToolStripMenuItem
        // 
        退出XToolStripMenuItem.Name = "退出XToolStripMenuItem";
        退出XToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
        退出XToolStripMenuItem.Size = new Size(226, 24);
        退出XToolStripMenuItem.Text = "退出(&X)";
        退出XToolStripMenuItem.Click += 退出XToolStripMenuItem_Click;
        // 
        // 视图VToolStripMenuItem
        // 
        视图VToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 代码CToolStripMenuItem, 项目资源管理器ToolStripMenuItem });
        视图VToolStripMenuItem.Name = "视图VToolStripMenuItem";
        视图VToolStripMenuItem.Size = new Size(62, 23);
        视图VToolStripMenuItem.Text = "视图(&V)";
        // 
        // 代码CToolStripMenuItem
        // 
        代码CToolStripMenuItem.Name = "代码CToolStripMenuItem";
        代码CToolStripMenuItem.ShortcutKeys = Keys.F7;
        代码CToolStripMenuItem.Size = new Size(252, 24);
        代码CToolStripMenuItem.Text = "新建代码窗口(&C)";
        代码CToolStripMenuItem.Click += 代码CToolStripMenuItem_Click;
        // 
        // 项目资源管理器ToolStripMenuItem
        // 
        项目资源管理器ToolStripMenuItem.Name = "项目资源管理器ToolStripMenuItem";
        项目资源管理器ToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Alt | Keys.L;
        项目资源管理器ToolStripMenuItem.Size = new Size(252, 24);
        项目资源管理器ToolStripMenuItem.Text = "项目资源管理器(&P)";
        项目资源管理器ToolStripMenuItem.Click += 项目资源管理器ToolStripMenuItem_Click;
        // 
        // 项目PToolStripMenuItem2
        // 
        项目PToolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] { 添加项ToolStripMenuItem, 添加现有项ToolStripMenuItem, toolStripMenuItem2, 新建文件夹ToolStripMenuItem, toolStripMenuItem3, 生成ToolStripMenuItem, 发布ToolStripMenuItem, toolStripMenuItem4, 测试运行ToolStripMenuItem, toolStripMenuItem6, 项目属性ToolStripMenuItem });
        项目PToolStripMenuItem2.Name = "项目PToolStripMenuItem2";
        项目PToolStripMenuItem2.Size = new Size(63, 23);
        项目PToolStripMenuItem2.Text = "项目(&P)";
        // 
        // 添加项ToolStripMenuItem
        // 
        添加项ToolStripMenuItem.Name = "添加项ToolStripMenuItem";
        添加项ToolStripMenuItem.Size = new Size(160, 24);
        添加项ToolStripMenuItem.Text = "添加新项(&W)";
        // 
        // 添加现有项ToolStripMenuItem
        // 
        添加现有项ToolStripMenuItem.Name = "添加现有项ToolStripMenuItem";
        添加现有项ToolStripMenuItem.Size = new Size(160, 24);
        添加现有项ToolStripMenuItem.Text = "添加现有项(&G)";
        // 
        // toolStripMenuItem2
        // 
        toolStripMenuItem2.Name = "toolStripMenuItem2";
        toolStripMenuItem2.Size = new Size(157, 6);
        // 
        // 新建文件夹ToolStripMenuItem
        // 
        新建文件夹ToolStripMenuItem.Name = "新建文件夹ToolStripMenuItem";
        新建文件夹ToolStripMenuItem.Size = new Size(160, 24);
        新建文件夹ToolStripMenuItem.Text = "新建文件夹(&D)";
        // 
        // toolStripMenuItem3
        // 
        toolStripMenuItem3.Name = "toolStripMenuItem3";
        toolStripMenuItem3.Size = new Size(157, 6);
        // 
        // 生成ToolStripMenuItem
        // 
        生成ToolStripMenuItem.Name = "生成ToolStripMenuItem";
        生成ToolStripMenuItem.Size = new Size(160, 24);
        生成ToolStripMenuItem.Text = "生成(&B)";
        // 
        // 发布ToolStripMenuItem
        // 
        发布ToolStripMenuItem.Name = "发布ToolStripMenuItem";
        发布ToolStripMenuItem.Size = new Size(160, 24);
        发布ToolStripMenuItem.Text = "发布(&P)";
        // 
        // toolStripMenuItem4
        // 
        toolStripMenuItem4.Name = "toolStripMenuItem4";
        toolStripMenuItem4.Size = new Size(157, 6);
        // 
        // 测试运行ToolStripMenuItem
        // 
        测试运行ToolStripMenuItem.Image = Properties.Resources.exec;
        测试运行ToolStripMenuItem.Name = "测试运行ToolStripMenuItem";
        测试运行ToolStripMenuItem.Size = new Size(160, 24);
        测试运行ToolStripMenuItem.Text = "测试运行(&T)";
        // 
        // toolStripMenuItem6
        // 
        toolStripMenuItem6.Name = "toolStripMenuItem6";
        toolStripMenuItem6.Size = new Size(157, 6);
        // 
        // 项目属性ToolStripMenuItem
        // 
        项目属性ToolStripMenuItem.Name = "项目属性ToolStripMenuItem";
        项目属性ToolStripMenuItem.Size = new Size(160, 24);
        项目属性ToolStripMenuItem.Text = "项目属性(&P)";
        // 
        // 调试ToolStripMenuItem
        // 
        调试ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 开始调试ToolStripMenuItem, 开始执行不调试ToolStripMenuItem, toolStripMenuItem8, 调试属性ToolStripMenuItem });
        调试ToolStripMenuItem.Name = "调试ToolStripMenuItem";
        调试ToolStripMenuItem.Size = new Size(64, 23);
        调试ToolStripMenuItem.Text = "调试(&D)";
        // 
        // 开始调试ToolStripMenuItem
        // 
        开始调试ToolStripMenuItem.Image = Properties.Resources.exec;
        开始调试ToolStripMenuItem.Name = "开始调试ToolStripMenuItem";
        开始调试ToolStripMenuItem.Size = new Size(177, 24);
        开始调试ToolStripMenuItem.Text = "开始调试";
        // 
        // 开始执行不调试ToolStripMenuItem
        // 
        开始执行不调试ToolStripMenuItem.Name = "开始执行不调试ToolStripMenuItem";
        开始执行不调试ToolStripMenuItem.Size = new Size(177, 24);
        开始执行不调试ToolStripMenuItem.Text = "开始执行(不调试)";
        // 
        // toolStripMenuItem8
        // 
        toolStripMenuItem8.Name = "toolStripMenuItem8";
        toolStripMenuItem8.Size = new Size(174, 6);
        // 
        // 调试属性ToolStripMenuItem
        // 
        调试属性ToolStripMenuItem.Name = "调试属性ToolStripMenuItem";
        调试属性ToolStripMenuItem.Size = new Size(177, 24);
        调试属性ToolStripMenuItem.Text = "调试属性";
        // 
        // 窗口WToolStripMenuItem
        // 
        窗口WToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 层叠放置CToolStripMenuItem, 垂直平铺VToolStripMenuItem, 水平平铺HToolStripMenuItem, 全部关闭LToolStripMenuItem });
        窗口WToolStripMenuItem.Name = "窗口WToolStripMenuItem";
        窗口WToolStripMenuItem.Size = new Size(66, 23);
        窗口WToolStripMenuItem.Text = "窗口(&W)";
        // 
        // 层叠放置CToolStripMenuItem
        // 
        层叠放置CToolStripMenuItem.Name = "层叠放置CToolStripMenuItem";
        层叠放置CToolStripMenuItem.Size = new Size(147, 24);
        层叠放置CToolStripMenuItem.Text = "层叠放置(&C)";
        层叠放置CToolStripMenuItem.Click += 层叠CToolStripMenuItem_Click;
        // 
        // 垂直平铺VToolStripMenuItem
        // 
        垂直平铺VToolStripMenuItem.Name = "垂直平铺VToolStripMenuItem";
        垂直平铺VToolStripMenuItem.Size = new Size(147, 24);
        垂直平铺VToolStripMenuItem.Text = "垂直平铺(&V)";
        垂直平铺VToolStripMenuItem.Click += 垂直平铺VToolStripMenuItem_Click;
        // 
        // 水平平铺HToolStripMenuItem
        // 
        水平平铺HToolStripMenuItem.Name = "水平平铺HToolStripMenuItem";
        水平平铺HToolStripMenuItem.Size = new Size(147, 24);
        水平平铺HToolStripMenuItem.Text = "水平平铺(&H)";
        水平平铺HToolStripMenuItem.Click += 水平平铺HToolStripMenuItem_Click;
        // 
        // 全部关闭LToolStripMenuItem
        // 
        全部关闭LToolStripMenuItem.Name = "全部关闭LToolStripMenuItem";
        全部关闭LToolStripMenuItem.Size = new Size(147, 24);
        全部关闭LToolStripMenuItem.Text = "全部关闭(&L)";
        全部关闭LToolStripMenuItem.Click += 全部关闭LToolStripMenuItem_Click;
        // 
        // 帮助ToolStripMenuItem
        // 
        帮助ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 关于ToolStripMenuItem, toolStripMenuItem5, 首选项ToolStripMenuItem });
        帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
        帮助ToolStripMenuItem.Size = new Size(64, 23);
        帮助ToolStripMenuItem.Text = "帮助(&H)";
        // 
        // 关于ToolStripMenuItem
        // 
        关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
        关于ToolStripMenuItem.Size = new Size(133, 24);
        关于ToolStripMenuItem.Text = "关于(&A)";
        // 
        // toolStripMenuItem5
        // 
        toolStripMenuItem5.Name = "toolStripMenuItem5";
        toolStripMenuItem5.Size = new Size(130, 6);
        // 
        // 首选项ToolStripMenuItem
        // 
        首选项ToolStripMenuItem.Name = "首选项ToolStripMenuItem";
        首选项ToolStripMenuItem.Size = new Size(133, 24);
        首选项ToolStripMenuItem.Text = "首选项(&P)";
        // 
        // openFileDialog1
        // 
        openFileDialog1.FileName = "openFileDialog1";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 19F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(700, 450);
        Controls.Add(toolStrip1);
        Controls.Add(menuStrip1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        IsMdiContainer = true;
        MainMenuStrip = menuStrip1;
        MinimumSize = new Size(716, 489);
        Name = "MainForm";
        Text = "Visual ScnScript Editor";
        MdiChildActivate += MainForm_MdiChildActivate;
        toolStrip1.ResumeLayout(false);
        toolStrip1.PerformLayout();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private ToolStrip toolStrip1;
    private ToolStripButton toolStripButton1;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripComboBox toolStripComboBox1;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripButton toolStripButton2;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton toolStripButton3;
    private ToolStripButton toolStripButton4;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripButton toolStripButton5;
    private ToolStripButton toolStripButton6;
    private ToolStripButton toolStripButton7;
    private ToolStripSeparator toolStripSeparator5;
    private ToolStripButton toolStripButton9;
    private ToolStripSeparator toolStripSeparator6;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem 文件FToolStripMenuItem;
    private ToolStripMenuItem 新建ToolStripMenuItem;
    private ToolStripMenuItem 项目PToolStripMenuItem;
    private ToolStripMenuItem 文件FToolStripMenuItem1;
    private ToolStripMenuItem 打开ToolStripMenuItem;
    private ToolStripMenuItem 项目PToolStripMenuItem1;
    private ToolStripMenuItem 文件夹DToolStripMenuItem;
    private ToolStripMenuItem 文件FToolStripMenuItem2;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem 退出XToolStripMenuItem;
    private ToolStripMenuItem 视图VToolStripMenuItem;
    private ToolStripMenuItem 代码CToolStripMenuItem;
    private ToolStripMenuItem 项目资源管理器ToolStripMenuItem;
    private ToolStripMenuItem 项目PToolStripMenuItem2;
    private ToolStripMenuItem 添加项ToolStripMenuItem;
    private ToolStripMenuItem 新建文件夹ToolStripMenuItem;
    private ToolStripMenuItem 添加现有项ToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripSeparator toolStripMenuItem3;
    private ToolStripMenuItem 生成ToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem4;
    private ToolStripMenuItem 帮助ToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem7;
    private ToolStripMenuItem 打开项目目录ToolStripMenuItem;
    private ToolStripMenuItem 默认方式打开此文件ToolStripMenuItem;
    private ToolStripMenuItem 重新关联格式ToolStripMenuItem;
    private ToolStripMenuItem 发布ToolStripMenuItem;
    private ToolStripMenuItem 测试运行ToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem6;
    private ToolStripMenuItem 项目属性ToolStripMenuItem;
    private ToolStripMenuItem 调试ToolStripMenuItem;
    private ToolStripMenuItem 关于ToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem5;
    private ToolStripMenuItem 首选项ToolStripMenuItem;
    private ToolStripMenuItem 开始调试ToolStripMenuItem;
    private ToolStripMenuItem 开始执行不调试ToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem8;
    private ToolStripMenuItem 调试属性ToolStripMenuItem;
    private ToolStripMenuItem 窗口WToolStripMenuItem;
    private ToolStripMenuItem 层叠放置CToolStripMenuItem;
    private ToolStripMenuItem 垂直平铺VToolStripMenuItem;
    private ToolStripMenuItem 水平平铺HToolStripMenuItem;
    private ToolStripMenuItem 全部关闭LToolStripMenuItem;
    private OpenFileDialog openFileDialog1;
    private SaveFileDialog saveFileDialog1;
    private FolderBrowserDialog folderBrowserDialog1;
    private ToolStripMenuItem 保存SToolStripMenuItem;
    private ToolStripMenuItem 全部保存AToolStripMenuItem;
}
