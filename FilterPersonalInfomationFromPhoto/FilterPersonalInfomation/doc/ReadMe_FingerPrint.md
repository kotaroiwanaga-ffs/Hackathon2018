### FilterFingerPrint.cs を使うための準備

1. **OpenPoseダウンロードしzip解凍**   
こちらからダウンロード↓  
https://github.com/CMU-Perceptual-Computing-Lab/openpose/releases/download/v1.4.0/openpose-1.4.0-win64-cpu-binaries.zip  
GitHubのページ  
https://github.com/CMU-Perceptual-Computing-Lab/openpose/blob/master/doc/output.md  

1. **pythonのインストール**  
python3ならたぶん大丈夫  [インストール手順](https://www.python.jp/install/windows/install_py3.html)  
windows 64bit なら[こちら](https://www.python.org/ftp/python/3.6.8/python-3.6.8-amd64.exe)  
  
1. FingerPrintConfig.xmlにパスを入力 (そのうち省略できるようにしたい)
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
