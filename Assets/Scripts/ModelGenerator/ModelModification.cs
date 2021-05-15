using UnityEngine;
using System.Collections;

public class ModelModification : MonoBehaviour{
   
    public GameObject stParent;    

    // n rings - 2 axis 

    //  [ 0 , 90 , 45 , -45  ]

    void Start(){
        StartCoroutine(AnimateSt(2f));
    }


    IEnumerator AnimateSt(float watTime){
        yield return new WaitForSeconds(watTime);

        //  not up , forword
        // LeanTween.rotateAroundLocal(stParent,stParent.transform.right,360f,2.2f).setRepeat(999);
        // LeanTween.rotateAroundLocal(stParent,stParent.transform.forward,360f,2.2f).setRepeat(999);

        print(stParent.transform.childCount.ToString() + " - TEST");
        for(int i = 0 ; i < stParent.transform.childCount ; ++i){
                GameObject child = stParent.transform.GetChild(i).gameObject;
                if(child.tag == "Ring"){
                        RingRotation(child,i);
                        // rotateRing = true;
                        // break;
                }
            }


    }
    

    public void RingRotation(GameObject ringObj,int i){

        print("In function!");
        // x axis
        float angle = (i%2==0)  ? -360f : 360f ;
        LeanTween.rotateAroundLocal(ringObj,ringObj.transform.up,angle,2.2f).setRepeat(999).setDelay(Random.Range(2, 4));

    }   

}
