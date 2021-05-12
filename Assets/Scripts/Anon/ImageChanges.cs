using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;
using TMPro;


public class ImageChanges : MonoBehaviour
{

    public Texture2D baseTexture;
    public RawImage sourceRawImage;
    public TextMeshProUGUI info;

    void Start()
    {
        Mat mainMat = new Mat(baseTexture.height, baseTexture.width, CvType.CV_8UC3);
        Mat grayMat = new Mat();

        sourceRawImage.texture = baseTexture;

        Utils.texture2DToMat(baseTexture, mainMat);
        mainMat.copyTo(grayMat);

        Imgproc.cvtColor(grayMat, grayMat, Imgproc.COLOR_BGR2GRAY);
        Imgproc.GaussianBlur(grayMat, grayMat, new Size(5, 5), 0);
        Imgproc.threshold(grayMat, grayMat, 110, 225, Imgproc.THRESH_BINARY);
        Imgproc.Canny(grayMat,grayMat,20,190);

        List<MatOfPoint> contours = new List<MatOfPoint>();
        Imgproc.findContours(grayMat,contours,new Mat(),Imgproc.RETR_EXTERNAL,Imgproc.CHAIN_APPROX_SIMPLE);

        int num = 0;
        List<MatOfPoint> contours_list = new List<MatOfPoint>();

        //  new logic
        // List<MatOfPoint> contours_list = new List<MatOfPoint>();
        for(int i =0; i< contours.Count ; i++){
            MatOfPoint cp = contours[i];
            MatOfPoint2f cn = new MatOfPoint2f(cp.toArray());
            double p = Imgproc.arcLength(cn, true);
            MatOfPoint2f approx = new MatOfPoint2f();
            Imgproc.approxPolyDP(cn, approx, 0.01 * p, true);
            double area = Imgproc.contourArea(contours[i]);
            if( (area > 30 && area < 100) && approx.toArray().Length > 8 ){
                // Imgproc.drawContours(mainMat, contours, -1, new Scalar(0, 255, 0), 4);
                contours_list.Add(contours[i]);
                num = num + 1;
                Debug.Log(area);
            }
        }

        

        // previously working 
        // for(int i =0; i< contours.Count ; i++){
        //     MatOfPoint cp = contours[i];
        //     MatOfPoint2f cn = new MatOfPoint2f(cp.toArray());
        //     double p = Imgproc.arcLength(cn,true);

        //     //  fron akshay file
        //     double area = Imgproc.contourArea(contours[i]);
        //     if(area > 50){
        //         Imgproc.drawContours(mainMat, contours, -1, new Scalar(0, 255, 0), 4);
        //         num = num + 1;
        //         Debug.Log(area);
        //     }
        
        // }


       
        // for(int i =0; i< contours.Count ; i++){
        //     double area = Imgproc.contourArea(contours[i]);
        //     if(area > 50){
        //         // Imgproc.drawContours(mainMat, contours, -1, new Scalar(0, 255, 0), 4);
        //         contours_list.Add(contours[i]);
        //         num = num + 1;
        //     }
        // }

        for(int i = 0 ; i < contours_list.Count ; i++){
            Imgproc.drawContours(mainMat, contours_list, -1, new Scalar(0, 255, 0), 4);
        }

        Debug.Log("Number : "+num);
        info.text += (num-1).ToString();

        Texture2D finaltexture = new Texture2D (grayMat.cols (), grayMat.rows (), TextureFormat.RGBA32, false);
        
        Utils.matToTexture2D (grayMat,finaltexture);
        sourceRawImage.texture = finaltexture;
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}



// using OpenCVForUnity.ObjdetectModule;
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