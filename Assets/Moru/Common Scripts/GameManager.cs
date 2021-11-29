using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTone<GameManager>
{
    public Game_Patten gamePatten;

    [SerializeField]
    public List<SettingGameConfig> PattenDatas;
    public List<int> whatPatten;

    public GameObject Casting_bar;

    public int PattenAdress;
    protected override void Awake()
    {
        base.Awake();
        gamePatten = GetComponent<Game_Patten>();
        int RandomPatten = Random.Range(1, gamePatten.GetPattenCount());
        PattenAdress = RandomPatten;
        whatPatten = gamePatten.GetData(RandomPatten);
    }

    protected void Start()
    {
        OnStartGame();
        SoundManager.instance.PlayBGM(SoundManager.BGM.Ingame);
    }
    private void OnStartGame()
    {

        SpawnManager.instance.Init();
        Unit PlayerUnit = Instantiate(Object_Pooler.instance.targets[whatPatten[1] - 1], new Vector3(0, 10, 0), Quaternion.identity);
        PlayerUnit.GetComponent<Unit>().Spawn(new Vector3(0, 10, 0), Quaternion.identity);
        Debug.Log($"플레이어 레벨 : {PlayerUnit.level}");
        //Object_Pooler.instance.FindUnit(whatPatten[1]-1, new Vector3(0, 10, 0), Quaternion.identity);
        var AI = PlayerUnit.GetComponent<Unit_AI>();
        AI.StopAllCoroutines();
        Destroy(AI);
        Player pl = PlayerUnit.gameObject.AddComponent<Player>();
        PlayerUnit.gameObject.name += "__Player";
        PlayerUnit.foot.transform.rotation = Quaternion.identity;
        CameraCtr.instance.target = PlayerUnit.gameObject.transform;
        if (pl.myUnit.jop == Unit.Jop.Magician_1 || pl.myUnit.jop == Unit.Jop.Magician_2)
        {
            SoundManager.instance.PlaySFX(SoundManager.SFX.Magician);
        }
        else if (pl.myUnit.jop == Unit.Jop.thief_1 || pl.myUnit.jop == Unit.Jop.thief_2)
        {
            SoundManager.instance.PlaySFX(SoundManager.SFX.Thief);
        }
        else
        {
            SoundManager.instance.PlaySFX(SoundManager.SFX.Warrior);
        }
    }

    /// <summary>
    /// 정해진 패턴에 따라서 
    /// </summary>
    /// <param name="jop"></param>
    /// <returns></returns>
    public int Get_Level(Unit.Jop jop)
    {
        int adress = whatPatten[(int)jop];       //현재 패턴에서 jop번째의 숫자, 직업은 무조건 0,1,2,3,4,5 순서
        int level = 0;
        for (int i = 0; i < whatPatten.Count; i++)
        {
            if ((whatPatten[i] == (int)jop+1))
            {
                level = i;
            }
        }
        return level;
    }


    [System.Serializable]
    public class SettingGameConfig
    {
        public string PattenName;
        [SerializeField]
        public List<int> Scores;
        [SerializeField]
        public List<int> GetEXP;
        [SerializeField]
        public List<int> EXP_Cost;
    }

}


