using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(MouseRotation))]
public class Player : SingleTone<Player>
{
    public Unit myUnit;
    public int score;
    public int maxHealth
    {
        get
        {
            if (myUnit == null) return 5;
            else { return myUnit.MaxHealth; }
        }
    }

    public int curHealth
    {
        get
        {
            if (myUnit == null) return 5;
            else { return myUnit.CurrentHealth; }
        }
    }

    Vector3 movement;
    float x, y;
    MouseRotation mouse;
    public float intensity; //이거를 업데이트 주기로 ~초마다 0.0x값 (0.2~1f) 늘려줘야된다.
                            //횟불아이템은 이 값을 줄여줌




    protected override void Awake()
    {
        base.Awake();
        this.gameObject.tag = "Player";
        myUnit = GetComponent<Unit>();
        mouse = GetComponent<MouseRotation>();
        CircleCollider2D attackBoundary =
        this.gameObject.AddComponent<CircleCollider2D>();
        attackBoundary.isTrigger = true;
        attackBoundary.radius = 2;
        intensity = 0.4f;

        myUnit.del_OnDamage += OnDamaged;
        myUnit.del_getScore += GetScore;
        myUnit.del_getEXP += GetEXP;
        OnDamaged(null);
        if (GameManager2.instance != null)
        {
            GameManager2.instance.OnInit(this.maxHealth, this.curHealth);
        }
        StartCoroutine(StartCo());
    }
    IEnumerator StartCo()
    {
        WaitForSeconds sec = new WaitForSeconds(0.05f);
        while (true)
        {
            if (myUnit.lastAttack >= myUnit.AttackSpeed)
            {
                Collider2D[] colliders =
                Physics2D.OverlapCircleAll(this.transform.position, 2);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].TryGetComponent<Unit>(out Unit unit))
                    {
                        if (unit != myUnit)
                        {
                            TryAttack(unit);
                            break;
                        }
                    }
                }
                //CircleCollider2D attackBoundary =
                //this.gameObject.AddComponent<CircleCollider2D>();
                //attackBoundary.isTrigger = true;
                //attackBoundary.radius = 2;
            }
            yield return sec;
        }
    }
    protected void Start()
    {
        if (GameManager2.instance != null)
        {
            GameManager2.instance.OnInit(this.maxHealth, this.curHealth);
            GetScore(score);
        }
    }
    private void OnDisable()
    {
        if (SoundManager.instance != null)
        { SoundManager.instance.PlayBGM(SoundManager.BGM.GameOver); }
    }
    float decreaseHpTimer = 0f;
    bool isDecreas;
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        if (Input.GetMouseButtonDown(0))
        {
            if (myUnit.jop == Unit.Jop.Magician_1 || myUnit.jop == Unit.Jop.Magician_2)
            {
                myUnit.GetComponent<Magician>().OnAttack(mouse.mouse);
            }
        }
        GameManager2.instance.currentHealth = this.curHealth;
        if (intensity < 0f) intensity = 0f;
        if (intensity < 1f) intensity += 0.007f * Time.deltaTime;
        else { isDecreas = true; intensity = 0.99f; }
        if (isDecreas)
        {
            if (decreaseHpTimer > 0f) decreaseHpTimer -= Time.deltaTime;
            else
            {
                decreaseHpTimer = 5f;
                myUnit.OnDamaged(null);
            }
        }
        ChangeBGM();
    }


    public void ChangeBGM()
    {
        if (intensity > 0.9f || curHealth < 2)
        {
            if (SoundManager.instance.audios.clip !=
                SoundManager.instance.bgms[(int)SoundManager.BGM.Emergency])
            {
                SoundManager.instance.PlayBGM(SoundManager.BGM.Emergency);
            }
        }
        else
        {
            if (SoundManager.instance.audios.clip !=
                SoundManager.instance.bgms[(int)SoundManager.BGM.Ingame])
            {
                SoundManager.instance.PlayBGM(SoundManager.BGM.Ingame);
            }
        }
    }

    bool justOne;
    private void FixedUpdate()
    {
        movement.Set(x, y, 0);
        movement = movement.normalized * myUnit.MoveSpeed * Time.deltaTime;
        OnMove(movement);
    }
    public void OnMove(Vector3 Nextpositon)
    {
        if (Nextpositon != Vector3.zero)
        { SoundManager.instance.PlayFoot(true); }
        else
        { SoundManager.instance.PlayFoot(false); }
        myUnit.OnMove(Nextpositon * myUnit.MoveSpeed, true); //moveSpeed가 0이여서 그런거같은데
        //transform.localPosition = new Vector3(this.transform.position.x + Nextpositon.x, this.transform.position.y + Nextpositon.y, 0);
        if (myUnit.state == Unit.State.Die)
        { SoundManager.instance.PlayFoot(false); }
    }

    public void OnDamaged(Unit Attacker)
    {
        if (GameManager2.instance != null)
        {
            GameManager2.instance.OnInit(this.maxHealth, this.curHealth);
            if (myUnit.CurrentHealth <= 0)
            {
                GameManager2.instance.EndGame(score);
            }
        }
    }

    public void GetScore(int howMany)
    {
        score += howMany;
        GameManager2.instance.UpdateScore(score);
        return;
    }

    public void GetEXP(int Current, int Max)
    {
        Debug.Log($"{Current}//{Max}");
        if (GameManager2.instance != null)
        { GameManager2.instance.SetEXP(Current, Max); }
        if (Current >= Max)
        {
            myUnit.LevelUP();
            GameManager2.instance.SetEXP(0, Max);

            //레벨업 이펙트 실행

        }
    }

    public void TryAttack(Unit Target)
    {
        if (myUnit.lastAttack >= myUnit.AttackSpeed)
        {
            ///공격대상을 바라보게 하기
            myUnit.SetState(Unit.State.Attack);
            myUnit.OnAttack(Target);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 눈지형 or 흙 지형에 있는동안
        string tileTag = collision.gameObject.tag;

        switch (tileTag)
        {
            case "snow":
                if (!justOne)
                {
                    myUnit.SetMoveSpeed_rate(2f);
                    justOne = true;
                }

                break;

            case "sand":
                if (!justOne)
                {
                    myUnit.SetMoveSpeed_rate(0.5f);
                    justOne = true;
                }

                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //나가면 원래 이동속도로 돌아오기
        //CSV를 통해 원래 이동속도로 돌아오기
        string tileTag = collision.gameObject.tag;

        switch (tileTag)
        {
            case "snow":
                myUnit.SetMoveSpeed_rate(0.5f, true);
                break;

            case "sand":
                myUnit.SetMoveSpeed_rate(0.5f, true);
                break;
        }
        justOne = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Unit>(out Unit target))
        {
            if (myUnit.jop != Unit.Jop.Magician_1 || myUnit.jop != Unit.Jop.Magician_2)
            {
                //TryAttack(target);
            }
        }
    }
}
