using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject connectedObject;
    //Change this so it just looks at some other script to get layer information
    private int InteractObjectLayer = 1 << 6;

    #region Object detection
    private Ray ObjectDetection; 
    private RaycastHit ObjDetails;
    private bool activated = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Vector3 CurrentPosition = this.transform.position;
        //Offsetting on Y because Unity Doesn't register collision if object is a little bit below the ray
        Vector3 RayStartPoint = new Vector3(CurrentPosition.x,CurrentPosition.y - .5f, CurrentPosition.z);
        //Creating a new ray that just points up 1 unit
        ObjectDetection = new Ray(RayStartPoint, Vector3.up);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region Physics intraction with button
        if (!activated)
        {
            if (Physics.Raycast(ObjectDetection, out ObjDetails, 1.0f, InteractObjectLayer))
            {
                GameObject obj = ObjDetails.collider.gameObject;
                if(obj.GetComponent<Rigidbody>().velocity == Vector3.zero){

                    activated = true;
                    activate();
                }


            }
            //Debug ray should disappear when after you place an object and it comes to a complete stop 
            Debug.DrawRay(ObjectDetection.origin, ObjectDetection.direction, Color.green);
        }

    }
    #endregion

    public void activate(){

        Debug.Log("Activated");
        connectedObject.GetComponent<Activate>().activate();
        if(this.GetComponent<Animator>()) this.GetComponent<Activate>().activate();


    }
}
