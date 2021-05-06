using System.IO;
using UnityEngine;
using UnityEngine.UI;

using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ObjdetectModule;

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
        faceClass = new CascadeClassifier("haarcascade_frontalface_default.xml");

        webCamTexture = new WebCamTexture();
        webCamTexture.Play();

        baseTexture = new Texture2D(webCamTexture.width,webCamTexture.height);
        mainMat = new Mat(baseTexture.height, baseTexture.width, CvType.CV_8UC3);
        srcImage.texture = baseTexture;
        srcImage.material.mainTexture = baseTexture;

        arr = new Color32[webCamTexture.width * webCamTexture.height];
    }

    private bool testbool = true;
    // Update is called once per frame
    void Update()
    {
        if(webCamTexture.isPlaying){

            webCamTexture.GetPixels32(arr);
            baseTexture.SetPixels32(arr);
            baseTexture.Apply();
            

            // Perfom electron detection logic !! 
            

            Utils.texture2DToMat(baseTexture, mainMat);
            Imgproc.cvtColor(mainMat, mainMat, Imgproc.COLOR_BGR2GRAY);

            MatOfRect test = new MatOfRect(mainMat);
            faceClass.detectMultiScale(mainMat,test,4);

            if(testbool){
                print(mainMat);
                print(test);
                testbool = false;
            }


            Utils.matToTexture2D(mainMat,baseTexture);
        }
       
    }

    public void TakeSS(){
        byte[] test = baseTexture.EncodeToPNG();
        File.WriteAllBytes("Assets/DATA/test.png",test);
        print("saved!");
    }
}



        // mainMat = new(baseTexture.height, baseTexture.width, CvType.CV_8UC3);
        // grayMat = new Mat();

        // Utils.texture2DToMat(baseTexture, mainMat);

        // mainMat.copyTo(grayMat);

        // Imgproc.cvtColor(grayMat, grayMat, Imgproc.COLOR_BGR2GRAY);