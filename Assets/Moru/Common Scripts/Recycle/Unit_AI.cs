using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_AI : MonoBehaviour
{
    public enum AIState { Targeting, Move, Standby }
    AIState aIstate;
    public float ObserveTime { get { return myClass.ObserveTime; } }                //������ �ð��� �帣�� Ÿ�� ���� ����
    public float CancelObserveTime { get { return myClass.CancelObserveTime; } }    //�� �ð� �̻��� ����� ���
    public float ObserveRange { get { return myClass.ObserveRange; } }  //�� �Ÿ� �̻��� ����� ���
    public float SightRange { get { return myClass.SightRange; } }                  //ã�� ���� �Ÿ�
    public float Patrol_Distance;//�ִ� �̵��Ÿ�
    public float TargetDistance; //���ݰŸ�
    public float MaxHoldTime;    //���ð�
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
    [ContextMenu("��ŸƮAI")]
    public void OnStartAI()
    {
        targetUnit = null;
        StartCoroutine(Co_FindUnit());
    }

    float dummyTime_Observing;
    private void Update()
    {
        if (myClass.state == Unit.State.Die) return;


        //�Ҵ�� Ÿ���� ������ �����ϰ� ��ǥ�� ���ϴ�.
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

        //�Ҵ�� Ÿ���� ������ �ش���ǥ�� �̵��մϴ�.
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
                    // Debug.LogError("Ÿ���� ��");
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
    /// ���� ����� ������ ã���ϴ�.
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
                //Debug.Log($"{units[i].name}�� �ڱ��ڽ��Դϴ�."); 
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
        //Debug.Log($"Result : {units.Length}// {Observing_Distance}�Ÿ�");

        return closedUnit;
    }

    /// <summary>
    /// ����� ã�� ������ ���� 
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
        //Debug.Log($"����� ã�ҽ��ϴ�. ������� : {nextPos} // {lookAt}");

    }

    /// <summary>
    /// ������ ��ġ�� �����ݴϴ�.
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
        //Debug.Log($"�������� ������ �������� ã���ϴ�. ������� : {nextPos} // {lookAt}");
        this._nextPos = (nextPos - transform.position);

        return nextPos;
    }

    /// <summary>
    /// ������ ������ġ���� ��õ��� ����մϴ�.
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
