using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pooler : SingleTone<Object_Pooler>
{
    [Header("최초 실행 시 풀링할 카운트")] public int default_PoolCount;

    [Header("생성할 프리팹")]
    public List<Unit> targets;
    public List<Multi_Pooler> pooler;

    protected override void Awake()
    {
        base.Awake();
        pooler = new List<Multi_Pooler>();
        for (int i = 0; i < targets.Count; i++)
        {
            GameObject poolingObj = new GameObject();
            poolingObj.transform.SetParent(this.transform);
            Multi_Pooler _pooler = poolingObj.AddComponent<Multi_Pooler>();
            _pooler.Init(default_PoolCount, targets[i].gameObject);
            _pooler.name = $"____<Pool>_{targets[i].name}____";
            pooler.Add(_pooler);
        }
    }

    [ContextMenu("테스트")]
    public void TEST()
    {
        FindUnit(Unit.Jop.Warrior_1, transform.position, Quaternion.identity);
    }
    public Unit FindUnit(Unit.Jop jop, Vector3 pos, Quaternion rotation, Transform parent = null, int level = 0)
    {
        int adress = (int)jop;
        if(pooler.Count >= adress)
        {adress--;}
        Unit find = null;
        find = pooler[adress].OnSelected_One(this.transform).GetComponent<Unit>();
        if (find == null)
        { Debug.Log("파인드값 널"); }
        find.transform.position = pos;
        find.transform.rotation = rotation;
        find.gameObject.SetActive(true);
        if (parent != null)
        {
            find.transform.parent = parent;
        }
        find.Spawn(pos, rotation, level);

        return find;
    }
    public Unit FindUnit(int jop, Vector3 pos, Quaternion rotation, Transform parent = null, int level = 0)
    {
        int adress = (int)jop;
        if (pooler.Count >= adress)
        { adress--; }
        Unit find = null;
        if(pooler[jop].OnSelected_One() == null)
        { Debug.Log("널값 불러오기"); }
        find = pooler[jop].OnSelected_One().GetComponent<Unit>();
        if (find == null)
        { Debug.Log("파인드값 널"); }
        else
        { Debug.Log($"불러오는 값 : {find.name}"); }
        find.gameObject.SetActive(true);
        find.transform.position = pos;
        find.transform.rotation = rotation;
        if (parent != null)
        {
            find.transform.parent = parent;
        }
        find.Spawn(pos, rotation, level);

        return find;
    }



    [System.Serializable]
    public class Multi_Pooler : MonoBehaviour
    {
        [SerializeField] public GameObject Target_Object;
        public int howManySpawn_InLoad;
        public List<GameObject> Objects = new List<GameObject>(0);
        public Transform myTransform;

        public void Init(int default_Load, GameObject _target)
        {
            myTransform = this.transform;
            Target_Object = _target;
            if (default_Load == 0) return;
            for (int i = 0; i < default_Load; i++)
            {
                GameObject obj = Instantiate(Target_Object, gameObject.transform);
                if (Target_Object == null) Debug.Log($"풀링하고자 하는 오브젝트가 null입니다. {instance}'\'{Target_Object}");
                else { Objects.Add(obj); obj.SetActive(false); }
            }
        }

        public GameObject OnSelected_One(Transform parent = null)
        {
            GameObject obj = null;
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].activeInHierarchy == true) continue;
                else { obj = Objects[i]; break; }
            }
            if (obj == null)
            {
                GameObject new_obj;
                new_obj = Instantiate(Target_Object, myTransform);
                Objects.Add(new_obj);
                new_obj.SetActive(false);
                obj = new_obj;
            }
            obj.SetActive(true);
            if (parent == null) { obj.transform.SetParent(myTransform); }
            else { obj.transform.SetParent(parent); }

            return obj;
        }


        public void SetTransform(GameObject target, Vector3 pos, Quaternion rotation, Transform parent = null)
        {
            target.transform.position = pos;
            target.transform.rotation = rotation;
            if (parent == null) return;
            else
            {
                target.transform.parent = parent;
            }
        }
    }
}
