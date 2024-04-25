using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{

    [SerializeField] private GameObject Home_Menu;
    [SerializeField] private GameObject Options_Menu;
    [SerializeField] private HudManager Hud;
    public AudioMixer mixer;

	//Change the audio mixer
	public void AdjustAudioMixer(string MixerName, float value){

		if(!mixer) Debug.LogError("NO MIXER DUMMY");
		else{


			float SetValue = 0f;
			if(value == 0) SetValue = -80f;
			else SetValue = value * 30 -20;


			if(mixer.SetFloat(MixerName, SetValue)) Debug.Log("Set Audio Successfully");
			else Debug.Log("WHY GAUUUD  WHYYY");


		}

	}	

    	//Resume the game
	public void ResumeGame(){

		Time.timeScale = 1;
		PlayerController.Paused = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = true;
		if(Hud) Hud.UnPause();

	}

    	//Quit to Menu
	public void QuitToMenu(){

		//Load Scene no. 0
		SceneManager.LoadScene(0);

	}

    	//Go back to the home pause menu
	public void ToHome(){

		Options_Menu.SetActive(false);
		Home_Menu.SetActive(true);

	}

	public void ToOptionsMenu(){


		Home_Menu.SetActive(false);
		Options_Menu.SetActive(true);

	}

}
