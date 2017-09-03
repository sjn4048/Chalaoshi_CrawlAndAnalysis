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
    public partial class CrawlShouldKnow : Form
    {
        public CrawlShouldKnow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                var form = new CrawlPage() {StartPosition = FormStartPosition.CenterScreen };
                this.Hide();
                form.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("请同意本须知或返回主菜单。", "错误提示");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void CrawlShouldKnow_Load(object sender, EventArgs e)
        {

        }
    }
}
