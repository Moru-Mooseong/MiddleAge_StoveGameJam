using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public const int ASCII_toInt = 48;

    public enum Class { S = 0, A, B, C, D, E }
    public Sprite[] Ranks;
    public Sprite[] Title;
    public Sprite[] Numbers;

    [Space(20)]
    public Image[] ScoreDisplayer;
    public Image Rank_Displayer;
    public Image Title_Displayer;


    int myScore;
    public void Init(int score)
    {
        Time.timeScale = 0;
        myScore = score;
        char[] value = spritScore(myScore);
        SetNumberImage(value);
        SetRank();
    }

    char[] spritScore(int score)
    {
        string str = score.ToString();
        char[] returnvalue = new char[11] { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' };


        //10부터 0까지 거꾸로 
        //ex 13이면 0,1 2번
        for (int i = str.Length; i > 0; i--)
        {
            returnvalue[returnvalue.Length - i] = str[i - 1]; //거꾸로 계산딤
        }
        return returnvalue;
    }

    void SetNumberImage(char[] args)
    {
        for (int i = 0; i < ScoreDisplayer.Length; i++)
        {
            ScoreDisplayer[i].color = new Color(1, 1, 1, 1);
            ScoreDisplayer[i].sprite = Numbers[args[i]- ASCII_toInt];
        }
    }

    void SetRank()
    {
        // /1000으로 나눔, 너무 어려움
        if (myScore >= 540) 
        {
            MyRank(Class.S);
        }
        else if (myScore >= 336)
        {
            MyRank(Class.A);
        }
        else if (myScore >= 234)
        {
            MyRank(Class.B);
        }
        else if (myScore >= 132)
        {
            MyRank(Class.C);
        }
        else if (myScore >= 60)
        {
            MyRank(Class.D);
        }
        else
        {
            MyRank(Class.E);
        }
    }

    void MyRank(Class @class)
    {
        Rank_Displayer.sprite = Ranks[(int)@class];
        Title_Displayer.sprite = Title[(int)@class];
    }
}
