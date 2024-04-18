using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCollider : MonoBehaviour
{

	public Transform Target_Transform; 
	private Transform trans;

	//Move Only on Z-Axis
	private float X_Constant = 0;
	private float Y_Constant = 0;

	void Start(){

		trans = this.transform;
		X_Constant = trans.position.x; 
		Y_Constant = trans.position.y;

		if(!Target_Transform) return;
		//Getting the start position of the object
		Vector3 Start_Position = Target_Transform.position;
		Start_Position.x = X_Constant; 
		Start_Position.y = Y_Constant; 

		trans.position = Start_Position;

	}

	void FixedUpdate(){

		if (!Target_Transform) return;

		//Get the transform location we need to move too by taking the the objects transform, multiply it by the movement axis vector.
		Vector3 Target_Position = Target_Transform.position;	

		Target_Position.x = X_Constant; 
		Target_Position.y = Y_Constant; 
		//Debug.Log("Here is our new target transform: " + Target_Position);

		//Then we probably use smooth dammp to move to a specific

		trans.position = Target_Position; 

	}

}
