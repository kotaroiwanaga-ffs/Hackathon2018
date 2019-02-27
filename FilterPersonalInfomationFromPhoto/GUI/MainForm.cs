using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FilterPersonalInfomation;

namespace GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void fileDialogue_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "画像ファイル(*.png,*.jpg,*.bmp,*.gif)|*.png;*.jpg;*.bmp;*.gif|すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath_textBox.Text = ofd.FileName;
                try
                {
                    selectedImage_pictureBox.ImageLocation = ofd.FileName;
                    Image image = Image.FromFile(ofd.FileName);
                    filtering_button.Enabled = true;

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
                //filteredFilePath = filterEyeReflection(filteredFilePath);
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
