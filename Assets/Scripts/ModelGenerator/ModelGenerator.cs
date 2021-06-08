using UnityEngine;
using System.IO;
using Newtonsoft;
using System;
using System.Collections.Generic;
using CustomUtils;

public class ModelGenerator : MonoBehaviour
{

    // Others
    public GameObject ePanel;
    public GameObject nPanel;

    // model parent variable
    public GameObject structureParent;

    //  Model components variables
    public GameObject m_nucleusPrefab;
    public GameObject m_electronPrefab; 
    public List<GameObject> m_ringsPrefab;


    void Start(){
    

        int num = ElectrionDectectionUIManager.SHARED_STRUCTURE.number;
        Debug.Log("PRINT NUM : "+num.ToString());
        if(num != 0)
            CreateStructure(num);
        else{
            CreateStructure(11);
            Debug.Log("NO received value from screen shot script!");
        }
          
        // CreateStructure(8);
    }


    ///<summary>
    ///Extract data from json file and extract data of pass value of number 
    ///of electrons and calls GenerateStructureModel with Structure variable
    ///</summary>
    ///<param name="num_elec">num_elec</param>
    public void CreateStructure(int num_elec){

        string jsonstring = UtilsClass.GetElementData(num_elec);
        // Debug.Log(jsonstring);
        try{
            Structure st = Newtonsoft.Json.JsonConvert.DeserializeObject<Structure>(jsonstring);
            GenerateStructureModel(st);

            // disable all electrons gizmos 
            UtilsClass.ToggleGizmos(
                structureParent,
                false
            );

            // GenerateStructureModel(ElectrionDectectionUIManager.SHARED_STRUCTURE);
        }
        catch(Exception e){
            Debug.Log("JSON DATA HAVE SOME VALUE ERROR!");
        }

    }


    ///<summary>
    ///main function for generating entire structure model of provided structure variable
    ///</summary>
    ///<param name="mol">mol</param>
    public void GenerateStructureModel(Structure mol){

        // GameObject parentObj = new GameObject(mol.name);
        // parentObj.transform.position = Vector3.zero;

        CreateNucleus(structureParent.transform.position,structureParent.transform);
        // create electron function is called in createShellRing function 
        CreateShellRing(structureParent.transform.position,structureParent.transform,mol.shells);

    }

    void CreateNucleus(Vector3 pos,Transform parentTrans){
        GameObject tmp =  Instantiate(
            m_nucleusPrefab,
            pos,
            m_nucleusPrefab.transform.rotation,
            parentTrans
        );

        GameObject tempObj = Instantiate(
            nPanel,
            pos,
            nPanel.transform.rotation
        );
        tempObj.transform.parent = tmp.transform;
        tempObj.transform.localScale = new Vector3(
            tempObj.transform.localScale.x / 4f,
            tempObj.transform.localScale.y / 4f,
            tempObj.transform.localScale.z / 4f
        );
        tempObj.GetComponent<Canvas>().worldCamera = Camera.main;

    }

    void CreateElectron(GameObject ringObject,int num_ele){

        BoxCollider boxCollider = ringObject.GetComponent<BoxCollider>();
        float radius = (boxCollider.size.x / 2) - 0.05f;
        for(int i = 0 ; i < num_ele ; i++){        
            float x =  radius * Mathf.Cos(2 * Mathf.PI * i / num_ele); 
            float z =  radius * Mathf.Sin(2 * Mathf.PI * i / num_ele);
            GameObject elec = Instantiate(
                m_electronPrefab,
                new Vector3(x,structureParent.transform.position.y,z),
                m_electronPrefab.transform.rotation,
                ringObject.transform
            );  

            // generating gizmos
            CreateElecPanel(
                new Vector3(x,structureParent.transform.position.y,z),
                elec.transform
            );
        }

    }


    void CreateElecPanel(Vector3 ePose,Transform eParent){
        GameObject tempObj = Instantiate(
            ePanel,
            ePose,
            ePanel.transform.rotation,
            eParent
        );
        tempObj.GetComponent<Canvas>().worldCamera = Camera.main;
    }


    void CreateShellRing(Vector3 pos,Transform parentTrans,List<int> shellList){
       
        int num = shellList.Count;
        for(int i = 0 ; i < num ; ++i){
            GameObject tmp = Instantiate<GameObject>(
               m_ringsPrefab[i],
               pos,
               m_ringsPrefab[i].transform.rotation,
               parentTrans
            );
            CreateElectron(tmp,shellList[i]);
        }
    }

}
