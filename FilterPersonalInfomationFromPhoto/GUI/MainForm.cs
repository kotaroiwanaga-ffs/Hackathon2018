using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FilterPersonalInfomation;
using System.IO;

namespace GUI
{
    public partial class MainForm : Form
    {
        private OpenFileDialog ofd;

        public MainForm()
        {
            InitializeComponent();
            this.ofd = new OpenFileDialog();

            ofd.InitialDirectory = @"C:\OpenPose\sample\sample";
            ofd.Filter = "画像ファイル(*.png,*.jpg,*.bmp,*.gif)|*.png;*.jpg;*.bmp;*.gif|すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;
        }


        private void fileDialogue_button_Click(object sender, EventArgs e)
        {
            

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath_textBox.Text = ofd.FileName;
                ofd.InitialDirectory = Path.GetDirectoryName(ofd.FileName);

                try
                {
                    selectedImage_pictureBox.ImageLocation = ofd.FileName;
                    Image image = Image.FromFile(ofd.FileName);
                    filtering_button.Enabled = true;

                    selectedImage_pictureBox.Width = (int)(image.Width * ((double)selectedImage_pictureBox.Height / image.Height));
                    selectedImage_pictureBox.Left = (int)((this.Width - selectedImage_pictureBox.Width) / 2);

                    //selectedImage_pictureBox.Width = 300;

                }
                catch (Exception)

                {
                    filtering_button.Enabled = false;
                    MessageBox.Show("選択したファイルが不正です");
                }

            }
        }

        private void filtering_button_Click(object sender, EventArgs e)
        {
            string filteredFilePath = filePath_textBox.Text;

            if (eyeReflection_checkBox.Checked)
            {
                // 目の反射のフィルタリング
                filteredFilePath = FilterReflectedGlare.DrawFilteredReflectedGlare(filteredFilePath);
            }

            if (fingerPrint_checkBox.Checked)
            {
                // 指紋のフィルタリング
                filteredFilePath = FilterFingerPrint.DrawFilteredFingerPrint(filteredFilePath);
            }

            if (word_checkBox.Checked)
            {
                // 文字のフィルタリング
                //filteredFilePath = filterWord(filteredFilterPath);
            }

            selectedImage_pictureBox.ImageLocation = filteredFilePath;
            filePath_textBox.Text = filteredFilePath;
        }
    }
}
