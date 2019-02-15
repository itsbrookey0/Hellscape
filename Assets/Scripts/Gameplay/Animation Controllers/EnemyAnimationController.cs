using UnityEngine;
using System.Collections;

public class EnemyAnimationController : AnimationController
{
    public GameObject[] BloodSplatters;

    protected new Renderer renderer;
    protected IEnumerator currentFlashRoutine;

    protected override void Awake()
    {
        base.Awake();
        renderer = GetComponent<Renderer>();
    }    

    public virtual void EffectOnMoveStart()
    {
        if (sk_an == null) return;

        //sk_an.AnimationState.SetAnimation(0, "Attack", true);
    }
    public virtual void EffectOnMoveEnd()
    {
        sk_an.AnimationState.SetAnimation(0, "Idle", true); 
    }
    public virtual void EffectOnDeath()
    {
        if (sk_an == null) return;

        sk_an.AnimationState.SetAnimation(0, "Death", false);
    }
    public virtual void EffectOnHit()
    {
        if (renderer == null) return;
        if (sk_an == null) return;

        sk_an.AnimationState.SetAnimation(0, "HitStun", false);
        DoBloodFx(0);
    }
    public virtual void EffectOnCrit()
    {
        sk_an.AnimationState.SetAnimation(0, "SuperStunHit", false);
        sk_an.AnimationState.AddAnimation(0, "Dizzy", true, 0);
    }
    public virtual void EffectOnAttack()
    {
        sk_an.AnimationState.SetAnimation(0, "Attack", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }    

    public virtual void DoBloodFx(int index)
    {
        if (index >= BloodSplatters.Length) return;
        
        var particle = Instantiate(BloodSplatters[index], transform);

        Destroy(particle, 5f);
    }
}
