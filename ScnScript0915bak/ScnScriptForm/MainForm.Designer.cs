namespace ScnScriptForm
{
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip1 = new MenuStrip();
            文件FToolStripMenuItem = new ToolStripMenuItem();
            打开文件夹ToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            退出ToolStripMenuItem = new ToolStripMenuItem();
            视图FToolStripMenuItem = new ToolStripMenuItem();
            数据查看器ToolStripMenuItem = new ToolStripMenuItem();
            资源管理器ToolStripMenuItem = new ToolStripMenuItem();
            日志记录器ToolStripMenuItem = new ToolStripMenuItem();
            调试DToolStripMenuItem = new ToolStripMenuItem();
            开始调试ToolStripMenuItem = new ToolStripMenuItem();
            folderBrowserDialog1 = new FolderBrowserDialog();
            toolStrip1 = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            toolStripLabel1 = new ToolStripLabel();
            toolStripButton2 = new ToolStripButton();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { 文件FToolStripMenuItem, 视图FToolStripMenuItem, 调试DToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1153, 31);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            文件FToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 打开文件夹ToolStripMenuItem, toolStripMenuItem1, 退出ToolStripMenuItem });
            文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            文件FToolStripMenuItem.Size = new Size(75, 27);
            文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // 打开文件夹ToolStripMenuItem
            // 
            打开文件夹ToolStripMenuItem.Name = "打开文件夹ToolStripMenuItem";
            打开文件夹ToolStripMenuItem.Size = new Size(160, 28);
            打开文件夹ToolStripMenuItem.Text = "打开文件夹";
            打开文件夹ToolStripMenuItem.Click += 打开文件夹ToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(157, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            退出ToolStripMenuItem.Size = new Size(160, 28);
            退出ToolStripMenuItem.Text = "退出";
            // 
            // 视图FToolStripMenuItem
            // 
            视图FToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 数据查看器ToolStripMenuItem, 资源管理器ToolStripMenuItem, 日志记录器ToolStripMenuItem });
            视图FToolStripMenuItem.Name = "视图FToolStripMenuItem";
            视图FToolStripMenuItem.Size = new Size(75, 27);
            视图FToolStripMenuItem.Text = "视图(&F)";
            // 
            // 数据查看器ToolStripMenuItem
            // 
            数据查看器ToolStripMenuItem.Name = "数据查看器ToolStripMenuItem";
            数据查看器ToolStripMenuItem.Size = new Size(160, 28);
            数据查看器ToolStripMenuItem.Text = "数据查看器";
            数据查看器ToolStripMenuItem.Click += 数据查看器ToolStripMenuItem_Click;
            // 
            // 资源管理器ToolStripMenuItem
            // 
            资源管理器ToolStripMenuItem.Name = "资源管理器ToolStripMenuItem";
            资源管理器ToolStripMenuItem.Size = new Size(160, 28);
            资源管理器ToolStripMenuItem.Text = "资源管理器";
            资源管理器ToolStripMenuItem.Click += 资源管理器ToolStripMenuItem_Click;
            // 
            // 日志记录器ToolStripMenuItem
            // 
            日志记录器ToolStripMenuItem.Name = "日志记录器ToolStripMenuItem";
            日志记录器ToolStripMenuItem.Size = new Size(160, 28);
            日志记录器ToolStripMenuItem.Text = "日志记录器";
            日志记录器ToolStripMenuItem.Click += 日志记录器ToolStripMenuItem_Click;
            // 
            // 调试DToolStripMenuItem
            // 
            调试DToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 开始调试ToolStripMenuItem });
            调试DToolStripMenuItem.Name = "调试DToolStripMenuItem";
            调试DToolStripMenuItem.Size = new Size(78, 27);
            调试DToolStripMenuItem.Text = "调试(&D)";
            // 
            // 开始调试ToolStripMenuItem
            // 
            开始调试ToolStripMenuItem.Name = "开始调试ToolStripMenuItem";
            开始调试ToolStripMenuItem.Size = new Size(144, 28);
            开始调试ToolStripMenuItem.Text = "开始调试";
            开始调试ToolStripMenuItem.Click += 开始调试ToolStripMenuItem_Click;
            // 
            // folderBrowserDialog1
            // 
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.CommonDocuments;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripLabel1, toolStripButton2 });
            toolStrip1.Location = new Point(0, 31);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1153, 30);
            toolStrip1.TabIndex = 4;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(94, 27);
            toolStripButton1.Text = "打开文件夹";
            toolStripButton1.Click += 打开文件夹ToolStripMenuItem_Click;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(74, 27);
            toolStripLabel1.Text = "调试目标";
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButton2.Image = (Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(78, 27);
            toolStripButton2.Text = "开始调试";
            toolStripButton2.Click += 开始调试ToolStripMenuItem_Click;
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1153, 683);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "ScnScriptForm - MainForm";
            Load += MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem 文件FToolStripMenuItem;
        private ToolStripMenuItem 打开文件夹ToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem 退出ToolStripMenuItem;
        private ToolStripMenuItem 视图FToolStripMenuItem;
        private ToolStripMenuItem 数据查看器ToolStripMenuItem;
        private ToolStripMenuItem 资源管理器ToolStripMenuItem;
        private ToolStripMenuItem 日志记录器ToolStripMenuItem;
        private FolderBrowserDialog folderBrowserDialog1;
        private ToolStripMenuItem 调试DToolStripMenuItem;
        private ToolStripMenuItem 开始调试ToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripLabel toolStripLabel1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
