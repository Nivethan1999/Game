using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    private CanvasGroup healthGroup;

    private Transform target;

    private IState state;

    public float enemyAttackRange { get; set; }

    public float AttackTime { get; set; }

    public Transform Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }

    protected void Awake()
    {
        enemyAttackRange = 1;
        changeState(new IdleState());
    }



    protected override void Update()
    {
        if (IsAlive)
        {
            if (!isAttacking)
            {
                AttackTime += Time.deltaTime;
            }

            state.Update();

           
        }
        base.Update();



    }

    public override Transform Select()
    {
        healthGroup.alpha = 1;

        return base.Select();
    }

    public override void DeSelect()
    {
        healthGroup.alpha = 0;

        base.DeSelect();
    }

 

    public void changeState(IState newstate)
    {
        if (state != null)
        {
            state.Exit();
        }

        state = newstate;

        state.Enter(this);
    }
}
