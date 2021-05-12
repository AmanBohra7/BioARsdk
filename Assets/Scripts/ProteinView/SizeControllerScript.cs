using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeControllerScript : MonoBehaviour
{

    Transform m_anchorTransform;
    private bool initialized = false;

    Camera m_mainCamera;

    void Start(){
        m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<Camera>();
        gameObject.GetComponent<Canvas>().worldCamera = m_mainCamera;
    }

    public void SizeControllerInitialized(Transform anchorTransform){
        Debug.Log("Called!");
        m_anchorTransform = anchorTransform;
        initialized = true;
    }


    public void IncreaseButtonPressed(){
        if(!initialized) return;
        Vector3 scaleChange = new Vector3(.05f, .05f, .05f);
        m_anchorTransform.transform.localScale += scaleChange;
        Debug.Log("Increase button pressed!");
    }

    public void DecreaseButtonPressed(){
        if(!initialized) return;
        Vector3 scaleChange = new Vector3(-.05f, -.05f, -.05f);
        m_anchorTransform.transform.localScale += scaleChange;
        Debug.Log("Descrease button pressed!");
    }

}
