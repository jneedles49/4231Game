using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Activate : MonoBehaviour
{

    //Everything except the activate function in this script is placeholder, can and should be deleted in the near future
    private string SelfTag;
    [SerializeField] private AudioClip soundEffect;
    private AudioSource SoundPlayer;
    // Start is called before the first frame update
    void Start()
    {
        SelfTag = this.tag;
        SoundPlayer = GetComponent<AudioSource>();
        SoundPlayer.clip = soundEffect;
    }
    public void activate(){
        //This switch case should be changed and added on to
        switch(SelfTag){

                //Opening Door by Playing open animation
                case "Door":

                    this.GetComponent<Animator>().SetTrigger("OpenDoor");
                    if (!SoundPlayer.isPlaying) SoundPlayer.Play();

                break;


        }
    }
}
