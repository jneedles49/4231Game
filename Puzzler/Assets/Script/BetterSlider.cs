using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent (typeof(AudioSource))]
public class BetterSlider : MonoBehaviour, IPointerUpHandler
{
    private Slider slider;
    //TODO: Get handle to the text field we need to update

    public enum SliderTypes{

        Volume_Main,
        Volume_SFX,
        Volume_Music


    };

    [SerializeField] private SliderTypes sliderType;

    //This is the audio clip that plays after the user finishes adjusting the slider
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private TextMeshProUGUI text;
    private AudioSource SliderAudioSource;

    private bool IsValueAdjusted; //If value of the slider is changing we know the user has the pointer down on the slider, when they release the mouse click that is when we play a sound.
    private string SaveLoadName;
    private float savedValue;
    public Main_Menu menuScript;

    void Start(){

        //Setting key value to save and load data from
        switch(sliderType){

            case SliderTypes.Volume_Main:
                SaveLoadName = "Volume_Main";

            break;
            case SliderTypes.Volume_SFX:
                SaveLoadName = "Volume_SFX";

            break;
            case SliderTypes.Volume_Music:
                SaveLoadName = "Volume_Music";

            break;

        };

        slider = this.GetComponent<Slider>();
        SliderAudioSource = this.GetComponent<AudioSource>();

        slider.onValueChanged.AddListener(delegate {ValueChange();});


        //Load Data from user Preferences
        savedValue = PlayerPrefs.GetFloat(SaveLoadName, -1.0f);

        //If saved value doesn't exist we save a new value and just use that 
        if( savedValue == -1.0f){ 
            Debug.Log("Slider Value Not Found: Creating Slider Value");
            float StartValue = .5f;
            PlayerPrefs.SetFloat(SaveLoadName, StartValue);
            savedValue = StartValue; 
        } 

        Debug.Log("Starting Value is: " + savedValue);
        slider.value = savedValue;
        text.text = Mathf.Round(savedValue * 100) + "%";

    }

    //Value goes from 0-1
    public void ValueChange(){

        IsValueAdjusted = true;
        //TODO: Update the text field with the [round up or floor?]((slider value) * 10)
        text.text = Mathf.Round(slider.value * 100) + "%";

    }

    public void OnPointerUp(PointerEventData eventData){

        //Save Data to user preferences

        //If the value has been changed we save that data and play a sound
        if(IsValueAdjusted){
            savedValue = slider.value;
            Debug.Log("Value Adjusted: Saving current slider value: " + savedValue);

            PlayerPrefs.SetFloat(SaveLoadName, savedValue);
	    menuScript.AdjustAudioMixer(SaveLoadName, savedValue);

            SliderAudioSource.clip = audioClip;
            SliderAudioSource.Play();


            IsValueAdjusted = false;

        }

    }

    void OnEnable(){

        SliderAudioSource.Stop();

    }

    void OnDisable(){



    }

}
