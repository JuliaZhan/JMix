namespace WindowsServiceClient
{
    partial class Form1
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
            this.installBtn = new System.Windows.Forms.Button();
            this.startBtn = new System.Windows.Forms.Button();
            this.endBtn = new System.Windows.Forms.Button();
            this.unstallBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // installBtn
            // 
            this.installBtn.Location = new System.Drawing.Point(54, 64);
            this.installBtn.Name = "installBtn";
            this.installBtn.Size = new System.Drawing.Size(125, 43);
            this.installBtn.TabIndex = 0;
            this.installBtn.Text = "安装服务";
            this.installBtn.UseVisualStyleBackColor = true;
            this.installBtn.Click += new System.EventHandler(this.installBtn_Click);
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(212, 64);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(99, 43);
            this.startBtn.TabIndex = 1;
            this.startBtn.Text = "启动服务";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // endBtn
            // 
            this.endBtn.Location = new System.Drawing.Point(357, 64);
            this.endBtn.Name = "endBtn";
            this.endBtn.Size = new System.Drawing.Size(106, 43);
            this.endBtn.TabIndex = 2;
            this.endBtn.Text = "停止服务";
            this.endBtn.UseVisualStyleBackColor = true;
            this.endBtn.Click += new System.EventHandler(this.endBtn_Click);
            // 
            // unstallBtn
            // 
            this.unstallBtn.Location = new System.Drawing.Point(510, 64);
            this.unstallBtn.Name = "unstallBtn";
            this.unstallBtn.Size = new System.Drawing.Size(122, 43);
            this.unstallBtn.TabIndex = 3;
            this.unstallBtn.Text = "卸载服务";
            this.unstallBtn.UseVisualStyleBackColor = true;
            this.unstallBtn.Click += new System.EventHandler(this.unstallBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 190);
            this.Controls.Add(this.unstallBtn);
            this.Controls.Add(this.endBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.installBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button installBtn;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button endBtn;
        private System.Windows.Forms.Button unstallBtn;
    }
}

