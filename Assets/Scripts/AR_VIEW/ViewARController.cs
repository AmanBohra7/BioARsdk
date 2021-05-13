using GoogleARCore;
using UnityEngine;

#if UNITY_EDITOR
     using Input = GoogleARCore.InstantPreviewInput;
#endif


public class ViewARController : MonoBehaviour{  

    int screenHeight;
    int screenWidth;

    bool markerCreated = false;
    bool m_ProteinPlaced = false;

    public GameObject AR_MARKER;
    GameObject markderObj;

    GameObject STRUCTURE_ELEMENT;

    Anchor m_ProtienAnchor = null;

    public Camera FirstPersonCamera;
    // Start is called before the first frame update

    void Start(){
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        QuitOnConnectionErrors();

        STRUCTURE_ELEMENT = GameObject.FindGameObjectWithTag("StructureParent");
        STRUCTURE_ELEMENT.SetActive(false);

    }

    void QuitOnConnectionErrors()
    {
        if (Session.Status ==  SessionStatus.ErrorPermissionNotGranted){
            Debug.Log(
                "Camera permission is needed to run this application.");
        }
        else if (Session.Status.IsError()){
            Debug.Log(
                "ARCore encountered a problem connecting. Please restart the app.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Session.Status != SessionStatus.Tracking){
            int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Vector2 centerPose = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f);
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;
            
        if(!m_ProteinPlaced)
            if(Frame.Raycast(centerPose.x, centerPose.y, raycastFilter, out hit)){
 
                if(hit.Trackable is DetectedPlane){
                    
                    // section of code to instantiate a AR_MARKER
                    if(!markerCreated){

                        markderObj = Instantiate(AR_MARKER,
                                    hit.Pose.position,
                                    AR_MARKER.transform.rotation);

                        // markderObj.transform.position =
                        //         hit.Pose.position;
                        markerCreated = true;
                    }
                    else{
                        if(!m_ProteinPlaced) markderObj.transform.position =
                                    hit.Pose.position;
                    }

                    // after marker
                    if(Input.touchCount > 0 && markerCreated && !m_ProteinPlaced){

                        m_ProteinPlaced = true;
                        
                        Vector3 newScale = new Vector3(.05f,.05f,.05f);
                        STRUCTURE_ELEMENT.transform.localScale = newScale;
                        
                        // creating new anchor
                        m_ProtienAnchor = hit.Trackable.CreateAnchor(hit.Pose);
                        
                        
                        // position of the placed structure model in AR 
                        Vector3 newPose = new Vector3(
                            m_ProtienAnchor.transform.position.x,
                            m_ProtienAnchor.transform.position.y + .045f,
                            m_ProtienAnchor.transform.position.z
                        );
                        STRUCTURE_ELEMENT.transform.position = newPose;
                        
                        // set rotation
                        // Vector3 newRot = new Vector3(
                            
                        // );
                        
                        STRUCTURE_ELEMENT.transform.parent = m_ProtienAnchor.transform;
                        
                        STRUCTURE_ELEMENT.SetActive(true);
                        // Debug.Log(hit.Pose);
                        // Debug.Log(m_ProteinObject.transform.position);
                        // m_ProteinObject.GetComponent<ProteinContoller>().InitializeContollers();

                        Destroy(markderObj);
                        markderObj.SetActive(false);

                    }
                }
            }
    } // end update


    void UpdateValues(Transform toThis){
        
    }

}
