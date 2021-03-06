﻿using System;
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
    public partial class SetConfigForm : Form
    {
        public SetConfigForm()
        {
            InitializeComponent();
        }

        private void SetConfigForm_Load(object sender, EventArgs e)
        {
            Config.ReadConfig();
            ShowConfig();
        }

        private void SetConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            new SettingForm() { StartPosition = FormStartPosition.CenterScreen, TopMost = true }.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("这将重置设置为默认参数，是否确认？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Config.SetInitialConfig();
                ShowConfig();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Config.SetConfig(int.Parse(resultTextBox.Text), hideCheckBox.Checked, (Config.ShowOrder)orderComboBox.SelectedIndex);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Config.SetConfig(int.Parse(resultTextBox.Text), hideCheckBox.Checked, (Config.ShowOrder)orderComboBox.SelectedIndex);
            this.Close();
        }

        public void ShowConfig()
        {
            resultTextBox.Text = Config.MaxResultPerPage.ToString();
            orderComboBox.SelectedIndex = (int)Enum.Parse(typeof(Config.ShowOrder), Config.Order.ToString());
            hideCheckBox.Checked = Config.HideUnrated;
        }
    }

    public static class Config
    {
        public static int MaxResultPerPage;
        public static bool HideUnrated;
        public static int DatabaseVersion;

        public enum ShowOrder
        {
            None = 0,
            Score,
            HotNum,
            CallNameRate,
        };
        public static ShowOrder Order;

        public static void ReadConfig()//读取配置数据，如果没有的话新建配置并使用默认
        {
            try
            {
                if (File.Exists("Config.ini"))
                {
                    using (StreamReader configSr = new StreamReader(new FileStream("Config.ini", FileMode.Open)))
                    {
                        var configPart = configSr.ReadToEnd().Split(',');

                        MaxResultPerPage = int.Parse(configPart[0]);
                        HideUnrated = bool.Parse(configPart[1]);
                        Order = (ShowOrder)Enum.Parse(typeof(ShowOrder), configPart[2]);
                        DatabaseVersion = int.Parse(configPart[3]);
                    }
                }
                else
                {
                    SetInitialConfig();
                }
            }
            catch
            {
                SetInitialConfig();
            }
        }

        public static void SetConfig(int MaxResultPerPage, bool HideUnrated, Config.ShowOrder ShowOrder)//将窗体中的设置保存为文件，如果没有的话新建配置并保存
        {
            if (MaxResultPerPage > 100)
            {
                if (MessageBox.Show("同时显示超过100个结果可能会导致界面卡顿，是否确认？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            using (var configSr = new StreamWriter(new FileStream("Config.ini", FileMode.Create, FileAccess.ReadWrite)))
            {
                string configString = $"{MaxResultPerPage},{HideUnrated},{ShowOrder},{Config.DatabaseVersion}";
                configSr.Write(configString);
            }
            ReadConfig();
        }

        public static void SetInitialConfig()
        {
            using (FileStream configFile = new FileStream("Config.ini", FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter configSr = new StreamWriter(configFile))
                {
                    string configString = $"30,False,None";
                    configSr.Write(configString);
                    ReadConfig();
                }
            }
        }
    }
}
