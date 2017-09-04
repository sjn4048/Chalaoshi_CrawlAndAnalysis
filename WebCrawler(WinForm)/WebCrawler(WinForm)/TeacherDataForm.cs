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
    public partial class TeacherDataForm : Form
    {
        TeacherData teacherData;

        public TeacherDataForm()
        {
            InitializeComponent();
        }

        public TeacherDataForm(TeacherData inputTeacherData)
        {
            this.teacherData = inputTeacherData;
            InitializeComponent();
            ListAllItems();
        }

        private void TeacherDataForm_Load(object sender, EventArgs e)
        {

        }

        private void ListAllItems()
        {
            this.Text = $"{teacherData.name}, 分数：{teacherData.score_string}";
            var groupBox1 = new GroupBox()
            {
                Text = string.Empty,
                Location = new Point(50, 30),
                Size = new Size(650,100),
                AutoSize = true,
            };
            this.Controls.Add(groupBox1);

            var nameLabel = new Label()
            {
                Text = teacherData.name,
                Font = new Font("微软雅黑", 18, FontStyle.Regular),
                Location = new Point(30, 30),
                AutoSize = true
            };
            groupBox1.Controls.Add(nameLabel);

            var facultyLabel = new Label()
            {
                Text = $"浙江大学 {teacherData.faculty}",
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(35, 90),
                AutoSize = true
            };
            groupBox1.Controls.Add(facultyLabel);

            var scoreLabel = new Label()
            {
                Text = $"分数：{teacherData.score_string}",
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(35, 120),
                AutoSize = true
            };
            groupBox1.Controls.Add(scoreLabel);
            var hotLabel = new Label()
            {
                Text = $"热度：{teacherData.voteNum_int + teacherData.commentNum_int}",
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(400, 90),
                AutoSize = true
            };
            if (hotLabel.Text == "0")
            {
                hotLabel.Text = "<10";
            }
            groupBox1.Controls.Add(hotLabel);

            var callNameRateLabel = new Label()
            {
                Text = $"点名率：{teacherData.callNameRate_string}",
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(35, 150),
                AutoSize = true
            };
            groupBox1.Controls.Add(callNameRateLabel);

            var groupBox2 = new GroupBox()
            {
                Text = string.Empty,
                Location = new Point(50, 230),
                Size = new Size(650, 100),
                AutoSize = true,
            };
            this.Controls.Add(groupBox2);


            var courseLabel = new Label()
            {
                Text = "课程名称（点击查看详情）",
                Font = new Font("微软雅黑", 12, FontStyle.Underline),
                Location = new Point(35, 20),
                AutoSize = true
            };
            groupBox2.Controls.Add(courseLabel);

            var courseGPALabel = new Label()
            {
                Text = $"平均绩点/样本量",
                Font = new Font("微软雅黑", 12, FontStyle.Underline),
                Location = new Point(400, 20),
                AutoSize = true
            };
            groupBox2.Controls.Add(courseGPALabel);

            int i = 0;
            int step = 45;
            if (teacherData.courseList.Count == 0)
            {
                var noCourseLabel = new Label()
                {
                    Text = "暂无这位老师的授课数据",
                    Font = new Font("微软雅黑", 11, FontStyle.Regular),
                    Location = new Point(35, 70),
                    AutoSize = true
                };
                groupBox2.Controls.Add(noCourseLabel);
            }

            else
            {
                foreach (CourseData thisCourse in teacherData.courseList)
                {
                    var thisCourseLabel = new Label()
                    {
                        Text = thisCourse.CourseName.Replace("\"", ""),
                        Font = new Font("微软雅黑", 11, FontStyle.Regular),
                        Location = new Point(35, 70 + i * step),
                        AutoSize = true
                    };

                    CourseData selectedCourse = null;
                    foreach (CourseData course in CourseData.courseDataList)
                    {
                        if (course.CourseName.Replace("\"", "") == thisCourseLabel.Text)
                        {
                            selectedCourse = course;
                            break;
                        }
                    }

                    thisCourseLabel.Click += (s, arg) =>
                    {
                        if (selectedCourse != null)
                        {
                            new CourseDataForm(selectedCourse)
                            {
                                TopMost = true,
                                StartPosition = FormStartPosition.CenterScreen,
                            }
                            .Show();
                        }
                    };
                    groupBox2.Controls.Add(thisCourseLabel);
                    var thisCourseGPALabel = new Label()
                    {
                        Text = $"{thisCourse.OverallGPAOfTeacher}/{thisCourse.GPASampleSizeOfTeacher_int}",
                        Font = new Font("微软雅黑", 11, FontStyle.Regular),
                        Location = new Point(400, 70 + i * step),
                        AutoSize = true
                    };
                    if (selectedCourse.GPASampleSizeOfTeacher_int == 0 || selectedCourse.OverallGPAOfTeacher == 0)
                    {
                        thisCourseGPALabel.ForeColor = Color.DarkGray;
                    }
                    else if (thisCourse.OverallGPAOfTeacher > 4.5)
                    {
                        thisCourseGPALabel.ForeColor = Color.Green;
                    }
                    else if (thisCourse.OverallGPAOfTeacher > 4)
                    {
                        thisCourseGPALabel.ForeColor = Color.Blue;
                    }
                    else if (thisCourse.OverallGPAOfTeacher > 3.5)
                    {
                        thisCourseGPALabel.ForeColor = Color.Goldenrod;
                    }
                    else if (thisCourse.OverallGPAOfTeacher > 3)
                    {
                        thisCourseGPALabel.ForeColor = Color.IndianRed;
                    }
                    else
                    {
                        thisCourseGPALabel.ForeColor = Color.Maroon;
                    }
                    groupBox2.Controls.Add(thisCourseGPALabel);
                    i++;
                    if (thisCourseLabel.Text.Length > 17)
                    {
                        var nameLabel2 = new Label()
                        {
                            Location = new Point(35, 70 + i * step),
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
                        groupBox2.Controls.Add(nameLabel2);
                        thisCourseLabel.Text = thisCourseLabel.Text.Substring(0, 17);
                        i++;
                    }
                }
            }
            var emptyLabel = new Label()
            {
                Text = string.Empty,
                Location = new Point(450, 290 + i * step + 25),
                AutoSize = true
            };
            this.Controls.Add(emptyLabel);
        }
    }
}
