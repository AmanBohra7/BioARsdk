namespace GoogleARCore.Examples.Common{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;

    public class DPV_test : MonoBehaviour{
        private DetectedPlane m_DetectedPlane;
        private List<Vector3> m_PreviousFrameMeshVertices = new List<Vector3>();
        private List<Vector3> m_MeshVertices = new List<Vector3>();
        private Vector3 m_PlaneCenter = new Vector3();

        private List<Color> m_Meshcolors = new List<Color>();

        private List<int> m_MeshIndices = new List<int>();

        private Mesh m_Mesh;
        private MeshRenderer m_MeshRenderer;

        public void Awake(){
            m_Mesh = GetComponent<MeshFilter>().mesh;
            m_MeshRenderer = GetComponent<UnityEngine.MeshRenderer>();
        }

        public void Update(){
            if(m_DetectedPlane == null) return;
            else if(m_DetectedPlane.SubsumedBy != null){
                Destroy(gameObject);
                return;
            }
            else if(m_DetectedPlane.TrackingState != TrackingState.Tracking){
                m_MeshRenderer.enabled = false;
                return;
            }
            m_MeshRenderer.enabled = true;
            // _UpdateReq();
        }

        public void Initialize(DetectedPlane plane){
            m_DetectedPlane = plane;
            m_MeshRenderer.material.SetColor("_GridColor",Color.green);
            m_MeshRenderer.material.SetFloat("_UvRotation",Random.Range(0.0f,360.0f));
            Update();
        }



    }

}