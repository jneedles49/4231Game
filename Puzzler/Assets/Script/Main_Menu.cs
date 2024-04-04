using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    [SerializeField] private GameObject Home_Menu;
    [SerializeField] private GameObject Options_Menu;
    [SerializeField] private BetterButton ContinueButton;


    public void Start(){


	//We need to see if there is the key SceneNumber in memory, if not then we need to grey out and deactivate the continue button	
	if(!PlayerPrefs.HasKey("SceneNumber")){
		ContinueButton.interactable = false;	
	}
	

    }

    public void ToOptions(){

        Home_Menu.SetActive(false);
        Options_Menu.SetActive(true);

    }

    public void GoBack(){

        Options_Menu.SetActive(false);
        Home_Menu.SetActive(true);

    }

    public void Continue(){

	if(PlayerPrefs.HasKey("SceneNumber")){
		int CurScene = PlayerPrefs.GetInt("SceneNumber");	
		SceneManager.LoadScene(CurScene);
	}

    }

    public void NewGame(){

	PlayerPrefs.SetInt("SceneNumber", 1);
	LoadSelectedLevel(1);

    }

    public void LoadSelectedLevel(int levelID){

        SceneManager.LoadScene(levelID);

    }


    //*******ADD-ANY-OTHER-BUTTON-FUNCTIONS-HERE*********\\



}
