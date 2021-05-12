using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteinContoller : MonoBehaviour
{

    public GameObject rotationControllerPrefab;
    public GameObject sizeControllerPrefab;

    private GameObject m_RotationControllerObjectInstance;
    private GameObject m_SizeControllerObjectInstance;

 
    void Start(){
        LeanTween.scale(gameObject,new Vector3(1.5f,1.5f,1.5f),0.5f)
            .setEaseOutBack()
            .setDelay(.20f);
    }


    public void InitializeContollers(){
        
        Debug.Log("Controllers Initalized!");

        InitializeRotationContoller();

        InitializeSizeContoller();

    }

    private void InitializeRotationContoller(){
        
        Vector3 rotateControllerPosition = new Vector3(
                gameObject.transform.position.x,
                gameObject.transform.position.y - 0.125f,
                gameObject.transform.position.z
        );

        m_RotationControllerObjectInstance = Instantiate(
                rotationControllerPrefab,
                rotateControllerPosition,
                rotationControllerPrefab.transform.rotation,
                gameObject.transform
        );

        gameObject.transform.parent.gameObject.transform.rotation = gameObject.transform.rotation;
        // gameObject.transform.parent = m_RotationControllerObjectInstance.transform;
    }


    private void InitializeSizeContoller(){
        
        Debug.Log("Parent name: "+gameObject.transform.parent.gameObject.name);

        Vector3 sizeControllerPosition = new Vector3(
            gameObject.transform.parent.transform.position.x + 0.25f,
            gameObject.transform.parent.transform.position.y + 0.25f,
            gameObject.transform.parent.transform.position.z
        );
        
        m_SizeControllerObjectInstance = Instantiate(
                sizeControllerPrefab,
                sizeControllerPosition,
                sizeControllerPrefab.transform.rotation,
                gameObject.transform.parent.transform     
        );

        m_SizeControllerObjectInstance.GetComponentInChildren<SizeControllerScript>()
            .SizeControllerInitialized(gameObject.transform.parent.transform);

        Debug.Log("Size Location:"+m_SizeControllerObjectInstance.transform.position);

    }

}
