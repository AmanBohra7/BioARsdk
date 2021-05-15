using System.Collections;
using UnityEngine;
using TMPro;
using CustomUtils;
using System;
using UnityEngine.SceneManagement;

public class ModelUI : MonoBehaviour
{
    // starting main
    public GameObject startDisplay;
    public GameObject endDisplay;
    public GameObject structureParent;
    
    // data updation UI related variables
    //  TOP SECTION
    public TextMeshProUGUI mol_heading;
    public TextMeshProUGUI mol_desp;
    public TextMeshProUGUI mol_bg;

    //  BOTTOM SECTION
    public TextMeshProUGUI melt_point;
    public TextMeshProUGUI boil_point;
    public TextMeshProUGUI atomic_mass;
    public TextMeshProUGUI phase;
    public TextMeshProUGUI density;


    // Colors
    public Material whiteColor;

    void Start(){
        // Color c = new Color(255,255,255,255);
        // startDisplay.GetComponent<Image>().color = c;
        startDisplay.SetActive(true);
        LeanTween.alpha(startDisplay.GetComponent<RectTransform>(),0,1.5f);

        endDisplay.SetActive(true);
        endDisplay.GetComponent<RectTransform>().localPosition = new Vector3(0,170,0);

        structureParent.transform.localScale = new Vector3(0f,0f,0f); 
        StartCoroutine(StructureSizeAnim(1.5f));

        UpdateMolData(ScreenShot.DETECTED_VALUE);
        // UpdateMolData(20);

        // Updating color to white defualt one
        UtilsClass.RemoveStructureColor(structureParent,whiteColor);
    }
    


    private void UpdateMolData(int req_num){
        string jsonstring = UtilsClass.GetElementData(req_num);
        // test.text = jsonstring;
        try{
            Structure st = Newtonsoft.Json.JsonConvert.DeserializeObject<Structure>(jsonstring);
            // print("TEST : " + st.name);
            mol_heading.text = st.name;
            mol_desp.text = st.summary;
            mol_bg.text = st.symbol;
            phase.text = st.phase;
            density.text = st.density.ToString();
            atomic_mass.text = st.atomic_mass.ToString();
            melt_point.text = st.melt.ToString()+" K";
            boil_point.text = st.boil.ToString()+" K";


        }catch(Exception e){
            Debug.Log("JSON DATA HAVE SOME VALUE ERROR!");
        }
    }



    IEnumerator StructureSizeAnim(float waitTime){
        yield return new WaitForSeconds(waitTime);
        LeanTween.scale(structureParent,new Vector3(1,1,1),1f).setEaseOutBack();
        Vector3 rot = new Vector3(26f,-130f,-106);
        // Vector3 rot = new Vector3(7.6f,60f,229f);
        LeanTween.rotate(structureParent,rot,1f).setEaseOutBack();
    }
    

    public void ViewARpressed(){
        print("called!");
        StartCoroutine(EndSceneCoroutine(.1f));
    }

    IEnumerator EndSceneCoroutine(float waitTime){
        yield return new WaitForSeconds(waitTime);

        // call for animation
        LeanTween.scale(endDisplay.GetComponent<RectTransform>(),new Vector3(40,40,40),1.5f).setEaseInOutQuad();

        // load AR scene
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
