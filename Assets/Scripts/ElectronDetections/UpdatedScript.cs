using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;
using TMPro;

public class UpdatedScript : MonoBehaviour
{
    public RawImage srcImage;
    public RawImage destImage;

    public Texture2D srcTexture;
    private Texture2D destTexture;

    private Mat srcMat;
    private Mat destMat;

    byte[] arr;

    void Start(){

        // set srcTexture to srcImage
        srcImage.texture = srcTexture;

        // create src Mat
        srcMat = new Mat(srcTexture.height,srcTexture.width,CvType.CV_8UC3);
        Utils.texture2DToMat(srcTexture,srcMat);
        
        // craete and copy in destMat 
        destMat = new Mat();
        srcMat.copyTo(destMat);

        //  perfrom operations of destMat
        Imgproc.cvtColor(destMat, destMat, Imgproc.COLOR_BGR2GRAY);
        // Imgproc.GaussianBlur(destMat, destMat, new Size(9, 9), 2);
        Imgproc.threshold(destMat, destMat, 50, 225, Imgproc.THRESH_BINARY_INV);
        // Imgproc.Canny(destMat,destMat,50,150);

        //  finding contours
        List<MatOfPoint> contours = new List<MatOfPoint>();
        Imgproc.findContours(destMat,contours,new Mat(),Imgproc.RETR_EXTERNAL,Imgproc.CHAIN_APPROX_SIMPLE);
        int num = 0;
        List<MatOfPoint> contours_list = new List<MatOfPoint>();
        for(int i =0; i< contours.Count ; i++){
            double area = Imgproc.contourArea(contours[i]);
            print(area);
            if(area > 1000 && area < 3000){
                // Imgproc.drawContours(mainMat, contours, -1, new Scalar(0, 255, 0), 4);
                contours_list.Add(contours[i]);
                num = num + 1;
            }
        }
        for(int i = 0 ; i < contours_list.Count ; i++){
            Imgproc.drawContours(srcMat, contours_list, -1, new Scalar(0, 255, 0), 4);
        }
        print("Number of valid contours detected : "+contours_list.Count.ToString());

        //  creating new texture for srcImage;
        Texture2D finalTexture = new Texture2D(srcMat.width(), srcMat.height(), TextureFormat.RGB24, false);
        Utils.matToTexture2D (srcMat, finalTexture);
        srcImage.texture = finalTexture;


        //  convert destMat to destTexture
        destTexture = new Texture2D(srcTexture.width,srcTexture.height);
        Utils.matToTexture2D(destMat,destTexture);
        destTexture.Apply();
        destImage.texture = destTexture;


    }

}
