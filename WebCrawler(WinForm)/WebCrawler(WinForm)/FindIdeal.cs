﻿using System;
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
    public partial class FindIdeal : Form
    {
        Panel teacherPanel = new Panel()
        {
            Location = new Point(12, 12),
            Size = new Size(720, 460),
        };
        Panel coursePanel = new Panel()
        {
            Location = new Point(12, 12),
            Size = new Size(720, 460),
        };
        Panel thisPanel;

        public FindIdeal()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void findTeacherLabel_Click(object sender, EventArgs e)
        {

        }

        private void FindIdeal_Load(object sender, EventArgs e)
        {
            thisPanel = startPanel;
            DrawTeacherPanel();
            DrawCoursePanel();
        }

        private void DrawTeacherPanel()
        {
            teacherPanel.Hide();
            this.Controls.Add(teacherPanel);

            pictureBox1.Click += (s, arg) =>
            {
                startPanel.Hide();
                teacherPanel.Show();
                thisPanel = teacherPanel;
            };
            pictureBox1.MouseEnter += (s, arg) =>
            {
                pictureBox1.BackColor = SystemColors.GradientActiveCaption;
                findTeacherLabel.BackColor = SystemColors.GradientActiveCaption;
            };
            pictureBox1.MouseLeave += (s, arg) =>
            {
                pictureBox1.BackColor = SystemColors.Control;
                findTeacherLabel.BackColor = SystemColors.Control;
            };
            findTeacherLabel.Click += (s, arg) =>
            {
                startPanel.Hide();
                teacherPanel.Show();
                thisPanel = teacherPanel;
            };
            findTeacherLabel.MouseEnter += (s, arg) =>
            {
                pictureBox1.BackColor = SystemColors.GradientActiveCaption;
                findTeacherLabel.BackColor = SystemColors.GradientActiveCaption;
            };
            findTeacherLabel.MouseLeave += (s, arg) =>
            {
                pictureBox1.BackColor = SystemColors.Control;
                findTeacherLabel.BackColor = SystemColors.Control;
            };

            var teacherSearchLabel = new Label()
            {
                Text = "请选择筛选条件",
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(30, 60),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            teacherPanel.Controls.Add(teacherSearchLabel);

            var teacherReturnLabel = new Label()
            {
                Text = "返回上一级",
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(25, 15),
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
            };
            teacherReturnLabel.Click += (s, arg) =>
            {
                thisPanel.Hide();
                startPanel.Show();
                thisPanel = startPanel;
            };
            teacherPanel.Controls.Add(teacherReturnLabel);

            var scoreLabel = new Label()
            {
                Text = "评分高于：",
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(30, 120),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            teacherPanel.Controls.Add(scoreLabel);

            var scoreTextbox = new TextBox()
            {
                Text = "0",
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(140, 120),
                AutoSize = true,
                TabIndex = 0,
            };
            teacherPanel.Controls.Add(scoreTextbox);

            var callNameLabel = new Label()
            {
                Text = "点名情况：",
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(30, 160),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            teacherPanel.Controls.Add(callNameLabel);

            var callNameComboBox = new ComboBox()
            {
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(140, 160),
                AutoSize = true,
                Items = { "不限", "不点名", "点名" },
                SelectedIndex = 0,
                TabIndex = 1,
            };
            teacherPanel.Controls.Add(callNameComboBox);
        }

        private void DrawCoursePanel()
        {
            coursePanel.Hide();
            this.Controls.Add(coursePanel);

            pictureBox2.Click += (s, arg) =>
            {
                startPanel.Hide();
                coursePanel.Show();
                thisPanel = coursePanel;
            };
            pictureBox2.MouseEnter += (s, arg) =>
            {
                pictureBox2.BackColor = SystemColors.GradientActiveCaption;
                findCourseLabel.BackColor = SystemColors.GradientActiveCaption;
            };
            pictureBox2.MouseLeave += (s, arg) =>
            {
                pictureBox2.BackColor = SystemColors.Control;
                findCourseLabel.BackColor = SystemColors.Control;
            };
            findCourseLabel.Click += (s, arg) =>
            {
                startPanel.Hide();
                coursePanel.Show();
                thisPanel = coursePanel;
            };
            findCourseLabel.MouseEnter += (s, arg) =>
            {
                pictureBox2.BackColor = SystemColors.GradientActiveCaption;
                findCourseLabel.BackColor = SystemColors.GradientActiveCaption;
            };
            findCourseLabel.MouseLeave += (s, arg) =>
            {
                pictureBox2.BackColor = SystemColors.Control;
                findCourseLabel.BackColor = SystemColors.Control;
            };

            var courseSearchLabel = new Label()
            {
                Text = "请选择筛选条件",
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(30, 60),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            coursePanel.Controls.Add(courseSearchLabel);

            var courseReturnLabel = new Label()
            {
                Text = "返回上一级",
                Font = new Font("微软雅黑", 11, FontStyle.Regular),
                Location = new Point(25, 15),
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
            };
            courseReturnLabel.Click += (s, arg) =>
            {
                thisPanel.Hide();
                startPanel.Show();
                thisPanel = startPanel;
            };
            coursePanel.Controls.Add(courseReturnLabel);
        }
    }
}
