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
        Quit


    };

    [SerializeField] private GameObject Main_Menu_Obj;
    [SerializeField] private GameObject TextObj;
    [SerializeField] private ButtonType buttonType;
    TextMeshProUGUI textStyle;
    Main_Menu MenuScript;

    void Start(){

	 textStyle =TextObj.GetComponent<TextMeshProUGUI>(); 
	 MenuScript = Main_Menu_Obj.GetComponent<Main_Menu>();

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

                MenuScript.ToOptions();

		break;

	    case ButtonType.Continue:

		MenuScript.Continue();

		break;

	    case ButtonType.Quit:

		Application.Quit();

		break;

	    case ButtonType.Back:

		MenuScript.GoBack();

		break;

	    case ButtonType.NewGame:

	    	MenuScript.NewGame();

		break;

            //ADD ANY OTHER BUTTON TYPES HERE


        };


    }


}
