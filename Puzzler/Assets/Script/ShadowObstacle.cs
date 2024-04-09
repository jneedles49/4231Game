using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowObstacle : MonoBehaviour
{

    [SerializeField] private bool Move_X;
    [SerializeField] private bool Move_Y;
    [SerializeField] private bool Move_Z;

    private float X_Constant;
    private float Y_Constant;
    private float Z_Constant;

    private float delta_x;
    private float delta_y;
    private float delta_z;

    private Rigidbody rg;
    private Vector3 velocity = Vector3.zero;
    private Transform trans;

    // Start is called before the first frame update
    void Start()
    {
        //We get our constants once the game starts and then we are able to dynamically constrain our object if we need to
        X_Constant = this.transform.position.x;
        Y_Constant = this.transform.position.y;
        Z_Constant = this.transform.position.z;
        rg = this.GetComponent<Rigidbody>();
	    trans = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
