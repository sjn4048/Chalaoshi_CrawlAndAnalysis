﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace WebCrawler_WinForm_
{
    public partial class MainForm : Form
    {
        Task task;
        bool isDataFinished = false;

        public MainForm()
        {
            InitializeComponent();
            StartLoadingTask();
            Control.CheckForIllegalCrossThreadCalls = false;
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
            if (TeacherData.totalTeacherList == null && CourseData.courseDataList == null)
            {
                MessageBox.Show("数据库可能已丢失或被篡改，请点击“更新数据”重新获取或联系作者", "数据库丢失");
                return;
            }
            if (!isDataFinished && task.Status == TaskStatus.Running)
            {
                WaitForm waitForm = new WaitForm() { TopMost = true, StartPosition = FormStartPosition.CenterScreen };
                waitForm.Show();
                task.Wait();
                waitForm.Close();
            }
            else if (!isDataFinished && task.Status == TaskStatus.RanToCompletion)
            {
                StartLoadingTask();
            }
            new BillboardForm
            {
                TopMost = true,
                StartPosition = FormStartPosition.CenterScreen
            }
            .Show();
        }

        private void SearchBox_Click(object sender, EventArgs e)
        {
            if (TeacherData.totalTeacherList == null && CourseData.courseDataList == null)
            {
                MessageBox.Show("数据库可能已丢失或被篡改，请点击“更新数据”重新获取或联系作者","数据库丢失");
                return;
            }
            if (!isDataFinished && task.Status == TaskStatus.Running)
            {
                WaitForm waitForm = new WaitForm() { TopMost = true, StartPosition = FormStartPosition.CenterScreen };
                waitForm.Show();
                task.Wait();
                waitForm.Close();
            }
            else if (!isDataFinished && task.Status == TaskStatus.RanToCompletion)
            {
                StartLoadingTask();
            }
            var form = new SearchForm() { StartPosition = FormStartPosition.CenterScreen, TopMost = true };
            form.Show();
        }

        private void CrawlBox_Click(object sender, EventArgs e)
        {
            if (DateTime.Now.Month == 7 && DateTime.Now.Day >= 28 && DateTime.Now.Day <= 30)
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

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void StartLoadingTask()
        {
            task = new Task(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    FileStream file = new FileStream("CLSDatabase.csv", FileMode.Open, FileAccess.Read);
                    var getData = new GetData();
                    toolStripStatusLabel1.Text = "正在载入数据库中...";
                    getData.GetTeacherDataFromCsv(file);
                    isDataFinished = true;
                    stopwatch.Stop();
                    toolStripStatusLabel1.Text = $"数据库载入完成，用时{(stopwatch.ElapsedMilliseconds / 1000.0):F2}秒";
                }
                catch
                {
                    toolStripStatusLabel1.Text = "未发现数据库，请点击“更新数据”获取数据库";
                }
            });
            task.Start();
        }
    }
}
