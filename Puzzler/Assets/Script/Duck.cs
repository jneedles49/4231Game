using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
public class Duck : MonoBehaviour
{

	private bool isLooking;
	private Transform trans;
	private Rigidbody rigid;
	private AudioSource Audio_Player;
	private readonly float Pause_Timer = 180;
	private float current_Timer = 0;
	[Header("Target")]
	[SerializeField] private Transform Player_Trans;
	[Header("Attributes")]
	[SerializeField] private float Speed = 5f;
	[SerializeField] private float RotationSpeed = 5f;
	[Header("Audio")]
	[SerializeField] private AudioClip quack;


    void Awake(){

	    trans = this.transform;
	    rigid = this.GetComponent<Rigidbody>();
	    Audio_Player = this.GetComponent<AudioSource>();
	    Audio_Player.clip = quack;

    }

    void OnBecameVisible(){

	    isLooking = true;

    }

    void OnBecameInvisible(){

	    isLooking = false;
	    Debug.Log("I'm gonna get ya");

    }

    void FixedUpdate()
    {
	    if(!isLooking){

			//Pausing the duck
			if (current_Timer > 0){

				current_Timer--;	
				return;
			}

		    Vector3 Player_Pos = Player_Trans.position;
		    Player_Pos.y = 0;
		    Vector3 Current_Pos = trans.position;
		    Current_Pos.y = 0;

		    //Movement
		    Vector3 Move_Direction =  Player_Pos - Current_Pos; 
		    Move_Direction.Normalize();

		    Vector3 Target_Direction = Move_Direction;

		    Move_Direction *= Speed;
		    Move_Direction.y = 0;
		    rigid.velocity = Move_Direction;

		    //Rotatino (I'm not fixing this mispelling)
		    Quaternion Target_Rotation = Quaternion.LookRotation(Target_Direction);
		    Quaternion Current_Rotation = Quaternion.Slerp(trans.rotation, Target_Rotation, RotationSpeed * Time.deltaTime);

		    trans.rotation = Current_Rotation;
		

	    }
        
    }

    void OnCollisionEnter(Collision other){


	    if (other.gameObject.tag == "User") Debug.Log("Got Ya!");
	    if (!Audio_Player.isPlaying) Audio_Player.Play();
	    current_Timer = Pause_Timer;

    }
}
