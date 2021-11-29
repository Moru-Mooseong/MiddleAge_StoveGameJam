using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Unit
{
    [SerializeField] ParticleSystem AttackParticle;
    public void PlayAttack_Particle()
    {
        if(AttackParticle != null) AttackParticle.Play(true);
    }

    public override void OnAttack(Unit target)
    {
        base.OnAttack(target);
    }
}
