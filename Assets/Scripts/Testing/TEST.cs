using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;
using System.IO;

public class TEST : MonoBehaviour
{
    bool takePicture;
    public RawImage srcImage;
    public TextMeshProUGUI infoText;

    // for test
    public Texture2D testingTexture;
    private Texture2D savedTexture;
    public RawImage crop_image;

    public TMP_InputField lowInput;
    // public TMP_InputField highInput;
    private int low;
    private int high;

    private Mat srcMat;
    private Mat destMat;

    

    void Start(){
        takePicture = false;

        low = 12;
        lowInput.text = low.ToString();
        // high = 225;
        // highInput.text = high.ToString();

        lowInput.onEndEdit.AddListener(delegate {UpdateValues();} );

        // CropImage();

    }


    void CropImage(Texture2D textureToCrop = null){
        //  convert testingTexture into crop_image size
        int size = 900;
        Color[] arr;
        if(textureToCrop == null)
            arr = testingTexture.GetPixels((testingTexture.width - size) / 2,(testingTexture.height - size) / 2,size,size,0);
        else
            arr = textureToCrop.GetPixels((textureToCrop.width - size) / 2,(textureToCrop.height - size) / 2,size,size,0);
        
        Texture2D cropTexture = new Texture2D(size,size,TextureFormat.RGBA32,false);
        cropTexture.SetPixels(0,0,size,size,arr,0);
        cropTexture.Apply();

        // SaveTextureAsPNG(cropTexture,"CropImage");
        // perform opencv steps 
        ConvertIntoDest(cropTexture,crop_image);

        // crop_image.texture = cropTexture;

    }

    void UpdateValues(){
        low = int.Parse(lowInput.text);
        // high = int.Parse(highInput.text);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        
        if(takePicture){
            takePicture = false;
            print("Screen shot button pressed!");
            var tempRend = RenderTexture.GetTemporary(src.width,src.height);
            Graphics.Blit(src,tempRend);

            Texture2D tmpTexture = new Texture2D(src.width,src.height,TextureFormat.RGBA32,false);
            UnityEngine.Rect rect = new UnityEngine.Rect(0,0,src.width,src.height);
            tmpTexture.ReadPixels(rect,0,0,false);
            tmpTexture.Apply();
            savedTexture = tmpTexture;

            // SaveTextureAsPNG(tmpTexture);

            // srcImage.texture = tmpTexture;
            // srcImage.enabled = true;
            RenderTexture.ReleaseTemporary(tempRend);


            CropImage(tmpTexture);

            // ConvertIntoDest(tmpTexture);

        }

        Graphics.Blit(src, dest);
    }


    void SaveTextureAsPNG(Texture2D reqTexture,string name = "image"){
        byte[] arr = reqTexture.EncodeToPNG();
        var dirPath = Application.dataPath + "./SaveImages/";
        if(!Directory.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(dirPath + name + ".png", arr);
    }

    void ConvertIntoDest(Texture2D srcTexture,RawImage destImage = null){

        srcMat = new Mat(srcTexture.height,srcTexture.width,CvType.CV_8UC3);
        Utils.texture2DToMat(srcTexture,srcMat);

        destMat = new Mat();
        srcMat.copyTo(destMat);

        Imgproc.cvtColor(destMat, destMat, Imgproc.COLOR_BGR2GRAY);
        // Imgproc.GaussianBlur(destMat,destMat,new Size(5,5) , 1);
        Imgproc.blur(destMat,destMat,new Size(low,low));
        Imgproc.threshold(destMat, destMat, 120, 255, Imgproc.THRESH_BINARY);
        Imgproc.Canny(destMat,destMat,20,190);

        List<MatOfPoint> contours = new List<MatOfPoint>();
        Imgproc.findContours(destMat,contours,new Mat(),Imgproc.RETR_EXTERNAL,Imgproc.CHAIN_APPROX_SIMPLE);
        
        int num = 0;
        List<MatOfPoint> contours_list = new List<MatOfPoint>();
        for(int i =0; i< contours.Count ; i++){
            double area = Imgproc.contourArea(contours[i]);
            print(area);
            // if(area > 1000 && area < 3000){
            //     contours_list.Add(contours[i]);
            //     num = num + 1;
            // }
            if(area > 80){
                contours_list.Add(contours[i]);
                num = num + 1;
            }
        }
        for(int i = 0 ; i < contours_list.Count ; i++){
            Imgproc.drawContours(srcMat, contours_list, -1, new Scalar(0, 255, 0), 4);
        }
        print("Number of valid contours detected : "+contours_list.Count.ToString());
        infoText.text = "Detection : " + contours_list.Count.ToString();

        Texture2D finalTexture = new Texture2D(srcMat.width(), srcMat.height(), TextureFormat.RGB24, false);
        Utils.matToTexture2D (srcMat, finalTexture);
        
        if(destImage == null)
            srcImage.texture = finalTexture;
        else{
            destImage.texture = finalTexture;
            // SaveTextureAsPNG(finalTexture,"CropImageOutput");
            destImage.enabled = true;
        }
            

    }

    public void Regenerate(){
            ConvertIntoDest(savedTexture);
    }

    public void TakeScreenShot(){
        takePicture = true;
    }
}
