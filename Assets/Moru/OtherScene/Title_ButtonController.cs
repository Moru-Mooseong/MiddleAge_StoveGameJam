using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_ButtonController : MonoBehaviour
{
    public GameObject GuideUI;
    public GameObject CreditUI;

    public SoundManager soundManager;
    void Start()
    {
        soundManager.PlayBGM(SoundManager.BGM.Title);
    }
    public void OnGameStart()
    {
        SceneManager.LoadScene(1);
        SoundManager.instance.PlaySFX(SoundManager.SFX.Button);
    }
    public void OnHowToPlay()
    {
        if(GuideUI != null)
        GuideUI.SetActive(true);
        SoundManager.instance.PlaySFX(SoundManager.SFX.Button);
    }
    public void OnCredit()
    {
        if(CreditUI != null)
        CreditUI.SetActive(true);
        SoundManager.instance.PlaySFX(SoundManager.SFX.Button);
    }
    public void OnExit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIClose();


        }
    }
    public void UIClose()
    {
        if(GuideUI.activeInHierarchy)
        {
            GuideUI.SetActive(false);
        }
        if (CreditUI.activeInHierarchy)
        {
            CreditUI.SetActive(false);
        }
        SoundManager.instance.PlaySFX(SoundManager.SFX.Button);
    }
}
