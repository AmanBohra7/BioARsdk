using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateSizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 UpdatedPose = gameObject.transform.position;
        Debug.Log("Current: "+UpdatedPose);
        UpdatedPose.x = 0.35f;
        Debug.Log("Updated: "+UpdatedPose);
        // LeanTween.moveX(gameObject,.25f,0.5f)
        //     .setEaseOutBack()
        //     .setDelay(.20f);
        LeanTween.scale(gameObject,new Vector3(0.015f,0.015f,0.015f),0.5f)
            .setEaseOutBack()
            .setDelay(.20f);
    }

   
}
