using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingleTone<SoundManager>
{

    public AudioSource audios;
    public AudioSource audios2;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public enum BGM { Title, Ingame, Emergency, GameOver}
    public enum SFX{ Button, Warrior, Thief, Magician, Walk}
    public List<AudioClip> bgms;
    public List<AudioClip> sfxs;
    public void PlayBGM(BGM whatBGM)
    {
        audios.clip = bgms[(int)whatBGM];
        audios.Play();
    }
    public void PlaySFX(SFX sfx)
    {
        audios.PlayOneShot(sfxs[(int)sfx]);
    }
    public void PlayFoot(bool Active)
    {
        audios2.clip = sfxs[4];
        if (audios2.isPlaying)
        {
            if (Active)
            { }
            else
            { audios2.Stop(); }
        }
        else if(Active)
        {
            audios2.Play();
        }
            
    }
}
