using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager2 : MonoBehaviour
{
    public GameObject HowToPlay_Obj;

    public const string defaultScore = "Score : ";

    public static GameManager2 instance;
    public Button pauseBtn;
    public Text scoreText;
    public static GameManager2 GM
    {
        get
        {
            if (instance == null) instance = FindObjectOfType(typeof(GameManager2)) as GameManager2;

            return instance;
        }

    }


    public GameObject[] filledHp;
    public GameObject gameOverPanel;

    public int maxHealth;
    public int currentHealth;



    public GameOverManager gameOver;


    [Space(20)]
    public Image filling;


    void Start()
    {

        if (instance == null) instance = this;
        scoreText.text = $"score : 0";
        currentHealth = 100;
    }


    void OnEnable()
    {

    }


    public void OnInit(int MaxHealth, int CurrentHealth)
    {
        this.maxHealth = MaxHealth;
        this.currentHealth = CurrentHealth;
        for(int i = 0; i < filledHp.Length; i++)
        {
            if(i <= currentHealth-1)
            { filledHp[i].gameObject.SetActive(true); }
            else
            { filledHp[i].gameObject.SetActive(false); }
        }
    }


    public void Pause()
    {
        pauseBtn.gameObject.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("일시정지 디버깅");
    }

    public void ReGame()
    {
        Time.timeScale = 1;
    }

    public void HowToPlay()
    {
        HowToPlay_Obj.SetActive(true);
    }

    public void GoToTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ReStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }


    public void SetEXP(int current, int NextEXP)
    {
        float rate = (float)current / (float)NextEXP;
        float defaultFilling = 0.122f;
        float MaxFilling = 0.878f;
        float fillLerp = 1 - (defaultFilling + MaxFilling);
        float fillAmount = Mathf.Lerp(defaultFilling, MaxFilling, rate);
        //filling.fillAmount = (float)current / (float)NextEXP;
        filling.fillAmount = fillAmount;
        Debug.Log($"채워진 량 : {filling.fillAmount}");
    }


    private void Update()
    {
        //if(currentHealth<1f) 
    }

    public void EndGame(int score)
    {
        if(gameOverPanel != null)
        gameOverPanel.SetActive(true);
        gameOver.Init(score);
        Time.timeScale = 0;
    }



    public void UpdateScore(int total_score)
    {
        //스코어점수를 매개변수값으로 바꾸는 기능
        if(scoreText != null)
        {
            scoreText.text = defaultScore+total_score.ToString();
        }
    }
}
