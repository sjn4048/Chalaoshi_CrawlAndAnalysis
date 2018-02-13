using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler_WinForm_
{
    class DataAnalyzer
    {
        //构造函数
        public DataAnalyzer()
        {
            Initialize();
        }

        //初始化Analyzer，此后将通过文件词典引入的办法，早期先将就一下吧
        private void Initialize()
        {
            Decorate_Word.Add("非常", 1);
            Decorate_Word.Add("超级", 1);
            Decorate_Word.Add("特别", 1);
            Decorate_Word.Add("很", 1);
            Decorate_Word.Add("very", 1);
            Decorate_Word.Add("不", -1);
            Decorate_Word.Add("非常", 1);
            Decorate_Word.Add("非常", 1);
        }

        //默认数值
        const int DEFAULT = 60;

        //用于分析的辅助变量
        private Dictionary<string, double> Decorate_Word = new Dictionary<string, double>(); //装饰词，如“非常”等
        private Dictionary<string, double> Comment_Word = new Dictionary<string, double>(); //装饰词，如“非常”等

        //double 
        public void Analyze(TeacherData teacher)
        {
            foreach (var comment in teacher.CommentList)
            {
                string text = comment.Text; //文本
                int weight = comment.Vote; //权重

            }
        }

        public void Analyze(CourseData course)
        {

        }
    }
}
