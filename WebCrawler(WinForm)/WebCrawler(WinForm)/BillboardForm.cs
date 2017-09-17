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
        GroupBox groupBox1 = new GroupBox()
        {
            Location = new Point(40, 60),
            Size = new Size(540, 490),
            AutoSize = true
        };

        public BillboardForm()
        {
            InitializeComponent();
            this.Controls.Add(groupBox1);
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
                    teacherList = TeacherData.totalTeacherList.OrderByDescending(x => x.Score).ToList();
                    break;
                case 1:
                    foreach (TeacherData teacherData in TeacherData.totalTeacherList)
                    {
                        if (teacherData.Score != 0) teacherList.Add(teacherData);
                    }
                    teacherList = teacherList.OrderBy(y => y.Score).ToList();
                    break;
                case 2:
                    teacherList = TeacherData.totalTeacherList.OrderByDescending(x => x.CommentNum + x.VoteNum).ToList();
                    break;
                case 3:
                    teacherList = TeacherData.totalTeacherList.OrderByDescending(x => x.CourseList.Count).ToList();
                    break;
                case 4:
                    foreach (var teacherData in TeacherData.totalTeacherList)
                    {
                        teacherList.Insert(new Random().Next(teacherList.Count), teacherData);
                    }
                    break;
            }
            ShowBillBoard(teacherList, 50);
        }

        void ShowBillBoard(List<TeacherData> teacherList, int listNum)
        {
            int step = 45;

            if (groupBox1 != null)
            {
                groupBox1.Controls.Clear();
            }

            groupBox1.Text = comboBox1.SelectedText;
            var scoreLabel1 = new Label()
            {
                Text = "教师分数",
                Font = new Font("微软雅黑", 11, FontStyle.Underline),
                Location = new Point(185, 20),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            groupBox1.Controls.Add(scoreLabel1);

            var courseNumLabel1 = new Label()
            {
                Text = "授课数量",
                Font = new Font("微软雅黑", 11, FontStyle.Underline),
                Location = new Point(325, 20),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            groupBox1.Controls.Add(courseNumLabel1);

            var hotLabel1 = new Label()
            {
                Text = "总热度",
                Font = new Font("微软雅黑", 11, FontStyle.Underline),
                Location = new Point(440, 20),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            groupBox1.Controls.Add(hotLabel1);

            for (int i = 0; i < listNum; i++)
            {
                var thisTeacher = teacherList[i];
                var nameLabel = new Label()
                {
                    Text = thisTeacher.Name,
                    Font = new Font("微软雅黑", 11, FontStyle.Regular),
                    Location = new Point(35, 70 + i * step),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                    
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

                var scoreLabel = new Label()
                {
                    Location = new Point(200, 70 + i * step),
                    Text = thisTeacher.ToString("Score"),
                    Font = new Font("微软雅黑", 13),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                if (thisTeacher.Score > 9)
                {
                    scoreLabel.ForeColor = Color.Green;
                }
                else if (thisTeacher.Score > 7.5)
                {
                    scoreLabel.ForeColor = Color.Blue;
                }
                else if (thisTeacher.Score > 6)
                {
                    scoreLabel.ForeColor = Color.Goldenrod;
                }
                else if (thisTeacher.Score > 4)
                {
                    scoreLabel.ForeColor = Color.IndianRed;
                }
                else
                {
                    scoreLabel.ForeColor = Color.Maroon;
                }
                groupBox1.Controls.Add(scoreLabel);

                var courseNumLabel = new Label()
                {
                    Text = thisTeacher.CourseList.Count.ToString(),
                    Font = new Font("微软雅黑", 11, FontStyle.Regular),
                    Location = new Point(350, 70 + i * step),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                if (thisTeacher.CourseList.Count > 13)
                {
                    courseNumLabel.ForeColor = Color.Green;
                }
                else if (thisTeacher.CourseList.Count > 7)
                {
                    courseNumLabel.ForeColor = Color.Green;
                }
                else if (thisTeacher.CourseList.Count > 2)
                {
                    courseNumLabel.ForeColor = Color.Green;
                }
                else if (thisTeacher.CourseList.Count > 0)
                {
                    courseNumLabel.ForeColor = Color.IndianRed;
                }
                else
                {
                    courseNumLabel.ForeColor = Color.Maroon;
                }
                groupBox1.Controls.Add(courseNumLabel);


                var hotLabel = new Label()
                {
                    Text = thisTeacher.ToString("HotNum"),
                    Font = new Font("微软雅黑", 11, FontStyle.Regular),
                    Location = new Point(450, 70 + i * step),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                if (thisTeacher.CommentNum + thisTeacher.VoteNum > 499)
                {
                    hotLabel.ForeColor = Color.Green;
                }
                else if (thisTeacher.CommentNum + thisTeacher.VoteNum > 199)
                {
                    hotLabel.ForeColor = Color.Blue;
                }
                else if (thisTeacher.CommentNum + thisTeacher.VoteNum > 69)
                {
                    hotLabel.ForeColor = Color.Goldenrod;
                }
                else if (thisTeacher.CommentNum + thisTeacher.VoteNum > 19)
                {
                    hotLabel.ForeColor = Color.IndianRed;
                }
                else
                {
                    hotLabel.ForeColor = Color.Maroon;
                }
                groupBox1.Controls.Add(hotLabel);
            }
            var emptyLabel = new Label()
            {
                Text = string.Empty,
                Location = new Point(80, 165 + listNum * step),
                AutoSize = true
            };
            this.Controls.Add(emptyLabel);
        }
    }
}
