using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIRE : MonoBehaviour
{
    public float CoolTime;

    [SerializeField] GameObject particle;

    private float t;

    bool isActive;

    public UnityEngine.UI.Image fillAmount;
    public Canvas canvas;


    public AudioClip healClip;
    public AudioSource Audiosource;
    void Start()
    {
        isActive = false;
        t = 0;
        Audiosource = GetComponent<AudioSource>();
        canvas.worldCamera = Camera.main;
    }
    void Update()
    {
        t += Time.deltaTime;
        if (t > CoolTime)
        {
            t = CoolTime;
            isActive = true;
        }
        SetFill();
        if (isActive)
        { particle.SetActive(true); }
        else
        { particle.SetActive(false); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player target))
        {
            if (isActive)
            {
                Player.instance.intensity -= 0.2f;
                isActive = false;
                t = 0;
                Audiosource.PlayOneShot(healClip);
            }
        }
    }

    public void SetFill()
    {
        float rate = t / CoolTime;
        float amount = Mathf.Lerp(0.1f, 0.9f, rate);
        fillAmount.fillAmount = amount;
    }
    
}
