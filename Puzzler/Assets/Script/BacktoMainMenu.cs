using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoMainMenu : MonoBehaviour
{
	void Start(){
		Cursor.visible = true;  
		Cursor.lockState = CursorLockMode.None;
	}

	public void buttonClicked() {
		SceneManager.LoadScene(0);
	}
}
