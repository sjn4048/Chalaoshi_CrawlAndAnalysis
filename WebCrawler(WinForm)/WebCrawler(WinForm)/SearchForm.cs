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
        GroupBox groupBox1;
        GroupBox groupBox2;
        int maxItemsPerPage = 30;

        public SearchForm()
        {
            InitializeComponent();
            groupBox1 = new GroupBox()
            {
                Text = "教师搜索结果（点击教师名查看详情）",
                Font = new Font("微软雅黑", 10),
                Location = new Point(50, 80),
                Size = new Size(640, 160),
                AutoSize = true,
            };
            this.Controls.Add(groupBox1);
            groupBox2 = new GroupBox()
            {
                Text = "课程搜索结果（点击课程名查看详情）",
                Font = new Font("微软雅黑", 10),
                Size = new Size(640, 160),
                Location = new Point(50, 260),
                AutoSize = true,
            };
            this.Controls.Add(groupBox2);
        }
        private void SearchForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var keyword = textBox1.Text.Trim();
            if (keyword == string.Empty)
            {
                MessageBox.Show("请输入搜索内容", "错误提示");
            }
            else
            {
                var search = new SearchAlgorithm();
                var searchedTeacherList = search.SearchTeacherName(keyword);
                var searchedCourseList = search.SearchCourseName(keyword);
                DisplayResults(searchedTeacherList, searchedCourseList);
            }

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void DisplayResults(List<TeacherData>searchedTeacherList, List<CourseData>searchedCourseList)
        {
            var xStart = groupBox1.Location.X;
            var yStart = groupBox1.Location.Y + 20;
            var step = 45;

            //this.Size = new Size(760,530);
            groupBox1.Controls.Clear();
            groupBox2.Controls.Clear();
            groupBox1.Size = new Size(640, 100);
            groupBox1.Location = new Point(50, 80);
            groupBox2.Size = new Size(640, 100);
            groupBox2.Location = new Point(50, 260);
            
            if (searchedTeacherList.Count == 0)
            {
                var noContentLabel = new Label()
                {
                    Text = "没有匹配结果的教师",
                    Font = new Font("微软雅黑", 11, FontStyle.Regular),
                    Location = new Point(xStart, yStart - 50),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                groupBox1.Controls.Add(noContentLabel);
            }
            else
            {
                var nameLabel1 = new Label()
                {
                    Text = "教师姓名",
                    Font = new Font("微软雅黑", 11, FontStyle.Underline),
                    Location = new Point(xStart, yStart - 50),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                groupBox1.Controls.Add(nameLabel1);

                var callNameRateLabel1 = new Label()
                {
                    Text = "点名率",
                    Font = new Font("微软雅黑", 11, FontStyle.Underline),
                    Location = new Point(xStart + 350, yStart - 50),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                callNameRateLabel1.Click += (s, arg) =>
                {
                    DisplayResults(searchedTeacherList.OrderBy(m => m.callNameState).ToList(), searchedCourseList);
                };
                callNameRateLabel1.MouseEnter += (s, arg) =>
                {
                    callNameRateLabel1.BackColor = SystemColors.GradientInactiveCaption;
                };
                callNameRateLabel1.MouseLeave += (s, arg) =>
                {
                    callNameRateLabel1.BackColor = SystemColors.Control;
                };
                groupBox1.Controls.Add(callNameRateLabel1);

                var scoreLabel1 = new Label()
                {
                    Text = "评分",
                    Font = new Font("微软雅黑", 11, FontStyle.Underline),
                    Location = new Point(xStart + 220, yStart - 50),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                groupBox1.Controls.Add(scoreLabel1);
                scoreLabel1.Click += (s, arg) =>
                {
                    DisplayResults(searchedTeacherList.OrderByDescending(m => m.score).ToList(), searchedCourseList);
                };
                scoreLabel1.MouseEnter += (s, arg) =>
                {
                    scoreLabel1.BackColor = SystemColors.GradientInactiveCaption;
                };
                scoreLabel1.MouseLeave += (s, arg) =>
                {
                    scoreLabel1.BackColor = SystemColors.Control;
                };
                scoreLabel1.MouseEnter += (s, arg) =>
                {
                    scoreLabel1.BackColor = SystemColors.GradientInactiveCaption;
                };
                scoreLabel1.MouseLeave += (s, arg) =>
                {
                    scoreLabel1.BackColor = SystemColors.Control;
                };

                var commentNumLabel1 = new Label()
                {
                    Text = "总热度",
                    Font = new Font("微软雅黑", 11, FontStyle.Underline),
                    Location = new Point(xStart + 480, yStart - 50),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,

                };
                commentNumLabel1.Click += (s, arg) =>
                {
                    DisplayResults(searchedTeacherList.OrderByDescending(m => m.commentNum_int + m.voteNum_int).ToList(), searchedCourseList);
                };
                commentNumLabel1.MouseEnter += (s, arg) =>
                {
                    commentNumLabel1.BackColor = SystemColors.GradientInactiveCaption;
                };
                commentNumLabel1.MouseLeave += (s, arg) =>
                {
                    commentNumLabel1.BackColor = SystemColors.Control;
                };
                groupBox1.Controls.Add(commentNumLabel1);
            }

            int i = 0;
            foreach (TeacherData thisTeacher in searchedTeacherList)
            {
                var nameLabel = new Label()
                {
                    Location = new Point(xStart, yStart + i * step),
                    Text = thisTeacher.name,
                    Font = new Font("微软雅黑", 12),
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
                nameLabel.MouseEnter += (s, arg) =>
                {
                    nameLabel.BackColor = SystemColors.GradientInactiveCaption;
                };
                nameLabel.MouseLeave += (s, arg) =>
                {
                    nameLabel.BackColor = SystemColors.Control;
                };
                groupBox1.Controls.Add(nameLabel);

                var callNameRateLabel = new Label()
                {
                    Text = thisTeacher.callNameRate_string,
                    Font = new Font("微软雅黑", 11, FontStyle.Regular),
                    Location = new Point(xStart + 350, yStart + i * step),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                if (thisTeacher.callNameState == TeacherData.CallName_enum.Unknown)
                {
                    callNameRateLabel.Text = "N/A";
                    callNameRateLabel.ForeColor = Color.DarkGray;
                }
                else if (thisTeacher.callNameState == TeacherData.CallName_enum.Yes)
                {
                    callNameRateLabel.ForeColor = Color.Maroon;
                }
                else if (thisTeacher.callNameState == TeacherData.CallName_enum.Possible)
                {
                    callNameRateLabel.ForeColor = Color.Goldenrod;
                }
                else
                {
                    callNameRateLabel.ForeColor = Color.Green;
                }
                groupBox1.Controls.Add(callNameRateLabel);

                var scoreLabel = new Label()
                {
                    Location = new Point(xStart + 220, yStart + i * step),
                    Text = thisTeacher.score_string,
                    Font = new Font("微软雅黑", 12),
                    AutoSize = true,
                };
                if (thisTeacher.score == 0)
                {
                    scoreLabel.ForeColor = Color.DarkGray;
                }
                else if (thisTeacher.score > 9)
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
                groupBox1.Controls.Add(scoreLabel);

                var hotLabel = new Label()
                {
                    Location = new Point(xStart + 480, yStart + i * step),
                    Text = (thisTeacher.commentNum_int + thisTeacher.voteNum_int).ToString(),
                    Font = new Font("微软雅黑", 12),
                    AutoSize = true,
                };
                if (thisTeacher.commentNum_int + thisTeacher.voteNum_int > 999)
                {
                    hotLabel.ForeColor = Color.Green;
                }
                else if (thisTeacher.commentNum_int + thisTeacher.voteNum_int > 299)
                {
                    hotLabel.ForeColor = Color.Blue;
                }
                else if (thisTeacher.commentNum_int + thisTeacher.voteNum_int > 99)
                {
                    hotLabel.ForeColor = Color.Goldenrod;
                }
                else if (thisTeacher.commentNum_int + thisTeacher.voteNum_int > 19)
                {
                    hotLabel.ForeColor = Color.IndianRed;
                }
                else
                {
                    hotLabel.ForeColor = Color.Maroon;
                }
                if (hotLabel.Text == "0")
                {
                    hotLabel.Text = "<10";
                    hotLabel.ForeColor = Color.DarkGray;
                }
                groupBox1.Controls.Add(hotLabel);
                i++;
                if (i >= maxItemsPerPage) break;
            }

            groupBox2.Location = new Point(50, 240 + i * step);
            int j = 0;
            if (searchedCourseList.Count == 0)
            {
                var noContentLabel = new Label()
                {
                    Text = "没有匹配结果的课程",
                    Font = new Font("微软雅黑", 11, FontStyle.Regular),
                    Location = new Point(xStart, yStart - 50),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                groupBox2.Controls.Add(noContentLabel);
            }
            else
            {
                foreach (CourseData thisCourse in searchedCourseList)
                {
                    var nameLabel2 = new Label()
                    {
                        Text = "课程名称",
                        Font = new Font("微软雅黑", 11, FontStyle.Underline),
                        Location = new Point(xStart, yStart - 50),
                        AutoSize = true,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    groupBox2.Controls.Add(nameLabel2);

                    var scoreLabel2 = new Label()
                    {
                        Text = "均绩",
                        Font = new Font("微软雅黑", 11, FontStyle.Underline),
                        Location = new Point(xStart + 350, yStart - 50),
                        AutoSize = true,
                        TextAlign = ContentAlignment.MiddleCenter,

                    };
                    groupBox2.Controls.Add(scoreLabel2);
                    scoreLabel2.Click += (s, arg) =>
                    {
                        DisplayResults(searchedTeacherList, searchedCourseList.OrderByDescending(m => m.OverallGPAOfThisCourse).ToList());
                    };
                    scoreLabel2.MouseEnter += (s, arg) =>
                    {
                        scoreLabel2.BackColor = SystemColors.GradientInactiveCaption;
                    };
                    scoreLabel2.MouseLeave += (s, arg) =>
                    {
                        scoreLabel2.BackColor = SystemColors.Control;
                    };

                    var hotLabel2 = new Label()
                    {
                        Text = "总热度",
                        Font = new Font("微软雅黑", 11, FontStyle.Underline),
                        Location = new Point(xStart + 480, yStart - 50),
                        AutoSize = true,
                        TextAlign = ContentAlignment.MiddleCenter,

                    };
                    hotLabel2.Click += (s, arg) =>
                    {
                        DisplayResults(searchedTeacherList, searchedCourseList.OrderByDescending(m => m.TotalSampleSize).ToList());
                    };
                    hotLabel2.MouseEnter += (s, arg) =>
                    {
                        hotLabel2.BackColor = SystemColors.GradientInactiveCaption;
                    };
                    hotLabel2.MouseLeave += (s, arg) =>
                    {
                        hotLabel2.BackColor = SystemColors.Control;
                    };
                    groupBox2.Controls.Add(hotLabel2);

                    var nameLabel = new Label()
                    {
                        Location = new Point(xStart, yStart + j * step),
                        Text = thisCourse.CourseName.Replace("\"", ""),
                        Font = new Font("微软雅黑", 12),
                        AutoSize = true,

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
                    nameLabel.MouseEnter += (s, arg) =>
                    {
                        nameLabel.BackColor = SystemColors.GradientInactiveCaption;
                    };
                    nameLabel.MouseLeave += (s, arg) =>
                    {
                        nameLabel.BackColor = SystemColors.Control;
                    };
                    groupBox2.Controls.Add(nameLabel);

                    var gpaLabel = new Label()
                    {
                        Location = new Point(xStart + 350, yStart + j * step),
                        Text = $"{thisCourse.OverallGPAOfThisCourse:F2}",
                        Font = new Font("微软雅黑", 12),
                        AutoSize = true,
                    };
                    if (gpaLabel.Text == "NaN")
                    {
                        gpaLabel.ForeColor = Color.DarkGray;
                    }
                    else if (thisCourse.OverallGPAOfThisCourse > 4.5)
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
                    groupBox2.Controls.Add(gpaLabel);

                    var hotLabel = new Label()
                    {
                        Location = new Point(xStart + 480, yStart + j * step),
                        Text = thisCourse.TotalSampleSize.ToString(),
                        Font = new Font("微软雅黑", 12),
                        AutoSize = true,
                    };
                    if (thisCourse.TotalSampleSize == 0)
                    {
                        hotLabel.ForeColor = Color.DarkGray;
                    }
                    else if (thisCourse.TotalSampleSize > 999)
                    {
                        hotLabel.ForeColor = Color.Green;
                    }
                    else if (thisCourse.TotalSampleSize > 499)
                    {
                        hotLabel.ForeColor = Color.Blue;
                    }
                    else if (thisCourse.TotalSampleSize > 199)
                    {
                        hotLabel.ForeColor = Color.Goldenrod;
                    }
                    else if (thisCourse.TotalSampleSize > 49)
                    {
                        hotLabel.ForeColor = Color.IndianRed;
                    }
                    else
                    {
                        hotLabel.ForeColor = Color.Maroon;
                    }
                    groupBox2.Controls.Add(hotLabel);
                    j++;
                    if (nameLabel.Text.Length > 15)
                    {
                        var nameLabelAppend = new Label()
                        {
                            Location = new Point(xStart, yStart + j * step),
                            Text = thisCourse.CourseName.Replace("\"", "").Substring(15),
                            Font = new Font("微软雅黑", 12),
                            AutoSize = true,

                        };
                        nameLabelAppend.Click += (s, arg) =>
                        {
                            new CourseDataForm(thisCourse)
                            {
                                TopMost = true,
                                StartPosition = FormStartPosition.CenterScreen
                            }
                            .Show();
                        };
                        nameLabel.MouseEnter += (s, arg) =>
                        {
                            nameLabelAppend.BackColor = SystemColors.GradientInactiveCaption;
                        };
                        nameLabel.MouseLeave += (s, arg) =>
                        {
                            nameLabelAppend.BackColor = SystemColors.Control;
                        };
                        nameLabelAppend.MouseEnter += (s, arg) =>
                        {
                            nameLabel.BackColor = nameLabelAppend.BackColor = SystemColors.GradientInactiveCaption;
                        };
                        nameLabelAppend.MouseLeave += (s, arg) =>
                        {
                            nameLabel.BackColor = nameLabelAppend.BackColor = SystemColors.Control;
                        };
                        groupBox2.Controls.Add(nameLabelAppend);
                        nameLabel.Text = nameLabel.Text.Substring(0, 15);
                        j++;
                    }
                    if (j >= maxItemsPerPage) break;
                }
            }
            var emptyLabel = new Label()
            {
                Text = string.Empty,
                Location = new Point(xStart + 400, yStart + (i + j) * step + 300),
                AutoSize = true
            };
            this.Controls.Add(emptyLabel);
        }
    }
}