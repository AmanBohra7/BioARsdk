using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;
using System.IO;
using CustomUtils;

// Take screen shot
// crop screen shot
// place both images in center
// detect electrons
// start scanning | animation 
// show slider panel 


public class ScreenShot : MonoBehaviour
{

    private bool takePicture;

    private ElectrionDectectionUIManager Instance;

    // texture for storing saved screen shot
    private Texture2D savedScreenShotTexture;
    private Texture2D cropTexture;
    private Texture2D scannedTexture;

    public TMP_InputField blurSizeInput;

    private int blurSize;

    void Start(){
        Instance = ElectrionDectectionUIManager.Instance;

        takePicture = false;
        blurSize = 10;
        blurSizeInput.text = blurSize.ToString();
        blurSizeInput.onEndEdit.AddListener(delegate {UpdateValues();} );
    }

    void UpdateValues(){
        blurSize = int.Parse(blurSizeInput.text);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        
        if(takePicture){
            takePicture = false;

            var tempRend = RenderTexture.GetTemporary(src.width,src.height);
            Graphics.Blit(src,tempRend);

            Texture2D tmpTexture = new Texture2D(src.width,src.height,TextureFormat.RGBA32,false);
            UnityEngine.Rect rect = new UnityEngine.Rect(0,0,src.width,src.height);
            tmpTexture.ReadPixels(rect,0,0,false);
            tmpTexture.Apply();
            RenderTexture.ReleaseTemporary(tempRend);

            CallForProcessing(tmpTexture);
        }

        Graphics.Blit(src, dest);
    }

    private void CallForProcessing(Texture2D tmpTexture){
        
        savedScreenShotTexture = tmpTexture;

        // create and set crop image
        cropTexture = UtilsClass.CropImage(tmpTexture,900,0);
        Instance.SetCropedImageTexture(cropTexture);
        Instance.CropedImageSetState(true);

        //  create scanned image
        int num_detections;
        scannedTexture =  UtilsClass.ApplyScanning(cropTexture,blurSize,out num_detections);

        // set scanned image
        Instance.SetScannedImageTexture(scannedTexture);
        Instance.ScannedImageSetState(true);

        // Instance.SetDetectedElectronText(num_detections);

        // saving number of electrons data in UI manager file
        // Instance.num_of_elec = num_detections;
        Instance.GetJSONData(num_detections-1);
        // calling for scanning animation o
        Instance.StartScanningSection(3);
    }

    public void Regenerate(){
        
    }

    public void TakeScreenShot(){
        takePicture = true;
    }

}
