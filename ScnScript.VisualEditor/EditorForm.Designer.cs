namespace ScnScript.VisualEditor;

partial class EditorForm
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
        var resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
        toolStrip1 = new ToolStrip();
        toolStripLabel1 = new ToolStripLabel();
        toolStripComboBox1 = new ToolStripComboBox();
        statusStrip1 = new StatusStrip();
        toolStripStatusLabel1 = new ToolStripStatusLabel();
        toolStripStatusLabel2 = new ToolStripStatusLabel();
        toolStripStatusLabel3 = new ToolStripStatusLabel();
        panel1 = new Panel();
        textBox1 = new TextBox();
        toolStrip1.SuspendLayout();
        statusStrip1.SuspendLayout();
        panel1.SuspendLayout();
        SuspendLayout();
        // 
        // toolStrip1
        // 
        toolStrip1.BackColor = Color.FromArgb(31, 31, 31);
        toolStrip1.CanOverflow = false;
        toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
        toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, toolStripComboBox1 });
        toolStrip1.Location = new Point(0, 0);
        toolStrip1.Name = "toolStrip1";
        toolStrip1.Size = new Size(375, 27);
        toolStrip1.TabIndex = 0;
        toolStrip1.Text = "toolStrip1";
        // 
        // toolStripLabel1
        // 
        toolStripLabel1.ForeColor = Color.White;
        toolStripLabel1.Name = "toolStripLabel1";
        toolStripLabel1.Size = new Size(61, 24);
        toolStripLabel1.Text = "查看模式";
        // 
        // toolStripComboBox1
        // 
        toolStripComboBox1.BackColor = Color.FromArgb(61, 61, 61);
        toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        toolStripComboBox1.ForeColor = Color.White;
        toolStripComboBox1.Items.AddRange(new object[] { "源代码", "剧本块" });
        toolStripComboBox1.Name = "toolStripComboBox1";
        toolStripComboBox1.Size = new Size(121, 27);
        // 
        // statusStrip1
        // 
        statusStrip1.BackColor = Color.FromArgb(66, 66, 66);
        statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, toolStripStatusLabel2, toolStripStatusLabel3 });
        statusStrip1.Location = new Point(0, 304);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(375, 24);
        statusStrip1.SizingGrip = false;
        statusStrip1.TabIndex = 1;
        statusStrip1.Text = "statusStrip1";
        // 
        // toolStripStatusLabel1
        // 
        toolStripStatusLabel1.ForeColor = Color.White;
        toolStripStatusLabel1.Name = "toolStripStatusLabel1";
        toolStripStatusLabel1.Size = new Size(48, 19);
        toolStripStatusLabel1.Text = "共 0 行";
        // 
        // toolStripStatusLabel2
        // 
        toolStripStatusLabel2.ForeColor = Color.White;
        toolStripStatusLabel2.Name = "toolStripStatusLabel2";
        toolStripStatusLabel2.Size = new Size(103, 19);
        toolStripStatusLabel2.Text = "共 0 字 占用 0KB";
        // 
        // toolStripStatusLabel3
        // 
        toolStripStatusLabel3.ForeColor = Color.White;
        toolStripStatusLabel3.Name = "toolStripStatusLabel3";
        toolStripStatusLabel3.Size = new Size(42, 19);
        toolStripStatusLabel3.Text = "100%";
        toolStripStatusLabel3.Click += toolStripStatusLabel3_Click;
        // 
        // panel1
        // 
        panel1.Controls.Add(textBox1);
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(0, 27);
        panel1.Name = "panel1";
        panel1.Size = new Size(375, 277);
        panel1.TabIndex = 2;
        // 
        // textBox1
        // 
        textBox1.AcceptsReturn = true;
        textBox1.AcceptsTab = true;
        textBox1.BackColor = Color.FromArgb(31, 31, 31);
        textBox1.BorderStyle = BorderStyle.None;
        textBox1.Dock = DockStyle.Fill;
        textBox1.ForeColor = Color.White;
        textBox1.Location = new Point(0, 0);
        textBox1.Multiline = true;
        textBox1.Name = "textBox1";
        textBox1.ScrollBars = ScrollBars.Vertical;
        textBox1.Size = new Size(375, 277);
        textBox1.TabIndex = 0;
        textBox1.WordWrap = false;
        textBox1.TextChanged += textBox1_TextChanged;
        textBox1.KeyDown += textBox1_KeyDown;
        textBox1.KeyUp += textBox1_KeyDown;
        textBox1.MouseDown += textBox1_MouseDown;
        textBox1.MouseUp += textBox1_MouseDown;
        // 
        // EditorForm
        // 
        AutoScaleDimensions = new SizeF(7F, 19F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(375, 328);
        Controls.Add(panel1);
        Controls.Add(statusStrip1);
        Controls.Add(toolStrip1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MinimumSize = new Size(391, 367);
        Name = "EditorForm";
        Text = "代码编辑器";
        Load += EditorForm_Load;
        toolStrip1.ResumeLayout(false);
        toolStrip1.PerformLayout();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ToolStrip toolStrip1;
    private ToolStripLabel toolStripLabel1;
    private ToolStripComboBox toolStripComboBox1;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel toolStripStatusLabel1;
    private ToolStripStatusLabel toolStripStatusLabel2;
    private ToolStripStatusLabel toolStripStatusLabel3;
    private Panel panel1;
    private TextBox textBox1;
}