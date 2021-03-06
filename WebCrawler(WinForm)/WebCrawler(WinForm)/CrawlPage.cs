﻿using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Net;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

namespace WebCrawler_WinForm_
{
    public partial class CrawlPage : Form
    {
        public static bool isCrawlerRunning = false;

        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken ct;
        public TaskDialogProgressBar taskDialogProgressBar;

        public CrawlPage()
        {
            InitializeComponent();
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ct = cts.Token;
            if (!ToolFunctions.CheckNetworkStatus())
            {
                MessageBox.Show("抱歉，检测到您的网络未连接或不稳定，请稍后再试", "更新失败");
                return;
            }
            //以上为联网监测
            if (isCrawlerRunning == false)
            {
                isCrawlerRunning = true;
                WebCrawlerProcess crawler = new WebCrawlerProcess(this);
                new Task(() => crawler.StartWebCrawler(ct), ct).Start();
                button2.Enabled = true;
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
            taskDialogProgressBar = new TaskDialogProgressBar(0, Chalaoshi.MaximumTeacherPage, 0);
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
                    isCrawlerRunning = false;
                    MessageBox.Show("更新任务被手动停止", "更新失败");
                    this.progressBar1.Value = 0;
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
                    cts.Cancel();
                    isCrawlerRunning = false;
                    MessageBox.Show("更新任务被手动停止", "更新失败");
                    this.Close();//debug
                    button2.Enabled = false;
                }
            }
        }
    }
}
