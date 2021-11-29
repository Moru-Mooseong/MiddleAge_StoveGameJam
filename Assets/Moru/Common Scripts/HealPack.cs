using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealPack : MonoBehaviour
{
    public Sprite[] Sprites;
    public SpriteRenderer sprite;
    public float CoolTime;

    private float t = 0;

    bool isActive;

    public Canvas canvas;
    public Text per_Rate;
    public ParticleSystem On_Particle;

    public AudioClip healClip;
    public AudioSource Audiosource;
    void Start()
    {
        t = 0;
        isActive = false;
        Audiosource = GetComponent<AudioSource>();
        canvas.worldCamera = Camera.main;
    }

    void Update()
    {
        t += Time.deltaTime;
        SetText();
        if (t > CoolTime)
        { isActive = true; t = CoolTime; }
        if (isActive)
        {
            sprite.sprite = Sprites[0];
            if (!On_Particle.gameObject.activeInHierarchy)
            {
                On_Particle.gameObject.SetActive(true);
                if (!On_Particle.isPlaying)
                {
                    On_Particle.Play(true);
                }
            }
        }
        else
        {
            sprite.sprite = Sprites[1];
            if (On_Particle.gameObject.activeInHierarchy)
            {
                On_Particle.gameObject.SetActive(false);
                if (On_Particle.isPlaying)
                {
                    On_Particle.Stop(true);
                }
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player target))
        {
            if (isActive)
            {
                target.myUnit.OnHeal(1);
                isActive = false;
                t = 0;
                Audiosource.PlayOneShot(healClip);
            }
        }
    }

    public void SetText()
    {
        float rate = t / CoolTime * 100;
        per_Rate.text = rate.ToString("F0");
    }
}
