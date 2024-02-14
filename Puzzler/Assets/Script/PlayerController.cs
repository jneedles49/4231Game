using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public Camera main_cam; 
    private CharacterController playerController;
    public float speed = 7.5f;
    public float lookSensitivity = 2.0f;
    public float lookXlimit = 45.0f;
    float rotation_x = 0;
    float gravity = 20.0f;
    Vector3 MoveDirection = Vector3.zero;
    public Vector3 holdPos;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  
    }

    // Update is called once per frame
    void Update()
    {
        //Transforming object along the x and z axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        //Obtaining current speed values
        float spd_x = speed * Input.GetAxis("Vertical");
        float spd_y = speed * Input.GetAxis("Horizontal");

        //Getting the pre-adjusted y value before speed is applied to move direction
        float movementDirectionY = MoveDirection.y;
        MoveDirection = (forward * spd_x) + (right * spd_y);
        //Resetting y value
        MoveDirection.y = movementDirectionY;

        if(!playerController.isGrounded){

            MoveDirection.y -=gravity * Time.deltaTime;

        }

        playerController.Move(MoveDirection * Time.deltaTime);

        rotation_x += -Input.GetAxis("Mouse Y") * lookSensitivity;
        rotation_x = Mathf.Clamp(rotation_x, -lookXlimit, lookXlimit);
        main_cam.transform.localRotation = Quaternion.Euler(rotation_x,0,0);
        transform.rotation *= Quaternion.Euler(0,Input.GetAxis("Mouse X") * lookSensitivity, 0);

        holdPos = new Vector3();

    }
}
