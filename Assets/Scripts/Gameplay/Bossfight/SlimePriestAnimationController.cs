using UnityEngine;
using System.Collections;

public class SlimePriestAnimationController : EnemyAnimationController
{
    public override void EffectOnHit()
    {
        if (renderer == null) return;
        if (sk_an == null) return;

        sk_an.AnimationState.SetAnimation(0, "Hit", false);
        sk_an.AnimationState.AddAnimation(0, "Idle", true, 0);
    }
}
