using System.Collections;
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

    private ARviewUI UIinstance;

    void Start(){
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        QuitOnConnectionErrors();

        STRUCTURE_ELEMENT = GameObject.FindGameObjectWithTag("StructureParent");
        STRUCTURE_ELEMENT.SetActive(false);

        UIinstance = ARviewUI.Instance;

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
            
        #region MODEL_PLACEMENT
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

                        // creating new anchor
                        m_ProtienAnchor = hit.Trackable.CreateAnchor(hit.Pose);

                        
                        StartCoroutine(HideMarkerAnim());
                        
                        // UIinstance.CreateNamePanel(m_ProtienAnchor.transform.position);
                        UIinstance.CreateConfigPanel(m_ProtienAnchor.transform.position,
                                        m_ProtienAnchor.transform);

                        StartCoroutine(ShowStructure(1.1f));

                    }
                }
            }

        #endregion


    } // end update


    IEnumerator HideMarkerAnim(float waitTime = 0){
        yield return new WaitForSeconds(waitTime);
        float animTime = 1.2f;
        LeanTween.scale(markderObj,new Vector3(0f,0f,0f),animTime).setEaseInBack();
        yield return new WaitForSeconds(animTime);
        markderObj.SetActive(false);
    }

    IEnumerator ShowStructure(float waitTime = 0){
        yield return new WaitForSeconds(waitTime);

        STRUCTURE_ELEMENT.SetActive(true);

        Vector3 newScale = new Vector3(.01f,.01f,.01f);
        STRUCTURE_ELEMENT.transform.localScale = newScale;

        // position of the placed structure model in AR 
        Vector3 newPose = new Vector3(
            m_ProtienAnchor.transform.position.x,
            m_ProtienAnchor.transform.position.y,
            m_ProtienAnchor.transform.position.z
        );
        STRUCTURE_ELEMENT.transform.position = newPose;

        // parent
        STRUCTURE_ELEMENT.transform.parent = m_ProtienAnchor.transform;

        float animationTime = 1.2f;
        LeanTween.moveY(STRUCTURE_ELEMENT,m_ProtienAnchor.transform.position.y + .15f,animationTime).setEaseOutBack();
        newScale = new Vector3(.05f,.05f,.05f);
        LeanTween.scale(STRUCTURE_ELEMENT,newScale,animationTime).setEaseOutBack();
        // Vector3 newRot = new Vector3(-37f,-0.7f,-14f);
        Vector3 newRot = new Vector3(-10f,-122f,-132f);
        LeanTween.rotate(STRUCTURE_ELEMENT,newRot,animationTime).setEaseOutBack();

    }

}
