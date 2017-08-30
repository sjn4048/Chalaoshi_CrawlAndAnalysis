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

namespace WebCrawler_WinForm_
{
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            try
            {
                FileStream file = new FileStream("CLSDatabase.csv", FileMode.Open, FileAccess.Read);
                var getData = new GetData();
                getData.getTeacherDataFromCsv(file);
            }
            catch
            {
                MessageBox.Show("离线数据库可能被人为删除，请点击“更新数据”重新获取", "Oops...");
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var keyword = textBox1.Text;
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("请输入搜索内容", "错误提示");
            }
            else
            {
                var search = new SearchAlgorithm();
                var searchedTeacherList = search.SearchTeacherName(keyword);
                var searchedCourseList = search.SearchCourseName(keyword);
                for (int i = 0; i < searchedCourseList.Count + searchedCourseList.Count; i++)
                {
                    
                }
            }
            
        }
    }
}
