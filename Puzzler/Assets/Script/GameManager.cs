using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
	[SerializeField] private HudManager Hud;
	private int maxSceneNumber; 
	public AudioMixer mixer;

    

    //NOTE: The game manager should be controlling all of the saving/loading user progress, loading levels, and all other constant things.
    //very little should actually interact with game manager only to ask it to load the next level and to complete all of the other assoctiated processes.

    public void Start(){

	maxSceneNumber = SceneManager.sceneCount;	

    }

    public void LoadNextLevel(){

        if(Hud){
            //Fadeout for the player
	    Hud.Fade(false);

            //If the SceneNumber value doesn't exist then we create it.
            if (!PlayerPrefs.HasKey("SceneNumber")) PlayerPrefs.SetInt("SceneNumber", 1);
        }

	Scene Current_Scene = SceneManager.GetActiveScene();
	int NextSceneNumber = Current_Scene.buildIndex + 1;

        //Incrementing SceneNumber on disk
        if(NextSceneNumber < maxSceneNumber - 1) PlayerPrefs.SetInt("SceneNumber", NextSceneNumber);
        else PlayerPrefs.SetInt("SceneNumber", maxSceneNumber - 1);
        //Other Code that we want to throw goes here 


        //We at last load the next scene
        SceneManager.LoadScene(NextSceneNumber);

    }

    public void AdjustAudioMixer(string MixerName, float value){

	if(!mixer) Debug.LogError("NO MIXER DUMMY");
	else{
		float SetValue = 0f;
		if(value == 0) SetValue = -80f;
		else SetValue = value * 30 -20;

		if(mixer.SetFloat(MixerName,SetValue)) Debug.Log("Set New Audio Successfully");
		else Debug.Log("Why....? You have betrayed me, betrayed your own blood, for that you.. will... Suffer!");

	}


    }

}
