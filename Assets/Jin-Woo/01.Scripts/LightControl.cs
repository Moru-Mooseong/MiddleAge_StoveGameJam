using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.PostProcessing;

public class LightControl : SingleTone<LightControl>
{
    public ParticleSystem[] particles;
    public PostProcessVolume volume;
    public Vignette vignette;

    public float intensity { get {
            if (Player.instance != null)
                return Player.instance.intensity;
            else return 0.5f;
                } }
    void Start()
    {
        vignette = volume.profile.GetSetting<Vignette>();

    }

    private void Update()
    {
        vignette.intensity.value = intensity;
        Switch();
    }

    private void Switch()
    {
        if(intensity >= 0.5f)
        {
            SetParticle(0);
        }
        else if(intensity >= 0.3f && intensity < 0.5f )
        {
            SetParticle(1);
        }
        else
        {
            SetParticle(2);
        }
    }

    private void SetParticle(int adress)
    {
        for(int i = 0; i < particles.Length; i ++)
        {
            if(i == adress)
            {
                particles[i].gameObject.SetActive(true);
            }
            else
            {
                particles[i].gameObject.SetActive(false);
            }
        }
    }
}
