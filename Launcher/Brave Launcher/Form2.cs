using System.Windows.Forms;

namespace Brave_Launcher
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, System.EventArgs e)
        {
            if (radioButton1.Checked)
            {
                System.IO.File.WriteAllText(@"Brave\Profile.txt", "--user-data-dir=\"profile\"");
                this.Close();
            }
            if (radioButton2.Checked)
            {
                System.IO.File.WriteAllText(@"Brave\Profile.txt", "--user-data-dir=\"Brave\\profile\"");
                this.Close();
            }
            if (radioButton3.Checked)
            {
                System.IO.File.WriteAllText(@"Brave\Profile.txt", "");
                this.Close();
            }
        }
    }
}
