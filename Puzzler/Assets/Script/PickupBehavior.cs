using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class PickupBehavior : MonoBehaviour
{
    private bool pickedUp = false;
    //private GameObject player;
    private Transform trans;

    private Rigidbody rg;
    private Quaternion LockRotation = new Quaternion();

    [Header ("Audio")]
    private AudioSource audioController;
    [SerializeField] private AudioClip CollisionSound;

    private Obj_Interaction playerInteract;

    // Start is called before the first frame update
    void Start()
    {

        rg = GetComponent<Rigidbody>();
        audioController = GetComponent<AudioSource>();
	trans = this.transform;

        
    }

    // Update is called once per frame
    void Update()
    {
        #region Object movement constraints

        //This isn't a perfect way to clamp the objects postition but it's getting there
        if (pickedUp)
        {


            #region Player Rotating Object
            //NOTE: Object still seems to spin when the right mouse button is just held down sometimes, need to do more testing
            if(Input.GetKey(KeyCode.Mouse1)){

                //Getting player input
                float yawMouse = Input.GetAxis("Mouse X");
                float pitchMouse = Input.GetAxis("Mouse Y");

                if(Mathf.Abs(yawMouse) > .2f || Mathf.Abs(pitchMouse) > .2f){

                    trans.Rotate(Vector3.up, -yawMouse * 5.0f, Space.World);
                    trans.Rotate(Vector3.right, pitchMouse * 5.0f, Space.World);
                }

                LockRotation = trans.rotation;

            }
            else {

                trans.rotation = LockRotation;

            }
            

            #endregion


        }

        #endregion

            //Resetting Rotation for when object is not being held
            LockRotation = trans.rotation;
    }

/* NOTE WE MAY NOT NEED ANY OF THIS ANYMORE 
    void FixedUpdate(){

        #region Object movement

        //We don't have to worry about the player var not being set since it will have to be instantated by the time it gets here
        //There is a potential edge-case if we destroy the player so let's just not
	
        if(pickedUp){

		//Setup
            //  I'm only instantiating variables for readability
		if(!playerInteract)playerInteract = player.GetComponent<Obj_Interaction>();
		float rayEndpoint = playerInteract.playerInteractionRadius;

		Ray interactRay = playerInteract.interactRay; 
		Vector3 holdPos = interactRay.GetPoint(rayEndpoint);

		Current_Distance = Vector3.Distance(holdPos, trans.position);

		if(Current_Distance > Break_Distance) drop();

		Vector3 Move_Direction = holdPos - trans.position;
		rg.AddForce(Move_Direction.normalized * Hold_Force);

		Debug.DrawRay(trans.position, Move_Direction.normalized, Color.green);
	
        }

        #endregion


    }
	*/

    // This function wakes the object from it's inactve (not picked up state) 
    // it also retrieves the player object which we need to retrieve the current location of the hold position, 
    // and it also soves us the headaches of trying to find the player since we know they are in the scene because they just picked up something 
    public void pickup()
    {
        pickedUp = true;
        rg.freezeRotation = false;
	rg.useGravity = false;
	rg.drag = 10;

    }


    public void drop(){

	    pickedUp = false;
	    rg.freezeRotation = false;
	    rg.useGravity = true;
	    rg.drag = 1;
		
	    rg.AddForce(Vector3.up * 2.5f, ForceMode.VelocityChange); 

    }

    public void ThrowInDirection(Vector3 Direction, float throwForce){

	    this.drop();

	    if (!pickedUp){
		    rg.AddForce(Direction * throwForce, ForceMode.Impulse); 
	    } 

    }

    private void OnTriggerEnter(Collider other) {

        audioController.clip = CollisionSound;
        if(!audioController.isPlaying) audioController.Play();

    }

}
