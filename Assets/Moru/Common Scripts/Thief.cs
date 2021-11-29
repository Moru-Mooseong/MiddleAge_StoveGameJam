using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Unit
{
    public float MaxSpeed;
    public float Move_increaseLevel;

    public float InvisibleTime;

    public bool isVisible;

    [SerializeField] SpriteRenderer[] Sprites;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Spawn(Vector3 SpawnPos, Quaternion rotation, int level = 0)
    {
        base.Spawn(SpawnPos, rotation, level);
        for (int i = 0; i < Sprites.Length; i++)
        {
            if(Sprites[i] != null)
            Sprites[i].color = new Color(1,1,1,1);
        }
    }


    public override void OnAttack(Unit target)
    {
        if (jop == Jop.thief_1)
        {
            base.OnAttack(target);
            MoveSpeed = defautl_MoveSpeed;
            StartCoroutine(Co_Visible());
        }
        else
        {
            if (lastAttack < AttackSpeed) return;
            MoveSpeed = defautl_MoveSpeed * 5;
            Vector3 next = target.transform.position - transform.position;
            next = next.normalized;
            OnMove(next);
            if(Vector3.Distance(target.transform.position, this.transform.position) < 3f)
            {
                Attack2(target);
            }
        }
    }

    public void Attack2(Unit target)
    {
        base.OnAttack(target);
        MoveSpeed = defautl_MoveSpeed;
        lastAttack = 0;
    }

    [SerializeField] ParticleSystem AttackParticle_R;
    [SerializeField] ParticleSystem AttackParticle_L;

    public void PlayAttack_Particle()
    {
        if (AttackParticle_R != null) AttackParticle_R.Play(true);
    }

    public void Play_LeftParticle()
    {
        AttackParticle_R.Play(true);
    }
    public void Play_RightParticle()
    {
        AttackParticle_L.Play(true);
    }

    public override void OnMove(Vector3 Nextpositon, bool isWorld = false)
    {
        base.OnMove(Nextpositon, isWorld);
        MoveSpeed += Move_increaseLevel * Time.deltaTime;
        if(MoveSpeed > MaxSpeed)
        {
            MoveSpeed = MaxSpeed;
        }
        
    }
    public override bool isCanFind(Unit target)
    {
        if (isVisible)
        { return base.isCanFind(target); }
        else
        { return false; }
    }

    bool isActive = true;
    private IEnumerator Co_Visible()
    {
        float t = 0;
        isVisible = false;
        isActive = true;
        Color decurveColor = new Color(0, 0, 0, Time.deltaTime * 2);
        while (t < InvisibleTime)
        {
            if (isActive)
            {
                for (int i = 0; i < Sprites.Length; i++)
                {
                    Sprites[i].color -= decurveColor;
                }
            }
            else
            {
                t = InvisibleTime;
            }
            yield return Time.deltaTime;
        }
        while (Sprites[0].color.a > 1)
        {
            for (int i = 0; i < Sprites.Length; i++)
            {
                Sprites[i].color -= decurveColor;
            }
            yield return Time.deltaTime;
        }
        isVisible = true;
    }

}
