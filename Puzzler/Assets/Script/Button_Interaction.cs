using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject connectedObject;
    [SerializeField] private Animator ParentAnimator;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Collided");
        activate();
    }

    public void activate(){

        Debug.Log("Activated");
        connectedObject.GetComponent<Activate>().activate();
	ParentAnimator.SetTrigger("Button_Activate");

    }
}
