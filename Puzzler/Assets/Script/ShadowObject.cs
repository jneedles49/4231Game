using UnityEngine;

public class ShadowObject : MonoBehaviour
{
    [SerializeField] private GameObject ConnectedObject;

    [SerializeField] private bool Move_X;
    [SerializeField] private bool Move_Y;
    [SerializeField] private bool Move_Z;

    private float X_Constant;
    private float Y_Constant;
    private float Z_Constant;

    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //We get our constants once the game starts and then we are able to dynamically constrain our object if we need to
        X_Constant = this.transform.position.x;
        Y_Constant = this.transform.position.y;
        Z_Constant = this.transform.position.z;
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
        if(Move_X) new_X = ConnectedObj_position.x;
        else new_X = X_Constant; 

        if(Move_Y) new_Y = ConnectedObj_position.y;
        else new_Y = Y_Constant; 

        if(Move_Z) new_Z = ConnectedObj_position.z;
        else new_Z = Z_Constant; 

        //This is our new position we will be moving to
        Vector3 new_pos = new Vector3(new_X, new_Y, new_Z);
        
        //This is the actual movement from where we are now to where we need to go
        this.transform.position = Vector3.SmoothDamp(this.transform.position, new_pos, ref velocity, .1f);
        
    }
}