using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class PickupBehavior : MonoBehaviour
{
    private bool pickedUp = false;
    private GameObject player;

    //Physics
    private Vector3 holdPos;
    private float holdForceMult = 0.5f;
    private Rigidbody rg;
    private Quaternion LockRotation = new Quaternion();
    private float ThrowForce = 15.0f;

    [Header ("Audio")]
    private AudioSource audioController;
    [SerializeField] private AudioClip CollisionSound;

    //Connected SHADOW OBJECT
    public bool shadowInterrupt = false; 
    public bool greaterOrSmaller = false;
    public Vector3 ShadowOverride;
    public Vector3 ShadowNormal;

    // Start is called before the first frame update
    void Start()
    {

        rg = GetComponent<Rigidbody>();
        audioController = GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update()
    {
        #region Object movement constraints

        //This isn't a perfect way to clamp the objects postition but it's getting there
        if (pickedUp)
        {

            //Clamping the affect of gravity and maximum and minimum force applied to the object
            rg.velocity = new Vector3(
                Mathf.Clamp(rg.velocity.x, -5.0f, 5.0f),
                Mathf.Clamp(rg.velocity.y, -5.0f, 5.0f),
                Mathf.Clamp(rg.velocity.z, -5.0f, 5.0f)
            );
            


            #region Player Rotating Object
            //NOTE: Object still seems to spin when the right mouse button is just held down sometimes, need to do more testing
            if(Input.GetKey(KeyCode.Mouse1)){

                //Getting player input
                float yawMouse = Input.GetAxis("Mouse X");
                float pitchMouse = Input.GetAxis("Mouse Y");

                if(Mathf.Abs(yawMouse) > .2f || Mathf.Abs(pitchMouse) > .2f){

                    //Locking down rotation to just the object because the rigid body somehow starts applying angular velocity onto itself
                    rg.freezeRotation = true;

                    transform.Rotate(Vector3.up, -yawMouse * 5.0f, Space.World);
                    transform.Rotate(Vector3.right, pitchMouse * 5.0f, Space.World);
                }

                LockRotation = transform.rotation;

            }
            else {

                transform.rotation = LockRotation;

            }
            

            #endregion


        }

        #endregion

            //Resetting Rotation for when object is not being held
            LockRotation = transform.rotation;
    }

    void FixedUpdate(){

        #region Object movement

        //We don't have to worry about the player var not being set since it will have to be instantated by the time it gets here
        //There is a potential edge-case if we destroy the player so let's just not
        if(pickedUp){

            //  I'm only instantiating variables for readability
            Obj_Interaction playerInteract = player.GetComponent<Obj_Interaction>();
            float rayEndpoint = playerInteract.playerInteractionRadius;


            Ray interactRay = playerInteract.interactRay; 
            holdPos = interactRay.GetPoint(rayEndpoint);

            
            //Getting the vectory from the objects current position to the player's hold position.
            Vector3 directionToHoldPos = (holdPos - this.transform.position).normalized;

            //Think of this as the direction to hold position with the object moving at MAX speed.
            Vector3 holdForce = directionToHoldPos*holdForceMult;


            //This is self explanatory 
            float distanceToHoldPos = Vector3.Distance(holdPos, this.transform.position);

            //Clamping the value, so we adjust objects' translation speed based on the distance 
            float clampedDistance = Mathf.Clamp(distanceToHoldPos, 0.0f, 1.0f);


            //We influence the hold force by both the actual distance and the clamped distance to achieve a slighty better feel
            holdForce *= (distanceToHoldPos * .5f) + (clampedDistance * .5f);

            //You can't move past the shadow box when it notifies that there has been collision 

            //IDK This kind of works better than the Rigid body add force method I was trying before
            
            //NOTE: THIS WORKS SOMEWHAT AS INTENDED, THERE IS STUTTERING I would assume because of the number of calculations. Being Done and the amount of resources it uses.
            //We could work to solve this by instead doing this computation elsewhere, and implementing essentially a cooldown on the collision enter as it can register a collision enter multiple times in a short span 
            //Needs Testing and work done to ensure more stability
            if(!shadowInterrupt) rg.transform.position = Vector3.SmoothDamp(this.transform.position, holdPos, ref holdForce, .02f);
            else{
                Vector3 position = this.transform.position;

                //Determining how to clamp
                if(ShadowOverride.x == 0) ShadowOverride.x = holdPos.x;
                else{

                    if(ShadowNormal.x > 0 && holdPos.x > ShadowOverride.x) ShadowOverride.x = holdPos.x;
                    if(ShadowNormal.x < 0  && holdPos.x < ShadowOverride.x) ShadowOverride.x = holdPos.x;


                }

                if(ShadowOverride.y == 0) ShadowOverride.y = holdPos.y;
                else{

                    if(ShadowNormal.y > 0 && holdPos.y > ShadowOverride.y) ShadowOverride.y = holdPos.y;
                    if(ShadowNormal.y < 0  && holdPos.y < ShadowOverride.y) ShadowOverride.y = holdPos.y;


                }

                if(ShadowOverride.z == 0) ShadowOverride.z = holdPos.z;
                else{

                    if(ShadowNormal.z > 0 && holdPos.z > ShadowOverride.z) ShadowOverride.z = holdPos.z;
                    if(ShadowNormal.z < 0  && holdPos.z < ShadowOverride.z) ShadowOverride.z = holdPos.z;

                }

                
                rg.transform.position = Vector3.SmoothDamp(this.transform.position, ShadowOverride, ref holdForce, .02f);


            } 
 
            
            //rg.transform.position = Vector3.SmoothDamp(this.transform.position, holdPos, ref holdForce, .02f);

        }

        #endregion



    }

    // This function wakes the object from it's inactve (not picked up state) 
    // it also retrieves the player object which we need to retrieve the current location of the hold position, 
    // and it also sves us the headaches of trying to find the player since we know they are in the scene because they just picked up something 
    public void pickup(GameObject player)
    {
        pickedUp = !pickedUp;
        //Renabling rigidbody rotations when the object is either picked up or dropped for safety's sake
        rg.freezeRotation = false;
        this.player = player;
        if (!pickedUp){
           rg.AddForce(Vector3.up * 2.5f, ForceMode.VelocityChange); 
         } 
    }

    private void OnTriggerEnter(Collider other) {

        //We want to implement a switch statement here so that we can change sound based on the surface it is hitting

        audioController.clip = CollisionSound;
        playCollisionSound();

    }

    public void playCollisionSound(){

        if(!audioController.isPlaying) audioController.Play();

    }

    public void ThrowInDirection(Vector3 Direction){


        pickedUp = false;
        //Renabling rigidbody rotations when the object is either picked up or dropped for safety's sake
        rg.freezeRotation = false;
        if (!pickedUp){
           rg.AddForce(Direction * ThrowForce, ForceMode.Impulse); 
         } 

    }


}