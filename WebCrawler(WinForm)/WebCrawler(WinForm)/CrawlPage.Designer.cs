namespace WebCrawler_WinForm_
{
    partial class CrawlPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrawlPage));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.FinishButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.FailLogBox = new System.Windows.Forms.ListBox();
            this.UpdateProcessList = new System.Windows.Forms.ListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(482, 475);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(607, 475);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 32);
            this.button2.TabIndex = 0;
            this.button2.Text = "停止";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FinishButton
            // 
            this.FinishButton.Location = new System.Drawing.Point(721, 475);
            this.FinishButton.Name = "FinishButton";
            this.FinishButton.Size = new System.Drawing.Size(97, 32);
            this.FinishButton.TabIndex = 0;
            this.FinishButton.Text = "完成";
            this.FinishButton.UseVisualStyleBackColor = true;
            this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Font = new System.Drawing.Font("微软雅黑", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox1.Location = new System.Drawing.Point(44, 478);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(134, 27);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "显示更新进程";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Font = new System.Drawing.Font("微软雅黑", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox2.Location = new System.Drawing.Point(223, 478);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(134, 27);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "显示失败日志";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // FailLogBox
            // 
            this.FailLogBox.FormattingEnabled = true;
            this.FailLogBox.ItemHeight = 15;
            this.FailLogBox.Items.AddRange(new object[] {
            "失败日志"});
            this.FailLogBox.Location = new System.Drawing.Point(440, 27);
            this.FailLogBox.Name = "FailLogBox";
            this.FailLogBox.Size = new System.Drawing.Size(378, 379);
            this.FailLogBox.TabIndex = 2;
            // 
            // UpdateProcessList
            // 
            this.UpdateProcessList.FormattingEnabled = true;
            this.UpdateProcessList.ItemHeight = 15;
            this.UpdateProcessList.Items.AddRange(new object[] {
            "更新进程"});
            this.UpdateProcessList.Location = new System.Drawing.Point(44, 27);
            this.UpdateProcessList.Name = "UpdateProcessList";
            this.UpdateProcessList.Size = new System.Drawing.Size(377, 379);
            this.UpdateProcessList.TabIndex = 2;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(44, 423);
            this.progressBar1.Maximum = 3736;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(774, 35);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 3;
            // 
            // CrawlPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 527);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.UpdateProcessList);
            this.Controls.Add(this.FailLogBox);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.FinishButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CrawlPage";
            this.Text = "离线数据更新";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CrawlPage_FormClosing);
            this.Load += new System.EventHandler(this.CrawlPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.ListBox FailLogBox;
        public System.Windows.Forms.ListBox UpdateProcessList;
        public System.Windows.Forms.Button FinishButton;
        public System.Windows.Forms.CheckBox checkBox1;
        public System.Windows.Forms.CheckBox checkBox2;
        public System.Windows.Forms.ProgressBar progressBar1;
    }
}