using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// 追加
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using Microsoft.Win32;

namespace ComputerVisionAPI
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        // ----------------------------------------------
        // サブスクリプション　キー
        // ----------------------------------------------
        string accessKey = "f3d2bcfbd44442ba8bd612421c797b47";


        // ----------------------------------------------
        // コンストラクタ
        // ----------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
        }

        // ----------------------------------------------
        // 画像ファイル選択ボタンクリック時処理
        // ----------------------------------------------
        private void btnSelectImageFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.CheckFileExists = true;
                openFileDialog.FileName = "";
                openFileDialog.Filter = "画像ファイル|*.jpg;*jpeg;*.png;*.gif;*.bmp";
                openFileDialog.FilterIndex = 0;

                if (openFileDialog.ShowDialog() == true)
                {
                    imgOCR.Source = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception exception)
            {
                tboxResult.Text = exception.Message;
            }
        }

        // ----------------------------------------------
        // OKボタンクリック時処理
        // ----------------------------------------------
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (imgOCR.Source == null) return;

                this.IsEnabled = false;
                this.Cursor = Cursors.Wait;

                tboxResult.Text = "";

                string ComboBox_language = ((ComboBoxItem)cmbLanguage.SelectedItem).Content.ToString();

                //コンボボックス内の指定言語を変換
                string language;
                if (ComboBox_language == "日本語")
                {
                    language = "ja";
                }
                else if (ComboBox_language == "英語")
                {
                    language = "en";
                }
                else
                {
                    language = "unk";
                }

                string file = ((BitmapImage)imgOCR.Source).UriSource.LocalPath;
                OCRResult result = GetOCRResult(accessKey, language, file);

                if (result.regions.Count() == 0) tboxResult.Text = "文字が見つかりませんでした。";

                foreach (OCRResult.Region region in result.regions)
                {
                    foreach (OCRResult.Line line in region.lines)
                    {
                        foreach (OCRResult.Word word in line.words)
                        {
                            //テキストボックスに追加
                            tboxResult.Text += word.text;
                            if (result.language == "en") tboxResult.Text += " ";
                        }
                        tboxResult.Text += "\r\n";
                    }
                    tboxResult.Text += "\r\n";
                }
            }
            catch (Exception exception)
            {
                tboxResult.Text = exception.Message;
            }
            finally
            {
                this.IsEnabled = true;
                this.Cursor = Cursors.Arrow;

            }

        }

        // ----------------------------------------------
        // OCR機能API処理
        // ----------------------------------------------
        private OCRResult GetOCRResult(string pAccessKey, string pLanguage, string pFile)
        {
            try
            {
                string url = "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/ocr";
                if (pLanguage != null && pLanguage != "") url += "?language=" + pLanguage;

                // リクエスト作成
                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.Method = "POST";
                request.ContentType = "application/octet-stream";
                request.Headers.Add("Ocp-Apim-Subscription-Key", pAccessKey);

                // 画像ファイルの読み込み
                FileStream fs = new FileStream(pFile, FileMode.Open, FileAccess.Read);
                byte[] bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
                fs.Close();

                Stream writer = request.GetRequestStream();
                writer.Write(bs, 0, bs.Length);

                WebResponse webResponse = request.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();

                DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(OCRResult));
                OCRResult result = (OCRResult)dcjs.ReadObject(responseStream);

                return result;

            }
            catch (WebException webException)
            {
                Stream responseStream = webException.Response.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                tboxResult.Text = streamReader.ReadToEnd();
                return null;
            }
            catch (Exception exception)
            {
                tboxResult.Text = exception.Message;
                return null;
            }

        }

        // ----------------------------------------------
        // モザイク処理
        // ----------------------------------------------






    }
}