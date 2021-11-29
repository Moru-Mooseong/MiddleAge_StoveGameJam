using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : SingleTone<SpawnManager>
{
    public int HowManySpawn_inMap;
    [Header("최초 생성")]
    public int Width;
    public int Height;
    [Header("플레이어주변 생성")]
    public int Player_Width;
    public int Player_Height;

    public List<float> termList;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {

        StartCoroutine(InitSpawn());
        StartCoroutine(Co_Spawning());
        if (Object_Pooler.instance == null) return;
        //for (int i = 0; i < 50; i++)
        //{
        //    Debug.Log("최초 스폰작업실행");
        //    if (HowManySpawn_inMap < 50)
        //    {

        //        int RandomX = Random.Range(3, Width - 3);
        //        int RandomY = Random.Range(3, Height - 3);
        //        Vector3 ObstaclePost = new Vector3((-Width * 0.5f + 0.5f) + RandomX, (Height * 0.5f - 0.5f) - RandomY, 0);


        //        int j = Random.Range(0, (int)Unit.Jop.none - 1);
        //        var unit = Object_Pooler.instance.FindUnit(j, ObstaclePost, Quaternion.identity, transform.parent);
        //    }
        //}
    }

    public IEnumerator InitSpawn()
    {
        WaitForSeconds sec = new WaitForSeconds(0.3f);
        int i = 0;
        yield return new WaitForSeconds(3f);
        while (i <= 50)
        {
            int RandomX = Random.Range(3, Width - 3);
            int RandomY = Random.Range(3, Height - 3);
            Vector3 ObstaclePost = new Vector3((-Width * 0.5f + 0.5f) + RandomX, (Height * 0.5f - 0.5f) - RandomY, 0);

            int j = Random.Range(0, (int)Unit.Jop.none - 1);
            //var unit = Object_Pooler.instance.FindUnit(j, ObstaclePost, Quaternion.identity, transform.parent);
            var unit = Instantiate(Object_Pooler.instance.targets[j], ObstaclePost, Quaternion.identity, transform);
            unit.GetComponent<Unit>().Spawn(ObstaclePost, Quaternion.identity);
            i++;
            yield return sec;
        }
        
    }


    int playerLevel
    {
        get { if (Player.instance != null) return Player.instance.myUnit.level; else { return 2; } }
    }
    IEnumerator Co_Spawning()
    {
        float term = 0;
        while (true)
        {
            //float RandomX = Random.Range((float)-Player_Width, (float)Player_Width);
            //float RandomY = Random.Range((float)-Player_Height, (float)Player_Height);
            //Vector3 ObstaclePost = new Vector3
            //    (Player.instance.transform.position.x + RandomX, Player.instance.transform.position.y + RandomY, 0);

            int max = playerLevel + 1;
            if (max > (int)Unit.Jop.none)
            { max = (int)Unit.Jop.none; }
            //Unit.Jop randomJop = (Unit.Jop)Random.Range(Player.instance.myUnit.level - 1, max);
            //var unit = Object_Pooler.instance.FindUnit(randomJop, ObstaclePost, Quaternion.identity);
            //if(unit != null) unit.Spawn(ObstaclePost, Quaternion.identity, 0);
            term = 0;

            int RandomX = Random.Range(3, Width - 3);
            int RandomY = Random.Range(3, Height - 3);
            Vector3 ObstaclePost = new Vector3((-Width * 0.5f + 0.5f) + RandomX, (Height * 0.5f - 0.5f) - RandomY, 0);

            if(max > Object_Pooler.instance.targets.Count)
            { max = Object_Pooler.instance.targets.Count; }
            int j = Random.Range(playerLevel -1, max);
            //var unit = Object_Pooler.instance.FindUnit(j, ObstaclePost, Quaternion.identity);
            var unit = Instantiate(Object_Pooler.instance.targets[j], ObstaclePost, Quaternion.identity, transform);
            unit.GetComponent<Unit>().Spawn(ObstaclePost, Quaternion.identity);
            float i = 0;
            if(playerLevel > termList.Count)
            {
                i = termList[termList.Count - 1];
            }
            else
            {
                i = termList[playerLevel];
            }
            yield return new WaitForSeconds(i);
        }
    }
}
