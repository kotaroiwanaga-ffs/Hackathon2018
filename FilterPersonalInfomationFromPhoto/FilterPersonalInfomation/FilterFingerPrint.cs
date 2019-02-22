using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace FilterPersonalInfomation
{
    public class FilterFingerPrint
    {
        public static string DrawFilteredFingerPrint(string imagePath)
        {
            XDocument xml = XDocument.Load("FingerPrintConfig.xml");
            XElement pathConfig = xml.Element("Path");

            string imageDir = Path.GetDirectoryName(imagePath);
            string imageName = Path.GetFileName(imagePath);
            string outputDir= CreateDirectory(Path.Combine(imageDir,"output"));
            string copyImagePath = Path.Combine(outputDir, imageName);
            File.Copy(imagePath, copyImagePath);

            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = pathConfig.Element("exe_openpose_bat").Value;
            p.StartInfo.Arguments = "\"" + pathConfig.Element("openpose").Value + "\" \"" + outputDir + "\" \"" + outputDir + "\"";
            p.Start();
            p.WaitForExit();

            p.StartInfo.FileName = pathConfig.Element("python").Value;
            string python_path = pathConfig.Element("analyze_json_python").Value;
            string jsonPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(copyImagePath) + "_keypoints.json");
            string outputImagePath = Path.Combine(outputDir, "output_" + imageName);
            p.StartInfo.Arguments = "\"" + python_path + "\" \"" + jsonPath + "\" \"" + copyImagePath + "\" \"" + outputImagePath + "\"";
            p.Start();
            p.WaitForExit();

            return outputImagePath;
        }


        private static string CreateDirectory(string path)
        {
            int i = 1;
            var newPath = path;

            while (Directory.Exists(newPath))
            {
                newPath = $"{path}_{i++}";
            }

            Directory.CreateDirectory(newPath);

            return newPath;
        }
    }
}
