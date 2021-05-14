using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Color c = ToColor("#2699FB");
        colorBtn.GetComponent<Image>().color = c;
        colorBtn.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(255,255,255,255);
      }else{
        colorBtn.GetComponent<Image>().color = new Color(255,255,255,255);
        Color c = ToColor("#464A53");
        colorBtn.transform.GetChild(0).GetComponentInChildren<Image>().color = c;
      }
    }

    public void GizmosBtnPressed(){
      gizmosPressed = !gizmosPressed;

      if(gizmosPressed){
        Color c = ToColor("#2699FB");
        gizmosBtn.GetComponent<Image>().color = c;
        gizmosBtn.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(255,255,255,255);
      }else{
        gizmosBtn.GetComponent<Image>().color = new Color(255,255,255,255);
        Color c = ToColor("#464A53");
        gizmosBtn.transform.GetChild(0).GetComponentInChildren<Image>().color = c;
      }
    }


    public Color32 ToColor(string hex)
    {
         hex = hex.Replace ("0x", "");//in case the string is formatted 0xFFFFFF
         hex = hex.Replace ("#", "");//in case the string is formatted #FFFFFF
         byte a = 255;//assume fully visible unless specified in hex
         byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
         byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
         byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
         //Only use alpha if the string has enough characters
         if(hex.Length == 8){
             a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
         }
         return new Color32(r,g,b,a);
    }

}
