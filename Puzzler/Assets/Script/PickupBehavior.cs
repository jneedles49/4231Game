using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickupBehavior : MonoBehaviour
{
    private bool pickedUp = false;
    private GameObject player;
    private Vector3 holdPos;
    private float holdForceMult = 0.5f;
    private Rigidbody rg;

    [Header ("Audio")]
    private AudioSource audioController;
    [SerializeField] private AudioClip CollisionSound;

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
            //if the object get's too far from the player then it just locked to the furthest postion away it can get
            //we do this by creating a new vector which is the objects postion clamped to within 2.1 units of the player
                //The issue is that for some reason the transform get's clamped to the max value too much? that it inverts to the lowest value
            this.transform.position = new Vector3(
                Mathf.Clamp(this.transform.position.x, player.transform.position.x - 2.5f, player.transform.position.x + 2.5f),
                Mathf.Clamp(this.transform.position.y, player.transform.position.y - 2.5f, player.transform.position.y + 2.5f),
                Mathf.Clamp(this.transform.position.z, player.transform.position.z - 2.5f, player.transform.position.z + 2.5f)
            );

            rg.velocity = new Vector3(
                Mathf.Clamp(rg.velocity.x, -5.0f, 5.0f),
                Mathf.Clamp(rg.velocity.y, -5.0f, 5.0f),
                Mathf.Clamp(rg.velocity.z, -5.0f, 5.0f)
            );

            this.transform.rotation = new Quaternion(0,0,0,0);

            //Debug.Log("Velocity: " + rg.velocity);

        }

        #endregion
    }

    void FixedUpdate(){

        #region Object movement

        //We don't have to worry about the player var not being set since it will have to be instantated by the time it gets here
        //There is a potential edge-case if we destroy the player so let's just not
        if(pickedUp){

            //  I'm only instantiating variables for readability
            Obj_Interaction playerInteract = player.GetComponent<Obj_Interaction>();
            float rayEndpoint = playerInteract.playerInteractionRadius;

            //Debug.Log("Endpoint: " + rayEndpoint);

            Ray interactRay = playerInteract.interactRay; 
            Vector3 holdPos = interactRay.GetPoint(rayEndpoint);

            //Debug.Log("Object is gravitating to: " + holdPos.ToString());
            
            //Getting the vectory from the objects current position to the player's hold position.
            Vector3 directionToHoldPos = (holdPos - this.transform.position).normalized;

            //Think of this as the direction to hold position with the object moving at MAX speed.
            Vector3 holdForce = directionToHoldPos*holdForceMult;


            //This is self explanatory 
            float distanceToHoldPos = Vector3.Distance(holdPos, this.transform.position);

            //Clamping the value, so we adjust objects' translation speed based on the distance 
            float clampedDistance = Mathf.Clamp(distanceToHoldPos, 0.0f, 1.0f);

            //Debug.Log("Interactable Object, Distance to Hold Postion: " + distanceToHoldPos);


            //We influence the hold force by both the actual distance and the clamped distance to achieve a slighty better feel
            holdForce *= (distanceToHoldPos * .5f) + (clampedDistance * .5f);

            //Debug.Log("Hold Force: " + holdForce);

            //If object is close enough to the hold position we just translate right to it. Not Perfect but good enough
            if(clampedDistance > .45)rg.AddForce(holdForce,ForceMode.VelocityChange);
            else rg.MovePosition(holdPos);

        }

        #endregion



    }

    // This function wakes the object from it's inactve (not picked up state) 
    // it also retrieves the player object which we need to retrieve the current location of the hold position, 
    // and it also sves us the headaches of trying to find the player since we know they are in the scene because they just picked up something 
    public void pickup(GameObject player)
    {
        pickedUp = !pickedUp;
        //rg.useGravity = !rg.useGravity;
        this.player = player;
        //Debug.Log("You picked up the object!");
    }

    private void OnTriggerEnter(Collider other) {

        audioController.clip = CollisionSound;
        playCollisionSound();

    }

    public void playCollisionSound(){

        if(!audioController.isPlaying) audioController.Play();

    }

}