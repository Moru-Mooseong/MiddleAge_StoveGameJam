using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_AI : MonoBehaviour
{
    public enum AIState { Targeting, Move, Standby }
    AIState aIstate;
    public float ObserveTime { get { return myClass.ObserveTime; } }                //이정도 시간이 흐르면 타겟 포착 시작
    public float CancelObserveTime { get { return myClass.CancelObserveTime; } }    //이 시간 이상을 벗어나면 취소
    public float ObserveRange { get { return myClass.ObserveRange; } }  //이 거리 이상을 벗어나면 취소
    public float SightRange { get { return myClass.SightRange; } }                  //찾을 때의 거리
    public float Patrol_Distance;//최대 이동거리
    public float TargetDistance; //공격거리
    public float MaxHoldTime;    //대기시간
    Unit myClass;
    Coroutine myCoroutine;

    [SerializeField]
    public Unit targetUnit = null;

    private void Awake()
    {
        myClass = GetComponent<Unit>();
        StartCoroutine(Co_FindUnit());
    }

    private void OnEnable()
    {
        aIstate = AIState.Move;
        //StartCoroutine(Co_FindUnit());
    }
    [ContextMenu("스타트AI")]
    public void OnStartAI()
    {
        targetUnit = null;
        StartCoroutine(Co_FindUnit());
    }

    float dummyTime_Observing;
    private void Update()
    {
        if (myClass.state == Unit.State.Die) return;


        //할당된 타겟이 없으면 랜덤하게 좌표를 땁니다.
        if (targetUnit == null)
        {
            dummyTime_Observing = 0;
            if (aIstate == AIState.Standby)
            {
                myClass.OnMove(Vector3.zero);
                return;
            }

            if (Vector3.Distance(transform.position, nextPos) < 1f || nextPos == Vector3.zero)
            { Patrol_Position(); StartCoroutine(HoldPostion()); }
            else if (nextPos != Vector3.zero)
            { myClass.OnMove(_nextPos.normalized); }
        }

        //할당된 타겟이 있으면 해당좌표로 이동합니다.
        else if (targetUnit != null)
        {
            dummyTime_Observing += Time.deltaTime;

            if (dummyTime_Observing < CancelObserveTime)
            {
                if (Vector3.Distance(transform.position, targetUnit.transform.position) < TargetDistance)
                {
                    myClass.OnMove(Vector3.zero);
                    if (myClass.jop != Unit.Jop.Magician_1 && myClass.jop != Unit.Jop.Magician_1)
                    {
                        myClass.OnAttack(targetUnit);
                    }
                    else if (myClass.TryGetComponent<Magician>(out Magician magician))
                    { magician.OnAttack(targetUnit.transform.position); }
                }
                else
                {
                    myClass.OnMove(_nextPos.normalized);
                }
                if (targetUnit.state == Unit.State.Die || !targetUnit.gameObject.activeInHierarchy || Vector3.Distance(transform.position, targetUnit.transform.position) > ObserveRange)
                { targetUnit = null; t = 0; dummyTime_Observing = 0; }
            }
            else
            {
                targetUnit = null; t = 0; dummyTime_Observing = 0;

            }
        }
    }

    float t;
    private IEnumerator Co_FindUnit()
    {
        t = 0;
        float RandomTime = Random.Range(myClass.SearchTime / 5, myClass.SearchTime);
        WaitForSeconds sec = new WaitForSeconds(0.1f);
        while (true)
        {
            t += Time.deltaTime;
            if (targetUnit == null && t > RandomTime)
            {
                if (FindUnit_Closed() != null)
                {
                    targetUnit = FindUnit_Closed();
                    Targeting_On(targetUnit);
                    // Debug.LogError("타게팅 온");
                }
            }
            else if (targetUnit != null)
            {
                if (targetUnit.state == Unit.State.Die || !targetUnit.gameObject.activeInHierarchy)
                { targetUnit = null; }
            }
            yield return sec;
        }
    }


    /// <summary>
    /// 가장 가까운 유닛을 찾습니다.
    /// </summary>
    /// <returns></returns>
    public Unit FindUnit_Closed()
    {
        Unit closedUnit = null;
        float closedDist = 100;

        int LayMask = 1 << LayerMask.NameToLayer("Unit");
        Collider2D[] units = Physics2D.OverlapCircleAll(transform.position, SightRange, LayMask);

        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].gameObject == this.gameObject)
            {
                //Debug.Log($"{units[i].name}은 자기자신입니다."); 
                continue;
            }
            if (myClass.isCanAttack(units[i].GetComponent<Unit>()) && units[i].GetComponent<Unit>().state != Unit.State.Die)
            {
                if (!myClass.isCanFind(units[i].GetComponent<Unit>()))
                { continue; }
                if (Vector3.Distance(transform.position, units[i].transform.position) < closedDist)
                {
                    closedUnit = units[i].GetComponent<Unit>();
                    closedDist = Vector3.Distance(transform.position, units[i].transform.position);
                }
            }
            else { continue; }
        }
        //Debug.Log($"Result : {units.Length}// {Observing_Distance}거리");

        return closedUnit;
    }

    /// <summary>
    /// 대상을 찾아 목적지 설정 
    /// </summary>
    /// <param name="target"></param>
    private void Targeting_On(Unit target)
    {
        nextPos = target.transform.position;
        Vector3 _nextPos = (nextPos - transform.position).normalized;
        Quaternion lookAt = Quaternion.LookRotation(Vector3.forward, -_nextPos);
        if (myClass.gameObject.activeInHierarchy || myClass.state != Unit.State.Attack)
        { 
            if(myClass.foot != null)
            myClass.foot.transform.rotation = lookAt; 
        }
        this._nextPos = (nextPos - transform.position);
        //Debug.Log($"대상을 찾았습니다. 다음장소 : {nextPos} // {lookAt}");

    }

    /// <summary>
    /// 랜덤한 위치를 돌려줍니다.
    /// </summary>
    Vector3 nextPos;
    Vector3 _nextPos;
    public Vector3 Patrol_Position()
    {
        float x = Random.Range(-Patrol_Distance, Patrol_Distance);
        float y = Random.Range(-Patrol_Distance, Patrol_Distance);
        x = Mathf.Clamp(x, 3, x);
        y = Mathf.Clamp(y, 3, y);

        nextPos = new Vector3(x, y, 0);

        Vector3 _nextPos = (nextPos - transform.position).normalized;
        Quaternion lookAt = Quaternion.LookRotation(Vector3.forward, -_nextPos);
        myClass.foot.transform.rotation = lookAt;
        //Debug.Log($"다음으로 움직일 포지션을 찾습니다. 다음장소 : {nextPos} // {lookAt}");
        this._nextPos = (nextPos - transform.position);

        return nextPos;
    }

    /// <summary>
    /// 정해진 랜덤위치에서 잠시동안 대기합니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator HoldPostion()
    {
        aIstate = AIState.Standby;
        float RandomHoldTime = Random.Range(0, MaxHoldTime);
        yield return new WaitForSeconds(RandomHoldTime);
        aIstate = AIState.Move;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Unit>(out Unit target))
        {
            if (myClass.state != Unit.State.Die || target.state != Unit.State.Die)
            {
                Vector3 LookPoint = (collision.transform.position - transform.position).normalized;
                Quaternion lookAt = Quaternion.LookRotation(Vector3.forward, -LookPoint);
                myClass.foot.transform.rotation = lookAt;
                targetUnit = target;
                if (myClass.jop != Unit.Jop.Magician_1 && myClass.jop != Unit.Jop.Magician_1)
                {
                    myClass.OnAttack(targetUnit);
                }
                else if (myClass.TryGetComponent<Magician>(out Magician magician))
                { magician.OnAttack(targetUnit.transform.position); }
                dummyTime_Observing = 0;
            }
        }
    }
}
