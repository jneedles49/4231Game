using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private AudioSource Music;
    public AudioClip MainTrack;
    // Start is called before the first frame update
    void Start()
    {
        Music = GetComponent<AudioSource>();
        Music.clip = MainTrack; 
    }

    // Update is called once per frame
    void Update()
    {
       if(!Music.isPlaying) Music.Play(); 
        
    }
}
