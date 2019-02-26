########### Python 3.2 #############

#概要
#・画像に対してfaceAPIを叩き、戻り値の内容を画像に反映して出力するプログラム
# py RemoveReflectedGlare.py [inputPath] [outputPath]
#
#詳細
#・使用するAPIレスポンス内容：左右の上下座標，左右の瞳孔の中心点
#
#TODO：
#・瞳領域の検出方法検討
#・瞳領域の加工方法
#
#


import http.client, urllib.request, urllib.parse, urllib.error, base64
import json
import cv2
import sys

#### Request headers
#'Content-Type': APIに送るメディアのタイプ. 
#  'application/json'(URL指定の場合), 'application/octet-stream' (Local ファイル転送の場合)
#'Ocp-Apim-Subscription-Key': APIキーを指定する
headers = {
#    'Content-Type': 'application/json',
    'Content-Type': 'application/octet-stream',
    'Ocp-Apim-Subscription-Key': '',
}
 
#### Request parameters
# 取得したい情報について、パラメータを指定する
# 指定できるパラメータは以下で、コンマで分けて複数指定可能
#    age, gender, headPose, smile, facialHair, glasses, emotion, hair, makeup, occlusion, accessories, blur, exposure and noise
params = urllib.parse.urlencode({
    'returnFaceId': 'true', #入力した顔画像に付与されるIDを返すかどうか
    'returnFaceLandmarks': 'true', #目や口などの特徴となる部分の座標を返すかどうか
    'returnFaceAttributes': 'age,gender,smile,facialHair,emotion' #認識した顔からわかる属性を返す
})
 
#### Request body
# 入力したい画像の指定をする. 画像URLの指定, local ファイルの指定から選択
# 画像はJPEG, PNG, GIF, BMPに対応
# サイズの上限は4MB
# 認識可能な顔のサイズは 36x36 - 4096x4096 pixelsの範囲
# 最大64個の顔を認識可能
 
## URL 指定の場合以下のコメントアウトを外すし、image_urlを指定する
#image_url = 'https://XXXXX'
#body = { 'url': image_url }
#body = json.dumps(body)
 
## Local file指定の場合
# 以下の image_file_path に読み込むファイルのパスを指定する
image_file_path = 'test.jpg'
output_file_path = 'test_out.jpg'
image_file = open(image_file_path,'rb')
body = image_file.read()
image_file.close()
 
#### API request
# 接続先リージョンによっては, 以下HTTPSConnection の "westus.api.cognitive.microsoft.com" 部分は変更する.
# この場合は「westus」なので北米西部リージョン
# なお "/face/v1.0/detect?%s" の部分が接続先APIの機能を指定している
def doAction(image_file_path):   
    try:
        image_file = open(image_file_path,'rb')
        body = image_file.read()
        image_file.close()
        conn = http.client.HTTPSConnection('japaneast.api.cognitive.microsoft.com')
        conn.request("POST", "/face/v1.0/detect?%s" % params, body, headers)
        response = conn.getresponse()
        data = json.loads(response.read())
        #print(data)
        conn.close()

        ### 画像の加工
        img = cv2.imread(image_file_path)
        # 検出した顔ごとに枠追加
        for face_info in data:
                
            landmarks = face_info["faceLandmarks"];
            # 瞳の枠描画
            retouchEye(img,landmarks)


            # cv2.rectangle(
            #     img
            #     , (int(landmarks["eyeLeftOuter"]["x"]), int(landmarks["eyeLeftTop"]["y"]))
            #     , (int(landmarks["eyeLeftInner"]["x"]), int(landmarks["eyeLeftBottom"]["y"])),             
            #     (255,255,255), 2, 16
            # )
            # cv2.rectangle(
            #     img
            #     , (int(landmarks["eyeRightOuter"]["x"]), int(landmarks["eyeRightTop"]["y"]))
            #     , (int(landmarks["eyeRightInner"]["x"]), int(landmarks["eyeRightBottom"]["y"])),             
            #     (255,255,255), 2, 16
            # )

        cv2.imwrite(output_file_path, img)

    except Exception as e:
        print("[Errno {0}] {1}".format(e.errno, e.strerror))
 
def retouchEye(img, landmarks):
    height = landmarks["eyeLeftBottom"]["y"] - landmarks["eyeLeftTop"]["y"]
    detectPupil(img, landmarks["pupilLeft"],int(height*0.9/2))
    detectPupil(img, landmarks["pupilRight"],int(height*0.9/2))


def detectPupil(img, pupil,radius):
    pup={}
    pup["x"] = int(pupil["x"])
    pup["y"] = int(pupil["y"])
    # cv2.rectangle(
    #     img, (pup["x"]-radius,pup["y"]-radius), (pup["x"]+radius,pup["y"]+radius),             
    #     (255,255,255), -1
    # )
    cv2.circle(
        img, (pup["x"],pup["y"]), radius,             
        (255,255,255), -1
    )
    cv2.rectangle(
        img, (pup["x"]-1,pup["y"]-1), (pup["x"]+1,pup["y"]+1),             
        (128,128,255), -1
    )
    print(str(pup["x"]) + " " + str(pup["y"]))
    return img
####################################

args = sys.argv  # コマンドライン引数を格納したリストの取得
# デバッグプリント
if len(args) > 1:
    image_file_path = args[1]
if len(args) > 2:
    output_file_path = args[2]
print (image_file_path)
print (output_file_path)


doAction(image_file_path)