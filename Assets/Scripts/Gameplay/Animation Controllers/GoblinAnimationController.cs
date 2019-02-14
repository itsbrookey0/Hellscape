using UnityEngine;
using System.Collections;

public class GoblinAnimationController : EnemyAnimationController
{
    public override void EffectOnAttack()
    {
        sk_an.AnimationState.SetAnimation(0, "Throw", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }
    public override void EffectOnHit()
    {
        sk_an.AnimationState.SetAnimation(0, "Stun", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }
    public override void EffectOnCrit()
    {
        sk_an.AnimationState.SetAnimation(0, "SuperStun", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }
}
