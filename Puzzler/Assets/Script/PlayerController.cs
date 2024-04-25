using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [Header("Player Attributes")]

    [SerializeField] private Camera main_cam; 
    private CharacterController playerController;
    [SerializeField] private float speed = 7.5f;
    [SerializeField] private float lookSensitivity = 2.0f;
    [SerializeField] private float lookXlimit = 45.0f;
    [SerializeField] private List<AudioClip> Footstep_Sounds;
    private AudioSource Footstep_Audio;
    private int Current_Step_Track = 0;

    [Header("HUD")]
    [SerializeField] private HudManager Hud;

    float rotation_x = 0;
    float gravity = 20.0f;
    Vector3 MoveDirection = Vector3.zero;
    public static bool Paused = false;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  
	Footstep_Audio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
	if(Input.GetKeyUp(KeyCode.Escape)){
		if(!Paused){

			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
			Time.timeScale = 0;
			Paused = true;
			if(Hud) Hud.Pause();

		}
		else{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;  
			Time.timeScale = 1;
			Paused = false;
			if(Hud) Hud.UnPause();
		}
	}

	//Need to return here so we don't keep handling input and turning while paused
	if(Paused){ 
		return;
	}
	
        //Transforming object along the x and z axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //Obtaining current speed values
        float spd_x = speed * Input.GetAxis("Vertical");
        float spd_y = speed * Input.GetAxis("Horizontal");

	if(spd_x > 0 || spd_y > 0){ 

		if(!Footstep_Audio.isPlaying){

			Footstep_Audio.clip = Footstep_Sounds[Current_Step_Track];
			Footstep_Audio.Play();
			Current_Step_Track++;
			if (Current_Step_Track >= Footstep_Sounds.Count) Current_Step_Track = 0;

		}

	}
	else if(Footstep_Audio.isPlaying) Footstep_Audio.Stop();

        //Getting the pre-adjusted y value before speed is applied to move direction
        float movementDirectionY = MoveDirection.y;
        MoveDirection = (forward * spd_x) + (right * spd_y);
        //Resetting y value
        MoveDirection.y = movementDirectionY;

        if (!playerController.isGrounded)
        {

            MoveDirection.y -= gravity * Time.deltaTime;

        }

        playerController.Move(MoveDirection * Time.deltaTime);

        float rotation_y = Input.GetAxis("Mouse X") * lookSensitivity;

        //Locks the camera if right mouse button is being held
        //Note: Either this or Obj_interactions code may need to be migrated to just one Script to simplify a lot of things, will take a while but it will probably be worth it
        //      As this script doesn't have the capability to just lock when an object's being held unless it connects to the obj_interaction script, which is bad in general
        if (!Input.GetKey(KeyCode.Mouse1))
        {

            rotation_x += -Input.GetAxis("Mouse Y") * lookSensitivity;
            rotation_x = Mathf.Clamp(rotation_x, -lookXlimit, lookXlimit);

            main_cam.transform.localRotation = Quaternion.Euler(rotation_x, 0, 0);
            transform.rotation *= Quaternion.Euler(0, rotation_y, 0);

        }

    }
}
