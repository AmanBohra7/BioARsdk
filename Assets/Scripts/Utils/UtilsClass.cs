using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

namespace CustomUtils{

    public static class UtilsClass
    {
        
        ///<summary>
        /// Crop an Texture2D with from center with provided size
        ///</summary>
        ///<parms name="textureToCrop">textureToCrop</params>
        ///<parms name="size">size</params>
        ///<parms name="offset">offset</params>
        public static Texture2D CropImage(Texture2D textureToCrop,int size = 900,int offset = 0){

            Color[] arr;
            arr = textureToCrop.GetPixels((textureToCrop.width - size) / 2,
                ((textureToCrop.height - size) / 2) + 200,
                size,size,0);
                
            Texture2D tmpTexture = new Texture2D(size,size,TextureFormat.RGBA32,false);
            tmpTexture.SetPixels(0,0,size,size,arr,0);
            tmpTexture.Apply();

            return tmpTexture;
    
        }

        ///<summary>
        /// Detects number of electrons in the texture image and return new texture with
        /// contours and out with number of detected electrons
        ///</summary>
        public static Texture2D ApplyScanning(Texture2D srcTexture,int blurSize,out int detectionCount){

            Mat srcMat = new Mat(srcTexture.height,srcTexture.width,CvType.CV_8UC3);
            Utils.texture2DToMat(srcTexture,srcMat);

            Mat destMat = new Mat();
            srcMat.copyTo(destMat);

            Imgproc.cvtColor(destMat, destMat, Imgproc.COLOR_BGR2GRAY);
            // Imgproc.GaussianBlur(destMat,destMat,new Size(5,5) , 1);
            Imgproc.blur(destMat,destMat,new Size(blurSize,blurSize));
            Imgproc.threshold(destMat, destMat, 120, 255, Imgproc.THRESH_BINARY);
            Imgproc.Canny(destMat,destMat,20,190);

            List<MatOfPoint> contours = new List<MatOfPoint>();
            Imgproc.findContours(destMat,contours,new Mat(),Imgproc.RETR_EXTERNAL,Imgproc.CHAIN_APPROX_SIMPLE);
            
            int num = 0;
            List<MatOfPoint> contours_list = new List<MatOfPoint>();
            for(int i =0; i< contours.Count ; i++){
                double area = Imgproc.contourArea(contours[i]);
                // print(area);
                // if(area > 1000 && area < 3000){
                //     contours_list.Add(contours[i]);
                //     num = num + 1;
                // }
                if(area > 80){
                    contours_list.Add(contours[i]);
                    num = num + 1;
                }
            }
            detectionCount = num;

            for(int i = 0 ; i < contours_list.Count ; i++){
                Imgproc.drawContours(srcMat, contours_list, -1, new Scalar(0, 255, 0), 4);
            }
            Texture2D scannedTexture = new Texture2D(srcMat.width(), srcMat.height(), TextureFormat.RGB24, false);
            Utils.matToTexture2D (srcMat, scannedTexture);
            
            return scannedTexture;
        }


        ///<summary>
        /// Save provided texture2D into png image in SavedImage folder with
        /// the name provided with second argument
        ///</summary>
        public static void SaveTextureAsPng(Texture2D srcTexture,string fileName = "Image"){
            byte[] arr = srcTexture.EncodeToPNG();
            var dirPath = Application.dataPath + "./SaveImages/";
            if(!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllBytes(dirPath + fileName + ".png", arr);
        }

        public static string GetElementData(int num){
            string jsonstring = "";

            if (File.Exists("Assets/DATA/PeriodicTableJson/Data.txt")){
                string saveString = File.ReadAllText("Assets/DATA/PeriodicTableJson/Data.txt");
                SimpleJSON.JSONNode data = SimpleJSON.JSON.Parse(saveString);
                jsonstring = data[num.ToString()].ToString();
                return jsonstring;
            }else{
                Debug.Log("JSON DATA HAVE SOME VALUE ERROR!");
            }

            return jsonstring;
        }   


    }

}

