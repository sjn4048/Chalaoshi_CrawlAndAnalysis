﻿using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using System.Net;



namespace WebCrawler_WinForm_
{
    public partial class CrawlPage : Form
    {
        Task task;
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public CrawlPage()
        {
            Control.CheckForIllegalCrossThreadCalls = false;//防止线程间操作error
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebCrawlerProcess crawler = new WebCrawlerProcess(this);
            task = new Task(()=>crawler.StartWebCrawler(), tokenSource.Token);
            task.Start();
        }

        private void CrawlPage_Load(object sender, EventArgs e)
        {
            FinishButton.Hide();
            checkBox1.Checked = checkBox2.Checked = true;
            progressBar1.Maximum = Chalaoshi.MaximumTeacherPage;
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("恭喜你，数据更新已顺利完成", "更新完成");
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CrawlPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("爬虫正在运行中，停止任务将导致更新失败，是否确认？", "停止任务", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                tokenSource.Cancel();
            }
            this.Close();
            MessageBox.Show("更新任务被手动停止", "更新失败");
        }
    }
}
