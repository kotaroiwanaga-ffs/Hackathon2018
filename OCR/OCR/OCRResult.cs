using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 追加
using System.Runtime.Serialization;

namespace ComputerVisionAPI
{
    [DataContract]
    class OCRResult
    {
        [DataMember]
        public string language { get; set; }
        [DataMember]
        public float textAngle { get; set; }
        [DataMember]
        public string orientation { get; set; }
        [DataMember]
        public Region[] regions { get; set; }

        //リージョン
        [DataContract]
        public class Region
        {
            [DataMember]
            //座標
            public string boundingBox { get; set; }
            [DataMember]
            //行
            public Line[] lines { get; set; }
        }

        //行
        [DataContract]
        public class Line
        {
            [DataMember]
            //座標
            public string boundingBox { get; set; }
            [DataMember]
            //単語
            public Word[] words { get; set; }
        }

        //単語
        [DataContract]
        public class Word
        {
            [DataMember]
            //座標
            public string boundingBox { get; set; }
            [DataMember]
            //文字
            public string text { get; set; }
        }


    }
}