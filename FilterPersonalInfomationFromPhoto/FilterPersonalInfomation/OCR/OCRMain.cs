using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;


// 追加
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using Microsoft.Win32;
using FilterPersonalInfomation;
using System.Drawing;
using System.Drawing.Imaging;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 

namespace FilterPersonalInfomation
{
    public class OCRMain
    {
        // ----------------------------------------------
        // サブスクリプション　キー
        // ----------------------------------------------
        static string accessKey = "f3d2bcfbd44442ba8bd612421c797b47";

        // ----------------------------------------------
        // 文字モザイクメイン関数
        // ----------------------------------------------
        public static void OCR_MAIN(string Input_ImgPath, string Output_ImgPath)
        {
            string input_imgpath = Input_ImgPath;           //参照イメージパス
            string output_imgpath = Output_ImgPath;         //書き出しイメージパス
            string language = "ja";                         //文字検出言語：日本語

            // ----------------------------------------------
            // 文字検出結果を保存
            // ----------------------------------------------

            try
            {   
                //文字検出結果を取得
                OCRResult result = Get_OCR_Result(accessKey, language, input_imgpath);

                //モザイク処理・出力
                Letter_Mosaic(result.regions, Input_ImgPath, Output_ImgPath);

            }
            catch (Exception exception)
            {
                //tboxResult.Text = exception.Message;
            }
            finally
            {
                

            }

        }




        // ----------------------------------------------
        // OCR機能API処理(文字検出関数)
        // ----------------------------------------------
        private static OCRResult Get_OCR_Result(string pAccessKey, string pLanguage, string pFile)
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
                //Stream responseStream = webException.Response.GetResponseStream();
                //StreamReader streamReader = new StreamReader(responseStream);
                //tboxResult.Text = streamReader.ReadToEnd();
                return null;
            }
            catch (Exception exception)
            {
                //tboxResult.Text = exception.Message;
                return null;
            }
        }

        // ----------------------------------------------
        // 画像回転
        // ----------------------------------------------        
        private void Img_Rotation(float Text_Angle)
        {
            float text_angle = Text_Angle;                            //画像角度
        }
        
        // ----------------------------------------------
        // モザイク処理
        // ----------------------------------------------
        private static void Letter_Mosaic(OCRResult.Region[] regions, string input_imgpath,string output_imgpath)
        {
            OCRResult.Region[] region = regions;

            string[] cordinate = regions[0].boundingBox.Split(',');

            int convert_intx, convert_inty, convert_intw, convert_inth;
            convert_intx = Convert.ToInt32(cordinate[0]);
            convert_inty = Convert.ToInt32(cordinate[1]);
            convert_intw = Convert.ToInt32(cordinate[2]);
            convert_inth = Convert.ToInt32(cordinate[3]);


            //描画先とするImageオブジェクトを作成する
            //Bitmap canvas = new Bitmap(convert_intw, convert_inth);

            //画像ファイルのImageオブジェクトを作成する
            Bitmap img = new Bitmap(input_imgpath);

            //切り取る部分の範囲を決定する。
            Rectangle srcRect = new Rectangle(convert_intx, convert_intw, convert_intw, convert_inth);

            //描画する部分の範囲を決定する。ここでは、位置(0,0)、大きさ100x100で描画する
            Rectangle desRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);

            //切り取った部分をモザイク

            Bitmap bmp = img.Clone(desRect, img.PixelFormat);

            //Bitmap after_bmp = img.Clone(srcRect, img.PixelFormat);

            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(bmp);

            g.Clear(Color.White);

            Graphics h = Graphics.FromImage(img);
            
            //画像の一部を描画する
            h.DrawImage(bmp, convert_intx, convert_inty);

            //Graphicsオブジェクトのリソースを解放する
            h.Dispose();

            //画像出力
            img.Save(output_imgpath, ImageFormat.Jpeg);
        }



        //private void DrawImage(Image prm_img, int x, int y)
        //{
        //    // グラフィック用オブジェクトを生成
        //    Graphics gr = .CreateGraphics();

        //    // 画像の描画
        //    gr.DrawImage(prm_img, new Point(x, y));
        //}

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    //Graphicsオブジェクトの取得
        //    Graphics g = e.Graphics;
        //    //ペンオブジェクトの作成
        //    Pen p = new Pen(Color.Black);

        //    //直線を描画
        //    g.DrawLine(p, 0, 0, 20, 10);

        //    //SmoothingModeにAntiAlias
        //    //(アンチエイリアス処理されたレタリング)を指定する
        //    g.SmoothingMode = SmoothingMode.AntiAlias;
        //    //直線を描画
        //    g.DrawLine(p, 10, 0, 30, 10);

        //    //SmoothingModeにDefault
        //    //(既定のモード)を指定する
        //    g.SmoothingMode = SmoothingMode.Default;
        //    //直線を描画
        //    g.DrawLine(p, 20, 0, 40, 10);

        //    //SmoothingModeにHighQuality
        //    //(高品質で低速なレンダリング)を指定する
        //    g.SmoothingMode = SmoothingMode.HighQuality;
        //    //直線を描画
        //    g.DrawLine(p, 30, 0, 50, 10);

        //    //SmoothingModeにHighSpeed
        //    //(高速で、低品質のレンダリング)を指定する
        //    g.SmoothingMode = SmoothingMode.HighSpeed;
        //    //直線を描画
        //    g.DrawLine(p, 40, 0, 60, 10);

        //    //SmoothingModeにNone
        //    //(アンチエイリアス処理しない)を指定する
        //    g.SmoothingMode = SmoothingMode.None;
        //    //直線を描画
        //    g.DrawLine(p, 50, 0, 70, 10);

        //    p.Dispose();
        //}







        //// ----------------------------------------------
        //// 画像ファイル選択ボタンクリック時処理
        //// ----------------------------------------------
        //private void btnSelectImageFile_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        OpenFileDialog openFileDialog = new OpenFileDialog();
        //        openFileDialog.CheckFileExists = true;
        //        openFileDialog.FileName = "";
        //        openFileDialog.Filter = "画像ファイル|*.jpg;*jpeg;*.png;*.gif;*.bmp";
        //        openFileDialog.FilterIndex = 0;

        //        if (openFileDialog.ShowDialog() == true)
        //        {
        //            imgOCR.Source = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.RelativeOrAbsolute));
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        tboxResult.Text = exception.Message;
        //    }
        //}

        //// ----------------------------------------------
        //// OKボタンクリック時処理
        //// ----------------------------------------------
        //private void btnOK_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (imgOCR.Source == null) return;

        //        this.IsEnabled = false;
        //        this.Cursor = Cursors.Wait;

        //        tboxResult.Text = "";

        //        string ComboBox_language = ((ComboBoxItem)cmbLanguage.SelectedItem).Content.ToString();

        //        //コンボボックス内の指定言語を変換
        //        string language;
        //        if (ComboBox_language == "日本語")
        //        {
        //            language = "ja";
        //        }
        //        else if (ComboBox_language == "英語")
        //        {
        //            language = "en";
        //        }
        //        else
        //        {
        //            language = "unk";
        //        }

        //        string file = ((BitmapImage)imgOCR.Source).UriSource.LocalPath;
        //        OCRResult result = GetOCRResult(accessKey, language, file);

        //        if (result.regions.Count() == 0) tboxResult.Text = "文字が見つかりませんでした。";

        //        foreach (OCRResult.Region region in result.regions)
        //        {
        //            foreach (OCRResult.Line line in region.lines)
        //            {
        //                foreach (OCRResult.Word word in line.words)
        //                {
        //                    //テキストボックスに追加
        //                    tboxResult.Text += word.text;
        //                    if (result.language == "en") tboxResult.Text += " ";
        //                }
        //                tboxResult.Text += "\r\n";
        //            }
        //            tboxResult.Text += "\r\n";
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        tboxResult.Text = exception.Message;
        //    }
        //    finally
        //    {
        //        this.IsEnabled = true;
        //        this.Cursor = Cursors.Arrow;

        //    }

        //}

        //// ----------------------------------------------
        //// OCR機能API処理
        //// ----------------------------------------------
        //private OCRResult GetOCRResult(string pAccessKey, string pLanguage, string pFile)
        //{
        //    try
        //    {
        //        string url = "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/ocr";
        //        if (pLanguage != null && pLanguage != "") url += "?language=" + pLanguage;

        //        // リクエスト作成
        //        HttpWebRequest request = WebRequest.CreateHttp(url);
        //        request.Method = "POST";
        //        request.ContentType = "application/octet-stream";
        //        request.Headers.Add("Ocp-Apim-Subscription-Key", pAccessKey);

        //        // 画像ファイルの読み込み
        //        FileStream fs = new FileStream(pFile, FileMode.Open, FileAccess.Read);
        //        byte[] bs = new byte[fs.Length];
        //        fs.Read(bs, 0, bs.Length);
        //        fs.Close();

        //        Stream writer = request.GetRequestStream();
        //        writer.Write(bs, 0, bs.Length);

        //        WebResponse webResponse = request.GetResponse();
        //        Stream responseStream = webResponse.GetResponseStream();

        //        DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(OCRResult));
        //        OCRResult result = (OCRResult)dcjs.ReadObject(responseStream);

        //        return result;

        //    }
        //    catch (WebException webException)
        //    {
        //        Stream responseStream = webException.Response.GetResponseStream();
        //        StreamReader streamReader = new StreamReader(responseStream);
        //        tboxResult.Text = streamReader.ReadToEnd();
        //        return null;
        //    }
        //    catch (Exception exception)
        //    {
        //        tboxResult.Text = exception.Message;
        //        return null;
        //    }

        //}

        //// ----------------------------------------------
        //// モザイク処理
        //// ----------------------------------------------
    }
}
