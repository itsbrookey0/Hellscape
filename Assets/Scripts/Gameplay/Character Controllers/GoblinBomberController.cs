using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sierra;

public class GoblinBomberController : EnemyController
{
    // NORMAL STUN: on any melee, causes a held bomb to be dropped
    // SUPERSTUN: on critical ranged, causes a held bomb to be dropped

    #region Public Vars
    public float Range;    
    public float BombUpVel;    
    public float BombHorVel;    

    public Behaviour CurrentBehaviour;
    public enum Behaviour
    {
        Idle, Throwing
    }

    public AttackData AttackData;
    public GameObject BombPrefab;

    public GameObject goblinHide;
    public Vector2 AverageItemVariance = new Vector2(3, 3);
    #endregion
    #region Private Vars
    private IEnumerator currentRoutine;
    private bool holdingBomb = false;
    #endregion

    #region Unity Messages
    protected override void Awake()
    {
        base.Awake();
    }
    protected void OnDrawGizmosSelected()
    {
        DrawCircle(Range, Color.cyan);
    }
    #endregion

    #region Public Methods
    public void SetBehaviour(Behaviour newBehaviour)
    {
        if (newBehaviour != CurrentBehaviour)
            CurrentBehaviour = newBehaviour;
    }
    public override void Die()
    {
        int lootNum = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < lootNum; i++)
        {
            var hideVarianceY = AverageItemVariance.y * Utility.GetRandomFloat();
            var hideVarianceX = AverageItemVariance.x * Utility.GetRandomFloat();

            var hideSpawnPos = new Vector3(
                transform.position.x + hideVarianceX,
                transform.position.y + hideVarianceY,
                transform.position.z);

            Instantiate(goblinHide, hideSpawnPos, transform.rotation);
        }

        Debug.Log(name + "Is Dead.");
        SetState(State.Dead);
        if (currentRoutine != null) StopCoroutine(currentRoutine);

        // Despawn
        if (holdingBomb)
        {
            var bomb = Instantiate(BombPrefab, transform.position = Vector3.up, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    #endregion
    #region Protected/Private Methods
    protected override void Act()
    {
        // ATTACK:
        // launch bombs at player on regular timer @ a fixed angle
        FacePlayer();

        if (CurrentBehaviour == Behaviour.Throwing && CurrentState != State.Action)
        {
            if (currentRoutine != null) StopCoroutine(currentRoutine);
            currentRoutine = ThrowBombRoutine();
            StartCoroutine(currentRoutine);
        }
        else
        {
            hp.Hurtbox.SetState(Sierra.Combat2D.Hurtbox.State.Vulnerable);
        }
    }
    protected override void DecideAction()
    {
        // If player in range, face player & start attacking
        // otherwise: idle

        if (DistToPlayer < Range)
        {
            SetBehaviour(Behaviour.Throwing);
        }
        else
        {
            SetBehaviour(Behaviour.Idle);
        }
    }
    protected void DrawCircle(float radius, Color colour)
    {
        Handles.color = colour;
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }
    private IEnumerator ThrowBombRoutine()
    {
        // Startup
        SetState(State.Action);
        holdingBomb = true;
        hp.Hurtbox.SetState(Sierra.Combat2D.Hurtbox.State.Critical);
        GenericEvents.OnAttack.Invoke();

        var timer = 0;        
        while (timer < AttackData.Startup) { yield return new WaitForFixedUpdate(); timer++; }

        // Active: Throw bomb
        var bomb = Instantiate(BombPrefab, transform.position + (PlayerToLeft ? -new Vector3(1, 0) : new Vector3(1, 0)), Quaternion.identity);
        bomb.GetComponent<CharacterMotionController>().ContMotionVector.y = BombUpVel;
        bomb.GetComponent<CharacterMotionController>().XSpeed = PlayerToLeft ? -BombHorVel : BombHorVel;
        bomb.GetComponent<BouncingExplosiveProjectileController>().AttackData = AttackData;

        timer = 0;
        while (timer < AttackData.Active) { yield return new WaitForFixedUpdate(); timer++; }

        // Recovery
        holdingBomb = false;
        hp.Hurtbox.SetState(Sierra.Combat2D.Hurtbox.State.Vulnerable);

        timer = 0;
        while (timer < AttackData.Recovery) { yield return new WaitForFixedUpdate(); timer++; }

        // End of attack
        SetState(State.Ready);
    }
    #endregion
}
