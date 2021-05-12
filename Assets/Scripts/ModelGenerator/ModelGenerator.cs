using UnityEngine;
using System.IO;
using Newtonsoft;
using System;
using System.Collections.Generic;
using CustomUtils;

public class ModelGenerator : MonoBehaviour
{

    public GameObject m_nucleusPrefab;
    public GameObject m_electronPrefab; 
    public List<GameObject> m_ringsPrefab;


    void Start(){
        // The value in the function will be changed according to the input from 
        // electron detect scene
        CreateStructure(3);
    }


    ///<summary>
    ///Extract data from json file and extract data of pass value of number 
    ///of electrons and calls GenerateStructureModel with Structure variable
    ///</summary>
    ///<param name="num_elec">num_elec</param>
    public void CreateStructure(int num_elec){

        string jsonstring = UtilsClass.GetElementData(num_elec);
        Debug.Log(jsonstring);
        try{
            Structure st = Newtonsoft.Json.JsonConvert.DeserializeObject<Structure>(jsonstring);
            GenerateStructureModel(st);
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

        GameObject parentObj = new GameObject(mol.name);
        parentObj.transform.position = Vector3.zero;

        CreateNucleus(new Vector3(0,0,0),parentObj.transform);
        // create electron function is called in createShellRing function 
        CreateShellRing(new Vector3(0,0,0),parentObj.transform,mol.shells);

    }

    void CreateNucleus(Vector3 pos,Transform parentTrans){
        Instantiate(
            m_nucleusPrefab,
            pos,
            m_nucleusPrefab.transform.rotation,
            parentTrans
        );
    }

    void CreateElectron(GameObject ringObject,int num_ele){

        BoxCollider boxCollider = ringObject.GetComponent<BoxCollider>();
        float radius = (boxCollider.size.x / 2) - 0.05f;
        for(int i = 0 ; i < num_ele ; i++){        
            float x =  radius * Mathf.Cos(2 * Mathf.PI * i / num_ele); 
            float y =  radius * Mathf.Sin(2 * Mathf.PI * i / num_ele);
            Instantiate(
                m_electronPrefab,
                new Vector3(x,0,y),
                m_electronPrefab.transform.rotation,
                ringObject.transform
            );
        }

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
