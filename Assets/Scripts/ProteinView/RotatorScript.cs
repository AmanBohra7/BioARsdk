using TMPro;
using UnityEngine;


#if UNITY_EDITOR
     using Input = GoogleARCore.InstantPreviewInput;
#endif

public class RotatorScript : MonoBehaviour
{
    Vector3 cameraPreviousPose = Vector3.zero;
    Vector3 cameraPoseoffset = Vector3.zero;

    bool test = false;

    GameObject infoTagObject;
    TextMeshProUGUI textPro;


    void Start()
    {
        LeanTween.scale(gameObject, new Vector3(.2f, .2f, .2f), 0.5f)
            .setEaseOutBack()
            .setDelay(.20f);
        infoTagObject = GameObject.FindGameObjectWithTag("InfoTag");
        textPro = infoTagObject.GetComponent<TextMeshProUGUI>();

    }

    float zoomMax = 60f;
    float zoomMin = 20f;
    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;



            Zoom(difference * 0.1f);

        }


        // if (Input.GetMouseButton(0))
        // {

        //     textPro.text = "pressed!";
        //     RaycastHit hit = new RaycastHit();
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //     if (Physics.Raycast(ray, out hit, 30))
        //     {
        //         if (hit.collider.gameObject.name == "gray_cirlce_ratator") test = true;
        //         else test = false;

        //     }

        //     if (test)
        //     {
        //         cameraPoseoffset = Input.mousePosition - cameraPreviousPose;
        //         cameraPoseoffset = new Vector3(
        //             cameraPoseoffset.x % 20.0f,
        //             cameraPoseoffset.y % 20.0f,
        //             cameraPoseoffset.z % 20.0f
        //         );
        //         transform.Rotate(transform.up, -Vector3.Dot(cameraPoseoffset, Camera.main.transform.right), Space.World);
        //         gameObject.transform.parent.transform.Rotate(transform.up, -Vector3.Dot(cameraPoseoffset, Camera.main.transform.right), Space.World);

        //     }
        // }

        // if (Input.GetMouseButtonUp(0))
        // {
        //     test = false;
        // }

        // cameraPreviousPose = Input.mousePosition;
    }


    void Zoom(float increment)
    {

        Debug.Log("Difference : ");
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - increment, zoomMin, zoomMax);
        textPro.text = "PRESSED : " + Camera.main.fieldOfView.ToString();
    }
}

