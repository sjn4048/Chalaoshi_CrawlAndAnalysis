using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using System.Net;
using Abot.Core;
using Abot.Crawler;
using Abot.Poco;
using CsQuery.HtmlParser;
using log4net.Config;



namespace WebCrawler_WinForm_
{
    public partial class CrawlPage : Form
    {
        public CrawlPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebCrawlerProcess crawler = new WebCrawlerProcess();
            crawler.StartWebCrawler();
        }

        private void CrawlPage_Load(object sender, EventArgs e)
        {
            FinishButton.Hide();
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {
            
        }
    }

    static class Chalaoshi
    {
        public static int MaximumTeacherPage = 3743;
        public static Regex TeacherDataRegex = new Regex("^https://chalaoshi.cn/teacher/\\d+/");
    }

    class WebCrawlerProcess
    {
        static int CrawledPageCount = 0;//已爬的页面数

        static int FailedPageCount = 0;//用于debug
        public List<string> PageLinkList = new List<string>();
        public List<string> LeftLinkList = new List<string>();
        public List<string> TotalInfo = new List<string>();

        public void StartWebCrawler()
        {
            if (File.Exists("FailLog.txt"))
            {
                File.Delete("FailLog.txt");
            }

            FileStream FailLog = new FileStream("FailLog.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(FailLog, Encoding.Default);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 1; i <= Chalaoshi.MaximumTeacherPage; i++)
            {
                LeftLinkList.Add("https://chalaoshi.cn/teacher/" + i.ToString() + "/");
            }

            for (int Loopi = 0; Loopi < 4; Loopi++) //最多循环6次，还爬不到的就是服务器问题了
            {
                if (LeftLinkList.Count == 0)
                    break;
                PageLinkList.Clear();
                LeftLinkList.ForEach(i => PageLinkList.Add(i));
                Task task = new Task(() =>
                {
                    Parallel.For(0, PageLinkList.Count, (i) =>
                    {
                        var crawler = new PoliteWebCrawler();
                        var url = PageLinkList[i];
                        Console.WriteLine("Start:" + url);
                        crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
                        crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
                        crawler.PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
                        //crawler.PageLinksCrawlDisallowedAsync += crawler_PageLinksCrawlDisallowed;
                        try
                        {
                            crawler.Crawl(new Uri(url));
                        }
                        catch (Exception ex)
                        {
                            sw.WriteLine(url + ex.Message);
                        }
                        Console.WriteLine("Finish:" + url);
                        Thread.Sleep(200);//给服务器休息一下
                    });
                });
                task.Start();
                task.Wait();
                Thread.Sleep(1000);//给服务器休息一下
            }

            for (int i = 0; i < PageLinkList.Count; i++)
            {
                sw.WriteLine("#{0}#未写入", LeftLinkList[i]);
            }
            sw.Close();
            FailLog.Close();

            var fileName = $"CLStext_{DateTime.Now.Year.ToString()}_{DateTime.Now.Month.ToString()}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.csv";
            FileStream file = new FileStream(fileName, FileMode.Append, FileAccess.Write);//在此处定义，保证读写锁的最大性能
            StreamWriter streamWriter = new StreamWriter(file, Encoding.Default); // 创建写入流

            for (int i = 0; i < TotalInfo.Count; i++)
            {
                streamWriter.WriteLine(TotalInfo[i]);
            }
            streamWriter.Close();
            file.Close();

            stopwatch.Stop();
            Console.WriteLine("Finished Completely.\nTime Consume:{0}\nPage Number:{1}\nTime Per Page:{2} milliseconds.\nPress any key to continue.", stopwatch.Elapsed, TotalInfo.Count, stopwatch.ElapsedMilliseconds / TotalInfo.Count);
            Console.WriteLine(FailedPageCount.ToString());
            Console.ReadLine();
        }

        void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            //Console.WriteLine("About to crawl link {0} which was found on page {1}", pageToCrawl.Uri.AbsoluteUri, pageToCrawl.ParentUri.AbsoluteUri);
        }

        void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;

            //Console.WriteLine("Completed函数标识位"); - 调试用

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
            {
                FileStream FailLog = new FileStream("FailLog.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(FailLog);
                sw.WriteLine(crawledPage.Uri.AbsoluteUri + crawledPage.HttpWebResponse.StatusCode.ToString());
                sw.Close();
                FailLog.Close();
                return;
            }

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
            {
                FileStream FailLog = new FileStream("FailLog.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(FailLog, Encoding.Default);
                sw.WriteLine(crawledPage.Uri.AbsoluteUri + "No Content");
                sw.Close();
                FailLog.Close();
                return;
            }

            var Uri = crawledPage.Uri.AbsoluteUri;
            var ID = Regex.Replace(Uri, @"[^0-9]+", "");
            var Name = crawledPage.CsQueryDocument.Select("h3").FirstElement().FirstChild.ToString();
            var Score = crawledPage.CsQueryDocument.Select("h2").FirstElement().FirstChild.ToString();
            Score = (Score == "N/A") ? 0.00.ToString() : Score;
            var Info = $"{Name},{Score},{ID},{Uri}";

            //var WriteContent = Name + "\t\t\t" + Score + "\t" + Uri;

            if (Uri == string.Empty || Score == string.Empty || Name == string.Empty)
            {
                FileStream FailLog = new FileStream("FailLog.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(FailLog);
                sw.WriteLine(crawledPage.Uri.AbsoluteUri + "Some of the content is empty ");
                sw.Close();
                FailLog.Close();
                return;
            }

            try
            {
                //FileStream file = new FileStream("CLStest.csv", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);//在此处定义，保证读写锁的最大性能
                //StreamWriter sw = new StreamWriter(file, Encoding.Default); // 创建写入流
                //sw.WriteLine("{0},{1},{2}", Uri, Score, Name);
                //sw.Close();
                //file.Close();
                if (Regex.IsMatch(Score, @"^[+-]?\d*[.]?\d*$"))
                {
                    if (!TotalInfo.Contains(Info))
                    {
                        TotalInfo.Add(Info);
                        LeftLinkList.Remove(crawledPage.Uri.AbsoluteUri);
                    }
                    else
                    {
                        FileStream FailLog = new FileStream("FailLog.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                        StreamWriter sw = new StreamWriter(FailLog);
                        sw.WriteLine(crawledPage.Uri.AbsoluteUri + "Repeated Info Detected");
                        sw.Close();
                        FailLog.Close();
                    }
                    CrawledPageCount++;
                }
            }
            catch (Exception ex)
            {
                FailedPageCount++;
                Console.WriteLine(ex.Message);
            }
        }

        void crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            Console.WriteLine("Did not crawl the links on page {0} due to {1}", crawledPage.Uri.AbsoluteUri, e.DisallowedReason);
            FileStream FailLog = new FileStream("FailLog.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(FailLog);
            sw.WriteLine(crawledPage.Uri.AbsoluteUri + e.DisallowedReason);
            sw.Close();
            FailLog.Close();
            return;
        }

        void crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("Did not crawl page {0} due to {1}", pageToCrawl.Uri.AbsoluteUri, e.DisallowedReason);
        }
    }
}
