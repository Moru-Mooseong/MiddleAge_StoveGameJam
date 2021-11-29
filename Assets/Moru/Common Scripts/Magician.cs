using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : Unit
{

    public Transform SpawnPivot;
    public GameObject Projectile;
    [SerializeField] ParticleSystem AttackParticle;

    Vector3 targetingPos;

    public void PlayAttack_Particle()
    {
        if (AttackParticle != null) AttackParticle.Play(true);
    }

    public void StartProjectile()
    {
        GameObject protile = Instantiate(Projectile, SpawnPivot.position, Quaternion.identity);
        if (TryGetComponent<Unit_AI>(out Unit_AI AI))
        {
            if (AI.targetUnit != null)
            { protile.AddComponent<Projectile>().Initialized(this, AI.targetUnit.transform.position); }
            else
            {
                Destroy(protile.gameObject);
            }
        }
        else if (TryGetComponent<MouseRotation>(out MouseRotation player))
        {
            protile.AddComponent<Projectile>().Initialized(this, player.GetComponent<MouseRotation>().mouse);
        }
    }

    public void OnAttack(Vector3 target)
    {
        if (lastAttack >= AttackSpeed)
        {
            audios.clip = clip;
            audios.Play();
            SetState(State.Attack);
            targetingPos = target;
            lastAttack = 0;
        }
    }

    void Update()
    {
        currentCoolTime += Time.deltaTime;
    }

    public override void OnAttack(Unit target)
    {
        if (lastAttack >= AttackSpeed)
        {
            SetState(State.Attack);
            lastAttack = 0;
        }
    }
}

public class Projectile : MonoBehaviour
{
    Unit Attaker;
    Vector3 Target;
    Rigidbody2D rigid;
    int count;
    float t;
    int i;
    public AudioSource Audiosource;
    public void Initialized(Unit Attaker, Vector3 targetPos, int i = 0)
    {
        if (rigid == null)
        { rigid = GetComponent<Rigidbody2D>(); }
        this.Attaker = Attaker;
        Vector3 targets = targetPos - transform.position;

        Target = targets.normalized;
        count = 2;
        t = 0;
        this.i = i;
        SetVector();
    }

    Vector3 nextPos;
    void SetVector()
    {

        Vector3 _nextPos = (nextPos - transform.position).normalized;
        Quaternion lookAt = Quaternion.LookRotation(Vector3.forward, -_nextPos);
        this.transform.rotation = lookAt;
    }

    void Update()
    {
        AddForce();
    }

    void AddForce()
    {
        transform.position += Target * Time.deltaTime * 5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Unit>(out Unit target))
        {
            if (target == Attaker) return;
            Audiosource = GetComponent<AudioSource>();

            target.OnDamaged(Attaker);
            if (!Audiosource.isPlaying)
            {
                Audiosource.Play();
            }
            count--;
            if (count == 0)
            { Destroy(this.gameObject); }
        }
    }


}

