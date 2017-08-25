using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
    static class Chalaoshi
    {
        public static int MaximumTeacherPage = 3743;
        public static Regex TeacherDataRegex = new Regex("^https://chalaoshi.cn/teacher/\\d+/");
    }

    class WebCrawlerProcess
    {
        //public static readonly Uri FeedUri = new Uri(@"https://chalaoshi.cn"); 暂时不需要

        static int CrawledPageCount = 0;//已爬的页面数
        static int FailedPageCount = 0;//用于debug

        public void StartWebCrawler()
        {
            XmlConfigurator.Configure();//本行控制是否采用log4net提供的log功能
            /*
             *单步写法，已被废弃
             *
          PoliteWebCrawler[] crawler = new PoliteWebCrawler[Chalaoshi.MaximumTeacherPage];
            for (int i = 0; i < Chalaoshi.MaximumTeacherPage; i++)
            {
                crawler[i] = new PoliteWebCrawler(crawlConfig, null, null, null, null, null, null, null, null);
                crawler[i].PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
                crawler[i].PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
                crawler[i].PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
                crawler[i].PageLinksCrawlDisallowedAsync += crawler_PageLinksCrawlDisallowed;

                CrawlResult result = crawler[i].Crawl(new Uri($"https://chalaoshi.cn/teacher/{i + 1}/")); //This is synchronous, it will not go to the next line until the crawl has completed

                if (result.ErrorOccurred)
                    Console.WriteLine("Crawl of {0} completed with error: {1}", result.RootUri.AbsoluteUri, result.ErrorException.Message);
                else
                    Console.WriteLine("Crawl of {0} completed without error.", result.RootUri.AbsoluteUri);
            }
            */

            /* 
             *Task 写法未成功，原因不明
             * Task[] TotalTasks = new Task[Chalaoshi.MaximumTeacherPage];
            Console.WriteLine("Start Crawling");
            for (var i = 0; i < Chalaoshi.MaximumTeacherPage; i++)
            {
                string url = "https://chalaoshi.cn/teacher/" + (i + 1).ToString() + "/";
                TotalTasks[i] = new Task(() =>
                {
                    var crawler = new PoliteWebCrawler(crawlConfig, null, null, null, null, null, null, null, null);
                    Console.WriteLine("Start:" + url);
                    crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
                    crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
                    crawler.PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
                    crawler.Crawl(new Uri(url));
                    Console.WriteLine("Finish:" + url);
                });
                TotalTasks[i].Start();
            }
            Task.WaitAll(TotalTasks);
            */

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Task task = new Task(() =>
            {
                Parallel.For(1, Chalaoshi.MaximumTeacherPage + 1, (i) =>
                {
                    var crawler = new PoliteWebCrawler();
                    string url = "https://chalaoshi.cn/teacher/" + i.ToString() + "/";
                    Console.WriteLine("Start:" + url);
                    crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
                    crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
                    crawler.PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
                    crawler.PageLinksCrawlDisallowedAsync += crawler_PageLinksCrawlDisallowed;
                    crawler.Crawl(new Uri(url));
                    Console.WriteLine("Finish:" + url);
                    Thread.Sleep(50);//给服务器喘息一下
                });
            });
            task.Start();
            task.Wait();
            stopwatch.Stop();
            Console.WriteLine("Finished Completely.\nTime Consume:{0}\nPage Number:{1}\nTime Per Page:{2} milliseconds.\nPress any key to continue.", stopwatch.Elapsed, CrawledPageCount, stopwatch.ElapsedMilliseconds / CrawledPageCount);
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

            /*
             * 是否爬取成功的标识
             * 
            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Crawl of page failed {0}", crawledPage.Uri.AbsoluteUri);
            else
                Console.WriteLine("Crawl of page succeeded {0}", crawledPage.Uri.AbsoluteUri);
            */

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
                Console.WriteLine("Page had no content {0}", crawledPage.Uri.AbsoluteUri);

            var Uri = crawledPage.Uri.AbsoluteUri;
            var Score = crawledPage.CsQueryDocument.Select("h2").FirstElement().FirstChild.ToString();
            var Name = crawledPage.CsQueryDocument.Select("h3").FirstElement().FirstChild.ToString();
            var WriteContent = Name + "\t" + Score + "\t" + Uri;

            try
            {
                FileStream file = new FileStream("CLStest.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);//在此处定义，保证读写锁的最大性能
                StreamWriter sw = new StreamWriter(file); // 创建写入流
                sw.WriteLine(WriteContent);
                CrawledPageCount++;
                sw.Close();
                file.Close();
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
        }

        void crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("Did not crawl page {0} due to {1}", pageToCrawl.Uri.AbsoluteUri, e.DisallowedReason);
        }
    }
}
