using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Obj_Interaction : MonoBehaviour
{
    public GameObject cube;
    public Camera mainCamera;
    public Vector3 pos;
    public readonly float playerInteractionRadius = 2.0f;
    private int objectLayerMask = 1 << 6;
    public Ray interactRay;
    private RaycastHit hit;
    private float mouseClickCooldown_remaining = 0f;
    private readonly float mouseClickCooldown = 1.0f;
    private bool holdingObj = false;
    GameObject obj;
    public GameObject reticle;
    private RectTransform reticleTransform;
    private RawImage reticleColor;
    private readonly Color HighlightColor = Color.white;
    private readonly Color normalColor = Color.grey;
    private readonly Vector3 ReticleMaxSize = new Vector3(.3f,.3f,.3f);
    private readonly Vector3 ReticleMinSize = new Vector3(.15f,.15f,.15f);

    // Start is called before the first frame update
    void Start()
    {

        //gets the screen width and height
       pos = new Vector3(Screen.width/2, Screen.height/2, 7);
       reticleTransform = reticle.GetComponent<RectTransform>();
       reticleColor = reticle.GetComponent<RawImage>();

        
    }

    //Called once every froma as dictated by the project
    private void FixedUpdate() {

        //is a ray that projects from the center of the screen, no matter the resolution
        interactRay = mainCamera.ScreenPointToRay(pos);

        //Looking to see if ray hits an interactable object
        if (Physics.Raycast(interactRay, playerInteractionRadius, objectLayerMask) || holdingObj)
        {


            //Adjust the size and color of the recticle to highlight when it is over a interactable object
            if (reticleTransform.localScale.x != ReticleMaxSize.x)
            {
                reticleTransform.localScale = ReticleMaxSize;
                reticleColor.color = HighlightColor;
            }

        } else{

            //if the player is not looking at an interactable object then we set the reticle back to normal
            if (reticleTransform.localScale.x != ReticleMinSize.x)
            {
                reticleTransform.localScale = ReticleMinSize;
                reticleColor.color = HighlightColor;
            }

        }

        //If the player clicks the left mouse button and the physics raycast hits a collider on physics layer 6 then we enter into the if statement
        if (Input.GetMouseButton(0) && mouseClickCooldown_remaining == 0.0f)
        {

            //if the player is not holding an object then we call a raycast and get the colliders
            if (!holdingObj)
            {

                if (Physics.Raycast(interactRay, out hit, playerInteractionRadius, objectLayerMask))
                {
                    // If the cooldown of the players mouse click is zero, then we print a message, then restart the cooldown
                    if (hit.collider)
                    {
                        //print("You clicked on the box");
                        //Get the game object from the RaycastHit and get the interactable object script component
                        obj = hit.transform.gameObject;
                        obj.GetComponent<PickupBehavior>().pickup(this.gameObject);
                        holdingObj = true;

                        mouseClickCooldown_remaining = mouseClickCooldown;
                    }
                }
            }
            //if the player is holding an object then we just call the current objects pickup script, dropping the object, and resettinga cooldown
            else
            {

                obj.GetComponent<PickupBehavior>().pickup(this.gameObject);
                holdingObj = false;

                mouseClickCooldown_remaining = mouseClickCooldown;
            }
        }


        //if the left mouse click cooldown is greater than zero than we dercement the cooldown 
        if (mouseClickCooldown_remaining > 0.0f)
        {
            mouseClickCooldown_remaining -= .1f;
            //print("Click Cooldown: " + mouseClickCooldown_remaining);
        }
        //if the cooldown somehow gets below 0 than we just est it to zero
        else if (mouseClickCooldown_remaining < 0.0f) mouseClickCooldown_remaining = 0.0f;

        Debug.DrawRay(interactRay.origin, interactRay.direction * playerInteractionRadius, Color.blue);

    }

    // Update is called once per frame
    void Update()
    {


    }


}
