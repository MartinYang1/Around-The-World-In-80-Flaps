using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private AudioSource BgMusic;
    private AudioSource buttonClick;
    private AudioSource pageFlip;

    public AudioClip BgMusicClip;
    public AudioClip ButtonCickClip;
    public AudioClip pageFlipClip;
    public AudioMixerGroup master;

    private void Awake()
    {
        BgMusic = gameObject.AddComponent<AudioSource>();
        buttonClick = gameObject.AddComponent<AudioSource>();
        pageFlip = gameObject.AddComponent<AudioSource>();

        buttonClick.playOnAwake = false;
        pageFlip.playOnAwake = false;
        pageFlip.outputAudioMixerGroup = master;
        BgMusic.outputAudioMixerGroup = master;
        buttonClick.outputAudioMixerGroup = master;
        
        BgMusic.clip = BgMusicClip;
        buttonClick.clip = ButtonCickClip;
        pageFlip.clip = pageFlipClip;

        BgMusic.volume *= 0.4f;
    }

    private void Start()
    {
        BgMusic.playOnAwake = true;
        BgMusic.loop = true;
        BgMusic.Play();
    }

    public void PlayButtonClicked()
    {
        buttonClick.Play();
    }

    public void PlayPageFlip()
    {
        pageFlip.Play();
    }
}
