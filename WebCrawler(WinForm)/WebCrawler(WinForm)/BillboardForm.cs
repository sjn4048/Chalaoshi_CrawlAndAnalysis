using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebCrawler_WinForm_
{
    public partial class BillboardForm : Form
    {
        public BillboardForm()
        {
            InitializeComponent();
        }

        private void BillboardForm_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<TeacherData> teacherList = new List<TeacherData>();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    teacherList = TeacherData.totalTeacherList.OrderByDescending(x => x.score).ToList();
                    break;
                case 1:
                    teacherList = TeacherData.totalTeacherList.OrderBy(x => x.score).ToList();
                    break;
                case 2:
                    teacherList = TeacherData.totalTeacherList.OrderByDescending(x => x.commentNum_int + x.voteNum_int).ToList();
                    break;
                case 3:
                    teacherList = TeacherData.totalTeacherList.OrderByDescending(x => x.courseList.Count).ToList();
                    break;
                case 4:
                    foreach (var teacherData in TeacherData.totalTeacherList)
                    {
                        teacherList.Insert(new Random().Next(teacherList.Count), teacherData);
                    }
                    break;
            }
            ShowBillBoard(teacherList);
        }

        void ShowBillBoard(List<TeacherData> teacherList)
        {

        }
    }
}
