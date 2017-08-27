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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void 帮助HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StillUnderConstruction();
        }

        public void StillUnderConstruction()
        {
            MessageBox.Show("这个功能正在紧张施工中，要不先试试别的用能~?");
        }

        private void 关于本软件AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.StartPosition = FormStartPosition.CenterParent;
            aboutForm.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void UpdateLabel_Click(object sender, EventArgs e)
        {

        }

        private void DbBox_Click(object sender, EventArgs e)
        {

        }

        private void SearchBox_Click(object sender, EventArgs e)
        {
            var form = new SearchForm() { StartPosition = FormStartPosition.CenterScreen, TopMost = true };
            form.Show();
        }

        private void CrawlBox_Click(object sender, EventArgs e)
        {
            if (DateTime.Now.Month == 8 && DateTime.Now.Day >= 28 && DateTime.Now.Day <= 30)
            {
                var form = new UpdateFailedDueToTime() { TopMost = true, StartPosition = FormStartPosition.CenterScreen };
                form.ShowDialog();
            }
            else
            {
                var form = new CrawlShouldKnow() {StartPosition = FormStartPosition.CenterScreen, TopMost = true};
                form.Show();
            }
        }

        private void SettingBox_Click(object sender, EventArgs e)
        {
            var form = new SettingForm();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
        }
    }
}
