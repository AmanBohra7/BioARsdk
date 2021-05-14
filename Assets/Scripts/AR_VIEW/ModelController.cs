using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{

    Vector3 cameraPreviousPose = Vector3.zero;
    Vector3 cameraPoseoffset;

    private Vector3 _startingPosition;

    float rotationAngle;

    void Update(){
        
        if (Input.GetMouseButton(0))
        {
            cameraPoseoffset = Input.mousePosition - cameraPreviousPose;
            cameraPoseoffset = new Vector3(
                cameraPoseoffset.x,
                cameraPoseoffset.y,
                cameraPoseoffset.z);
            

            // right turn
            if (Input.mousePosition.x > cameraPreviousPose.x){

                gameObject.transform.Rotate(
                new Vector3(0, - rotationAngle * .5f, 0),
                Space.World);


            }else 
            // left turn
            if (Input.mousePosition.x < cameraPreviousPose.x){
            
                gameObject.transform.Rotate(
                new Vector3(0, + rotationAngle * .5f, 0),
                Space.World);

            }
            else{
                return;
            }

            rotationAngle = Mathf.Abs(Vector3.Dot(cameraPoseoffset, Camera.main.transform.right));

            // transform.Rotate(transform.up, -Vector3.Dot(cameraPoseoffset, Camera.main.transform.right), Space.World);
            // gameObject.transform.parent.transform.Rotate(transform.up, -Vector3.Dot(cameraPoseoffset, Camera.main.transform.right), Space.World);

        }
        cameraPreviousPose = Input.mousePosition;

    }
}
