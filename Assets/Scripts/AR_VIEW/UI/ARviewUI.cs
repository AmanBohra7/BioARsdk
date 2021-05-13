using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARviewUI : MonoBehaviour
{
    // starting main
    public GameObject startDisplay;

    public GameObject stParent;

    private Structure structure;
    private ModelGenerator instance;

    void Start(){
      
      // starting loading animation
      startDisplay.SetActive(true);
      LeanTween.alpha(startDisplay.GetComponent<RectTransform>(),0,1.5f);


      // updated structure variable from electron detection scene
      structure = ElectrionDectectionUIManager.SHARED_STRUCTURE;

      
     

    }


}
