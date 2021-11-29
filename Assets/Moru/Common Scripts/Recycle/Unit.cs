using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class Unit : Actor
{
    //���� ���� ����
    public enum Jop { Warrior_1, thief_1, Magician_1, Warrior_2, thief_2, Magician_2, none }
    public Jop jop;

    public int _level;
    #region ���� ����
    //���� ����
    public string Name { get; protected set; }
    public int level { get; protected set; }

    public int MaxHealth { get; protected set; }
    public int CurrentHealth { get; protected set; }

    public float MoveSpeed { get; protected set; }

    public int EXP { get; protected set; }
    public int CurrentEXP { get; protected set; }

    public float SearchTime { get; protected set; }

    public float AttackSpeed { get; protected set; }

    public float Cooltime { get; protected set; }
    public float currentCoolTime { get; protected set; }

    public float SightRange { get; protected set; }

    public int myEXP { get; protected set; }
    public int myScore { get; protected set; }

    //���� ����
    public enum State { Idle, Attack, Walk, Hit, Die }
    public State state { get; private set; }

    public float ObserveTime { get; protected set; }
    public float CancelObserveTime { get; protected set; }
    public float ObserveRange { get; protected set; }
    #endregion


    public delegate void Del_OnDamage(Unit Attacker);
    public Del_OnDamage del_OnDamage;
    public delegate void Del_GetScore(int Score);
    public Del_GetScore del_getScore;
    public delegate void Del_GetEXP(int EXP, int MaxEXP);
    public Del_GetEXP del_getEXP;


    public Transform foot;
    private SortingGroup sortingGroup;
    private Animator animator;
    public int healthCount = 0;

    public AudioSource audios;
    public AudioClip clip;
    public ParticleSystem HitParticle;
    public ParticleSystem LevelUpParticle;
    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        sortingGroup = GetComponent<SortingGroup>();
        audios = GetComponent<AudioSource>();
        sortingGroup.sortingLayerName = "Unit";
        foot = transform.GetChild(0);
    }


    public virtual void Spawn(Vector3 SpawnPos, Quaternion rotation, int level = 0)
    {
        //transform.position = SpawnPos;
        CurrentHealth = MaxHealth;
        SpawnManager.instance.HowManySpawn_inMap++;
        if (TryGetComponent<Unit_AI>(out Unit_AI AI))
        {
            AI.OnStartAI();
        }
        SetLevel();
        CurrentEXP = 0;
        lastAttack = AttackSpeed;
        if (TryGetComponent<Player>(out Player player))
        {
            MoveSpeed *= 10;
        }
        defautl_MoveSpeed = MoveSpeed;

    }



    public virtual void SetCharacterSpeed(float speed)
    {
        this.MoveSpeed = speed;
    }

    public virtual void SetLevel(int level = 0)
    {
        List<float> stat = CSV_Reader.instance.GetData((int)jop);
        this.level = GameManager.instance.Get_Level(jop);
        this.level += level;
        MaxHealth = (int)stat[1];
        MoveSpeed = stat[2];
        ObserveTime = stat[4];
        CancelObserveTime = stat[5];
        ObserveRange = stat[5];
        SearchTime = stat[4];
        AttackSpeed = stat[7];
        Cooltime = stat[8];
        SightRange = stat[9];

        //ü�� �ʱ�ȭ
        CurrentHealth = MaxHealth;
        //��Ÿ�� �ʱ�ȭ
        currentCoolTime = Cooltime;
        //���ݼӵ� �ʱ�ȭ
        lastAttack = AttackSpeed;



        EXP = GameManager.instance.PattenDatas[0].EXP_Cost[this.level];
        myScore = GameManager.instance.PattenDatas[0].Scores[this.level];
        myEXP = GameManager.instance.PattenDatas[0].GetEXP[this.level];

        _level = level;
        SetState(State.Idle);
        //SpriteRenderer[] rendereres = transform.GetComponentsInChildren<SpriteRenderer>();
        //for(int i = 0; i <rendereres)
        //Debug.Log($"���� : {this.level}\nü�� : {MaxHealth}\n�̵��ӵ� : {MoveSpeed}\n����ġ : {EXP}\n��ġŸ�� : {SearchTime}\n�����ֱ� : {AttackSpeed}\n��Ÿ�� : {Cooltime}\n�þ߹��� : {SightRange}");
    }


    private void OnDisable()
    {
        if (SpawnManager.instance != null) SpawnManager.instance.HowManySpawn_inMap--;
    }

    private void Update()
    {
        if (lastAttack < AttackSpeed)
        { lastAttack += Time.deltaTime; }
        if (currentCoolTime < Cooltime)
        { currentCoolTime += Time.deltaTime; }
    }

    //0.02��(FixTime)���� ȣ��
    protected virtual void FixedUpdate()
    {
        Sort();
    }
    //y���� ���� ���÷��̾ ��ȭ�մϴ�.
    protected void Sort()
    { sortingGroup.sortingOrder = Screen.height - (int)(foot.position.y * 10f); }


    public float lastAttack { get; protected set; }
    /// <summary>
    /// ȣ�� �� ��� �������� �����ϴ�.
    /// ���ο��� ���ְ� ���ְ��� ���ط����� Ȯ�� �� �������� �����մϴ�.
    /// </summary>
    /// <param name="attaker"></param>
    public virtual void OnAttack(Unit target)
    {
        if (lastAttack < AttackSpeed) return;
        audios.clip = clip;
        audios.Play();
        lastAttack = 0;
        SetState(State.Attack);
        if (isCanAttack(target) && target.state != State.Die)
        { target.OnDamaged(this); }
    }

    public virtual void OnDamaged(Unit Attacker)
    {
        CurrentHealth--;
        if(TryGetComponent<Unit_AI>(out Unit_AI AI))
        {
            AI.targetUnit = Attacker;
        }
        SetState(State.Hit);
        if (CurrentHealth <= 0)
        { OnDie(Attacker); }
        //�����Լ� ȣ��
        if (del_OnDamage != null)
        { del_OnDamage(Attacker); }
    }

    /// <summary>
    /// ü���� 0�� �Ǿ� �׽��ϴ�.
    /// </summary>
    /// <param name="attacker"></param>
    public virtual void OnDie(Unit attacker)
    {
        SetState(State.Die);
        if (attacker != null)
        {
            attacker.GetEXP(this);
            attacker.GetScore(this);
        }
        StartCoroutine(CO_Die());
    }

    IEnumerator CO_Die()
    {
        yield return new WaitForSeconds(1f);
        SetActiveFalse();
    }

    public virtual void SetActiveFalse()
    { gameObject.SetActive(false); }

    public virtual bool isCanAttack(Unit target)
    {
        if (target == null) return false;
        //Ÿ���� �ڽź��� ������ ������?
        if (target.level > this.level)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public virtual bool isCanFind(Unit target)
    {
        return true;
    }

    public void GetEXP(Unit target)
    {
        CurrentEXP += target.myEXP;
        if (del_getEXP != null)
        {
            del_getEXP(CurrentEXP, EXP);
        }
    }

    public void GetScore(Unit target)
    {
        int score = target.myScore;
        if (del_getScore != null)
        { del_getScore(GameManager.instance.PattenDatas[0].Scores[target.level]); }
    }

    public void LevelUP()
    {
        level++;
        CurrentEXP = 0;
        LevelUpParticle.Play(true);
        EXP = GameManager.instance.PattenDatas[0].EXP_Cost[this.level];
        myScore = GameManager.instance.PattenDatas[0].Scores[this.level];
        myEXP = GameManager.instance.PattenDatas[0].GetEXP[this.level];
    }

    protected float defautl_MoveSpeed;
    public float SetMoveSpeed_rate(float rate, bool isDefault = false)
    {
        MoveSpeed = MoveSpeed * rate;
        if (isDefault)
        { MoveSpeed = defautl_MoveSpeed; }
        return MoveSpeed;
    }
    public virtual void OnMove(Vector3 Nextpositon, bool isWorld = false)
    {

        if (isWorld)
        {
            transform.position = new Vector3(this.transform.position.x + Nextpositon.x, this.transform.position.y + Nextpositon.y, 0);
        }
        else
        {
            transform.Translate(Nextpositon * MoveSpeed * Time.deltaTime); //�������������� �ؾ� ���콺 ȸ���Ҷ��� �״�ΰ��Ű�����
        }
        if (Nextpositon == Vector3.zero)
        {
            SetState(State.Idle);
        }
        else
        {
            SetState(State.Walk);
        }
    }

    public virtual State SetState(State whatState, float time = 0)
    {
        state = whatState;
        switch (state)
        {
            case State.Idle:
                animator.SetBool("OnWalk", false);
                break;
            case State.Walk:
                animator.SetBool("OnWalk", true);
                break;
            case State.Attack:
                animator.SetTrigger("OnAttack");
                break;
            case State.Hit:
                HitParticle.Play(true);
                //animator.SetTrigger("Hit");
                break;
            case State.Die:
                animator.SetTrigger("Die");
                break;
        }
        return state;
    }

    //////////�׽�Ʈ�ڵ�
    ///
    [ContextMenu("�׽�Ʈ ����")]
    public void AttackTest()
    {
        OnAttack(null);
    }


    private void OnCollisionStay2D(Collision2D collision) //���� �÷��̾�� �ε������� ���̻罽 ���� �����ؼ� �Ʒ����̸� ������
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            if (GameManager2.instance != null)
            {
                //GameManager2.instance.Hp.GetComponent<Slider>().value -= 1f; //�� ������ ���ݷ� ��ŭ ��� �̹����� empitedHp�� ����
            }
            StartCoroutine(waitForseconds());

        }
    }

    IEnumerator waitForseconds()
    {

        yield return new WaitForSecondsRealtime(1f); //�°� �� ���� ���������� 1�ʵ����� ����
    }

    public void OnHeal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        del_OnDamage(null);
    }

}

