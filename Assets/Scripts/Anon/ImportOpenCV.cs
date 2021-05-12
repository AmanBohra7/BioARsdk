using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;
// using OpenCVForUnity.ArucoModule;
// using OpenCVForUnity.BgsegmModule;
// using OpenCVForUnity.Calib3dModule;
// using OpenCVForUnity.DnnModule;
// using OpenCVForUnity.FaceModule;
// using OpenCVForUnity.Features2dModule;
// using OpenCVForUnity.Img_hashModule;
// using OpenCVForUnity.ImgcodecsModule;
// using OpenCVForUnity.MlModule;
// using OpenCVForUnity.Phase_unwrappingModule;
// using OpenCVForUnity.PhotoModule;
// using OpenCVForUnity.PlotModule;
// using OpenCVForUnity.Structured_lightModule;
// using OpenCVForUnity.TextModule;
// using OpenCVForUnity.TrackingModule;
// using OpenCVForUnity.UtilsModule;
// using OpenCVForUnity.VideoioModule;
// using OpenCVForUnity.VideoModule;
// using OpenCVForUnity.Xfeatures2dModule;
// using OpenCVForUnity.XimgprocModule;
// using OpenCVForUnity.XphotoModule;

public class ImportOpenCV : MonoBehaviour
{

    public RawImage srcImage;  
    Texture2D baseTexture;

    public Texture tex;

    Mat mainMat;
    Mat grayMat;

    
    CascadeClassifier faceClass;

    Color32[] arr;

    WebCamTexture webCamTexture;

    // Start is called before the first frame update
    void Start()
    {   
        

        webCamTexture = new WebCamTexture();
        webCamTexture.Play();

        baseTexture = new Texture2D(webCamTexture.width,webCamTexture.height);

        srcImage.texture = baseTexture;
        srcImage.material.mainTexture = baseTexture;

        arr = new Color32[webCamTexture.width * webCamTexture.height];

        mainMat = new Mat(baseTexture.height, baseTexture.width, CvType.CV_8UC3);
        grayMat = new Mat();
    }

    private bool testbool = true;
    // Update is called once per frame
    void Update()
    {
        if(webCamTexture.isPlaying){

            webCamTexture.GetPixels32(arr);
            baseTexture.SetPixels32(arr);
            baseTexture.Apply();
            

            Utils.texture2DToMat(baseTexture, mainMat);
            mainMat.copyTo(grayMat);
            Imgproc.cvtColor(grayMat, grayMat, Imgproc.COLOR_BGR2GRAY);
            Imgproc.GaussianBlur(grayMat, grayMat, new Size(9, 9), 1);
            Imgproc.threshold(grayMat, grayMat, 110, 225, Imgproc.THRESH_BINARY);
            Imgproc.Canny(grayMat,grayMat,20,190);
            
            List<MatOfPoint> contours = new List<MatOfPoint>();
            Imgproc.findContours(grayMat,contours,new Mat(),Imgproc.RETR_EXTERNAL,Imgproc.CHAIN_APPROX_SIMPLE);

            int num = 0;

            List<MatOfPoint> contours_list = new List<MatOfPoint>();
            for(int i =0; i< contours.Count ; i++){
                double area = Imgproc.contourArea(contours[i]);
                if(area > 100){
                    // Imgproc.drawContours(mainMat, contours, -1, new Scalar(0, 255, 0), 4);
                    contours_list.Add(contours[i]);
                    num = num + 1;
                }
            }

            for(int i = 0 ; i < contours_list.Count ; i++){
                Imgproc.drawContours(mainMat, contours_list, -1, new Scalar(0, 255, 0), 4);
            }

            Debug.Log("Number : "+num);

            Utils.matToTexture2D (mainMat, baseTexture);
            // Utils.texture2DToMat(baseTexture, mainMat);
            // Mat dstMat = new Mat();
            // dstMat = mainMat.clone();

            // Size kSize = new Size(7d, 7d);
            // double sigmaX = 2d;
            // double sigmaY = 2d;
            // Imgproc.GaussianBlur(dstMat, dstMat, kSize, sigmaX, sigmaY);

            // Imgproc.threshold(dstMat,dstMat,110,255,Imgproc.THRESH_BINARY);

            // Imgproc.Canny(dstMat,dstMat,20,190);

            // List<MatOfPoint> srcContours = new List<MatOfPoint> ();
            // Mat srcHierarchy = new Mat ();
            // Imgproc.findContours(dstMat,srcContours, srcHierarchy, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);

            // Debug.Log ("srcContours.Count " + srcContours.Count);

            // Utils.matToTexture2D(dstMat,baseTexture);
        }
       
    }

    public void TakeSS(){
        byte[] test = baseTexture.EncodeToPNG();
        File.WriteAllBytes("Assets/DATA/test.png",test);
        Debug.Log("saved!");
    }
}



/*
Utils.texture2DToMat(baseTexture, mainMat);
Imgproc.cvtColor(mainMat, mainMat, Imgproc.COLOR_BGR2GRAY);
Utils.matToTexture2D(mainMat,baseTexture);
*/

        // mainMat = new(baseTexture.height, baseTexture.width, CvType.CV_8UC3);
        // grayMat = new Mat();

        // Utils.texture2DToMat(baseTexture, mainMat);

        // mainMat.copyTo(grayMat);

        // Imgproc.cvtColor(grayMat, grayMat, Imgproc.COLOR_BGR2GRAY);