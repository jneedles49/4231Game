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
    public Ray interactRay;
    private RaycastHit hit;

    [Header ("Interaction Paramaters")]
    public float playerInteractionRadius;
    private readonly float MinimumHoldDistance = .5f;
    private readonly float MaximumHoldDistance = 2.00f;
    private readonly float DefaultHoldDistance = 1.5f;
    private readonly float ScrollObjHoldOffset = .1f;
    private float mouseClickCooldownRemaining = 0f;
    private readonly float mouseClickCooldown = 1.0f;

    #endregion

    #region Object Interaction

    private bool holdingObj = false;
    private GameObject obj;

    #endregion

    #region HUD

    [Header ("HUD")]

    public GameObject reticle;
    private RectTransform reticleTransform;
    private RawImage reticleColor;

    [SerializeField]private Color HighlightColor = Color.white;
    [SerializeField]private Color normalColor = Color.grey;

    private readonly Vector3 ReticleMaxSize = new Vector3(.3f,.3f,.3f);
    private readonly Vector3 ReticleMinSize = new Vector3(.15f,.15f,.15f);

    [SerializeField] private GameObject FadeObj;
    private Image image;

    #endregion

    // Start is called before the first frame update
    void Start()
    {

        //gets the screen width and height
        pos = new Vector3(Screen.width / 2, Screen.height / 2, 7);
        reticleTransform = reticle.GetComponent<RectTransform>();
        reticleColor = reticle.GetComponent<RawImage>();

        //Fading In
        image = FadeObj.GetComponent<Image>();
        Fade(true);

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

                if (Physics.Raycast(interactRay, out hit, playerInteractionRadius, objectLayerMask))
                {
                    // If the cooldown of the players mouse click is zero, then we print a message, then restart the cooldown
                    if (hit.collider)
                    {
                        //Get the game object from the RaycastHit and get the interactable object script component
                        obj = hit.transform.gameObject;
                        switch(obj.tag){
                            case "Pickup":
                                obj.GetComponent<PickupBehavior>().pickup(this.gameObject);
                                holdingObj = true;
                                break;
			    case "Chest":
				obj.GetComponent<Activate>().activate();
				break;
                        }
                        mouseClickCooldownRemaining = mouseClickCooldown;
                    }
                }
            }
            //if the player is holding an object then we just call the current objects pickup script, dropping the object, and resettinga cooldown
            else
            {

                obj.GetComponent<PickupBehavior>().pickup(this.gameObject);
                holdingObj = false;

                mouseClickCooldownRemaining = mouseClickCooldown;
            }
        }
        else if(Input.GetMouseButton(2) && holdingObj){ // Middle Mouse

            //Throw Object 
            obj.GetComponent<PickupBehavior>().ThrowInDirection(interactRay.direction);
            holdingObj = false;

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

        Debug.DrawRay(interactRay.origin, interactRay.direction * playerInteractionRadius, Color.blue);

    }

    // Update is called once per frame
    void Update()
    {

        #region Dynamic Reticle Changing

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

        #endregion

        //Moving the hold position while the player is holding an object
        #region Pushing and pulling object
        if(holdingObj){

            playerInteractionRadius += Input.mouseScrollDelta.y * ScrollObjHoldOffset;
            playerInteractionRadius = Mathf.Clamp(playerInteractionRadius, MinimumHoldDistance, MaximumHoldDistance);

        }
        else {

            playerInteractionRadius = DefaultHoldDistance;

        }
        #endregion
    }

    public void Fade(bool fadein){

        StartCoroutine(FadeScreen(fadein));

    }


    IEnumerator FadeScreen(bool fadeIn){

        if(fadeIn){

            for(float i = 1; i >= 0; i -= Time.deltaTime){

                image.color = new Color(0,0,0,i);
                yield return null;   

            }


        }
        else{

            for(float i = 0; i <= 1; i += Time.deltaTime){

                image.color = new Color(0,0,0,i);
                yield return null;   

            }

        }

    }

}
