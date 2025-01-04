namespace ScnScript.VisualEditor;

partial class EditorParagraph
{
    /// <summary> 
    /// 必需的设计器变量。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// 清理所有正在使用的资源。
    /// </summary>
    /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region 组件设计器生成的代码

    /// <summary> 
    /// 设计器支持所需的方法 - 不要修改
    /// 使用代码编辑器修改此方法的内容。
    /// </summary>
    private void InitializeComponent()
    {
        splitContainer1 = new SplitContainer();
        label1 = new Label();
        panel1 = new Panel();
        textBox1 = new TextBox();
        pictureBox1 = new PictureBox();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        splitContainer1.Orientation = Orientation.Horizontal;
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(pictureBox1);
        splitContainer1.Panel1.Controls.Add(label1);
        splitContainer1.Panel1.Controls.Add(panel1);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(textBox1);
        splitContainer1.Size = new Size(248, 147);
        splitContainer1.SplitterDistance = 40;
        splitContainer1.TabIndex = 0;
        // 
        // label1
        // 
        label1.BackColor = Color.Transparent;
        label1.Font = new Font("Resource Han Rounded CN Medium", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 134);
        label1.Location = new Point(12, 2);
        label1.Name = "label1";
        label1.Size = new Size(124, 38);
        label1.TabIndex = 1;
        label1.Text = "编辑段落";
        label1.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // panel1
        // 
        panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        panel1.BackColor = Color.FromArgb(255, 128, 128);
        panel1.Location = new Point(0, 6);
        panel1.Margin = new Padding(0, 5, 0, 5);
        panel1.Name = "panel1";
        panel1.Size = new Size(9, 28);
        panel1.TabIndex = 0;
        // 
        // textBox1
        // 
        textBox1.AcceptsReturn = true;
        textBox1.Font = new Font("Resource Han Rounded CN", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
        textBox1.Location = new Point(7, 8);
        textBox1.Margin = new Padding(7, 8, 7, 8);
        textBox1.Multiline = true;
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(234, 87);
        textBox1.TabIndex = 0;
        // 
        // pictureBox1
        // 
        pictureBox1.Location = new Point(216, 8);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(25, 25);
        pictureBox1.TabIndex = 2;
        pictureBox1.TabStop = false;
        // 
        // EditorParagraph
        // 
        AutoScaleDimensions = new SizeF(7F, 19F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        Controls.Add(splitContainer1);
        Name = "EditorParagraph";
        Size = new Size(248, 147);
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        splitContainer1.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    private Label label1;
    private Panel panel1;
    private TextBox textBox1;
    private PictureBox pictureBox1;
}
