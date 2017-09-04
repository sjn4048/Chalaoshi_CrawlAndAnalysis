using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
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
using AngleSharp;
using HtmlParserSharp;
using HtmlAgilityPack;


namespace WebCrawler_Console_
{
    class Program
    {
        static void Main(string[] args)
        {
            //XmlConfigurator.Configure();//本行控制是否采用log4net提供的log功能

            WebCrawlerProcess crawler = new WebCrawlerProcess();
            crawler.StartWebCrawler();
        }
    }

    static class Chalaoshi
    {
        public static int MaximumTeacherPage = 10;
        public static Regex TeacherDataRegex = new Regex("^https://chalaoshi.cn/teacher/\\d+/");
        public struct CourseAndGPA
        {
            public string CourseName;
            public string OverallGPA; //后期要用Regex拆出来两个double
        }
    }

    class WebCrawlerProcess
    {
        //public static readonly Uri FeedUri = new Uri(@"https://chalaoshi.cn"); 暂时不需要

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

            LeftLinkList.Remove("https://chalaoshi.cn/teacher/2485/");
            LeftLinkList.Remove("https://chalaoshi.cn/teacher/3433/");

            for (int Loopi = 0; Loopi < 5; Loopi++) //最多循环6次，还爬不到的就是服务器问题了
            {
                if (LeftLinkList.Count == 0)
                    break;
                PageLinkList.Clear();
                LeftLinkList.ForEach(i => PageLinkList.Add(i));
                //Task task = new Task(() =>
                //{
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
                        Thread.Sleep(20);//给服务器休息一下
                    });
                //});
                //task.Start();
                //task.Wait();
                //Thread.Sleep(200);//给服务器休息一下
            }

            if (LeftLinkList.Count > 0)
            {
                for (int i = 0; i < LeftLinkList.Count; i++)
                {
                    sw.WriteLine("#{0}#未写入", LeftLinkList[i]);
                }
                sw.Close();
                FailLog.Close();
            }

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
            try
            {
                Console.WriteLine("Finished completely.\nTime Consumption:{0}\nPage Number:{1}\nTime Per Page:{2} milliseconds.\nPress any key to continue.", stopwatch.Elapsed, TotalInfo.Count, stopwatch.ElapsedMilliseconds / TotalInfo.Count);
            }
            catch
            {
                Console.WriteLine("当前已爬网页数量为0");
            }
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

            var Agility = crawledPage.HtmlDocument.DocumentNode;

            var Uri = crawledPage.Uri.AbsoluteUri;
            var ID = Regex.Replace(Uri, @"[^0-9]+", "");
            var Name = Agility.SelectSingleNode("/html/body/div[3]/div[1]/div[1]/h3")?.InnerHtml.ToString();
            var Score = Agility.SelectSingleNode("/html/body/div[3]/div[1]/div[2]/h2")?.InnerHtml.ToString();
            Score = (Score == "N/A") ? "0.00" : Score;

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
            var CourseList = new List<Chalaoshi.CourseAndGPA>();

            List<string> CombineList = new List<string>();// 即将被淘汰
            while (true)
            {
                try
                {
                    var courseName = crawledPage.CsQueryDocument.Select($"body > div.main > div:nth-child(2) > div > div:nth-child({i}) > div.left > p").FirstElement().FirstChild.ToString();
                    var overallGPA = crawledPage.CsQueryDocument.Select($"body > div.main > div:nth-child(2) > div > div:nth-child({i}) > div.right > p").FirstElement().FirstChild.ToString();
                    var NameGPACombination = courseName + ":" + overallGPA; // 即将被淘汰
                    CombineList.Add(NameGPACombination);// 即将被淘汰
                    Chalaoshi.CourseAndGPA newCourse;
                    newCourse.CourseName = courseName;
                    newCourse.OverallGPA = overallGPA;
                    CourseList.Add(newCourse);
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
