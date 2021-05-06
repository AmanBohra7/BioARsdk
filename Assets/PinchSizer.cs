using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchSizer : MonoBehaviour

{
    public GameObject cube;
    bool pressed = false;


    Vector2 startPos, direction;
    bool directionChosen;


    float zoomMax = 60f;
    float zoomMin = 20f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if(Input.touchCount == 2){
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;


            Debug.Log("Difference : "+difference);
            Zoom(difference * 0.1f);

        }

        // if ( Input.touchCount >= 2){
        //     Touch touch = Input.touches[1];

        //     switch (touch.phase)
        //     {
        //         // Record initial touch position.
        //         case TouchPhase.Began:
        //             startPos = touch.position;
        //             directionChosen = false;
        //             // Debug.Log(startPos);
        //             break;

        //         // Determine direction by comparing the current touch position with the initial one.
        //         case TouchPhase.Moved:
        //             direction = touch.position - startPos;
        //             break;

        //         // Report that a direction has been chosen when the finger is lifted.
        //         case TouchPhase.Ended:
        //             directionChosen = true;
        //             break;
        //     }


        // if(!pressed){
        //     Vector3 testPose = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x,touch.position.y,10f));
        //     Debug.Log(touch.position+" - "+testPose);
        //     GameObject test = Instantiate(cube,testPose,cube.transform.rotation);
        //     pressed = true;
        // }
        // if (directionChosen)
        // {
        //     if (direction.x > 0) Debug.Log("RIGHT");
        //     else Debug.Log("LEFT");

        //     directionChosen = false;
        // }

        // }
    }


    void Zoom(float increment)
    {
        
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView- increment, zoomMin, zoomMax);
        // Debug.Log("CALLED! - "+Camera.main.fieldOfView);
    }

    void OnGUI()
    {
        // GUI.Box(new Rect(10,10,100,200),"TEST BOX");

        // if(GUI.Button(new Rect(20,30,50,100),"Button")){
        //     Debug.Log("PRESSED BUTTON!");
        // }

    }

    //  void OnMouseDown()
    // {

    // }

}
