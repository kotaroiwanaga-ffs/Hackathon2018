namespace GUI
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.selectedImage_pictureBox = new System.Windows.Forms.PictureBox();
            this.fileDialogue_button = new System.Windows.Forms.Button();
            this.filePath_textBox = new System.Windows.Forms.TextBox();
            this.filtering_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.selectedImage_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // selectedImage_pictureBox
            // 
            this.selectedImage_pictureBox.Location = new System.Drawing.Point(93, 68);
            this.selectedImage_pictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.selectedImage_pictureBox.Name = "selectedImage_pictureBox";
            this.selectedImage_pictureBox.Size = new System.Drawing.Size(400, 250);
            this.selectedImage_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.selectedImage_pictureBox.TabIndex = 0;
            this.selectedImage_pictureBox.TabStop = false;
            // 
            // fileDialogue_button
            // 
            this.fileDialogue_button.Location = new System.Drawing.Point(417, 370);
            this.fileDialogue_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fileDialogue_button.Name = "fileDialogue_button";
            this.fileDialogue_button.Size = new System.Drawing.Size(75, 22);
            this.fileDialogue_button.TabIndex = 1;
            this.fileDialogue_button.Text = "選択";
            this.fileDialogue_button.UseVisualStyleBackColor = true;
            this.fileDialogue_button.Click += new System.EventHandler(this.fileDialogue_button_Click);
            // 
            // filePath_textBox
            // 
            this.filePath_textBox.Location = new System.Drawing.Point(103, 370);
            this.filePath_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.filePath_textBox.Name = "filePath_textBox";
            this.filePath_textBox.ReadOnly = true;
            this.filePath_textBox.Size = new System.Drawing.Size(296, 22);
            this.filePath_textBox.TabIndex = 2;
            // 
            // filtering_button
            // 
            this.filtering_button.Enabled = false;
            this.filtering_button.Location = new System.Drawing.Point(417, 414);
            this.filtering_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.filtering_button.Name = "filtering_button";
            this.filtering_button.Size = new System.Drawing.Size(75, 22);
            this.filtering_button.TabIndex = 3;
            this.filtering_button.Text = "開始";
            this.filtering_button.UseVisualStyleBackColor = true;
            this.filtering_button.Click += new System.EventHandler(this.filtering_button_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 485);
            this.Controls.Add(this.filtering_button);
            this.Controls.Add(this.filePath_textBox);
            this.Controls.Add(this.fileDialogue_button);
            this.Controls.Add(this.selectedImage_pictureBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.selectedImage_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox selectedImage_pictureBox;
        private System.Windows.Forms.Button fileDialogue_button;
        private System.Windows.Forms.TextBox filePath_textBox;
        private System.Windows.Forms.Button filtering_button;
    }
}

