using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

#if UNITY_EDITOR
     using Input = GoogleARCore.InstantPreviewInput;
#endif

public class SceneController : MonoBehaviour
{

    private int screenHeight;
    private int screenWidth;

    private bool m_PrefabObjectCreated = false;
    private bool m_ProteinPlaced = false;

    public GameObject m_PrefabObject;
    private GameObject m_TrackerObjectInstance;

    public GameObject m_ProteinPrefab;
    private GameObject m_ProteinObject;

    Anchor m_ProtienAnchor = null;

    public Camera FirstPersonCamera;
    // Start is called before the first frame update

    void Start(){
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        QuitOnConnectionErrors();
    }

    void QuitOnConnectionErrors()
    {
        if (Session.Status ==  SessionStatus.ErrorPermissionNotGranted)
        {
            Debug.Log(
                "Camera permission is needed to run this application.");
        }
        else if (Session.Status.IsError())
        {

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
                        if(!m_PrefabObjectCreated){
                            m_TrackerObjectInstance = Instantiate(m_PrefabObject,
                                        Vector3.zero,
                                        m_PrefabObject.transform.rotation);
                            m_TrackerObjectInstance.transform.position =
                                    hit.Pose.position;
                            m_PrefabObjectCreated = true;
                        }else{
                        if(!m_ProteinPlaced) m_TrackerObjectInstance.transform.position =
                                    hit.Pose.position;
                        }
                        if(Input.touchCount > 0 && m_PrefabObjectCreated && !m_ProteinPlaced){
                            m_ProteinPlaced = true;
                             Vector3 updateTransform = new Vector3(
                                hit.Pose.position.x,
                                hit.Pose.position.y + 0.125f,
                                hit.Pose.position.z
                            );
                            m_ProteinObject = Instantiate(m_ProteinPrefab,
                                    updateTransform,
                                    m_ProteinPrefab.transform.rotation);
                           
                            m_ProtienAnchor = hit.Trackable.CreateAnchor(hit.Pose);
                            m_ProteinObject.transform.parent = m_ProtienAnchor.transform;
                            Debug.Log(hit.Pose);
                            Debug.Log(m_ProteinObject.transform.position);
                            m_ProteinObject.GetComponent<ProteinContoller>().InitializeContollers();
                            Destroy(m_TrackerObjectInstance);
                            m_TrackerObjectInstance.SetActive(false);
                        }
                    }
                }
    }
}
