using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Net;
using Abot.Core;
using Abot.Crawler;
using Abot.Poco;
using CsQuery.HtmlParser;
using HtmlParserSharp;
using HtmlAgilityPack;

namespace WebCrawler_WinForm_
{
    class WebCrawlerProcess
    {
        CrawlPage crawlPage;

        //public static readonly Uri FeedUri = new Uri(@"https://chalaoshi.cn"); 暂时不需要
        public WebCrawlerProcess(CrawlPage form)
        {
            this.crawlPage = form;
        }
        static int CrawledPageCount = 0;//已爬的页面数
        static int FailedPageCount = 0;//用于debug

        public List<string> PageLinkList = new List<string>();
        public List<string> LeftLinkList = new List<string>();
        public List<string> TotalInfo = new List<string>();

        public void StartWebCrawler()
        {
            CrawlConfiguration crawlConfig = AbotConfigurationSectionHandler.LoadFromXml().Convert();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 1; i <= Chalaoshi.MaximumTeacherPage; i++)
            {
                LeftLinkList.Add("https://chalaoshi.cn/teacher/" + i.ToString() + "/");
            }
            LeftLinkList.Remove("https://chalaoshi.cn/teacher/2485/");//这两个网址有问题
            LeftLinkList.Remove("https://chalaoshi.cn/teacher/3433/");

            for (int Loopi = 0; Loopi < 5; Loopi++) //循环，还爬不到的就是服务器问题了
            {
                if (LeftLinkList.Count == 0)
                    break;
                PageLinkList.Clear();
                LeftLinkList.ForEach(i => PageLinkList.Add(i));
                Parallel.For(0, PageLinkList.Count, (i) =>
                {
                    var crawler = new PoliteWebCrawler(crawlConfig, null, null, null, null, null, null, null, null);
                    var url = PageLinkList[i];
                    //crawler.PageLinksCrawlDisallowedAsync += crawler_PageLinksCrawlDisallowed;

                    if (!string.IsNullOrEmpty(url))
                    {
                        if (crawlPage.checkBox1.Checked == true)
                        {
                            crawlPage.UpdateProcessList.Items.Add("Start:" + url);
                            crawlPage.UpdateProcessList.TopIndex = crawlPage.UpdateProcessList.Items.Count - 1;
                        }
                        crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
                        crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
                        crawler.PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
                        var crawlResult = crawler.Crawl(new Uri(url));
                        if (crawlPage.checkBox2.Checked == true && crawlResult.ErrorOccurred)
                        {
                            crawlPage.UpdateProcessList.Items.Add($"completed with error: {crawlResult.ErrorException.Message}");
                        }
                    }
                    Thread.Sleep(500);//给服务器休息一下
                });
                Thread.Sleep(1000);
            }

            if (LeftLinkList.Count > 0)
            {
                for (int i = 0; i < LeftLinkList.Count; i++)
                {
                    if (crawlPage.checkBox2.Checked == true)
                    {
                        crawlPage.FailLogBox.Items.Add($"#{LeftLinkList[i]}#未写入");
                    }
                }
            }
            {
                var fileName = $"CLSDatabase.csv";
                FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);//在此处定义，保证读写锁的最大性能
                StreamWriter streamWriter = new StreamWriter(file, Encoding.Default); // 创建写入流
                for (int i = 0; i < TotalInfo.Count; i++)
                {
                    streamWriter.WriteLine(TotalInfo[i]);
                }
                if (crawlPage.checkBox1.Checked == true)
                {
                    crawlPage.UpdateProcessList.Items.Add("数据库已成功更新");
                }
                streamWriter.Close();
                file.Close();
            }

            stopwatch.Stop();
            try
            {
                if (crawlPage.checkBox1.Checked == true)
                {
                    crawlPage.UpdateProcessList.Items.Add($"爬虫程序运行完毕");
                    crawlPage.UpdateProcessList.Items.Add($"总耗时:{stopwatch.Elapsed}");
                    crawlPage.UpdateProcessList.Items.Add($"总页面数:{TotalInfo.Count}");
                    crawlPage.UpdateProcessList.Items.Add($"页面平均耗时:{stopwatch.ElapsedMilliseconds / TotalInfo.Count}毫秒.");
                    crawlPage.UpdateProcessList.TopIndex = crawlPage.UpdateProcessList.Items.Count - 1;
                }
            }
            catch
            {
                if (crawlPage.checkBox1.Checked == true)
                {
                    crawlPage.UpdateProcessList.Items.Add("当前已爬网页数量为0");
                }
            }

            crawlPage.FinishButton.Show();
        }

        void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            //form.UpdateProcessList.Items.Add("About to crawl link {0} which was found on page {1}", pageToCrawl.Uri.AbsoluteUri, pageToCrawl.ParentUri.AbsoluteUri);
        }

        void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
            {
                if (crawlPage.checkBox2.Checked == true)
                {
                    crawlPage.FailLogBox.Items.Add(crawledPage.Uri.AbsoluteUri + crawledPage.HttpWebResponse.StatusCode.ToString());
                }
                return;
            }

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
            {
                if (crawlPage.checkBox2.Checked == true)
                {
                    crawlPage.FailLogBox.Items.Add(crawledPage.Uri.AbsoluteUri + "No Content");
                }
                return;
            }

            var Agility = crawledPage.HtmlDocument.DocumentNode;

            var Uri = crawledPage.Uri.AbsoluteUri;
            var ID = Regex.Replace(Uri, @"[^0-9]+", "");
            var Name = Agility.SelectSingleNode("/html/body/div[3]/div[1]/div[1]/h3")?.InnerHtml.ToString();
            var Score = Agility.SelectSingleNode("/html/body/div[3]/div[1]/div[2]/h2")?.InnerHtml.ToString();

            var VoteNumber = new Regex(@"[0-9]*")
                 .Match(Agility.SelectSingleNode("/html/body/div[3]/div[1]/div[2]/p").InnerHtml.ToString())
                 .Value;
            if (VoteNumber == String.Empty)
            {
                VoteNumber = "<5";
            }

            string CommentNumber;
            try
            {
                CommentNumber = new Regex(@"[0-9]*")
                .Match(Agility.SelectSingleNode("/html/body/div[3]/div[3]/div[1]/p").InnerHtml.ToString())
                .Value;
            }
            catch
            {
                CommentNumber = "<5";
            }

            var School = Agility.SelectSingleNode("/html/body/div[3]/div[1]/div[1]/p[1]")?.InnerHtml.ToString();
            var Faculty = Agility.SelectSingleNode("/html/body/div[3]/div[1]/div[1]/p[2]")?.InnerHtml.ToString();
            string CallNameRate;
            try
            {
                CallNameRate = new Regex(@"[0-9]*(\.[0-9]+)?%")
                                    .Match(Agility.SelectSingleNode("/html/body/div[3]/div[1]/div[1]/p[3]/text()").InnerHtml.ToString())
                                    .Value;
            }
            catch
            {
                CallNameRate = "0.0%";
            }
            #region

            int i = 2;
            var CourseList = new List<CourseData>();

            List<string> CombineList = new List<string>();// 即将被淘汰
            while (true)
            {
                try
                {
                    var courseName = crawledPage.CsQueryDocument.Select($"body > div.main > div:nth-child(2) > div > div:nth-child({i}) > div.left > p").FirstElement().FirstChild.ToString();
                    var overallGPA = crawledPage.CsQueryDocument.Select($"body > div.main > div:nth-child(2) > div > div:nth-child({i}) > div.right > p").FirstElement().FirstChild.ToString();
                    
                    string overallGPA_number = overallGPA.Substring(0, 4);
                    string GPASampleSize_size = overallGPA.Substring(5);

                    var NameGPACombination = $"{courseName},{overallGPA_number},{GPASampleSize_size}";
                    CombineList.Add(NameGPACombination);// 即将被淘汰
                }
                catch
                {
                    break;
                }
                i++;
            }

            /*
             * 这块是读取评论。不知道为什么Xpath找不到。我一定能做出来的。

            i = 1;
            while (true)
            {
                try//*[@id="comment-page"]/div/div[1]/p
                {
                    var CommentXpath = $"/html/body/div[3]/div[3]/div[2]/div[{i}]/div/p";
                    var VoteXpath = $"/html/body/div[3]/div[3]/div[2]/div[{i}]/div[1]/div[1]/p";
                    var comment = Agility.SelectSingleNode(CommentXpath).InnerHtml.ToString();
                    var voteNum = Agility.SelectSingleNode(VoteXpath).InnerHtml.ToString();
                    //var commentx = crawledPage.CsQueryDocument.Select(selector).FirstElement().FirstChild.ToString();
                    i++;
                }
                catch
                {
                    break;
                }
            }

            */
            #endregion

            var Info = $"{Name},{ID},{Uri},{Faculty},{Score},{CallNameRate},{VoteNumber},{CommentNumber}";

            for (int Loopi = 0; Loopi < CombineList.Count; Loopi++) // 即将被淘汰
            {
                Info += $",{CombineList[Loopi]}";
            }

            //var WriteContent = Name + "\t\t\t" + Score + "\t" + Uri;

            if (Uri == string.Empty || Score == string.Empty || Name == string.Empty)
            {
                if (crawlPage.checkBox2.Checked == true)
                {
                    crawlPage.FailLogBox.Items.Add(crawledPage.Uri.AbsoluteUri + "Some of the content is empty ");
                }
                return;
            }

            try
            {
                if (!TotalInfo.Contains(Info) && !string.IsNullOrEmpty(Uri))
                {
                    TotalInfo.Add(Info);
                    LeftLinkList.Remove(crawledPage.Uri.AbsoluteUri);
                    if (crawlPage.progressBar1.Value < crawlPage.progressBar1.Maximum)
                    {
                        crawlPage.progressBar1.Value += crawlPage.progressBar1.Step;
                    }
                    if (crawlPage.checkBox1.Checked == true)
                    {
                        crawlPage.UpdateProcessList.Items.Add("finish:" + crawledPage.Uri.AbsoluteUri);
                        crawlPage.UpdateProcessList.TopIndex = crawlPage.UpdateProcessList.Items.Count - 1;
                    }
                }
                else
                {
                    if (crawlPage.checkBox2.Checked == true)
                    {
                        crawlPage.FailLogBox.Items.Add(crawledPage.Uri.AbsoluteUri + "Repeated Info Detected");
                    }
                }
                CrawledPageCount++;
            }
            catch (Exception ex)
            {
                FailedPageCount++;
                if (crawlPage.checkBox2.Checked == true)
                {
                    crawlPage.FailLogBox.Items.Add(ex.Message);
                }
            }
        }

        void crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            var form = new CrawlPage();
            if (crawlPage.checkBox2.Checked == true)
            {
                form.FailLogBox.Items.Add(crawledPage.Uri.AbsoluteUri + e.DisallowedReason);
            }
            return;
        }

        void crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            var form = new CrawlPage();
            if (crawlPage.checkBox1.Checked == true)
            {
                form.UpdateProcessList.Items.Add($"Did not crawl page {pageToCrawl.Uri.AbsoluteUri} due to {e.DisallowedReason}");
                crawlPage.UpdateProcessList.TopIndex = crawlPage.UpdateProcessList.Items.Count - 1;
            }
        }
    }
}