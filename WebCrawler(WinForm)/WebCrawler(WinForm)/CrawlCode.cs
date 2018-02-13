using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Net;
using HtmlAgilityPack;
using Newtonsoft.Json;


namespace WebCrawler_WinForm_
{
    class WebCrawlerProcess
    {
        CrawlPage crawlForm;
        //long TotalFlow; //debug

        public WebCrawlerProcess(CrawlPage form)
        {
            this.crawlForm = form;
        }

        public List<int> IDList = new List<int>();
        public List<int> LeftIDList = new List<int>();
        public List<string> TotalInfo = new List<string>();

        public void StartWebCrawler(object obj)
        {
            CancellationToken ct = (CancellationToken)obj;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 1; i <= Chalaoshi.MaximumTeacherPage; i++)
            {
                LeftIDList.Add(i);
            }
            LeftIDList.Remove(2485);//这4个网址(ID)有问题
            LeftIDList.Remove(3433);
            LeftIDList.Remove(4081);
            LeftIDList.Remove(4796);

            for (int Loopi = 0; Loopi < 5; Loopi++) //循环，还爬不到的就是服务器问题了
            {
                if (ct.IsCancellationRequested)
                    return;
                LeftIDList.RemoveAll(m => m == 0);
                if (LeftIDList.Count == 0)
                    break;
                IDList.Clear();
                LeftIDList.Distinct().ToList().ForEach(i => IDList.Add(i)); //将LeftIDList赋值给IDList
                Parallel.For(0, IDList.Count, (i) =>
                {
                    if (ct.IsCancellationRequested)
                        return;
                    var url = "https://chalaoshi.cn/teacher/" + IDList[i].ToString() + "/";
                    crawlForm.Invoke(new Action(() =>
                    {
                        if (crawlForm.checkBox1.Checked == true)
                        {
                            crawlForm.UpdateProcessList.Items.Add("Start:" + url);
                            crawlForm.UpdateProcessList.TopIndex = crawlForm.UpdateProcessList.Items.Count - 1;
                        }
                    }));
                    try
                    {
                        CrawlUrl(url);
                        crawlForm.Invoke(new Action(() =>
                        {
                            if (crawlForm.progressBar1.Value < crawlForm.progressBar1.Maximum)
                            {
                                crawlForm.progressBar1.Value += crawlForm.progressBar1.Step;
                                crawlForm.taskDialogProgressBar.Value += 1;
                            }
                            if (crawlForm.checkBox1.Checked == true)
                            {
                                crawlForm.UpdateProcessList.Items.Add("finish:" + url);
                                crawlForm.UpdateProcessList.TopIndex = crawlForm.UpdateProcessList.Items.Count - 1;
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        crawlForm.Invoke(new Action(() =>
                        {
                            if (crawlForm.checkBox2.Checked == true)
                            {
                                crawlForm.FailLogBox.Items.Add($"Error@{i}: {ex.Message}");
                            }
                        }));
                    }
                });
            }

            if (LeftIDList.Count > 0 && File.Exists("CLSDatabase.csv"))
            {
                foreach (var leftID in LeftIDList)
                {
                    crawlForm.Invoke(new Action(() =>
                    {
                        if (crawlForm.checkBox2.Checked == true)
                        {
                            crawlForm.FailLogBox.Items.Add($"{leftID}：未写入");
                        }
                    }));
                }
            }
            else
                WriteIntoCSV();//写入csv

            stopwatch.Stop();
            CrawlPage.isCrawlerRunning = false;

            try
            {
                crawlForm.Invoke(new Action(() =>
                {
                    if (crawlForm.checkBox1.Checked == true)
                    {
                        crawlForm.UpdateProcessList.Items.Add($"爬虫程序运行完毕");
                        crawlForm.UpdateProcessList.Items.Add($"总耗时:{stopwatch.Elapsed}");
                        crawlForm.UpdateProcessList.Items.Add($"总页面数:{TotalInfo.Count}");
                        crawlForm.UpdateProcessList.Items.Add($"页面平均耗时:{stopwatch.ElapsedMilliseconds / TotalInfo.Count}毫秒.");
                        crawlForm.UpdateProcessList.TopIndex = crawlForm.UpdateProcessList.Items.Count - 1;
                        crawlForm.progressBar1.Value = crawlForm.progressBar1.Maximum;
                        crawlForm.taskDialogProgressBar.Value = crawlForm.taskDialogProgressBar.Maximum;
                    }
                }));

            }
            catch
            {
                crawlForm.Invoke(new Action(() =>
                {
                    if (crawlForm.checkBox1.Checked == true)
                    {
                        crawlForm.UpdateProcessList.Items.Add("当前已爬网页数量为0");
                    }
                }));
            }
            crawlForm.Invoke(new Action(() =>
            {
                crawlForm.FinishButton.Enabled = true;
                crawlForm.button2.Enabled = false;
            }));

            using (var file = new FileStream("CLSDatabase.csv", FileMode.Open, FileAccess.Read))
            {
                new GetData().GetTeacherDataFromCsv(file);
            }
        }

        HtmlNode GetNodes(string url)
        {
            HtmlWeb web = new HtmlWeb()
            {
                UserAgent = "Mozilla / 5.0(iPhone; CPU iPhone OS 6_0 like Mac OS X) AppleWebKit / 536.26(KHTML, like Gecko) Version / 6.0 Mobile / 10A5376e Safari/ 8536.25"
            };
            HtmlDocument html = web.Load(url);
            if (web.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Status Code {web.StatusCode.ToString()}");

            return html.DocumentNode;
        } //获取url下的DOM Node

        void CrawlUrl(string url)
        {
            var Nodes = GetNodes(url);
            if (Nodes.ChildNodes.Count == 0)
            {
                throw new Exception("No Content");
            }

            var id = Regex.Replace(url, @"[^0-9]+", "");
            var name = Nodes.SelectSingleNode("/html/body/div[3]/div[1]/div[1]/h3")?.InnerHtml.ToString();
            var score = Nodes.SelectSingleNode("/html/body/div[3]/div[1]/div[2]/h2")?.InnerHtml.ToString();

            var voteNumber = new Regex(@"[0-9]*")
                 .Match(Nodes.SelectSingleNode("/html/body/div[3]/div[1]/div[2]/p").InnerHtml.ToString())
                 .Value;
            if (voteNumber == String.Empty)
            {
                voteNumber = "<5";
            }

            string commentNumber;
            try
            {
                commentNumber = new Regex(@"[0-9]*")
                .Match(Nodes.SelectSingleNode("/html/body/div[3]/div[3]/div[1]/p").InnerHtml.ToString())
                .Value;
            }
            catch
            {
                commentNumber = "<5";
            }

            var School = Nodes.SelectSingleNode("/html/body/div[3]/div[1]/div[1]/p[1]")?.InnerHtml.ToString();
            var faculty = Nodes.SelectSingleNode("/html/body/div[3]/div[1]/div[1]/p[2]")?.InnerHtml.ToString();
            string callNameRate;
            try
            {
                callNameRate = new Regex(@"[0-9]*(\.[0-9]+)?%")
                                    .Match(Nodes.SelectSingleNode("/html/body/div[3]/div[1]/div[1]/p[3]/text()").InnerHtml.ToString())
                                    .Value;
            }
            catch
            {
                callNameRate = "0.0%";
            };

            List<string> CombineList = new List<string>();
            for (int i = 2; ; i++)
            {
                try
                {
                    var courseName = Nodes.SelectSingleNode($"/html/body/div[3]/div[2]/div/div[{i}]/div[1]/p").InnerText.Trim().Replace("\r\n", " ").Replace(',', '，').Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&lt;", "<").Replace("&gt;", ">");
                    var overallGPA = Nodes.SelectSingleNode($"/html/body/div[3]/div[2]/div/div[{i}]/div[2]/p").InnerText;

                    string overallGPA_number = overallGPA.Substring(0, 4);
                    string GPASampleSize_size = overallGPA.Substring(5);

                    var NameGPACombination = $"{courseName},{overallGPA_number},{GPASampleSize_size}";
                    CombineList.Add(NameGPACombination);//即将被淘汰
                }
                catch
                {
                    break;
                }
            }

            var Info = $"{name},{id},{url},{faculty},{score},{callNameRate},{voteNumber},{commentNumber}";
            for (int Loopi = 0; Loopi < CombineList.Count; Loopi++)
                Info += $",{CombineList[Loopi]}";

            Info += '\n';
            ///爬取评论
            if (commentNumber != "<5")
            {
                var count = 0;
                for (int i = 0; ; i++)
                {
                    var comment_url = $"https://chalaoshi.cn/teacher/{id}/comment_list?page={i}&order_by=rate";
                    var comment_nodes = GetNodes(comment_url);
                    if (comment_nodes.ChildNodes.Count == 0)
                        break;

                    Regex timeRegex = new Regex(@"(\d{4}.\d{2}.\d{2})");
                    for (int j = 1; ; j++)
                    {
                        if (comment_nodes.SelectSingleNode($"div[{j}]") == null)
                            break;

                        string comment_text = comment_nodes.SelectSingleNode($"div[{j}]/div[1]/div[1]/p").InnerText.Trim().Replace("\r\n", "&NewLine").Replace(',', '，').Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&lt;", "<").Replace("&gt;", ">");
                        string comment_vote = comment_nodes.SelectSingleNode($"div[{j}]/div[1]/div[2]/p").InnerText.Trim().Replace("&nbsp;", "");
                        string comment_time = timeRegex.Match(comment_nodes.SelectSingleNode($"div[{j}]/p").InnerText)?.Groups[1].Value;
                        ///debug
                        count++;
                        Info += $"{comment_text},{comment_vote},{comment_time},";
                    }
                    if (count % 10 != 0)
                        break;
                }
                if (count < int.Parse(commentNumber)) //检验得到的评论数与查老师api提供的评论数是否相等
                    throw new Exception("comment num error(Not Vital)");
                Info.Trim(','); //去掉尾部逗号
            }

            if (url == string.Empty || score == string.Empty || name == string.Empty)
                throw new Exception("Attributes blank.");

            lock (LeftIDList) //锁住LeftIDList,防止线程错误
            {
                LeftIDList.Remove(int.Parse(id));
            }
            lock (TotalInfo)
            {
                if (!TotalInfo.Contains(Info))
                {
                    lock (TotalInfo) //锁住TotalInfo,防止线程错误
                    {
                        TotalInfo.Add(Info);
                    }
                }
                else
                {
                    throw new Exception("Repeated Content(Not Vital)"); //debug
                }
            }
        }


        void WriteIntoCSV()
        {
            var fileName = $"CLSDatabase.csv";
            using (FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(file, Encoding.Default)) // 创建写入流
                {
                    for (int i = 0; i < TotalInfo.Count; i++)
                    {
                        streamWriter.WriteLine(TotalInfo[i]);
                    }
                    crawlForm.Invoke(new Action(() =>
                    {
                        if (crawlForm.checkBox1.Checked == true)
                        {
                            crawlForm.UpdateProcessList.Items.Add("数据库已成功更新");
                        }
                    }));
                }
            }
        }
    }
}