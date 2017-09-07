namespace WebCrawler_WinForm_
{
    partial class FindIdeal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindIdeal));
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.startPanel = new System.Windows.Forms.Panel();
            this.findCourseLabel = new System.Windows.Forms.Label();
            this.findTeacherLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.startPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(419, 119);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(177, 140);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(119, 119);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 140);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // startPanel
            // 
            this.startPanel.Controls.Add(this.findCourseLabel);
            this.startPanel.Controls.Add(this.findTeacherLabel);
            this.startPanel.Controls.Add(this.pictureBox1);
            this.startPanel.Controls.Add(this.pictureBox2);
            this.startPanel.Location = new System.Drawing.Point(12, 12);
            this.startPanel.Name = "startPanel";
            this.startPanel.Size = new System.Drawing.Size(720, 460);
            this.startPanel.TabIndex = 1;
            // 
            // findCourseLabel
            // 
            this.findCourseLabel.AutoSize = true;
            this.findCourseLabel.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.findCourseLabel.Location = new System.Drawing.Point(458, 303);
            this.findCourseLabel.Name = "findCourseLabel";
            this.findCourseLabel.Size = new System.Drawing.Size(90, 32);
            this.findCourseLabel.TabIndex = 1;
            this.findCourseLabel.Text = "找课程";
            // 
            // findTeacherLabel
            // 
            this.findTeacherLabel.AutoSize = true;
            this.findTeacherLabel.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.findTeacherLabel.Location = new System.Drawing.Point(161, 303);
            this.findTeacherLabel.Name = "findTeacherLabel";
            this.findTeacherLabel.Size = new System.Drawing.Size(90, 32);
            this.findTeacherLabel.TabIndex = 1;
            this.findTeacherLabel.Text = "找老师";
            this.findTeacherLabel.Click += new System.EventHandler(this.findTeacherLabel_Click);
            // 
            // FindIdeal
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(742, 483);
            this.Controls.Add(this.startPanel);
            this.Name = "FindIdeal";
            this.Text = "找老师/找课";
            this.Load += new System.EventHandler(this.FindIdeal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.startPanel.ResumeLayout(false);
            this.startPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel startPanel;
        private System.Windows.Forms.Label findCourseLabel;
        private System.Windows.Forms.Label findTeacherLabel;
    }
}