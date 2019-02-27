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
import numpy as np
import sys
import os
import xml.etree.ElementTree as ET

base = os.path.dirname(os.path.abspath(__file__))
name = os.path.normpath(os.path.join(base, '../local/local_db'))

### json読み込み
# filePath = os.path.normpath(os.path.join(base, './ReflectedGlareConfig.json'))
# f = open(filePath, 'r')
# config = json.load(f) #JSON形式で読み込む

### xml読み込み
filePath = os.path.normpath(os.path.join(base, './ReflectedGlareConfig.xml'))
tree = ET.parse(filePath)
root = tree.getroot()

#### Request headers
#'Content-Type': APIに送るメディアのタイプ. 
#  'application/json'(URL指定の場合), 'application/octet-stream' (Local ファイル転送の場合)
#'Ocp-Apim-Subscription-Key': APIキーを指定する
headers = {
#    'Content-Type': 'application/json',
    'Content-Type': 'application/octet-stream',
    'Ocp-Apim-Subscription-Key': root.find("AzureKey").text,
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
image_file_path = os.path.normpath(os.path.join(base, 'test.jpg'))
output_file_path = os.path.normpath(os.path.join(base, 'test_out.jpg'))
 
#### API request
# 接続先リージョンによっては, 以下HTTPSConnection の "westus.api.cognitive.microsoft.com" 部分は変更する.
# この場合は「westus」なので北米西部リージョン
# なお "/face/v1.0/detect?%s" の部分が接続先APIの機能を指定している
def main(image_file_path):   
    try:
        #ファイルを読み込みfaceAPIを叩く
        image_file = open(image_file_path,'rb')
        body = image_file.read()
        image_file.close()
        conn = http.client.HTTPSConnection(root.find("ApiHost").text)
        conn.request("POST", root.find("ApiPath").text + "?%s" % params, body, headers)
        response = conn.getresponse()
        data = json.loads(response.read())
        conn.close()

        ### 画像の加工
        img = cv2.imread(image_file_path)
        # 検出した顔ごとに枠追加
        for face_info in data:
                
            landmarks = face_info["faceLandmarks"];
            # 瞳の枠描画
            img = retouchEye(img, landmarks)

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
    radius = int(height*1/2)
    img = detectPupil(img, landmarks["pupilLeft"],radius)
    img = detectPupil(img, landmarks["pupilRight"],radius)
    return img


def detectPupil(img, pupil,radius):
    ###目の座標の抽出
    pup={}
    pup["x"] = int(pupil["x"])
    pup["y"] = int(pupil["y"])
    # cv2.rectangle(
    #     img, (pup["x"]-radius,pup["y"]-radius), (pup["x"]+radius,pup["y"]+radius),             
    #     (255,255,255), -1
    # )
    print ("pix:" + str(img[pup["y"],pup["x"]]) + " radius:" + str(radius))
    
    roi = (pup["x"] - radius, pup["x"] + radius, pup["y"] - radius, pup["y"] + radius)
    print (roi)
    gauss = int(radius / 2) * 2 -1
    

    for i in range(1,radius+2,3):
        print(range(pup["y"]-i,pup["y"]+i))
        gauss = int( (radius - i) / 2) * 2 + 3
        print("gauss:"+str(gauss))
        img[pup["y"]-i : pup["y"]+i, pup["x"]-i : pup["x"]+i] = cv2.GaussianBlur(img[pup["y"]-i : pup["y"]+i, pup["x"]-i : pup["x"]+i], (gauss, gauss), 0)
    ### 画像ぼかし本処理
    #img[roi[2] : roi[3], roi[0] : roi[1]] = cv2.GaussianBlur(img[roi[2] : roi[3], roi[0] : roi[1]], (5,5), 0)
    #img[ roi[2] : roi[3], roi[0] : roi[1]] = cv2.medianBlur(img[ roi[2] : roi[3], roi[0] : roi[1]], i*2+1)

    #img[ roi[2]+2 : roi[3]-2, roi[0]+2 : roi[1]-2] = cv2.GaussianBlur(img[ roi[2]+2 : roi[3]-2, roi[0]+2 : roi[1]-2],(gauss,gauss), 0)
    #img[roi[2] : roi[3], roi[0] : roi[1]] = cv2.GaussianBlur(img[roi[2] : roi[3], roi[0] : roi[1]], (3, 3), 0)
    #for i in range(5):
    #    )
    #     #img[roi[0] : roi[1], roi[2] : roi[3]] = cv2.GaussianBlur(img[roi[0] : roi[1], roi[2] : roi[3]], (11, 11),0)
        
    mid = {}
    mid[0] = np.median(img[roi[0] : roi[1], roi[2] : roi[3], 0])
    mid[1] = np.median(img[roi[0] : roi[1], roi[2] : roi[3], 1])
    mid[2] = np.median(img[roi[0] : roi[1], roi[2] : roi[3], 2])

    ### 画像平均
    #img[roi[0] : roi[1], roi[2] : roi[3]] = np.median(img[roi[0] : roi[1], roi[2] : roi[3]])
    # for i in range(roi[0], roi[1]):
    #     for j in range(roi[2], roi[3]):
    #         for k in range(0, 3):
    #             img[j, i, k] = mid[k];

    ### 領域囲みテスト用
    # cv2.rectangle(
    #     img, (roi[0],roi[2]), (roi[1],roi[3]),             
    #     (128,128,255), 1
    # )
    # cv2.circle(
    #     img, (pup["x"],pup["y"]), radius,             
    #     (0,0,0), -1
    # )
    # cv2.rectangle(
    #     img, (pup["x"]-1,pup["y"]-1), (pup["x"]+1,pup["y"]+1),             
    #     (128,128,255), -1
    # )


    print(str(pup["x"]) + " " + str(pup["y"]))
    return img
####################################


if __name__ == '__main__':
    args = sys.argv
    if len(args) > 1:
        image_file_path = args[1]
    if len(args) > 2:
        output_file_path = args[2]
    print (image_file_path)
    print (output_file_path)
    main(image_file_path)
    
