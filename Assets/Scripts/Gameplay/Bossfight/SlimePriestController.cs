using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;
using System.Collections;

public class SlimePriestController : EnemyController, IBossAttackTriggerResponder
{
    #region Public Vars
    [Serializable]
    public class LocalPreistEvents
    {
        public UnityEvent OnStageSwitchAlpha;
        public UnityEvent OnStageSwitchBeta;
    }
    public LocalPreistEvents PriestEvents;

    public Behaviour CurrentBehaviour;
    public enum Behaviour
    {
        Standing, Aiming
    }
    public BossAttackTrigger AttackTrigger;
    public int ChangeStageDelayFrames = 90;
    #endregion
    #region Protected Vars
    #endregion

    #region Unity Messages
    protected override void Awake()
    {
        base.Awake();        
    }
    protected void Start()
    {
        FindObjectOfType<FightManager>().GoToNextStage();
        AttackTrigger?.SetResponder(this);
    }
    protected override void FixedUpdate()
    {
        if (CurrentState == State.Ready)
        {
            DecideAction();
            Act();
        }
    }
    protected void OnDrawGizmosSelected()
    {
    }
    #endregion

    #region Public Methods
    public void StartAttack()
    {
        am.Attack();
        GenericEvents.OnAttack.Invoke();
    }
    public void ChangeStage()
    {
        StartCoroutine(ChangeStageRoutine());
    }
    public void SetBehaviour(Behaviour newBehaviour)
    {
        if (CurrentBehaviour != newBehaviour) CurrentBehaviour = newBehaviour;
    }
    public override void Die()
    {
        Debug.Log(name + "Is Dead.");
        SetState(State.Dead);
        am?.StopAttack();

        // Despawn
        if (transform.parent != null) Destroy(transform.parent.gameObject, 3f);
        else Destroy(gameObject, 3f);
    }
    #endregion
    #region Protected Methods
    protected override void Act()
    {
    }
    protected override void DecideAction()
    {
    }
    protected void DrawCircle(float radius, Color colour)
    {
        Handles.color = colour;
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }
    
    protected IEnumerator ChangeStageRoutine()
    {
        if (hp.Hp == 0) yield break;

        var timer = 0;
        while (timer < ChangeStageDelayFrames)
        {
            timer++;
            yield return new WaitForFixedUpdate();
        }

        PriestEvents.OnStageSwitchAlpha.Invoke();

        timer = 0;
        while (timer < 20)
        {
            timer++;
            yield return new WaitForFixedUpdate();
        }

        PriestEvents.OnStageSwitchBeta.Invoke();
    }
    #endregion
}