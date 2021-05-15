using UnityEngine;
using UnityEngine.UI;
using CustomUtils;

public class ARviewUI : MonoBehaviour
{
    public static ARviewUI Instance;

    // starting main
    public GameObject startDisplay;

    public GameObject stParent;

    private Structure structure;
    private ModelGenerator instance;

    // AR related information variables 
    public GameObject namePanel;
    public GameObject configPanel;

    public GameObject colorBtn;
    public GameObject gizmosBtn;

    private bool colorPressed;
    private bool gizmosPressed;

    // colors
    public Material blueColor;
    public Material redColor;
    public Material ringColor;
    public Material whiteColor;


    void Awake(){
        if(Instance == null){
            Instance = this;
        } 
        else{
            Destroy(this);
        }
    }

    void Start(){
      
      // starting loading animation
      startDisplay.SetActive(true);
      LeanTween.alpha(startDisplay.GetComponent<RectTransform>(),0,1.5f);


      // updated structure variable from electron detection scene
      structure = ElectrionDectectionUIManager.SHARED_STRUCTURE;
  
      // default state for both the toggles
      colorPressed = false;
      gizmosPressed = false;


      // calling this function initially to set the state of color button to active
      ColorBtnPressed();

    }

    public GameObject CreateNamePanel(Vector3 pose,Transform parent){
      GameObject newNamePanel = Instantiate(
        namePanel,
        pose,
        namePanel.transform.rotation,
        parent
      );
      return newNamePanel;
    }

    public GameObject CreateConfigPanel(Vector3 pose,Transform parent){
      GameObject newConfigPanel = Instantiate(
          configPanel,
          pose,
          configPanel.transform.rotation,
          parent
        );
        return newConfigPanel;
    }


    public void ColorBtnPressed(){
      colorPressed = !colorPressed;

      if(colorPressed){
        
        Color c = UtilsClass.ToColor("#2699FB");
        colorBtn.GetComponent<Image>().color = c;
        colorBtn.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(255,255,255,255);
        
        UtilsClass.ChangeStructureToColored(
          stParent,
          redColor,
          blueColor,
          ringColor
        );


      }else{
        colorBtn.GetComponent<Image>().color = new Color(255,255,255,255);
        Color c = UtilsClass.ToColor("#464A53");
        colorBtn.transform.GetChild(0).GetComponentInChildren<Image>().color = c;

        UtilsClass.RemoveStructureColor(
          stParent,
          whiteColor
        );

      }

    }

    public void GizmosBtnPressed(){
      gizmosPressed = !gizmosPressed;

      if(gizmosPressed){
        Color c = UtilsClass.ToColor("#2699FB");
        gizmosBtn.GetComponent<Image>().color = c;
        gizmosBtn.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(255,255,255,255);

        UtilsClass.ToggleGizmos(stParent,true);

      }else{
        gizmosBtn.GetComponent<Image>().color = new Color(255,255,255,255);
        Color c = UtilsClass.ToColor("#464A53");
        gizmosBtn.transform.GetChild(0).GetComponentInChildren<Image>().color = c;

        UtilsClass.ToggleGizmos(stParent,false);
      }

    }


}
