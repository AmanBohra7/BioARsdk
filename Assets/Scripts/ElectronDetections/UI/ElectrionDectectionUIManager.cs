using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;
using System;
using CustomUtils;
using UnityEngine.SceneManagement;

public class ElectrionDectectionUIManager : MonoBehaviour{
   
    public static ElectrionDectectionUIManager Instance {get; private set;}

    public static Structure SHARED_STRUCTURE;

    // TESTING
    public TextMeshProUGUI test;

    // functionalites related variables
    public GameObject bottomSlideBar;
    public GameObject informationBar;
    public GameObject cameraBtn;

    // Slider related variable
    public GameObject slider_content_hider;
    public GameObject bottom_slider;

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
            // DontDestroyOnLoad(gameObject);
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

    #region SceneChange Region

    // called from GenerateBtn region - ChangeSceenAnimation function
    IEnumerator LoadNextScene(float waitTime){
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    #endregion

    #region Updating Data

    // called from ScreenShot.cs
    public void GetJSONData(int req_num){
        test.text = "GOT INTO GET JSON";
        string jsonstring = UtilsClass.GetElementData(req_num);
        if(jsonstring == "JSON DATA HAVE SOME VALUE ERROR!")
            test.text = jsonstring;
        // test.text = jsonstring;
        try{
            Structure st = Newtonsoft.Json.JsonConvert.DeserializeObject<Structure>(jsonstring);
            SHARED_STRUCTURE = st;
            UpdateSliderBarData(st,req_num);
        }catch(Exception e){
            Debug.Log("JSON DATA HAVE SOME VALUE ERROR!");
        }
    }

    private void UpdateSliderBarData(Structure st,int num){
        elem_name.text = st.name;
        elem_atomic_number.text = st.number.ToString();
        elem_symbol.text = st.symbol;
        elem_disp.text = "Detected drawing has " + (num+1).ToString() + " dots, or " + (num).ToString()+ " electrons. The element is "+st.name+".";
    }

    #endregion

    #region GenerateBtn

    public void GenerateBtnCalled(){
        StartCoroutine(ChangeSceenAnimation());
    }

    private IEnumerator ChangeSceenAnimation(int waitTime = 0){

        // hide slider content
        yield return new WaitForSeconds(.5f);
        LeanTween.value(slider_content_hider,LeanTweenValueHelper,1,0,1f);

        // move slider
        yield return new WaitForSeconds(1f);
        float movePoseY = Screen.height ;
        LeanTween.moveLocalY(bottomSlideBar,
           100,
            .8f
        ).setEaseOutCubic();

        // hide slider
        yield return new WaitForSeconds(.9f);
        // LeanTween.value(bottom_slider,HideSliderOverAllCoroutine,1,0,1.5f);
        
        // change seen
        StartCoroutine(LoadNextScene(0f));         
    }

    void HideSliderOverAllCoroutine(float val,float ratio){
        bottom_slider.GetComponent<CanvasGroup>().alpha = val;
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
        
        // hiding camera btn
        LeanTween.alpha(cameraBtn.GetComponent<RectTransform>(),0,.5f);

        // moving the bottom slider upside to show information of the detection structure
        float movePoseY = Screen.height - 600;
        LeanTween.moveLocalY(bottomSlideBar,
            -movePoseY,
            .5f
        ).setEaseOutQuad().setDelay(.5f);
        
        // showing up the buttons in slider
        yield return new WaitForSeconds(1.1f);
        LeanTween.value(slider_content_hider,LeanTweenValueHelper,0,1,1f);
        
    }
    
    private void LeanTweenValueHelper(float val,float ratio){
        // Debug.Log("TEST");
        slider_content_hider.GetComponent<CanvasGroup>().alpha = val;
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