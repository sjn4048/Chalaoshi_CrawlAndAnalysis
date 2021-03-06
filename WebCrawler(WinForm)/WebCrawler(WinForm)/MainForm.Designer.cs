﻿namespace WebCrawler_WinForm_
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.DBlabel = new System.Windows.Forms.Label();
            this.SearchLabel = new System.Windows.Forms.Label();
            this.UpdateLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.SettingBox = new System.Windows.Forms.PictureBox();
            this.CrawlBox = new System.Windows.Forms.PictureBox();
            this.SearchBox = new System.Windows.Forms.PictureBox();
            this.DbBox = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrawlBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DbBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DBlabel
            // 
            this.DBlabel.AutoSize = true;
            this.DBlabel.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DBlabel.ForeColor = System.Drawing.SystemColors.InfoText;
            this.DBlabel.Location = new System.Drawing.Point(414, 236);
            this.DBlabel.Name = "DBlabel";
            this.DBlabel.Size = new System.Drawing.Size(127, 36);
            this.DBlabel.TabIndex = 5;
            this.DBlabel.Text = "趣味榜单";
            this.DBlabel.Click += new System.EventHandler(this.DbBox_Click);
            // 
            // SearchLabel
            // 
            this.SearchLabel.AutoSize = true;
            this.SearchLabel.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SearchLabel.ForeColor = System.Drawing.SystemColors.InfoText;
            this.SearchLabel.Location = new System.Drawing.Point(131, 236);
            this.SearchLabel.Name = "SearchLabel";
            this.SearchLabel.Size = new System.Drawing.Size(127, 36);
            this.SearchLabel.TabIndex = 5;
            this.SearchLabel.Text = "离线搜索";
            this.SearchLabel.Click += new System.EventHandler(this.SearchBox_Click);
            // 
            // UpdateLabel
            // 
            this.UpdateLabel.AutoSize = true;
            this.UpdateLabel.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UpdateLabel.ForeColor = System.Drawing.SystemColors.InfoText;
            this.UpdateLabel.Location = new System.Drawing.Point(131, 510);
            this.UpdateLabel.Name = "UpdateLabel";
            this.UpdateLabel.Size = new System.Drawing.Size(127, 36);
            this.UpdateLabel.TabIndex = 5;
            this.UpdateLabel.Text = "更新数据";
            this.UpdateLabel.Click += new System.EventHandler(this.CrawlBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label1.Location = new System.Drawing.Point(439, 510);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 36);
            this.label1.TabIndex = 5;
            this.label1.Text = "设置";
            this.label1.Click += new System.EventHandler(this.SettingBox_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Menu;
            this.statusStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI Light", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 611);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(666, 28);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // SettingBox
            // 
            this.SettingBox.Image = global::WebCrawler_WinForm_.Properties.Resources.Settings;
            this.SettingBox.Location = new System.Drawing.Point(402, 343);
            this.SettingBox.Name = "SettingBox";
            this.SettingBox.Size = new System.Drawing.Size(151, 164);
            this.SettingBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.SettingBox.TabIndex = 4;
            this.SettingBox.TabStop = false;
            this.SettingBox.Click += new System.EventHandler(this.SettingBox_Click);
            // 
            // CrawlBox
            // 
            this.CrawlBox.Image = global::WebCrawler_WinForm_.Properties.Resources.Update;
            this.CrawlBox.Location = new System.Drawing.Point(115, 343);
            this.CrawlBox.Name = "CrawlBox";
            this.CrawlBox.Size = new System.Drawing.Size(151, 164);
            this.CrawlBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CrawlBox.TabIndex = 4;
            this.CrawlBox.TabStop = false;
            this.CrawlBox.Click += new System.EventHandler(this.CrawlBox_Click);
            // 
            // SearchBox
            // 
            this.SearchBox.Image = global::WebCrawler_WinForm_.Properties.Resources.搜索_矢量;
            this.SearchBox.Location = new System.Drawing.Point(115, 69);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(151, 164);
            this.SearchBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.SearchBox.TabIndex = 3;
            this.SearchBox.TabStop = false;
            this.SearchBox.Click += new System.EventHandler(this.SearchBox_Click);
            // 
            // DbBox
            // 
            this.DbBox.Image = global::WebCrawler_WinForm_.Properties.Resources.数据库;
            this.DbBox.Location = new System.Drawing.Point(412, 69);
            this.DbBox.Name = "DbBox";
            this.DbBox.Size = new System.Drawing.Size(138, 164);
            this.DbBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DbBox.TabIndex = 2;
            this.DbBox.TabStop = false;
            this.DbBox.Click += new System.EventHandler(this.DbBox_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(295, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "新功能入口1";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Microsoft YaHei UI Light", 10F);
            this.toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(165, 23);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(666, 639);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UpdateLabel);
            this.Controls.Add(this.SearchLabel);
            this.Controls.Add(this.DBlabel);
            this.Controls.Add(this.SettingBox);
            this.Controls.Add(this.CrawlBox);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.DbBox);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "选课助手";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrawlBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DbBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox DbBox;
        private System.Windows.Forms.PictureBox SearchBox;
        private System.Windows.Forms.PictureBox CrawlBox;
        private System.Windows.Forms.Label DBlabel;
        private System.Windows.Forms.Label SearchLabel;
        private System.Windows.Forms.Label UpdateLabel;
        private System.Windows.Forms.PictureBox SettingBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

