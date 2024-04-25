using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Obj_Interaction : MonoBehaviour
{

    #region Character Attributes
    
    [Header ("Character Attributes")]
    
    [SerializeField] private Camera mainCamera;

    private Vector3 pos;
    private int objectLayerMask = 1 << 6;

    private float mouseClickCooldownRemaining = 0f;
    private readonly float mouseClickCooldown = 1.0f;

    #endregion

    #region Object Interaction

    [Header ("Interaction Paramaters")]

    private float CURRENT_HoldDistance;
    private readonly float DEFAULT_HoldDistance = 1.5f;
    private readonly float MIN_HoldDistance = .5f;
    private readonly float MAX_HoldDistance = 2.00f;
    private readonly float HoldDistance_Increment = .1f;

    [SerializeField] private float Hold_Force = 20f;
    [SerializeField] private float ThrowForce = 15.0f;
    [SerializeField] private float Break_Distance = 5f;

    public Ray interactRay;
    private RaycastHit hit;
    private bool holdingObj = false;
    private GameObject Held_Obj;
    private Rigidbody HeldObject_Rigidbody;
    private float HeldObject_Distance = 0f;
    private Transform HeldObject_Transform;

    #endregion

    #region HUD

    [Header ("HUD")]

	    [SerializeField] private HudManager Hud;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        //gets the screen width and height
        pos = new Vector3(Screen.width / 2, Screen.height / 2, 7);

    }

    //Called once every froma as dictated by the project
    private void FixedUpdate() {

        //is a ray that projects from the center of the screen, no matter the resolution
        interactRay = mainCamera.ScreenPointToRay(pos);


        #region Object interaction

        //If the player clicks the left mouse button and the physics raycast hits a collider on physics layer 6 then we enter into the if statement
        if (Input.GetMouseButton(0) && mouseClickCooldownRemaining == 0.0f) // Left Mouse
        {

            //if the player is not holding an object then we call a raycast and get the colliders
            if (!holdingObj)
            {

                if (Physics.Raycast(interactRay, out hit, CURRENT_HoldDistance, objectLayerMask))
                {
                    // If the cooldown of the players mouse click is zero, then we print a message, then restart the cooldown
                    if (hit.collider)
                    {
                        //Get the game object from the RaycastHit and get the interactable object script component
                        Held_Obj = hit.transform.gameObject;
                        switch(Held_Obj.tag){
                            case "Pickup":
                                Held_Obj.GetComponent<PickupBehavior>().pickup();
				HeldObject_Rigidbody = Held_Obj.GetComponent<Rigidbody>();
				HeldObject_Transform = Held_Obj.transform;
                                holdingObj = true;
                                break;
				//Add additional logic here for items that can be interacted with
                        }
                        mouseClickCooldownRemaining = mouseClickCooldown;
                    }
                }
            }
            else // Dropping Object if holding one
            {

		HeldObject_Transform = null;
		HeldObject_Rigidbody = null;
                holdingObj = false;
                Held_Obj.GetComponent<PickupBehavior>().drop();

                mouseClickCooldownRemaining = mouseClickCooldown;
            }
        }
        else if(Input.GetMouseButton(2) && holdingObj){ // Throw object if holding one

		HeldObject_Transform = null;
		HeldObject_Rigidbody = null;
                holdingObj = false;
		Held_Obj.GetComponent<PickupBehavior>().ThrowInDirection(interactRay.direction, ThrowForce);

        }

        #endregion


        #region Click Cooldown

        //if the left mouse click cooldown is greater than zero than we dercement the cooldown 
        if (mouseClickCooldownRemaining > 0.0f)
        {
            mouseClickCooldownRemaining -= .1f;
            //print("Click Cooldown: " + mouseClickCooldownRemaining);
        }
        //if the cooldown somehow gets below 0 than we just est it to zero
        else if (mouseClickCooldownRemaining < 0.0f) mouseClickCooldownRemaining = 0.0f;
        
        #endregion

	#region Handling Object Manipulation

	if(holdingObj){

		Vector3 holdPos = interactRay.GetPoint(CURRENT_HoldDistance);

		HeldObject_Distance = Vector3.Distance(holdPos, HeldObject_Transform.position);

		Vector3 Move_Direction = holdPos - HeldObject_Transform.position;
		if (HeldObject_Distance > .1) HeldObject_Rigidbody.AddForce(Move_Direction * Hold_Force);

		Debug.DrawRay(HeldObject_Transform.position, Move_Direction.normalized, Color.green);

		if(HeldObject_Distance > Break_Distance){
			HeldObject_Transform = null;
			HeldObject_Rigidbody = null;
			holdingObj = false;
			Held_Obj.GetComponent<PickupBehavior>().drop();
		}

	}

	#endregion

        Debug.DrawRay(interactRay.origin, interactRay.direction * CURRENT_HoldDistance, Color.blue);

    }

    // Update is called once per frame
    void Update()
    {

        #region Dynamic Reticle Changing

        //Looking to see if ray hits an interactable object
        if (holdingObj || Physics.Raycast(interactRay, CURRENT_HoldDistance, objectLayerMask) )
        {

		if(Hud) Hud.ReticleSize(true);


        } else{

		if(Hud) Hud.ReticleSize(false);

        }

        #endregion

        //Moving the hold position while the player is holding an object
        #region Pushing and pulling object
        if(holdingObj){

            CURRENT_HoldDistance += Input.mouseScrollDelta.y * HoldDistance_Increment;
            CURRENT_HoldDistance = Mathf.Clamp(CURRENT_HoldDistance, MIN_HoldDistance, MAX_HoldDistance);

        }
        else {

            CURRENT_HoldDistance = DEFAULT_HoldDistance;

        }
        #endregion
    }


}
