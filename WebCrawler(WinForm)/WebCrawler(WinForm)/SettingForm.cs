using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebCrawler_WinForm_
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm() { StartPosition = FormStartPosition.CenterScreen, TopMost = true };
            form.Show();
            this.Close();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            new SetConfigForm() { StartPosition = FormStartPosition.CenterScreen, TopMost = true }.Show();
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("说明文档正在制作中，将在下个版本上线", "Oops...");
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = SystemColors.ActiveCaption;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = SystemColors.Control;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackColor = SystemColors.ActiveCaption;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = SystemColors.Control;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackColor = SystemColors.ActiveCaption;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = SystemColors.Control;
        }
    }
}
