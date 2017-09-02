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
                groupBox1.Controls.Clear();
                groupBox1.Size = new Size(642, 368);
                var search = new SearchAlgorithm();
                var searchedTeacherList = search.SearchTeacherName(keyword);
                var searchedCourseList = search.SearchCourseName(keyword);
                var xStart = groupBox1.Location.X;
                var yStart = groupBox1.Location.Y - 35;
                var step = 42;

                int i = 0;
                foreach (TeacherData thisTeacher in searchedTeacherList)
                {
                    var nameLabel = new Label()
                    {
                        Location = new Point(xStart, yStart + i * step),
                        Text = thisTeacher.name,
                        Font = new Font("微软雅黑", 11),
                        AutoSize = true,
                    };

                    nameLabel.Click += (s, arg) =>
                    {
                        new TeacherDataForm(thisTeacher)
                        {
                            TopMost = true,
                            StartPosition = FormStartPosition.CenterScreen,
                        }
                        .Show();
                    };
                    var scoreLabel = new Label()
                    {
                        Location = new Point(xStart + 400, yStart + i * step),
                        Text = thisTeacher.score_string,
                        Font = new Font("微软雅黑", 11),
                        AutoSize = true,
                    };
                    if (thisTeacher.score > 9)
                    {
                        scoreLabel.ForeColor = Color.Green;
                    }
                    else if (thisTeacher.score > 7.5)
                    {
                        scoreLabel.ForeColor = Color.Blue;
                    }
                    else if (thisTeacher.score > 6)
                    {
                        scoreLabel.ForeColor = Color.Goldenrod;
                    }
                    else if (thisTeacher.score > 4)
                    {
                        scoreLabel.ForeColor = Color.IndianRed;
                    }
                    else
                    {
                        scoreLabel.ForeColor = Color.Maroon;
                    }
                    i++;
                    groupBox1.Controls.Add(nameLabel);
                    groupBox1.Controls.Add(scoreLabel);
                }
                foreach (CourseData thisCourse in searchedCourseList)
                {
                    var nameLabel = new Label()
                    {
                        Location = new Point(xStart, yStart + i * step),
                        Text = thisCourse.CourseName.Replace("\"",""),
                        Font = new Font("微软雅黑", 11),
                        AutoSize = true
                    };
                    nameLabel.Click += (s, arg) =>
                    {
                        new CourseDataForm(thisCourse)
                        {
                            TopMost = true,
                            StartPosition = FormStartPosition.CenterScreen
                        }
                        .Show();
                    };
                    groupBox1.Controls.Add(nameLabel);

                    var gpaLabel = new Label()
                    {
                        Location = new Point(xStart + 400, yStart + i * step),
                        Text = $"{thisCourse.OverallGPAOfThisCourse:F2}/{thisCourse.TotalSampleSize}",
                        Font = new Font("微软雅黑", 11),
                        AutoSize = true,
                    };
                    if (thisCourse.TotalSampleSize > 999)
                    {
                        gpaLabel.Text = $"{thisCourse.OverallGPAOfThisCourse:F2}/999+";
                    }
                    if (thisCourse.OverallGPAOfThisCourse > 4.5)
                    {
                        gpaLabel.ForeColor = Color.Green;
                    }
                    else if (thisCourse.OverallGPAOfThisCourse > 4)
                    {
                        gpaLabel.ForeColor = Color.Blue;
                    }
                    else if (thisCourse.OverallGPAOfThisCourse > 3.5)
                    {
                        gpaLabel.ForeColor = Color.Goldenrod;
                    }
                    else if (thisCourse.OverallGPAOfThisCourse > 3)
                    {
                        gpaLabel.ForeColor = Color.IndianRed;
                    }
                    else
                    {
                        gpaLabel.ForeColor = Color.Maroon;
                    }
                    groupBox1.Controls.Add(gpaLabel);
                    i++;
                    if (nameLabel.Text.Length > 17)
                    {
                        var nameLabel2 = new Label()
                        {
                            Location = new Point(xStart, yStart + i * step),
                            Text = thisCourse.CourseName.Replace("\"", "").Substring(17),
                            Font = new Font("微软雅黑", 11),
                            AutoSize = true
                        };
                        nameLabel2.Click += (s, arg) =>
                        {
                            new CourseDataForm(thisCourse)
                            {
                                TopMost = true,
                                StartPosition = FormStartPosition.CenterScreen
                            }
                            .Show();
                        };
                        groupBox1.Controls.Add(nameLabel2);
                        nameLabel.Text = nameLabel.Text.Substring(0, 17);
                        i++;
                    }
                }
                var emptyLabel = new Label()
                {
                    Text = string.Empty,
                    Location = new Point(xStart + 400, yStart + i * step + 100),
                    AutoSize = true
                };
                this.Controls.Add(emptyLabel);
            }

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
