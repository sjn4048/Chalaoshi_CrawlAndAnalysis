using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Net;



namespace WebCrawler_WinForm_
{
    public partial class CrawlPage : Form
    {
        public static bool isCrawlerRunning = false;
        Task task;
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public CrawlPage()
        {
            Control.CheckForIllegalCrossThreadCalls = false;//防止线程间操作error
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objPinReply = objPingSender.Send("baidu.com", intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (strInfo != "Success")
                {
                    MessageBox.Show("抱歉，检测到您的网络未连接或不稳定，请稍后再试", "更新失败");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("抱歉，检测到您的网络未连接或不稳定，请稍后再试", "更新失败");
                return;
            }
            //以上为联网监测
            if (isCrawlerRunning == false)
            {
                isCrawlerRunning = true;
                WebCrawlerProcess crawler = new WebCrawlerProcess(this);
                task = new Task(() => crawler.StartWebCrawler(), tokenSource.Token);
                task.Start();
            }
            else
            {
                MessageBox.Show("爬虫已经开始运行","无法开始任务");
            }
        }

        private void CrawlPage_Load(object sender, EventArgs e)
        {
            FinishButton.Enabled = false;
            checkBox1.Checked = checkBox2.Checked = true;
            progressBar1.Maximum = Chalaoshi.MaximumTeacherPage;
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("恭喜你，数据更新已顺利完成", "更新完成");
            this.Controls.Clear();
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CrawlPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isCrawlerRunning)
            {
                if (MessageBox.Show("爬虫正在运行中，停止任务将导致更新失败，是否确认？", "停止任务", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    tokenSource.Cancel();
                    isCrawlerRunning = false;
                    this.Controls.Clear();
                    MessageBox.Show("更新任务被手动停止", "更新失败");
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                this.Controls.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isCrawlerRunning)
            {
                if(MessageBox.Show("爬虫正在运行中，停止任务将导致更新失败，是否确认？", "停止任务", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    tokenSource.Cancel();
                    isCrawlerRunning = false;
                    this.Close();
                    MessageBox.Show("更新任务被手动停止", "更新失败");
                }
            }
            else
            {
                this.Close();
            }
        }
    }
}
