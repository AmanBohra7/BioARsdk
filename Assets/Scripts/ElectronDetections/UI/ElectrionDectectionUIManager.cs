 using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;
using System;
using CustomUtils;


public class ElectrionDectectionUIManager : MonoBehaviour{
   
    public static ElectrionDectectionUIManager Instance {get; private set;}

    // functionalites related variables
    public GameObject bottomSlideBar;
    public GameObject informationBar;
    public GameObject cameraBtn;

    // Slider related variable
    public GameObject slider_btn_hider;

    // Update information variables;
    public TextMeshProUGUI elem_name;
    public TextMeshProUGUI elem_symbol;
    public TextMeshProUGUI elem_atomic_number;
    public TextMeshProUGUI elem_disp;

    // scanner variables
    public GameObject output_mask;
    public GameObject scanner_mask;
    public GameObject scanner_lines;

    private Vector3 sliderInitialPose;

    // Screen shot related varaibles
    public GameObject cropedImage;
    public GameObject scannedImage;
    public GameObject focus;
    public GameObject ScannerObj;
    public GameObject detectionInforamtion;

    void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else{
            Destroy(this);
        }
    }

    void Start(){
        float tmpY =  -(Screen.height / 2);
        sliderInitialPose = new Vector3(0,-Screen.height,0);
        bottomSlideBar.GetComponent<RectTransform>().localPosition = sliderInitialPose;
    }

    #region Updating Data

    // called from ScreenShot.cs
    public void GetJSONData(int test){
        string jsonstring = UtilsClass.GetElementData(test);
        Debug.Log(jsonstring);

        try{
            Structure st = Newtonsoft.Json.JsonConvert.DeserializeObject<Structure>(jsonstring);
            UpdateSliderBarData(st,test);
        }catch(Exception e){
            Debug.Log("JSON DATA HAVE SOME VALUE ERROR!");
        }

    }


    private void UpdateSliderBarData(Structure st,int num){
        elem_name.text = st.name;
        elem_atomic_number.text = st.number.ToString();
        elem_symbol.text = st.symbol;
        elem_disp.text = "Detected drawing has" + num.ToString() + " dots, or " + (num-1).ToString()+ "electrons. The element is "+st.name+".";
    }

    #endregion

    #region GenerateBtn

    public void GenerateBtnCalled(){
        StartCoroutine(ChangeSceenAnimation());
    }

    private IEnumerator ChangeSceenAnimation(int waitTime = 0){
        yield return new WaitForSeconds(waitTime);
        // performa animation code
        float movePoseY = Screen.height ;
        LeanTween.moveLocalY(bottomSlideBar,
           0,
            .8f
        ).setEaseOutCubic().setDelay(.5f);

    

        LeanTween.alpha(slider_btn_hider.GetComponent<RectTransform>(),1,0.4f).setDelay(.15f);
    }

    #endregion

    #region Scanner

    // main
    public void StartScanningSection(int waitTime = 0){
        // call for scanning function
        StartScanningAnimation(waitTime);
        // call for view output function
        ShowOutputAnimation(waitTime + 5);
        // pop up bottom slider
        ShowBottomSlideBar(waitTime + 7);
    }

    private void ShowOutputAnimation(int waitTime = 0){
       StartCoroutine(ShowOutputAnimationCoroutine(waitTime));  
    }

    private IEnumerator  ShowOutputAnimationCoroutine(int waitTime){
        yield return new WaitForSeconds(waitTime);
        LeanTween.moveY(output_mask.GetComponent<RectTransform>(),0f,2f).setEaseOutQuad();
        LeanTween.moveLocalY(scannedImage,0f,2f).setEaseOutQuad();
    }


    private void StartScanningAnimation(int waitTime = 0){
        StartCoroutine(ScanningAnimationCoroutine(waitTime));
    }

    private IEnumerator ScanningAnimationCoroutine(int waitTime){
        yield return new WaitForSeconds(waitTime);
        //  aniamtion logic
        LeanTween.moveLocalY(scanner_lines,-20f,2f).setEaseOutQuad();
        LeanTween.alpha(scanner_lines.GetComponent<RectTransform>(),0,.2f).setDelay(2f);

        LeanTween.moveLocalY(scanner_lines,100f,0).setEaseOutQuad().setDelay(4f);

        LeanTween.alpha(scanner_lines.GetComponent<RectTransform>(),1,.2f).setDelay(4f);
        LeanTween.moveLocalY(scanner_lines,-20f,2f).setEaseOutQuad().setDelay(5f);

        LeanTween.alpha(scanner_lines.GetComponent<RectTransform>(),0,.2f).setDelay(8f);
    }

    #endregion

    #region DetectedSlider

    public void ShowBottomSlideBar(int waitTime = 0){
        StartCoroutine(ShowBottomSlideBarCorutine(waitTime));
    }

    private IEnumerator ShowBottomSlideBarCorutine(int waitTime = 0){
        yield return new WaitForSeconds(waitTime);
        // performa animation code
        print("TEST");
        float movePoseY = Screen.height - 600;
        LeanTween.moveLocalY(bottomSlideBar,
            -movePoseY,
            .5f
        ).setEaseOutQuad();
        LeanTween.alpha(cameraBtn.GetComponent<RectTransform>(),0,.1f);
        LeanTween.alpha(slider_btn_hider.GetComponent<RectTransform>(),0,0.4f).setDelay(.7f);
    }
    
    private IEnumerator HideBottomSlideBarCorutine(int waitTime = 0){
        yield return new WaitForSeconds(waitTime);
        // performa animation code
    }

    #endregion

    #region  ScreenShot

    public void SetCropedImageTexture(Texture2D srcTexture){
        cropedImage.GetComponent<RawImage>().texture = srcTexture;
    }

    public void SetScannedImageTexture(Texture2D srcTexture){
        scannedImage.GetComponent<RawImage>().texture = srcTexture;
    }

    public void CropedImageSetState(bool state){
        cropedImage.SetActive(state);
    }

    public void ScannedImageSetState(bool state){
        scannedImage.SetActive(state);
    }

    public void SetDetectedElectronText(int count){
        detectionInforamtion.GetComponent<TextMeshProUGUI>().text = 
            "Detected : "+ count.ToString();
    }

    #endregion

}



// Slider bar
// center sprite
// info / setting icon
// scanner style
// ingo pannel with dummy information 


// slider intial pose - 