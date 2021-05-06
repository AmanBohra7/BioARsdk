using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeTester : MonoBehaviour
{

    BoxCollider boxCollider;
    public GameObject electron;

    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        int num_ele = 8;
        float offsetAngle = 360 / num_ele;
        // print("offset Angle: "+offsetAngle);
        float intialAgle = 0;
        float radius = (boxCollider.size.x / 2) - 0.05f;
        for(int i = 0 ; i < num_ele ; i++){        
            float x =  radius * Mathf.Cos(2 * Mathf.PI * i / num_ele); 
            float y =  radius * Mathf.Sin(2 * Mathf.PI * i / num_ele);
            // print(intialAgle + " "+ x + " " + y);
             Instantiate(
                electron,
                new Vector3(x,0,y),
                electron.transform.rotation
            );
            intialAgle += offsetAngle;
        }
    }

}
