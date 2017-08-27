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
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            MailLabel.Links.Add(0, MailLabel.Text.Length, "mailto:3160105216@zju.edu.cn");
        }

        private void NameLabel_Click(object sender, EventArgs e)
        {

        }

        private void AboutForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SettingForm form = new SettingForm() { StartPosition = FormStartPosition.CenterScreen, TopMost = true };
            form.Show();
        }

        private void MailLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:3160105216@zju.edu.cn");
        }
    }
}
