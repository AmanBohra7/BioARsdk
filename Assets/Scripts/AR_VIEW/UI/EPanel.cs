using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class EPanel : MonoBehaviour
{
    public TextMeshProUGUI namePanel;
    public TextMeshProUGUI configPanel;

    void Start(){
        if(namePanel)
            namePanel.text = ElectrionDectectionUIManager.SHARED_STRUCTURE.name;
        
        if(configPanel){

            List<int> elec_count = ElectrionDectectionUIManager.SHARED_STRUCTURE.shells;

            string str = "";
            for(int i = 0 ; i < elec_count.Count ; ++i){
                str += elec_count[i];
                if(i != (elec_count.Count - 1))
                    str += " , ";
            }

            configPanel.text = str;
        }
            

    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
