using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private int maxSceneNumber = 3;

    //NOTE: The game manager should be controlling all of the saving/loading user progress, loading levels, and all other constant things.
    //very little should actually interact with game manager only to ask it to load the next level and to complete all of the other assoctiated processes.

    public void LoadNextLevel(){

        //Fadeout for the player
        player.GetComponent<Obj_Interaction>().Fade(false);

        //If the SceneNumber value doesn't exist then we create it.
        if(!PlayerPrefs.HasKey("SceneNumber")) PlayerPrefs.SetInt("SceneNumber", 0);

        //Getting SceneNumber

        int SceneNumber = PlayerPrefs.GetInt("SceneNumber");
        Debug.Log("Loading Scene Number: " + SceneNumber);

        //Incrementing SceneNumber on disk
        if(SceneNumber < maxSceneNumber - 1) PlayerPrefs.SetInt("SceneNumber", SceneNumber+1);
        else PlayerPrefs.SetInt("SceneNumber", maxSceneNumber - 1);


        //Other Code that we want to throw goes here 


        //We at last load the next scene
        SceneManager.LoadScene(SceneNumber);

    }
}
