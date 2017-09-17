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
    public partial class SetConfigForm : Form
    {
        public SetConfigForm()
        {
            InitializeComponent();
        }

        private void SetConfigForm_Load(object sender, EventArgs e)
        {
            Config.ReadConfig();

            resultTextBox.Text = Config.MaxResultPerPage.ToString();
            orderComboBox.SelectedIndex = (int)Enum.Parse(typeof(Config.ShowOrder), Config.showOrder.ToString());
            hideCheckBox.Checked = Config.HideUnrated;
        }

        private void SetConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            new SettingForm() { StartPosition = FormStartPosition.CenterScreen, TopMost = true }.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Config.ReadConfig();
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
    }

    public static class Config
    {
        public static int MaxResultPerPage;
        public static bool HideUnrated;

        public enum ShowOrder
        {
            None = 0,
            Score,
            HotNum,
            CallNameRate,
        };
        public static ShowOrder showOrder;

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
                        showOrder = (ShowOrder)Enum.Parse(typeof(ShowOrder), configPart[2]);
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
            using (var configSr = new StreamWriter(new FileStream("Config.ini", FileMode.Create, FileAccess.ReadWrite)))
            {
                string configString = $"{MaxResultPerPage},{HideUnrated},{ShowOrder}";
                configSr.Write(configString);
            }

            ReadConfig();
        }

        public static void SetInitialConfig()
        {
            FileStream configFile = new FileStream("Config.ini", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter configSr = new StreamWriter(configFile);
            string configString = $"30,False,None";
            configSr.Write(configString);
            configSr.Close();
            configFile.Close();
            ReadConfig();
        }
    }
}
