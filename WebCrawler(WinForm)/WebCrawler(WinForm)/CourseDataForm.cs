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
    public partial class CourseDataForm : Form
    {
        public CourseData courseData;

        public CourseDataForm()
        {
            InitializeComponent();
        }

        public CourseDataForm(CourseData inputCourseData)
        {
            this.courseData = inputCourseData;
            InitializeComponent();
            ListAllItems();
        }

        private void ClassDataForm_Load(object sender, EventArgs e)
        {
        }

        private void ListAllItems()
        {
            this.Text = courseData.CourseName.Replace("\"", "");
            var groupBox1 = new GroupBox()
            {
                Text = string.Empty,
                Location = new Point(50, 30),
                Size = new Size(650, 100),
                AutoSize = true,
            };
            this.Controls.Add(groupBox1);

            var courseNameLabel = new Label()
            {
                Text = courseData.CourseName.Replace("\"", ""),
                Font = new Font("微软雅黑", 18, FontStyle.Regular),
                Location = new Point(30, 30),
                AutoSize = true
            };
            groupBox1.Controls.Add(courseNameLabel);

            var overallGPALabel = new Label()
            {
                Text = $"课程均绩/样本量：{courseData.OverallGPAOfThisCourse:F2}/{courseData.TotalSampleSize}",
                Font = new Font("微软雅黑", 12, FontStyle.Regular),
                Location = new Point(30, 90),
                AutoSize = true
            };
            if (courseData.TotalSampleSize > 999)
            {
                overallGPALabel.Text = $"课程均绩/样本量：{courseData.OverallGPAOfThisCourse:F2}/999+";
            }
            groupBox1.Controls.Add(overallGPALabel);

            var groupBox2 = new GroupBox()
            {
                Text = string.Empty,
                Location = new Point(50, 170),
                Size = new Size(650, 100),
                AutoSize = true,
            };
            this.Controls.Add(groupBox2);

            int i = 0;
            int step = 45;

            groupBox2.Controls.Add(new Label()
            {
                Text = "教师姓名",
                Font = new Font("微软雅黑", 11, FontStyle.Underline),
                Location = new Point(30, 30),
                AutoSize = true,
            });

            groupBox2.Controls.Add(new Label()
            {
                Text = "教师评分",
                Font = new Font("微软雅黑", 11, FontStyle.Underline),
                Location = new Point(250, 30),
                AutoSize = true
            });

            groupBox2.Controls.Add(new Label()
            {
                Text = "教师均绩",
                Font = new Font("微软雅黑", 11, FontStyle.Underline),
                Location = new Point(470, 30),
                AutoSize = true
            });

            foreach (TeacherData teacher in courseData.CourseTeachers)
            {
                var teacherNameLabel = new Label()
                {
                    Text = teacher.name,
                    Font = new Font("微软雅黑", 11, FontStyle.Regular),
                    Location = new Point(30, 80 + i * step),
                    AutoSize = true
                };
                CourseData selectedCourse = null;
                foreach (CourseData course in teacher.courseList)
                {
                    if (course.CourseName == courseData.CourseName)
                    {
                        selectedCourse = course;
                        break;
                    }
                }
                teacherNameLabel.Click += (s, arg) =>
                {
                    new TeacherDataForm(teacher)
                    {
                        TopMost = true,
                        StartPosition = FormStartPosition.CenterScreen,
                    }
                    .Show();
                };
                groupBox2.Controls.Add(teacherNameLabel);

                var scoreLabel = new Label()
                {
                    Location = new Point(250, 80 + i * step),
                    Text = teacher.score_string,
                    Font = new Font("微软雅黑", 13),
                    AutoSize = true,
                };
                if (teacher.score > 9)
                {
                    scoreLabel.ForeColor = Color.Green;
                }
                else if (teacher.score > 7.5)
                {
                    scoreLabel.ForeColor = Color.Blue;
                }
                else if (teacher.score > 6)
                {
                    scoreLabel.ForeColor = Color.Goldenrod;
                }
                else if (teacher.score > 4)
                {
                    scoreLabel.ForeColor = Color.IndianRed;
                }
                else
                {
                    scoreLabel.ForeColor = Color.Maroon;
                }
                groupBox2.Controls.Add(scoreLabel);

                var hisCourseGPALabel = new Label()
                {
                    Text = $"{selectedCourse.OverallGPAOfTeacher}/{selectedCourse.GPASampleSizeOfTeacher_string}",
                    Font = new Font("微软雅黑", 13),
                    Location = new Point(470, 80 + i * step),
                    AutoSize = true
                };
                if (selectedCourse.OverallGPAOfTeacher / courseData.OverallGPAOfThisCourse > 1.07)
                {
                    hisCourseGPALabel.ForeColor = Color.Green;
                }
                else if (selectedCourse.OverallGPAOfTeacher / courseData.OverallGPAOfThisCourse > 0.93)
                {
                    hisCourseGPALabel.ForeColor = Color.Goldenrod;
                }
                else
                {
                    hisCourseGPALabel.ForeColor = Color.Maroon;
                }
                groupBox2.Controls.Add(hisCourseGPALabel);
                i++;
            }
            var emptyLabel = new Label()
            {
                Text = string.Empty,
                Location = new Point(80, 275 + i * step),
                AutoSize = true
            };
            this.Controls.Add(emptyLabel);
        }
    }
}
