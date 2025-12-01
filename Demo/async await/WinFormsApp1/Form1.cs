using System.Threading.Tasks;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var s = await File.ReadAllTextAsync(@"E:\Test\12.2.txt");
            MessageBox.Show(s.Substring(0,20));
        }
    }
}
