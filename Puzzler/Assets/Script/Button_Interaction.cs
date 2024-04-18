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
        //Creating a new ray that just points up 1 unit
        ObjectDetection = new Ray(transform.position, Vector3.up);
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Collided");
        activate();
    }

    public void activate(){

        Debug.Log("Activated");
        connectedObject.GetComponent<Activate>().activate();

    }
}
