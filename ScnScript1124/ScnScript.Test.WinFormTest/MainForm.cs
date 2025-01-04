using ScnScript.CodeAnalysis;

namespace ScnScript.Test.WinFormTest
{
    public partial class MainForm : Form
    {
        public BehaviorTree Tree { get; set; } = new BehaviorTree();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
