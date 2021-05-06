using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
     using Input = GoogleARCore.InstantPreviewInput;
#endif

public class PlaneCenterPoseFinder : MonoBehaviour{
    
    private int screenHeight;
    private int screenWidth;

    private bool m_PrefabObjectCreated = false;

    public GameObject m_PrefabObject;
    private GameObject m_ObjectInstance;

    public Camera FirstPersonCamera;

    public void Update(){
        Vector2 centerPose = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f);
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;
            if (Frame.Raycast(centerPose.x, centerPose.y, raycastFilter, out hit)){
                if(hit.Trackable is DetectedPlane){
                    if(!m_PrefabObjectCreated){
                        m_ObjectInstance = Instantiate(m_PrefabObject,
                                    Vector3.zero,
                                    m_PrefabObject.transform.rotation);
                        m_ObjectInstance.transform.position =
                                hit.Pose.position;
                    }else{
                        m_ObjectInstance.transform.position =
                                hit.Pose.position;
                    }
                }
            }
    }

}
