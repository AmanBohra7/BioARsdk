using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureController : MonoBehaviour
{
    Vector3 cameraPreviousPose = Vector3.zero;
    Vector3 cameraPoseoffset;
    float rotationAngle;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            cameraPoseoffset = Input.mousePosition - cameraPreviousPose;
            cameraPoseoffset = new Vector3(
                cameraPoseoffset.x,
                cameraPoseoffset.y,
                cameraPoseoffset.z);
            

            if (Input.mousePosition.x > cameraPreviousPose.x){

                gameObject.transform.Rotate(
                new Vector3(0, 
                    - rotationAngle * .5f, 0),
                Space.World);


            }else if (Input.mousePosition.x < cameraPreviousPose.x){
                
                gameObject.transform.Rotate(
                new Vector3(0, + rotationAngle * .5f, 0),
                Space.World);

            }
            else{
                return;
            }

            rotationAngle = Mathf.Abs(Vector3.Dot(cameraPoseoffset, Camera.main.transform.right));
        }
        cameraPreviousPose = Input.mousePosition;
    }
}
