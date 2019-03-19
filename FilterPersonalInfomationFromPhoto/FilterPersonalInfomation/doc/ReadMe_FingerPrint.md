## FilterFingerPrint.cs を使うための準備

### OpenPoseの設定
1. OpenPoseのダウンロード
こちらからダウンロード↓  
https://github.com/CMU-Perceptual-Computing-Lab/openpose/releases/download/v1.4.0/openpose-1.4.0-win64-cpu-binaries.zip  
GitHubのページ  
https://github.com/CMU-Perceptual-Computing-Lab/openpose/blob/master/doc/output.md  

1. ダウンロードしたzipファイルを解凍し、空白のないパス以下に配置  

1. モデルの生成  
models\getModels.batを実行する  

**ここまでできたらOpenPoseDemo.exeが動くか確認してみる**  
参考
- コマンド
https://qiita.com/wada-n/items/c747525f96efb6cb9a35  
https://qiita.com/wada-n/items/e9e6653effc1e3d0c566  
- 導入手順
https://qiita.com/miu200521358/items/539aaa63f16869191508


### pythonのインストール 
python3ならたぶん大丈夫  [インストール手順](https://www.python.jp/install/windows/install_py3.html)  
windows 64bit なら[こちら](https://www.python.org/ftp/python/3.6.8/python-3.6.8-amd64.exe)  
  
### FingerPrintConfig.xmlにパスを入力 (そのうち省略できるようにしたい)
```
<?xml version="1.0" encoding="utf-8" ?>

<Path>
  <exe_openpose_bat>ここにexe_openpose.batのパスを記入</exe_openpose_bat>
  
  <openpose>ここにOpenPoseのパスを記入</openpose>

  <python>ここにpython.exeのパスを記入</python>

  <analyze_json_python>ここにanalyze_json.pyのパスを記入</analyze_json_python>
  
</Path>
```
※OpenPoseのパス：binの親ディレクトリ(Cドライブでzip解凍してそのままならC:\openpose-1.4.0-win64-cpu-binaries\openpose-1.4.0-win64-cpu-binariesとか)
