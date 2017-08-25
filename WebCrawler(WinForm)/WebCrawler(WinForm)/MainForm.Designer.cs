namespace WebCrawler_WinForm_
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
            this.DBlabel = new System.Windows.Forms.Label();
            this.SearchLabel = new System.Windows.Forms.Label();
            this.UpdateLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SettingBox = new System.Windows.Forms.PictureBox();
            this.CrawlBox = new System.Windows.Forms.PictureBox();
            this.SearchBox = new System.Windows.Forms.PictureBox();
            this.DbBox = new System.Windows.Forms.PictureBox();
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
            this.DBlabel.Location = new System.Drawing.Point(111, 241);
            this.DBlabel.Name = "DBlabel";
            this.DBlabel.Size = new System.Drawing.Size(155, 36);
            this.DBlabel.TabIndex = 5;
            this.DBlabel.Text = "查看数据库";
            this.DBlabel.Click += new System.EventHandler(this.DbBox_Click);
            // 
            // SearchLabel
            // 
            this.SearchLabel.AutoSize = true;
            this.SearchLabel.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SearchLabel.ForeColor = System.Drawing.SystemColors.InfoText;
            this.SearchLabel.Location = new System.Drawing.Point(425, 241);
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
            this.UpdateLabel.Location = new System.Drawing.Point(126, 489);
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
            this.label1.Location = new System.Drawing.Point(455, 489);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 36);
            this.label1.TabIndex = 5;
            this.label1.Text = "设置";
            this.label1.Click += new System.EventHandler(this.SettingBox_Click);
            // 
            // SettingBox
            // 
            this.SettingBox.Image = global::WebCrawler_WinForm_.Properties.Resources.Settings;
            this.SettingBox.Location = new System.Drawing.Point(418, 322);
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
            this.CrawlBox.Location = new System.Drawing.Point(115, 322);
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
            this.SearchBox.Location = new System.Drawing.Point(418, 74);
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
            this.DbBox.Location = new System.Drawing.Point(128, 74);
            this.DbBox.Name = "DbBox";
            this.DbBox.Size = new System.Drawing.Size(138, 164);
            this.DbBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DbBox.TabIndex = 2;
            this.DbBox.TabStop = false;
            this.DbBox.Click += new System.EventHandler(this.DbBox_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(740, 585);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UpdateLabel);
            this.Controls.Add(this.SearchLabel);
            this.Controls.Add(this.DBlabel);
            this.Controls.Add(this.SettingBox);
            this.Controls.Add(this.CrawlBox);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.DbBox);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.Name = "MainForm";
            this.Text = "查老师助手， 离线查询&爬取数据";
            this.Load += new System.EventHandler(this.MainForm_Load);
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
    }
}

