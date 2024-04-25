using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class BetterButton: Selectable, IPointerDownHandler 
{

     private enum ButtonType
    {

        Back,
	NewGame,
        Options,
        Continue,
        Quit,
	Resume,
	ToMenu


    };

    [SerializeField] private GameObject Main_Menu_Obj;
    [SerializeField] private GameObject Pause_Menu_Obj;
    [SerializeField] private TextMeshProUGUI textStyle;
    [SerializeField] private ButtonType buttonType;
    private Main_Menu MenuScript;
    private Pause_Menu Pause_Script;

    void Start(){

	 if(Main_Menu_Obj) MenuScript = Main_Menu_Obj.GetComponent<Main_Menu>();
	 if(Pause_Menu_Obj) Pause_Script = Pause_Menu_Obj.GetComponent<Pause_Menu>();

    }


    void Update(){

        if(IsHighlighted()){

            textStyle.fontStyle = FontStyles.Underline;

        } else {

            textStyle.fontStyle = FontStyles.Normal;

        }

	if(!interactable) textStyle.color  = Color.gray;
	else textStyle.color = Color.white;


    }

    public override void OnPointerDown(PointerEventData eventData){

        //Button type logic here

        switch(buttonType){

            case ButtonType.Options:

                if (MenuScript) MenuScript.ToOptions();
		if (Pause_Script) Pause_Script.ToOptionsMenu();

		break;

	    case ButtonType.Continue:

		if (MenuScript)MenuScript.Continue();

		break;

	    case ButtonType.Quit:

		if (MenuScript)Application.Quit();

		break;

	    case ButtonType.Back:

		if (MenuScript)MenuScript.GoBack();
		if (Pause_Script) Pause_Script.ToHome();

		break;

	    case ButtonType.NewGame:

	    	if (MenuScript)MenuScript.NewGame();

		break;

            //ADD ANY OTHER BUTTON TYPES HERE

            case ButtonType.Resume:

		if (Pause_Script) Pause_Script.ResumeGame();

		break;

            case ButtonType.ToMenu:

		if (Pause_Script) Pause_Script.QuitToMenu();

		break;

        };


    }


}
