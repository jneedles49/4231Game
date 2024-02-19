using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate : MonoBehaviour
{

    //Everything except the activate function in this script is placeholder, can and should be deleted in the near future
    private string SelfTag;
    private Rigidbody rg; 
    // Start is called before the first frame update
    void Start()
    {
        SelfTag = this.tag;
        //DELETE THIS
        rg = this.GetComponent<Rigidbody>();
        rg.useGravity = false; 
    }
    public void activate(){
        //This switch case should be changed and added on to
        switch(SelfTag){

               case "Pickup":
                    rg.useGravity = true;
               break;

        }
    }
}
