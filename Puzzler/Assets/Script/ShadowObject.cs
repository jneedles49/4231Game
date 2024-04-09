using UnityEngine;
using System.Collections;

public class ShadowObject : MonoBehaviour
{
    [SerializeField] private GameObject ConnectedObject;
    private PickupBehavior ConnectedObj_PickupBehavior;

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

        //Connecting shadow objcet to pickup object
        ConnectedObj_PickupBehavior = ConnectedObject.GetComponent<PickupBehavior>();

    }

    void FixedUpdate()
    {

        //These will become our paramaters when we create our new Vector3 position
        float new_X;
        float new_Y;
        float new_Z;

        //We take the connected objects position once every updeate 
        Vector3 ConnectedObj_position = ConnectedObject.transform.position;

        //We set our new X,Y,Z to the connected objects corresponding value or the constant
        if(Move_X){ 
            new_X = ConnectedObj_position.x;
        }
        else new_X = X_Constant; 

        if(Move_Y){
            new_Y = ConnectedObj_position.y;
        }
        else new_Y = Y_Constant; 

        if(Move_Z){
            new_Z = ConnectedObj_position.z;
        }
        else new_Z = Z_Constant; 

        //This is our new position we will be moving to
        Vector3 new_pos = new Vector3(new_X, new_Y, new_Z); 


	Ray Direction = new Ray(trans.position, Vector3.Normalize(new_pos) * 2);


	Debug.DrawRay(Direction.origin, Direction.direction, Color.blue);

        //This is the actual movement from where we are now to where we need to go
        trans.position = Vector3.SmoothDamp(this.transform.position, new_pos, ref velocity, .3f);

    }


    
    private void OnCollisionEnter(Collision other) {

        Debug.Log("Collided");

        ConnectedObj_PickupBehavior.shadowInterrupt = true;

        //Getting the first cotact point to try and know the direction of the collision
        Vector3 ContactNormal = other.GetContact(0).normal;
        Vector3 position = this.transform.position;

        float new_x;
        float new_y;
        float new_z;

        if((ContactNormal.x < 0) || (ContactNormal.x > 0)) new_x = position.x; 
        else new_x = 0; 
        
        if((ContactNormal.y < 0) || (ContactNormal.y > 0)) new_y = position.y; 
        else new_y = 0;

        if((ContactNormal.z < 0) || (ContactNormal.z > 0)) new_z = position.z; 
        else new_z = 0;

        ConnectedObj_PickupBehavior.ShadowOverride = new Vector3(new_x, new_y, new_z); 
        ConnectedObj_PickupBehavior.ShadowNormal = ContactNormal;

        Debug.Log("Normal of the Contact Point: " + ContactNormal);

    }

    private void OnCollisionExit(Collision other) {

        ConnectedObj_PickupBehavior.shadowInterrupt = false;

        Debug.Log("Left Collision");


    }


}
