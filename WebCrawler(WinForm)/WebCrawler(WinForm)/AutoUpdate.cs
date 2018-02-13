using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;

namespace WebCrawler_WinForm_
{
    class AutoUpdate
    {
        public void UpdateDatabase()//读取github release的json，进行正则解析，根据版本号判断是否有更新并进行更新实装
        {
            if (!ToolFunctions.CheckNetworkStatus())
                return;

            string jsonUrl = "https://api.github.com/repos/sjn4048/Chalaoshi_CrawlAndAnalysis/releases/latest";
            HttpWebRequest request = WebRequest.Create(jsonUrl) as HttpWebRequest;
            request.UserAgent = "Mozilla/4.0";
            request.Method = "GET";
            request.AllowReadStreamBuffering = false;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream jsonStream = response.GetResponseStream();
            byte[] jsonBuf = new byte[1024];
            string jsonString = string.Empty;

            int count = jsonStream.Read(jsonBuf, 0, jsonBuf.Length);
            while (count > 0) //通过循环收集json内容
            {
                jsonString += Encoding.Default.GetString(jsonBuf, 0, count);
                count = jsonStream.Read(jsonBuf, 0, jsonBuf.Length);
            }

            //分析是否有更新
            string databaseRegexString = @"""name"":"".*([0-9]{8})"""; //正则表达式
            Regex versionRegex = new Regex(databaseRegexString);
            Match match = Regex.Match(jsonString, databaseRegexString);
            int LatestDatabaseVersion;

            if (!match.Success) //匹配失败
                return;
            else
                LatestDatabaseVersion = int.Parse(match.Groups[1].Value);

            if (Config.DatabaseVersion == LatestDatabaseVersion) //无更新时
                return;
            //有更新时
            if (MessageBox.Show(caption: "数据库有更新", text: "检测到服务器上有数据库更新，是否立即更新？", buttons: MessageBoxButtons.OKCancel, icon: MessageBoxIcon.Information) == DialogResult.Cancel)
                return; //选择不更新

            //选择更新
            string downloadRegexString = @"""browser_download_url"":""(.*?)"""; //正则表达式
            Regex downloadRegex = new Regex(downloadRegexString);
            match = Regex.Match(jsonString, downloadRegexString);
            if (!match.Success) //匹配失败
                return;
            //下载文件
            string url = match.Groups[1].Value;
            string path = "downloading_tmp.csv";
            WebClient webClient = new WebClient();
            try
            {
                webClient.DownloadFileAsync(new Uri(url), path);
            }
            catch
            {
                MessageBox.Show(caption: "下载失败", text: "由于服务器架设在Github.com上，国内网络访问较不稳定，导致下载失败。请尝试稍后再试或科学上网解决。", icon: MessageBoxIcon.Error, buttons: MessageBoxButtons.OK);
            }
            MessageBox.Show(caption: "开始下载", text: "已开始更新，结束后将提示。",icon:MessageBoxIcon.Information, buttons:MessageBoxButtons.OK);
            webClient.DownloadFileCompleted += (s, arg) =>
            {
                Config.DatabaseVersion = LatestDatabaseVersion;
                Config.SetConfig(Config.MaxResultPerPage,Config.HideUnrated,Config.Order);
                new GetData().GetTeacherDataFromCsv(new FileStream("downloading_tmp.csv", FileMode.Open, FileAccess.Read));
                MessageBox.Show(caption: "更新完成", text: "恭喜您，数据库已更新成功", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.None);
            };
            //别忘了重新加载数据库、更新数据库版本
        }
    }
}
